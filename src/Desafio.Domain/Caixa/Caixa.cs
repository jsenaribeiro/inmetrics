using System.Collections;
using System.Collections.Immutable;

namespace Desafio.Domain;

public class Caixa : IEntity<Guid>, IAggregate
{
    private readonly List<Lancamento> _lancamentos;

    public Caixa(DateOnly data)
    {
        Data = data;
        _lancamentos = new List<Lancamento>();
    }

    public Guid Id { get; private set; }

    public DateOnly Data { get; private set; }

    public IReadOnlyList<Lancamento> Lancamentos => _lancamentos.AsReadOnly();

    public decimal Saldo => Lancamentos.Sum(x => x.Valor);

    public Audit Audit { get; private set; } = Audit.Empty;

    public void IncluirLancamento(Lancamento lancamento) => _lancamentos.Add(lancamento);

    public IEnumerator GetEnumerator() => Lancamentos.GetEnumerator();
}
