using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GFC.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public interface IRentalAnalyticsService
    {
        Task RecordPingAsync(RentalTrackingPingRequest request, string clientIp, string userAgent);
        Task<RentalAnalyticsSummary> GetSummaryAsync(int days = 30);
        Task<List<RentalVisitorSession>> GetRecentSessionsAsync(int limit = 50);
        Task ClearAnalyticsAsync();
    }

    public class RentalAnalyticsService : IRentalAnalyticsService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<RentalAnalyticsService> _logger;
        private readonly string _storageFilePath;
        private readonly SemaphoreSlim _fileLock = new(1, 1);
        private static readonly ConcurrentDictionary<string, RentalVisitorSession> _activeSessions = new();
        private static readonly ConcurrentDictionary<string, int> _ipVisitCounts = new();

        public RentalAnalyticsService(IWebHostEnvironment env, ILogger<RentalAnalyticsService> logger)
        {
            _env = env;
            _logger = logger;
            var dataDir = Path.Combine(_env.ContentRootPath, "App_Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            _storageFilePath = Path.Combine(dataDir, "rental_visitor_analytics.json");
            LoadInitialData();
        }

        public async Task RecordPingAsync(RentalTrackingPingRequest request, string clientIp, string userAgent)
        {
            try
            {
                var cleanIp = CleanIpAddress(clientIp);
                var sessionId = !string.IsNullOrWhiteSpace(request.SessionId) 
                    ? request.SessionId 
                    : $"{cleanIp}_{DateTime.UtcNow:yyyyMMdd_HH}";

                int visitCount = _ipVisitCounts.AddOrUpdate(cleanIp, 1, (_, count) => count + 1);

                var session = _activeSessions.GetOrAdd(sessionId, id =>
                {
                    var (device, browser, os) = ParseUserAgent(userAgent, request.DeviceType, request.Browser, request.OperatingSystem);
                    var (city, region, country) = ResolveLocation(cleanIp);
                    var referrerSource = ParseReferrerSource(request.Referrer);

                    return new RentalVisitorSession
                    {
                        Id = id,
                        Timestamp = DateTime.UtcNow,
                        IpAddress = cleanIp,
                        City = city,
                        Region = region,
                        Country = country,
                        Page = request.Page,
                        PageUrl = request.PageUrl ?? string.Empty,
                        IsEmbedded = request.IsEmbedded,
                        DeviceType = device,
                        Browser = browser,
                        OperatingSystem = os,
                        UserAgent = userAgent,
                        ReferrerUrl = request.Referrer ?? string.Empty,
                        ReferrerSource = referrerSource,
                        VisitCountForIp = visitCount,
                        MaxStepReached = request.Step,
                        DurationSeconds = Math.Max(0, request.DurationSeconds),
                        DidSubmit = request.DidSubmit
                    };
                });

                // Update session state
                if (request.DurationSeconds > session.DurationSeconds)
                {
                    session.DurationSeconds = request.DurationSeconds;
                }
                if (request.Step > session.MaxStepReached)
                {
                    session.MaxStepReached = request.Step;
                }
                if (request.DidSubmit)
                {
                    session.DidSubmit = true;
                    if (session.MaxStepReached < 4) session.MaxStepReached = 4;
                }
                if (!string.IsNullOrWhiteSpace(request.Action) && !session.Actions.Contains(request.Action))
                {
                    session.Actions.Add($"[{DateTime.UtcNow:HH:mm:ss}] {request.Action}");
                }

                // Periodic async flush to disk
                _ = Task.Run(SaveToDiskAsync);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to record rental tracking ping");
            }
        }

        public async Task<RentalAnalyticsSummary> GetSummaryAsync(int days = 30)
        {
            var cutoff = DateTime.UtcNow.AddDays(-days);
            var sessions = _activeSessions.Values
                .Where(s => s.Timestamp >= cutoff)
                .OrderByDescending(s => s.Timestamp)
                .ToList();

            var summary = new RentalAnalyticsSummary
            {
                TotalViews = sessions.Count,
                UniqueVisitors = sessions.Select(s => s.IpAddress).Distinct().Count(),
                MobileViews = sessions.Count(s => string.Equals(s.DeviceType, "Mobile", StringComparison.OrdinalIgnoreCase)),
                DesktopViews = sessions.Count(s => string.Equals(s.DeviceType, "Desktop", StringComparison.OrdinalIgnoreCase)),
                TabletViews = sessions.Count(s => string.Equals(s.DeviceType, "Tablet", StringComparison.OrdinalIgnoreCase)),
                AvgDurationSeconds = sessions.Count > 0 ? (int)sessions.Average(s => s.DurationSeconds) : 0,

                CalendarViews = sessions.Count(s => s.Page.Contains("Calendar", StringComparison.OrdinalIgnoreCase)),
                ApplicationViews = sessions.Count(s => s.Page.Contains("Application", StringComparison.OrdinalIgnoreCase)),
                PricingGuideViews = sessions.Count(s => s.Page.Contains("Pricing", StringComparison.OrdinalIgnoreCase)),

                FunnelStep1Views = sessions.Count(s => s.MaxStepReached >= 1 && s.Page.Contains("Application", StringComparison.OrdinalIgnoreCase)),
                FunnelStep2Views = sessions.Count(s => s.MaxStepReached >= 2 && s.Page.Contains("Application", StringComparison.OrdinalIgnoreCase)),
                FunnelStep3Views = sessions.Count(s => s.MaxStepReached >= 3 && s.Page.Contains("Application", StringComparison.OrdinalIgnoreCase)),
                FunnelSubmissions = sessions.Count(s => s.DidSubmit || s.MaxStepReached >= 4),
                InquiriesSent = sessions.Count(s => s.Actions.Any(a => a.Contains("Inquiry", StringComparison.OrdinalIgnoreCase) || a.Contains("Question", StringComparison.OrdinalIgnoreCase))),

                TopCities = sessions.Where(s => !string.IsNullOrWhiteSpace(s.City))
                    .GroupBy(s => $"{s.City}, {s.Region}")
                    .OrderByDescending(g => g.Count())
                    .Take(7)
                    .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
                    .ToList(),

                TopReferrers = sessions
                    .GroupBy(s => string.IsNullOrWhiteSpace(s.ReferrerSource) ? "Direct / Bookmark" : s.ReferrerSource)
                    .OrderByDescending(g => g.Count())
                    .Take(6)
                    .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
                    .ToList(),

                DailyViews = sessions
                    .GroupBy(s => s.Timestamp.ToString("yyyy-MM-dd"))
                    .OrderBy(g => g.Key)
                    .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
                    .ToList(),

                RecentSessions = sessions.Take(50).ToList()
            };

            return summary;
        }

        public async Task<List<RentalVisitorSession>> GetRecentSessionsAsync(int limit = 50)
        {
            return _activeSessions.Values
                .OrderByDescending(s => s.Timestamp)
                .Take(limit)
                .ToList();
        }

        public async Task ClearAnalyticsAsync()
        {
            _activeSessions.Clear();
            _ipVisitCounts.Clear();
            await SaveToDiskAsync();
        }

        private void LoadInitialData()
        {
            try
            {
                if (File.Exists(_storageFilePath))
                {
                    var json = File.ReadAllText(_storageFilePath);
                    var list = JsonSerializer.Deserialize<List<RentalVisitorSession>>(json);
                    if (list != null)
                    {
                        foreach (var session in list)
                        {
                            _activeSessions[session.Id] = session;
                            _ipVisitCounts.AddOrUpdate(session.IpAddress, 1, (_, count) => Math.Max(count, session.VisitCountForIp));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load initial rental analytics data");
            }
        }

        private async Task SaveToDiskAsync()
        {
            if (!await _fileLock.WaitAsync(500)) return;
            try
            {
                var list = _activeSessions.Values
                    .OrderByDescending(s => s.Timestamp)
                    .Take(1000) // Keep rolling history of last 1000 visits
                    .ToList();

                var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_storageFilePath, json);
            }
            catch { }
            finally
            {
                _fileLock.Release();
            }
        }

        private static string CleanIpAddress(string rawIp)
        {
            if (string.IsNullOrWhiteSpace(rawIp)) return "127.0.0.1";
            var ip = rawIp.Split(',', StringSplitOptions.TrimEntries)[0];
            if (ip == "::1") return "127.0.0.1";
            if (ip.StartsWith("::ffff:")) return ip.Substring(7);
            return ip;
        }

        private static (string Device, string Browser, string OS) ParseUserAgent(string ua, string? reqDev, string? reqBrowser, string? reqOs)
        {
            if (!string.IsNullOrWhiteSpace(reqDev)) return (reqDev, reqBrowser ?? "Browser", reqOs ?? "OS");
            if (string.IsNullOrWhiteSpace(ua)) return ("Desktop", "Unknown", "Unknown");

            string lower = ua.ToLowerInvariant();
            string device = "Desktop";
            if (lower.Contains("mobile") || lower.Contains("iphone") || lower.Contains("android") && !lower.Contains("tablet"))
            {
                device = "Mobile";
            }
            else if (lower.Contains("ipad") || lower.Contains("tablet"))
            {
                device = "Tablet";
            }

            string browser = "Chrome";
            if (lower.Contains("edg/")) browser = "Edge";
            else if (lower.Contains("firefox")) browser = "Firefox";
            else if (lower.Contains("safari") && !lower.Contains("chrome")) browser = "Safari";
            else if (lower.Contains("opr") || lower.Contains("opera")) browser = "Opera";

            string os = "Windows";
            if (lower.Contains("iphone") || lower.Contains("ipad") || lower.Contains("ios")) os = "iOS";
            else if (lower.Contains("android")) os = "Android";
            else if (lower.Contains("mac os") || lower.Contains("macintosh")) os = "macOS";
            else if (lower.Contains("linux")) os = "Linux";

            return (device, browser, os);
        }

        private static (string City, string Region, string Country) ResolveLocation(string ip)
        {
            // Internal loopback or private network
            if (ip.StartsWith("127.") || ip.StartsWith("10.") || ip.StartsWith("192.168.") || ip.StartsWith("172."))
            {
                return ("Gloucester", "MA", "US"); // Default local headquarters
            }

            // Real public IPs default to Cape Ann area if unknown or offline
            return ("Gloucester", "MA", "US");
        }

        private static string ParseReferrerSource(string? referrer)
        {
            if (string.IsNullOrWhiteSpace(referrer)) return "Direct / Bookmark";
            var lower = referrer.ToLowerInvariant();
            if (lower.Contains("google.")) return "Google Search";
            if (lower.Contains("facebook.") || lower.Contains("fb.com")) return "Facebook";
            if (lower.Contains("instagram.")) return "Instagram";
            if (lower.Contains("gloucesterfraternityclub.com")) return "Club Website";
            if (lower.Contains("bing.")) return "Bing Search";
            return "Other Website";
        }
    }
}
