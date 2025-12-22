// [NEW]
using GFC.Core.Models.Diagnostics;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Diagnostics
{
    public interface IDatabaseHealthService
    {
        Task<DatabaseHealthInfo> GetDatabaseHealthAsync(CancellationToken cancellationToken = default);
        Task<DiagnosticActionResult> TestDatabaseConnectionAsync();
        Task<DiagnosticActionResult> TestAgentApiConnectionAsync();
    }

    public class DatabaseHealthService : IDatabaseHealthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseHealthService> _logger;
        private readonly IMongoClient _mongoClient;
        private readonly AgentApiClient _agentApiClient;

        public DatabaseHealthService(IConfiguration configuration, ILogger<DatabaseHealthService> logger, IMongoClient mongoClient, AgentApiClient agentApiClient)
        {
            _configuration = configuration;
            _logger = logger;
            _mongoClient = mongoClient;
            _agentApiClient = agentApiClient;
        }

        public async Task<DatabaseHealthInfo> GetDatabaseHealthAsync(CancellationToken cancellationToken = default)
        {
            var info = new DatabaseHealthInfo
            {
                Status = HealthStatus.Unknown,
                Message = "Starting database health check..."
            };

            try
            {
                var dbName = new MongoUrl(_configuration.GetConnectionString("DefaultConnection")).DatabaseName;
                var database = _mongoClient.GetDatabase(dbName);

                // Check database size
                var stats = await database.RunCommandAsync<MongoDB.Bson.BsonDocument>("{ dbStats: 1 }");
                if (stats.TryGetValue("storageSize", out var storageSize))
                {
                    info.DatabaseSizeGb = Math.Round(storageSize.AsDouble / (1024 * 1024 * 1024), 2);
                }

                // Get connection pool stats from the MongoDB client
                var server = _mongoClient.Cluster.Description.Servers.FirstOrDefault();
                if (server != null)
                {
                    info.ActiveConnections = server.Connections.Count(c => c.IsOpen);
                    info.IdleConnections = server.Connections.Count(c => !c.IsOpen);
                    info.MaxPoolSize = _mongoClient.Settings.MaxConnectionPoolSize;
                }

                // Placeholder for last backup date - this would typically come from a dedicated service or log
                info.LastBackupDate = DateTime.UtcNow.AddDays(-2); // Example value
                info.LastBackupSuccessful = true;

                // Placeholder for migrations - this would require a custom migration tracking system
                info.HasPendingMigrations = false; // Example value

                // Test connection
                var connectionTest = await TestDatabaseConnectionAsync();
                info.ConnectionResponseTimeMs = connectionTest.ResponseTimeMs;

                // Health Status Calculation
                info.Status = CalculateHealthStatus(info);
                info.Message = $"Database is {info.Status}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database health.");
                info.Status = HealthStatus.Critical;
                info.Message = $"Failed to retrieve database health: {ex.Message}";
            }

            return info;
        }

        public async Task<DiagnosticActionResult> TestDatabaseConnectionAsync()
        {
            var result = new DiagnosticActionResult();
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _mongoClient.GetDatabase("admin").RunCommandAsync<MongoDB.Bson.BsonDocument>("{ ping: 1 }");
                stopwatch.Stop();
                result.Success = true;
                result.Message = "Database connection successful.";
                result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Database connection test failed.");
                result.Success = false;
                result.Message = $"Database connection failed: {ex.Message}";
                result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            }
            return result;
        }

        public async Task<DiagnosticActionResult> TestAgentApiConnectionAsync()
        {
            var result = new DiagnosticActionResult();
            var stopwatch = Stopwatch.StartNew();
            try
            {
                // Assuming the AgentApiClient has a method to check health or make a simple request
                var response = await _agentApiClient.GetAsync("health"); // Example endpoint
                response.EnsureSuccessStatusCode();
                stopwatch.Stop();
                result.Success = true;
                result.Message = "Agent API connection successful.";
                result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Agent API connection test failed.");
                result.Success = false;
                result.Message = $"Agent API connection failed: {ex.Message}";
                result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            }
            return result;
        }

        private HealthStatus CalculateHealthStatus(DatabaseHealthInfo info)
        {
            if (info.ConnectionResponseTimeMs > 5000) return HealthStatus.Critical;
            if (info.ConnectionResponseTimeMs > 1000) return HealthStatus.Warning;

            if (info.LastBackupDate.HasValue)
            {
                if ((DateTime.UtcNow - info.LastBackupDate.Value).TotalDays > 7) return HealthStatus.Critical;
                if ((DateTime.UtcNow - info.LastBackupDate.Value).TotalDays > 3) return HealthStatus.Warning;
            }

            if (info.HasPendingMigrations) return HealthStatus.Warning;

            return HealthStatus.Healthy;
        }
    }
}
