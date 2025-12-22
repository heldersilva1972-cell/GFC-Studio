using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models.Diagnostics
{
    public class PerformanceSnapshot
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public double CpuUsage { get; set; }
        public double MemoryUsageGb { get; set; }
        public double MemoryUsagePercentage { get; set; }
        public int ActiveThreads { get; set; }
        public int ActiveConnections { get; set; }
        public int RequestsPerMinute { get; set; }
        public int Gen0Collections { get; set; }
        public int Gen1Collections { get; set; }
        public int Gen2Collections { get; set; }
    }
}
