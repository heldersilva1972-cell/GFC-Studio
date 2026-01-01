using GFC.Core.Enums;

namespace GFC.Core.DTOs;

public record DuesListItemDto(
    int MemberId,
    string FullName,
    MemberStatus Status,
    int Year,
    decimal? Amount,
    DateTime? PaidDate,
    string PaymentType,
    int MonthsOverdue,
    bool IsWaived,
    string? WaiverReason,
    string Notes,
    bool IsBoardMember);

