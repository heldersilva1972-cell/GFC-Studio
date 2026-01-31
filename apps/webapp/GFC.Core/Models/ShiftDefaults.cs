using System;

namespace GFC.Core.Models
{
    public static class ShiftDefaults
    {
        public static TimeSpan DayStart { get; set; } = new TimeSpan(9, 0, 0);
        public static TimeSpan DayEnd { get; set; } = new TimeSpan(17, 0, 0);
        public static TimeSpan NightStart { get; set; } = new TimeSpan(18, 0, 0);
        public static TimeSpan NightEnd { get; set; } = new TimeSpan(2, 0, 0);

        public static void SetDefaults(TimeSpan dayStart, TimeSpan dayEnd, TimeSpan nightStart, TimeSpan nightEnd)
        {
            DayStart = dayStart;
            DayEnd = dayEnd;
            NightStart = nightStart;
            NightEnd = nightEnd;
        }
    }
}
