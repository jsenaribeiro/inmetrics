namespace Desafio.Domain;

public record Handled() : IResponse;

public interface IHandle { }

public interface IHandled : IHandle { }

public interface IHandler : IHandle
{
    Task<IResponse> HandleAsync(IRequest request) =>
       throw new NotImplementedException();

    public IResponse Ok() => new Handled();
}

public interface IHandler<T, U> : IHandler where T : IRequest
{
    Task<U> HandleAsync(T request);
}

public interface IHandler<in T> : IHandler where T : IRequest
{
    Task<Void> HandleAsync(T request);
}