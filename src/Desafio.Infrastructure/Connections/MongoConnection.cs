using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure;

public record MongoConnection : IConnection
{
    public string ConnectionString => "mongodb://localhost:27017";

    public void UseContext(DbContextOptionsBuilder options) => new NotImplementedException();
}