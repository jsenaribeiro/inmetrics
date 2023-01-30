using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure;

public record OracleConnection : IConnection
{
    public string ConnectionString => "Data Source=localhost; User Id=sa; Password=P4ssw0rd; ";

    public void UseContext(DbContextOptionsBuilder options) => options.UseOracle(ConnectionString);
}