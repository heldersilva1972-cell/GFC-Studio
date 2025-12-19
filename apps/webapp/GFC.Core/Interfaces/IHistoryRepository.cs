using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Provides access to member change history records.
/// </summary>
public interface IHistoryRepository
{
    void LogMemberChange(int memberId, string fieldName, string? oldValue, string? newValue);
    void LogMemberChange(int memberId, string fieldName, string? oldValue, string? newValue, string? changedBy);
    bool MemberHasHistory(int memberId);
    List<MemberChangeHistory> GetMemberHistory(int memberId);
    DateTime? GetGuestToRegularDate(int memberId);
    DateTime? GetEarliestRegularDate(int memberId);
}

