// [MODIFIED]
using GFC.Core.Models;
using GFC.Core.Models.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace GFC.BlazorServer.Services
{
    public class DiagnosticsService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DiagnosticsService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            // It's better to get the connection string once. Assuming "DefaultConnection".
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SystemDiagnosticsInfo> GetDiagnosticsAsync()
        {
            var process = Process.GetCurrentProcess();
            var memoryUsageMb = process.WorkingSet64 / (1024 * 1024);
            var uptime = DateTime.Now - process.StartTime;

            var diagnostics = new SystemDiagnosticsInfo
            {
                MemoryUsage = memoryUsageMb,
                Uptime = uptime,
                DotNetVersion = RuntimeInformation.FrameworkDescription,
                OsArchitecture = RuntimeInformation.OSArchitecture.ToString(),
                OsDescription = RuntimeInformation.OSDescription,
                ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
                ConnectionString = GetSanitizedConnectionString(),
                LastUpdated = DateTime.UtcNow
            };

            diagnostics.OverallHealth = await CalculateOverallHealthAsync(diagnostics);

            return diagnostics;
        }

        private async Task<HealthStatus> CalculateOverallHealthAsync(SystemDiagnosticsInfo diagnostics)
        {
            // Check 1: Database Connectivity (Most Critical)
            if (!await IsDatabaseConnectedAsync())
            {
                return HealthStatus.Critical;
            }

            // Check 2: Memory Usage
            if (diagnostics.MemoryUsage > 1024) // 1 GB threshold for a warning
            {
                return HealthStatus.Warning;
            }

            // If all checks pass, system is healthy
            return HealthStatus.Healthy;
        }

        private async Task<bool> IsDatabaseConnectedAsync()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return false;
            }

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetSanitizedConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return "Not configured.";
            }

            try
            {
                var builder = new SqlConnectionStringBuilder(_connectionString)
                {
                    // Remove sensitive information
                    Password = "*****",
                    UserID = "*****"
                };
                return builder.ConnectionString;
            }
            catch (Exception)
            {
                return "Invalid connection string format.";
            }
        }
    }
}
