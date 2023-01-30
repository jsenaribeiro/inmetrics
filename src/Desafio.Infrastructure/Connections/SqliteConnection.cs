using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure;

public record SqliteConnection : IConnection
{
    public string ConnectionString => "Data Source=database.db";

    public void UseContext(DbContextOptionsBuilder options) => options.UseSqlite(ConnectionString);
}