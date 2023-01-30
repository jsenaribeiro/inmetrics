namespace Desafio.Domain;

public class NotFoundException : DomainException
{
    public NotFoundException(string field) : base(string.Format(Resource.NOT_FOUND, field)) { }
}
