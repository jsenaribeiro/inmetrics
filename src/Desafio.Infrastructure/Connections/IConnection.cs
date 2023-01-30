using Microsoft.EntityFrameworkCore;
using System;

namespace Desafio.Infrastructure;

public interface IConnection
{
    string Name => this.GetType().Name;

    string ConnectionString { get; }

    void UseContext(DbContextOptionsBuilder options);

    bool IsInMemoryDatabase => Name == SGBD.InMemory.Name;

    bool Equals(IConnection? connection) => connection?.Name == this?.Name;
}