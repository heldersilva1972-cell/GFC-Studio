using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    /// <summary>
    /// Partial class extension of <see cref="GoogleCalendarService"/> that adds
    /// bi-directional sync methods: Create/Update and Delete events via the Google Calendar REST API.
    /// These are idempotent — supply the existing <c>googleEventId</c> to update instead of create.
    /// </summary>
    public partial class GoogleCalendarService
    {
        // =====================================================================
        // CREATE / UPDATE
        // =====================================================================

        /// <summary>
        /// Creates a new Google Calendar event, or PATCHes an existing one when
        /// <paramref name="googleEventId"/> is already set.
        /// Returns the Google event ID on success, or <c>null</c> on failure.
        /// </summary>
        public async Task<string?> CreateOrUpdateEventAsync(
            string calendarId,
            GoogleCalendarSettings settings,
            string? googleEventId,
            string title,
            DateTime start,
            DateTime end,
            bool isAllDay = false,
            string? description = null,
            string? location = null)
        {
            try
            {
                var token = await GetServiceAccountAccessTokenAsync(settings);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("CreateOrUpdateEventAsync: unable to obtain access token.");
                    return null;
                }

                var bodyObject = BuildGoogleEventBody(title, start, end, isAllDay, description, location);
                var json = System.Text.Json.JsonSerializer.Serialize(bodyObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string encodedCalId = Uri.EscapeDataString(calendarId.Trim());
                HttpResponseMessage response;

                if (string.IsNullOrWhiteSpace(googleEventId))
                {
                    var url = $"https://www.googleapis.com/calendar/v3/calendars/{encodedCalId}/events";
                    response = await client.PostAsync(url, content);
                }
                else
                {
                    var url = $"https://www.googleapis.com/calendar/v3/calendars/{encodedCalId}/events/{Uri.EscapeDataString(googleEventId)}";
                    response = await client.PatchAsync(url, content);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("CreateOrUpdateEventAsync failed ({Status}): {Body}", response.StatusCode, err);
                    return null;
                }

                var resultJson = await response.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(resultJson);
                if (doc.RootElement.TryGetProperty("id", out var idProp))
                    return idProp.GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateOrUpdateEventAsync: unexpected error.");
            }

            return null;
        }

        // =====================================================================
        // DELETE
        // =====================================================================

        /// <summary>
        /// Deletes a Google Calendar event by its event ID.
        /// Returns <c>true</c> if the event was deleted or was already gone.
        /// </summary>
        public async Task<bool> DeleteEventAsync(
            string calendarId,
            GoogleCalendarSettings settings,
            string googleEventId)
        {
            if (string.IsNullOrWhiteSpace(googleEventId)) return false;

            try
            {
                var token = await GetServiceAccountAccessTokenAsync(settings);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("DeleteEventAsync: unable to obtain access token.");
                    return false;
                }

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var encodedCalId = Uri.EscapeDataString(calendarId.Trim());
                var encodedEvtId = Uri.EscapeDataString(googleEventId.Trim());
                var url = $"https://www.googleapis.com/calendar/v3/calendars/{encodedCalId}/events/{encodedEvtId}";
                var response = await client.DeleteAsync(url);

                // Already gone → treat as success
                if (response.StatusCode == System.Net.HttpStatusCode.Gone ||
                    response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return true;

                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("DeleteEventAsync failed ({Status}): {Body}", response.StatusCode, err);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteEventAsync: unexpected error for event '{EventId}'.", googleEventId);
                return false;
            }
        }

        // =====================================================================
        // HELPERS
        // =====================================================================

        private static object BuildGoogleEventBody(
            string title, DateTime start, DateTime end,
            bool isAllDay, string? description, string? location)
        {
            if (isAllDay)
            {
                return new
                {
                    summary = title,
                    description,
                    location,
                    start = new { date = start.ToString("yyyy-MM-dd") },
                    end   = new { date = end.ToString("yyyy-MM-dd") }
                };
            }

            return new
            {
                summary = title,
                description,
                location,
                start = new { dateTime = start.ToString("o"), timeZone = "America/New_York" },
                end   = new { dateTime = end.ToString("o"),   timeZone = "America/New_York" }
            };
        }
    }
}
