# Plan: Camera System Integration - Phase 1: Complete Setup with Remote Access
**Project:** GFC Camera Viewer  
**Status:** Planning / In-Process  
**Security Reference:** See `CAMERA_SECURITY_MASTER_GUIDE.md`  
**Architecture:** Secure VPN Access (Local + Remote)  
**Setup Guide:** See `CAMERA_PHASE_1_LOCAL_PC_SETUP.md` for step-by-step instructions

---

## 🎯 Objective
Prove reliable connectivity between the GFC Web App and the NVR (192.168.1.64) via the Local Video Agent, with **secure remote access included**. Establish a real-time status bridge with plain-English feedback for the user, accessible both locally and remotely via VPN.

**Phase 1 Now Includes:**
- ✅ Local camera viewing (at the club)
- ✅ Remote camera viewing (from home/anywhere) via WireGuard VPN
- ✅ Single camera proof-of-concept
- ✅ Complete security implementation
- ✅ Easy setup on one local PC

**Phase 1 Security Posture:**
- ✅ Secure VPN tunnel for remote access (WireGuard)
- ✅ NVR never exposed to internet (only VPN port 51820 open)
- ✅ No cloud dependencies
- ✅ Credentials encrypted with Windows DPAPI
- ✅ Per-user VPN access control
- ✅ Full audit logging from day one


## 🏗️ Technical Checklist

### 1. 🔐 Security Foundation (MUST BE FIRST)

#### A) Network Security Verification
- [ ] **Verify NVR Configuration**:
    - Confirm NVR IP is 192.168.1.64 (private, LAN-only)
    - Verify NO port forwarding exists on router for:
      - Port 554 (RTSP)
      - Port 8000 (HTTP API)
      - Any other NVR ports
    - Confirm UPnP is disabled on router
    - Document current NVR cloud/DDNS status (to be disabled in Phase 2)

#### B) Credential Security Setup
- [ ] **Implement Secure Credential Storage**:
    - Create `SecureCredentialManager` class in Video Agent
    - Use Windows DPAPI for encryption at rest
    - Store NVR credentials in encrypted config file
    - **NEVER** expose credentials via API or logs
    - Change NVR admin password from installer default
    - Document password in secure location (password manager)

#### C) Access Control Foundation
- [ ] **Implement Authentication Requirements**:
    - Camera viewer page requires user login
    - No anonymous/guest access to cameras
    - Session-based authorization
    - Role-based permissions (prepare for Phase 5)

---

### 2. 🎨 UI Integration & Visual Tracking

#### A) Dashboard Integration
- [ ] **Modify Dashboard.razor**: [**TAG: MODIFIED**]
    - Add "Cameras" button to top action bar
    - Implement status LED component:
      - 🟢 Green: Video Agent connected, NVR reachable
      - 🟡 Yellow: Video Agent connected, NVR unreachable
      - 🔴 Red: Video Agent offline
      - ⚪ Gray: Never connected
    - Add tooltip showing last heartbeat time
    - Display connection status text

#### B) Camera Viewer Page
- [ ] **Create CameraViewer.razor**: [**TAG: NEW**]
    - **Authentication Check**: Redirect to login if not authenticated
    - **Plain-English Status Window**:
      - "Connecting to Video Agent..."
      - "Video Agent connected ✓"
      - "Probing NVR at 192.168.1.64..."
      - "NVR found ✓"
      - "Authenticating with NVR..."
      - "Login valid ✓"
      - "Starting video stream..."
      - "Video active ✓"
    - **Single Camera Video Player**:
      - HLS.js implementation for Camera 1
      - Loading skeleton while connecting
      - Error state with retry button
      - Bandwidth indicator
    - **Connection Info Panel**:
      - Camera name/number
      - Stream resolution
      - Bitrate
      - Latency
      - Connection duration

#### C) Navigation Updates
- [ ] **Modify NavMenu.razor**: [**TAG: MODIFIED**]
    - Add "Cameras" link under Controllers section
    - Show status LED next to link
    - Temporary placement (will move in Phase 3)

#### D) Visual Tracking Styles
- [ ] **Update app.css**:
    - Ensure `.gfc-modified-tag` exists
    - Ensure `.gfc-new-tag` exists
    - Add camera-specific status LED styles

---

### 3. 🖥️ Video Agent Service (C# Windows Service)

#### A) Core Service Structure
- [ ] **Create Video Agent Project**:
    - New .NET 8 Windows Service project
    - Name: `GFC.VideoAgent`
    - Install NuGet packages:
      - `Microsoft.Extensions.Hosting.WindowsServices`
      - `Serilog` (for logging)
      - `System.Security.Cryptography` (for DPAPI)

#### B) Heartbeat & Status API
- [ ] **Implement Heartbeat Endpoint**:
    - HTTP endpoint: `GET /api/heartbeat`
    - Returns JSON:
      ```json
      {
        "status": "online",
        "nvrReachable": true,
        "lastNvrCheck": "2025-12-21T11:50:00Z",
        "activeStreams": 0,
        "version": "1.0.0"
      }
      ```
    - Update every 10 seconds
    - Include NVR ping test

#### C) Secure Credential Management
- [ ] **Implement SecureCredentialManager**:
    - Method: `EncryptAndStore(string username, string password)`
    - Method: `DecryptAndRetrieve()` → returns credentials
    - Uses Windows DPAPI (`ProtectedData.Protect`)
    - Credentials stored in: `%ProgramData%\GFC\VideoAgent\credentials.dat`
    - **Never** log credentials (even encrypted)

#### D) NVR Probe & Discovery
- [ ] **Implement Automated NVR Probe**:
    - **Step 1: Network Check**
      - Ping 192.168.1.64
      - Log: "NVR reachable" or "NVR unreachable"
    - **Step 2: HTTP API Login**
      - POST to NVR login endpoint
      - Validate credentials
      - Store session token (if applicable)
      - Log: "NVR login successful" or "Authentication failed"
    - **Step 3: Camera Discovery**
      - GET camera list from NVR API
      - Parse camera names, IDs, RTSP URLs
      - Cache camera metadata
      - Log: "Found X cameras"
    - **Step 4: RTSP Validation**
      - Test RTSP connection to Camera 1 (sub-stream)
      - Verify stream is accessible
      - Log: "RTSP stream valid" or "Stream unavailable"

#### E) FFmpeg Transcoding
- [ ] **Implement HLS Transcoding**:
    - **FFmpeg Command Template**:
      ```bash
      ffmpeg -rtsp_transport tcp -i rtsp://192.168.1.64:554/stream1 
             -c:v copy -c:a aac -f hls 
             -hls_time 2 -hls_list_size 5 
             -hls_flags delete_segments 
             output.m3u8
      ```
    - **Process Management**:
      - Track FFmpeg process ID
      - Monitor for crashes
      - Auto-restart on failure (max 3 attempts)
      - Log all FFmpeg output
    - **HLS Output Location**:
      - `%ProgramData%\GFC\VideoAgent\streams\camera1\`
      - Serve via HTTP endpoint: `GET /streams/camera1/output.m3u8`
    - **Cleanup**:
      - Delete old segments after 30 seconds
      - Kill FFmpeg on stream stop

#### F) Stream Management API
- [ ] **Implement Stream Control Endpoints**:
    - `POST /api/stream/start` - Start stream for camera
      - Requires: `cameraId`, `sessionToken`
      - Returns: HLS URL
      - Logs: User, camera, start time
    - `POST /api/stream/stop` - Stop stream
      - Requires: `sessionToken`
      - Kills FFmpeg process
      - Logs: User, camera, stop time, duration
    - `GET /api/stream/status` - Get stream status
      - Returns: active streams, uptime, bitrate

#### G) Security & API Key Validation
- [ ] **Implement API Key Authentication**:
    - Generate shared secret key (GUID)
    - Store in Video Agent config (encrypted)
    - Store in Web App config (encrypted)
    - Validate on every API call
    - Reject requests without valid key
    - Log all authentication failures

---

### 4. 🌐 Web App Integration

#### A) Video Agent Client Service
- [ ] **Create VideoAgentClient.cs**: [**TAG: NEW**]
    - Method: `Task<HeartbeatResponse> GetHeartbeatAsync()`
    - Method: `Task<string> StartStreamAsync(int cameraId)`
    - Method: `Task StopStreamAsync(string sessionToken)`
    - Method: `Task<StreamStatus> GetStreamStatusAsync()`
    - Include API key in all requests
    - Handle connection failures gracefully
    - Implement retry logic (3 attempts, exponential backoff)

#### B) Camera Viewer Component Logic
- [ ] **Implement CameraViewer.razor.cs**: [**TAG: NEW**]
    - `OnInitializedAsync()`:
      - Check user authentication
      - Call `VideoAgentClient.GetHeartbeatAsync()`
      - Update status window
      - Call `VideoAgentClient.StartStreamAsync(1)`
      - Initialize HLS.js player
    - `OnAfterRenderAsync()`:
      - Load HLS stream
      - Handle player errors
    - `DisposeAsync()`:
      - Call `VideoAgentClient.StopStreamAsync()`
      - Clean up HLS player

#### C) HLS.js Integration
- [ ] **Implement HLS Video Player**:
    - Include HLS.js library (CDN or local)
    - JavaScript interop for player control
    - Error handling and retry logic
    - Display loading state
    - Show connection quality indicator

---

### 5. 🛡️ Security & Session Management

#### A) Inactivity Timeout
- [ ] **Implement "Still Working?" Modal**: [**TAG: NEW**]
    - Trigger after 5 minutes of no mouse/keyboard activity
    - Modal displays:
      - "Are you still viewing cameras?"
      - "Session will end in 60 seconds"
      - [Continue Watching] [End Session]
    - If no response in 60 seconds:
      - Auto-stop stream
      - Log session timeout
      - Redirect to dashboard

#### B) Auto-Kill Logic
- [ ] **Implement Stream Cleanup**:
    - **On page unload**: Call `StopStreamAsync()`
    - **On browser close**: Use `beforeunload` event
    - **On timeout**: Auto-stop via modal
    - **On logout**: Stop all user streams
    - **Verify cleanup**: Check FFmpeg processes are killed

#### C) Session Tracking
- [ ] **Implement Session Management**:
    - Generate unique session token per stream
    - Track: userId, cameraId, startTime, ipAddress
    - Store in memory (Video Agent)
    - Expire sessions after 30 minutes max
    - One session per user per camera

---

### 6. 📊 Audit Logging (Phase 1 Foundation)

#### A) Video Agent Logging
- [ ] **Implement Comprehensive Logging**:
    - Log to: `%ProgramData%\GFC\VideoAgent\logs\`
    - Use Serilog with daily rolling files
    - Log levels: Debug, Info, Warning, Error
    - **Log ALL of the following**:
      - Service start/stop
      - Heartbeat checks
      - NVR probe results
      - Stream start (user, camera, time)
      - Stream stop (user, camera, duration)
      - Authentication failures
      - FFmpeg errors
      - API key validation failures

#### B) Web App Logging
- [ ] **Implement Camera Access Logging**:
    - Log to existing GFC audit system
    - **Log ALL of the following**:
      - Camera viewer page access (user, time)
      - Stream requests (user, camera, time)
      - Session timeouts (user, camera, duration)
      - Errors and failures

---

### 7. 🧪 Testing & Validation

#### A) Security Testing
- [ ] **Verify Security Requirements**:
    - ✅ NVR not accessible from internet
    - ✅ No port forwarding configured
    - ✅ Credentials encrypted at rest
    - ✅ Credentials never in browser/logs
    - ✅ API key required for all requests
    - ✅ Authentication required for viewer
    - ✅ Audit logs capture all access

#### B) Connectivity Testing
- [ ] **Test Connection Flow**:
    - Video Agent starts successfully
    - Heartbeat returns "online"
    - NVR probe succeeds
    - Camera 1 stream starts
    - HLS player loads video
    - Video plays smoothly (< 3 second latency)
    - Stream stops cleanly

#### C) Cleanup Testing
- [ ] **Test Auto-Kill Logic**:
    - Close browser → FFmpeg killed
    - Timeout modal → FFmpeg killed
    - Logout → FFmpeg killed
    - Page navigation → FFmpeg killed
    - Verify no orphaned processes

#### D) Error Handling Testing
- [ ] **Test Failure Scenarios**:
    - Video Agent offline → Red LED, error message
    - NVR unreachable → Yellow LED, retry option
    - Invalid credentials → Error logged, alert shown
    - FFmpeg crash → Auto-restart, log error
    - Network interruption → Reconnect attempt

---

## 🚀 Success Criteria

### Functional Requirements
- [ ] **Visual Tracking**: All new/modified elements tagged with [NEW] or [MODIFIED]
- [ ] **Connection**: Dashboard LED shows Green when Agent + NVR are online
- [ ] **Automation**: Clicking "Cameras" automatically:
  - Checks authentication
  - Probes NVR
  - Starts stream
  - Displays video
- [ ] **Human-Readable Status**: Status window shows clear, plain-English messages during connection
- [ ] **Video Playback**: Camera 1 video plays with < 3 second latency
- [ ] **Cleanup**: FFmpeg processes confirmed dead after page close/timeout

### Security Requirements (NON-NEGOTIABLE)
- [ ] **Zero Internet Exposure**: NVR remains LAN-only
- [ ] **Credential Security**: Credentials encrypted, never exposed
- [ ] **Authentication**: Login required before camera access
- [ ] **API Security**: API key validated on all requests
- [ ] **Audit Logging**: All access logged with user/time/camera
- [ ] **Session Management**: Inactivity timeout works correctly
- [ ] **Process Cleanup**: No orphaned FFmpeg processes

### Performance Requirements
- [ ] **Heartbeat Response**: < 500ms
- [ ] **Stream Start Time**: < 3 seconds
- [ ] **Video Latency**: < 3 seconds behind live
- [ ] **CPU Usage**: < 10% per stream (Video Agent)
- [ ] **Memory Usage**: < 200MB per stream (Video Agent)

---

## 📋 Phase 1 Deliverables

1. ✅ **Video Agent Windows Service** (fully functional)
2. ✅ **Camera Viewer Page** (single camera, HLS playback)
3. ✅ **Dashboard Integration** (status LED, cameras button)
4. ✅ **Security Implementation** (credentials, API key, auth)
5. ✅ **Audit Logging** (comprehensive access logs)
6. ✅ **Documentation** (setup guide, security verification)

---

## 🔄 Next Steps After Phase 1

Once Phase 1 is verified and stable:
- **Phase 2**: Multi-camera grid, modern UI, PTZ controls
- **Phase 3**: Video playback timeline, clip downloads
- **Phase 4**: Archive management, retention policies
- **Phase 5**: VPN setup, remote access, advanced security

---

**Document Version:** 2.0 (Security-Enhanced)  
**Last Updated:** 2025-12-21  
**Security Compliance:** Aligned with `CAMERA_SECURITY_MASTER_GUIDE.md`
