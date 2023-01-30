using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Infrastructure;

public record PostgresConnection : IConnection
{
    public string ConnectionString => "Server=localhost; Port=5432; Database=desafio; User Id=sa; Password=P4ssw0rd";

    public void UseContext(DbContextOptionsBuilder options) => options.UseNpgsql(ConnectionString);
}
