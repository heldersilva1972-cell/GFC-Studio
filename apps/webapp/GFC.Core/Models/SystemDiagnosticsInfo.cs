// [NEW]
using GFC.Core.Models.Diagnostics;
using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class SystemDiagnosticsInfo
    {
        public HealthStatus OverallHealth { get; set; }
        public long MemoryUsage { get; set; } // In MB
        public TimeSpan Uptime { get; set; }
        public string DotNetVersion { get; set; }
        public string OsArchitecture { get; set; }
        public string OsDescription { get; set; }
        public string ProcessArchitecture { get; set; }
        public string ConnectionString { get; set; } // For display only
        public DateTime LastUpdated { get; set; }
    }
}
