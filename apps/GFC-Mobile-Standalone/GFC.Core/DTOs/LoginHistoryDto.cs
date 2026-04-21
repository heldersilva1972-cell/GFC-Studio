namespace GFC.Core.DTOs;

/// <summary>
/// DTO for displaying login history.
/// </summary>
public record LoginHistoryDto(
    int LoginHistoryId,
    int? UserId,
    string Username,
    DateTime LoginDate,
    string? IpAddress,
    bool LoginSuccessful,
    string? FailureReason);

