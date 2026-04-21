using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

public interface IDashboardService
{
    Task<MemberSummaryDto> GetMemberSummaryAsync(CancellationToken cancellationToken = default);
    Task<DuesSummaryDto> GetCurrentYearDuesSummaryAsync(CancellationToken cancellationToken = default);
    Task<AlertSummaryDto> GetAlertSummaryAsync(CancellationToken cancellationToken = default);
    Task<BackupStatusDto> GetBackupStatusAsync(CancellationToken cancellationToken = default);
}

