// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Diagnostics
{
    public interface IAlertManagementService
    {
        Task<List<AlertThreshold>> GetAlertThresholdsAsync(CancellationToken cancellationToken = default);
        Task UpdateAlertThresholdAsync(AlertThreshold threshold, CancellationToken cancellationToken = default);
        Task<List<DiagnosticAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);
        Task AcknowledgeAlertAsync(Guid alertId, string acknowledgedBy, CancellationToken cancellationToken = default);
        Task CheckAlertsAsync(PerformanceMetrics metrics, CancellationToken cancellationToken = default);
    }

    public class AlertManagementService : IAlertManagementService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;

        public AlertManagementService(IDbContextFactory<GfcDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<AlertThreshold>> GetAlertThresholdsAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.AlertThresholds.ToListAsync(cancellationToken);
        }

        public async Task UpdateAlertThresholdAsync(AlertThreshold threshold, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            dbContext.AlertThresholds.Update(threshold);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<DiagnosticAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.DiagnosticAlerts
                .Include(a => a.AlertThreshold)
                .Where(a => !a.IsAcknowledged)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task AcknowledgeAlertAsync(Guid alertId, string acknowledgedBy, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var alert = await dbContext.DiagnosticAlerts.FindAsync(new object[] { alertId }, cancellationToken);
            if (alert != null)
            {
                alert.IsAcknowledged = true;
                alert.AcknowledgedAt = DateTime.UtcNow;
                alert.AcknowledgedBy = acknowledgedBy;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task CheckAlertsAsync(PerformanceMetrics metrics, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var thresholds = await dbContext.AlertThresholds.Where(t => t.IsEnabled).ToListAsync(cancellationToken);

            foreach (var threshold in thresholds)
            {
                var metricValue = GetMetricValue(metrics, threshold.MetricType);
                if (metricValue >= threshold.ThresholdValue)
                {
                    var lastAlert = await dbContext.DiagnosticAlerts
                        .Where(a => a.AlertThresholdId == threshold.Id)
                        .OrderByDescending(a => a.Timestamp)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (lastAlert == null || lastAlert.Timestamp.AddMinutes(threshold.CooldownMinutes) < DateTime.UtcNow)
                    {
                        var newAlert = new DiagnosticAlert
                        {
                            Id = Guid.NewGuid(),
                            AlertThresholdId = threshold.Id,
                            Timestamp = DateTime.UtcNow,
                            TriggerValue = metricValue
                        };
                        await dbContext.DiagnosticAlerts.AddAsync(newAlert, cancellationToken);
                    }
                }
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private double GetMetricValue(PerformanceMetrics metrics, MetricType metricType)
        {
            return metricType switch
            {
                MetricType.CpuUsage => metrics.CpuUsage,
                MetricType.MemoryUsagePercentage => metrics.MemoryUsagePercentage,
                MetricType.ActiveConnections => metrics.ActiveConnections,
                MetricType.RequestsPerMinute => metrics.RequestsPerMinute,
                _ => 0
            };
        }
    }
}
