using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Persists audit-style notes tied to member changes.
/// </summary>
public interface IGlobalNoteRepository
{
    int InsertNote(GlobalNote note);
    void LogStatusChange(int memberId, string oldStatus, string newStatus, string? userName = null);
    IReadOnlyList<GlobalNote> GetByCategory(string category);
    GlobalNote? GetLatestByCategory(string category);
}

