using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Abstraction for managing dues waiver periods.
/// </summary>
public interface IDuesWaiverRepository
{
    List<DuesWaiverPeriod> GetWaiversForMember(int memberId);
    List<DuesWaiverPeriod> GetWaiversForYear(int year);
    List<DuesWaiverPeriod> GetAllWaivers();
    bool HasWaiverForYear(int memberId, int year);
    void AddWaiver(DuesWaiverPeriod waiver);
    void UpdateWaiver(DuesWaiverPeriod waiver);
    void DeleteWaiver(int waiverId);
    DuesWaiverPeriod? GetWaiverById(int waiverId);
}

