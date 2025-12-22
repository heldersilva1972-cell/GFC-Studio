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
        private readonly GfcDbContext _dbContext;

        public PerformanceHistoryService(GfcDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPerformanceSnapshotAsync(PerformanceMetrics metrics, CancellationToken cancellationToken = default)
        {

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

            await _dbContext.PerformanceSnapshots.AddAsync(snapshot, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<PerformanceSnapshot>> GetPerformanceSnapshotsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PerformanceSnapshots
                .Where(s => s.Timestamp >= startDate && s.Timestamp <= endDate)
                .OrderBy(s => s.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task PurgeOldSnapshotsAsync(int retentionDays = 7, CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
            var oldSnapshots = await _dbContext.PerformanceSnapshots.Where(s => s.Timestamp < cutoffDate).ToListAsync(cancellationToken);
            if(oldSnapshots.Any())
            {
                _dbContext.PerformanceSnapshots.RemoveRange(oldSnapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
