using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Defines the contract for evaluating whether a member can manage key card privileges.
/// </summary>
public interface ICardEligibilityService
{
    CardEligibilityResult Evaluate(Member member);
}
