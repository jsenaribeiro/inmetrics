using Desafio.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharpCompress.Common;
using System.Collections;

namespace Desafio.Infrastructure;

public class EfCoreFactory<E, I> : IRepositoryFactory<E, I>
   where E : class, IEntity<I>
   where I : IEquatable<I>
{
    private bool ignoreSoftDelete = false;
    private readonly DbSet<E> current;
    private readonly EfCoreContext context;
    protected readonly IUnitOfWork unitOfWork;
    protected readonly ILoginService loginService;

    public EfCoreFactory(IServiceProvider provider)
    {
        loginService = provider.GetService<ILoginService>()!;
        context = provider.GetService<EfCoreContext>()!;
        unitOfWork = provider.GetService<IUnitOfWork>()!;

        var dbSetTypeFullName = typeof(DbSet<E>).FullName;
        var dbSetProps = context.GetType().GetProperties() ?? new PropertyInfo[] { };
        var getProps = new Func<PropertyInfo, bool>(x => x.PropertyType.FullName == dbSetTypeFullName);

        current = (dbSetProps.FirstOrDefault(getProps)!.GetValue(context) as DbSet<E>)!;
    }

    private IQueryable<E> query => current.Where(x => x.Audit.CRUD != CRUD.Delete || ignoreSoftDelete);

    public Task<E?> LoadAsync(Expression<Func<E, bool>> expression) =>
       query.FirstOrDefaultAsync(expression);

    public Task<List<E>> ListAsync(Expression<Func<E, bool>>? expression = null) =>
       query.Where(expression ?? (x => true)).ToListAsync() ?? Task.FromResult(new List<E>());

    public Task<bool> CheckAsync(Expression<Func<E, bool>> expression) => query.AnyAsync(expression);

    public Task<int> CountAsync(Expression<Func<E, bool>>? expression = null) =>
       expression is null ? query.CountAsync() : query.CountAsync(expression);

    public async Task<I> SaveAsync(E entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        var empty = entity.Id.Equals(default);
        var found = !empty && await CheckAsync(x => x.Id.Equals(entity.Id));
        var crud = found ? CRUD.Update : CRUD.Create;
        var login = await loginService.LogonAsync();

        entity.SetAudit(login.User, crud);

        var entry = found ? current.Update(entity) : current.Add(entity);

        if (entity is IAggregate aggregate) SaveAggregate(aggregate);

        await context.SaveChangesAsync();

        return entry.Entity.Id;
    }

    public async Task<bool> DropAsync(Expression<Func<E, bool>> expression)
    {
        var entities = await ListAsync(expression);
        if (entities is null || entities.Count == 0) return false;

        var login = await loginService.LogonAsync();

        foreach (var entity in entities)
        {
            var emptyIdentity = entity.Id.Equals(default);
            if (emptyIdentity) continue;

            if (Settings.IsDbSoftDeleted)
            {
                entity.SetAudit(login.User, CRUD.Delete);
                current.Update(entity);
            }

            else current.Entry(entity).State = EntityState.Deleted;
        }

        await context.SaveChangesAsync();

        return true;
    }

    public IReadRepository<E, I> Options(bool ignoreSoftDelete)
    {
        this.ignoreSoftDelete = ignoreSoftDelete;
        return this;
    }

    private void SaveAggregate(IAggregate aggregate)
    {
        if (aggregate.GetEnumerator() is not IEnumerator<object> iterator) return;

        while (iterator.MoveNext())
        {
            var entity = iterator.Current;

            if (entity.GetType().GetProperty("Id") is not PropertyInfo propertyId) continue;
            if (propertyId.PropertyType is not Type propertyType) continue;

            var isValueType = propertyType?.IsValueType ?? false;
            var defaultValue = isValueType ? Activator.CreateInstance(propertyType!) : null;
            var isEmptyId = propertyId!.GetValue(entity) == defaultValue;

            if (isEmptyId) context.Add(iterator.Current);
            else
            {
                var entityTypeName = entity.GetType().Name;

                var dbSetProps = context.GetType().GetProperties();

                var dbSet = dbSetProps.FirstOrDefault(p => p
                   .PropertyType.GenericTypeArguments
                   .Any(g => g.Name == entityTypeName))?
                   .GetValue(context) as dynamic;

                if (dbSet is null) continue;

                var exists = Enumerable.Contains(dbSet, entity);

                if (exists) context.Update(entity);
                else context.Add(entity);
            }
        }
    }
}