using System;

namespace GFC.Core.Helpers
{
    public static class TimeSpanExtensions
    {
        public static string HoursToTime(this TimeSpan ts)
        {
            var dt = DateTime.Today.Add(ts);
            return dt.ToString("htt");
        }
    }
}
