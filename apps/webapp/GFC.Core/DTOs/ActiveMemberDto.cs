namespace GFC.Core.DTOs;

/// <summary>
/// DTO for active members eligible for user account creation.
/// </summary>
public record ActiveMemberDto(
    int MemberId,
    string DisplayName,
    string FirstName,
    string LastName,
    string Status);

