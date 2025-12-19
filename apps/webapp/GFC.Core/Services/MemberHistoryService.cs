using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Thin wrapper over <see cref="IHistoryRepository"/> that adds source tagging and provides
/// a single place for the WebApp to log history entries.
/// </summary>
public sealed class MemberHistoryService : IMemberHistoryService
{
    private readonly IHistoryRepository _historyRepository;

    public MemberHistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
    }

    public void LogChange(int memberId, string fieldName, string? oldValue, string? newValue, string? changedBy)
    {
        // The changedBy parameter should be the username from the authentication system
        _historyRepository.LogMemberChange(memberId, fieldName, oldValue, newValue, changedBy);
    }

    public IReadOnlyList<MemberChangeHistory> GetHistory(int memberId)
        => _historyRepository.GetMemberHistory(memberId);
}

