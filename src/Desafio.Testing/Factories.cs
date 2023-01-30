using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desafio.Application;
using Desafio.Domain;
using Desafio.Infrastructure;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Desafio.Testing;

public abstract class Factories
{
    private WebApplication app = WebApplication.Create();
    private ILoginService loginService = Substitute.For<ILoginService>();

    protected void GetServiceProvider(ref WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

        var logger = LoggerFactory.Create(builder => builder
           .AddFilter("Microsoft", LogLevel.Warning)
           .AddFilter("System", LogLevel.Warning)
           .AddConsole())
           .CreateLogger("testing");

        builder.Services.AddSingleton(logger);
        builder.Services.AddSingleton<IConfiguration>(configuration);
        builder.Services.AddDependencies(ref loginService);

        app = builder.Build();

        UseLoginMock("test");
    }

    protected void UseMqService(MqType mq)
    {
        if (mq == MqType.Rabbit) app.UseRabbitMQ("test");
        if (mq == MqType.Kafka) app.UseKafka("test");
    }

    protected Login Createlogin(IUnitOfWork unitOfWork) => Login.Empty;

    protected void UseLoginMock(string? username)
    {
        var login = new Login(username ?? "", string.Empty);

        loginService.LoginAsync("", "", "", new string[0])
           .ReturnsForAnyArgs(Task.FromResult(string.Empty));

        loginService.LogoutAsync().ReturnsForAnyArgs(ci =>
        {
            login = default(Login);
            return Task.CompletedTask;
        });

        if (username is null) loginService.LogonAsync().Throws<UnauthorizedException>();

        else loginService.LogonAsync().ReturnsForAnyArgs(x => Task.FromResult(login));
    }
}
