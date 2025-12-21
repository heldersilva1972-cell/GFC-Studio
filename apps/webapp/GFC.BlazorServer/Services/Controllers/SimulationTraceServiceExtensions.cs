using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services.Controllers;

public static class SimulationTraceServiceExtensions
{
    public static async Task<IReadOnlyList<SimulationControllerTrace>> GetRecentTracesAsync(
        this ISimulationTraceService service,
        int count = 10,
        CancellationToken ct = default)
    {
        return await service.GetTracesAsync(pageSize: count, ct: ct);
    }
}
