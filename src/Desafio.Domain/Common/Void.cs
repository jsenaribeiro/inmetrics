using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Domain;

public record struct Void() : IResponse
{
    public static Void Default => new Void();

    public static Task<Void> DefaultTask => Task.FromResult(Default);
}
