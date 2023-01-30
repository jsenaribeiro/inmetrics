using Serilog;
using Microsoft.EntityFrameworkCore;
using Desafio.Domain;
using Desafio.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Update;

namespace Desafio.Infrastructure;

public class EfCoreContext : DbContext
{
    public virtual DbSet<Lancamento>? Lancamentos { get; set; }

    public virtual DbSet<Audit>? Audits { get; set; }

    public virtual DbSet<Caixa>? Caixas { get; set; }

    public EfCoreContext() => Database.EnsureCreated();

    public EfCoreContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var database = Settings.Database;

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        Console.WriteLine(database.ToString());

        database.UseContext(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyEntityMappings();
        base.OnModelCreating(modelBuilder);
    }
}