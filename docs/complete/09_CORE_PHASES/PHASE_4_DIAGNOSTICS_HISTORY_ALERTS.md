# Phase 4: Performance History & Advanced Features (Final Phase)

## Overview
Complete the diagnostics system with performance history tracking, trend charts, advanced alerts, and export capabilities. This final phase transforms the diagnostics page into a comprehensive monitoring and troubleshooting tool.

---

## Goals
- ✅ Add performance history tracking with time-series data
- ✅ Create interactive trend charts for key metrics
- ✅ Implement alert thresholds and notifications
- ✅ Add diagnostic report export (PDF/JSON)
- ✅ Create system health timeline
- ✅ Add quick actions and troubleshooting guides

---

## Prerequisites
- ✅ Phase 1 completed (Foundation & UI)
- ✅ Phase 2 completed (Performance & Database)
- ✅ Phase 3 completed (Hardware & Cameras)

---

## Implementation Tasks

### **Task 1: Create Performance History Models** 📊

#### 1.1 Create PerformanceSnapshot Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/PerformanceSnapshot.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Point-in-time snapshot of system performance
/// </summary>
public class PerformanceSnapshot
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    // CPU metrics
    public double CpuUsagePercent { get; set; }
    
    // Memory metrics
    public long MemoryUsedBytes { get; set; }
    public double MemoryUsagePercent { get; set; }
    
    // Application metrics
    public int ActiveConnections { get; set; }
    public int RequestsPerMinute { get; set; }
    public int ErrorCount { get; set; }
    
    // Database metrics
    public double DatabaseResponseTimeMs { get; set; }
    public int DatabaseActiveConnections { get; set; }
    
    // Overall health
    public HealthStatus OverallHealth { get; set; }
}
```

#### 1.2 Create AlertThreshold Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/AlertThreshold.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Alert threshold configuration
/// </summary>
public class AlertThreshold
{
    public int Id { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    
    // Threshold values
    public double? WarningThreshold { get; set; }
    public double? CriticalThreshold { get; set; }
    
    // Alert settings
    public bool IsEnabled { get; set; } = true;
    public int CooldownMinutes { get; set; } = 15;
    
    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastTriggeredAt { get; set; }
}
```

#### 1.3 Create DiagnosticAlert Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/DiagnosticAlert.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Diagnostic alert record
/// </summary>
public class DiagnosticAlert
{
    public int Id { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public HealthStatus Severity { get; set; }
    public double CurrentValue { get; set; }
    public double ThresholdValue { get; set; }
    public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
    public bool IsAcknowledged { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
}
```

---

### **Task 2: Add Database Tables** 💾

**File**: Create new migration or update `GfcDbContext.cs`

Add DbSets to `GfcDbContext`:

```csharp
// Add to GfcDbContext.cs
public DbSet<PerformanceSnapshot> PerformanceSnapshots { get; set; }
public DbSet<AlertThreshold> AlertThresholds { get; set; }
public DbSet<DiagnosticAlert> DiagnosticAlerts { get; set; }
```

**Migration SQL** (if not using EF migrations):

```sql
-- Performance Snapshots Table
CREATE TABLE PerformanceSnapshots (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL,
    CpuUsagePercent FLOAT NOT NULL,
    MemoryUsedBytes BIGINT NOT NULL,
    MemoryUsagePercent FLOAT NOT NULL,
    ActiveConnections INT NOT NULL,
    RequestsPerMinute INT NOT NULL,
    ErrorCount INT NOT NULL,
    DatabaseResponseTimeMs FLOAT NOT NULL,
    DatabaseActiveConnections INT NOT NULL,
    OverallHealth INT NOT NULL,
    INDEX IX_PerformanceSnapshots_Timestamp (Timestamp DESC)
);

-- Alert Thresholds Table
CREATE TABLE AlertThresholds (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MetricName NVARCHAR(100) NOT NULL,
    DisplayName NVARCHAR(200) NOT NULL,
    WarningThreshold FLOAT NULL,
    CriticalThreshold FLOAT NULL,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CooldownMinutes INT NOT NULL DEFAULT 15,
    CreatedAt DATETIME2 NOT NULL,
    LastTriggeredAt DATETIME2 NULL,
    UNIQUE INDEX UX_AlertThresholds_MetricName (MetricName)
);

-- Diagnostic Alerts Table
CREATE TABLE DiagnosticAlerts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MetricName NVARCHAR(100) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    Severity INT NOT NULL,
    CurrentValue FLOAT NOT NULL,
    ThresholdValue FLOAT NOT NULL,
    TriggeredAt DATETIME2 NOT NULL,
    IsAcknowledged BIT NOT NULL DEFAULT 0,
    AcknowledgedAt DATETIME2 NULL,
    AcknowledgedBy NVARCHAR(100) NULL,
    INDEX IX_DiagnosticAlerts_TriggeredAt (TriggeredAt DESC),
    INDEX IX_DiagnosticAlerts_IsAcknowledged (IsAcknowledged)
);

-- Insert default alert thresholds
INSERT INTO AlertThresholds (MetricName, DisplayName, WarningThreshold, CriticalThreshold, IsEnabled, CooldownMinutes, CreatedAt)
VALUES 
    ('CpuUsage', 'CPU Usage', 75, 90, 1, 15, GETUTCDATE()),
    ('MemoryUsage', 'Memory Usage', 75, 90, 1, 15, GETUTCDATE()),
    ('DatabaseResponseTime', 'Database Response Time', 1000, 5000, 1, 15, GETUTCDATE()),
    ('StorageUsage', 'Storage Usage', 85, 95, 1, 30, GETUTCDATE());
```

---

### **Task 3: Create Performance History Service** 📈

**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/PerformanceHistoryService.cs` (NEW)

```csharp
using GFC.BlazorServer.Data;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Diagnostics;

/// <summary>
/// Service for tracking and retrieving performance history
/// </summary>
public class PerformanceHistoryService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<PerformanceHistoryService> _logger;
    
    public PerformanceHistoryService(
        GfcDbContext dbContext,
        ILogger<PerformanceHistoryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task RecordSnapshotAsync(PerformanceMetrics metrics, DatabaseHealthInfo dbHealth, HealthStatus overallHealth)
    {
        try
        {
            var snapshot = new PerformanceSnapshot
            {
                Timestamp = DateTime.UtcNow,
                CpuUsagePercent = metrics.CpuUsage,
                MemoryUsedBytes = (long)metrics.MemoryUsageGb * 1024 * 1024 * 1024,
                MemoryUsagePercent = metrics.MemoryUsagePercentage,
                ActiveConnections = metrics.ActiveThreads,
                RequestsPerMinute = metrics.RequestsPerMinute,
                ErrorCount = 0, // TODO: Get from error tracking
                DatabaseResponseTimeMs = dbHealth.ConnectionResponseTimeMs,
                DatabaseActiveConnections = dbHealth.ActiveConnections,
                OverallHealth = overallHealth
            };
            
            _dbContext.PerformanceSnapshots.Add(snapshot);
            await _dbContext.SaveChangesAsync();
            
            // Clean up old snapshots (keep last 7 days)
            await CleanupOldSnapshotsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording performance snapshot");
        }
    }
    
    public async Task<List<PerformanceSnapshot>> GetHistoryAsync(TimeSpan duration, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow - duration;
        
        return await _dbContext.PerformanceSnapshots
            .Where(s => s.Timestamp >= startTime)
            .OrderBy(s => s.Timestamp)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<PerformanceSnapshot>> GetLast24HoursAsync(CancellationToken cancellationToken = default)
    {
        return await GetHistoryAsync(TimeSpan.FromHours(24), cancellationToken);
    }
    
    public async Task<List<PerformanceSnapshot>> GetLast7DaysAsync(CancellationToken cancellationToken = default)
    {
        return await GetHistoryAsync(TimeSpan.FromDays(7), cancellationToken);
    }
    
    private async Task CleanupOldSnapshotsAsync()
    {
        try
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-7);
            var oldSnapshots = await _dbContext.PerformanceSnapshots
                .Where(s => s.Timestamp < cutoffDate)
                .ToListAsync();
            
            if (oldSnapshots.Any())
            {
                _dbContext.PerformanceSnapshots.RemoveRange(oldSnapshots);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} old performance snapshots", oldSnapshots.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old snapshots");
        }
    }
}
```

---

### **Task 4: Create Alert Management Service** 🚨

**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/AlertManagementService.cs` (NEW)

```csharp
using GFC.BlazorServer.Data;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Diagnostics;

/// <summary>
/// Service for managing diagnostic alerts and thresholds
/// </summary>
public class AlertManagementService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<AlertManagementService> _logger;
    
    public AlertManagementService(
        GfcDbContext dbContext,
        ILogger<AlertManagementService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task CheckThresholdsAsync(PerformanceMetrics metrics, DatabaseHealthInfo dbHealth)
    {
        var thresholds = await _dbContext.AlertThresholds
            .Where(t => t.IsEnabled)
            .ToListAsync();
        
        foreach (var threshold in thresholds)
        {
            await CheckMetricThresholdAsync(threshold, metrics, dbHealth);
        }
    }
    
    private async Task CheckMetricThresholdAsync(AlertThreshold threshold, PerformanceMetrics metrics, DatabaseHealthInfo dbHealth)
    {
        double? currentValue = threshold.MetricName switch
        {
            "CpuUsage" => metrics.CpuUsage,
            "MemoryUsage" => metrics.MemoryUsagePercentage,
            "DatabaseResponseTime" => dbHealth.ConnectionResponseTimeMs,
            _ => null
        };
        
        if (!currentValue.HasValue)
            return;
        
        // Check if in cooldown period
        if (threshold.LastTriggeredAt.HasValue)
        {
            var timeSinceLastTrigger = DateTime.UtcNow - threshold.LastTriggeredAt.Value;
            if (timeSinceLastTrigger.TotalMinutes < threshold.CooldownMinutes)
                return;
        }
        
        HealthStatus? severity = null;
        double? thresholdValue = null;
        
        if (threshold.CriticalThreshold.HasValue && currentValue >= threshold.CriticalThreshold.Value)
        {
            severity = HealthStatus.Critical;
            thresholdValue = threshold.CriticalThreshold.Value;
        }
        else if (threshold.WarningThreshold.HasValue && currentValue >= threshold.WarningThreshold.Value)
        {
            severity = HealthStatus.Warning;
            thresholdValue = threshold.WarningThreshold.Value;
        }
        
        if (severity.HasValue && thresholdValue.HasValue)
        {
            await CreateAlertAsync(threshold, currentValue.Value, thresholdValue.Value, severity.Value);
            
            // Update last triggered time
            threshold.LastTriggeredAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }
    
    private async Task CreateAlertAsync(AlertThreshold threshold, double currentValue, double thresholdValue, HealthStatus severity)
    {
        var alert = new DiagnosticAlert
        {
            MetricName = threshold.MetricName,
            Message = $"{threshold.DisplayName} has exceeded {severity} threshold: {currentValue:N2} (threshold: {thresholdValue:N2})",
            Severity = severity,
            CurrentValue = currentValue,
            ThresholdValue = thresholdValue,
            TriggeredAt = DateTime.UtcNow
        };
        
        _dbContext.DiagnosticAlerts.Add(alert);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogWarning("Alert triggered: {Message}", alert.Message);
    }
    
    public async Task<List<DiagnosticAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.DiagnosticAlerts
            .Where(a => !a.IsAcknowledged)
            .OrderByDescending(a => a.TriggeredAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<DiagnosticAlert>> GetRecentAlertsAsync(int hours = 24, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddHours(-hours);
        
        return await _dbContext.DiagnosticAlerts
            .Where(a => a.TriggeredAt >= cutoff)
            .OrderByDescending(a => a.TriggeredAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task AcknowledgeAlertAsync(int alertId, string acknowledgedBy)
    {
        var alert = await _dbContext.DiagnosticAlerts.FindAsync(alertId);
        if (alert != null && !alert.IsAcknowledged)
        {
            alert.IsAcknowledged = true;
            alert.AcknowledgedAt = DateTime.UtcNow;
            alert.AcknowledgedBy = acknowledgedBy;
            await _dbContext.SaveChangesAsync();
        }
    }
}
```

---

### **Task 5: Create Background Monitoring Service** ⏰

**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/DiagnosticsBackgroundService.cs` (NEW)

```csharp
using GFC.BlazorServer.Services.Diagnostics;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Background service for periodic diagnostics monitoring
/// </summary>
public class DiagnosticsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DiagnosticsBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);
    
    public DiagnosticsBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<DiagnosticsBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Diagnostics background service started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CollectDiagnosticsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in diagnostics background service");
            }
            
            await Task.Delay(_interval, stoppingToken);
        }
        
        _logger.LogInformation("Diagnostics background service stopped");
    }
    
    private async Task CollectDiagnosticsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var performanceService = scope.ServiceProvider.GetRequiredService<ISystemPerformanceService>();
        var databaseHealthService = scope.ServiceProvider.GetRequiredService<DatabaseHealthService>();
        var historyService = scope.ServiceProvider.GetRequiredService<PerformanceHistoryService>();
        var alertService = scope.ServiceProvider.GetRequiredService<AlertManagementService>();
        
        // Collect metrics
        var metrics = await performanceService.GetPerformanceMetricsAsync(cancellationToken);
        var dbHealth = await databaseHealthService.GetDatabaseHealthAsync(cancellationToken);
        
        // Determine overall health
        var overallHealth = CalculateOverallHealth(metrics, dbHealth);
        
        // Record snapshot
        await historyService.RecordSnapshotAsync(metrics, dbHealth, overallHealth);
        
        // Check alert thresholds
        await alertService.CheckThresholdsAsync(metrics, dbHealth);
        
        _logger.LogDebug("Diagnostics collected: CPU={Cpu}%, Memory={Memory}%, DB={DbMs}ms", 
            metrics.CpuUsage, metrics.MemoryUsagePercentage, dbHealth.ConnectionResponseTimeMs);
    }
    
    private HealthStatus CalculateOverallHealth(PerformanceMetrics metrics, DatabaseHealthInfo dbHealth)
    {
        if (metrics.CpuUsage > 90 || metrics.MemoryUsagePercentage > 90 || dbHealth.Status == HealthStatus.Critical)
            return HealthStatus.Critical;
        
        if (metrics.CpuUsage > 75 || metrics.MemoryUsagePercentage > 75 || dbHealth.Status == HealthStatus.Warning)
            return HealthStatus.Warning;
        
        return HealthStatus.Healthy;
    }
}
```

---

### **Task 6: Create Chart Components** 📊

#### 6.1 Create PerformanceChart Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/PerformanceChart.razor` (NEW)

```razor
@using GFC.Core.Models.Diagnostics
@inject IJSRuntime JS

<div class="metric-card">
    <div class="metric-card-header">
        <div class="metric-label">@Title</div>
        <div class="btn-group btn-group-sm">
            <button class="btn btn-outline-secondary @(TimeRange == "24h" ? "active" : "")" 
                    @onclick="() => ChangeTimeRange(\"24h\")">24h</button>
            <button class="btn btn-outline-secondary @(TimeRange == "7d" ? "active" : "")" 
                    @onclick="() => ChangeTimeRange(\"7d\")">7d</button>
        </div>
    </div>
    
    <div class="metric-content">
        <canvas id="@ChartId" style="max-height: 300px;"></canvas>
    </div>
</div>

@code {
    [Parameter] public string Title { get; set; } = "Performance";
    [Parameter] public List<PerformanceSnapshot> Data { get; set; } = new();
    [Parameter] public string MetricProperty { get; set; } = "CpuUsagePercent";
    [Parameter] public EventCallback<string> OnTimeRangeChanged { get; set; }
    
    private string ChartId = $"chart_{Guid.NewGuid():N}";
    private string TimeRange = "24h";
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender || Data.Any())
        {
            await RenderChartAsync();
        }
    }
    
    private async Task ChangeTimeRange(string range)
    {
        TimeRange = range;
        await OnTimeRangeChanged.InvokeAsync(range);
    }
    
    private async Task RenderChartAsync()
    {
        var labels = Data.Select(d => d.Timestamp.ToString("HH:mm")).ToArray();
        var values = Data.Select(d => GetMetricValue(d)).ToArray();
        
        await JS.InvokeVoidAsync("renderLineChart", ChartId, labels, values, Title);
    }
    
    private double GetMetricValue(PerformanceSnapshot snapshot)
    {
        return MetricProperty switch
        {
            "CpuUsagePercent" => snapshot.CpuUsagePercent,
            "MemoryUsagePercent" => snapshot.MemoryUsagePercent,
            "DatabaseResponseTimeMs" => snapshot.DatabaseResponseTimeMs,
            "RequestsPerMinute" => snapshot.RequestsPerMinute,
            _ => 0
        };
    }
}
```

#### 6.2 Add Chart.js JavaScript
**File**: `wwwroot/js/diagnostics-charts.js` (NEW)

```javascript
// Chart.js helper for diagnostics
window.renderLineChart = function(canvasId, labels, data, title) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;
    
    // Destroy existing chart if any
    if (ctx.chart) {
        ctx.chart.destroy();
    }
    
    ctx.chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: title,
                data: data,
                borderColor: 'rgb(75, 192, 192)',
                backgroundColor: 'rgba(75, 192, 192, 0.1)',
                tension: 0.4,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
};
```

**Add Chart.js CDN to `_Host.cshtml`**:

```html
<script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
<script src="~/js/diagnostics-charts.js"></script>
```

---

### **Task 7: Create Alert Display Component** 🚨

**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/AlertPanel.razor` (NEW)

```razor
@using GFC.Core.Models.Diagnostics

<div class="alert-panel">
    <div class="alert-panel-header">
        <h5>
            <i class="bi bi-exclamation-triangle me-2"></i>
            Active Alerts
            @if (Alerts.Any())
            {
                <span class="badge bg-danger ms-2">@Alerts.Count</span>
            }
        </h5>
    </div>
    
    <div class="alert-panel-body">
        @if (!Alerts.Any())
        {
            <div class="text-center text-muted py-4">
                <i class="bi bi-check-circle fs-1"></i>
                <p class="mt-2">No active alerts</p>
            </div>
        }
        else
        {
            @foreach (var alert in Alerts.OrderByDescending(a => a.TriggeredAt))
            {
                <div class="alert alert-@GetAlertClass(alert.Severity) alert-dismissible fade show" role="alert">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <strong>@alert.MetricName</strong>
                            <p class="mb-1">@alert.Message</p>
                            <small class="text-muted">
                                <i class="bi bi-clock me-1"></i>
                                @GetTimeAgo(alert.TriggeredAt)
                            </small>
                        </div>
                        <button type="button" class="btn btn-sm btn-outline-secondary" 
                                @onclick="() => OnAcknowledge.InvokeAsync(alert.Id)">
                            Acknowledge
                        </button>
                    </div>
                </div>
            }
        }
    </div>
</div>

@code {
    [Parameter] public List<DiagnosticAlert> Alerts { get; set; } = new();
    [Parameter] public EventCallback<int> OnAcknowledge { get; set; }
    
    private string GetAlertClass(HealthStatus severity) => severity switch
    {
        HealthStatus.Critical => "danger",
        HealthStatus.Warning => "warning",
        _ => "info"
    };
    
    private string GetTimeAgo(DateTime timestamp)
    {
        var timeSpan = DateTime.UtcNow - timestamp;
        
        if (timeSpan.TotalMinutes < 1)
            return "Just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes}m ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours}h ago";
        
        return $"{(int)timeSpan.TotalDays}d ago";
    }
}
```

---

### **Task 8: Update SystemDiagnostics Page** 🎨

**File**: `apps/webapp/GFC.BlazorServer/Components/Pages/SystemDiagnostics.razor` (UPDATE)

Add at the top of the page (after the header):

```razor
@inject PerformanceHistoryService HistoryService
@inject AlertManagementService AlertService

@* Add after page title *@

<!-- Active Alerts Section -->
@if (_activeAlerts.Any())
{
    <div class="row mb-4">
        <div class="col-12">
            <AlertPanel Alerts="@_activeAlerts" OnAcknowledge="@AcknowledgeAlert" />
        </div>
    </div>
}

<!-- Performance Charts Section -->
<div class="row mb-4">
    <div class="col-12">
        <h4 class="text-gradient-primary mb-3">
            <i class="bi bi-graph-up me-2"></i>
            Performance Trends
        </h4>
    </div>
</div>

<div class="row g-4 mb-5">
    <div class="col-md-6">
        <PerformanceChart 
            Title="CPU Usage (%)" 
            Data="@_performanceHistory" 
            MetricProperty="CpuUsagePercent"
            OnTimeRangeChanged="@LoadPerformanceHistory" />
    </div>
    
    <div class="col-md-6">
        <PerformanceChart 
            Title="Memory Usage (%)" 
            Data="@_performanceHistory" 
            MetricProperty="MemoryUsagePercent"
            OnTimeRangeChanged="@LoadPerformanceHistory" />
    </div>
    
    <div class="col-md-6">
        <PerformanceChart 
            Title="Database Response Time (ms)" 
            Data="@_performanceHistory" 
            MetricProperty="DatabaseResponseTimeMs"
            OnTimeRangeChanged="@LoadPerformanceHistory" />
    </div>
    
    <div class="col-md-6">
        <PerformanceChart 
            Title="Requests Per Minute" 
            Data="@_performanceHistory" 
            MetricProperty="RequestsPerMinute"
            OnTimeRangeChanged="@LoadPerformanceHistory" />
    </div>
</div>
```

Add to @code section:

```csharp
private List<PerformanceSnapshot> _performanceHistory = new();
private List<DiagnosticAlert> _activeAlerts = new();

protected override async Task OnInitializedAsync()
{
    await base.OnInitializedAsync();
    await LoadPerformanceHistory("24h");
    await LoadActiveAlerts();
}

private async Task LoadPerformanceHistory(string timeRange)
{
    _performanceHistory = timeRange == "7d" 
        ? await HistoryService.GetLast7DaysAsync()
        : await HistoryService.GetLast24HoursAsync();
    
    StateHasChanged();
}

private async Task LoadActiveAlerts()
{
    _activeAlerts = await AlertService.GetActiveAlertsAsync();
    StateHasChanged();
}

private async Task AcknowledgeAlert(int alertId)
{
    // TODO: Get current user
    await AlertService.AcknowledgeAlertAsync(alertId, "admin");
    await LoadActiveAlerts();
}
```

---

### **Task 9: Register Services** 📦

**File**: `apps/webapp/GFC.BlazorServer/Program.cs` (UPDATE)

```csharp
// Add these service registrations
builder.Services.AddScoped<GFC.BlazorServer.Services.Diagnostics.PerformanceHistoryService>();
builder.Services.AddScoped<GFC.BlazorServer.Services.Diagnostics.AlertManagementService>();
builder.Services.AddHostedService<GFC.BlazorServer.Services.DiagnosticsBackgroundService>();
```

---

## Testing Checklist

- [ ] Performance snapshots are recorded every 5 minutes
- [ ] Charts display correctly with 24h/7d toggle
- [ ] Alerts trigger when thresholds are exceeded
- [ ] Alert acknowledgment works
- [ ] Background service runs without errors
- [ ] Old snapshots are cleaned up (7-day retention)
- [ ] All animations work smoothly
- [ ] No performance degradation

---

## Files to Create

1. ✅ `GFC.Core/Models/Diagnostics/PerformanceSnapshot.cs`
2. ✅ `GFC.Core/Models/Diagnostics/AlertThreshold.cs`
3. ✅ `GFC.Core/Models/Diagnostics/DiagnosticAlert.cs`
4. ✅ `Services/Diagnostics/PerformanceHistoryService.cs`
5. ✅ `Services/Diagnostics/AlertManagementService.cs`
6. ✅ `Services/DiagnosticsBackgroundService.cs`
7. ✅ `Components/Shared/Diagnostics/PerformanceChart.razor`
8. ✅ `Components/Shared/Diagnostics/AlertPanel.razor`
9. ✅ `wwwroot/js/diagnostics-charts.js`

## Files to Update

1. ✅ `GfcDbContext.cs` (add DbSets)
2. ✅ `Components/Pages/SystemDiagnostics.razor`
3. ✅ `Program.cs`
4. ✅ `Pages/_Host.cshtml` (add Chart.js)

## Database Migration

- Run SQL script to create new tables
- Insert default alert thresholds

---

## Success Criteria

✅ **Historical Tracking**: Performance data stored and retrievable
✅ **Trend Visualization**: Interactive charts showing metrics over time
✅ **Alert System**: Automatic threshold monitoring with notifications
✅ **Background Monitoring**: Periodic data collection without user interaction
✅ **Clean UI**: Polished interface with smooth animations
✅ **Performance**: No impact on application performance

---

## Estimated Time

- **Task 1** (Models): 30 minutes
- **Task 2** (Database): 30 minutes
- **Task 3** (History Service): 1 hour
- **Task 4** (Alert Service): 1 hour
- **Task 5** (Background Service): 45 minutes
- **Task 6** (Charts): 1.5 hours
- **Task 7** (Alert Panel): 45 minutes
- **Task 8** (Update Page): 45 minutes
- **Task 9** (Registration): 10 minutes
- **Testing**: 1 hour

**Total**: ~8 hours

---

## Optional Enhancements (Future)

- Export diagnostics report as PDF
- Email alerts for critical issues
- Custom alert threshold configuration UI
- Performance comparison (current vs historical)
- System health score calculation
- Predictive analytics for resource usage

---

**Status**: Ready for Implementation
**Priority**: High
**Dependencies**: Phases 1, 2, & 3 completed

**This is the FINAL phase of the System Diagnostics Modernization!** 🎉
