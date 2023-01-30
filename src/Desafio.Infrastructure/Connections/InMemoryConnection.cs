using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure;

public record InMemoryConnection : IConnection
{
    public string ConnectionString => "INMEMORY";

    public void UseContext(DbContextOptionsBuilder options) => options.UseInMemoryDatabase(ConnectionString);
}