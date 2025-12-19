using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

public class LifeEligibilityService : ILifeEligibilityService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IHistoryRepository _historyRepository;

    public LifeEligibilityService(IMemberRepository memberRepository, IHistoryRepository historyRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
    }

    public Task<IReadOnlyList<LifeEligibilityDto>> GetEligibleMembersAsync(bool includeUpcoming, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var horizon = includeUpcoming ? DateTime.Today.AddMonths(12) : DateTime.Today;
            var members = _memberRepository.GetLifeEligibleMembers(horizon, _historyRepository);

            return (IReadOnlyList<LifeEligibilityDto>)members
                .Select(m =>
                {
                    var regularSince = m.RegularSince ?? MemberStatusHelper.GetRegularSinceDate(m, _historyRepository);
                    MemberStatusHelper.TryCalculateLifeEligibility(m, horizon, _historyRepository, out var eligibilityDate);
                    var age = m.DateOfBirth.HasValue ? (int)((horizon - m.DateOfBirth.Value).TotalDays / 365.25) : 0;

                    return new LifeEligibilityDto(
                        m.MemberID,
                        $"{m.FirstName} {m.LastName}",
                        age,
                        regularSince,
                        eligibilityDate,
                        eligibilityDate.HasValue && eligibilityDate.Value <= DateTime.Today);
                })
                .OrderBy(dto => dto.EligibilityDate ?? DateTime.MaxValue)
                .ToList();
        }, cancellationToken);
    }
}

