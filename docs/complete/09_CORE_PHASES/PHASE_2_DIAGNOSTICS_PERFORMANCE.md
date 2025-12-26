# Phase 2: System Performance & Database Health Diagnostics

## Overview
Build upon Phase 1's foundation by adding comprehensive system performance monitoring, database health diagnostics, and performance trend charts. This phase transforms the diagnostics page from static information display to a dynamic monitoring dashboard.

---

## Goals
- ✅ Add real-time system performance monitoring (CPU, memory, connections)
- ✅ Implement comprehensive database health checks
- ✅ Create performance history tracking with trend charts
- ✅ Add diagnostic action buttons (test connections, run health checks)
- ✅ Implement threshold warnings and alerts

---

## Prerequisites
- ✅ Phase 1 completed and tested
- ✅ All Phase 1 components working (StatusBadge, MetricCard, etc.)
- ✅ diagnostics.css loaded and working
- ✅ HealthStatus enum available

---

## Implementation Tasks

### **Task 1: Enhanced Performance Metrics** 📊

#### 1.1 Create PerformanceMetrics Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/PerformanceMetrics.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// System performance metrics
/// </summary>
public class PerformanceMetrics
{
    // Memory metrics
    public long MemoryUsedBytes { get; set; }
    public long MemoryTotalBytes { get; set; }
    public double MemoryUsagePercent => MemoryTotalBytes > 0 
        ? (double)MemoryUsedBytes / MemoryTotalBytes * 100 
        : 0;
    
    // CPU metrics
    public double CpuUsagePercent { get; set; }
    
    // Application metrics
    public TimeSpan Uptime { get; set; }
    public int ActiveConnections { get; set; }
    public int TotalThreads { get; set; }
    
    // Request metrics
    public int RequestsPerMinute { get; set; }
    public double AverageResponseTimeMs { get; set; }
    
    // Error metrics
    public int ErrorCount24Hours { get; set; }
    public int ErrorCountLastHour { get; set; }
    
    // Garbage collection metrics
    public int Gen0Collections { get; set; }
    public int Gen1Collections { get; set; }
    public int Gen2Collections { get; set; }
    
    // Timestamps
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    
    // Health calculation
    public HealthStatus OverallHealth => CalculateHealth();
    
    private HealthStatus CalculateHealth()
    {
        // Critical conditions
        if (MemoryUsagePercent > 90 || CpuUsagePercent > 90 || ErrorCount24Hours > 100)
            return HealthStatus.Critical;
        
        // Warning conditions
        if (MemoryUsagePercent > 75 || CpuUsagePercent > 75 || ErrorCount24Hours > 50)
            return HealthStatus.Warning;
        
        // Error conditions
        if (ErrorCountLastHour > 10)
            return HealthStatus.Error;
        
        return HealthStatus.Healthy;
    }
    
    // Helper properties
    public double MemoryUsedMB => MemoryUsedBytes / 1024.0 / 1024.0;
    public double MemoryUsedGB => MemoryUsedBytes / 1024.0 / 1024.0 / 1024.0;
    public double MemoryTotalGB => MemoryTotalBytes / 1024.0 / 1024.0 / 1024.0;
    
    public string UptimeFormatted => FormatUptime(Uptime);
    
    private static string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
            return $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m";
        if (uptime.TotalHours >= 1)
            return $"{(int)uptime.TotalHours}h {uptime.Minutes}m";
        if (uptime.TotalMinutes >= 1)
            return $"{(int)uptime.TotalMinutes}m {uptime.Seconds}s";
        return $"{(int)uptime.TotalSeconds}s";
    }
}
```

#### 1.2 Create DatabaseHealthInfo Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/DatabaseHealthInfo.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Database health and performance information
/// </summary>
public class DatabaseHealthInfo
{
    // Connection info
    public string Provider { get; set; } = string.Empty;
    public string ServerName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionStringName { get; set; } = "GFC";
    
    // Connection pool metrics
    public int ConnectionPoolSize { get; set; }
    public int ActiveConnections { get; set; }
    public int IdleConnections { get; set; }
    public double ConnectionPoolUsagePercent => ConnectionPoolSize > 0 
        ? (double)ActiveConnections / ConnectionPoolSize * 100 
        : 0;
    
    // Performance metrics
    public double AverageQueryTimeMs { get; set; }
    public int SlowQueriesCount { get; set; }
    public int TotalQueriesLast5Minutes { get; set; }
    
    // Database size
    public long DatabaseSizeBytes { get; set; }
    public double DatabaseSizeMB => DatabaseSizeBytes / 1024.0 / 1024.0;
    public double DatabaseSizeGB => DatabaseSizeBytes / 1024.0 / 1024.0 / 1024.0;
    
    // Backup info
    public DateTime? LastBackupDate { get; set; }
    public TimeSpan? TimeSinceLastBackup => LastBackupDate.HasValue 
        ? DateTime.UtcNow - LastBackupDate.Value 
        : null;
    
    // Migration status
    public string CurrentSchemaVersion { get; set; } = string.Empty;
    public bool MigrationsUpToDate { get; set; }
    public int PendingMigrations { get; set; }
    
    // Health status
    public HealthStatus OverallHealth { get; set; } = HealthStatus.Unknown;
    public bool IsReachable { get; set; }
    public string? ErrorMessage { get; set; }
    
    // Response time
    public double ConnectionTestTimeMs { get; set; }
    
    // Timestamps
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    
    // Helper methods
    public string GetBackupStatusText()
    {
        if (!LastBackupDate.HasValue)
            return "No backup information available";
        
        var timeSince = TimeSinceLastBackup!.Value;
        
        if (timeSince.TotalDays >= 7)
            return $"Last backup {(int)timeSince.TotalDays} days ago - OVERDUE";
        if (timeSince.TotalDays >= 1)
            return $"Last backup {(int)timeSince.TotalDays} days ago";
        if (timeSince.TotalHours >= 1)
            return $"Last backup {(int)timeSince.TotalHours} hours ago";
        
        return "Recently backed up";
    }
    
    public HealthStatus GetBackupHealth()
    {
        if (!LastBackupDate.HasValue)
            return HealthStatus.Unknown;
        
        var timeSince = TimeSinceLastBackup!.Value;
        
        if (timeSince.TotalDays >= 7)
            return HealthStatus.Critical;
        if (timeSince.TotalDays >= 3)
            return HealthStatus.Warning;
        
        return HealthStatus.Healthy;
    }
}
```

#### 1.3 Update SystemDiagnosticsInfo Model
**File**: `apps/webapp/GFC.Core/Models/SystemDiagnosticsInfo.cs` (UPDATE)

Add these properties:

```csharp
// Add to existing SystemDiagnosticsInfo class

// NEW: Detailed metrics
public PerformanceMetrics? Performance { get; set; }
public DatabaseHealthInfo? DatabaseHealth { get; set; }

// NEW: Action results
public Dictionary<string, DiagnosticActionResult> ActionResults { get; set; } = new();
```

#### 1.4 Create DiagnosticActionResult Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/DiagnosticActionResult.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Result of a diagnostic action (test, health check, etc.)
/// </summary>
public class DiagnosticActionResult
{
    public string ActionName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public double DurationMs { get; set; }
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    public Dictionary<string, object> AdditionalData { get; set; } = new();
}
```

---

### **Task 2: Create Performance Monitoring Service** ⚙️

#### 2.1 Create SystemPerformanceService
**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/SystemPerformanceService.cs` (NEW)

```csharp
using System.Diagnostics;
using GFC.Core.Models.Diagnostics;

namespace GFC.BlazorServer.Services.Diagnostics;

/// <summary>
/// Service for collecting system performance metrics
/// </summary>
public class SystemPerformanceService
{
    private static readonly DateTime _applicationStartTime = DateTime.UtcNow;
    private static int _requestCount = 0;
    private static readonly object _requestLock = new();
    
    public PerformanceMetrics GetPerformanceMetrics()
    {
        var metrics = new PerformanceMetrics
        {
            CollectedAt = DateTime.UtcNow,
            Uptime = DateTime.UtcNow - _applicationStartTime
        };
        
        try
        {
            var process = Process.GetCurrentProcess();
            
            // Memory metrics
            metrics.MemoryUsedBytes = process.WorkingSet64;
            
            // Try to get total physical memory (Windows)
            try
            {
                var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
                metrics.MemoryTotalBytes = (long)computerInfo.TotalPhysicalMemory;
            }
            catch
            {
                // Fallback: estimate based on current usage
                metrics.MemoryTotalBytes = metrics.MemoryUsedBytes * 4; // Rough estimate
            }
            
            // CPU metrics (requires PerformanceCounter or external library)
            // For now, we'll use a simple approximation
            metrics.CpuUsagePercent = GetCpuUsage(process);
            
            // Thread metrics
            metrics.TotalThreads = process.Threads.Count;
            
            // GC metrics
            metrics.Gen0Collections = GC.CollectionCount(0);
            metrics.Gen1Collections = GC.CollectionCount(1);
            metrics.Gen2Collections = GC.CollectionCount(2);
            
            // Request metrics (simplified - in production, use middleware)
            lock (_requestLock)
            {
                metrics.RequestsPerMinute = _requestCount;
            }
            
            // TODO: Implement error counting from logs/database
            metrics.ErrorCount24Hours = 0;
            metrics.ErrorCountLastHour = 0;
            
        }
        catch (Exception ex)
        {
            // Log error but return partial metrics
            Console.WriteLine($"Error collecting performance metrics: {ex.Message}");
        }
        
        return metrics;
    }
    
    private double GetCpuUsage(Process process)
    {
        try
        {
            // Simple CPU usage calculation
            // Note: For accurate CPU usage, consider using PerformanceCounter or a library
            var startTime = DateTime.UtcNow;
            var startCpuUsage = process.TotalProcessorTime;
            
            System.Threading.Thread.Sleep(100); // Small delay for measurement
            
            var endTime = DateTime.UtcNow;
            var endCpuUsage = process.TotalProcessorTime;
            
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            
            return cpuUsageTotal * 100;
        }
        catch
        {
            return 0;
        }
    }
    
    public static void IncrementRequestCount()
    {
        lock (_requestLock)
        {
            _requestCount++;
        }
    }
    
    public static void ResetRequestCount()
    {
        lock (_requestLock)
        {
            _requestCount = 0;
        }
    }
}
```

**Note**: Add reference to `Microsoft.VisualBasic` for ComputerInfo:
```xml
<!-- Add to GFC.BlazorServer.csproj -->
<ItemGroup>
  <Reference Include="Microsoft.VisualBasic" />
</ItemGroup>
```

#### 2.2 Create DatabaseHealthService
**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/DatabaseHealthService.cs` (NEW)

```csharp
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
    
    public DatabaseHealthService(GfcDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<DatabaseHealthInfo> GetDatabaseHealthAsync(CancellationToken cancellationToken = default)
    {
        var healthInfo = new DatabaseHealthInfo
        {
            CollectedAt = DateTime.UtcNow
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
            healthInfo.ConnectionTestTimeMs = stopwatch.ElapsedMilliseconds;
            healthInfo.IsReachable = true;
            
            // Get basic database info
            await GetDatabaseInfoAsync(connection, healthInfo, cancellationToken);
            
            // Get database size
            await GetDatabaseSizeAsync(connection, healthInfo, cancellationToken);
            
            // Get backup information
            await GetBackupInfoAsync(connection, healthInfo, cancellationToken);
            
            // Get migration status
            await GetMigrationStatusAsync(healthInfo, cancellationToken);
            
            // Calculate overall health
            healthInfo.OverallHealth = CalculateDatabaseHealth(healthInfo);
        }
        catch (Exception ex)
        {
            healthInfo.IsReachable = false;
            healthInfo.ErrorMessage = ex.Message;
            healthInfo.OverallHealth = HealthStatus.Error;
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
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
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
            
            var result = await command.ExecuteScalarAsync(cancellationToken);
            if (result != null && result != DBNull.Value)
            {
                healthInfo.DatabaseSizeBytes = Convert.ToInt64(result);
            }
        }
        catch
        {
            // Size query failed, not critical
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
            
            var result = await command.ExecuteScalarAsync(cancellationToken);
            if (result != null && result != DBNull.Value)
            {
                healthInfo.LastBackupDate = Convert.ToDateTime(result);
            }
        }
        catch
        {
            // Backup query failed (might not have permissions), not critical
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
            healthInfo.MigrationsUpToDate = healthInfo.PendingMigrations == 0;
            healthInfo.CurrentSchemaVersion = appliedMigrations.LastOrDefault() ?? "None";
        }
        catch
        {
            healthInfo.MigrationsUpToDate = true; // Assume OK if can't check
            healthInfo.PendingMigrations = 0;
            healthInfo.CurrentSchemaVersion = "Unknown";
        }
    }
    
    private HealthStatus CalculateDatabaseHealth(DatabaseHealthInfo healthInfo)
    {
        // Critical conditions
        if (!healthInfo.IsReachable)
            return HealthStatus.Critical;
        
        if (!healthInfo.MigrationsUpToDate)
            return HealthStatus.Warning;
        
        // Check backup status
        var backupHealth = healthInfo.GetBackupHealth();
        if (backupHealth == HealthStatus.Critical)
            return HealthStatus.Warning; // Downgrade to warning for backup issues
        
        // Check connection time
        if (healthInfo.ConnectionTestTimeMs > 1000)
            return HealthStatus.Warning;
        
        return HealthStatus.Healthy;
    }
    
    public async Task<DiagnosticActionResult> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        var result = new DiagnosticActionResult
        {
            ActionName = "Database Connection Test",
            ExecutedAt = DateTime.UtcNow
        };
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            stopwatch.Stop();
            
            result.Success = canConnect;
            result.DurationMs = stopwatch.ElapsedMilliseconds;
            result.Message = canConnect 
                ? $"Successfully connected to database in {result.DurationMs}ms" 
                : "Failed to connect to database";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            result.Success = false;
            result.DurationMs = stopwatch.ElapsedMilliseconds;
            result.Message = $"Connection test failed: {ex.Message}";
        }
        
        return result;
    }
}
```

---

### **Task 3: Update DiagnosticsService** 🔄

**File**: `apps/webapp/GFC.BlazorServer/Services/DiagnosticsService.cs` (UPDATE)

Add these dependencies and methods:

```csharp
// Add to constructor parameters
private readonly SystemPerformanceService _performanceService;
private readonly DatabaseHealthService _databaseHealthService;

public DiagnosticsService(
    GfcDbContext dbContext,
    IConfiguration configuration,
    IHostEnvironment environment,
    IVersionService versionService,
    IControllerClient controllerClient,
    SystemPerformanceService performanceService,
    DatabaseHealthService databaseHealthService)
{
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    _versionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
    _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
    _performanceService = performanceService ?? throw new ArgumentNullException(nameof(performanceService));
    _databaseHealthService = databaseHealthService ?? throw new ArgumentNullException(nameof(databaseHealthService));
}

// UPDATE GetDiagnosticsAsync to include new metrics
public async Task<SystemDiagnosticsInfo> GetDiagnosticsAsync(CancellationToken cancellationToken = default)
{
    var diagnostics = new SystemDiagnosticsInfo
    {
        EnvironmentName = _environment.EnvironmentName,
        ApplicationVersion = _versionService.GetFullVersion(),
        AgentApiBaseUrl = _configuration["AgentApi:BaseUrl"] ?? string.Empty,
        CollectedAt = DateTime.UtcNow
    };

    // Collect performance metrics
    diagnostics.Performance = _performanceService.GetPerformanceMetrics();
    diagnostics.Uptime = diagnostics.Performance.Uptime;
    diagnostics.MemoryUsedBytes = diagnostics.Performance.MemoryUsedBytes;
    diagnostics.ErrorCount24Hours = diagnostics.Performance.ErrorCount24Hours;

    // Collect database health
    diagnostics.DatabaseHealth = await _databaseHealthService.GetDatabaseHealthAsync(cancellationToken);
    
    // Legacy properties for backward compatibility
    diagnostics.DatabaseProvider = diagnostics.DatabaseHealth.Provider;
    diagnostics.DatabaseServer = diagnostics.DatabaseHealth.ServerName;
    diagnostics.DatabaseName = diagnostics.DatabaseHealth.DatabaseName;
    diagnostics.DatabaseHealth = diagnostics.DatabaseHealth.OverallHealth;
    
    // Populate agent status
    await PopulateAgentStatusAsync(diagnostics, cancellationToken);
    
    // Calculate overall health
    CalculateOverallHealth(diagnostics);

    return diagnostics;
}

// ADD new action methods
public async Task<DiagnosticActionResult> TestDatabaseConnectionAsync(CancellationToken cancellationToken = default)
{
    return await _databaseHealthService.TestConnectionAsync(cancellationToken);
}

public async Task<DiagnosticActionResult> TestAgentApiAsync(CancellationToken cancellationToken = default)
{
    var result = new DiagnosticActionResult
    {
        ActionName = "Agent API Connection Test",
        ExecutedAt = DateTime.UtcNow
    };
    
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    
    try
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        
        var reachable = await _controllerClient.PingAsync(cts.Token);
        stopwatch.Stop();
        
        result.Success = reachable;
        result.DurationMs = stopwatch.ElapsedMilliseconds;
        result.Message = reachable 
            ? $"Agent API is reachable (responded in {result.DurationMs}ms)" 
            : "Agent API is not reachable";
    }
    catch (Exception ex)
    {
        stopwatch.Stop();
        result.Success = false;
        result.DurationMs = stopwatch.ElapsedMilliseconds;
        result.Message = $"Agent API test failed: {ex.Message}";
    }
    
    return result;
}
```

---

### **Task 4: Register Services in DI** 📦

**File**: `apps/webapp/GFC.BlazorServer/Program.cs` (UPDATE)

Add service registrations:

```csharp
// Add these service registrations in the ConfigureServices section
builder.Services.AddScoped<GFC.BlazorServer.Services.Diagnostics.SystemPerformanceService>();
builder.Services.AddScoped<GFC.BlazorServer.Services.Diagnostics.DatabaseHealthService>();
```

---

### **Task 5: Create Performance Dashboard Component** 📈

#### 5.1 Create PerformanceMetricsCard Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/PerformanceMetricsCard.razor` (NEW)

```razor
@using GFC.Core.Models.Diagnostics

<div class="row g-3">
    <div class="col-md-3">
        <MetricCard 
            Label="CPU Usage"
            IconClass="bi bi-cpu"
            IconColorClass="@GetCpuHealthColor()"
            NumericValue="@Metrics.CpuUsagePercent"
            ValueFormat="N1"
            Unit="%"
            SubText="@GetCpuStatusText()"
            SubTextClass="@GetCpuSubTextClass()"
            Status="@GetCpuHealth()"
            ShowStatus="true"
            IsLoading="@IsLoading" />
    </div>
    
    <div class="col-md-3">
        <MetricCard 
            Label="Memory Usage"
            IconClass="bi bi-memory"
            IconColorClass="@GetMemoryHealthColor()"
            NumericValue="@Metrics.MemoryUsagePercent"
            ValueFormat="N1"
            Unit="%"
            SubText="@($"{Metrics.MemoryUsedGB:N2} GB / {Metrics.MemoryTotalGB:N2} GB")"
            Status="@GetMemoryHealth()"
            ShowStatus="true"
            IsLoading="@IsLoading" />
    </div>
    
    <div class="col-md-3">
        <MetricCard 
            Label="Active Connections"
            IconClass="bi bi-diagram-3"
            IconColorClass="info"
            NumericValue="@Metrics.ActiveConnections"
            SubText="@($"{Metrics.TotalThreads} threads")"
            Status="HealthStatus.Healthy"
            ShowStatus="false"
            IsLoading="@IsLoading" />
    </div>
    
    <div class="col-md-3">
        <MetricCard 
            Label="Requests/Min"
            IconClass="bi bi-graph-up"
            IconColorClass="success"
            NumericValue="@Metrics.RequestsPerMinute"
            SubText="@GetRequestRateText()"
            Status="HealthStatus.Healthy"
            ShowStatus="false"
            IsLoading="@IsLoading" />
    </div>
</div>

@code {
    [Parameter] public PerformanceMetrics Metrics { get; set; } = new();
    [Parameter] public bool IsLoading { get; set; }
    
    private HealthStatus GetCpuHealth() => Metrics.CpuUsagePercent switch
    {
        > 90 => HealthStatus.Critical,
        > 75 => HealthStatus.Warning,
        _ => HealthStatus.Healthy
    };
    
    private HealthStatus GetMemoryHealth() => Metrics.MemoryUsagePercent switch
    {
        > 90 => HealthStatus.Critical,
        > 75 => HealthStatus.Warning,
        _ => HealthStatus.Healthy
    };
    
    private string GetCpuHealthColor() => GetCpuHealth() switch
    {
        HealthStatus.Critical => "danger",
        HealthStatus.Warning => "warning",
        _ => "success"
    };
    
    private string GetMemoryHealthColor() => GetMemoryHealth() switch
    {
        HealthStatus.Critical => "danger",
        HealthStatus.Warning => "warning",
        _ => "success"
    };
    
    private string GetCpuStatusText() => GetCpuHealth() switch
    {
        HealthStatus.Critical => "Critical load",
        HealthStatus.Warning => "High usage",
        _ => "Normal"
    };
    
    private string GetCpuSubTextClass() => GetCpuHealth() switch
    {
        HealthStatus.Critical or HealthStatus.Warning => "negative",
        _ => "positive"
    };
    
    private string GetRequestRateText()
    {
        if (Metrics.RequestsPerMinute > 100)
            return "High traffic";
        if (Metrics.RequestsPerMinute > 50)
            return "Moderate traffic";
        return "Low traffic";
    }
}
```

#### 5.2 Create DatabaseHealthCard Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/DatabaseHealthCard.razor` (NEW)

```razor
@using GFC.Core.Models.Diagnostics

<div class="metric-card">
    <div class="metric-card-header">
        <div class="metric-icon @GetHealthIconColor()">
            <i class="bi bi-database"></i>
        </div>
        <StatusBadge Status="@DatabaseHealth.OverallHealth" />
    </div>
    
    <div class="metric-content">
        <div class="metric-label">Database Health</div>
        
        @if (IsLoading)
        {
            <SkeletonLoader Type="text" Size="small" />
            <SkeletonLoader Type="text" Size="large" />
            <SkeletonLoader Type="text" Size="small" />
        }
        else
        {
            <div class="info-row">
                <span class="info-label">Server</span>
                <span class="info-value">
                    @DatabaseHealth.ServerName
                    <CopyButton TextToCopy="@DatabaseHealth.ServerName" ShowText="false" />
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Database</span>
                <span class="info-value">
                    @DatabaseHealth.DatabaseName
                    <CopyButton TextToCopy="@DatabaseHealth.DatabaseName" ShowText="false" />
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Size</span>
                <span class="info-value">@DatabaseHealth.DatabaseSizeGB.ToString("N2") GB</span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Connection Time</span>
                <span class="info-value">
                    @DatabaseHealth.ConnectionTestTimeMs ms
                    @if (DatabaseHealth.ConnectionTestTimeMs > 500)
                    {
                        <i class="bi bi-exclamation-triangle text-warning"></i>
                    }
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Last Backup</span>
                <span class="info-value">
                    <StatusBadge Status="@DatabaseHealth.GetBackupHealth()" 
                                 CustomText="@DatabaseHealth.GetBackupStatusText()" />
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Migrations</span>
                <span class="info-value">
                    @if (DatabaseHealth.MigrationsUpToDate)
                    {
                        <span class="text-success">
                            <i class="bi bi-check-circle-fill"></i> Up to date
                        </span>
                    }
                    else
                    {
                        <span class="text-warning">
                            <i class="bi bi-exclamation-triangle-fill"></i> 
                            @DatabaseHealth.PendingMigrations pending
                        </span>
                    }
                </span>
            </div>
        }
    </div>
    
    @if (ShowActions)
    {
        <div class="metric-card-footer mt-3">
            <button class="btn-diagnostic secondary w-100" @onclick="OnTestConnection">
                <i class="bi bi-plug"></i>
                Test Connection
            </button>
        </div>
    }
</div>

@code {
    [Parameter] public DatabaseHealthInfo DatabaseHealth { get; set; } = new();
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public bool ShowActions { get; set; } = true;
    [Parameter] public EventCallback OnTestConnection { get; set; }
    
    private string GetHealthIconColor() => DatabaseHealth.OverallHealth switch
    {
        HealthStatus.Healthy => "success",
        HealthStatus.Warning => "warning",
        HealthStatus.Error => "danger",
        HealthStatus.Critical => "danger",
        _ => "info"
    };
}
```

---

### **Task 6: Update SystemDiagnostics Page** 🎨

**File**: `apps/webapp/GFC.BlazorServer/Components/Pages/SystemDiagnostics.razor` (UPDATE)

Add new sections after the existing content:

```razor
@* Add after the existing "Quick Stats Row" *@

<!-- Performance Metrics Section -->
<div class="row mb-4">
    <div class="col-12">
        <h4 class="text-gradient-primary mb-3">
            <i class="bi bi-speedometer2 me-2"></i>
            System Performance
        </h4>
    </div>
</div>

@if (_info?.Performance != null)
{
    <PerformanceMetricsCard Metrics="@_info.Performance" IsLoading="@_isRefreshing" />
}

<!-- Database Health Section -->
<div class="row mt-5 mb-4">
    <div class="col-12">
        <h4 class="text-gradient-primary mb-3">
            <i class="bi bi-database me-2"></i>
            Database Health
        </h4>
    </div>
</div>

<div class="row g-4">
    <div class="col-md-6">
        @if (_info?.DatabaseHealth != null)
        {
            <DatabaseHealthCard 
                DatabaseHealth="@_info.DatabaseHealth" 
                IsLoading="@_isRefreshing"
                OnTestConnection="@TestDatabaseConnection" />
        }
    </div>
    
    <div class="col-md-6">
        <!-- Action Results Card -->
        @if (_actionResults.Any())
        {
            <div class="metric-card">
                <div class="metric-card-header">
                    <div class="metric-icon info">
                        <i class="bi bi-clipboard-check"></i>
                    </div>
                </div>
                
                <div class="metric-content">
                    <div class="metric-label">Recent Actions</div>
                    
                    @foreach (var result in _actionResults.OrderByDescending(r => r.ExecutedAt).Take(5))
                    {
                        <div class="info-row">
                            <span class="info-label">@result.ActionName</span>
                            <span class="info-value">
                                @if (result.Success)
                                {
                                    <span class="text-success">
                                        <i class="bi bi-check-circle-fill"></i> 
                                        @result.DurationMs ms
                                    </span>
                                }
                                else
                                {
                                    <span class="text-danger">
                                        <i class="bi bi-x-circle-fill"></i> 
                                        Failed
                                    </span>
                                }
                            </span>
                        </div>
                        @if (!result.Success && !string.IsNullOrEmpty(result.Message))
                        {
                            <div class="text-muted small mb-2">@result.Message</div>
                        }
                    }
                </div>
            </div>
        }
    </div>
</div>
```

Add to @code section:

```csharp
private List<DiagnosticActionResult> _actionResults = new();

private async Task TestDatabaseConnection()
{
    var result = await DiagnosticsService.TestDatabaseConnectionAsync();
    _actionResults.Add(result);
    StateHasChanged();
}

private async Task TestAgentApi()
{
    var result = await DiagnosticsService.TestAgentApiAsync();
    _actionResults.Add(result);
    StateHasChanged();
}
```

---

## Testing Checklist

After implementation, verify:

- [ ] Performance metrics display correctly (CPU, memory, connections)
- [ ] Database health card shows all information
- [ ] Health status badges show correct colors
- [ ] "Test Connection" button works and shows results
- [ ] Action results display in the card
- [ ] All animations and transitions work smoothly
- [ ] No compilation errors
- [ ] Page loads without errors
- [ ] Auto-refresh updates all metrics
- [ ] Threshold warnings appear when metrics exceed limits

---

## Files to Create

1. ✅ `apps/webapp/GFC.Core/Models/Diagnostics/PerformanceMetrics.cs`
2. ✅ `apps/webapp/GFC.Core/Models/Diagnostics/DatabaseHealthInfo.cs`
3. ✅ `apps/webapp/GFC.Core/Models/Diagnostics/DiagnosticActionResult.cs`
4. ✅ `apps/webapp/GFC.BlazorServer/Services/Diagnostics/SystemPerformanceService.cs`
5. ✅ `apps/webapp/GFC.BlazorServer/Services/Diagnostics/DatabaseHealthService.cs`
6. ✅ `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/PerformanceMetricsCard.razor`
7. ✅ `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/DatabaseHealthCard.razor`

## Files to Update

1. ✅ `apps/webapp/GFC.Core/Models/SystemDiagnosticsInfo.cs`
2. ✅ `apps/webapp/GFC.BlazorServer/Services/DiagnosticsService.cs`
3. ✅ `apps/webapp/GFC.BlazorServer/Components/Pages/SystemDiagnostics.razor`
4. ✅ `apps/webapp/GFC.BlazorServer/Program.cs`
5. ✅ `apps/webapp/GFC.BlazorServer/GFC.BlazorServer.csproj` (add Microsoft.VisualBasic reference)

---

## Success Criteria

✅ **Performance Monitoring**: Real-time CPU, memory, and connection metrics
✅ **Database Health**: Comprehensive database diagnostics with size, backup, migrations
✅ **Action Buttons**: Test connection buttons work and show results
✅ **Health Indicators**: Color-coded status badges for all metrics
✅ **Threshold Warnings**: Visual warnings when metrics exceed safe limits
✅ **Smooth UX**: All animations and transitions work properly

---

## Estimated Time

- **Task 1** (Models): 45 minutes
- **Task 2** (Services): 1.5 hours
- **Task 3** (Update DiagnosticsService): 30 minutes
- **Task 4** (DI Registration): 5 minutes
- **Task 5** (Components): 1 hour
- **Task 6** (Update Page): 30 minutes
- **Testing**: 45 minutes

**Total**: ~5 hours

---

## Next Steps (Phase 3)

After Phase 2 is complete:
- Add hardware controller diagnostics
- Implement camera system monitoring
- Add network status checks
- Create performance trend charts with historical data

---

**Status**: Ready for Implementation
**Priority**: High
**Dependencies**: Phase 1 completed
