using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services.Controllers;

public static class SimulationTraceServiceExtensions
{
    /// <summary>
    /// Retrieves the most recent traces.
    /// </summary>
    /// <param name="service">The trace service.</param>
    /// <param name="count">The number of recent traces to retrieve.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of recent simulation traces.</returns>
    public static Task<IReadOnlyList<SimulationControllerTrace>> GetRecentTracesAsync(
        this ISimulationTraceService service,
        int count,
        CancellationToken ct = default)
    {
        return service.GetTracesAsync(pageSize: count, ct: ct);
    }
}
