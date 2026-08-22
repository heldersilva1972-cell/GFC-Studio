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
        public string SenderAddress { get; set; } = string.Empty;
        public string SubjectKeyword { get; set; } = "Lottery";
        public string DailyGmailLabel { get; set; } = "INBOX";
        public string WeeklyGmailLabel { get; set; } = "INBOX";
        public bool AutoSyncEnabled { get; set; } = false;
        public bool AutoCommitEnabled { get; set; } = false;
        public bool DownloadWeeklyEnabled { get; set; } = true;
        public bool DownloadDailyEnabled { get; set; } = true;
        public bool IncludeReadEmails { get; set; } = false;
        public int SyncIntervalHours { get; set; } = 6;
        public DateTime? LastSyncTime { get; set; }
        public string? LastSyncStatus { get; set; }
        public int LastSyncFileCount { get; set; } = 0;
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
            var appDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }
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

            var foldersToSearch = new List<string>();
            var dailyFolder = string.IsNullOrWhiteSpace(settings.DailyGmailLabel) ? "INBOX" : settings.DailyGmailLabel.Trim();
            var weeklyFolder = string.IsNullOrWhiteSpace(settings.WeeklyGmailLabel) ? "INBOX" : settings.WeeklyGmailLabel.Trim();

            if (settings.DownloadDailyEnabled)
            {
                foldersToSearch.Add(dailyFolder);
            }

            if (settings.DownloadWeeklyEnabled && !foldersToSearch.Contains(weeklyFolder))
            {
                foldersToSearch.Add(weeklyFolder);
            }

            if (!foldersToSearch.Any())
            {
                progress?.Report("Both Weekly and Daily report downloads are disabled in settings. Skipping email sync.");
                await client.DisconnectAsync(true, cancellationToken);
                return results;
            }

            int folderIdx = 0;
            foreach (var folderName in foldersToSearch)
            {
                cancellationToken.ThrowIfCancellationRequested();
                folderIdx++;
                progress?.Report($"[{folderIdx}/{foldersToSearch.Count}] Opening folder '{folderName}'...");
                
                IMailFolder? folder = null;
                if (!folderName.Equals("INBOX", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        folder = await client.GetFolderAsync(folderName, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        progress?.Report($"Warning: Could not open folder '{folderName}': {ex.Message}. Falling back to INBOX.");
                    }
                }

                if (folder == null)
                {
                    folder = client.Inbox;
                }

                await folder.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

                bool isDedicatedLabel = !folderName.Equals("INBOX", StringComparison.OrdinalIgnoreCase);
                
                // For dedicated Gmail labels or if IncludeReadEmails is enabled, scan ALL messages (read + unread)
                SearchQuery query = (isDedicatedLabel || settings.IncludeReadEmails) ? SearchQuery.All : SearchQuery.NotSeen;

                // Only filter by sender address if scanning generic INBOX and sender address is explicitly provided
                if (!isDedicatedLabel && !string.IsNullOrWhiteSpace(settings.SenderAddress) && settings.SenderAddress != "reports@lottery.com")
                {
                    query = query.And(SearchQuery.FromContains(settings.SenderAddress.Trim()));
                }

                if (!string.IsNullOrEmpty(settings.SubjectKeyword) && !isDedicatedLabel)
                {
                    query = query.And(SearchQuery.SubjectContains(settings.SubjectKeyword));
                }

                progress?.Report($"Searching '{folderName}' for matching messages...");
                var matchedUniqueIds = await folder.SearchAsync(query, cancellationToken);

                int totalEmails = matchedUniqueIds.Count;
                if (totalEmails == 0)
                {
                    progress?.Report($"No matching emails in '{folderName}'.");
                    continue;
                }

                progress?.Report($"Found {totalEmails} email(s) in '{folderName}'. Scanning attachments...");
                int totalCsvFiles = 0;
                var emailFilesList = new List<(UniqueId Uid, List<MimePart> CsvParts)>();

                foreach (var uid in matchedUniqueIds)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var message = await folder.GetMessageAsync(uid, cancellationToken);

                    // Scan ALL MIME body parts (attachments, inline parts, nested multiparts)
                    var reportParts = message.BodyParts
                        .OfType<MimePart>()
                        .Where(p => {
                            var fileName = p.FileName;
                            if (string.IsNullOrEmpty(fileName) && p.ContentType != null)
                            {
                                fileName = p.ContentType.Name;
                            }
                            if (string.IsNullOrEmpty(fileName)) return false;
                            
                            var ext = Path.GetExtension(fileName).ToLowerInvariant();
                            return ext == ".csv" || ext == ".txt" || ext == ".tsv" || ext == ".dat";
                        })
                        .ToList();

                    if (reportParts.Count > 0)
                    {
                        totalCsvFiles += reportParts.Count;
                        emailFilesList.Add((uid, reportParts));
                    }
                }

                if (totalCsvFiles == 0)
                {
                    progress?.Report($"No CSV/TXT report attachments found in matching emails of '{folderName}'.");
                    continue;
                }

                progress?.Report($"Found {totalCsvFiles} report file(s) in '{folderName}'. Downloading...");

                int currentFileIdx = 0;
                foreach (var item in emailFilesList)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var mimePart in item.CsvParts)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        currentFileIdx++;
                        
                        var fileName = mimePart.FileName;
                        if (string.IsNullOrEmpty(fileName) && mimePart.ContentType != null)
                        {
                            fileName = mimePart.ContentType.Name;
                        }
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = $"report_{currentFileIdx}.csv";
                        }

                        progress?.Report($"Downloading {currentFileIdx}/{totalCsvFiles} ({fileName}) from '{folderName}'...");
                        
                        using var memoryStream = new MemoryStream();
                        await mimePart.Content.DecodeToAsync(memoryStream, cancellationToken);
                        memoryStream.Position = 0;

                        progress?.Report($"Extracting {currentFileIdx}/{totalCsvFiles} ({fileName})...");
                        using var reader = new StreamReader(memoryStream);
                        var content = await reader.ReadToEndAsync(cancellationToken);

                        bool isWeekly = content.Contains("Gross Sales") || content.Contains("Return Sales") || content.Contains("Commission") || content.Contains("TOTAL DUE") || fileName.ToLowerInvariant().Contains("weekly");
                        
                        if (isWeekly && !settings.DownloadWeeklyEnabled)
                        {
                            progress?.Report($"Skipping weekly statement {fileName} (Weekly statement download disabled in settings).");
                            continue;
                        }
                        if (!isWeekly && !settings.DownloadDailyEnabled)
                        {
                            progress?.Report($"Skipping daily shift report {fileName} (Daily report download disabled in settings).");
                            continue;
                        }

                        results.Add(new EmailAttachmentFile
                        {
                            FileName = fileName,
                            FileSize = memoryStream.Length,
                            Content = content
                        });
                    }

                    // Mark email as read/seen so it is skipped next time
                    await folder.AddFlagsAsync(item.Uid, MessageFlags.Seen, true, cancellationToken);
                }
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
