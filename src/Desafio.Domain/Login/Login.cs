namespace Desafio.Domain;

public record Login(string User, string Role, string[] Tags)
{
    public Login(string user, string role) : this(user, role, new string[0]) { }

    public static Login Empty => new Login("", "");

    public bool IsEmpty => string.IsNullOrEmpty(User);
}
