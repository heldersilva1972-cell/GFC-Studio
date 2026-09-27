using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class ResendInboundWebhookController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IWebsiteSettingsService _settingsService;
        private readonly IRentalEmailDispatcher _emailDispatcher;
        private readonly ILogger<ResendInboundWebhookController> _logger;

        public ResendInboundWebhookController(
            IRentalService rentalService,
            IWebsiteSettingsService settingsService,
            IRentalEmailDispatcher emailDispatcher,
            ILogger<ResendInboundWebhookController> logger)
        {
            _rentalService = rentalService;
            _settingsService = settingsService;
            _emailDispatcher = emailDispatcher;
            _logger = logger;
        }

        public class ResendInboundPayload
        {
            [JsonPropertyName("type")]
            public string? Type { get; set; }

            [JsonPropertyName("created_at")]
            public string? CreatedAt { get; set; }

            [JsonPropertyName("data")]
            public ResendInboundData? Data { get; set; }
        }

        public class ResendInboundData
        {
            [JsonPropertyName("from")]
            public string? From { get; set; }

            [JsonPropertyName("to")]
            public object? To { get; set; }

            [JsonPropertyName("subject")]
            public string? Subject { get; set; }

            [JsonPropertyName("text")]
            public string? Text { get; set; }

            [JsonPropertyName("html")]
            public string? Html { get; set; }

            [JsonPropertyName("message_id")]
            public string? MessageId { get; set; }
        }

        [HttpPost("resend-inbound")]
        public async Task<IActionResult> HandleResendInbound()
        {
            var settings = await _settingsService.GetWebsiteSettingsAsync() ?? new WebsiteSettings();

            if (!settings.EnableRentalInboundWebhook)
            {
                _logger.LogInformation("[ResendInbound] Inbound webhook is currently disabled in WebsiteSettings.");
                return Ok(new { status = "ignored", reason = "inbound_disabled" });
            }

            // Optional Secret / Signature Verification
            if (!string.IsNullOrWhiteSpace(settings.RentalInboundWebhookSecret))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault() ?? 
                                 Request.Headers["X-Resend-Signature"].FirstOrDefault() ?? 
                                 Request.Headers["X-Webhook-Secret"].FirstOrDefault() ?? 
                                 Request.Query["secret"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(authHeader))
                {
                    var cleanToken = authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
                    if (!string.Equals(cleanToken, settings.RentalInboundWebhookSecret, StringComparison.Ordinal))
                    {
                        _logger.LogWarning("[ResendInbound] Secret token mismatch on inbound webhook request.");
                        return Unauthorized(new { status = "unauthorized", message = "Invalid webhook secret." });
                    }
                }
            }

            using var reader = new StreamReader(Request.Body);
            var bodyText = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(bodyText))
            {
                return BadRequest(new { status = "error", message = "Empty body received." });
            }

            try
            {
                var payload = JsonSerializer.Deserialize<ResendInboundPayload>(bodyText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var emailData = payload?.Data;
                if (emailData == null)
                {
                    // Direct email payload fallback
                    emailData = JsonSerializer.Deserialize<ResendInboundData>(bodyText, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                if (emailData == null)
                {
                    return BadRequest(new { status = "error", message = "Could not parse inbound email data." });
                }

                string subject = emailData.Subject ?? string.Empty;
                string sender = emailData.From ?? string.Empty;
                string messageBody = !string.IsNullOrWhiteSpace(emailData.Text) 
                    ? emailData.Text 
                    : (StripHtmlTags(emailData.Html) ?? string.Empty);

                _logger.LogInformation("[ResendInbound] Received inbound email. From: {Sender} | Subject: {Subject}", sender, subject);

                // Extract tracking token: [GFC-INQ-123] or [GFC-RENT-123] or [#123]
                int matchedRequestId = 0;
                var inqMatch = Regex.Match(subject, @"\[GFC-INQ-(\d+)\]", RegexOptions.IgnoreCase);
                var rentMatch = Regex.Match(subject, @"\[GFC-RENT-(\d+)\]", RegexOptions.IgnoreCase);
                var refMatch = Regex.Match(subject, @"Inquiry Reference #(\d+)", RegexOptions.IgnoreCase);

                if (inqMatch.Success && int.TryParse(inqMatch.Groups[1].Value, out var id1))
                {
                    matchedRequestId = id1;
                }
                else if (rentMatch.Success && int.TryParse(rentMatch.Groups[1].Value, out var id2))
                {
                    matchedRequestId = id2;
                }
                else if (refMatch.Success && int.TryParse(refMatch.Groups[1].Value, out var id3))
                {
                    matchedRequestId = id3;
                }

                var allRequests = await _rentalService.GetRentalRequestsAsync();
                HallRentalRequest? targetRequest = null;

                if (matchedRequestId > 0)
                {
                    targetRequest = allRequests.FirstOrDefault(r => r.Id == matchedRequestId);
                }

                // If not matched by tracking code, match by applicant's email
                if (targetRequest == null && !string.IsNullOrWhiteSpace(sender))
                {
                    var senderEmailClean = ExtractEmailAddress(sender);
                    targetRequest = allRequests
                        .Where(r => !string.IsNullOrWhiteSpace(r.RequesterEmail) && 
                                    string.Equals(r.RequesterEmail.Trim(), senderEmailClean, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(r => r.CreatedDate)
                        .FirstOrDefault();
                }

                if (targetRequest != null)
                {
                    // Clean quoted text from email reply if possible
                    string cleanedReply = CleanQuotedEmailReply(messageBody);

                    var noteBuilder = new System.Text.StringBuilder();
                    noteBuilder.AppendLine($"[{DateTime.Now:yyyy-MM-dd h:mm tt}]");
                    noteBuilder.AppendLine($"📥 Applicant Email Reply Received from {sender}");
                    noteBuilder.AppendLine($"• Subject: {subject}");
                    noteBuilder.AppendLine($"• Message:");
                    noteBuilder.AppendLine(cleanedReply);
                    noteBuilder.AppendLine();

                    targetRequest.InternalNotes = (noteBuilder.ToString() + (targetRequest.InternalNotes ?? "")).Trim();
                    
                    // Update status to Applicant Replied if currently in inquiry/responded state
                    if (string.Equals(targetRequest.Status, "Responded", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(targetRequest.Status, "Inquiry", StringComparison.OrdinalIgnoreCase))
                    {
                        targetRequest.Status = "Inquiry";
                        targetRequest.StatusChangedBy = "Inbound Email Webhook";
                        targetRequest.StatusChangedDate = DateTime.UtcNow;
                    }

                    await _rentalService.UpdateRentalRequestAsync(targetRequest);
                    _logger.LogInformation("[ResendInbound] Attached reply to Hall Rental Request #{Id} ({Applicant})", targetRequest.Id, targetRequest.ApplicantName);

                    // Notify staff recipients if enabled
                    if (settings.RentalInboundNotifyStaff && !settings.MasterEmailKillSwitch)
                    {
                        var staffList = settings.GetRecipientsList();
                        if (staffList.Any())
                        {
                            string staffSubject = $"[GFC Rental Reply] New message from {targetRequest.ApplicantName} for {targetRequest.EventDate:MM/dd/yyyy}";
                            string staffBody = $@"
                                <div style='font-family: Arial, sans-serif; font-size: 15px; color: #1e293b; line-height: 1.5;'>
                                    <div style='background: #0284c7; color: white; padding: 12px 16px; border-radius: 6px 6px 0 0;'>
                                        <h3 style='margin: 0;'>New Applicant Reply Received</h3>
                                        <p style='margin: 4px 0 0 0; font-size: 13px;'>Applicant: <strong>{targetRequest.ApplicantName}</strong> (Ref #{targetRequest.Id})</p>
                                    </div>
                                    <div style='padding: 16px; border: 1px solid #e2e8f0; border-top: none; border-radius: 0 0 6px 6px; background: #ffffff;'>
                                        <p><strong>From:</strong> {sender}</p>
                                        <p><strong>Event Date:</strong> {targetRequest.EventDate:MMMM dd, yyyy}</p>
                                        <div style='background: #f8fafc; border-left: 4px solid #0284c7; padding: 12px; margin: 12px 0; white-space: pre-wrap;'>{System.Net.WebUtility.HtmlEncode(cleanedReply)}</div>
                                        <p style='color: #64748b; font-size: 13px;'>This reply has been automatically logged inside the GFC Hall Rentals module.</p>
                                    </div>
                                </div>";

                            foreach (var staffEmail in staffList)
                            {
                                await _emailDispatcher.SendRentalEmailAsync(settings, staffEmail, staffSubject, staffBody);
                            }
                        }
                    }

                    return Ok(new { status = "success", matchedRequestId = targetRequest.Id });
                }

                _logger.LogWarning("[ResendInbound] Inbound email did not match any active rental request or inquiry.");
                return Ok(new { status = "unmatched", subject = subject });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ResendInbound] Error processing inbound email webhook.");
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        private static string ExtractEmailAddress(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
            var match = Regex.Match(raw, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
            return match.Success ? match.Value.Trim() : raw.Trim();
        }

        private static string StripHtmlTags(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;
            var plain = Regex.Replace(html, "<.*?>", string.Empty);
            return System.Net.WebUtility.HtmlDecode(plain).Trim();
        }

        private static string CleanQuotedEmailReply(string body)
        {
            if (string.IsNullOrWhiteSpace(body)) return string.Empty;

            var lines = body.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var cleanLines = new System.Collections.Generic.List<string>();

            foreach (var line in lines)
            {
                // Stop at standard email quote headers (e.g., "On Jan 1, 2026, at 2:00 PM, ... wrote:" or "-----Original Message-----")
                if (line.StartsWith("On ", StringComparison.OrdinalIgnoreCase) && line.Contains("wrote:", StringComparison.OrdinalIgnoreCase))
                    break;
                if (line.StartsWith("-----Original Message-----", StringComparison.OrdinalIgnoreCase))
                    break;
                if (line.StartsWith(">"))
                    continue;

                cleanLines.Add(line);
            }

            var result = string.Join("\n", cleanLines).Trim();
            return string.IsNullOrWhiteSpace(result) ? body.Trim() : result;
        }
    }
}
