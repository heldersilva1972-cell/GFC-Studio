using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class RentalVisitorSession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string IpAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        
        // Page / Section visited: "Calendar", "Application", "Pricing Guide"
        public string Page { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;
        public bool IsEmbedded { get; set; }
        
        // Device & Client Information
        public string DeviceType { get; set; } = "Desktop"; // "Mobile", "Tablet", "Desktop"
        public string Browser { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        
        // Referral & Navigation
        public string ReferrerUrl { get; set; } = string.Empty;
        public string ReferrerSource { get; set; } = "Direct / Bookmark"; // "Google", "Facebook", "WordPress", "Direct"
        
        // Engagement & Funnel
        public int DurationSeconds { get; set; } = 0;
        public int MaxStepReached { get; set; } = 1; // 1 = Date & Time, 2 = Details, 3 = Review, 4 = Submitted
        public bool DidSubmit { get; set; } = false;
        
        // User history
        public int VisitCountForIp { get; set; } = 1;
        
        // Action logs (e.g., "Clicked Book Date: 2026-10-21", "Clicked Ask a Question", "Viewed Bereavement Tier")
        public List<string> Actions { get; set; } = new();
    }

    public class RentalTrackingPingRequest
    {
        public string? SessionId { get; set; }
        public string Page { get; set; } = string.Empty;
        public string? PageUrl { get; set; }
        public bool IsEmbedded { get; set; }
        public string? Referrer { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? OperatingSystem { get; set; }
        public int Step { get; set; } = 1;
        public int DurationSeconds { get; set; } = 0;
        public string? Action { get; set; }
        public bool DidSubmit { get; set; } = false;
    }

    public class RentalAnalyticsSummary
    {
        public int TotalViews { get; set; }
        public int UniqueVisitors { get; set; }
        public int MobileViews { get; set; }
        public int DesktopViews { get; set; }
        public int TabletViews { get; set; }
        public double MobilePercentage => TotalViews > 0 ? Math.Round((double)MobileViews / TotalViews * 100, 1) : 0;
        public double DesktopPercentage => TotalViews > 0 ? Math.Round((double)DesktopViews / TotalViews * 100, 1) : 0;
        public int AvgDurationSeconds { get; set; }
        
        // Page view counts
        public int CalendarViews { get; set; }
        public int ApplicationViews { get; set; }
        public int PricingGuideViews { get; set; }
        
        // Application Funnel counts
        public int FunnelStep1Views { get; set; }
        public int FunnelStep2Views { get; set; }
        public int FunnelStep3Views { get; set; }
        public int FunnelSubmissions { get; set; }
        public int InquiriesSent { get; set; }
        
        // Breakdown lists
        public List<KeyValuePair<string, int>> TopCities { get; set; } = new();
        public List<KeyValuePair<string, int>> TopReferrers { get; set; } = new();
        public List<KeyValuePair<string, int>> DailyViews { get; set; } = new();
        public List<RentalVisitorSession> RecentSessions { get; set; } = new();
    }
}
