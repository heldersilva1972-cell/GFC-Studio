using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Provides access to member key card assignment information.
/// </summary>
public interface IMemberKeycardRepository
{
    MemberKeycardAssignment? GetCurrentAssignmentForMember(int memberId);
    MemberKeycardAssignment? GetCurrentAssignmentForCard(int keyCardId);
    List<MemberKeycardAssignment> GetHistoryForMember(int memberId);
    int AddAssignment(MemberKeycardAssignment assignment);
    void CloseAssignment(int assignmentId, DateTime toDate, string closingReason);
    int GetActiveAssignmentCount();
}

