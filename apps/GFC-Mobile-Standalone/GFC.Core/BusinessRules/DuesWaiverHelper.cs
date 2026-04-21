using System;
using System.Linq;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.BusinessRules;

/// <summary>
/// Shared helper methods for managing dues waiver side-effects.
/// </summary>
public class DuesWaiverHelper
{
    private readonly IDuesRepository _duesRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IDuesWaiverRepository _duesWaiverRepository;

    public DuesWaiverHelper(
        IDuesRepository duesRepository,
        IMemberRepository memberRepository,
        IBoardRepository boardRepository,
        IDuesWaiverRepository duesWaiverRepository)
    {
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        _duesWaiverRepository = duesWaiverRepository ?? throw new ArgumentNullException(nameof(duesWaiverRepository));
    }

    public bool IsLifeMemberForYear(Member member, int year)
    {
        if (!MemberStatusHelper.IsLifeStatus(member.Status))
        {
            return false;
        }

        if (member.LifeEligibleDate.HasValue && member.LifeEligibleDate.Value.Year <= year)
        {
            return true;
        }

        if (member.StatusChangeDate.HasValue && member.StatusChangeDate.Value.Year <= year)
        {
            return true;
        }

        return !member.LifeEligibleDate.HasValue && !member.StatusChangeDate.HasValue;
    }

    public bool IsBoardMemberForYear(int memberId, int year)
    {
        return _boardRepository.IsBoardMemberForYear(memberId, year);
    }

    public bool HasServiceWaiverForYear(int memberId, int year)
    {
        return _duesWaiverRepository
            .GetWaiversForMember(memberId)
            .Any(w => year >= w.StartYear && year <= w.EndYear);
    }

    public void EnsureServiceWaived(int memberId, int year, string reason)
    {
        _duesRepository.EnsureWaivedDuesRecord(memberId, year, reason);
    }

    public void RevertServiceWaiverForYear(int memberId, int year)
    {
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            return;
        }

        if (IsLifeMemberForYear(member, year) || IsBoardMemberForYear(memberId, year))
        {
            return;
        }

        var record = _duesRepository.GetDuesForMemberYear(memberId, year);
        if (record == null || !IsServiceWaiverRecord(record))
        {
            return;
        }

        record.PaidDate = null;
        record.PaymentType = null;
        record.Amount = null;
        record.Notes = null;

        _duesRepository.UpdateDuesRecord(record);
    }

    private static bool IsServiceWaiverRecord(DuesPayment payment)
    {
        return payment.PaymentType != null &&
               payment.PaymentType.Equals("WAIVED", StringComparison.OrdinalIgnoreCase) &&
               (payment.Notes?.Contains("Service", StringComparison.OrdinalIgnoreCase) ?? false);
    }
}



