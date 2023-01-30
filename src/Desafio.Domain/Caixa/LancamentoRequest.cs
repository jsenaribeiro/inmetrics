namespace Desafio.Domain;

public record LancamentoRequest(decimal Valor, DateTime DataHora) : IRequest
{
    public LancamentoRequest(decimal Valor) : this(Valor, DateTime.Now) { }

    public Task ValidateAsync(Validation validation)
    {
        validation.AddRequired(Valor, nameof(Valor));
        validation.ThrowsExceptionIfInvalidated();

        return Task.CompletedTask;
    }
}
