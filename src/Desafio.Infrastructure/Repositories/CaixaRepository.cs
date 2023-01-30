using Desafio.Domain;
using MongoDB.Driver;
using Azure.Core;

namespace Desafio.Infrastructure;

public class CaixaRepository : RepositoryFactory<Caixa, Guid>, ICaixaRepository
{
    public CaixaRepository(IServiceProvider provider) : base(provider) { }
}