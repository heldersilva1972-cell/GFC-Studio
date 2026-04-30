using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GFC.Core.DTOs;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IDashboardService
{
    Task<MemberSummaryDto> GetMemberSummaryAsync(CancellationToken cancellationToken = default);
    Task<DuesSummaryDto> GetCurrentYearDuesSummaryAsync(CancellationToken cancellationToken = default);
    Task<AlertSummaryDto> GetAlertSummaryAsync(List<Member>? members = null, CancellationToken cancellationToken = default);
    Task<BackupStatusDto> GetBackupStatusAsync(CancellationToken cancellationToken = default);
}

