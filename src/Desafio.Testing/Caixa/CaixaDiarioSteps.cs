using Desafio.Application;
using Desafio.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using TechTalk.SpecFlow;

namespace Desafio.Testing;

[Binding]
[Scope(Feature = "consultar caixa diário")]
public class CaixaDiarioSteps : AbstractCaixaSteps
{
    private decimal valor;
    private DateTime dataHora;
    private ObjectResult resultado;
    private readonly string user = "test@email.com";

    public CaixaDiarioSteps() { valor = 0; }

    [Given(@"um crédito de R\$ (.*)")]
    public void GivenUmCreditoDeR(decimal valor) => this.valor = valor;

    [Given(@"cuja data é ""([^""]*)""")]
    public void GivenCujaDataE(string data) => this.dataHora = DateTime.Parse(data);

    [When(@"consultar caixa em ""([^""]*)""")]
    public void WhenConsultarCaixaEm(string dataString)
    {
        if (valor > 0)
        {
            var lancamento = new Lancamento(valor, dataHora);
            var caixa = new Domain.Caixa(DateOnly.FromDateTime(dataHora));

            caixa.IncluirLancamento(lancamento);

            unitOfWork.Caixas.SaveAsync(caixa).Wait();
        }

        var data = string.IsNullOrEmpty(dataString) ? default : DateTime.Parse(dataString);
        var request = new CaixaDiarioRequest { Data= data }; 

        resultado = GetResult<CaixaController>(request);
    }

    [Then(@"retorna mensagem de data é obrigatório")]
    public void ThenRetornaMensagemDeDataEObrigatorio() => resultado.Should<RequiredException>("Data");

    [Then(@"retorna o valor R\$ (.*)")]
    public void ThenRetornaOValorR(decimal valor) => resultado.Value.Should().Be(valor);
}
