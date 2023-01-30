namespace Desafio.Domain;

public class UnauthorizedException : DomainException
{
    public UnauthorizedException() : base(string.Format(Resource.UNAUTHORIZED)) { }
}
