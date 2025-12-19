using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// High-level history helper used by the WebApp for logging and retrieving member change history.
/// </summary>
public interface IMemberHistoryService
{
    void LogChange(int memberId, string fieldName, string? oldValue, string? newValue, string? changedBy);
    IReadOnlyList<MemberChangeHistory> GetHistory(int memberId);
}

