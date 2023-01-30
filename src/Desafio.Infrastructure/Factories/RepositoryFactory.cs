using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Desafio.Domain;

namespace Desafio.Infrastructure;

public abstract class RepositoryFactory<E, I> : IRepository<E, I>
   where E : class, IEntity<I> where I : IEquatable<I>
{
    private readonly IRepository<E, I> repository;
    protected readonly IUnitOfWork unitOfWork;

    protected RepositoryFactory(IServiceProvider provider)
    {
        unitOfWork = new UnitOfWork(provider);

        repository = Settings.Database switch
        {
            MongoConnection _ => new MongoFactory<E, I>(provider),
            _ => new EfCoreFactory<E, I>(provider),
        };
    }

    public Task<List<E>> ListAsync(Expression<Func<E, bool>>? expression = null) => repository.ListAsync(expression);

    public Task<E?> LoadAsync(Expression<Func<E, bool>> expression) => repository.LoadAsync(expression);

    public Task<I> SaveAsync(E entity) => repository.SaveAsync(entity);

    public Task<bool> CheckAsync(Expression<Func<E, bool>> expression) => repository.CheckAsync(expression);

    public Task<int> CountAsync(Expression<Func<E, bool>>? expression = null) => repository.CountAsync(expression);

    public Task<bool> DropAsync(Expression<Func<E, bool>> expression) => repository.DropAsync(expression);

    public IReadRepository<E, I> Options(bool showSoftDeleteds) => repository.Options(showSoftDeleteds);
}