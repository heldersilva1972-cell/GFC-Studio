namespace GFC.Core.Models;

/// <summary>
/// Represents a club member with all personal and status information.
/// </summary>
public class Member
{
    public int MemberID { get; set; }
    public string Status { get; set; } = string.Empty; // REGULAR, GUEST, LIFE, INACTIVE
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public string? Address1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? CellPhone { get; set; }
    public string? Email { get; set; }
    public DateTime? ApplicationDate { get; set; }
    public DateTime? AcceptedDate { get; set; }
    public DateTime? StatusChangeDate { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Notes { get; set; }
    public bool IsNonPortugueseOrigin { get; set; }
    public DateTime? LifeEligibleDate { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public DateTime? InactiveDate { get; set; }
    public bool AddressInvalid { get; set; }
    public DateTime? AddressInvalidDate { get; set; }

    public bool ShowOnWebsite { get; set; }
    
    /// <summary>
    /// Gets the date when the member became REGULAR status.
    /// This is calculated from change history (Guest to Regular transition) or StatusChangeDate,
    /// falling back to AcceptedDate if neither is available.
    /// This property is computed on-demand and is not stored in the database.
    /// </summary>
    public DateTime? RegularSince { get; set; }
}



