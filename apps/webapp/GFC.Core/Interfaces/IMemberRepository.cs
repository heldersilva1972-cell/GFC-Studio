using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Abstraction over member persistence operations so services can remain UI-agnostic.
/// </summary>
public interface IMemberRepository
{
    List<Member> GetAllMembers();
    Member? GetMemberById(int memberId);
    List<Member> SearchByLastName(string lastName);
    int InsertMember(Member member);
    void UpdateMember(Member member);
    void DeleteMember(int memberId);
    Dictionary<string, int> GetMemberCountsByStatus();
    int GetTotalMemberCount();
    (int newMembers, int deactivatedMembers) GetMemberChangeSummaryForYear(int year);
    List<string> GetDistinctCities();
    List<string> GetDistinctStates();
    List<string> GetDistinctPostalCodes();
    int GetNonPortugueseRegularCount();
    List<MemberQueueItem> GetNonPortugueseGuestQueue();
    int GetNonPortugueseQueueCount();
    List<Member> GetLifeMembers();
    List<Member> GetLifeEligibleMembers(DateTime asOfDate, IHistoryRepository? historyRepository = null);
    int GetLifeEligibleCount(DateTime asOfDate, IHistoryRepository? historyRepository = null);
}

