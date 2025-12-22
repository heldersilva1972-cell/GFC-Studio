# Phase 3: Hardware Controller & Camera System Diagnostics

## Overview
Expand the diagnostics system to monitor hardware controllers and the camera system. This phase adds real-time monitoring of access control hardware and camera infrastructure, providing visibility into device connectivity and performance.

---

## Goals
- ✅ Add hardware controller diagnostics and monitoring
- ✅ Implement camera system health checks
- ✅ Create device connectivity status tracking
- ✅ Add controller communication metrics
- ✅ Implement camera stream monitoring

---

## Prerequisites
- ✅ Phase 1 completed (Foundation & UI components)
- ✅ Phase 2 completed (Performance & Database health)
- ✅ All existing components working properly

---

## Implementation Tasks

### **Task 1: Create Controller Diagnostics Models** 📦

#### 1.1 Create ControllerHealthInfo Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/ControllerHealthInfo.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Hardware controller health and status information
/// </summary>
public class ControllerHealthInfo
{
    // Controller identification
    public uint ControllerSerialNumber { get; set; }
    public string ControllerName { get; set; } = string.Empty;
    
    // Connection status
    public bool IsReachable { get; set; }
    public DateTime? LastCommunication { get; set; }
    public double ResponseTimeMs { get; set; }
    
    // Controller information
    public string FirmwareVersion { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    
    // Device counts
    public int TotalDoors { get; set; }
    public int TotalReaders { get; set; }
    public int ActiveCards { get; set; }
    
    // Status
    public HealthStatus OverallHealth { get; set; } = HealthStatus.Unknown;
    public string? ErrorMessage { get; set; }
    
    // Metrics
    public int EventsLast24Hours { get; set; }
    public DateTime? LastEventTime { get; set; }
    
    // Timestamps
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    
    // Helper properties
    public TimeSpan? TimeSinceLastCommunication => LastCommunication.HasValue 
        ? DateTime.UtcNow - LastCommunication.Value 
        : null;
    
    public string GetCommunicationStatusText()
    {
        if (!IsReachable)
            return "Offline";
        
        if (!LastCommunication.HasValue)
            return "Unknown";
        
        var timeSince = TimeSinceLastCommunication!.Value;
        
        if (timeSince.TotalMinutes < 1)
            return "Just now";
        if (timeSince.TotalMinutes < 60)
            return $"{(int)timeSince.TotalMinutes}m ago";
        if (timeSince.TotalHours < 24)
            return $"{(int)timeSince.TotalHours}h ago";
        
        return $"{(int)timeSince.TotalDays}d ago";
    }
    
    public HealthStatus GetCommunicationHealth()
    {
        if (!IsReachable)
            return HealthStatus.Critical;
        
        if (!LastCommunication.HasValue)
            return HealthStatus.Unknown;
        
        var timeSince = TimeSinceLastCommunication!.Value;
        
        if (timeSince.TotalMinutes > 30)
            return HealthStatus.Critical;
        if (timeSince.TotalMinutes > 10)
            return HealthStatus.Warning;
        
        return HealthStatus.Healthy;
    }
}
```

#### 1.2 Create CameraSystemInfo Model
**File**: `apps/webapp/GFC.Core/Models/Diagnostics/CameraSystemInfo.cs` (NEW)

```csharp
namespace GFC.Core.Models.Diagnostics;

/// <summary>
/// Camera system health and status information
/// </summary>
public class CameraSystemInfo
{
    // System overview
    public int TotalCameras { get; set; }
    public int OnlineCameras { get; set; }
    public int OfflineCameras { get; set; }
    public int ActiveStreams { get; set; }
    
    // Storage
    public long RecordingStorageUsedBytes { get; set; }
    public long RecordingStorageTotalBytes { get; set; }
    public double StorageUsagePercent => RecordingStorageTotalBytes > 0 
        ? (double)RecordingStorageUsedBytes / RecordingStorageTotalBytes * 100 
        : 0;
    
    // NVR status
    public bool NvrReachable { get; set; }
    public string NvrAddress { get; set; } = string.Empty;
    public double NvrResponseTimeMs { get; set; }
    
    // Recording status
    public int CamerasRecording { get; set; }
    public DateTime? OldestRecording { get; set; }
    public DateTime? NewestRecording { get; set; }
    
    // Events
    public int EventsLast24Hours { get; set; }
    public DateTime? LastEventTime { get; set; }
    
    // Health status
    public HealthStatus OverallHealth { get; set; } = HealthStatus.Unknown;
    public string? ErrorMessage { get; set; }
    
    // Timestamps
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    
    // Helper properties
    public double StorageUsedGB => RecordingStorageUsedBytes / 1024.0 / 1024.0 / 1024.0;
    public double StorageTotalGB => RecordingStorageTotalBytes / 1024.0 / 1024.0 / 1024.0;
    public double StorageAvailableGB => StorageTotalGB - StorageUsedGB;
    
    public HealthStatus GetStorageHealth()
    {
        if (StorageUsagePercent > 95)
            return HealthStatus.Critical;
        if (StorageUsagePercent > 85)
            return HealthStatus.Warning;
        
        return HealthStatus.Healthy;
    }
    
    public string GetStorageStatusText()
    {
        if (StorageUsagePercent > 95)
            return "Storage critically low";
        if (StorageUsagePercent > 85)
            return "Storage running low";
        
        return "Storage healthy";
    }
}
```

---

### **Task 2: Create Diagnostics Services** ⚙️

#### 2.1 Create ControllerDiagnosticsService
**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/ControllerDiagnosticsService.cs` (NEW)

```csharp
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services.Controllers;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Diagnostics;

/// <summary>
/// Service for monitoring hardware controller health
/// </summary>
public class ControllerDiagnosticsService
{
    private readonly GfcDbContext _dbContext;
    private readonly IControllerClient _controllerClient;
    private readonly ILogger<ControllerDiagnosticsService> _logger;
    
    public ControllerDiagnosticsService(
        GfcDbContext dbContext,
        IControllerClient controllerClient,
        ILogger<ControllerDiagnosticsService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<List<ControllerHealthInfo>> GetAllControllersHealthAsync(CancellationToken cancellationToken = default)
    {
        var healthInfos = new List<ControllerHealthInfo>();
        
        try
        {
            // Get all registered controllers from database
            var controllers = await _dbContext.Controllers
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            foreach (var controller in controllers)
            {
                var healthInfo = await GetControllerHealthAsync(controller.SerialNumber, cancellationToken);
                healthInfos.Add(healthInfo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all controllers health");
        }
        
        return healthInfos;
    }
    
    public async Task<ControllerHealthInfo> GetControllerHealthAsync(uint serialNumber, CancellationToken cancellationToken = default)
    {
        var healthInfo = new ControllerHealthInfo
        {
            ControllerSerialNumber = serialNumber,
            CollectedAt = DateTime.UtcNow
        };
        
        try
        {
            // Get controller from database
            var controller = await _dbContext.Controllers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.SerialNumber == serialNumber, cancellationToken);
            
            if (controller == null)
            {
                healthInfo.OverallHealth = HealthStatus.Unknown;
                healthInfo.ErrorMessage = "Controller not found in database";
                return healthInfo;
            }
            
            healthInfo.ControllerName = controller.Name ?? $"Controller {serialNumber}";
            healthInfo.IpAddress = controller.IpAddress ?? "Unknown";
            healthInfo.Port = controller.Port;
            
            // Test connectivity
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var isReachable = await _controllerClient.PingAsync(cancellationToken);
                stopwatch.Stop();
                
                healthInfo.IsReachable = isReachable;
                healthInfo.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                
                if (isReachable)
                {
                    healthInfo.LastCommunication = DateTime.UtcNow;
                    
                    // Get run status for additional info
                    var runStatus = await _controllerClient.GetRunStatusAsync(serialNumber, cancellationToken);
                    if (runStatus != null)
                    {
                        // Extract firmware version if available
                        // Note: Adjust based on actual AgentRunStatusDto structure
                        healthInfo.FirmwareVersion = "Available"; // Placeholder
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                healthInfo.IsReachable = false;
                healthInfo.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                healthInfo.ErrorMessage = ex.Message;
            }
            
            // Get device counts from database
            // Note: Adjust based on your actual database schema
            healthInfo.TotalDoors = 4; // Placeholder - get from controller config
            healthInfo.TotalReaders = 4; // Placeholder - get from controller config
            
            // Get active cards count
            // Note: This would come from your card management system
            healthInfo.ActiveCards = 0; // Placeholder
            
            // Get recent events
            var last24Hours = DateTime.UtcNow.AddHours(-24);
            healthInfo.EventsLast24Hours = await _dbContext.ControllerEvents
                .Where(e => e.ControllerSn == serialNumber && e.Timestamp >= last24Hours)
                .CountAsync(cancellationToken);
            
            healthInfo.LastEventTime = await _dbContext.ControllerEvents
                .Where(e => e.ControllerSn == serialNumber)
                .OrderByDescending(e => e.Timestamp)
                .Select(e => e.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);
            
            // Calculate overall health
            healthInfo.OverallHealth = CalculateControllerHealth(healthInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting controller health for SN {SerialNumber}", serialNumber);
            healthInfo.OverallHealth = HealthStatus.Error;
            healthInfo.ErrorMessage = ex.Message;
        }
        
        return healthInfo;
    }
    
    private HealthStatus CalculateControllerHealth(ControllerHealthInfo info)
    {
        // Critical conditions
        if (!info.IsReachable)
            return HealthStatus.Critical;
        
        // Warning conditions
        if (info.ResponseTimeMs > 1000)
            return HealthStatus.Warning;
        
        var commHealth = info.GetCommunicationHealth();
        if (commHealth == HealthStatus.Critical || commHealth == HealthStatus.Warning)
            return commHealth;
        
        return HealthStatus.Healthy;
    }
    
    public async Task<DiagnosticActionResult> TestControllerConnectionAsync(uint serialNumber, CancellationToken cancellationToken = default)
    {
        var result = new DiagnosticActionResult
        {
            ActionName = $"Test Controller {serialNumber} Connection",
            ExecutedAt = DateTime.UtcNow
        };
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            var isReachable = await _controllerClient.PingAsync(cancellationToken);
            stopwatch.Stop();
            
            result.Success = isReachable;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = isReachable 
                ? $"Controller is reachable (responded in {result.ResponseTimeMs}ms)" 
                : "Controller is not reachable";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Controller connection test failed for SN {SerialNumber}", serialNumber);
            result.Success = false;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = $"Connection test failed: {ex.Message}";
        }
        
        return result;
    }
}
```

#### 2.2 Create CameraDiagnosticsService
**File**: `apps/webapp/GFC.BlazorServer/Services/Diagnostics/CameraDiagnosticsService.cs` (NEW)

```csharp
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services.Camera;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Diagnostics;

/// <summary>
/// Service for monitoring camera system health
/// </summary>
public class CameraDiagnosticsService
{
    private readonly GfcDbContext _dbContext;
    private readonly ICameraService _cameraService;
    private readonly ILogger<CameraDiagnosticsService> _logger;
    
    public CameraDiagnosticsService(
        GfcDbContext dbContext,
        ICameraService cameraService,
        ILogger<CameraDiagnosticsService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<CameraSystemInfo> GetCameraSystemHealthAsync(CancellationToken cancellationToken = default)
    {
        var systemInfo = new CameraSystemInfo
        {
            CollectedAt = DateTime.UtcNow
        };
        
        try
        {
            // Get all cameras
            var cameras = await _cameraService.GetAllCamerasAsync();
            
            systemInfo.TotalCameras = cameras.Count;
            systemInfo.OnlineCameras = cameras.Count(c => c.IsOnline);
            systemInfo.OfflineCameras = cameras.Count(c => !c.IsOnline);
            
            // Get active streams
            // Note: This would require tracking active stream connections
            systemInfo.ActiveStreams = 0; // Placeholder
            
            // Get recording status
            systemInfo.CamerasRecording = cameras.Count(c => c.IsRecording);
            
            // Get storage information
            // Note: This would come from your NVR or storage system
            systemInfo.RecordingStorageUsedBytes = 0; // Placeholder
            systemInfo.RecordingStorageTotalBytes = 1099511627776; // 1TB placeholder
            
            // Get NVR status
            // Note: Adjust based on your NVR configuration
            systemInfo.NvrReachable = true; // Placeholder
            systemInfo.NvrAddress = "nvr.local"; // Placeholder
            systemInfo.NvrResponseTimeMs = 0; // Placeholder
            
            // Get recording timestamps
            var recordings = await _dbContext.Recordings
                .AsNoTracking()
                .OrderBy(r => r.StartTime)
                .Select(r => r.StartTime)
                .ToListAsync(cancellationToken);
            
            if (recordings.Any())
            {
                systemInfo.OldestRecording = recordings.First();
                systemInfo.NewestRecording = recordings.Last();
            }
            
            // Get recent events
            var last24Hours = DateTime.UtcNow.AddHours(-24);
            systemInfo.EventsLast24Hours = await _dbContext.CameraEvents
                .Where(e => e.Timestamp >= last24Hours)
                .CountAsync(cancellationToken);
            
            systemInfo.LastEventTime = await _dbContext.CameraEvents
                .OrderByDescending(e => e.Timestamp)
                .Select(e => e.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);
            
            // Calculate overall health
            systemInfo.OverallHealth = CalculateCameraSystemHealth(systemInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting camera system health");
            systemInfo.OverallHealth = HealthStatus.Error;
            systemInfo.ErrorMessage = ex.Message;
        }
        
        return systemInfo;
    }
    
    private HealthStatus CalculateCameraSystemHealth(CameraSystemInfo info)
    {
        // Critical conditions
        if (info.TotalCameras > 0 && info.OnlineCameras == 0)
            return HealthStatus.Critical;
        
        if (!info.NvrReachable)
            return HealthStatus.Critical;
        
        var storageHealth = info.GetStorageHealth();
        if (storageHealth == HealthStatus.Critical)
            return HealthStatus.Critical;
        
        // Warning conditions
        if (info.OfflineCameras > 0)
            return HealthStatus.Warning;
        
        if (storageHealth == HealthStatus.Warning)
            return HealthStatus.Warning;
        
        return HealthStatus.Healthy;
    }
    
    public async Task<DiagnosticActionResult> TestCameraConnectionAsync(int cameraId, CancellationToken cancellationToken = default)
    {
        var result = new DiagnosticActionResult
        {
            ActionName = $"Test Camera {cameraId} Connection",
            ExecutedAt = DateTime.UtcNow
        };
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            var camera = await _cameraService.GetCameraByIdAsync(cameraId);
            if (camera == null)
            {
                result.Success = false;
                result.Message = "Camera not found";
                return result;
            }
            
            // Test camera connectivity
            // Note: Implement actual camera ping/test logic
            var isReachable = camera.IsOnline;
            stopwatch.Stop();
            
            result.Success = isReachable;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = isReachable 
                ? $"Camera '{camera.Name}' is reachable" 
                : $"Camera '{camera.Name}' is not reachable";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Camera connection test failed for ID {CameraId}", cameraId);
            result.Success = false;
            result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            result.Message = $"Connection test failed: {ex.Message}";
        }
        
        return result;
    }
}
```

---

### **Task 3: Update SystemDiagnosticsInfo Model** 📊

**File**: `apps/webapp/GFC.Core/Models/SystemDiagnosticsInfo.cs` (UPDATE)

Add these properties:

```csharp
// Add to existing SystemDiagnosticsInfo class

// NEW: Controller diagnostics
public List<ControllerHealthInfo> Controllers { get; set; } = new();
public int TotalControllers => Controllers.Count;
public int OnlineControllers => Controllers.Count(c => c.IsReachable);
public int OfflineControllers => Controllers.Count(c => !c.IsReachable);

// NEW: Camera system diagnostics
public CameraSystemInfo? CameraSystem { get; set; }
```

---

### **Task 4: Update DiagnosticsService** 🔄

**File**: `apps/webapp/GFC.BlazorServer/Services/DiagnosticsService.cs` (UPDATE)

Add these dependencies and methods:

```csharp
// Add to constructor parameters
private readonly ControllerDiagnosticsService _controllerDiagnosticsService;
private readonly CameraDiagnosticsService _cameraDiagnosticsService;

public DiagnosticsService(
    ISystemPerformanceService performanceService,
    DatabaseHealthService databaseHealthService,
    ControllerDiagnosticsService controllerDiagnosticsService,
    CameraDiagnosticsService cameraDiagnosticsService,
    IConfiguration configuration)
{
    _performanceService = performanceService;
    _databaseHealthService = databaseHealthService;
    _controllerDiagnosticsService = controllerDiagnosticsService ?? throw new ArgumentNullException(nameof(controllerDiagnosticsService));
    _cameraDiagnosticsService = cameraDiagnosticsService ?? throw new ArgumentNullException(nameof(cameraDiagnosticsService));
    _configuration = configuration;
}

// UPDATE GetDiagnosticsAsync to include new diagnostics
public async Task<SystemDiagnosticsInfo> GetDiagnosticsAsync()
{
    var diagnostics = new SystemDiagnosticsInfo
    {
        EnvironmentName = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production",
        ApplicationVersion = "1.0.0", // Get from version service
        CollectedAt = DateTime.UtcNow
    };

    // Existing diagnostics
    diagnostics.Performance = await _performanceService.GetPerformanceMetricsAsync();
    diagnostics.DatabaseHealth = await _databaseHealthService.GetDatabaseHealthAsync();
    
    // NEW: Controller diagnostics
    diagnostics.Controllers = await _controllerDiagnosticsService.GetAllControllersHealthAsync();
    
    // NEW: Camera system diagnostics
    diagnostics.CameraSystem = await _cameraDiagnosticsService.GetCameraSystemHealthAsync();
    
    // Calculate overall health
    CalculateOverallHealth(diagnostics);

    return diagnostics;
}

// ADD new action methods
public async Task<DiagnosticActionResult> TestControllerConnectionAsync(uint serialNumber, CancellationToken cancellationToken = default)
{
    return await _controllerDiagnosticsService.TestControllerConnectionAsync(serialNumber, cancellationToken);
}

public async Task<DiagnosticActionResult> TestCameraConnectionAsync(int cameraId, CancellationToken cancellationToken = default)
{
    return await _cameraDiagnosticsService.TestCameraConnectionAsync(cameraId, cancellationToken);
}
```

---

### **Task 5: Register Services in DI** 📦

**File**: `apps/webapp/GFC.BlazorServer/Program.cs` (UPDATE)

Add service registrations:

```csharp
// Add these service registrations
builder.Services.AddScoped<GFC.BlazorServer.Services.Diagnostics.ControllerDiagnosticsService>();
builder.Services.AddScoped<GFC.BlazorServer.Services.Diagnostics.CameraDiagnosticsService>();
```

---

### **Task 6: Create UI Components** 🎨

#### 6.1 Create ControllerHealthCard Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/ControllerHealthCard.razor` (NEW)

```razor
@using GFC.Core.Models.Diagnostics

<div class="metric-card">
    <div class="metric-card-header">
        <div class="metric-icon @GetHealthIconColor()">
            <i class="bi bi-hdd-network"></i>
        </div>
        <StatusBadge Status="@Controller.OverallHealth" />
    </div>
    
    <div class="metric-content">
        <div class="metric-label">@Controller.ControllerName</div>
        
        @if (IsLoading)
        {
            <SkeletonLoader Type="text" Size="small" />
            <SkeletonLoader Type="text" Size="large" />
        }
        else
        {
            <div class="info-row">
                <span class="info-label">Serial Number</span>
                <span class="info-value">@Controller.ControllerSerialNumber</span>
            </div>
            
            <div class="info-row">
                <span class="info-label">IP Address</span>
                <span class="info-value">
                    @Controller.IpAddress:@Controller.Port
                    <CopyButton TextToCopy="@Controller.IpAddress" ShowText="false" />
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Status</span>
                <span class="info-value">
                    @if (Controller.IsReachable)
                    {
                        <span class="text-success">
                            <i class="bi bi-check-circle-fill"></i> Online
                        </span>
                    }
                    else
                    {
                        <span class="text-danger">
                            <i class="bi bi-x-circle-fill"></i> Offline
                        </span>
                    }
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Last Communication</span>
                <span class="info-value">
                    <StatusBadge Status="@Controller.GetCommunicationHealth()" 
                                 CustomText="@Controller.GetCommunicationStatusText()" />
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Response Time</span>
                <span class="info-value">@Controller.ResponseTimeMs ms</span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Devices</span>
                <span class="info-value">
                    @Controller.TotalDoors doors, @Controller.TotalReaders readers
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Events (24h)</span>
                <span class="info-value">@Controller.EventsLast24Hours</span>
            </div>
        }
    </div>
    
    @if (ShowActions)
    {
        <div class="metric-card-footer mt-3">
            <button class="btn-diagnostic secondary w-100" @onclick="OnTestConnection">
                <i class="bi bi-plug"></i>
                <text> Test Connection</text>
            </button>
        </div>
    }
</div>

@code {
    [Parameter] public ControllerHealthInfo Controller { get; set; } = new();
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public bool ShowActions { get; set; } = true;
    [Parameter] public EventCallback OnTestConnection { get; set; }
    
    private string GetHealthIconColor() => Controller.OverallHealth switch
    {
        HealthStatus.Healthy => "success",
        HealthStatus.Warning => "warning",
        HealthStatus.Error => "danger",
        HealthStatus.Critical => "danger",
        _ => "info"
    };
}
```

#### 6.2 Create CameraSystemCard Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/CameraSystemCard.razor` (NEW)

```razor
@using GFC.Core.Models.Diagnostics

<div class="metric-card">
    <div class="metric-card-header">
        <div class="metric-icon @GetHealthIconColor()">
            <i class="bi bi-camera-video"></i>
        </div>
        <StatusBadge Status="@CameraSystem.OverallHealth" />
    </div>
    
    <div class="metric-content">
        <div class="metric-label">Camera System</div>
        
        @if (IsLoading)
        {
            <SkeletonLoader Type="text" Size="small" />
            <SkeletonLoader Type="text" Size="large" />
        }
        else
        {
            <div class="row g-2 mb-3">
                <div class="col-4 text-center">
                    <div class="metric-value">@CameraSystem.TotalCameras</div>
                    <div class="metric-subtext">Total</div>
                </div>
                <div class="col-4 text-center">
                    <div class="metric-value text-success">@CameraSystem.OnlineCameras</div>
                    <div class="metric-subtext">Online</div>
                </div>
                <div class="col-4 text-center">
                    <div class="metric-value text-danger">@CameraSystem.OfflineCameras</div>
                    <div class="metric-subtext">Offline</div>
                </div>
            </div>
            
            <div class="info-row">
                <span class="info-label">Active Streams</span>
                <span class="info-value">@CameraSystem.ActiveStreams</span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Recording</span>
                <span class="info-value">@CameraSystem.CamerasRecording cameras</span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Storage</span>
                <span class="info-value">
                    <StatusBadge Status="@CameraSystem.GetStorageHealth()" 
                                 CustomText="@($"{CameraSystem.StorageUsedGB:N1} / {CameraSystem.StorageTotalGB:N1} GB")" />
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Storage Usage</span>
                <span class="info-value">
                    @CameraSystem.StorageUsagePercent.ToString("N1")%
                    <div class="progress mt-1" style="height: 6px;">
                        <div class="progress-bar @GetStorageProgressColor()" 
                             role="progressbar" 
                             style="width: @CameraSystem.StorageUsagePercent%"
                             aria-valuenow="@CameraSystem.StorageUsagePercent" 
                             aria-valuemin="0" 
                             aria-valuemax="100"></div>
                    </div>
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">NVR Status</span>
                <span class="info-value">
                    @if (CameraSystem.NvrReachable)
                    {
                        <span class="text-success">
                            <i class="bi bi-check-circle-fill"></i> Online
                        </span>
                    }
                    else
                    {
                        <span class="text-danger">
                            <i class="bi bi-x-circle-fill"></i> Offline
                        </span>
                    }
                </span>
            </div>
            
            <div class="info-row">
                <span class="info-label">Events (24h)</span>
                <span class="info-value">@CameraSystem.EventsLast24Hours</span>
            </div>
        }
    </div>
</div>

@code {
    [Parameter] public CameraSystemInfo CameraSystem { get; set; } = new();
    [Parameter] public bool IsLoading { get; set; }
    
    private string GetHealthIconColor() => CameraSystem.OverallHealth switch
    {
        HealthStatus.Healthy => "success",
        HealthStatus.Warning => "warning",
        HealthStatus.Error => "danger",
        HealthStatus.Critical => "danger",
        _ => "info"
    };
    
    private string GetStorageProgressColor() => CameraSystem.GetStorageHealth() switch
    {
        HealthStatus.Critical => "bg-danger",
        HealthStatus.Warning => "bg-warning",
        _ => "bg-success"
    };
}
```

---

### **Task 7: Update SystemDiagnostics Page** 🎨

**File**: `apps/webapp/GFC.BlazorServer/Components/Pages/SystemDiagnostics.razor` (UPDATE)

Add new sections after the Database Health section:

```razor
@* Add after the Database Health Section *@

<!-- Hardware Controllers Section -->
<div class="row mt-5 mb-4">
    <div class="col-12">
        <h4 class="text-gradient-primary mb-3">
            <i class="bi bi-hdd-network me-2"></i>
            Hardware Controllers
            @if (_info?.Controllers != null && _info.Controllers.Any())
            {
                <span class="badge bg-secondary ms-2">
                    @_info.OnlineControllers / @_info.TotalControllers Online
                </span>
            }
        </h4>
    </div>
</div>

@if (_info?.Controllers != null && _info.Controllers.Any())
{
    <div class="row g-4">
        @foreach (var controller in _info.Controllers)
        {
            <div class="col-md-6 col-lg-4">
                <ControllerHealthCard 
                    Controller="@controller" 
                    IsLoading="@_isRefreshing"
                    OnTestConnection="@(() => TestControllerConnection(controller.ControllerSerialNumber))" />
            </div>
        }
    </div>
}
else
{
    <div class="alert alert-info">
        <i class="bi bi-info-circle me-2"></i>
        No controllers registered in the system.
    </div>
}

<!-- Camera System Section -->
<div class="row mt-5 mb-4">
    <div class="col-12">
        <h4 class="text-gradient-primary mb-3">
            <i class="bi bi-camera-video me-2"></i>
            Camera System
        </h4>
    </div>
</div>

@if (_info?.CameraSystem != null)
{
    <div class="row g-4">
        <div class="col-md-6">
            <CameraSystemCard 
                CameraSystem="@_info.CameraSystem" 
                IsLoading="@_isRefreshing" />
        </div>
    </div>
}
else
{
    <div class="alert alert-info">
        <i class="bi bi-info-circle me-2"></i>
        Camera system not configured.
    </div>
}
```

Add to @code section:

```csharp
private async Task TestControllerConnection(uint serialNumber)
{
    var result = await DiagnosticsService.TestControllerConnectionAsync(serialNumber);
    _actionResults.Add(result);
    StateHasChanged();
}
```

---

## Testing Checklist

After implementation, verify:

- [ ] Controller health cards display correctly
- [ ] Camera system card shows all metrics
- [ ] Controller connection status updates
- [ ] Camera online/offline counts are accurate
- [ ] Storage usage displays with progress bar
- [ ] Test connection buttons work
- [ ] Action results display properly
- [ ] All animations work smoothly
- [ ] No compilation errors
- [ ] Page loads without errors

---

## Files to Create

1. ✅ `apps/webapp/GFC.Core/Models/Diagnostics/ControllerHealthInfo.cs`
2. ✅ `apps/webapp/GFC.Core/Models/Diagnostics/CameraSystemInfo.cs`
3. ✅ `apps/webapp/GFC.BlazorServer/Services/Diagnostics/ControllerDiagnosticsService.cs`
4. ✅ `apps/webapp/GFC.BlazorServer/Services/Diagnostics/CameraDiagnosticsService.cs`
5. ✅ `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/ControllerHealthCard.razor`
6. ✅ `apps/webapp/GFC.BlazorServer/Components/Shared/Diagnostics/CameraSystemCard.razor`

## Files to Update

1. ✅ `apps/webapp/GFC.Core/Models/SystemDiagnosticsInfo.cs`
2. ✅ `apps/webapp/GFC.BlazorServer/Services/DiagnosticsService.cs`
3. ✅ `apps/webapp/GFC.BlazorServer/Components/Pages/SystemDiagnostics.razor`
4. ✅ `apps/webapp/GFC.BlazorServer/Program.cs`

---

## Success Criteria

✅ **Controller Monitoring**: Real-time status of all hardware controllers
✅ **Camera System Health**: Comprehensive camera system diagnostics
✅ **Device Connectivity**: Visual indicators for online/offline status
✅ **Storage Monitoring**: Camera storage usage with warnings
✅ **Test Actions**: Ability to test individual controller/camera connections
✅ **Visual Feedback**: Color-coded health indicators and progress bars

---

## Estimated Time

- **Task 1** (Models): 30 minutes
- **Task 2** (Services): 2 hours
- **Task 3** (Update Model): 10 minutes
- **Task 4** (Update DiagnosticsService): 30 minutes
- **Task 5** (DI Registration): 5 minutes
- **Task 6** (Components): 1.5 hours
- **Task 7** (Update Page): 45 minutes
- **Testing**: 45 minutes

**Total**: ~6 hours

---

## Notes

- **Placeholders**: Some values are placeholders and should be replaced with actual data sources
- **Controller Data**: Adjust based on your actual controller database schema
- **Camera Integration**: Ensure camera service methods exist and work correctly
- **NVR Integration**: Add actual NVR connectivity checks if available
- **Storage Metrics**: Implement actual storage monitoring if possible

---

**Status**: Ready for Implementation
**Priority**: High
**Dependencies**: Phase 1 & 2 completed
