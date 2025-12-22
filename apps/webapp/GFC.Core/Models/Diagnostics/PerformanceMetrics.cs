// [NEW]
namespace GFC.Core.Models.Diagnostics
{
    /// <summary>
    /// Represents the performance metrics of the system.
    /// </summary>
    public class PerformanceMetrics
    {
        /// <summary>
        /// Gets or sets the CPU usage percentage.
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the memory usage in gigabytes.
        /// </summary>
        public double MemoryUsageGb { get; set; }

        /// <summary>
        /// Gets or sets the memory usage as a percentage.
        /// </summary>
        public double MemoryUsagePercentage { get; set; }

        /// <summary>
        /// Gets or sets the number of active connections.
        /// </summary>
        public int ActiveConnections { get; set; }

        /// <summary>
        /// Gets or sets the number of active threads.
        /// </summary>
        public int ActiveThreads { get; set; }

        /// <summary>
        /// Gets or sets the number of requests per minute.
        /// </summary>
        public int RequestsPerMinute { get; set; }

        /// <summary>

        /// Gets or sets the number of Gen 0 garbage collections.
        /// </summary>
        public int Gen0Collections { get; set; }

        /// <summary>
        /// Gets or sets the number of Gen 1 garbage collections.
        /// </summary>
        public int Gen1Collections { get; set; }

        /// <summary>
        /// Gets or sets the number of Gen 2 garbage collections.
        /// </summary>
        public int Gen2Collections { get; set; }
    }
}
