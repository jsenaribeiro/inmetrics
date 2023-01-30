using Microsoft.AspNetCore.Builder;
using Org.BouncyCastle.Utilities;
using System.Data;
using System.Reflection;
using Desafio.Domain;

namespace Desafio.Infrastructure;

public static class EventBus
{
    private static async Task<dynamic?> SendAsync(IServiceProvider provider, Type type, dynamic request)
    {
        bool isSameArgument(MethodInfo method) =>
           type.Name == method.GetParameters()
              .FirstOrDefault()?.ParameterType.Name;

        var handledMethod = AppDomain.CurrentDomain
           .GetAssemblies().SelectMany(x => x.GetTypes())
           .Where(x => x!.IsAssignableTo(typeof(IHandle)))
           .SelectMany(x => x.GetMethods())
           .Where(x => x.Name == "HandleAsync")
           .FirstOrDefault(isSameArgument);

        if (handledMethod is not MethodInfo method) return default;
        if (method.DeclaringType is not Type handlerClass) return default;

        var argument = new[] { request };
        var instance = provider.GetService(handlerClass)!;

        if (instance is null) throw new NotImplementedException(handlerClass.Name);

        var returned = method.Invoke(instance, argument) as dynamic;
        var response = returned is not null ? await returned : null;

        if (response is IEvent e) Dispatch(new MQEvent(e));

        return response;
    }

    public static Task<dynamic?> SendAsync<R>(IServiceProvider provider, R request)
       where R : class, IRequest => SendAsync(provider, typeof(R), request);

    public static Task ListenAsync(WebApplication app, byte[] bytes) => ListenAsync(app, MQEvent.Deserialize(bytes));

    public static Task ListenAsync(WebApplication app, string json) => ListenAsync(app, MQEvent.Deserialize(json));

    public static async Task ListenAsync(WebApplication app, IEvent @event)
    {
        if (@event.GetType() is not Type argumentType) return;
        else await SendAsync(app.Services, argumentType, @event);
    }

    public static Action<MQEvent> Dispatch { get; set; } = x => { };
}