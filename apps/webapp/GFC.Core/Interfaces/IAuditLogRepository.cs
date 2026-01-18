using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IAuditLogRepository
{
    void Insert(AuditLogEntry entry);

    Task<PagedResult<AuditLogRecord>> GetAuditLogsAsync(
        string? actionFilter,
        string? searchText,
        DateTimeOffset? from,
        DateTimeOffset? to,
        int? targetUserId,
        int pageNumber,
        int pageSize);

    Task<IReadOnlyList<string>> GetDistinctActionsAsync();
    Task<IReadOnlyList<AuditLogRecord>> GetLiveActivityAsync();
    void UpdateDuration(int userId, string pageUrl, int additionalSeconds);
}
