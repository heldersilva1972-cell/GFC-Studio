using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Exposes lookups related to board-of-directors participation.
/// </summary>
public interface IBoardRepository
{
    List<BoardPosition> GetAllPositions();
    bool IsBoardMemberForYear(int memberId, int year);
    int? GetMostRecentTermYear();
    List<int> GetAllTermYears();
    List<BoardAssignment> GetAllAssignments();
    List<BoardAssignment> GetAssignmentsByYear(int termYear);
    int InsertAssignment(BoardAssignment assignment);
    void UpdateAssignment(BoardAssignment assignment);
    void RemoveAssignment(int assignmentId);
}

