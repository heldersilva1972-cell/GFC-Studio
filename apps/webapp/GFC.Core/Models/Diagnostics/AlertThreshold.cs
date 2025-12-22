using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models.Diagnostics
{
    public enum MetricType
    {
        CpuUsage,
        MemoryUsagePercentage,
        ActiveConnections,
        RequestsPerMinute
    }

    public enum AlertLevel
    {
        Warning,
        Critical
    }

    public class AlertThreshold
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        public AlertLevel AlertLevel { get; set; }

        [Required]
        public double ThresholdValue { get; set; }

        [Required]
        public int CooldownMinutes { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
