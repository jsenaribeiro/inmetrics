namespace Desafio.Domain;

public class ConflictException : DomainException
{
    public ConflictException(string field) : base(string.Format(Resource.CONFLICT, field)) { }
}
