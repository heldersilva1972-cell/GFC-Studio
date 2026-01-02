using System.Data;
using System.Diagnostics;
using GFC.BlazorServer.Data;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Diagnostics;

/// <summary>
/// Service for checking database health and performance
/// </summary>
public class DatabaseHealthService
{
    private readonly GfcDbContext _dbContext;
    private readonly AgentApiClient _agentApiClient;
    private readonly ILogger<DatabaseHealthService> _logger;
    
    public DatabaseHealthService(
        GfcDbContext dbContext,
        AgentApiClient agentApiClient,
        ILogger<DatabaseHealthService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _agentApiClient = agentApiClient ?? throw new ArgumentNullException(nameof(agentApiClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<DatabaseHealthInfo> GetDatabaseHealthAsync(CancellationToken cancellationToken = default)
    {
        var healthInfo = new DatabaseHealthInfo
        {
            CollectedAt = DateTime.UtcNow,
            Status = HealthStatus.Unknown,
            Message = "Starting database health check..."
        };
        
        var connection = _dbContext.Database.GetDbConnection();
        healthInfo.Provider = connection.GetType().Name;
        
        var shouldCloseConnection = false;
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
                shouldCloseConnection = true;
            }
            
            stopwatch.Stop();
            healthInfo.ConnectionResponseTimeMs = stopwatch.ElapsedMilliseconds;
            
            // Get basic database info
            await GetDatabaseInfoAsync(connection, healthInfo, cancellationToken);
            
            // Get database size
            await GetDatabaseSizeAsync(connection, healthInfo, cancellationToken);
            
            // Get backup information
            await GetBackupInfoAsync(connection, healthInfo, cancellationToken);
            
            // Get migration status
            await GetMigrationStatusAsync(healthInfo, cancellationToken);
            
            // Get query performance
            await GetQueryPerformanceAsync(connection, healthInfo, cancellationToken);

            // Calculate overall health
            healthInfo.Status = CalculateDatabaseHealth(healthInfo);
            healthInfo.Message = $"Database is {healthInfo.Status}.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database health");
            healthInfo.Status = HealthStatus.Critical;
            healthInfo.Message = $"Failed to retrieve database health: {ex.Message}";
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
        
        return healthInfo;
    }
    
    private async Task GetDatabaseInfoAsync(IDbConnection connection, DatabaseHealthInfo healthInfo, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT @@SERVERNAME, DB_NAME()";
        
        using var reader = await ((System.Data.Common.DbCommand)command).ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            healthInfo.ServerName = reader.IsDBNull(0) ? "(unknown)" : reader.GetString(0);
            healthInfo.DatabaseName = reader.IsDBNull(1) ? "(unknown)" : reader.GetString(1);
        }
    }
    
    private async Task GetDatabaseSizeAsync(IDbConnection connection, DatabaseHealthInfo healthInfo, CancellationToken cancellationToken)
    {
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT SUM(size) * 8 * 1024 
                FROM sys.master_files 
                WHERE database_id = DB_ID()";
            
            var result = await ((System.Data.Common.DbCommand)command).ExecuteScalarAsync(cancellationToken);
            if (result != null && result != DBNull.Value)
            {
                healthInfo.DatabaseSizeBytes = Convert.ToInt64(result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get database size");
            healthInfo.DatabaseSizeBytes = 0;
        }
    }
    
    private async Task GetBackupInfoAsync(IDbConnection connection, DatabaseHealthInfo healthInfo, CancellationToken cancellationToken)
    {
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT TOP 1 backup_finish_date 
                FROM msdb.dbo.backupset 
                WHERE database_name = DB_NAME() 
                ORDER BY backup_finish_date DESC";
            
            var result = await ((System.Data.Common.DbCommand)command).ExecuteScalarAsync(cancellationToken);
            if (result != null && result != DBNull.Value)
            {
                healthInfo.LastBackupDate = Convert.ToDateTime(result);
                healthInfo.LastBackupSuccessful = true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get backup info (might not have permissions)");
            healthInfo.LastBackupDate = null;
        }
    }
    
    private async Task GetMigrationStatusAsync(DatabaseHealthInfo healthInfo, CancellationToken cancellationToken)
    {
        try
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            var appliedMigrations = await _dbContext.Database.GetAppliedMigrationsAsync(cancellationToken);
            
            healthInfo.PendingMigrations = pendingMigrations.Count();
            healthInfo.HasPendingMigrations = healthInfo.PendingMigrations > 0;
            healthInfo.CurrentSchemaVersion = appliedMigrations.LastOrDefault() ?? "None";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get migration status");
            healthInfo.HasPendingMigrations = false;
            healthInfo.PendingMigrations = 0;
            healthInfo.CurrentSchemaVersion = "Unknown";
        }
    }
    
    private HealthStatus CalculateDatabaseHealth(DatabaseHealthInfo healthInfo)
    {
        // Critical conditions
        if (healthInfo.ConnectionResponseTimeMs > 5000)
            return HealthStatus.Critical;
        
        if (healthInfo.HasPendingMigrations)
            return HealthStatus.Warning;
        
        // Check backup status
        if (healthInfo.LastBackupDate.HasValue)
        {
            var daysSinceBackup = (DateTime.UtcNow - healthInfo.LastBackupDate.Value).TotalDays;
            if (daysSinceBackup > 7)
                return HealthStatus.Critical;
            if (daysSinceBackup > 3)
                return HealthStatus.Warning;
        }
        
        // Check connection time
        if (healthInfo.ConnectionResponseTimeMs > 1000)
            return HealthStatus.Warning;
        
        // Check slow query
        if (healthInfo.SlowQueryResponseTimeMs > 5000)
            return HealthStatus.Critical;
        if (healthInfo.SlowQueryResponseTimeMs > 1000)
            return HealthStatus.Warning;

        return HealthStatus.Healthy;
    }
    
    private async Task GetQueryPerformanceAsync(IDbConnection connection, DatabaseHealthInfo healthInfo, CancellationToken cancellationToken)
    {
        const int SlowQueryThresholdMs = 1000;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            using var command = connection.CreateCommand();
            // Optimized health check query Use NOLOCK to prevent blocking
            command.CommandText = @"
                SELECT TOP 1 e.Id
                FROM ControllerEvents e WITH (NOLOCK)
                ORDER BY e.Timestamp DESC";

            using (var reader = await ((System.Data.Common.DbCommand)command).ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    // Consume the results
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to execute performance query");
        }
        finally
        {
            stopwatch.Stop();
            healthInfo.SlowQueryResponseTimeMs = stopwatch.ElapsedMilliseconds;

            if (healthInfo.SlowQueryResponseTimeMs > SlowQueryThresholdMs)
            {
                _logger.LogWarning("Slow query detected: {QueryName} took {ResponseTime}ms", "GetTop10ControllerEvents", healthInfo.SlowQueryResponseTimeMs);
            }
        }
    }

    public async Task<DiagnosticActionResult> TestDatabaseConnectionAsync()
    {
        var result = new DiagnosticActionResult
        {
            ActionName = "Database Connection Test",
            ExecutedAt = DateTime.UtcNow
        };
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync();
            stopwatch.Stop();
            
            result.Success = canConnect;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = canConnect 
                ? $"Successfully connected to database in {result.ResponseTimeMs}ms" 
                : "Failed to connect to database";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Database connection test failed");
            result.Success = false;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = $"Connection test failed: {ex.Message}";
        }
        
        return result;
    }
    
    public async Task<DiagnosticActionResult> TestAgentApiConnectionAsync()
    {
        var result = new DiagnosticActionResult
        {
            ActionName = "Agent API Connection Test",
            ExecutedAt = DateTime.UtcNow
        };
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var reachable = await _agentApiClient.PingAsync();
            stopwatch.Stop();
            
            result.Success = reachable;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = reachable 
                ? $"Agent API is reachable (responded in {result.ResponseTimeMs}ms)" 
                : "Agent API is not reachable";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Agent API connection test failed");
            result.Success = false;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = $"Agent API test failed: {ex.Message}";
        }
        
        return result;
    }
}
