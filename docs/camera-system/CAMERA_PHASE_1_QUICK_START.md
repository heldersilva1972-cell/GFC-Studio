# Phase 1 Setup - Visual Quick Start

**Time Required:** 20-25 minutes  
**Difficulty:** Easy  
**Technical Skill Required:** Basic Windows administration

---

## 🎯 The Big Picture

```
┌─────────────────────────────────────────────────────────────────┐
│                    YOUR CLUB NETWORK (LAN)                      │
│                                                                 │
│  ┌──────────────┐         ┌──────────────┐                     │
│  │   Your PC    │         │  Club PC     │                     │
│  │  (Browser)   │         │ (Video Agent)│                     │
│  │              │         │              │                     │
│  │  GFC Web App │ ◄─────► │  Converts    │                     │
│  │  + Camera    │  HTTP   │  RTSP → HLS  │                     │
│  │  Viewer      │         │              │                     │
│  └──────────────┘         └──────┬───────┘                     │
│                                  │                              │
│                                  │ RTSP                         │
│                                  │ (LAN only)                   │
│                                  ▼                              │
│                           ┌──────────────┐                      │
│                           │     NVR      │                      │
│                           │ 192.168.1.64 │                      │
│                           │              │                      │
│                           │  📹📹📹📹    │                      │
│                           │  16 Cameras  │                      │
│                           └──────────────┘                      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  │ ❌ NO CONNECTION
                                  │ ❌ NO PORT FORWARDING
                                  ▼
                            [ INTERNET ]
```

---

## 📋 Setup Checklist (In Order)

### ✅ STEP 1: Pre-Flight Check (5 min)

**What you're checking:** Make sure everything is ready

```
[ ] NVR is powered on and working
[ ] NVR IP is 192.168.1.64
[ ] You can ping 192.168.1.64 from club PC
[ ] You know NVR admin username and password
[ ] Club PC is Windows 10 or newer
[ ] Club PC has at least 4GB RAM
```

**How to ping NVR:**
1. Open Command Prompt (type `cmd` in Windows search)
2. Type: `ping 192.168.1.64`
3. Should see: `Reply from 192.168.1.64: bytes=32 time<1ms`
4. If you see "Request timed out" → check NVR network cable

---

### ✅ STEP 2: Install FFmpeg (5 min)

**What is FFmpeg?** The video conversion tool Video Agent uses

**Option A: Automatic Install (Easiest)**
```powershell
# Run PowerShell as Administrator
winget install ffmpeg
```

**Option B: Manual Install**
1. Download: https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip
2. Extract to: `C:\ffmpeg\`
3. Add to PATH:
   - Open System Properties → Environment Variables
   - Edit "Path" variable
   - Add: `C:\ffmpeg\bin`
4. Restart Command Prompt

**Verify it works:**
```cmd
ffmpeg -version
```
Should show version info (e.g., "ffmpeg version 6.1")

---

### ✅ STEP 3: Install Video Agent Service (5 min)

**What you're installing:** The background service that connects to NVR

**Installation Steps:**
1. Download `GFC.VideoAgent.Installer.msi`
2. Double-click to run installer
3. Click "Next" through wizard
4. When prompted, enter:
   - **NVR IP Address:** 192.168.1.64
   - **NVR Username:** [your NVR admin username]
   - **NVR Password:** [your NVR admin password]
   - **API Key:** [auto-generated, copy this down!]
5. Click "Install"
6. Wait for "Installation Complete"
7. Click "Finish"

**What just happened:**
- Service installed to: `C:\Program Files\GFC\VideoAgent\`
- Credentials encrypted and stored
- Service started automatically
- Now running in background

**Verify it's running:**
1. Open Windows Services (type `services.msc` in Windows search)
2. Find "GFC Video Agent"
3. Status should be "Running"
4. Startup Type should be "Automatic"

---

### ✅ STEP 4: Configure GFC Web App (3 min)

**What you're doing:** Tell the web app where to find Video Agent

**Edit appsettings.json:**
```json
{
  "VideoAgent": {
    "BaseUrl": "http://localhost:5100",
    "ApiKey": "[paste the API key from Step 3]",
    "HeartbeatIntervalSeconds": 10
  }
}
```

**If Video Agent is on a different PC:**
```json
{
  "VideoAgent": {
    "BaseUrl": "http://192.168.1.100:5100",  // ← IP of Video Agent PC
    "ApiKey": "[paste the API key from Step 3]",
    "HeartbeatIntervalSeconds": 10
  }
}
```

**Restart GFC Web App:**
```cmd
# If running as Windows Service:
net stop "GFC Web App"
net start "GFC Web App"

# If running via dotnet:
# Just stop and restart the dotnet run command
```

---

### ✅ STEP 5: Test the Connection (2 min)

**What you're testing:** Everything works end-to-end

**Test Steps:**
1. Open browser
2. Navigate to GFC Web App (e.g., http://localhost:5000)
3. Log in with your credentials
4. Look at Dashboard
5. **Check Status LED:**
   - 🟢 Green = SUCCESS! Everything working
   - 🟡 Yellow = Video Agent running, but can't reach NVR
   - 🔴 Red = Video Agent not running
   - ⚪ Gray = Web app can't reach Video Agent

**If Green (🟢):**
6. Click "Cameras" button
7. Watch status window:
   ```
   ✓ Connecting to Video Agent...
   ✓ Video Agent connected
   ✓ Probing NVR at 192.168.1.64...
   ✓ NVR found
   ✓ Authenticating with NVR...
   ✓ Login valid
   ✓ Starting video stream...
   ✓ Video active
   ```
8. See live video from Camera 1
9. **SUCCESS! Phase 1 is working! 🎉**

---

## 🐛 Troubleshooting Guide

### Problem: Status LED is Red 🔴

**Meaning:** Web app can't reach Video Agent

**Fixes to try:**
1. Check Video Agent service is running:
   - Open Services → Find "GFC Video Agent" → Should be "Running"
   - If not, right-click → Start
2. Check firewall:
   - Windows Firewall might be blocking port 5100
   - Add exception for `GFC.VideoAgent.exe`
3. Check `appsettings.json`:
   - Verify `BaseUrl` is correct
   - Verify `ApiKey` matches what installer generated
4. Check Video Agent logs:
   - Open: `C:\ProgramData\GFC\VideoAgent\logs\`
   - Look for errors in latest log file

---

### Problem: Status LED is Yellow 🟡

**Meaning:** Video Agent running, but can't reach NVR

**Fixes to try:**
1. Ping NVR from Video Agent PC:
   ```cmd
   ping 192.168.1.64
   ```
   - Should get replies
   - If not, check NVR network cable
2. Check NVR is powered on and booted
3. Check NVR credentials:
   - Try logging into NVR web interface: http://192.168.1.64:8000
   - Use same username/password you gave to installer
   - If login fails, credentials are wrong
4. Re-run Video Agent installer to update credentials

---

### Problem: Status LED is Green, but video won't load

**Meaning:** Connection works, but stream failed

**Fixes to try:**
1. Check status window for specific error
2. Check FFmpeg is installed:
   ```cmd
   ffmpeg -version
   ```
3. Check Video Agent logs for FFmpeg errors:
   - `C:\ProgramData\GFC\VideoAgent\logs\`
4. Try restarting Video Agent service
5. Check camera is online in NVR interface

---

### Problem: Video is choppy or laggy

**Meaning:** Performance issue

**Fixes to try:**
1. Check Video Agent PC CPU usage:
   - Should be < 10% per camera
   - If higher, PC may be underpowered
2. Check network bandwidth:
   - One camera uses ~500 Kbps
   - Check for network congestion
3. Use wired Ethernet instead of Wi-Fi
4. Close other programs on Video Agent PC

---

## 📊 What Success Looks Like

### Dashboard View:
```
┌─────────────────────────────────────────────────────────┐
│  GFC Studio - Dashboard                                 │
│                                                          │
│  [Members] [Access] [Controllers] [🎥 Cameras]          │
│                                                          │
│  🟢 Video System Online                                 │
│  Last check: 2 seconds ago                              │
│  Active streams: 0                                      │
└─────────────────────────────────────────────────────────┘
```

### Camera Viewer:
```
┌─────────────────────────────────────────────────────────┐
│  Camera Viewer - Live View                              │
│                                                          │
│  Status: ✓ All systems operational                      │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │                                                   │  │
│  │         [SMOOTH LIVE VIDEO PLAYING]              │  │
│  │                                                   │  │
│  │         Latency: 2.1 seconds                     │  │
│  │                                                   │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  Camera 1: Front Entrance | 640x480 | 512 Kbps         │
│  Duration: 00:01:23                                     │
└─────────────────────────────────────────────────────────┘
```

---

## 🎓 Understanding the Components

### What is the "Video Agent"?

Think of it like a translator:
- **NVR speaks:** RTSP (camera language)
- **Browser speaks:** HLS (web language)
- **Video Agent translates:** RTSP → HLS

It also acts as a security guard:
- Keeps NVR credentials safe
- Only allows authenticated users
- Logs who watches what

### What is HLS?

**HLS = HTTP Live Streaming**
- Apple's web video format
- Works in all modern browsers
- No plugins needed
- Adaptive quality

### What is RTSP?

**RTSP = Real-Time Streaming Protocol**
- Camera/NVR native format
- High quality, low latency
- Not browser-compatible (that's why we need Video Agent)

### What is FFmpeg?

**FFmpeg = Video Swiss Army Knife**
- Converts video formats
- Free and open-source
- Industry standard
- Used by YouTube, Netflix, etc.

---

## 💾 Where Everything Lives

### On Video Agent PC:

```
C:\Program Files\GFC\VideoAgent\
├── GFC.VideoAgent.exe          (The service)
├── appsettings.json            (Configuration)
└── ffmpeg.exe                  (Video converter)

C:\ProgramData\GFC\VideoAgent\
├── credentials.dat             (Encrypted NVR credentials)
├── logs\                       (Service logs)
│   └── videoagent-20251221.log
└── streams\                    (Temporary HLS files)
    └── camera1\
        ├── output.m3u8         (Playlist)
        ├── segment0.ts         (Video chunk)
        ├── segment1.ts
        └── segment2.ts
```

### On GFC Web App:

```
GFC-Studio V2\
├── Pages\
│   └── CameraViewer.razor      (Camera viewer page)
├── Services\
│   └── VideoAgentClient.cs     (Talks to Video Agent)
└── appsettings.json            (Video Agent URL + API Key)
```

---

## 🔒 Security Checklist

After setup, verify:

- [ ] NVR is NOT accessible from internet
- [ ] No port forwarding to NVR (554, 8000, etc.)
- [ ] UPnP is disabled on router
- [ ] NVR admin password is NOT default
- [ ] Video Agent credentials file is encrypted
- [ ] Only authenticated users can view cameras
- [ ] All camera views are being logged

---

## 📞 Getting Help

**If you're stuck:**

1. **Check the logs:**
   - Video Agent: `C:\ProgramData\GFC\VideoAgent\logs\`
   - Look for ERROR or WARN messages

2. **Check the services:**
   - Video Agent should be "Running"
   - GFC Web App should be "Running"

3. **Check the network:**
   - Can you ping 192.168.1.64?
   - Can you access NVR web interface?

4. **Review the guides:**
   - `CAMERA_PHASE_1_SIMPLE_GUIDE.md` (this file)
   - `CAMERA_PHASE_1_PRE_IMPLEMENTATION_CHECKLIST.md`

---

## ✅ Phase 1 Complete Checklist

You're done when you can check all these:

- [ ] Video Agent service is installed and running
- [ ] FFmpeg is installed and working
- [ ] Dashboard shows 🟢 green status LED
- [ ] Clicking "Cameras" loads the viewer page
- [ ] Status window shows all checkmarks (✓)
- [ ] Live video from Camera 1 appears within 3 seconds
- [ ] Video plays smoothly (< 3 second latency)
- [ ] Closing the page stops the stream
- [ ] Inactivity timeout works (5 min warning)
- [ ] All views are logged in audit system

**All checked? Congratulations! Phase 1 is complete! 🎉**

---

**Next:** Phase 2 - Multi-Camera Grid & Modern UI

**Questions?** See `CAMERA_DOCUMENTATION_INDEX.md` for all guides
