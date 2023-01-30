using Desafio.Domain;
using System.Linq.Expressions;

namespace Desafio.Domain;

public interface IRepository<E, I> : IReadRepository<E, I>, IWriteRepository<E, I>
   where E : IEntity<I> where I : IEquatable<I>
{
    async Task<bool> SaveAsync(I identity, Action<E> action)
    {
        var entity = await LoadAsync(identity);

        if (entity is null) return false;
        else action(entity);

        await SaveAsync(entity);

        return true;
    }
}

public interface IReadRepository<E, I> where E : IEntity<I> where I : IEquatable<I>
{
    Task<E?> LoadAsync(I identity) => LoadAsync(x => x.Id.Equals(identity));

    Task<E?> LoadAsync(Expression<Func<E, bool>> expression);

    Task<List<E>> ListAsync(Expression<Func<E, bool>>? expression = null);

    Task<IAsyncEnumerable<E>> LoopAsync(Expression<Func<E, bool>> expression) => throw new NotImplementedException();

    Task<bool> CheckAsync(Expression<Func<E, bool>> expression);

    Task<bool> CheckAsync(I identity) => CheckAsync(x => x.Id.Equals(identity));

    Task<int> CountAsync(Expression<Func<E, bool>>? expression = null);

    IReadRepository<E, I> Options(bool ignoreSoftDelete);
}

public interface IWriteRepository<E, I> where E : IEntity<I> where I : IEquatable<I>
{
    Task<I> SaveAsync(E entity);

    Task<bool> DropAsync(I identity) => DropAsync(x => x.Id.Equals(identity));

    Task<bool> DropAsync(Expression<Func<E, bool>> expression);
}