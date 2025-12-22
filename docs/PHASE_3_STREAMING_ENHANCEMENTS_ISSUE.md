# Phase 3: Video Streaming Enhancements & Polish

## ⚠️ MANDATORY REQUIREMENTS

Before creating your Pull Request, you MUST:

1. ✅ Read and follow `AGENT_WORKFLOW_RULES.md`
2. ✅ Complete ALL items in `docs/PR_PREFLIGHT_CHECKLIST.md`
3. ✅ Ensure `dotnet build` succeeds with ZERO errors on ALL projects
4. ✅ Test the feature end-to-end with actual NVR connection
5. ✅ Include the `.csproj` file for any new projects
6. ✅ Add all required `using` directives
7. ✅ Replace ALL placeholder values with actual configuration

**PRs that don't meet these requirements will be REJECTED.**

---

## 🎯 Objective

Complete the video streaming system by adding HLS.js player integration, stream status indicators, error handling, and multi-camera support.

## ✅ Prerequisites Completed

- [x] Phase 0: Network verification system
- [x] Phase 1: Camera Viewer UI foundation
- [x] Phase 2: Video Agent service (with fixes applied)
- [x] FFmpeg installed (version 8.0.1)
- [x] NVR credentials configured
- [x] Video Agent compiles and runs

## 🐛 Known Issues from Phase 2 (FIXED)

The following issues were found and fixed in Phase 2:
- ✅ Missing `.csproj` file - **FIXED**
- ✅ Missing `using System.Collections.Concurrent;` - **FIXED**
- ✅ Placeholder NVR password - **FIXED**

**DO NOT repeat these mistakes in Phase 3.**

---

## 📋 Tasks to Complete

### 1. Add HLS.js Player Integration

**Location:** `apps/webapp/GFC.BlazorServer/wwwroot/js/`

**Files to Create:**
- [ ] `camera-player.js` - HLS.js wrapper with error handling

**Implementation:**
```javascript
window.CameraPlayer = {
    init: function(videoElementId, streamUrl) {
        const video = document.getElementById(videoElementId);
        
        if (!video) {
            console.error(`Video element ${videoElementId} not found`);
            return null;
        }
        
        if (Hls.isSupported()) {
            const hls = new Hls({
                enableWorker: true,
                lowLatencyMode: true,
                backBufferLength: 90,
                maxBufferLength: 30,
                maxMaxBufferLength: 60
            });
            
            hls.loadSource(streamUrl);
            hls.attachMedia(video);
            
            hls.on(Hls.Events.MANIFEST_PARSED, () => {
                console.log('HLS manifest parsed, starting playback');
                video.play().catch(e => console.error('Autoplay failed:', e));
            });
            
            hls.on(Hls.Events.ERROR, (event, data) => {
                console.error('HLS Error:', data);
                if (data.fatal) {
                    switch(data.type) {
                        case Hls.ErrorTypes.NETWORK_ERROR:
                            console.log('Network error, attempting recovery...');
                            hls.startLoad();
                            break;
                        case Hls.ErrorTypes.MEDIA_ERROR:
                            console.log('Media error, attempting recovery...');
                            hls.recoverMediaError();
                            break;
                        default:
                            console.error('Fatal error, destroying HLS instance');
                            hls.destroy();
                            break;
                    }
                }
            });
            
            return hls;
        } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
            // Safari native HLS support
            video.src = streamUrl;
            video.addEventListener('loadedmetadata', () => {
                video.play().catch(e => console.error('Autoplay failed:', e));
            });
            return video;
        } else {
            console.error('HLS is not supported in this browser');
            return null;
        }
    },
    
    destroy: function(playerInstance) {
        if (playerInstance && playerInstance.destroy) {
            playerInstance.destroy();
        }
    }
};
```

**Files to Modify:**
- [ ] `Components/Pages/_Host.cshtml` or `Components/Layout/MainLayout.razor` - Add HLS.js CDN script

Add this script tag:
```html
<script src="https://cdn.jsdelivr.net/npm/hls.js@1.4.12"></script>
<script src="~/js/camera-player.js"></script>
```

---

### 2. Update Camera Viewer with Stream Status

**Location:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraViewer.razor`

**Enhancements:**
- [ ] Add real-time stream status display
- [ ] Add loading/buffering indicators
- [ ] Add error messages with retry button
- [ ] Add stream quality information (resolution, bitrate)
- [ ] Add fullscreen button
- [ ] Add visual "LIVE" badge when streaming

**Example Implementation:**
```razor
@page "/cameras/{CameraId:int}"
@inject IJSRuntime JS
@inject IConfiguration Configuration
@implements IAsyncDisposable

<div class="camera-viewer">
    <div class="header">
        <h3>
            <i class="bi bi-camera-video-fill me-2"></i>
            Camera @CameraId - @CameraName
            <NewFeatureBadge BadgeText="LIVE" TooltipText="Real-time video streaming" CssClass="live-badge" />
        </h3>
        <div class="stream-status-badge @StatusClass">
            <i class="bi @StatusIcon me-1"></i>
            @StreamStatus
        </div>
    </div>

    <div class="video-container">
        <video id="camera-@CameraId" 
               class="camera-video" 
               controls 
               autoplay 
               muted
               @ref="videoElement">
        </video>
        
        @if (IsLoading)
        {
            <div class="loading-overlay">
                <div class="spinner-border text-light" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="mt-3">Connecting to camera...</p>
            </div>
        }
        
        @if (!string.IsNullOrEmpty(ErrorMessage))
        {
            <div class="error-overlay">
                <i class="bi bi-exclamation-triangle-fill text-warning mb-3" style="font-size: 3rem;"></i>
                <h5>@ErrorMessage</h5>
                <button class="btn btn-primary mt-3" @onclick="RetryConnection">
                    <i class="bi bi-arrow-clockwise me-2"></i>Retry
                </button>
            </div>
        }
    </div>
    
    <div class="stream-info">
        <small class="text-muted">
            <i class="bi bi-info-circle me-1"></i>
            Stream URL: @StreamUrl
        </small>
    </div>
</div>

<style>
    .camera-viewer {
        padding: 20px;
        max-width: 1200px;
        margin: 0 auto;
    }
    
    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
    }
    
    .stream-status-badge {
        padding: 8px 16px;
        border-radius: 20px;
        font-weight: 600;
        font-size: 0.9rem;
    }
    
    .stream-status-badge.live {
        background: linear-gradient(135deg, #10b981 0%, #059669 100%);
        color: white;
        animation: pulse-glow 2s ease-in-out infinite;
    }
    
    .stream-status-badge.connecting {
        background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
        color: white;
    }
    
    .stream-status-badge.offline {
        background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
        color: white;
    }
    
    .video-container {
        position: relative;
        background: #000;
        border-radius: 12px;
        overflow: hidden;
        aspect-ratio: 16 / 9;
    }
    
    .camera-video {
        width: 100%;
        height: 100%;
        object-fit: contain;
    }
    
    .loading-overlay,
    .error-overlay {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        background: rgba(0, 0, 0, 0.8);
        color: white;
    }
    
    @keyframes pulse-glow {
        0%, 100% { box-shadow: 0 0 10px rgba(16, 185, 129, 0.5); }
        50% { box-shadow: 0 0 20px rgba(16, 185, 129, 0.8); }
    }
</style>

@code {
    [Parameter] public int CameraId { get; set; }
    
    private ElementReference videoElement;
    private IJSObjectReference? hlsPlayer;
    private string CameraName = "";
    private string StreamUrl = "";
    private string StreamStatus = "Connecting...";
    private string StatusClass = "connecting";
    private string StatusIcon = "bi-hourglass-split";
    private bool IsLoading = true;
    private string? ErrorMessage = null;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeStream();
        }
    }
    
    private async Task InitializeStream()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            StateHasChanged();
            
            var videoAgentUrl = Configuration["VideoAgent:BaseUrl"] ?? "https://localhost:5101";
            StreamUrl = $"{videoAgentUrl}/stream/camera{CameraId}.m3u8";
            CameraName = $"Camera {CameraId}"; // TODO: Get from config
            
            hlsPlayer = await JS.InvokeAsync<IJSObjectReference>(
                "CameraPlayer.init", 
                $"camera-{CameraId}", 
                StreamUrl
            );
            
            // Simulate status check (in real implementation, poll Video Agent API)
            await Task.Delay(2000);
            
            StreamStatus = "LIVE";
            StatusClass = "live";
            StatusIcon = "bi-circle-fill";
            IsLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to connect: {ex.Message}";
            StreamStatus = "Offline";
            StatusClass = "offline";
            StatusIcon = "bi-x-circle-fill";
            IsLoading = false;
            StateHasChanged();
        }
    }
    
    private async Task RetryConnection()
    {
        await InitializeStream();
    }
    
    public async ValueTask DisposeAsync()
    {
        if (hlsPlayer != null)
        {
            await JS.InvokeVoidAsync("CameraPlayer.destroy", hlsPlayer);
            await hlsPlayer.DisposeAsync();
        }
    }
}
```

---

### 3. Add Multi-Camera Grid View

**Location:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraGrid.razor` (NEW)

**Create a new page for viewing multiple cameras:**
- [ ] Grid layout (2x2, 3x3, or 4x4)
- [ ] Each camera shows live feed
- [ ] Click to expand to full screen
- [ ] Show status for each camera
- [ ] Add to navigation menu with NEW badge

**Route:** `@page "/cameras/grid"`

---

### 4. Add Video Agent Health Check API

**Location:** `apps/services/GFC.VideoAgent/Program.cs`

**Add API endpoints:**
```csharp
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.MapGet("/stream/{cameraId}/status", (int cameraId, StreamManager streamManager) => 
{
    var status = streamManager.GetStreamStatus(cameraId);
    return Results.Ok(new { cameraId, status = status.ToString(), timestamp = DateTime.UtcNow });
});
```

---

### 5. Configuration Updates

**Web App `appsettings.json`:**
```json
{
  "VideoAgent": {
    "BaseUrl": "https://localhost:5101",
    "HealthCheckIntervalSeconds": 10
  }
}
```

---

## 🎨 Visual Requirements

**MANDATORY Visual Indicators:**
- [ ] "LIVE" badge on Camera Viewer when streaming (green, pulsing)
- [ ] "CONNECTING" badge when loading (yellow)
- [ ] "OFFLINE" badge when stream fails (red)
- [ ] "NEW" badge on Camera Grid menu item
- [ ] Loading spinner during connection
- [ ] Error icon and message on failure

---

## ✅ Testing Checklist

### Build & Compilation:
- [ ] `dotnet build` succeeds on `GFC.BlazorServer` with ZERO errors
- [ ] `dotnet build` succeeds on `GFC.VideoAgent` with ZERO errors
- [ ] No missing `using` directives
- [ ] All `.csproj` files are present and complete

### Functionality:
- [ ] Video Agent starts without errors
- [ ] FFmpeg process launches for configured cameras
- [ ] HLS playlist (.m3u8) is generated in output directory
- [ ] HLS segments (.ts) are created and updated
- [ ] Web app loads Camera Viewer page
- [ ] Video player initializes
- [ ] Video plays in Chrome
- [ ] Video plays in Firefox
- [ ] Video plays in Edge
- [ ] Stream status updates correctly (LIVE/CONNECTING/OFFLINE)
- [ ] Error handling works (disconnect NVR and verify error message)
- [ ] Retry button reconnects successfully
- [ ] Multi-camera grid displays all cameras
- [ ] Health check API returns correct status

### Visual:
- [ ] "LIVE" badge appears and pulses when streaming
- [ ] Loading spinner shows during connection
- [ ] Error overlay shows when connection fails
- [ ] All NEW badges are present
- [ ] UI is responsive (works on different screen sizes)

---

## 📊 Success Criteria

✅ User can view live camera feed in browser  
✅ Stream status is clearly visible (LIVE/CONNECTING/OFFLINE)  
✅ Error messages are user-friendly with retry option  
✅ Multiple cameras can be viewed simultaneously in grid  
✅ Stream latency is <5 seconds  
✅ System handles stream interruptions gracefully  
✅ Works on Chrome, Firefox, Edge  
✅ All visual indicators are present and functional  
✅ Code compiles with ZERO errors  
✅ No placeholder values remain  

---

## 🔒 Security & Performance

- [ ] CORS properly configured in Video Agent
- [ ] No credentials exposed in client-side code
- [ ] HLS segments are cleaned up automatically
- [ ] Memory usage is reasonable (<500MB for 4 cameras)
- [ ] CPU usage is reasonable (<50% for 4 cameras)

---

## 📝 Implementation Notes

**Branch Naming:** `feat/camera-system-phase-3-streaming-enhancements`  
**Target Branch:** `master`  
**Estimated Effort:** 6-8 hours  

---

## 📚 Reference Files

**Read these BEFORE starting:**
- `AGENT_WORKFLOW_RULES.md` - Mandatory workflow requirements
- `docs/PR_PREFLIGHT_CHECKLIST.md` - Pre-flight checklist
- `docs/PHASE_2_VIDEO_STREAMING_PLAN.md` - Phase 2 implementation details
- `apps/services/GFC.VideoAgent/README.md` - Video Agent documentation

---

## ⚠️ Critical Reminders

1. **MUST include `.csproj` file** for any new projects
2. **MUST add all `using` directives** - check compilation!
3. **MUST replace placeholder values** - no "your_password"
4. **MUST test `dotnet build`** before creating PR
5. **MUST test end-to-end** with actual NVR
6. **MUST add visual indicators** per AGENT_WORKFLOW_RULES.md
7. **MUST resolve merge conflicts** before committing
8. **MUST document configuration changes** in PR description

---

## 🚀 Deployment Instructions

Include in PR description:

1. How to install HLS.js (CDN link provided)
2. How to configure Video Agent URL
3. How to test the streaming
4. Any new dependencies added
5. Screenshots of working video player

---

**Ready to implement? Follow the checklist, test thoroughly, and deliver quality code! 🎥**
