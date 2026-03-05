using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

public class LifeEligibilityService : ILifeEligibilityService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IHistoryRepository _historyRepository;
    private readonly IDuesRepository _duesRepository;

    public LifeEligibilityService(IMemberRepository memberRepository, IHistoryRepository historyRepository, IDuesRepository duesRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
    }

    public Task<IReadOnlyList<LifeEligibilityDto>> GetEligibleMembersAsync(bool includeUpcoming, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var horizon = includeUpcoming ? DateTime.Today.AddMonths(12) : DateTime.Today;
            var members = _memberRepository.GetLifeEligibleMembers(horizon, _historyRepository);

            var currentYear = DateTime.Today.Year;
            var currentYearDues = _duesRepository.GetDuesForYear(currentYear);
            var paidMemberIds = currentYearDues.Where(d => d.PaymentType != "UNPAID").Select(d => d.MemberID).ToHashSet();

            var allDues = _duesRepository.GetAllDues();
            var lastPaidDictionary = allDues
                .Where(d => d.PaymentType != "UNPAID")
                .GroupBy(d => d.MemberID)
                .ToDictionary(g => g.Key, g => g.Max(d => d.Year));

            return (IReadOnlyList<LifeEligibilityDto>)members
                .Select(m =>
                {
                    var regularSince = m.RegularSince ?? MemberStatusHelper.GetRegularSinceDate(m, _historyRepository);
                    MemberStatusHelper.TryCalculateLifeEligibility(m, horizon, _historyRepository, out var eligibilityDate);
                    var age = m.DateOfBirth.HasValue ? (int)((horizon - m.DateOfBirth.Value).TotalDays / 365.25) : 0;
                    var isPaid = paidMemberIds.Contains(m.MemberID);
                    
                    int? monthsUnpaid = null;
                    if (!isPaid)
                    {
                        if (lastPaidDictionary.TryGetValue(m.MemberID, out var lastPaidYear))
                        {
                            monthsUnpaid = Math.Max(0, (DateTime.Today.Year - lastPaidYear - 1) * 12 + DateTime.Today.Month);
                        }
                        else if (regularSince.HasValue)
                        {
                            monthsUnpaid = Math.Max(0, (DateTime.Today.Year - regularSince.Value.Year) * 12 + DateTime.Today.Month - regularSince.Value.Month);
                        }
                    }

                    return new LifeEligibilityDto(
                        m.MemberID,
                        BuildFullName(m.FirstName, m.MiddleName, m.LastName),
                        age,
                        regularSince,
                        eligibilityDate,
                        eligibilityDate.HasValue && eligibilityDate.Value <= DateTime.Today,
                        isPaid,
                        monthsUnpaid);
                })
                .OrderBy(dto => dto.EligibilityDate ?? DateTime.MaxValue)
                .ToList();
        }, cancellationToken);
    }

    private static string BuildFullName(string? first, string? middle, string? last)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(first)) parts.Add(first.Trim());
        if (!string.IsNullOrWhiteSpace(middle)) parts.Add(middle.Trim());
        if (!string.IsNullOrWhiteSpace(last)) parts.Add(last.Trim());
        return string.Join(" ", parts);
    }
}

