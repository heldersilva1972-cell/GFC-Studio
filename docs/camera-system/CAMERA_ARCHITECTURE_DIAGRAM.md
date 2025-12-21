# GFC Camera System - Security Architecture Diagram

```
═══════════════════════════════════════════════════════════════════════════════
                         GFC CAMERA SYSTEM ARCHITECTURE
                              (Security-First Design)
═══════════════════════════════════════════════════════════════════════════════

┌─────────────────────────────────────────────────────────────────────────────┐
│                              REMOTE ACCESS (Phase 2+)                        │
│                                                                              │
│  👤 Remote User (Laptop/Phone)                                              │
│         │                                                                    │
│         │ WireGuard VPN Tunnel                                              │
│         │ ✅ Encrypted                                                       │
│         │ ✅ Authenticated                                                   │
│         │ ✅ Per-User Keys                                                   │
│         ▼                                                                    │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      │
╔═════════════════════════════════════╩═══════════════════════════════════════╗
║                          🔥 FIREWALL / ROUTER                                ║
║                                                                              ║
║  ❌ NO Port Forwarding (554, 8000, 80, 443)                                 ║
║  ❌ NO UPnP                                                                  ║
║  ❌ NO Direct NVR Access                                                     ║
║  ❌ NO Cloud/DDNS (after Phase 2)                                           ║
╚══════════════════════════════════════════════════════════════════════════════╝
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                         GFC NETWORK (LAN - 192.168.1.x)                      │
│                                                                              │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │  💻 GFC Web App (Blazor Server)                                       │  │
│  │                                                                        │  │
│  │  ✅ HTTPS Only                                                         │  │
│  │  ✅ User Authentication Required                                       │  │
│  │  ✅ Role-Based Permissions                                             │  │
│  │  ✅ Session Management                                                 │  │
│  │  ✅ Audit Logging                                                      │  │
│  │                                                                        │  │
│  │  Components:                                                           │  │
│  │  • Dashboard.razor (Status LED, Cameras Button)                       │  │
│  │  • CameraViewer.razor (HLS Player, Status Window)                     │  │
│  │  • VideoAgentClient.cs (API Communication)                            │  │
│  └───────────────────────────────────────────────────────────────────────┘  │
│                                      │                                       │
│                                      │ HTTP + API Key                        │
│                                      │ ✅ Encrypted API Key                  │
│                                      │ ✅ Request Validation                 │
│                                      │ ✅ Rate Limiting                      │
│                                      ▼                                       │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │  🖥️ Video Agent Service (Windows Service - .NET 8)                    │  │
│  │                                                                        │  │
│  │  ✅ Secure Credential Storage (Windows DPAPI)                          │  │
│  │  ✅ API Key Authentication                                             │  │
│  │  ✅ Session Tracking                                                   │  │
│  │  ✅ Comprehensive Logging (Serilog)                                    │  │
│  │  ✅ FFmpeg Process Management                                          │  │
│  │  ✅ Auto-Cleanup (No Orphaned Processes)                               │  │
│  │                                                                        │  │
│  │  Endpoints:                                                            │  │
│  │  • GET  /api/heartbeat        (Status Check)                          │  │
│  │  • POST /api/stream/start     (Start Camera Stream)                   │  │
│  │  • POST /api/stream/stop      (Stop Camera Stream)                    │  │
│  │  • GET  /api/stream/status    (Stream Status)                         │  │
│  │  • GET  /streams/{id}/output.m3u8 (HLS Playlist)                      │  │
│  │                                                                        │  │
│  │  Security Boundary: Credentials NEVER leave this service              │  │
│  └───────────────────────────────────────────────────────────────────────┘  │
│                                      │                                       │
│                                      │ RTSP (LAN-Only)                       │
│                                      │ ✅ Private Network Only               │
│                                      │ ✅ No Internet Exposure               │
│                                      ▼                                       │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │  📹 NVR (Network Video Recorder)                                      │  │
│  │                                                                        │  │
│  │  IP: 192.168.1.64 (Static, Private)                                   │  │
│  │  Ports: 554 (RTSP), 8000 (HTTP API)                                   │  │
│  │                                                                        │  │
│  │  ✅ LAN-Only Access                                                    │  │
│  │  ✅ Strong Admin Password                                              │  │
│  │  ✅ Cloud/DDNS Disabled (Phase 2+)                                     │  │
│  │  ✅ Firmware Updated                                                   │  │
│  │  ❌ NO Internet Exposure                                               │  │
│  │  ❌ NO Port Forwarding                                                 │  │
│  │  ❌ NO Direct Browser Access                                           │  │
│  └───────────────────────────────────────────────────────────────────────┘  │
│                                      │                                       │
│                                      │ RTSP Streams                          │
│                                      ▼                                       │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │  📷 16 IP Cameras                                                     │  │
│  │                                                                        │  │
│  │  Camera 1: Front Entrance    Camera 5: Parking Lot                    │  │
│  │  Camera 2: Back Door          Camera 6: Side Entrance                 │  │
│  │  Camera 3: Main Hall          Camera 7-16: [Additional Cameras]       │  │
│  │  Camera 4: Office Area                                                │  │
│  │                                                                        │  │
│  │  Each Camera:                                                          │  │
│  │  • Main Stream: 1920x1080 (High Quality)                              │  │
│  │  • Sub Stream: 640x480 (Low Bandwidth - Used for Live View)           │  │
│  └───────────────────────────────────────────────────────────────────────┘  │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘


═══════════════════════════════════════════════════════════════════════════════
                              SECURITY FEATURES
═══════════════════════════════════════════════════════════════════════════════

┌─────────────────────────────────────────────────────────────────────────────┐
│  ✅ WHAT IS PROTECTED                                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│  • NVR never exposed to internet                                            │
│  • Credentials encrypted at rest (Windows DPAPI)                            │
│  • Credentials never sent to browser                                        │
│  • API key required for all Video Agent requests                            │
│  • User authentication required before camera access                        │
│  • Session-based authorization with unique tokens                           │
│  • Inactivity timeout (5 min warning, 6 min auto-stop)                      │
│  • Auto-cleanup of FFmpeg processes                                         │
│  • Comprehensive audit logging (who, what, when, where)                     │
│  • Rate limiting on video endpoints                                         │
│  • Security headers (CSP, X-Frame-Options)                                  │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│  ❌ WHAT IS PROHIBITED                                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│  • Port forwarding to NVR                                                   │
│  • Direct browser → NVR connections                                         │
│  • Credentials in browser/JavaScript                                        │
│  • Anonymous or guest camera viewing                                        │
│  • Unlogged camera access                                                   │
│  • Sharing VPN credentials (Phase 2+)                                       │
│  • Full-day unrestricted downloads                                          │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│  📊 AUDIT LOGGING                                                            │
├─────────────────────────────────────────────────────────────────────────────┤
│  Video Agent Logs:                                                           │
│  • Service start/stop                                                        │
│  • Heartbeat checks                                                          │
│  • NVR probe results                                                         │
│  • Stream start (user, camera, time)                                        │
│  • Stream stop (user, camera, duration)                                     │
│  • Authentication failures                                                   │
│  • FFmpeg errors                                                             │
│  • API key validation failures                                              │
│                                                                              │
│  Web App Logs:                                                               │
│  • Camera viewer page access (user, time)                                   │
│  • Stream requests (user, camera, time)                                     │
│  • Session timeouts (user, camera, duration)                                │
│  • Download requests (user, camera, time range)                             │
└─────────────────────────────────────────────────────────────────────────────┘


═══════════════════════════════════════════════════════════════════════════════
                              DATA FLOW (Phase 1)
═══════════════════════════════════════════════════════════════════════════════

1. USER AUTHENTICATION
   User → Web App Login → Authentication Check → Dashboard Access

2. CAMERA VIEWER ACCESS
   User Clicks "Cameras" → Check Auth → Load CameraViewer.razor

3. HEARTBEAT CHECK
   CameraViewer → VideoAgentClient → GET /api/heartbeat → Video Agent
   Video Agent → Ping NVR → Return Status → Update Status LED

4. STREAM START
   CameraViewer → VideoAgentClient → POST /api/stream/start
   Video Agent → Validate API Key → Check NVR → Start FFmpeg
   FFmpeg → Connect RTSP → Transcode to HLS → Save Segments
   Video Agent → Return HLS URL → CameraViewer

5. VIDEO PLAYBACK
   CameraViewer → HLS.js → GET /streams/camera1/output.m3u8
   HLS.js → Load Segments → Decode → Display Video

6. INACTIVITY TIMEOUT
   5 min idle → Show "Still Working?" Modal
   No response in 60s → Auto-stop stream → Log timeout → Redirect

7. STREAM STOP
   User Closes Page → beforeunload Event → POST /api/stream/stop
   Video Agent → Kill FFmpeg → Delete Segments → Log Stop → Return OK

8. AUDIT LOGGING
   Every Action → Log to Serilog → Daily Rolling Files
   Critical Events → Also log to GFC Audit System


═══════════════════════════════════════════════════════════════════════════════
                           PERFORMANCE REQUIREMENTS
═══════════════════════════════════════════════════════════════════════════════

┌─────────────────────────────────────────────────────────────────────────────┐
│  Metric                    │ Target         │ Maximum Acceptable            │
├────────────────────────────┼────────────────┼───────────────────────────────┤
│  Heartbeat Response Time   │ < 200ms        │ < 500ms                       │
│  Stream Start Time         │ < 2 seconds    │ < 3 seconds                   │
│  Video Latency             │ < 2 seconds    │ < 3 seconds                   │
│  CPU Usage (per stream)    │ < 5%           │ < 10%                         │
│  Memory Usage (per stream) │ < 150MB        │ < 200MB                       │
│  Network Bandwidth         │ ~500 Kbps      │ ~1 Mbps (sub-stream)          │
└─────────────────────────────────────────────────────────────────────────────┘


═══════════════════════════════════════════════════════════════════════════════
                              PHASE PROGRESSION
═══════════════════════════════════════════════════════════════════════════════

PHASE 1: Connectivity Proof (CURRENT)
├─ LAN-only operation
├─ Single camera viewer
├─ Security foundation
├─ Audit logging
└─ Zero internet exposure

PHASE 2: Modern UI & Multi-Camera
├─ Multi-camera grid (1x1, 2x2, 3x3, 4x4)
├─ Camera selector sidebar
├─ PTZ controls
├─ Snapshot functionality
└─ Deploy WireGuard VPN for remote access

PHASE 3: Playback & Timeline
├─ Video playback controls
├─ Timeline scrubbing
├─ Clip downloads (with watermarks)
└─ Export functionality

PHASE 4: Archive & Management
├─ Retention policies
├─ Archive management
├─ Storage monitoring
└─ Automated cleanup

PHASE 5: Advanced Security
├─ Per-user VPN keys
├─ Advanced role-based permissions
├─ Download restrictions
└─ Privacy compliance features


═══════════════════════════════════════════════════════════════════════════════
                           DEPLOYMENT ARCHITECTURE
═══════════════════════════════════════════════════════════════════════════════

┌─────────────────────────────────────────────────────────────────────────────┐
│  Component          │ Deployment Location    │ Technology                   │
├─────────────────────┼────────────────────────┼──────────────────────────────┤
│  GFC Web App        │ IIS / Kestrel          │ Blazor Server (.NET 8)       │
│  Video Agent        │ Windows Service        │ .NET 8 + FFmpeg              │
│  NVR                │ Physical Device        │ Vendor Firmware              │
│  Cameras            │ Physical Devices       │ IP Cameras (RTSP)            │
│  VPN Server         │ Router / Dedicated PC  │ WireGuard (Phase 2+)         │
└─────────────────────────────────────────────────────────────────────────────┘


═══════════════════════════════════════════════════════════════════════════════
                              LEGEND
═══════════════════════════════════════════════════════════════════════════════

✅ = Security Feature / Requirement Met
❌ = Prohibited Action / Security Violation
🔥 = Firewall / Security Boundary
💻 = Web Application
🖥️ = Windows Service
📹 = Network Video Recorder
📷 = IP Camera
👤 = User
│  = Data Flow
▼  = Direction of Flow
```

**Document Version:** 1.0  
**Last Updated:** 2025-12-21  
**Reference:** See `CAMERA_SECURITY_MASTER_GUIDE.md` for complete security requirements
