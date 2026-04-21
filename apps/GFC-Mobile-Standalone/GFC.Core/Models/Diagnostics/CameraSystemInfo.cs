// [NEW]
using GFC.Core.Models;
using System;

namespace GFC.Core.Models.Diagnostics
{
    public class CameraSystemInfo
    {
        public int TotalCameras { get; set; }
        public int OnlineCameras { get; set; }
        public int OfflineCameras { get; set; }
        public int ActiveStreams { get; set; }
        public double StorageUsagePercentage { get; set; }
        public HealthStatus NvrStatus { get; set; }
        public DateTime OldestRecording { get; set; }
        public DateTime NewestRecording { get; set; }
        public int EventsLast24h { get; set; }
    }
}
