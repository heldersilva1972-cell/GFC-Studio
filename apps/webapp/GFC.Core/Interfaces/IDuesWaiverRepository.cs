using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Abstraction for managing dues waiver periods.
/// </summary>
public interface IDuesWaiverRepository
{
    List<DuesWaiverPeriod> GetWaiversForMember(int memberId);
    void AddWaiver(DuesWaiverPeriod waiver);
    void DeleteWaiver(int waiverId);
}

