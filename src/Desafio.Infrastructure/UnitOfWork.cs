using Desafio.Domain;

namespace Desafio.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _provider;

    public UnitOfWork(IServiceProvider provider) => _provider = provider;

    public ICaixaRepository Caixas => new CaixaRepository(_provider);
}
