namespace GFC.BlazorServer.Models.Dashboard;

public class DashboardMetricsDto
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int PastDueMembers { get; set; }
    public int NpQueueCount { get; set; }
    public int EnabledCards { get; set; }
    public int DisabledCards { get; set; }
    public int OpenAlerts { get; set; }
    public int MembershipChangesLast24h { get; set; }
}

