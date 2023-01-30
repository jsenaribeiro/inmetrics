using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Desafio.Domain;
using Desafio.Infrastructure;
using System.Text.Json;

namespace Desafio.Application;

public record LoginService : ILoginService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IHttpContextAccessor accessor;

    public LoginService(IServiceProvider provider)
    {
        this.unitOfWork = provider.GetService<IUnitOfWork>()!;
        this.accessor = provider.GetService<IHttpContextAccessor>()!;
    }

    public async Task<string> LoginAsync(string guid, string user, string role, string[] tags)
    {
        if (!Settings.HasAuthentication) return string.Empty;

        var secret = Encoding.ASCII.GetBytes(Settings.JwtSecretKey);

        var claims = new Claim[]
        {
         new Claim(ClaimTypes.Sid, guid),
         new Claim(ClaimTypes.Name, user),
         new Claim(ClaimTypes.Role, role),
         new Claim(ClaimTypes.Email, user),
         new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(tags))
        };

        var credentials = new SigningCredentials(
           new SymmetricSecurityKey(secret),
           SecurityAlgorithms.HmacSha256Signature);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public async Task<Login> LogonAsync()
    {
        await Task.CompletedTask;

        if (!Settings.HasAuthentication)
            return new Login("anonymous", "user", new string[0]);

        var userClaimHttp = accessor?.HttpContext?.User;
        var unauthorized = !(userClaimHttp?.Identity?.IsAuthenticated ?? false);

        string? Get(string key) => userClaimHttp?.FindFirst(key)?.Value;

        if (unauthorized) throw new UnauthorizedException();

        var guid = Guid.Parse(Get(ClaimTypes.Sid) ?? Guid.Empty.ToString());
        var name = Get(ClaimTypes.Name) ?? string.Empty;
        var role = Get(ClaimTypes.Role) ?? string.Empty;
        var user = Get(ClaimTypes.Email) ?? string.Empty;
        var tags = JsonSerializer.Deserialize<string[]>(Get(ClaimTypes.UserData) ?? "[]") ?? new string[0];

        if (user is null) throw new UnauthorizedException();

        return new Login(user, role, tags);
    }

    public Task LogoutAsync()
    {
        var response = accessor?.HttpContext?.Response;

        if (response is not null)
            response.Headers.Remove("Authorization");

        return Task.CompletedTask;
    }
}
