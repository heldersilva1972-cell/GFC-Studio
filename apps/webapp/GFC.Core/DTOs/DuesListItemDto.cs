using GFC.Core.Enums;
using System;

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
    bool Satisfied,
    bool IsWaived,
    string? WaiverReason,
    string Notes,
    bool IsBoardMember,
    bool IsInGracePeriod = false);
