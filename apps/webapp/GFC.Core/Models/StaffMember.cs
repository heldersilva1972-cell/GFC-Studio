// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a staff member who can be assigned to shifts
    /// </summary>
    public class StaffMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Role { get; set; }

        /// <summary>
        /// Optional link to an existing GFC Member
        /// </summary>
        public int? MemberId { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        public decimal? HourlyRate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? HireDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation property
        public virtual ICollection<StaffShift> StaffShifts { get; set; } = new List<StaffShift>();
    }
}
