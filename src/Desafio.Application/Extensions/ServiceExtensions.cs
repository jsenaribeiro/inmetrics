using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Desafio.Domain;
using Desafio.Infrastructure;

namespace Desafio.Application;

public static class ServiceExtensions
{
    public static void AddDependencies(this IServiceCollection services, ref ILoginService loginService) => AddDependencies(services, loginService);

    public static void AddDependencies(this IServiceCollection services) => AddDependencies(services, null);

    private static void AddDependencies(this IServiceCollection services, ILoginService? loginService)
    {
        var testing = loginService is not null;

        Add<EfCoreContext>(services, testing);
        Add<Validation>(services, testing);
        Add<CaixaHandler>(services, testing);
        Add<IUnitOfWork, UnitOfWork>(services, testing);

        services.AddScoped<ILoginService>(p => loginService ?? new LoginService(p));
    }

    private static void Add<T>(IServiceCollection services, bool testing) where T : class
    {
        if (testing) services.AddTransient<T>();
        else services.AddScoped<T>();
    }
    private static void Add<T, U>(IServiceCollection services, bool testing) where T : class where U : class, T
    {
        if (testing) services.AddTransient<T, U>();
        else services.AddScoped<T, U>();
    }
}