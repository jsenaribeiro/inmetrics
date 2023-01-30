using Desafio.Domain;

namespace Desafio.Infrastructure;

public interface IRepositoryFactory<E, I> : IRepository<E, I>
   where E : class, IEntity<I>
   where I : IEquatable<I>
{ }
