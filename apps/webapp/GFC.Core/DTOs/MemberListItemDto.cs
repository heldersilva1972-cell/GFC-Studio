using GFC.Core.Enums;

namespace GFC.Core.DTOs;

/// <summary>
/// Lightweight projection used by list/grid experiences.
/// </summary>
public record MemberListItemDto(
    int MemberId,
    string FullName,
    MemberStatus Status,
    string? City,
    DateTime? MemberSince,
    DateTime? RegularSince,
    bool IsNonPortuguese,
    bool HasKeyCard,
    DateTime? InactiveDate,
    bool AddressInvalid,
    DateTime? AddressInvalidDate);

