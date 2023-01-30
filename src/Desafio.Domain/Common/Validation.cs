using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Domain;

public sealed class Validation
{
    public Validation(IUnitOfWork unitOfWork) => uow = unitOfWork;

    private readonly IUnitOfWork uow;

    private readonly List<Invalid> invalids = new List<Invalid>();

    public void Add(string error, string field) => invalids.Add(new Invalid(error, field));

    public void Add<T>(string field) where T : DomainException =>
       Add((Activator.CreateInstance(typeof(T), field!)! as dynamic).Message, field);

    public void AddRequired(object? value, string field)
    {
        var emptyGuid = value is Guid guid && guid == Guid.Empty;

        var emptyString = string.IsNullOrWhiteSpace(value?.ToString());

        var emptyNumberic = value is null
           || value is long lng && lng == 0
           || value is int itg && itg == 0
           || value is byte bte && bte == 0
           || value is double dbl && dbl == 0
           || value is short sht && sht == 0
           || value is float flt && flt == 0
           || value is sbyte sbt && sbt == 0
           || value is ushort ust && ust == 0
           || value is uint uit && uit == 0
           || value is ulong ulg && ulg == 0
           || value is decimal dec && dec == 0;

        var emptyEnum = (value?.GetType().IsEnum ?? false) && ((int)value) == 0;

        if (emptyString || emptyNumberic || emptyGuid || emptyEnum) Add<RequiredException>(field);
    }

    public void AddNotFound(string field) => Add<NotFoundException>(field);

    public bool Contains(string message, params string[] fields) =>
       invalids.Any(x => x.Error.ToLower().Contains(string.Format(message, fields)));

    public async Task<bool> Async<T, U>(
       Func<IUnitOfWork, IRepository<T, U>> getRepository,
       Expression<Func<T, bool>> predicate)
       where T : IEntity<U> where U : IEquatable<U>
    {
        return await getRepository(uow).CheckAsync(predicate);
    }

    public InvalidException ToException() => new InvalidException(this.invalids);

    public bool ThrowsExceptionIfInvalidated() =>
       invalids is not null && invalids.Count > 0
          ? throw this.ToException() : false;
}
