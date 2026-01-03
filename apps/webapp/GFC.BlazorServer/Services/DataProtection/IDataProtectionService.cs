namespace GFC.BlazorServer.Services.DataProtection;

public enum DataHealthStatus
{
    Healthy,
    Warning,
    Critical,
    Unknown
}

public interface IDataProtectionService
{
    Task<DataHealthStatus> GetHealthStatusAsync();
    Task LogBackupCompletesAsync(int userId);
    Task LogRestoreTestCompletedAsync(int userId);
    Task<DateTime?> GetLastBackupTimeAsync();
    Task<DateTime?> GetLastRestoreTestTimeAsync();
}
