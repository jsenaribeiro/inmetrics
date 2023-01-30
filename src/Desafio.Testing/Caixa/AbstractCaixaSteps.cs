using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Testing;

public abstract class AbstractCaixaSteps : AbstractSteps
{
    protected Guid CaixaId = Guid.Empty;

    [AfterScenario]
    protected override void Cleanup()
    {
        var data = DateOnly.FromDateTime(new DateTime(1001, 10, 01));

        unitOfWork.Caixas
           .DropAsync(x => x.Id == CaixaId
                        || x.Data == data).Wait();
    }
}
