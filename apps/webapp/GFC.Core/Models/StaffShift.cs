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

        public int? StaffMemberId { get; set; }

        public DateTime Date { get; set; }

        public int ShiftType { get; set; } // 1=Day, 2=Night

        public string? Status { get; set; }

        public DateTime? ClockInTime { get; set; }

        public DateTime? ClockOutTime { get; set; }

        public DateTime? CustomStartTime { get; set; }

        public DateTime? CustomEndTime { get; set; }

        [NotMapped]
        public string StaffName { get; set; }

        [NotMapped]
        public DateTime StartTime => CustomStartTime ?? (ShiftType == 1 ? Date.Date.AddHours(9) : Date.Date.AddHours(18));

        [NotMapped]
        public DateTime EndTime => CustomEndTime ?? (ShiftType == 1 ? Date.Date.AddHours(17) : Date.Date.AddHours(26));

        [ForeignKey("StaffMemberId")]
        public virtual StaffMember? StaffMember { get; set; }
    }
}
