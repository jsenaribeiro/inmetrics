namespace Desafio.Domain;

public class ForbidenException : DomainException
{
    public ForbidenException(string field) : base(string.Format(Resource.FORBIDEN, field)) { }
}
