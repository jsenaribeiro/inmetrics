namespace Desafio.Domain;

public class RequiredException : DomainException
{
    public RequiredException(string field) : base(string.Format(Resource.REQUIRED, field)) { }
}
