namespace GFC.Core.DTOs;

/// <summary>
/// Represents a guest awaiting promotion via the Non-Portuguese queue.
/// </summary>
public record NpQueueEntryDto(
    int Position,
    int MemberId,
    string FullName,
    DateTime? ApplicationDate,
    bool CanPromote);

