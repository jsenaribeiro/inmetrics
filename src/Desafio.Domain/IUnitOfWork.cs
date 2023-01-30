namespace Desafio.Domain;

public interface IUnitOfWork
{
    ICaixaRepository Caixas { get; }

    Task Begin() => throw new NotImplementedException();

    Task Commit() => throw new NotImplementedException();

    Task Rollback() => throw new NotImplementedException();
}