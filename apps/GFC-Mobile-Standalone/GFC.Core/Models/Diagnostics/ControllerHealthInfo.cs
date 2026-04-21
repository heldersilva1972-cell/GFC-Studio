// [NEW]
using GFC.Core.Models;
using System;
using System.Collections.Generic;

namespace GFC.Core.Models.Diagnostics
{
    public class ControllerHealthInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public HealthStatus Status { get; set; }
        public bool IsConnected { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public int DoorCount { get; set; }
        public int ReaderCount { get; set; }
        public int CardCount { get; set; }
        public int EventCount24h { get; set; }
        public DateTime LastCommunication { get; set; }
        public List<string> RecentEvents { get; set; }

        public ControllerHealthInfo()
        {
            RecentEvents = new List<string>();
        }
    }
}
