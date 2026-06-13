using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class PullTabGameDefinition : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string GameName { get; set; } = string.Empty;

        public int DefaultTicketCount { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18, 2)")]
        public decimal TicketPrice { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<PullTabPrizeOption> PrizeOptions { get; set; } = new List<PullTabPrizeOption>();
    }
}
