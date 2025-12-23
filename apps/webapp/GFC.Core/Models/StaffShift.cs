// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StaffShift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StaffMemberId { get; set; }

        public DateTime Date { get; set; }

        public int ShiftType { get; set; } // 1=Day, 2=Night

        public string Status { get; set; }

        [NotMapped]
        public string StaffName { get; set; }

        [NotMapped]
        public DateTime StartTime => ShiftType == 1 ? Date.AddHours(9) : Date.AddHours(18);

        [NotMapped]
        public DateTime EndTime => ShiftType == 1 ? Date.AddHours(17) : Date.AddHours(26);

        [ForeignKey("StaffMemberId")]
        public virtual AppUser StaffMember { get; set; }
    }
}
