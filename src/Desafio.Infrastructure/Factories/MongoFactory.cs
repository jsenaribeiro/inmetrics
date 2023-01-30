using Desafio.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Core.Configuration;
using Mongo2Go;
using Microsoft.Extensions.Configuration;

namespace Desafio.Infrastructure;

public class MongoFactory<E, I> : MongoContext, IRepositoryFactory<E, I>
   where E : class, IEntity<I> where I : IEquatable<I>
{
    protected readonly IConfiguration configuration;
    protected readonly IMongoQueryable<E> query;
    protected readonly IMongoCollection<E> collection;
    protected readonly IUnitOfWork unitOfWork;
    protected readonly IMongoDatabase database;
    protected readonly ILoginService loginService;
    private bool ignoreSoftDeleted = false;

    public MongoFactory(IServiceProvider provider)
    {
        this.configuration = (provider.GetService(typeof(IConfiguration)) as IConfiguration)!;
        this.unitOfWork = (provider.GetService(typeof(IUnitOfWork)) as IUnitOfWork)!;
        this.database = new MongoClient(ConnectionString).GetDatabase("desafio");
        this.loginService = (provider.GetService(typeof(ILoginService)) as ILoginService)!;
        this.collection = database.GetCollection<E>(typeof(E).Name);
        this.query = collection.AsQueryable().Where(x => ignoreSoftDeleted || x.Audit.CRUD != CRUD.Delete);
    }

    public async Task<E?> LoadAsync(Expression<Func<E, bool>> expression)
    {
        var result = await query.FirstOrDefaultAsync(expression);
        return result is null ? default : result;
    }

    public Task<List<E>> ListAsync(Expression<Func<E, bool>>? expression = null) =>
       query.Where(expression ?? (x => true)).ToListAsync();

    public Task<bool> CheckAsync(Expression<Func<E, bool>> expression) => query.AnyAsync(expression);

    public Task<int> CountAsync(Expression<Func<E, bool>>? expression = null) =>
       expression is null ? query.CountAsync() : query.CountAsync(expression);

    private FilterDefinition<E> GetFilterById(I identity) => Builders<E>.Filter.Eq(x => x.Id, identity);

    public async Task<I> SaveAsync(E entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        var emptyId = entity.Id.Equals(default);
        var unfound = !(await CheckAsync(x => x.Id.Equals(entity.Id)));
        var created = emptyId || unfound;

        var crud = created ? CRUD.Create : CRUD.Update;

        if (emptyId && entity.Id is long)
        {
            var latestSequenceId = await nextSequenceAsync();

            entity.GetType()!.GetProperty("Id")!
               .SetValue(entity, latestSequenceId);
        }

        var login = await loginService.LogonAsync();

        entity.SetAudit(login.User, crud);

        var filter = GetFilterById(entity.Id);

        if (created) await this.collection.InsertOneAsync(entity);

        else await this.collection.ReplaceOneAsync(filter, entity);

        await auditAsync(crud);

        return entity.Id;
    }

    public async Task<bool> DropAsync(Expression<Func<E, bool>> expression)
    {
        var entities = await ListAsync(expression);
        if (entities is null || entities.Count == 0) return false;

        var login = await loginService.LogonAsync();

        foreach (var entity in entities)
        {
            var filter = GetFilterById(entity.Id);

            if (Settings.IsDbSoftDeleted)
            {
                entity.SetAudit(login.User, CRUD.Delete);
                await collection.ReplaceOneAsync(filter, entity);
            }

            else await collection.DeleteOneAsync(filter);
        }

        return true;
    }

    private async Task auditAsync(CRUD crud)
    {
        var login = await loginService.LogonAsync();

        await database
           .GetCollection<Audit>(nameof(Audit))
           .InsertOneAsync(new Audit(login.User, crud));
    }

    private async Task<long> nextSequenceAsync()
    {
        var reference = typeof(E).Name;

        var sequences = database
           .GetCollection<Sequence>(nameof(Sequence));

        var resulted = await sequences.AsQueryable()
           .FirstOrDefaultAsync(x => x.Label == reference);

        var identity = Builders<Sequence>.Filter.Eq(x => x.Label, reference);

        var sequence = (resulted?.Value ?? 0) + 1;

        if (resulted is null) await sequences.InsertOneAsync(new Sequence(reference, 1));
        else await sequences.ReplaceOneAsync(identity, resulted.With(sequence));

        return sequence;
    }

    public IReadRepository<E, I> Options(bool showSoftDeleteds)
    {
        this.ignoreSoftDeleted = showSoftDeleteds;
        return this;
    }


    private string ConnectionString
    {
        get
        {
            var connectionString = Settings.Database.ConnectionString;

            if (Settings.Database.IsInMemoryDatabase)
            {
                var mongoDbRunner = MongoDbRunner.Start();
                connectionString = mongoDbRunner.ConnectionString;
            }

            return connectionString ?? string.Empty;
        }
    }
}