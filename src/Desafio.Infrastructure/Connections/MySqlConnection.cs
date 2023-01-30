using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure;

public record MySqlConnection : IConnection
{
    public string ConnectionString => "Server=localhost; port=3306; Database=desafio; Uid=root; Pwd=root";

    public void UseContext(DbContextOptionsBuilder options) => options.UseMySQL(ConnectionString);
}