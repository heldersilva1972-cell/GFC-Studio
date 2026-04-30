using GFC.Core.DTOs;

namespace GFC.BlazorServer.Models.Dashboard;

public class DashboardMetricsDto
{
    public AlertSummaryDto? AlertSummary { get; set; }
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int PastDueMembers { get; set; }
    public int NpQueueCount { get; set; }
    public int EnabledCards { get; set; }
    public int DisabledCards { get; set; }
    public int OpenAlerts { get; set; }
    public int MembershipChangesLast24h { get; set; }

    // Bar & Staff Real-time Data
    public decimal WeeklyBarSales { get; set; }
    public int WeeklyBarTransactionCount { get; set; }
    public double WeeklyBarSalesTrend { get; set; }
    public int TodaysMemberEntryCount { get; set; }
    public int TodaysBuzzedInCount { get; set; }
    public List<BartenderInfo> TonightBartenders { get; set; } = new();
    public List<ActivityFeedItem> RecentActivities { get; set; } = new();

    // Sign-in Draw Tracking
    public bool SignInDrawReprintRecommended { get; set; }
    public DateTime? LastSignInDrawExportDate { get; set; }
    public DateTime? LastSignInDrawChangeDate { get; set; }
    public List<string> SignInDrawChangeReasons { get; set; } = new();
    public int SignInDrawTotalCount { get; set; }

    // Shift Note Monitoring
    public List<ShiftNoteAlert> UnacknowledgedNotes { get; set; } = new();
}

public class ShiftNoteAlert
{
    public int RecordId { get; set; }
    public string NoteType { get; set; } = "Bar"; // "Bar" or "Lottery"
    public DateTime ShiftDate { get; set; }
    public string ShiftType { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string NoteText { get; set; } = string.Empty;
    public bool IsRentalHall { get; set; }
}

public class BartenderInfo
{
    public string Name { get; set; } = string.Empty;
    public string Assignment { get; set; } = string.Empty;
}

public class ActivityFeedItem
{
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public DateTime TimestampUtc { get; set; }
}

