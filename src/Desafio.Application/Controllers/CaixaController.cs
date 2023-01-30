using Microsoft.AspNetCore.Mvc;
using Desafio.Domain;

namespace Desafio.Application;

public class CaixaController : AbstractController
{
    public CaixaController(IServiceProvider provider) : base(provider) { }

    [HttpGet("[action]")]
    public Task<IActionResult> Caixa([FromQuery] CaixaDiarioRequest request) => SendAsync(request);

    [HttpPost("[action]")]
    public Task<IActionResult> Lancamento([FromBody] LancamentoRequest request) => SendAsync(request, true);
}