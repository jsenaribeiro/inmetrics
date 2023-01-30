namespace Desafio.Domain;

public interface ILoginService
{
    Task<string> LoginAsync(string guid, string user, string role, string[] tags);

    Task<Login> LogonAsync();

    Task LogoutAsync();
}