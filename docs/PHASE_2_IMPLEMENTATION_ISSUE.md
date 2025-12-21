# Phase 2: Live Video Streaming - Implementation Issue

## 🎯 Objective
Implement live RTSP-to-HLS video streaming from the NVR to enable users to view camera feeds in real-time through the web browser.

## ✅ Prerequisites Completed
- [x] Phase 0: Network verification system
- [x] Phase 1: Camera Viewer UI foundation
- [x] Camera Settings page with credential management
- [x] FFmpeg installed on server (version 8.0.1)
- [x] NVR credentials configured in appsettings.json

## 📋 Tasks to Complete

### 1. Create Video Agent Service
**Location:** `apps/services/GFC.VideoAgent/`

**Files to Create:**
- [ ] `Program.cs` - Entry point with host configuration
- [ ] `Services/StreamManager.cs` - Manages active camera streams
- [ ] `Services/FFmpegService.cs` - Handles FFmpeg process lifecycle
- [ ] `Models/CameraStream.cs` - Stream configuration model
- [ ] `Models/StreamStatus.cs` - Stream health monitoring
- [ ] `appsettings.json` - Configuration file
- [ ] `README.md` - Service documentation

**NuGet Packages to Add:**
```bash
dotnet add package FFMpegCore
dotnet add package Microsoft.Extensions.Hosting
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.AspNetCore.App
```

**Key Features:**
- Background service that auto-starts FFmpeg processes for each camera
- Health monitoring with auto-restart on failure
- HLS output with 2-second segments
- Minimal API endpoints to serve .m3u8 playlists and .ts segments
- Graceful shutdown handling

### 2. FFmpeg Integration
**FFmpeg Command Template:**
```bash
ffmpeg -rtsp_transport tcp -i rtsp://{username}:{password}@{nvr_ip}:{port}/{path} \
  -c:v copy -c:a aac -f hls \
  -hls_time 2 -hls_list_size 5 -hls_flags delete_segments \
  -hls_segment_filename "camera_{id}_%03d.ts" camera_{id}.m3u8
```

**Configuration:**
- Output directory: `C:\temp\hls-streams\`
- Listen port: `5101`
- CORS enabled for localhost:5000 (web app)

### 3. Web App Integration

**Files to Modify:**
- [ ] `Components/Pages/CameraViewer.razor` - Add video player
- [ ] `wwwroot/js/camera-player.js` - HLS.js wrapper (NEW)
- [ ] `Components/Layout/MainLayout.razor` - Add HLS.js script reference
- [ ] `appsettings.json` - Add VideoAgent URL configuration

**HLS.js Integration:**
```html
<!-- Add to _Host.cshtml or MainLayout -->
<script src="https://cdn.jsdelivr.net/npm/hls.js@latest"></script>
<script src="js/camera-player.js"></script>
```

**Video Player Component:**
```razor
<div class="video-container">
    <video id="camera-@CameraId" 
           class="camera-video" 
           controls 
           autoplay 
           muted>
    </video>
    <div class="stream-status">
        @StreamStatus
    </div>
</div>

@code {
    [Parameter] public int CameraId { get; set; }
    private string StreamStatus = "Connecting...";
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var streamUrl = $"https://localhost:5101/stream/{CameraId}/playlist.m3u8";
            await JS.InvokeVoidAsync("CameraPlayer.init", $"camera-{CameraId}", streamUrl);
        }
    }
}
```

### 4. Visual Indicators (MANDATORY)

**Add NEW badges to:**
- [ ] Navigation menu - "Live Cameras" link with NEW badge
- [ ] Camera Viewer page header - NEW badge
- [ ] Stream status indicators:
  - 🟢 "LIVE" badge when streaming
  - 🔴 "OFFLINE" badge when stream fails
  - ⏸️ "BUFFERING" indicator during load
  - 📊 Stream info (resolution, bitrate)

### 5. Configuration Files

**Video Agent `appsettings.json`:**
```json
{
  "VideoAgent": {
    "OutputDirectory": "C:\\temp\\hls-streams",
    "ListenPort": 5101,
    "FFmpegPath": "ffmpeg",
    "AllowedOrigins": ["https://localhost:5000", "https://localhost:5001"]
  },
  "NvrSettings": {
    "IpAddress": "192.168.1.64",
    "RtspPort": 554,
    "Username": "admin",
    "Password": "your_password",
    "Cameras": [
      {
        "Id": 1,
        "Name": "Main Entrance",
        "RtspPath": "/cam/realmonitor?channel=1&subtype=0",
        "Enabled": true
      }
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

**Web App `appsettings.json` Addition:**
```json
{
  "VideoAgent": {
    "BaseUrl": "https://localhost:5101"
  }
}
```

### 6. Testing Checklist

- [ ] Video Agent starts without errors
- [ ] FFmpeg process launches for each enabled camera
- [ ] HLS playlist (.m3u8) is generated
- [ ] HLS segments (.ts) are created and updated every 2 seconds
- [ ] Web app can fetch playlist from Video Agent
- [ ] Video plays in Chrome
- [ ] Video plays in Firefox
- [ ] Video plays in Edge
- [ ] Stream recovers from network interruptions
- [ ] Multiple cameras can stream simultaneously
- [ ] Visual indicators update correctly (LIVE/OFFLINE/BUFFERING)

### 7. Error Handling

**Common Issues to Handle:**
- RTSP connection failure → Retry with exponential backoff
- FFmpeg crash → Auto-restart process
- Network interruption → Reconnect automatically
- Invalid credentials → Log error and mark stream as offline
- Disk space full → Clean up old segments

### 8. Documentation

**Create/Update:**
- [ ] `docs/VIDEO_AGENT_SETUP.md` - Installation and configuration guide
- [ ] `docs/TROUBLESHOOTING.md` - Common issues and solutions
- [ ] Update `AGENT_WORKFLOW_RULES.md` - Add visual indicator requirements for video features

## 🎨 Design Requirements

**Visual Indicators (Per AGENT_WORKFLOW_RULES.md):**
- All new UI elements MUST have NEW/UPDATED badges
- Use `<NewFeatureBadge>` component
- Add badges to navigation menu and page headers
- Mark new sections within existing pages

**Styling:**
- Modern, premium design
- Smooth animations and transitions
- Responsive layout (works on mobile)
- Dark mode compatible

## 📊 Success Criteria

✅ User can view live camera feed in browser  
✅ Stream latency is <3 seconds  
✅ System handles stream interruptions gracefully  
✅ Multiple cameras can be viewed simultaneously  
✅ Works on Chrome, Firefox, Safari, Edge  
✅ Visual indicators show stream status accurately  
✅ All NEW badges are present per workflow rules  

## 🔒 Security Considerations

- [ ] HTTPS only for Video Agent in production
- [ ] API key authentication between Web App and Video Agent
- [ ] CORS properly configured
- [ ] Rate limiting to prevent abuse
- [ ] Credentials never exposed in client-side code

## 📝 Implementation Notes

**Branch Naming:** `feat/camera-system-phase-2-video-streaming`  
**Target Branch:** `master`  
**Estimated Effort:** 8-13 hours  

## 🚀 Deployment

**Development:**
- Video Agent runs on localhost:5101
- HLS files stored in `C:\temp\hls-streams\`

**Production:**
- Deploy Video Agent as Windows Service
- Use persistent storage for HLS segments
- Configure reverse proxy for HTTPS
- Set up monitoring and auto-restart

## 📚 Reference Documentation

- FFmpeg HLS Documentation: https://ffmpeg.org/ffmpeg-formats.html#hls-2
- HLS.js Documentation: https://github.com/video-dev/hls.js/
- Phase 2 Implementation Plan: `docs/PHASE_2_VIDEO_STREAMING_PLAN.md`
- Agent Workflow Rules: `AGENT_WORKFLOW_RULES.md`

## ⚠️ Important Reminders

1. **Follow AGENT_WORKFLOW_RULES.md** - All new features need visual indicators
2. **Target `master` branch** - Do NOT use `main`
3. **Test before marking complete** - All items in testing checklist must pass
4. **Commit frequently** - Small, logical commits with clear messages
5. **Update documentation** - Keep docs in sync with code changes

---

**Ready to implement? Let's build Phase 2! 🎥**
