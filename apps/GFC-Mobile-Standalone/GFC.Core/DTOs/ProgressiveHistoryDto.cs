using System;

namespace GFC.Core.DTOs
{
    public class ProgressiveHistoryDto
    {
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Details { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public decimal? PrizePaid { get; set; }
        public int? BallGoal { get; set; }
        public int? BallsCalled { get; set; }
    }
}
