namespace GFC.Core.Models;

/// <summary>
/// Lightweight view model used to populate the Key Cards grid.
/// </summary>
public class KeyCardMemberRow
{
    public int MemberId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string MemberStatus { get; set; } = string.Empty;
    public string? DuesPaymentType { get; set; }
    public DateTime? DuesPaidDate { get; set; }
    public string? PreviousYearPaymentType { get; set; }
    public DateTime? PreviousYearPaidDate { get; set; }
    public int? AssignmentId { get; set; }
    public int? KeyCardId { get; set; }
    public string? KeyCardNumber { get; set; }
    public bool IsNonPortugueseOrigin { get; set; }
    public string? Suffix { get; set; }

    public string DisplayName
    {
        get
        {
            var firstPart = string.IsNullOrWhiteSpace(MiddleName) ? FirstName : $"{FirstName} {MiddleName}";
            var name = string.IsNullOrWhiteSpace(LastName) ? firstPart : $"{LastName}, {firstPart}";
            return string.IsNullOrWhiteSpace(Suffix) ? name : $"{name} {Suffix}";
        }
    }
    public bool HasActiveAssignment => AssignmentId.HasValue;
}



