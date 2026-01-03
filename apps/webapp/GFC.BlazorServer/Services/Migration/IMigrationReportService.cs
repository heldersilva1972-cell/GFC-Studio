namespace GFC.BlazorServer.Services.Migration;

public interface IMigrationReportService
{
    Task<string> GenerateReportAsync(int profileId, string adminName);
    Task<string?> GetFormattedReportAsync(int profileId);
}
