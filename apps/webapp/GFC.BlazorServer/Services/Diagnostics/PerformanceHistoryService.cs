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
    public interface IPerformanceHistoryService
    {
        Task AddPerformanceSnapshotAsync(PerformanceMetrics metrics, CancellationToken cancellationToken = default);
        Task<List<PerformanceSnapshot>> GetPerformanceSnapshotsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task PurgeOldSnapshotsAsync(int retentionDays = 7, CancellationToken cancellationToken = default);
    }

    public class PerformanceHistoryService : IPerformanceHistoryService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;

        public PerformanceHistoryService(IDbContextFactory<GfcDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddPerformanceSnapshotAsync(PerformanceMetrics metrics, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var snapshot = new PerformanceSnapshot
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                CpuUsage = metrics.CpuUsage,
                MemoryUsageGb = metrics.MemoryUsageGb,
                MemoryUsagePercentage = metrics.MemoryUsagePercentage,
                ActiveThreads = metrics.ActiveThreads,
                ActiveConnections = metrics.ActiveConnections,
                RequestsPerMinute = metrics.RequestsPerMinute,
                Gen0Collections = metrics.Gen0Collections,
                Gen1Collections = metrics.Gen1Collections,
                Gen2Collections = metrics.Gen2Collections
            };

            await dbContext.PerformanceSnapshots.AddAsync(snapshot, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<PerformanceSnapshot>> GetPerformanceSnapshotsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.PerformanceSnapshots
                .Where(s => s.Timestamp >= startDate && s.Timestamp <= endDate)
                .OrderBy(s => s.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task PurgeOldSnapshotsAsync(int retentionDays = 7, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
            var oldSnapshots = await dbContext.PerformanceSnapshots.Where(s => s.Timestamp < cutoffDate).ToListAsync(cancellationToken);
            if(oldSnapshots.Any())
            {
                dbContext.PerformanceSnapshots.RemoveRange(oldSnapshots);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
