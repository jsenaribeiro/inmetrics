using System;
using System.Text.Json.Serialization;

namespace Desafio.Domain;

public class CaixaDiarioRequest : IRequest
{
   public DateTime Data { get; set; }

   public Task ValidateAsync(Validation validation)
    {
        if (Data == default || Data == DateTime.MinValue)
            validation.Add<RequiredException>(nameof(Data));

        validation.ThrowsExceptionIfInvalidated();

        return Task.CompletedTask;
    }
}
