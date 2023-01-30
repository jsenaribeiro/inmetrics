namespace Desafio.Infrastructure;

public static class SGBD
{
    public static readonly IConnection MySql = new MySqlConnection();

    public static readonly IConnection Sqlite = new SqliteConnection();

    public static readonly IConnection Oracle = new OracleConnection();

    public static readonly IConnection SqlServer = new SqlServerConnection();

    public static readonly IConnection Postgres = new PostgresConnection();

    public static readonly IConnection Mongo = new MongoConnection();

    public static readonly IConnection InMemory = new InMemoryConnection();

    public static IConnection Create(string? name)
    {
        return name?.ToLower().Replace("connection", "") switch
        {
            "mysql" => new MySqlConnection(),
            "sqlite" => new SqliteConnection(),
            "sqlserver" => new SqlServerConnection(),
            "postgres" => new PostgresConnection(),
            "mongodb" => new MongoConnection(),
            "oracle" => new OracleConnection(),
            _ => new InMemoryConnection()
        };
    }
}