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
            
            // 1. Fetch eligible candidates (already optimized in Repository to use batch history)
            var members = _memberRepository.GetLifeEligibleMembers(horizon, _historyRepository);
            var memberIds = members.Select(m => m.MemberID).ToList();

            if (!memberIds.Any()) return new List<LifeEligibilityDto>();

            // 2. Batch fetch payment status for current year
            var currentYear = DateTime.Today.Year;
            var currentYearPayments = _duesRepository.GetDuesForYear(currentYear)
                .Where(d => d.PaymentType != "UNPAID" && memberIds.Contains(d.MemberID))
                .Select(d => d.MemberID)
                .ToHashSet();

            // 3. Batch fetch last paid year
            var lastPaidDictionary = _duesRepository.GetLastPaidYears(memberIds);

            // 4. Batch fetch history for regularSince calculation (already handled in GetLifeEligibleMembers, but we need it here for DTO)
            var historyMap = _historyRepository.GetEarliestRegularDates(memberIds);

            return (IReadOnlyList<LifeEligibilityDto>)members
                .Select(m =>
                {
                    // Calculate regularSince consistently with GetLifeEligibleMembers logic
                    DateTime? historyDate = historyMap.TryGetValue(m.MemberID, out var d) ? d : null;
                    DateTime? statusDate = (string.Equals(m.Status, "REGULAR", StringComparison.OrdinalIgnoreCase) && m.StatusChangeDate.HasValue)
                        ? m.StatusChangeDate.Value : null;
                    var regularSince = historyDate ?? statusDate ?? m.AcceptedDate;

                    MemberStatusHelper.TryCalculateLifeEligibility(m, horizon, regularSince, out var eligibilityDate);
                    var age = m.DateOfBirth.HasValue ? (int)((horizon - m.DateOfBirth.Value).TotalDays / 365.25) : 0;
                    var isPaid = currentYearPayments.Contains(m.MemberID);
                    
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

