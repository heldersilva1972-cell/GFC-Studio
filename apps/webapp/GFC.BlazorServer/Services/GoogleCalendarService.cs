using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GoogleCalendarService> _logger;
        private readonly string _settingsFilePath;

        public GoogleCalendarService(
            IHttpClientFactory httpClientFactory,
            IWebHostEnvironment env,
            ILogger<GoogleCalendarService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
            _logger = logger;
            _settingsFilePath = Path.Combine(_env.ContentRootPath, "App_Data", "google_calendar_settings.json");
        }

        public async Task<GoogleCalendarSettings> GetSettingsAsync()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var json = await File.ReadAllTextAsync(_settingsFilePath);
                    var settings = JsonSerializer.Deserialize<GoogleCalendarSettings>(json);
                    if (settings != null)
                        return settings;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading Google Calendar settings from file.");
            }

            return new GoogleCalendarSettings();
        }

        public async Task SaveSettingsAsync(GoogleCalendarSettings settings)
        {
            try
            {
                var dir = Path.GetDirectoryName(_settingsFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Google Calendar settings to file.");
                throw;
            }
        }

        public async Task<List<CalendarEventItem>> FetchAllFeedsAsync(GoogleCalendarSettings settings)
        {
            var allEvents = new List<CalendarEventItem>();

            // Ensure backward compatibility if Feeds is empty but PublicCalendarUrl is set
            if ((settings.Feeds == null || settings.Feeds.Count == 0) && !string.IsNullOrWhiteSpace(settings.PublicCalendarUrl))
            {
                settings.Feeds = new List<CalendarFeedConfig>
                {
                    new()
                    {
                        Name = string.IsNullOrWhiteSpace(settings.CalendarName) ? "Primary Calendar" : settings.CalendarName,
                        Url = settings.PublicCalendarUrl,
                        Color = "#0d6efd",
                        IsEnabled = true
                    }
                };
            }

            if (settings.Feeds == null || settings.Feeds.Count == 0)
                return allEvents;

            foreach (var feed in settings.Feeds)
            {
                if (!feed.IsEnabled || string.IsNullOrWhiteSpace(feed.Url))
                    continue;

                try
                {
                    var feedEvents = await FetchEventsFromUrlAsync(feed.Url, feed.Name, feed.Color);
                    feed.LastSyncedAt = DateTime.Now;
                    feed.EventCount = feedEvents.Count;
                    feed.LastStatus = $"OK ({feedEvents.Count} events)";
                    allEvents.AddRange(feedEvents);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to sync calendar feed '{FeedName}'", feed.Name);
                    feed.LastStatus = $"Error: {ex.Message}";
                }
            }

            return allEvents;
        }

        public async Task<List<CalendarEventItem>> FetchEventsFromUrlAsync(string icalUrl, string sourceName = "Public Calendar", string color = "#0d6efd")
        {
            if (string.IsNullOrWhiteSpace(icalUrl))
                return new List<CalendarEventItem>();

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; GFC-System/2.0; +https://gloucesterfisherman.com)");
            client.Timeout = TimeSpan.FromSeconds(25);

            // Handle webcal:// links by replacing with https://
            if (icalUrl.StartsWith("webcal://", StringComparison.OrdinalIgnoreCase))
            {
                icalUrl = "https://" + icalUrl.Substring(9);
            }

            var response = await client.GetAsync(icalUrl);
            response.EnsureSuccessStatusCode();

            var icsContent = await response.Content.ReadAsStringAsync();
            return ParseIcsContent(icsContent, sourceName, color);
        }

        public async Task<(bool Success, string Message, int EventCount)> TestConnectionAsync(string icalUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(icalUrl))
                    return (false, "Please provide a valid Calendar URL.", 0);

                var events = await FetchEventsFromUrlAsync(icalUrl);
                return (true, $"Successfully connected! Found {events.Count} event(s).", events.Count);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogWarning(httpEx, "Failed to download calendar feed.");
                return (false, $"HTTP Connection error: {httpEx.Message}. Check if the calendar is public or if the secret URL is correct.", 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing calendar connection.");
                return (false, $"Failed to parse calendar: {ex.Message}", 0);
            }
        }

        public List<CalendarEventItem> ParseIcsContent(string icsText, string sourceName = "Public Calendar", string color = "#0d6efd")
        {
            var events = new List<CalendarEventItem>();
            if (string.IsNullOrWhiteSpace(icsText))
                return events;

            // Step 1: Unfold lines (RFC 5545 specifies that lines starting with space/tab are continuation of previous line)
            var unfolded = Regex.Replace(icsText, @"\r?\n[ \t]", string.Empty);
            var lines = unfolded.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            CalendarEventItem? current = null;
            bool insideEvent = false;

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.Equals("BEGIN:VEVENT", StringComparison.OrdinalIgnoreCase))
                {
                    insideEvent = true;
                    current = new CalendarEventItem
                    {
                        Source = sourceName,
                        ColorCategory = "badge-primary"
                    };
                    continue;
                }

                if (line.Equals("END:VEVENT", StringComparison.OrdinalIgnoreCase))
                {
                    if (insideEvent && current != null)
                    {
                        NormalizeEventData(current);

                        // Ignore cancelled events
                        if (!string.Equals(current.Status, "CANCELLED", StringComparison.OrdinalIgnoreCase) &&
                            !string.IsNullOrWhiteSpace(current.Title))
                        {
                            events.Add(current);
                        }
                    }
                    insideEvent = false;
                    current = null;
                    continue;
                }

                if (!insideEvent || current == null)
                    continue;

                // Split into Key (and parameters) and Value
                var colonIndex = line.IndexOf(':');
                if (colonIndex <= 0)
                    continue;

                var keyPart = line.Substring(0, colonIndex);
                var valuePart = line.Substring(colonIndex + 1);

                // Handle parameters like DTSTART;VALUE=DATE:20260916 or DTSTART;TZID=America/New_York:20260916T150000
                var key = keyPart;
                string? paramPart = null;
                var semiIndex = keyPart.IndexOf(';');
                if (semiIndex > 0)
                {
                    key = keyPart.Substring(0, semiIndex);
                    paramPart = keyPart.Substring(semiIndex + 1);
                }

                switch (key.ToUpperInvariant())
                {
                    case "UID":
                        current.Uid = valuePart;
                        // Google Calendar UIDs often format as id@google.com or id_R...
                        if (valuePart.EndsWith("@google.com", StringComparison.OrdinalIgnoreCase))
                        {
                            current.GoogleEventId = valuePart.Substring(0, valuePart.Length - "@google.com".Length);
                        }
                        else
                        {
                            current.GoogleEventId = valuePart;
                        }
                        break;
                    case "SUMMARY":
                        current.Title = UnescapeIcsText(valuePart);
                        break;
                    case "DESCRIPTION":
                        current.Description = UnescapeIcsText(valuePart);
                        break;
                    case "LOCATION":
                        current.Location = UnescapeIcsText(valuePart);
                        break;
                    case "STATUS":
                        current.Status = valuePart.Trim().ToUpperInvariant();
                        break;
                    case "DTSTART":
                        var (startDate, startIsAllDay) = ParseIcsDateTime(valuePart, paramPart);
                        current.Start = startDate;
                        if (startIsAllDay) current.IsAllDay = true;
                        break;
                    case "DTEND":
                        var (endDate, _) = ParseIcsDateTime(valuePart, paramPart);
                        current.End = endDate;
                        break;
                }
            }

            return events;
        }

        public string ExportToIcs(IEnumerable<CalendarEventItem> events, string calendarName = "GFC Calendar Export")
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//Gloucester Fishermen's Club//GFC System 2.0//EN");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:PUBLISH");
            sb.AppendLine($"X-WR-CALNAME:{EscapeIcsText(calendarName)}");

            foreach (var evt in events)
            {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine($"UID:{evt.Uid}");
                sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
                
                if (evt.IsAllDay)
                {
                    sb.AppendLine($"DTSTART;VALUE=DATE:{evt.Start:yyyyMMdd}");
                    sb.AppendLine($"DTEND;VALUE=DATE:{evt.End:yyyyMMdd}");
                }
                else
                {
                    sb.AppendLine($"DTSTART:{evt.Start.ToUniversalTime():yyyyMMddTHHmmssZ}");
                    sb.AppendLine($"DTEND:{evt.End.ToUniversalTime():yyyyMMddTHHmmssZ}");
                }

                sb.AppendLine($"SUMMARY:{EscapeIcsText(evt.Title)}");
                
                if (!string.IsNullOrEmpty(evt.Description))
                    sb.AppendLine($"DESCRIPTION:{EscapeIcsText(evt.Description)}");
                
                if (!string.IsNullOrEmpty(evt.Location))
                    sb.AppendLine($"LOCATION:{EscapeIcsText(evt.Location)}");

                sb.AppendLine($"STATUS:{evt.Status ?? "CONFIRMED"}");
                sb.AppendLine("END:VEVENT");
            }

            sb.AppendLine("END:VCALENDAR");
            return sb.ToString();
        }

        private static (DateTime Date, bool IsAllDay) ParseIcsDateTime(string value, string? paramPart)
        {
            value = value.Trim();
            bool isAllDay = (paramPart != null && paramPart.IndexOf("VALUE=DATE", StringComparison.OrdinalIgnoreCase) >= 0) || value.Length == 8;

            if (isAllDay && value.Length >= 8)
            {
                if (DateTime.TryParseExact(value.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                {
                    return (d, true);
                }
            }

            // ISO / iCal datetime formats: yyyyMMddTHHmmssZ or yyyyMMddTHHmmss
            string[] formats = { "yyyyMMdd'T'HHmmss'Z'", "yyyyMMdd'T'HHmmss", "yyyyMMdd'T'HHmm'Z'", "yyyyMMdd'T'HHmm" };
            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var dt))
            {
                return (dt.ToLocalTime(), false);
            }

            if (DateTime.TryParse(value, out var fallback))
            {
                return (fallback, isAllDay);
            }

            return (DateTime.UtcNow, false);
        }

        private static string UnescapeIcsText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace("\\n", "\n", StringComparison.OrdinalIgnoreCase)
                       .Replace("\\N", "\n")
                       .Replace("\\,", ",")
                       .Replace("\\;", ";")
                       .Replace("\\\\", "\\");
        }

        private static void NormalizeEventData(CalendarEventItem current)
        {
            // 1. Recover true dates if Start is invalid/epoch (e.g. 1969/1970) or if description has preferred event date
            if (!string.IsNullOrEmpty(current.Description))
            {
                var cleanDesc = Regex.Replace(current.Description, @"<br\s*/?>|</?p>", "\n", RegexOptions.IgnoreCase);
                cleanDesc = cleanDesc.Replace("&nbsp;", " ").Replace("&amp;", "&");

                // Check for "Preferred Event Date : MM/dd/yyyy" or "Event Date : MM/dd/yyyy"
                var dateMatch = Regex.Match(cleanDesc, @"(?:Preferred Event Date|Event Date)\s*:\s*([0-9]{1,2}/[0-9]{1,2}/[0-9]{4})", RegexOptions.IgnoreCase);
                if (dateMatch.Success && DateTime.TryParse(dateMatch.Groups[1].Value, out var preferredDate))
                {
                    var startTimeMatch = Regex.Match(cleanDesc, @"(?:Event Start Time|Start Time)\s*:\s*([0-9]{1,2}(?::[0-9]{2})?\s*(?:am|pm)?)", RegexOptions.IgnoreCase);
                    var endTimeMatch = Regex.Match(cleanDesc, @"(?:Event End Time|End Time)\s*:\s*([0-9]{1,2}(?::[0-9]{2})?\s*(?:am|pm)?)", RegexOptions.IgnoreCase);

                    if (startTimeMatch.Success && DateTime.TryParse($"{preferredDate:yyyy-MM-dd} {startTimeMatch.Groups[1].Value}", out var parsedStart))
                    {
                        current.Start = parsedStart;
                        current.IsAllDay = false;
                    }
                    else if (current.Start.Year < 2000)
                    {
                        current.Start = preferredDate;
                        current.IsAllDay = true;
                    }

                    if (endTimeMatch.Success && DateTime.TryParse($"{preferredDate:yyyy-MM-dd} {endTimeMatch.Groups[1].Value}", out var parsedEnd))
                    {
                        current.End = parsedEnd;
                    }
                    else if (current.End.Year < 2000 || current.End < current.Start)
                    {
                        current.End = current.IsAllDay ? current.Start.AddDays(1) : current.Start.AddHours(4);
                    }
                }
            }

            // 2. Normalize end time if missing
            if (current.End == DateTime.MinValue || current.End < current.Start)
            {
                current.End = current.IsAllDay ? current.Start.AddDays(1) : current.Start.AddHours(1);
            }

            // 3. Status classification
            if (current.Title.StartsWith("PENDING", StringComparison.OrdinalIgnoreCase))
            {
                current.Status = "PENDING";
                current.ColorCategory = "badge-warning";
            }
            else if (current.Title.StartsWith("APPROVED", StringComparison.OrdinalIgnoreCase))
            {
                current.Status = "APPROVED";
                current.ColorCategory = "badge-success";
            }
            else if (current.Title.StartsWith("DENIED", StringComparison.OrdinalIgnoreCase) || current.Title.StartsWith("CANCEL", StringComparison.OrdinalIgnoreCase))
            {
                current.Status = "DENIED";
                current.ColorCategory = "badge-danger";
            }
            else
            {
                current.Status = string.IsNullOrWhiteSpace(current.Status) ? "CONFIRMED" : current.Status;
                current.ColorCategory = "badge-primary";
            }
        }

        public bool ParseServiceAccountJson(string jsonContent, GoogleCalendarSettings settings)
        {
            try
            {
                using var doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;
                if (root.TryGetProperty("client_email", out var emailProp))
                {
                    settings.ServiceAccountEmail = emailProp.GetString() ?? string.Empty;
                }
                if (root.TryGetProperty("private_key", out var keyProp))
                {
                    settings.ServiceAccountPrivateKey = keyProp.GetString() ?? string.Empty;
                }
                if (root.TryGetProperty("project_id", out var projProp))
                {
                    settings.ServiceAccountProjectNumber = projProp.GetString() ?? string.Empty;
                }

                return !string.IsNullOrWhiteSpace(settings.ServiceAccountEmail) && !string.IsNullOrWhiteSpace(settings.ServiceAccountPrivateKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse Service Account JSON key.");
                return false;
            }
        }

        public async Task<(bool Success, string Message)> TestWriteAccessAsync(GoogleCalendarSettings settings)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(settings.ServiceAccountEmail) || string.IsNullOrWhiteSpace(settings.ServiceAccountPrivateKey))
                {
                    return (false, "Service Account email and private key are required.");
                }

                var calId = string.IsNullOrWhiteSpace(settings.PrimaryGoogleCalendarId) ? "primary" : settings.PrimaryGoogleCalendarId.Trim();
                var token = await GetServiceAccountAccessTokenAsync(settings);
                if (string.IsNullOrEmpty(token))
                {
                    return (false, "Authentication failed: Could not generate OAuth2 access token with provided Service Account credentials.");
                }

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Check calendar access by fetching calendar metadata
                var url = $"https://www.googleapis.com/calendar/v3/calendars/{Uri.EscapeDataString(calId)}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(content);
                    var summary = doc.RootElement.TryGetProperty("summary", out var s) ? s.GetString() : calId;
                    return (true, $"Success! Connected to calendar '{summary}' with full write access.");
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, $"Google API error ({response.StatusCode}): {err}. Make sure the calendar is shared with '{settings.ServiceAccountEmail}' with 'Make changes to events' permission.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Write test failed: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message, string? EventId)> CreateGoogleEventAsync(CalendarEventItem eventItem, GoogleCalendarSettings settings)
        {
            try
            {
                var token = await GetServiceAccountAccessTokenAsync(settings);
                if (string.IsNullOrEmpty(token))
                {
                    return (false, "Failed to obtain Google access token.", null);
                }

                var calId = string.IsNullOrWhiteSpace(settings.PrimaryGoogleCalendarId) ? "primary" : settings.PrimaryGoogleCalendarId.Trim();
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var payload = BuildGoogleEventJson(eventItem);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var url = $"https://www.googleapis.com/calendar/v3/calendars/{Uri.EscapeDataString(calId)}/events";
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var resJson = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(resJson);
                    var createdId = doc.RootElement.TryGetProperty("id", out var idProp) ? idProp.GetString() : null;
                    return (true, "Event created successfully in Google Calendar.", createdId);
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, $"Google API Error ({response.StatusCode}): {err}", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Failed to create event: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> UpdateGoogleEventAsync(string eventId, CalendarEventItem eventItem, GoogleCalendarSettings settings)
        {
            try
            {
                var token = await GetServiceAccountAccessTokenAsync(settings);
                if (string.IsNullOrEmpty(token))
                {
                    return (false, "Failed to obtain Google access token.");
                }

                var calId = string.IsNullOrWhiteSpace(settings.PrimaryGoogleCalendarId) ? "primary" : settings.PrimaryGoogleCalendarId.Trim();
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var payload = BuildGoogleEventJson(eventItem);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var url = $"https://www.googleapis.com/calendar/v3/calendars/{Uri.EscapeDataString(calId)}/events/{Uri.EscapeDataString(eventId)}";
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Event updated successfully in Google Calendar.");
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, $"Google API Error ({response.StatusCode}): {err}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Failed to update event: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteGoogleEventAsync(string eventId, GoogleCalendarSettings settings)
        {
            try
            {
                var token = await GetServiceAccountAccessTokenAsync(settings);
                if (string.IsNullOrEmpty(token))
                {
                    return (false, "Failed to obtain Google access token.");
                }

                var calId = string.IsNullOrWhiteSpace(settings.PrimaryGoogleCalendarId) ? "primary" : settings.PrimaryGoogleCalendarId.Trim();
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = $"https://www.googleapis.com/calendar/v3/calendars/{Uri.EscapeDataString(calId)}/events/{Uri.EscapeDataString(eventId)}";
                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Event deleted successfully from Google Calendar.");
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, $"Google API Error ({response.StatusCode}): {err}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Failed to delete event: {ex.Message}");
            }
        }

        private async Task<string?> GetServiceAccountAccessTokenAsync(GoogleCalendarSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.ServiceAccountEmail) || string.IsNullOrWhiteSpace(settings.ServiceAccountPrivateKey))
            {
                return null;
            }

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var exp = now + 3600; // 1 hour token

            var header = new { alg = "RS256", typ = "JWT" };
            var claimSet = new
            {
                iss = settings.ServiceAccountEmail.Trim(),
                scope = "https://www.googleapis.com/auth/calendar",
                aud = "https://oauth2.googleapis.com/token",
                exp = exp,
                iat = now
            };

            var headerJson = JsonSerializer.Serialize(header);
            var claimsJson = JsonSerializer.Serialize(claimSet);

            var encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            var encodedClaims = Base64UrlEncode(Encoding.UTF8.GetBytes(claimsJson));
            var stringToSign = $"{encodedHeader}.{encodedClaims}";

            var signatureBytes = SignRsaSha256(stringToSign, settings.ServiceAccountPrivateKey);
            var encodedSignature = Base64UrlEncode(signatureBytes);

            var assertion = $"{stringToSign}.{encodedSignature}";

            var client = _httpClientFactory.CreateClient();
            var postData = new Dictionary<string, string>
            {
                { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { "assertion", assertion }
            };

            var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(postData));
            if (!tokenResponse.IsSuccessStatusCode)
            {
                var err = await tokenResponse.Content.ReadAsStringAsync();
                _logger.LogError("Error getting access token from Google: {Error}", err);
                return null;
            }

            var resContent = await tokenResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(resContent);
            return doc.RootElement.TryGetProperty("access_token", out var tok) ? tok.GetString() : null;
        }

        private static byte[] SignRsaSha256(string data, string privateKeyPem)
        {
            var cleanKey = privateKeyPem
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            var keyBytes = Convert.FromBase64String(cleanKey);

            using var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportPkcs8PrivateKey(keyBytes, out _);

            var dataBytes = Encoding.UTF8.GetBytes(data);
            return rsa.SignData(dataBytes, System.Security.Cryptography.HashAlgorithmName.SHA256, System.Security.Cryptography.RSASignaturePadding.Pkcs1);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string BuildGoogleEventJson(CalendarEventItem ev)
        {
            var startDict = ev.IsAllDay 
                ? new Dictionary<string, object> { { "date", ev.Start.ToString("yyyy-MM-dd") } }
                : new Dictionary<string, object> { { "dateTime", ev.Start.ToString("yyyy-MM-ddTHH:mm:ssK") } };

            var endDict = ev.IsAllDay 
                ? new Dictionary<string, object> { { "date", ev.End.ToString("yyyy-MM-dd") } }
                : new Dictionary<string, object> { { "dateTime", ev.End.ToString("yyyy-MM-ddTHH:mm:ssK") } };

            var root = new Dictionary<string, object?>
            {
                { "summary", ev.Title },
                { "description", ev.Description },
                { "location", ev.Location },
                { "start", startDict },
                { "end", endDict }
            };

            return JsonSerializer.Serialize(root);
        }

        private static string EscapeIcsText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace("\\", "\\\\")
                       .Replace(";", "\\;")
                       .Replace(",", "\\,")
                       .Replace("\r\n", "\\n")
                       .Replace("\n", "\\n");
        }
    }
}
