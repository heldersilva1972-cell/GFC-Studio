# Phase 2: Live Video Streaming Implementation Plan

## Overview
Implement live RTSP-to-HLS video streaming from the NVR to the web browser, enabling users to view camera feeds in real-time.

## Architecture

```
NVR (RTSP) → Video Agent (FFmpeg) → HLS Stream → Web Browser (HLS.js)
```

## Prerequisites
- ✅ Phase 0 & 1 completed (verification and UI foundation)
- ✅ NVR credentials configured
- ⚠️ FFmpeg must be installed on the server

## Components to Build

### 1. Video Agent Service (New .NET Console Application)

**Location:** `apps/services/GFC.VideoAgent/`

**Purpose:** Standalone service that handles video transcoding

**Key Features:**
- Connects to NVR via RTSP
- Transcodes streams using FFmpeg to HLS format
- Serves HLS manifests (.m3u8) and segments (.ts) via HTTP
- Manages multiple camera streams simultaneously
- Auto-reconnects on stream failure

**Technology Stack:**
- .NET 8 Console Application
- FFmpeg wrapper (FFMpegCore NuGet package)
- Minimal API for serving HLS files
- Background services for stream management

### 2. HLS.js Integration (Web App)

**Location:** `apps/webapp/GFC.BlazorServer/wwwroot/js/`

**Purpose:** Browser-side video player

**Files to Add:**
- `hls.min.js` (from CDN or local copy)
- `camera-player.js` (custom wrapper)

### 3. Updated Camera Viewer Page

**Location:** `apps/webapp/GFC.BlazorServer/Components/Pages/CameraViewer.razor`

**Enhancements:**
- Replace placeholder with actual `<video>` element
- Initialize HLS.js player
- Display stream status (buffering, playing, error)
- Add controls (play, pause, fullscreen)

## Implementation Steps

### Step 1: Create Video Agent Project

```bash
cd apps/services
dotnet new console -n GFC.VideoAgent
cd GFC.VideoAgent
dotnet add package FFMpegCore
dotnet add package Microsoft.Extensions.Hosting
dotnet add package Microsoft.Extensions.Configuration.Json
```

**Files to Create:**
1. `Program.cs` - Entry point and host configuration
2. `Services/StreamManager.cs` - Manages active streams
3. `Services/FFmpegService.cs` - Handles FFmpeg process lifecycle
4. `Models/CameraStream.cs` - Stream configuration model
5. `appsettings.json` - Configuration (NVR settings, output paths)

### Step 2: Implement FFmpeg Transcoding

**FFmpeg Command Template:**
```bash
ffmpeg -rtsp_transport tcp -i rtsp://{username}:{password}@{nvr_ip}:{port}/{path} \
  -c:v copy -c:a aac -f hls \
  -hls_time 2 -hls_list_size 5 -hls_flags delete_segments \
  -hls_segment_filename "stream_%03d.ts" stream.m3u8
```

**Key Parameters:**
- `-rtsp_transport tcp`: Use TCP for reliability
- `-c:v copy`: Copy video codec (no re-encoding for performance)
- `-c:a aac`: Transcode audio to AAC
- `-f hls`: Output HLS format
- `-hls_time 2`: 2-second segments
- `-hls_list_size 5`: Keep last 5 segments in playlist
- `-hls_flags delete_segments`: Auto-cleanup old segments

### Step 3: Serve HLS Files

**Minimal API Endpoints:**
```csharp
app.MapGet("/stream/{cameraId}/playlist.m3u8", (int cameraId) => 
{
    var path = Path.Combine(outputDir, $"camera_{cameraId}", "stream.m3u8");
    return Results.File(path, "application/vnd.apple.mpegurl");
});

app.MapGet("/stream/{cameraId}/{segment}", (int cameraId, string segment) => 
{
    var path = Path.Combine(outputDir, $"camera_{cameraId}", segment);
    return Results.File(path, "video/MP2T");
});
```

### Step 4: Add HLS.js to Web App

**Download HLS.js:**
```bash
# Option 1: CDN (add to _Host.cshtml)
<script src="https://cdn.jsdelivr.net/npm/hls.js@latest"></script>

# Option 2: Local copy
# Download from https://github.com/video-dev/hls.js/releases
# Place in wwwroot/js/hls.min.js
```

**Create Player Wrapper (`wwwroot/js/camera-player.js`):**
```javascript
window.CameraPlayer = {
    init: function(videoElementId, streamUrl) {
        const video = document.getElementById(videoElementId);
        
        if (Hls.isSupported()) {
            const hls = new Hls({
                enableWorker: true,
                lowLatencyMode: true,
                backBufferLength: 90
            });
            
            hls.loadSource(streamUrl);
            hls.attachMedia(video);
            
            hls.on(Hls.Events.MANIFEST_PARSED, () => {
                video.play();
            });
            
            hls.on(Hls.Events.ERROR, (event, data) => {
                console.error('HLS Error:', data);
                if (data.fatal) {
                    switch(data.type) {
                        case Hls.ErrorTypes.NETWORK_ERROR:
                            hls.startLoad();
                            break;
                        case Hls.ErrorTypes.MEDIA_ERROR:
                            hls.recoverMediaError();
                            break;
                        default:
                            hls.destroy();
                            break;
                    }
                }
            });
            
            return hls;
        } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
            // Safari native HLS support
            video.src = streamUrl;
        }
    }
};
```

### Step 5: Update Camera Viewer Page

**Replace placeholder with video player:**
```razor
<div class="video-container">
    <video id="camera-@cameraId" 
           class="camera-video" 
           controls 
           autoplay 
           muted>
    </video>
</div>

@code {
    private IJSRuntime JS { get; set; }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var streamUrl = $"https://localhost:5101/stream/1/playlist.m3u8";
            await JS.InvokeVoidAsync("CameraPlayer.init", "camera-1", streamUrl);
        }
    }
}
```

### Step 6: Configuration

**Video Agent `appsettings.json`:**
```json
{
  "VideoAgent": {
    "OutputDirectory": "C:\\temp\\hls-streams",
    "ListenPort": 5101,
    "FFmpegPath": "C:\\ffmpeg\\bin\\ffmpeg.exe"
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
  }
}
```

## Testing Checklist

- [ ] Video Agent starts without errors
- [ ] FFmpeg process launches successfully
- [ ] HLS playlist (.m3u8) is generated
- [ ] HLS segments (.ts) are created and updated
- [ ] Web app can fetch playlist from Video Agent
- [ ] Video plays in browser
- [ ] Stream recovers from network interruptions
- [ ] Multiple cameras can stream simultaneously

## Visual Indicators

**Add to CameraViewer.razor:**
- 🟢 "LIVE" badge when streaming
- 🔴 "OFFLINE" badge when stream fails
- ⏸️ "BUFFERING" indicator during load
- 📊 Stream quality indicator (bitrate, resolution)

## Security Considerations

1. **HTTPS Only:** Video Agent must use HTTPS in production
2. **Authentication:** Add API key validation between Web App and Video Agent
3. **CORS:** Configure proper CORS headers
4. **Rate Limiting:** Prevent stream abuse

## Performance Optimization

1. **Adaptive Bitrate:** Generate multiple quality levels
2. **Caching:** Cache HLS segments with proper headers
3. **CDN:** Consider CDN for segment delivery in production
4. **Resource Limits:** Limit concurrent streams per user

## Error Handling

**Common Issues & Solutions:**
- **RTSP Connection Failed:** Check NVR credentials, network, firewall
- **FFmpeg Not Found:** Verify FFmpeg installation path
- **HLS Playback Fails:** Check CORS, HTTPS, browser compatibility
- **High Latency:** Reduce HLS segment duration, enable low-latency mode

## Deployment Notes

**Development:**
- Video Agent runs on localhost:5101
- HLS files stored in temp directory

**Production:**
- Deploy Video Agent as Windows Service or Docker container
- Use persistent storage for HLS segments
- Configure reverse proxy (nginx/IIS) for HTTPS
- Set up monitoring and auto-restart

## Next Steps (Phase 3)

After Phase 2 is complete:
- Event-based playback (rewind to specific events)
- Multi-camera grid view
- PTZ controls (if supported by NVR)
- Motion detection overlays
- Video download/export

---

## Installation Prerequisites

**FFmpeg Installation:**
```powershell
# Option 1: Chocolatey
choco install ffmpeg

# Option 2: Manual
# Download from https://ffmpeg.org/download.html
# Extract to C:\ffmpeg
# Add C:\ffmpeg\bin to PATH
```

**Verify Installation:**
```powershell
ffmpeg -version
```

## Estimated Effort

- Video Agent Development: 4-6 hours
- HLS.js Integration: 2-3 hours
- Testing & Debugging: 2-4 hours
- **Total: 8-13 hours**

## Success Criteria

✅ User can view live camera feed in browser
✅ Stream is smooth with <3 second latency
✅ System handles stream interruptions gracefully
✅ Multiple cameras can be viewed simultaneously
✅ Works on Chrome, Firefox, Safari, Edge
