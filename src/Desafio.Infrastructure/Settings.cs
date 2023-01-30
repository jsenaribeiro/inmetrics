using static System.Environment;

namespace Desafio.Infrastructure;

public static class Settings
{
    public static bool HasAuthentication => false;

    public static bool IsDbSoftDeleted
    {
        get => GetEnvironmentVariable("DB_SOFT_DELETE") == "true";
        set => SetEnvironmentVariable("DB_SOFT_DELETE", value ? "true" : "false");
    }

    public static IConnection Database
    {
        get => SGBD.Create(GetEnvironmentVariable("DATABASE"));
        set => SetEnvironmentVariable("DATABASE", value.Name);
    }

    public static string JwtSecretKey
    {
        get => GetEnvironmentVariable("SECRET_KEY") ?? string.Empty;
        set => SetEnvironmentVariable("SECRET_KEY", value);
    }

    public static string ConnectionString => Database.ConnectionString;
}