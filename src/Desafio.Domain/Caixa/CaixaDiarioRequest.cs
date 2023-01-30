using System;

namespace Desafio.Domain;

public record CaixaDiarioRequest(DateTime Data) : IRequest
{
    public Task ValidateAsync(Validation validation)
    {
        if (Data == default || Data == DateTime.MinValue)
            validation.Add<RequiredException>(nameof(Data));

        validation.ThrowsExceptionIfInvalidated();

        return Task.CompletedTask;
    }
}
