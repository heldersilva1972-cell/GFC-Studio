namespace GFC.BlazorServer.Services.Migration;

public interface IMigrationReportService
{
    Task<string> GenerateReportAsync(int profileId);
    Task<string?> GetFormattedReportAsync(int profileId);
}
