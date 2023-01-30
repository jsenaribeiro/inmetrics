
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using Desafio.Domain;

namespace Desafio.Application;

public static class JwtExtensions
{
    public static IServiceCollection AddAuthenticationJWT(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

        var secretKey = Encoding.ASCII.GetBytes(configuration["Secret:JWT"] ?? "");

        services
           .AddHttpContextAccessor()
           .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(x =>
           {
               x.SaveToken = true;
               x.RequireHttpsMetadata = false;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ClockSkew = TimeSpan.Zero,
                   IssuerSigningKey = new SymmetricSecurityKey(secretKey),
               };
           });

        services.AddTransient<ILoginService>(p => new LoginService(p));

        return services;
    }

    public static string Stringify(this SecurityTokenDescriptor descriptor)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}