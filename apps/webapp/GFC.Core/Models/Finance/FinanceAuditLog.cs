using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Finance
{
    [Table("FinanceAuditLogs")]
    public class FinanceAuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string ActionType { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(150)]
        public string? PerformedBy { get; set; }
    }
}
