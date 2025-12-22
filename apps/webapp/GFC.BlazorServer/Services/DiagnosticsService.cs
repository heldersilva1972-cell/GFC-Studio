// [MODIFIED]
using GFC.Core.Models;
using GFC.Core.Models.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GFC.BlazorServer.Services.Diagnostics;
using MongoDB.Driver;

namespace GFC.BlazorServer.Services
{
    public interface IDiagnosticsService
    {
        Task<SystemDiagnosticsInfo> GetDiagnosticsAsync();
    }

    public class DiagnosticsService : IDiagnosticsService
    {
        private readonly ISystemPerformanceService _performanceService;
        private readonly IDatabaseHealthService _databaseHealthService;
        private readonly IConfiguration _configuration;

        public DiagnosticsService(
            ISystemPerformanceService performanceService,
            IDatabaseHealthService databaseHealthService,
            IConfiguration configuration)
        {
            _performanceService = performanceService;
            _databaseHealthService = databaseHealthService;
            _configuration = configuration;
        }

        public async Task<SystemDiagnosticsInfo> GetDiagnosticsAsync()
        {
            var process = Process.GetCurrentProcess();
            var uptime = DateTime.Now - process.StartTime;

            var performanceTask = _performanceService.GetPerformanceMetricsAsync();
            var databaseHealthTask = _databaseHealthService.GetDatabaseHealthAsync();

            await Task.WhenAll(performanceTask, databaseHealthTask);

            var diagnostics = new SystemDiagnosticsInfo
            {
                Performance = await performanceTask,
                DatabaseHealth = await databaseHealthTask,
                Uptime = uptime,
                DotNetVersion = RuntimeInformation.FrameworkDescription,
                OsArchitecture = RuntimeInformation.OSArchitecture.ToString(),
                OsDescription = RuntimeInformation.OSDescription,
                ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
                ConnectionString = GetSanitizedConnectionString(),
                LastUpdated = DateTime.UtcNow
            };

            diagnostics.OverallHealth = CalculateOverallHealth(diagnostics);

            return diagnostics;
        }

        private HealthStatus CalculateOverallHealth(SystemDiagnosticsInfo diagnostics)
        {
            var perf = diagnostics.Performance;
            var db = diagnostics.DatabaseHealth;

            if (db.Status == HealthStatus.Critical || perf.CpuUsage > 90 || perf.MemoryUsagePercentage > 90)
            {
                return HealthStatus.Critical;
            }

            if (db.Status == HealthStatus.Warning || perf.CpuUsage > 75 || perf.MemoryUsagePercentage > 75)
            {
                return HealthStatus.Warning;
            }

            return HealthStatus.Healthy;
        }

        private string GetSanitizedConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                return "Not configured.";
            }

            try
            {
                var mongoUrl = new MongoUrl(connectionString);
                // Sanitize by rebuilding the URL without the password.
                var sanitizedUrl = new MongoUrlBuilder
                {
                    Scheme = mongoUrl.Scheme,
                    Server = mongoUrl.Server,
                    DatabaseName = mongoUrl.DatabaseName,
                    Username = mongoUrl.Username
                    // Password is intentionally omitted.
                }.ToMongoUrl();

                return sanitizedUrl.ToString();
            }
            catch (Exception)
            {
                return "Invalid connection string format.";
            }
        }
    }
}
