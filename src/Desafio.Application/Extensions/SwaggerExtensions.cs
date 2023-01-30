using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Desafio.Application;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services, string version = "v1")
    {
        var info = new OpenApiInfo { Title = "Desafio API", Version = version };

        var refer = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" };

        var safety = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        };
        var requirement = new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = refer }, new string[] { } } };

        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(version, info);
            opt.AddSecurityDefinition("Bearer", safety);
            opt.AddSecurityRequirement(requirement);
        });
    }
}