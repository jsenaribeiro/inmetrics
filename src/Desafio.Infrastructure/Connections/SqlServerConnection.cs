using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure;

public record SqlServerConnection : IConnection
{
    public string ConnectionString => @"Server=localhost,1433; Database=desafio; User Id=SA; "
       + "Password=P4ssw0rd; MultipleActiveResultSets=true; TrustServerCertificate=True;";

    public void UseContext(DbContextOptionsBuilder options) => options.UseSqlServer(ConnectionString);
}