// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class ShiftReport
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StaffShift")]
        public int StaffShiftId { get; set; }
        public virtual StaffShift StaffShift { get; set; }

        public decimal SalesAmount { get; set; }

        public string Notes { get; set; }

        public DateTime ReportTime { get; set; }
    }
}
