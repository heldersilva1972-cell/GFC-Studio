using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace GFC.BlazorServer.Services
{
    public class LotteryEmailSettings
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;
        public string SenderAddress { get; set; } = "reports@lottery.com";
        public string SubjectKeyword { get; set; } = "Lottery";
        public bool AutoSyncEnabled { get; set; } = false;
        public int SyncIntervalHours { get; set; } = 6;
        public DateTime? LastSyncTime { get; set; }
    }

    public class LotteryEmailService
    {
        private readonly string _settingsFilePath;

        public LotteryEmailService()
        {
            // Save in App_Data/lottery-email-settings.json within Blazor app root
            var appDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }
            _settingsFilePath = Path.Combine(appDataDir, "lottery-email-settings.json");
        }

        public LotteryEmailSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var json = File.ReadAllText(_settingsFilePath);
                    return JsonSerializer.Deserialize<LotteryEmailSettings>(json) ?? new LotteryEmailSettings();
                }
            }
            catch
            {
                // Fallback to empty defaults
            }
            return new LotteryEmailSettings();
        }

        public void SaveSettings(LotteryEmailSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsFilePath, json);
        }

        public async Task<List<EmailAttachmentFile>> FetchLotteryReportsAsync(
            LotteryEmailSettings settings, 
            IProgress<string>? progress = null, 
            CancellationToken cancellationToken = default)
        {
            var results = new List<EmailAttachmentFile>();

            if (string.IsNullOrEmpty(settings.EmailAddress) || string.IsNullOrEmpty(settings.AppPassword))
            {
                throw new ArgumentException("Email Address and App Password must be configured.");
            }

            using var client = new ImapClient();
            
            progress?.Report("Connecting to Gmail secure IMAP (imap.gmail.com)...");
            await client.ConnectAsync("imap.gmail.com", 993, true, cancellationToken);
            
            progress?.Report("Authenticating credentials...");
            await client.AuthenticateAsync(settings.EmailAddress, settings.AppPassword, cancellationToken);

            progress?.Report("Opening inbox...");
            await client.Inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

            // Query for unread emails only
            var query = SearchQuery.NotSeen;

            if (!string.IsNullOrEmpty(settings.SenderAddress))
            {
                query = query.And(SearchQuery.FromContains(settings.SenderAddress));
            }

            if (!string.IsNullOrEmpty(settings.SubjectKeyword))
            {
                query = query.And(SearchQuery.SubjectContains(settings.SubjectKeyword));
            }

            progress?.Report("Searching inbox for new matching unread messages...");
            var matchedUniqueIds = await client.Inbox.SearchAsync(query, cancellationToken);

            int totalEmails = matchedUniqueIds.Count;
            if (totalEmails == 0)
            {
                progress?.Report("No new matching emails found.");
                await client.DisconnectAsync(true, cancellationToken);
                return results;
            }

            progress?.Report("Scanning matching emails to count report files...");
            int totalCsvFiles = 0;
            var emailFilesList = new List<(UniqueId Uid, List<MimePart> CsvParts)>();

            foreach (var uid in matchedUniqueIds)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var message = await client.Inbox.GetMessageAsync(uid, cancellationToken);
                var csvParts = message.Attachments
                    .OfType<MimePart>()
                    .Where(a => a.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (csvParts.Count > 0)
                {
                    totalCsvFiles += csvParts.Count;
                    emailFilesList.Add((uid, csvParts));
                }
            }

            if (totalCsvFiles == 0)
            {
                progress?.Report("No lottery CSV attachments found in matching emails.");
                await client.DisconnectAsync(true, cancellationToken);
                return results;
            }

            progress?.Report($"Found {totalCsvFiles} CSV file(s). Starting download...");

            int currentFileIdx = 0;
            foreach (var item in emailFilesList)
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var mimePart in item.CsvParts)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    currentFileIdx++;
                    
                    progress?.Report($"Downloading file {currentFileIdx} of {totalCsvFiles} ({mimePart.FileName})...");
                    
                    using var memoryStream = new MemoryStream();
                    await mimePart.Content.DecodeToAsync(memoryStream, cancellationToken);
                    memoryStream.Position = 0;

                    progress?.Report($"Extracting file {currentFileIdx} of {totalCsvFiles} ({mimePart.FileName})...");
                    using var reader = new StreamReader(memoryStream);
                    var content = await reader.ReadToEndAsync(cancellationToken);

                    results.Add(new EmailAttachmentFile
                    {
                        FileName = mimePart.FileName,
                        FileSize = memoryStream.Length,
                        Content = content
                    });
                }

                // Mark email as read/seen so it is skipped next time
                await client.Inbox.AddFlagsAsync(item.Uid, MessageFlags.Seen, true, cancellationToken);
            }

            progress?.Report("Sync completed. Closing secure connection...");
            await client.DisconnectAsync(true, cancellationToken);
            return results;
        }
    }

    public class EmailAttachmentFile
    {
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
