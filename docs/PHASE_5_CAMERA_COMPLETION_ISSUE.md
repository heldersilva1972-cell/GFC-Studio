# Phase 5: Camera System Completion - Playback, Recording & Management

## 📋 Issue Overview

**Phase:** 5 of 5 (FINAL)  
**Priority:** High  
**Estimated Effort:** 10-14 hours  
**Dependencies:** Phase 4 (Multi-Camera Grid & Controls) must be complete and merged

## 🎯 Objectives

Complete the camera system by adding:
1. **Event-based playback timeline** with scrubbing
2. **Video recording and archiving** capabilities
3. **Motion detection alerts** integration
4. **Camera management interface** (add/edit/delete cameras)
5. **Access control integration** (who can view which cameras)
6. **Audit logging** for all camera access
7. **Advanced analytics dashboard**

## 📦 Deliverables

### 1. Playback Timeline Component
**File:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraPlayback.razor`

```razor
@page "/cameras/playback/{CameraId:int}"
@using GFC.BlazorServer.Services.Camera
@inject ICameraService CameraService
@inject IEventService EventService
@inject IJSRuntime JS

<PageTitle>Camera Playback - @CameraName</PageTitle>

<div class="playback-container">
    <div class="playback-header">
        <h1>
            <i class="bi bi-play-circle"></i> Playback: @CameraName
        </h1>
        <div class="date-selector">
            <input type="date" class="form-control" @bind="SelectedDate" />
            <button class="btn btn-primary" @onclick="LoadEvents">
                <i class="bi bi-search"></i> Load Events
            </button>
        </div>
    </div>

    <div class="video-player-section">
        <video id="playback-player" class="playback-video" controls @ref="videoElement"></video>
        
        <div class="playback-controls">
            <button class="btn btn-sm btn-outline-primary" @onclick="() => SeekRelative(-10)">
                <i class="bi bi-skip-backward"></i> -10s
            </button>
            <button class="btn btn-sm btn-outline-primary" @onclick="TogglePlayPause">
                <i class="bi @(IsPlaying ? "bi-pause" : "bi-play")"></i>
            </button>
            <button class="btn btn-sm btn-outline-primary" @onclick="() => SeekRelative(10)">
                <i class="bi bi-skip-forward"></i> +10s
            </button>
            <select class="form-select form-select-sm ms-2" @bind="PlaybackSpeed">
                <option value="0.5">0.5x</option>
                <option value="1">1x</option>
                <option value="2">2x</option>
                <option value="4">4x</option>
            </select>
        </div>
    </div>

    <div class="timeline-section">
        <h5>Event Timeline</h5>
        <div class="timeline-container">
            @foreach (var evt in Events)
            {
                <div class="timeline-event @GetEventClass(evt)" 
                     @onclick="() => JumpToEvent(evt)">
                    <div class="event-time">@evt.Timestamp.ToLocalTime().ToString("HH:mm:ss")</div>
                    <div class="event-type">@evt.EventType</div>
                    <div class="event-description">@evt.Description</div>
                </div>
            }
        </div>
    </div>
</div>

@code {
    [Parameter] public int CameraId { get; set; }
    
    private string CameraName = "";
    private DateTime SelectedDate = DateTime.Today;
    private List<CameraEvent> Events = new();
    private ElementReference videoElement;
    private bool IsPlaying = false;
    private double PlaybackSpeed = 1.0;

    protected override async Task OnInitializedAsync()
    {
        var camera = await CameraService.GetCameraByIdAsync(CameraId);
        CameraName = camera?.Name ?? $"Camera {CameraId}";
        await LoadEvents();
    }

    private async Task LoadEvents()
    {
        Events = await EventService.GetCameraEventsAsync(CameraId, SelectedDate);
        StateHasChanged();
    }

    private async Task JumpToEvent(CameraEvent evt)
    {
        await JS.InvokeVoidAsync("PlaybackPlayer.seekToTime", evt.Timestamp);
    }

    private async Task SeekRelative(int seconds)
    {
        await JS.InvokeVoidAsync("PlaybackPlayer.seekRelative", seconds);
    }

    private async Task TogglePlayPause()
    {
        IsPlaying = !IsPlaying;
        await JS.InvokeVoidAsync("PlaybackPlayer.togglePlayPause");
    }

    private string GetEventClass(CameraEvent evt)
    {
        return evt.EventType switch
        {
            "Motion" => "event-motion",
            "Access" => "event-access",
            "Alert" => "event-alert",
            _ => "event-normal"
        };
    }
}
```

### 2. Recording Management Component
**File:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraRecordings.razor`

**Features:**
- List all recordings by camera and date
- Download recordings
- Delete old recordings
- Set recording schedules
- Configure retention policies

### 3. Camera Management Interface
**File:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraManagement.razor`

**Features:**
- Add new cameras
- Edit camera settings (name, location, RTSP URL)
- Delete cameras
- Test camera connectivity
- Configure recording settings
- Set access permissions

### 4. Motion Detection & Alerts
**File:** `apps/webapp/GFC.BlazorServer/Services/Camera/MotionDetectionService.cs`

**Features:**
- Real-time motion detection
- Alert notifications
- Integration with notification system
- Configurable sensitivity
- Zone-based detection

### 5. Camera Analytics Dashboard
**File:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraAnalytics.razor`

**Metrics:**
- Total viewing time per camera
- Most viewed cameras
- Peak viewing hours
- Motion detection statistics
- Recording storage usage
- System health metrics

### 6. Enhanced Services

#### ICameraService Extensions
```csharp
// Add to ICameraService.cs
Task<CameraInfo?> GetCameraByIdAsync(int cameraId);
Task<CameraInfo> AddCameraAsync(CameraInfo camera);
Task UpdateCameraAsync(CameraInfo camera);
Task DeleteCameraAsync(int cameraId);
Task<bool> TestCameraConnectionAsync(int cameraId);
Task<List<Recording>> GetRecordingsAsync(int cameraId, DateTime date);
Task<byte[]> DownloadRecordingAsync(int recordingId);
Task DeleteRecordingAsync(int recordingId);
Task<RecordingSettings> GetRecordingSettingsAsync(int cameraId);
Task UpdateRecordingSettingsAsync(int cameraId, RecordingSettings settings);
```

#### New Services
```csharp
// IEventService.cs
public interface IEventService
{
    Task<List<CameraEvent>> GetCameraEventsAsync(int cameraId, DateTime date);
    Task<CameraEvent> AddEventAsync(CameraEvent evt);
    Task<List<CameraEvent>> GetRecentEventsAsync(int cameraId, int count = 10);
}

// IMotionDetectionService.cs
public interface IMotionDetectionService
{
    Task<bool> EnableMotionDetectionAsync(int cameraId);
    Task<bool> DisableMotionDetectionAsync(int cameraId);
    Task<MotionSettings> GetMotionSettingsAsync(int cameraId);
    Task UpdateMotionSettingsAsync(int cameraId, MotionSettings settings);
}

// ICameraAuditService.cs
public interface ICameraAuditService
{
    Task LogAccessAsync(int cameraId, int userId, string action);
    Task<List<CameraAuditLog>> GetAuditLogsAsync(int cameraId, DateTime startDate, DateTime endDate);
}
```

### 7. Database Models

**File:** `apps/webapp/GFC.BlazorServer/Data/Entities/Camera.cs`
```csharp
public class Camera
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string RtspUrl { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public bool RecordingEnabled { get; set; } = false;
    public bool MotionDetectionEnabled { get; set; } = false;
    public DateTime CreatedUtc { get; set; }
    public DateTime? LastSeenUtc { get; set; }
    
    // Navigation properties
    public List<CameraEvent> Events { get; set; } = new();
    public List<Recording> Recordings { get; set; } = new();
    public List<CameraPermission> Permissions { get; set; } = new();
}

public class CameraEvent
{
    public int Id { get; set; }
    public int CameraId { get; set; }
    public string EventType { get; set; } = string.Empty; // Motion, Access, Alert
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? SnapshotPath { get; set; }
    
    public Camera Camera { get; set; } = null!;
}

public class Recording
{
    public int Id { get; set; }
    public int CameraId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public long FileSizeBytes { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string Format { get; set; } = "mp4";
    
    public Camera Camera { get; set; } = null!;
}

public class CameraPermission
{
    public int Id { get; set; }
    public int CameraId { get; set; }
    public int UserId { get; set; }
    public bool CanView { get; set; } = true;
    public bool CanPlayback { get; set; } = false;
    public bool CanDownload { get; set; } = false;
    public bool CanManage { get; set; } = false;
    
    public Camera Camera { get; set; } = null!;
    public User User { get; set; } = null!;
}

public class CameraAuditLog
{
    public int Id { get; set; }
    public int CameraId { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = string.Empty; // View, Playback, Download, etc.
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    
    public Camera Camera { get; set; } = null!;
    public User User { get; set; } = null!;
}
```

### 8. Database Migration

**File:** `apps/webapp/GFC.BlazorServer/Migrations/AddCameraSystemTables.cs`

Create migration for:
- Cameras table
- CameraEvents table
- Recordings table
- CameraPermissions table
- CameraAuditLogs table

### 9. Video Agent Enhancements

**File:** `apps/services/GFC.VideoAgent/Controllers/RecordingController.cs`

**New Endpoints:**
```csharp
// POST /api/recordings/start
// POST /api/recordings/stop
// GET /api/recordings/{cameraId}
// GET /api/recordings/{recordingId}/download
// DELETE /api/recordings/{recordingId}
// POST /api/motion-detection/enable
// POST /api/motion-detection/disable
// GET /api/motion-detection/{cameraId}/events
```

### 10. JavaScript Enhancements

**File:** `apps/webapp/GFC.BlazorServer/wwwroot/js/playback-player.js`

```javascript
window.PlaybackPlayer = {
    init: function(videoElementId, recordingUrl) {
        const video = document.getElementById(videoElementId);
        video.src = recordingUrl;
        return video;
    },
    
    seekToTime: function(timestamp) {
        // Seek to specific timestamp
    },
    
    seekRelative: function(seconds) {
        // Seek forward/backward by seconds
    },
    
    togglePlayPause: function() {
        // Toggle play/pause
    },
    
    setSpeed: function(speed) {
        // Set playback speed
    }
};
```

## 🧪 Testing Requirements

### Manual Testing
1. **Playback Timeline**
   - [ ] Load events for a specific date
   - [ ] Click on event to jump to that time
   - [ ] Test seek forward/backward
   - [ ] Test playback speed controls
   - [ ] Verify timeline displays correctly

2. **Recording Management**
   - [ ] Start/stop recording
   - [ ] List recordings by date
   - [ ] Download recording
   - [ ] Delete recording
   - [ ] Test retention policies

3. **Camera Management**
   - [ ] Add new camera
   - [ ] Edit camera settings
   - [ ] Delete camera
   - [ ] Test camera connectivity
   - [ ] Configure recording settings

4. **Motion Detection**
   - [ ] Enable motion detection
   - [ ] Verify alerts are triggered
   - [ ] Test sensitivity settings
   - [ ] Test zone-based detection

5. **Access Control**
   - [ ] Set camera permissions
   - [ ] Verify users can only access permitted cameras
   - [ ] Test permission levels (view, playback, download, manage)

6. **Audit Logging**
   - [ ] Verify all camera access is logged
   - [ ] View audit logs
   - [ ] Filter logs by date/user/camera

## 📊 Success Criteria

1. ✅ **Playback timeline works** with event markers and scrubbing
2. ✅ **Recordings can be started/stopped** manually or on schedule
3. ✅ **Motion detection triggers alerts** and creates events
4. ✅ **Cameras can be managed** (add/edit/delete) through UI
5. ✅ **Access control works** - users see only permitted cameras
6. ✅ **All camera access is audited** with complete logs
7. ✅ **Analytics dashboard shows** meaningful metrics
8. ✅ **Database migrations run** without errors
9. ✅ **Build passes** with 0 errors, 0 warnings
10. ✅ **All tests pass** on actual hardware

## ⚠️ Critical Requirements (DO NOT SKIP)

### 1. Database Migration
```bash
# Create migration
dotnet ef migrations add AddCameraSystem

# Apply migration
dotnet ef database update
```

### 2. Security
- **MUST** implement proper authorization checks
- **MUST** validate all user inputs
- **MUST** sanitize file paths for recordings
- **MUST** implement rate limiting for API endpoints
- **MUST** encrypt sensitive camera credentials

### 3. Performance
- **MUST** implement pagination for event lists
- **MUST** optimize database queries with indexes
- **MUST** implement caching for frequently accessed data
- **MUST** handle large recording files efficiently

### 4. Error Handling
- **MUST** handle camera disconnections gracefully
- **MUST** handle storage full scenarios
- **MUST** provide meaningful error messages
- **MUST** log all errors for debugging

### 5. Build Verification
```bash
dotnet build
# Must complete with 0 errors, 0 warnings
```

## 🔧 Configuration

**appsettings.json additions:**
```json
{
  "CameraSystem": {
    "RecordingsPath": "C:\\CameraRecordings",
    "MaxRecordingDurationMinutes": 60,
    "RetentionDays": 30,
    "MotionDetection": {
      "Enabled": true,
      "Sensitivity": 50,
      "MinEventInterval": 5
    },
    "Storage": {
      "MaxStorageGB": 500,
      "CleanupThresholdPercent": 90
    }
  }
}
```

## 📝 PR Requirements

**MANDATORY PR Description Format:**
```markdown
## Phase 5: Camera System Completion

### Changes Made
- [ ] Event-based playback timeline with scrubbing
- [ ] Video recording and archiving
- [ ] Motion detection alerts
- [ ] Camera management interface
- [ ] Access control integration
- [ ] Audit logging for camera access
- [ ] Analytics dashboard

### Database Changes
- [ ] Created Cameras table
- [ ] Created CameraEvents table
- [ ] Created Recordings table
- [ ] Created CameraPermissions table
- [ ] Created CameraAuditLogs table
- [ ] Migration tested and verified

### Testing Completed
- [ ] Tested playback timeline
- [ ] Tested recording start/stop
- [ ] Tested motion detection
- [ ] Tested camera management CRUD
- [ ] Tested access control
- [ ] Tested audit logging
- [ ] Tested analytics dashboard
- [ ] Tested on actual hardware
- [ ] Verified database migration
- [ ] Tested on Chrome, Edge, Firefox

### Security Checklist
- [ ] Authorization checks implemented
- [ ] Input validation in place
- [ ] File path sanitization
- [ ] Rate limiting configured
- [ ] Sensitive data encrypted

### Build Status
- [ ] `dotnet build` passes with 0 errors
- [ ] Database migration successful
- [ ] All new files compile successfully
- [ ] No breaking changes to existing code

### Post-Merge Instructions
After merging this PR:
1. Run `git checkout master`
2. Run `git reset --hard origin/master`
3. Run `dotnet ef database update`
4. Verify camera system at `/cameras/management`
5. Test with actual camera hardware
```

## 🎨 UI/UX Requirements

### Visual Design
- **Consistent Theme:** Match existing GFC Studio design
- **Responsive:** Works on all screen sizes
- **Accessible:** WCAG 2.1 AA compliant
- **Intuitive:** Clear navigation and controls

### User Experience
- **Fast Loading:** Pages load within 2 seconds
- **Clear Feedback:** Loading states, success/error messages
- **Smooth Animations:** No jarring transitions
- **Help Text:** Tooltips and inline help where needed

## 📅 Timeline

- **Day 1-2:** Database models and migrations
- **Day 3-4:** Backend services (Event, Motion, Audit)
- **Day 5-6:** Playback timeline component
- **Day 7-8:** Recording management
- **Day 9-10:** Camera management interface
- **Day 11-12:** Motion detection and alerts
- **Day 13:** Analytics dashboard
- **Day 14:** Testing, bug fixes, and PR submission

## 🎉 Completion Criteria

This is the **FINAL PHASE**. Upon completion:
- ✅ Camera system is **fully functional**
- ✅ All features from Phases 1-5 are **integrated**
- ✅ System is **production-ready**
- ✅ Documentation is **complete**
- ✅ All tests **pass**

## 🔗 Related Documentation

- Phase 4 Implementation (Multi-Camera Grid)
- Phase 3 Implementation (Live Streaming)
- `docs/camera-system/IMPLEMENTATION_STATUS.md`
- Database schema documentation

---

**Remember:** This is the final phase - make it count! Test thoroughly and ensure everything works perfectly before submitting the PR.
