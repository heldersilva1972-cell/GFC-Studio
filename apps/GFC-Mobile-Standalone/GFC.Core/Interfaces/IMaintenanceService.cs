namespace GFC.Core.Interfaces;

public interface IMaintenanceService
{
    string GetAppVersion();
    List<string> GetDbMigrationStatus();
    Task<MaintenanceCountsDto> GetCountsAsync(CancellationToken cancellationToken = default);
    Task<bool> PingAgentAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<int, bool>> PingControllersAsync(CancellationToken cancellationToken = default);
}

public class MaintenanceCountsDto
{
    public int Members { get; set; }
    public int ActiveMembers { get; set; }
    public int Cards { get; set; }
    public int Reimbursements { get; set; }
    public int ControllerEvents { get; set; }
}
