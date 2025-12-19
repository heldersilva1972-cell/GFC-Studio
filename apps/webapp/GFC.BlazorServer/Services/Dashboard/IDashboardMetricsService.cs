using GFC.BlazorServer.Models.Dashboard;

namespace GFC.BlazorServer.Services.Dashboard;

public interface IDashboardMetricsService
{
    Task<DashboardMetricsDto> GetMetricsAsync(CancellationToken ct = default);
}

