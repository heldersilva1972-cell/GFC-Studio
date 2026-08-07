using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class TokenAuditLogEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string TokenName { get; set; } = string.Empty;
        public int ExpectedStock { get; set; }
        public int ActualCount { get; set; }
        public int Variance { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? PerformedBy { get; set; }
    }
}
