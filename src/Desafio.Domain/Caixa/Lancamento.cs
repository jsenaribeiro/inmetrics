using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Domain;

public class Lancamento : IEntity<Guid>
{
    public Lancamento(decimal valor, DateTime dataHora)
    {
        Id = Guid.NewGuid();
        Valor = valor;
        DataHora = dataHora;
    }

    public Guid Id { get; private set; }

    public decimal Valor { get; private set; }

    public DateTime DataHora { get; private set; }

    public TipoLancamento Tipo =>
         Valor > 0 ? TipoLancamento.Credito
       : Valor < 0 ? TipoLancamento.Debito
       : TipoLancamento.Nenhum;

    public Audit Audit { get; private set; } = Audit.Empty;
}
