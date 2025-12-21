# Camera System - Jules Implementation Task

**Project:** GFC Camera System Integration  
**Priority:** High  
**Estimated Time:** 8-12 hours  
**Dependencies:** None (new feature)

---

## 🎯 Overview

Implement a camera viewing system for the GFC Web App that allows secure viewing of NVR cameras both locally and remotely. This will be implemented in phases, starting with a verification system and basic single-camera viewer.

---

## 📋 What Jules Needs to Create

### **Phase 0: Verification System** (Create FIRST)
**Priority:** 1  
**Time:** 3-4 hours

Create a pre-implementation verification page that tests all connectivity and feasibility before full setup.

#### **Files to Create:**

1. **`Pages/CameraSystemVerification.razor`**
   - New Blazor page at route `/camera-verification`
   - Visual status dashboard with real-time updates
   - Plain-English status messages
   - Runs automated tests
   - Exports verification report

2. **`Services/CameraVerificationService.cs`**
   - Handles all verification tests
   - Tests network connectivity
   - Tests NVR accessibility
   - Tests video streaming capability
   - Tests VPN feasibility
   - Returns detailed results

3. **`Models/VerificationResult.cs`**
   - Data model for test results
   - Properties: TestName, Status, Message, Details, Timestamp

#### **Verification Tests to Implement:**

**Test 1: Network Connectivity**
```csharp
- Ping local network (192.168.1.1)
- Ping NVR (192.168.1.64)
- Check internet connection
- Measure latency
```

**Test 2: NVR Authentication**
```csharp
- Attempt HTTP connection to NVR (192.168.1.64:8000)
- Test login with credentials
- Retrieve camera list
- Verify RTSP URLs available
```

**Test 3: Video Streaming**
```csharp
- Test RTSP stream accessibility
- Verify FFmpeg can convert (if installed)
- Test HLS playback capability
- Measure stream latency
```

**Test 4: Remote Access Feasibility**
```csharp
- Check if port forwarding is possible
- Verify VPN software availability
- Test network path from web app server
```

**Test 5: Security Verification**
```csharp
- Scan for exposed ports
- Verify NVR not accessible from internet
- Check firewall status
- Validate encryption availability
```

**Test 6: Phase Feasibility**
```csharp
- Verify Phase 1 requirements met
- Check Phase 2 multi-camera capability
- Validate Phase 3 playback availability
- Confirm Phase 4 archive access
- Verify Phase 5 security features
```

#### **UI Requirements:**

**Visual Status Dashboard:**
```
┌─────────────────────────────────────────────────────────────────┐
│  GFC Camera System - Pre-Implementation Verification            │
│  Status: Testing...                                             │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│  NETWORK CONNECTIVITY                                           │
├─────────────────────────────────────────────────────────────────┤
│  ✅ Internet Connection                    OK (73.45.123.89)    │
│  ✅ Local Network                          OK (192.168.1.x)     │
│  ✅ Router Accessible                      OK (192.168.1.1)     │
│  ⏳ NVR Reachable                          Testing...           │
│  ⏳ NVR Login                              Testing...           │
└─────────────────────────────────────────────────────────────────┘

[Similar sections for other test categories]

[Run Verification] [Export Report] [View Logs]
```

**Status Indicators:**
- ✅ Green checkmark = Passed
- ❌ Red X = Failed
- ⏳ Spinner = Testing
- ⚠️ Warning = Passed with warnings

**Plain-English Messages:**
- "Checking internet connection..."
- "Internet connected (Public IP: 73.45.123.89)"
- "Pinging NVR..."
- "NVR reachable (Response time: 2ms)"
- "Testing NVR login..."
- "Login successful! Found 16 cameras"

---

### **Phase 1: Camera Viewer Components** (Create SECOND)
**Priority:** 2  
**Time:** 4-6 hours

Create the actual camera viewing functionality.

#### **Files to Create:**

1. **`Pages/CameraViewer.razor`**
   - New Blazor page at route `/cameras`
   - Single camera live view
   - Plain-English status window
   - HLS video player
   - Connection info panel

2. **`Services/VideoAgentClient.cs`**
   - HTTP client for Video Agent API
   - Methods: GetHeartbeat(), StartStream(), StopStream(), GetStatus()
   - API key authentication
   - Retry logic with exponential backoff

3. **`Models/CameraModels.cs`**
   - HeartbeatResponse
   - StreamStatus
   - CameraInfo

4. **`Components/CameraStatusLED.razor`**
   - Reusable status LED component
   - Shows: 🟢 Green, 🟡 Yellow, 🔴 Red, ⚪ Gray
   - Tooltip with last heartbeat time

5. **`Components/PlainEnglishStatusWindow.razor`**
   - Shows connection progress
   - Messages like "Connecting to Video Agent...", "Video active ✓"
   - Auto-scrolls to latest message

6. **`wwwroot/js/hls-player.js`**
   - HLS.js integration
   - Video player control
   - Error handling

#### **Dashboard Integration:**

**Modify `Dashboard.razor`:**
- Add "Cameras" button to top action bar
- Add CameraStatusLED component
- Show connection status

**Modify `NavMenu.razor`:**
- Add "Cameras" link under Controllers section
- Show status LED next to link

#### **Configuration:**

**Add to `appsettings.json`:**
```json
{
  "VideoAgent": {
    "BaseUrl": "http://10.0.0.1:5100",
    "ApiKey": "YOUR_API_KEY_HERE",
    "HeartbeatIntervalSeconds": 10,
    "StreamTimeoutMinutes": 5
  }
}
```

---

## 📊 Implementation Order

**Step 1: Phase 0 - Verification (FIRST)**
1. Create `CameraVerificationService.cs`
2. Create `VerificationResult.cs` model
3. Create `CameraSystemVerification.razor` page
4. Add link to Dashboard
5. Test all verification tests
6. Ensure plain-English messages work
7. Test export report functionality

**Step 2: Phase 1 - Camera Viewer (SECOND)**
1. Create `VideoAgentClient.cs`
2. Create camera models
3. Create `CameraStatusLED.razor` component
4. Create `PlainEnglishStatusWindow.razor` component
5. Create `CameraViewer.razor` page
6. Integrate HLS.js player
7. Modify Dashboard and NavMenu
8. Add configuration section
9. Test end-to-end

---

## 🔧 Technical Specifications

### **Video Agent API Endpoints:**

**Heartbeat:**
```
GET /api/heartbeat
Response: {
  "status": "online",
  "nvrReachable": true,
  "lastNvrCheck": "2025-12-21T12:00:00Z",
  "activeStreams": 0,
  "version": "1.0.0"
}
```

**Start Stream:**
```
POST /api/stream/start
Body: {
  "cameraId": 1,
  "sessionToken": "user-session-guid"
}
Response: {
  "hlsUrl": "http://10.0.0.1:5100/streams/camera1/output.m3u8",
  "sessionToken": "stream-session-guid"
}
```

**Stop Stream:**
```
POST /api/stream/stop
Body: {
  "sessionToken": "stream-session-guid"
}
Response: {
  "success": true
}
```

**Stream Status:**
```
GET /api/stream/status?sessionToken=xyz
Response: {
  "active": true,
  "uptime": 120,
  "bitrate": 512000,
  "latency": 2.3
}
```

### **Authentication:**

All API calls must include:
```
Headers: {
  "X-API-Key": "configured-api-key"
}
```

### **Error Handling:**

- Network errors: Show retry button
- Authentication errors: Show error message
- Stream errors: Auto-retry 3 times
- Timeout: Show "Still Working?" modal after 5 minutes

---

## 🎨 UI/UX Requirements

### **Visual Tracking:**
- All new components tagged with `[NEW]` in comments
- All modified components tagged with `[MODIFIED]` in comments

### **Styling:**
- Use existing GFC Studio dark theme
- Status LED colors: Green (#28a745), Yellow (#ffc107), Red (#dc3545), Gray (#6c757d)
- Smooth transitions between states
- Loading skeletons while connecting

### **Responsive:**
- Works on desktop (primary)
- Mobile support (Phase 2)

---

## ✅ Success Criteria

### **Phase 0 - Verification:**
- [ ] Verification page loads
- [ ] All 6 test categories run
- [ ] Status updates in real-time
- [ ] Plain-English messages display
- [ ] Can export report
- [ ] Shows pass/fail for each test
- [ ] Identifies any blockers

### **Phase 1 - Camera Viewer:**
- [ ] Dashboard shows "Cameras" button
- [ ] Status LED shows correct state
- [ ] Camera viewer page loads
- [ ] Status window shows connection progress
- [ ] Video plays within 3 seconds
- [ ] Can stop stream cleanly
- [ ] Inactivity timeout works
- [ ] All access is logged

---

## 📁 File Structure

```
GFC-Studio V2/
├── Pages/
│   ├── CameraSystemVerification.razor     [NEW]
│   └── CameraViewer.razor                  [NEW]
├── Services/
│   ├── CameraVerificationService.cs        [NEW]
│   └── VideoAgentClient.cs                 [NEW]
├── Models/
│   ├── VerificationResult.cs               [NEW]
│   └── CameraModels.cs                     [NEW]
├── Components/
│   ├── CameraStatusLED.razor               [NEW]
│   └── PlainEnglishStatusWindow.razor      [NEW]
├── wwwroot/
│   └── js/
│       └── hls-player.js                   [NEW]
├── Dashboard.razor                         [MODIFIED]
├── NavMenu.razor                           [MODIFIED]
└── appsettings.json                        [MODIFIED]
```

---

## 🔒 Security Requirements

- [ ] Authentication required before camera access
- [ ] API key validated on all Video Agent calls
- [ ] Session tokens used for stream management
- [ ] No credentials in browser/JavaScript
- [ ] All camera access logged to audit system
- [ ] Rate limiting on video endpoints
- [ ] Security headers enabled

---

## 📚 Reference Documentation

**See these files in `docs/camera-system/`:**
- `CAMERA_PHASE_0_VERIFICATION_PLAN.md` - Complete verification specs
- `CAMERA_PHASE_1_CONNECTIVITY_PROOF_PLAN.md` - Phase 1 technical specs
- `CAMERA_SECURITY_MASTER_GUIDE.md` - Security requirements
- `CAMERA_ARCHITECTURE_DIAGRAM.md` - System architecture

---

## 🚀 Deployment Notes

**After Jules completes:**
1. Review all created files
2. Test verification page thoroughly
3. Run verification tests
4. Review results
5. If all tests pass, proceed with local PC setup
6. Test camera viewer with actual Video Agent
7. Verify security requirements met

---

## 💡 Notes for Jules

**Important:**
- Phase 0 (Verification) MUST be created first
- Phase 1 depends on Phase 0 passing
- Use plain-English messages throughout
- Visual feedback is critical
- Error handling is essential
- Security is non-negotiable

**Testing:**
- Mock Video Agent responses for initial testing
- Test all error scenarios
- Verify timeout handling
- Test on different browsers

**Code Quality:**
- Follow existing GFC Studio patterns
- Use dependency injection
- Implement proper logging
- Add XML documentation comments
- Handle all edge cases

---

**Questions?** See documentation in `docs/camera-system/` folder.
