using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.SimulationReplay;

public interface IReplayService
{
    Task<IReadOnlyList<ReplayStep>> BuildReplayStepsAsync(
        string sessionFilter,
        CancellationToken ct = default);
}
