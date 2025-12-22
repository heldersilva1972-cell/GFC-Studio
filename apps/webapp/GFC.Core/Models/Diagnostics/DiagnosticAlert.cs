using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Diagnostics
{
    public class DiagnosticAlert
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AlertThresholdId { get; set; }

        [ForeignKey("AlertThresholdId")]
        public virtual AlertThreshold AlertThreshold { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public double TriggerValue { get; set; }

        public bool IsAcknowledged { get; set; } = false;

        public DateTime? AcknowledgedAt { get; set; }

        public string AcknowledgedBy { get; set; }
    }
}
