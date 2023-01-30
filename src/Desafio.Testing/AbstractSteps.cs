using Desafio.Domain;
using Desafio.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Desafio.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DnsClient.Protocol;

namespace Desafio.Testing;

public enum MqType { Nenhum, Rabbit, Kafka }

public abstract class AbstractSteps : Factories
{
    protected IServiceProvider provider => builder.Services.BuildServiceProvider();

    protected readonly WebApplicationBuilder builder = WebApplication.CreateBuilder();

    protected readonly IConfiguration settings;

    protected readonly IUnitOfWork unitOfWork;

    protected AbstractSteps()
    {
        GetServiceProvider(ref builder);
        unitOfWork = new UnitOfWork(provider);
        settings = provider.GetService<IConfiguration>()!;
    }

    protected ObjectResult GetResult<C>(IRequest request) where C : AbstractController
    {
        var controller = Activator.CreateInstance(typeof(C), provider) as C;
        var result = controller?.SendAsync(request as dynamic).Result as ObjectResult;

        result?.ShouldBeAnExpectedResult();

        return result!;
    }

    [AfterScenario]
    [BeforeScenario]
    protected abstract void Cleanup();
}
