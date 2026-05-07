using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class BingoPayoutTier : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int MinAdmissions { get; set; }
        
        [Range(0, 100)]
        public decimal PayoutPercentage { get; set; } = 100;
    }
}
