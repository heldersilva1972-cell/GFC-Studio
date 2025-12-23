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
    }
}
