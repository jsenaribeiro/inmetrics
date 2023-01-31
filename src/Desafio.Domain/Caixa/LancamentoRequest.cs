namespace Desafio.Domain;

public class LancamentoRequest : IRequest
{
   public decimal Valor { get; set; }
   
   public DateTime DataHora { get; set; }

   public LancamentoRequest(decimal valor) => Valor = valor;

   public Task ValidateAsync(Validation validation)
   {
      validation.AddRequired(Valor, nameof(Valor));
      validation.ThrowsExceptionIfInvalidated();

      return Task.CompletedTask;
   }
}
