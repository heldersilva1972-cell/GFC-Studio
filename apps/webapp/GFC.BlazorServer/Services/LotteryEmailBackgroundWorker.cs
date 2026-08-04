using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            }
            else
            {
                _logger.LogInformation("Lottery Email Auto-Sync check completed. No new reports found.");
            }

            // Update last sync time
            settings.LastSyncTime = DateTime.Now;
            emailService.SaveSettings(settings);
        }
    }
}
