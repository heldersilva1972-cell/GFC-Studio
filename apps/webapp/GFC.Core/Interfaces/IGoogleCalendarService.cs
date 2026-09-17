using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.Core.Interfaces
{
    public interface IGoogleCalendarService
    {
        Task<GoogleCalendarSettings> GetSettingsAsync();
        Task SaveSettingsAsync(GoogleCalendarSettings settings);
        Task<List<CalendarEventItem>> FetchAllFeedsAsync(GoogleCalendarSettings settings);
        Task<List<CalendarEventItem>> FetchEventsFromUrlAsync(string icalUrl, string sourceName = "Public Calendar", string color = "#0d6efd");
        List<CalendarEventItem> ParseIcsContent(string icsText, string sourceName = "Public Calendar", string color = "#0d6efd");
        string ExportToIcs(IEnumerable<CalendarEventItem> events, string calendarName = "GFC Calendar Export");
        Task<(bool Success, string Message, int EventCount)> TestConnectionAsync(string icalUrl);
        Task<(bool Success, string Message)> TestWriteAccessAsync(GoogleCalendarSettings settings);
        Task<(bool Success, string Message, string? EventId)> CreateGoogleEventAsync(CalendarEventItem eventItem, GoogleCalendarSettings settings);
        Task<(bool Success, string Message)> UpdateGoogleEventAsync(string eventId, CalendarEventItem eventItem, GoogleCalendarSettings settings);
        Task<(bool Success, string Message)> DeleteGoogleEventAsync(string eventId, GoogleCalendarSettings settings);
        bool ParseServiceAccountJson(string jsonContent, GoogleCalendarSettings settings);
    }
}
