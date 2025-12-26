# Phase 4: Camera System Enhancements - Multi-Camera Grid & Advanced Controls

## 📋 Issue Overview

**Phase:** 4 of 5  
**Priority:** High  
**Estimated Effort:** 8-12 hours  
**Dependencies:** Phase 3 (Live Streaming) must be complete and merged

## 🎯 Objectives

Build upon Phase 3's live streaming foundation by adding:
1. **Multi-camera grid view** with responsive layout
2. **Camera PTZ controls** (Pan, Tilt, Zoom) if supported by NVR
3. **Stream quality selection** (HD, SD, Low)
4. **Fullscreen mode** for individual cameras
5. **Camera health monitoring** with automatic reconnection
6. **Snapshot capture** functionality

## 📦 Deliverables

### 1. Multi-Camera Grid Component
**File:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraGrid.razor`

```razor
@page "/cameras/grid"
@using GFC.BlazorServer.Services.Camera
@inject ICameraService CameraService
@inject IJSRuntime JS

<PageTitle>Camera Grid - GFC Cameras</PageTitle>

<div class="camera-grid-container">
    <div class="camera-grid-header">
        <h1>Live Camera Grid</h1>
        <div class="grid-controls">
            <select @bind="GridLayout" class="form-select">
                <option value="1">1 Camera</option>
                <option value="4">2x2 Grid</option>
                <option value="9">3x3 Grid</option>
                <option value="16">4x4 Grid</option>
            </select>
            <button class="btn btn-primary" @onclick="RefreshAll">
                <i class="bi bi-arrow-clockwise"></i> Refresh All
            </button>
        </div>
    </div>

    <div class="camera-grid grid-layout-@GridLayout">
        @foreach (var camera in Cameras.Take(GridLayout))
        {
            <div class="camera-grid-item">
                <CameraFeedEnhanced 
                    CameraId="@camera.Id" 
                    CameraName="@camera.Name"
                    ShowControls="true"
                    OnFullscreen="@(() => EnterFullscreen(camera.Id))" />
            </div>
        }
    </div>
</div>

@code {
    private int GridLayout { get; set; } = 4;
    private List<Camera> Cameras { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadCameras();
    }

    private async Task LoadCameras()
    {
        Cameras = await CameraService.GetAllCamerasAsync();
    }

    private async Task RefreshAll()
    {
        await JS.InvokeVoidAsync("CameraGrid.refreshAll");
    }

    private async Task EnterFullscreen(int cameraId)
    {
        await JS.InvokeVoidAsync("CameraGrid.enterFullscreen", cameraId);
    }
}
```

**Styling Requirements:**
- Responsive grid that adapts to screen size
- Smooth transitions when changing grid layout
- Dark theme optimized for video viewing
- Minimal borders to maximize video area

### 2. Enhanced Camera Feed Component
**File:** `apps/webapp/GFC.BlazorServer/Components/Shared/CameraFeedEnhanced.razor`

**New Features:**
- Quality selector (HD/SD/Low)
- PTZ controls overlay
- Snapshot button
- Fullscreen toggle
- Connection status indicator
- Auto-reconnect on stream failure

**Parameters:**
```csharp
[Parameter] public int CameraId { get; set; }
[Parameter] public string? CameraName { get; set; }
[Parameter] public bool ShowControls { get; set; } = true;
[Parameter] public bool EnablePTZ { get; set; } = false;
[Parameter] public EventCallback OnFullscreen { get; set; }
[Parameter] public EventCallback<string> OnSnapshot { get; set; }
```

### 3. PTZ Control Component
**File:** `apps/webapp/GFC.BlazorServer/Components/Shared/PTZControls.razor`

**Features:**
- Directional pad (Up, Down, Left, Right)
- Zoom in/out buttons
- Preset positions (if supported)
- Speed control slider
- Modern, touch-friendly design

### 4. Camera Service Enhancements
**File:** `apps/webapp/GFC.BlazorServer/Services/Camera/CameraService.cs`

**New Methods:**
```csharp
Task<List<Camera>> GetAllCamerasAsync();
Task<CameraCapabilities> GetCameraCapabilitiesAsync(int cameraId);
Task<bool> SendPTZCommandAsync(int cameraId, PTZCommand command);
Task<byte[]> CaptureSnapshotAsync(int cameraId);
Task<StreamQuality> GetStreamQualityAsync(int cameraId);
Task SetStreamQualityAsync(int cameraId, StreamQuality quality);
```

### 5. Video Agent API Enhancements
**File:** `apps/services/GFC.VideoAgent/Controllers/StreamController.cs`

**New Endpoints:**
```csharp
// GET /api/streams/{cameraId}/quality
// POST /api/streams/{cameraId}/quality
// POST /api/cameras/{cameraId}/ptz
// GET /api/cameras/{cameraId}/snapshot
// GET /api/cameras/{cameraId}/capabilities
```

### 6. JavaScript Enhancements
**File:** `apps/webapp/GFC.BlazorServer/wwwroot/js/camera-player.js`

**New Functions:**
```javascript
window.CameraPlayer = {
    // Existing functions...
    
    setQuality: function(playerId, quality) {
        // Switch stream quality
    },
    
    captureSnapshot: function(playerId) {
        // Capture current frame as image
    },
    
    enterFullscreen: function(playerId) {
        // Enter fullscreen mode
    },
    
    exitFullscreen: function() {
        // Exit fullscreen mode
    }
};

window.CameraGrid = {
    refreshAll: function() {
        // Refresh all camera streams
    },
    
    enterFullscreen: function(cameraId) {
        // Enter fullscreen for specific camera
    }
};
```

### 7. CSS Enhancements
**File:** `apps/webapp/GFC.BlazorServer/wwwroot/css/camera-grid.css`

**Requirements:**
- Responsive grid layouts (1, 4, 9, 16 cameras)
- Smooth transitions
- Fullscreen mode styling
- PTZ control overlay
- Quality selector dropdown
- Dark theme optimized

## 🔧 Technical Requirements

### Stream Quality Implementation
```csharp
public enum StreamQuality
{
    Low = 0,    // 640x360, lower bitrate
    SD = 1,     // 1280x720, medium bitrate
    HD = 2      // 1920x1080, high bitrate
}
```

### PTZ Command Structure
```csharp
public class PTZCommand
{
    public PTZAction Action { get; set; }
    public int Speed { get; set; } = 50; // 0-100
    public int? PresetId { get; set; }
}

public enum PTZAction
{
    Up,
    Down,
    Left,
    Right,
    ZoomIn,
    ZoomOut,
    GotoPreset,
    Stop
}
```

### Camera Capabilities
```csharp
public class CameraCapabilities
{
    public bool SupportsPTZ { get; set; }
    public bool SupportsZoom { get; set; }
    public bool SupportsPresets { get; set; }
    public List<int> AvailablePresets { get; set; }
    public List<StreamQuality> AvailableQualities { get; set; }
}
```

## 📝 Implementation Checklist

### Backend (Video Agent)
- [ ] Add PTZ control endpoint
- [ ] Implement snapshot capture
- [ ] Add stream quality switching
- [ ] Implement camera capabilities detection
- [ ] Add health check for each camera stream
- [ ] Implement automatic stream recovery

### Frontend (Blazor)
- [ ] Create `CameraGrid.razor` page
- [ ] Create `CameraFeedEnhanced.razor` component
- [ ] Create `PTZControls.razor` component
- [ ] Create `QualitySelector.razor` component
- [ ] Update `CameraService.cs` with new methods
- [ ] Add navigation menu item for Camera Grid

### JavaScript
- [ ] Implement quality switching in HLS player
- [ ] Add snapshot capture functionality
- [ ] Implement fullscreen mode
- [ ] Add grid refresh functionality
- [ ] Implement automatic reconnection logic

### Styling
- [ ] Create `camera-grid.css`
- [ ] Add responsive grid layouts
- [ ] Style PTZ controls
- [ ] Add quality selector styling
- [ ] Implement fullscreen mode styles
- [ ] Add loading and error states

## 🧪 Testing Requirements

### Manual Testing
1. **Grid Layout**
   - [ ] Test 1x1, 2x2, 3x3, 4x4 layouts
   - [ ] Verify responsive behavior on mobile/tablet
   - [ ] Test switching between layouts

2. **Stream Quality**
   - [ ] Test quality switching (HD → SD → Low)
   - [ ] Verify stream reconnects after quality change
   - [ ] Test bandwidth adaptation

3. **PTZ Controls** (if supported)
   - [ ] Test all directional controls
   - [ ] Test zoom in/out
   - [ ] Test preset positions
   - [ ] Verify speed control

4. **Snapshot Capture**
   - [ ] Test snapshot capture
   - [ ] Verify image quality
   - [ ] Test download functionality

5. **Fullscreen Mode**
   - [ ] Test entering fullscreen
   - [ ] Test exiting fullscreen
   - [ ] Verify controls remain accessible

6. **Error Handling**
   - [ ] Test behavior when camera goes offline
   - [ ] Verify auto-reconnect works
   - [ ] Test error messages display correctly

## 🎨 UI/UX Requirements

### Visual Design
- **Modern & Clean:** Minimal UI that doesn't distract from video
- **Dark Theme:** Optimized for video viewing
- **Responsive:** Works on desktop, tablet, and mobile
- **Accessible:** Keyboard navigation, screen reader support

### User Experience
- **Fast Loading:** Cameras should start streaming within 2 seconds
- **Smooth Transitions:** No jarring layout shifts
- **Clear Feedback:** Loading states, error messages, success indicators
- **Intuitive Controls:** PTZ and quality controls easy to find and use

## 📊 Success Criteria

1. ✅ **Multi-camera grid displays correctly** with 1, 4, 9, or 16 cameras
2. ✅ **Stream quality can be changed** without page reload
3. ✅ **PTZ controls work** (if camera supports it)
4. ✅ **Snapshots can be captured** and downloaded
5. ✅ **Fullscreen mode works** for individual cameras
6. ✅ **Auto-reconnect works** when streams fail
7. ✅ **Page is responsive** on mobile and tablet
8. ✅ **No console errors** in browser
9. ✅ **All streams play simultaneously** without performance issues

## ⚠️ Critical Requirements (DO NOT SKIP)

### 1. Build Verification
```bash
dotnet build
# Must complete with 0 errors, 0 warnings
```

### 2. Code Quality
- All new code must have XML documentation comments
- Follow existing naming conventions
- Use async/await properly
- Handle all exceptions gracefully

### 3. Testing
- Test with **actual camera hardware** on the LAN
- Test with **multiple cameras** simultaneously
- Test **network failure scenarios**
- Test on **different browsers** (Chrome, Edge, Firefox)

### 4. PR Requirements
**MANDATORY PR Description Format:**
```markdown
## Phase 4: Camera System Enhancements

### Changes Made
- [ ] Multi-camera grid view (1x1, 2x2, 3x3, 4x4)
- [ ] Stream quality selection (HD/SD/Low)
- [ ] PTZ controls (if supported)
- [ ] Snapshot capture
- [ ] Fullscreen mode
- [ ] Auto-reconnect on stream failure

### Testing Completed
- [ ] Tested all grid layouts
- [ ] Tested quality switching
- [ ] Tested PTZ controls (if applicable)
- [ ] Tested snapshot capture
- [ ] Tested fullscreen mode
- [ ] Tested auto-reconnect
- [ ] Tested on mobile/tablet
- [ ] Verified no console errors

### Build Status
- [ ] `dotnet build` passes with 0 errors
- [ ] All new files compile successfully
- [ ] No breaking changes to existing code

### Post-Merge Instructions
After merging this PR:
1. Run `git checkout master`
2. Run `git reset --hard origin/master`
3. Verify camera grid page loads at `/cameras/grid`
4. Test with actual camera hardware
```

## 🔗 Related Documentation

- Phase 3 Implementation (Live Streaming)
- `docs/camera-system/IMPLEMENTATION_STATUS.md`
- `docs/PHASE_2_VIDEO_STREAMING_PLAN.md`
- Video Agent API documentation

## 📅 Timeline

- **Day 1-2:** Backend API enhancements (PTZ, quality, snapshot)
- **Day 3-4:** Frontend components (Grid, Enhanced Feed, PTZ Controls)
- **Day 5-6:** JavaScript enhancements and styling
- **Day 7:** Testing and bug fixes
- **Day 8:** Final review and PR submission

## 🚀 Next Phase Preview

**Phase 5** will add:
- Event-based playback timeline
- Video recording and archiving
- Motion detection alerts
- Advanced analytics
- Camera management interface

---

**Remember:** Test thoroughly with actual hardware before submitting PR!
