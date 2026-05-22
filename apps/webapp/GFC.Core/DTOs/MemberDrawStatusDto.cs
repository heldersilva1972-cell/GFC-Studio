namespace GFC.Core.DTOs;

/// <summary>
/// DTO representing the validation status and eligibility of a member for the sign-in draw.
/// </summary>
public class MemberDrawStatusDto
{
    public int MemberId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool DuesPaid { get; set; }
    public bool IsWaived { get; set; }
    public string WaiverReason { get; set; } = string.Empty;
    public bool IsEligible { get; set; } // true if IsActive and (DuesPaid or IsWaived)
}

/// <summary>
/// DTO representing the list of available member IDs and the maximum ID value for the drawing game.
/// </summary>
public class MemberDrawPoolDto
{
    public List<int> MemberIds { get; set; } = new();
    public int MaxMemberId { get; set; }
    public List<MemberDrawPoolItemDto> Members { get; set; } = new();
}

/// <summary>
/// DTO representing an individual member's essential drawing identity and eligibility inside the offline pool cache.
/// </summary>
public class MemberDrawPoolItemDto
{
    public int MemberId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsEligible { get; set; }
}


