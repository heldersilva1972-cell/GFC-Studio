using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Contract for reading and writing dues payments.
/// </summary>
public interface IDuesRepository
{
    List<DuesPayment> GetAllDues();
    List<DuesPayment> GetDuesForYear(int year);
    List<DuesPayment> GetDuesForMember(int memberId);
    DuesPayment? GetDuesForMemberYear(int memberId, int year);
    List<int> GetDistinctYears();
    void UpsertDues(DuesPayment payment);
    bool HasPaidForYear(int memberId, int year);
    DuesPayment EnsureWaivedDuesRecord(int memberId, int year, string reasonNote);
    bool MemberHasAnyDues(int memberId);
    bool MemberHasDuesHistory(int memberId);
    bool MemberHasPaidOrWaivedDuesForYear(int memberId, int year);
    void UpdateDuesRecord(DuesPayment payment);
    void AppendNoteToUnpaidDues(int memberId, string note);
}

