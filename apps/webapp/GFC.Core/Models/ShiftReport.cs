// [MODIFIED]
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
        public int ShiftId { get; set; }
        public virtual StaffShift StaffShift { get; set; }

        public int BartenderId { get; set; }

        public decimal BarSales { get; set; }

        public decimal LottoSales { get; set; }

        public decimal TotalDeposit { get; set; }

        public DateTime SubmittedAt { get; set; }

        public bool IsLate { get; set; }
    }
}
