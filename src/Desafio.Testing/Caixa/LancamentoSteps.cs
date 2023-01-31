using Desafio.Domain;
using Desafio.Application;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Testing;

[Binding]
[Scope(Feature = "executar lancamentos")]
public class LancamentoSteps : AbstractCaixaSteps
{
   private LancamentoRequest request;
   private ObjectResult resultado;

   [Given(@"um lancamento de R\$ (.*)")]
   public void GivenUmLancamentoDeR(decimal valor) => request = new LancamentoRequest(valor);

   [Given(@"cuja data é ""([^""]*)""")]
   public void GivenCujaDataE(string dataString) => request.DataHora = DateTime.Parse(dataString);

   [When(@"executar seu lancamento")]
   public void WhenExecutarSeuLancamento() => resultado = GetResult<CaixaController>(request);

   [Then(@"acusará que valor é obrigatório")]
   public void ThenAcusaraQueValorEObrigatorio() => resultado.Should<RequiredException>("Valor");

   [Then(@"registra saldo de R\$ (.*)")]
   public void ThenRegistraSaldoDeR(decimal valor)
   {
      var data = DateOnly.FromDateTime(request.DataHora);
      var caixa = unitOfWork.Caixas.LoadAsync(x => x.Data == data).Result!;
      var saldo = caixa?.Lancamentos.Sum(x => x.Valor) ?? 0;

      saldo.Should().Be(valor);
   }
}
