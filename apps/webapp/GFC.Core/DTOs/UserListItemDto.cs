namespace GFC.Core.DTOs;

/// <summary>
/// DTO for displaying users in a list.
/// </summary>
public record UserListItemDto(
    int UserId,
    string Username,
    bool IsAdmin,
    bool IsActive,
    int? MemberId,
    string? MemberName,
    DateTime? LastLoginDate,
    string? Notes,
    bool IsDirector = false);

