using GFC.BlazorServer.Models.Dashboard;

namespace GFC.BlazorServer.Services.Dashboard;

public class DashboardSessionState
{
    public bool HasShownSplash { get; set; }
    public DashboardMetricsDto? CachedMetrics { get; set; }
    public DateTime? MetricsLastUpdated { get; set; }
}
