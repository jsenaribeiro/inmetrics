using System.Linq.Expressions;

namespace Desafio.Domain;

public sealed class InvalidException : DomainException
{
    public InvalidException() : base(Resource.INVALIDS) { }

    public InvalidException(IEnumerable<Invalid> invalids) : this()
    {
        this.Invalids = invalids.ToList();
    }

    public List<Invalid> Invalids { get; set; } = new List<Invalid>();
}

