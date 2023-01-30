using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Domain;

public sealed class CaixaHandler
   : IHandler<CaixaDiarioRequest, decimal>
   , IHandler<LancamentoRequest>
{
    private readonly Validation validation;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILoginService loginService;

    public CaixaHandler(IServiceProvider provider)
    {
        validation = provider.Get<Validation>();
        unitOfWork = provider.Get<IUnitOfWork>();
        loginService = provider.Get<ILoginService>();
    }

    public async Task<decimal> HandleAsync(CaixaDiarioRequest request)
    {
        await request.ValidateAsync(validation);

        var data = DateOnly.FromDateTime(request.Data);

        var caixa = await unitOfWork.Caixas.LoadAsync(x => x.Data == data);
        if (caixa is null) throw new NotFoundException("Data");

        else return caixa.Lancamentos.Sum(x => x.Valor);
    }

    public async Task<Void> HandleAsync(LancamentoRequest request)
    {
        await request.ValidateAsync(validation);

        var data = DateOnly.FromDateTime(request.DataHora);

        var lancamento = new Lancamento(request.Valor, request.DataHora);

        var caixa = await unitOfWork.Caixas.LoadAsync(x => x.Data == data);

        caixa = caixa ?? new Caixa(data);

        caixa.IncluirLancamento(lancamento);

        await unitOfWork.Caixas.SaveAsync(caixa);

        return Void.Default;
    }
}
