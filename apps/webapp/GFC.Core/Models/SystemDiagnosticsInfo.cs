// [MODIFIED]
using GFC.Core.Models.Diagnostics;
using System;

namespace GFC.Core.Models
{
    /// <summary>
    /// Encapsulates comprehensive system diagnostic information, including performance and database health.
    /// </summary>
    public class SystemDiagnosticsInfo
    {
        /// <summary>
        /// Gets or sets the overall system health status.
        /// </summary>
        public HealthStatus OverallHealth { get; set; }

        /// <summary>
        /// Gets or sets the detailed system performance metrics.
        /// </summary>
        public PerformanceMetrics Performance { get; set; }

        /// <summary>
        /// Gets or sets the detailed database health information.
        /// </summary>
        public DatabaseHealthInfo DatabaseHealth { get; set; }

        /// <summary>
        /// Gets or sets the system uptime.
        /// </summary>
        public TimeSpan Uptime { get; set; }

        /// <summary>
        /// Gets or sets the .NET runtime version.
        /// </summary>
        public string DotNetVersion { get; set; }

        /// <summary>
        /// Gets or sets the operating system architecture.
        /// </summary>
        public string OsArchitecture { get; set; }

        /// <summary>
        /// Gets or sets the operating system description.
        /// </summary>
        public string OsDescription { get; set; }

        /// <summary>
        /// Gets or sets the process architecture.
        /// </summary>
        public string ProcessArchitecture { get; set; }

        /// <summary>
        /// Gets or sets the database connection string (for display purposes only).
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last update.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the health information for the hardware controllers.
        /// </summary>
        public ControllerHealthInfo ControllerHealth { get; set; }

        /// <summary>
        /// Gets or sets the health information for the camera system.
        /// </summary>
        public CameraSystemInfo CameraSystem { get; set; }

        public SystemDiagnosticsInfo()
        {
            Performance = new PerformanceMetrics();
            DatabaseHealth = new DatabaseHealthInfo();
            ControllerHealth = new ControllerHealthInfo();
            CameraSystem = new CameraSystemInfo();
        }
    }
}
