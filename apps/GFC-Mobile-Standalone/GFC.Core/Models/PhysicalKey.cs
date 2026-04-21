namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a physical door key assigned to a member.
    /// </summary>
    public class PhysicalKey
    {
        public int PhysicalKeyID { get; set; }
        public int MemberID { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string? IssuedBy { get; set; }
        public string? ReturnedBy { get; set; }
        public string? Notes { get; set; }
        
        // Navigation properties (populated when needed)
        public string? MemberName { get; set; }
        public bool IsReturned => ReturnedDate.HasValue;
    }
}

