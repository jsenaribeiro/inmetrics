namespace Desafio.Domain;

public interface IRequest : IHandled
{
    Task ValidateAsync(Validation validation);
}

public interface IEvent : IRequest, IResponse { }

public interface IResponse : IHandled { }
