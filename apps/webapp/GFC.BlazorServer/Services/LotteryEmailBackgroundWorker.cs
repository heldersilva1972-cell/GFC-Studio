using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;

namespace GFC.BlazorServer.Services
{
    public class LotteryEmailBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LotteryEmailBackgroundWorker> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(10); // Check settings/schedule every 10 minutes

        public LotteryEmailBackgroundWorker(IServiceProvider serviceProvider, ILogger<LotteryEmailBackgroundWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Lottery Email Background Auto-Sync Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PerformSyncIfDueAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during background lottery reports sync.");
                    try
                    {
                        using var errScope = _serviceProvider.CreateScope();
                        var emailSvc = errScope.ServiceProvider.GetRequiredService<LotteryEmailService>();
                        var st = emailSvc.LoadSettings();
                        st.LastSyncTime = DateTime.Now;
                        st.LastSyncStatus = $"Error: {ex.Message}";
                        emailSvc.SaveSettings(st);
                    }
                    catch { }
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Lottery Email Background Auto-Sync Worker stopped.");
        }

        private async Task PerformSyncIfDueAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<LotteryEmailService>();

            var settings = emailService.LoadSettings();

            if (!settings.AutoSyncEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(settings.EmailAddress) || string.IsNullOrEmpty(settings.AppPassword))
            {
                _logger.LogWarning("Lottery auto-sync is enabled but Gmail credentials are not fully configured.");
                return;
            }

            // Check if interval time has elapsed
            bool isDue = !settings.LastSyncTime.HasValue || 
                         (DateTime.Now - settings.LastSyncTime.Value) >= TimeSpan.FromHours(settings.SyncIntervalHours);

            if (!isDue)
            {
                return;
            }

            _logger.LogInformation("Lottery Email Auto-Sync is running. Connecting to Gmail for {EmailAddress}...", settings.EmailAddress);

            var fetchedFiles = await emailService.FetchLotteryReportsAsync(settings);

            if (fetchedFiles != null && fetchedFiles.Any())
            {
                _logger.LogInformation("Lottery Email Auto-Sync found {Count} new reports. Staging reports locally...", fetchedFiles.Count);

                var stagedDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "StagedReports");
                if (!Directory.Exists(stagedDir))
                {
                    Directory.CreateDirectory(stagedDir);
                }

                foreach (var file in fetchedFiles)
                {
                    var filePath = Path.Combine(stagedDir, file.FileName);
                    await File.WriteAllTextAsync(filePath, file.Content, stoppingToken);
                    _logger.LogInformation("Staged report saved to: {FilePath}", filePath);
                }

                if (settings.AutoCommitEnabled)
                {
                    _logger.LogInformation("Auto-Commit is enabled. Automatically committing staged reports to SQL Server...");
                    try
                    {
                        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<GfcDbContext>>();
                        using var db = await dbFactory.CreateDbContextAsync(stoppingToken);

                        foreach (var file in fetchedFiles)
                        {
                            bool isWeekly = file.Content.Contains("Gross Sales") || file.Content.Contains("Return Sales") || file.Content.Contains("Commission") || file.Content.Contains("TOTAL DUE");
                            if (isWeekly)
                            {
                                var weeklyStat = ParseWeeklyContent(file.FileName, file.Content);
                                var existing = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.LotteryWeeklyStats, w => w.WeekEndingDate.Date == weeklyStat.WeekEndingDate.Date, stoppingToken);
                                if (existing != null)
                                {
                                    existing.OnlineNetSales = weeklyStat.OnlineNetSales;
                                    existing.OnlineCommission = weeklyStat.OnlineCommission;
                                    existing.OnlineCashes = weeklyStat.OnlineCashes;
                                    existing.OnlineCashBonus = weeklyStat.OnlineCashBonus;
                                    existing.OnlineClaimsBonus = weeklyStat.OnlineClaimsBonus;
                                    existing.OnlineAdjustments = weeklyStat.OnlineAdjustments;
                                    existing.OnlineServiceFee = weeklyStat.OnlineServiceFee;
                                    existing.OnlineBondingFee = weeklyStat.OnlineBondingFee;
                                    existing.OnlineDue = weeklyStat.OnlineDue;
                                    existing.InstantGrossSales = weeklyStat.InstantGrossSales;
                                    existing.InstantReturnSales = weeklyStat.InstantReturnSales;
                                    existing.InstantCommission = weeklyStat.InstantCommission;
                                    existing.InstantCashes = weeklyStat.InstantCashes;
                                    existing.InstantCashBonus = weeklyStat.InstantCashBonus;
                                    existing.InstantClaimsBonus = weeklyStat.InstantClaimsBonus;
                                    existing.InstantAdjustments = weeklyStat.InstantAdjustments;
                                    existing.InstantDue = weeklyStat.InstantDue;
                                    existing.TotalDue = weeklyStat.TotalDue;
                                    existing.CreatedBy = "Auto-Sync Worker";
                                    existing.CreatedDate = DateTime.Now;
                                    db.LotteryWeeklyStats.Update(existing);
                                }
                                else
                                {
                                    weeklyStat.CreatedBy = "Auto-Sync Worker";
                                    weeklyStat.CreatedDate = DateTime.Now;
                                    await db.LotteryWeeklyStats.AddAsync(weeklyStat, stoppingToken);
                                }
                                await db.SaveChangesAsync(stoppingToken);

                                // Clean up file from staging
                                var filePath = Path.Combine(stagedDir, file.FileName);
                                if (File.Exists(filePath)) File.Delete(filePath);
                            }
                        }
                        _logger.LogInformation("Auto-Commit background processing complete.");
                    }
                    catch (Exception commitEx)
                    {
                        _logger.LogError(commitEx, "Failed to auto-commit reports in background worker.");
                    }
                }
            }

            int fileCount = fetchedFiles?.Count ?? 0;
            settings.LastSyncTime = DateTime.Now;
            settings.LastSyncFileCount = fileCount;
            settings.LastSyncStatus = fileCount > 0 
                ? $"Success: Imported {fileCount} report(s) at {DateTime.Now:h:mm tt}" 
                : $"Success: Checked at {DateTime.Now:h:mm tt} (No new emails)";

            emailService.SaveSettings(settings);
        }

        private GFC.Core.Models.LotteryWeeklyStat ParseWeeklyContent(string fileName, string csvContent)
        {
            var stat = new GFC.Core.Models.LotteryWeeklyStat();
            var lines = csvContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                var cols = lines[i].Split(',');
                if (cols.Length == 0) continue;
                var firstCol = cols[0].Trim().ToUpperInvariant();
                if (firstCol == "INSTANT" && i + 1 < lines.Length)
                {
                    var dataCols = lines[i + 1].Split(',');
                    if (dataCols.Length > 1) stat.InstantGrossSales = ParseCurrency(dataCols[1]);
                    if (dataCols.Length > 2) stat.InstantReturnSales = ParseCurrency(dataCols[2]);
                    if (dataCols.Length > 3) stat.InstantCommission = ParseCurrency(dataCols[3]);
                    if (dataCols.Length > 4) stat.InstantCashes = ParseCurrency(dataCols[4]);
                    if (dataCols.Length > 10) stat.InstantDue = ParseCurrency(dataCols[10]);
                }
                else if (firstCol == "ONLINE" && i + 1 < lines.Length)
                {
                    var dataCols = lines[i + 1].Split(',');
                    if (dataCols.Length > 1) stat.OnlineNetSales = ParseCurrency(dataCols[1]);
                    if (dataCols.Length > 3) stat.OnlineCommission = ParseCurrency(dataCols[3]);
                    if (dataCols.Length > 4) stat.OnlineCashes = ParseCurrency(dataCols[4]);
                    if (dataCols.Length > 10) stat.OnlineDue = ParseCurrency(dataCols[10]);
                }
                else if (lines[i].Contains("TOTAL DUE:"))
                {
                    if (cols.Length > 10) stat.TotalDue = ParseCurrency(cols[10]);
                }
            }
            if (stat.TotalDue == 0) stat.TotalDue = stat.OnlineDue + stat.InstantDue;
            stat.WeekEndingDate = DateTime.Now;
            return stat;
        }

        private decimal ParseCurrency(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            var clean = text.Replace("$", "").Replace(",", "").Trim();
            decimal.TryParse(clean, out var result);
            return result;
        }
    }
}
