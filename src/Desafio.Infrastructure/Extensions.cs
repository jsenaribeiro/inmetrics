using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Infrastructure;

public static class Extensions
{
    public static void ApplyEntityMappings(this ModelBuilder builder) =>
          AppDomain.CurrentDomain.GetAssemblies().ToList()
             .ForEach(x => builder.ApplyConfigurationsFromAssembly(x));
}
