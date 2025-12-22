// [NEW]
using GFC.Core.Models.Diagnostics;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Diagnostics
{
    public interface ISystemPerformanceService
    {
        Task<PerformanceMetrics> GetPerformanceMetricsAsync(CancellationToken cancellationToken = default);
        void IncrementRequestCount();
    }

    public class SystemPerformanceService : ISystemPerformanceService
    {
        private readonly Process _currentProcess;
        private readonly IDatabaseHealthService _dbHealthService;
        private DateTime _lastCpuSampleTime;
        private TimeSpan _lastTotalProcessorTime;
        private int _requestsSinceLastSample;
        private DateTime _lastRequestSampleTime;
        private int _requestsPerMinute;

        public SystemPerformanceService(IDatabaseHealthService dbHealthService)
        {
            _currentProcess = Process.GetCurrentProcess();
            _dbHealthService = dbHealthService;
            _lastCpuSampleTime = DateTime.MinValue;
            _lastTotalProcessorTime = TimeSpan.Zero;
            _requestsSinceLastSample = 0;
            _lastRequestSampleTime = DateTime.UtcNow;
            _requestsPerMinute = 0;
        }

        public async Task<PerformanceMetrics> GetPerformanceMetricsAsync(CancellationToken cancellationToken = default)
        {
            await Task.Yield(); // Ensure we don't block the calling thread for long

            var metrics = new PerformanceMetrics();

            // CPU Usage
            if (_lastCpuSampleTime == DateTime.MinValue)
            {
                _lastCpuSampleTime = DateTime.UtcNow;
                _lastTotalProcessorTime = _currentProcess.TotalProcessorTime;
                metrics.CpuUsage = 0;
            }
            else
            {
                var currentTime = DateTime.UtcNow;
                var currentTotalProcessorTime = _currentProcess.TotalProcessorTime;
                var cpuUsedMs = (currentTotalProcessorTime - _lastTotalProcessorTime).TotalMilliseconds;
                var totalTimeMs = (currentTime - _lastCpuSampleTime).TotalMilliseconds;
                var cpuUsage = (cpuUsedMs / (totalTimeMs * Environment.ProcessorCount)) * 100;

                metrics.CpuUsage = Math.Round(cpuUsage, 2);

                _lastCpuSampleTime = currentTime;
                _lastTotalProcessorTime = currentTotalProcessorTime;
            }

            // Memory Usage
            _currentProcess.Refresh();
            var workingSet = _currentProcess.WorkingSet64;
            metrics.MemoryUsageGb = Math.Round(workingSet / (1024.0 * 1024.0 * 1024.0), 2);

            var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
            if (totalMemory > 0)
            {
                metrics.MemoryUsagePercentage = Math.Round((double)workingSet / totalMemory * 100, 2);
            }
            else
            {
                metrics.MemoryUsagePercentage = 0; // Avoid division by zero
            }

            // Threads and Connections
            metrics.ActiveThreads = _currentProcess.Threads.Count;
            var dbHealth = await _dbHealthService.GetDatabaseHealthAsync(cancellationToken);
            metrics.ActiveConnections = dbHealth.ActiveConnections;

            // Requests per minute
            CalculateRequestsPerMinute();
            metrics.RequestsPerMinute = _requestsPerMinute;

            // Garbage Collection
            metrics.Gen0Collections = GC.CollectionCount(0);
            metrics.Gen1Collections = GC.CollectionCount(1);
            metrics.Gen2Collections = GC.CollectionCount(2);

            return metrics;
        }

        public void IncrementRequestCount()
        {
            Interlocked.Increment(ref _requestsSinceLastSample);
        }

        private void CalculateRequestsPerMinute()
        {
            var now = DateTime.UtcNow;
            var timeSinceLastSample = now - _lastRequestSampleTime;

            if (timeSinceLastSample.TotalSeconds >= 60)
            {
                var requests = Interlocked.Exchange(ref _requestsSinceLastSample, 0);
                _requestsPerMinute = (int)(requests / timeSinceLastSample.TotalMinutes);
                _lastRequestSampleTime = now;
            }
        }
    }
}
