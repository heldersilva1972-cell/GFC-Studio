# Camera Phase 1 - Simple Guide: What You'll Actually See & Do

**For:** Club owners, managers, non-technical users  
**Purpose:** Understand Phase 1 in plain English  
**Time to Read:** 5 minutes

---

## 🎯 What Is Phase 1?

**Phase 1 Goal:** Prove that we can view ONE camera from the GFC Web App, securely, on the local network only.

**What it's NOT:**
- ❌ Not a full camera system yet
- ❌ Not accessible from home (that's Phase 2)
- ❌ Not all 16 cameras at once (that's Phase 2)
- ❌ Not playback or downloads (that's Phase 3)

**What it IS:**
- ✅ A proof-of-concept test
- ✅ One camera, live view only
- ✅ Works only when you're AT the club (on club Wi-Fi)
- ✅ Completely safe and secure

---

## 👀 What You'll Actually See

### 1. **On the Dashboard** (Main Page)

When you log into the GFC Web App, you'll see:

```
┌─────────────────────────────────────────────────┐
│  GFC Studio - Dashboard                         │
│                                                  │
│  [Members] [Access] [Controllers] [🎥 Cameras]  │ ← NEW BUTTON
│                                                  │
│  Status: 🟢 Video System Online                 │ ← NEW STATUS LED
└─────────────────────────────────────────────────┘
```

**The Status LED tells you:**
- 🟢 **Green** = Everything working, cameras ready
- 🟡 **Yellow** = Video service running, but NVR not responding
- 🔴 **Red** = Video service is offline
- ⚪ **Gray** = Never connected yet

---

### 2. **When You Click "Cameras"**

You'll see a new page with:

```
┌─────────────────────────────────────────────────────────────┐
│  Camera Viewer - Live View                                  │
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Status Window:                                        │ │
│  │  ✓ Connecting to Video Agent...                       │ │
│  │  ✓ Video Agent connected                              │ │
│  │  ✓ Probing NVR at 192.168.1.64...                     │ │
│  │  ✓ NVR found                                           │ │
│  │  ✓ Authenticating with NVR...                         │ │
│  │  ✓ Login valid                                         │ │
│  │  ✓ Starting video stream...                           │ │
│  │  ✓ Video active - Camera 1 (Front Entrance)           │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │                                                        │ │
│  │                                                        │ │
│  │         [LIVE VIDEO FROM CAMERA 1]                    │ │
│  │                                                        │ │
│  │                                                        │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  Camera: Front Entrance | Resolution: 640x480               │
│  Latency: 2.1s | Bitrate: 512 Kbps | Duration: 00:02:34    │
└─────────────────────────────────────────────────────────────┘
```

**That's it!** Simple live view of one camera.

---

### 3. **If You're Idle for 5 Minutes**

A popup appears:

```
┌─────────────────────────────────────────┐
│  Still Watching?                        │
│                                         │
│  You've been idle for 5 minutes.       │
│  Session will end in 60 seconds.       │
│                                         │
│  [Continue Watching]  [End Session]    │
└─────────────────────────────────────────┘
```

**Why?** To save bandwidth and prevent cameras from running all day if you forget to close the page.

---

## 💻 What Needs to Be Installed (Local Computer)

Phase 1 requires **ONE additional program** running on a computer at the club (can be the same computer running the GFC Web App).

### The "Video Agent" Service

**What is it?**
- A small Windows service (background program)
- Runs 24/7 on a club computer
- Connects to the NVR and converts camera streams to web-friendly format
- Acts as a security bridge (keeps NVR credentials safe)

**Where does it run?**
- On a Windows PC at the club
- Same network as the NVR (192.168.1.x)
- Can be the same PC running the GFC Web App
- Or a dedicated PC if you prefer

**What does it do?**
1. Listens for requests from the GFC Web App
2. Connects to the NVR (192.168.1.64)
3. Grabs the camera stream (RTSP)
4. Converts it to HLS format (web-friendly)
5. Sends it to your browser

**Does it need internet?**
- No! Works entirely on local network
- No cloud services
- No external dependencies

---

## 🛠️ Setup Steps (Simple Version)

### Step 1: Verify Network (5 minutes)
- [ ] Make sure NVR is on and working (192.168.1.64)
- [ ] Make sure you can ping the NVR from the computer
- [ ] Verify no port forwarding is set up on router

### Step 2: Install Video Agent (10 minutes)
- [ ] Download Video Agent installer
- [ ] Run installer on club computer
- [ ] Enter NVR credentials (username/password)
- [ ] Service starts automatically

### Step 3: Update GFC Web App (5 minutes)
- [ ] Deploy updated GFC Web App with camera viewer page
- [ ] Restart web app
- [ ] Done!

### Step 4: Test (2 minutes)
- [ ] Log into GFC Web App
- [ ] Check Dashboard - should see 🟢 green status LED
- [ ] Click "Cameras" button
- [ ] Watch status window connect
- [ ] See live video from Camera 1

**Total Setup Time: ~20-25 minutes**

---

## 🖥️ Computer Requirements

### For the Video Agent PC:

**Minimum:**
- Windows 10 or Windows Server
- 4GB RAM
- 2 CPU cores
- 10GB free disk space
- Wired Ethernet connection (preferred)
- On same network as NVR (192.168.1.x)

**Recommended:**
- Windows 11 or Windows Server 2022
- 8GB RAM
- 4 CPU cores
- 50GB free disk space
- Gigabit Ethernet

**Can it be the same PC as the GFC Web App?**
- ✅ **Yes!** Absolutely fine for Phase 1
- The Video Agent uses minimal resources
- One camera = ~5% CPU, ~150MB RAM

---

## 📡 Network Requirements

**What you need:**
- ✅ NVR on local network (192.168.1.64)
- ✅ Video Agent PC on same network
- ✅ GFC Web App on same network (or accessible via LAN)
- ✅ Stable network connection

**What you DON'T need:**
- ❌ Port forwarding
- ❌ Static public IP
- ❌ Cloud services
- ❌ VPN (not until Phase 2)
- ❌ Special router configuration

**Bandwidth:**
- One camera (sub-stream): ~500 Kbps
- Minimal impact on network

---

## 🔐 Security: What's Protected

**Your NVR credentials are safe because:**
- Stored encrypted on Video Agent PC (Windows DPAPI)
- Never sent to browser
- Never in JavaScript
- Never in web app database

**Your cameras are safe because:**
- No internet exposure
- No port forwarding
- Only accessible on local network
- Only when logged into GFC Web App

**Your network is safe because:**
- No new ports opened
- No cloud services
- No external access
- Everything stays on LAN

---

## 🎬 Day-to-Day Usage (After Setup)

### As a Manager/Owner:

**To view cameras:**
1. Open GFC Web App (like you normally do)
2. Log in (like you normally do)
3. Click "Cameras" button
4. Watch live video

**To stop viewing:**
1. Just close the page or navigate away
2. Video stops automatically
3. No cleanup needed

**If you forget to close it:**
- After 5 minutes idle → popup appears
- After 6 minutes idle → auto-stops
- Saves bandwidth automatically

### As an Admin:

**To check if system is working:**
- Look at Dashboard status LED
- 🟢 Green = all good
- 🟡 Yellow or 🔴 Red = check Video Agent service

**To restart Video Agent:**
- Open Windows Services
- Find "GFC Video Agent"
- Right-click → Restart

---

## 🐛 Troubleshooting (Common Issues)

### Status LED is Red 🔴
**Problem:** Video Agent service is not running  
**Fix:** 
1. Open Windows Services
2. Find "GFC Video Agent"
3. Start the service

### Status LED is Yellow 🟡
**Problem:** Video Agent running, but can't reach NVR  
**Fix:**
1. Check NVR is powered on
2. Ping 192.168.1.64 from Video Agent PC
3. Check network cable

### Video won't load
**Problem:** Stream failed to start  
**Fix:**
1. Check status window for error message
2. Click retry button
3. Check NVR credentials are correct

### Video is choppy or laggy
**Problem:** Network congestion or PC overload  
**Fix:**
1. Check network bandwidth
2. Check Video Agent PC CPU usage
3. Close other programs on Video Agent PC

---

## 📊 What Gets Logged (Audit Trail)

**Every time someone views cameras, we log:**
- Who viewed (username)
- Which camera (Camera 1)
- When started (date/time)
- When stopped (date/time)
- How long (duration)

**Where are logs stored?**
- Video Agent PC: `C:\ProgramData\GFC\VideoAgent\logs\`
- GFC Web App: Existing audit log system

**Who can see logs?**
- Admins only
- Via GFC Web App (Phase 4)

---

## 🚀 What Comes After Phase 1?

### Phase 2: Multi-Camera Grid (Future)
- View multiple cameras at once (2x2, 3x3, 4x4 grid)
- Modern UI with drag-and-drop
- PTZ controls (pan/tilt/zoom)
- Remote access via VPN

### Phase 3: Playback & Downloads (Future)
- Scrub through recorded video
- Download clips
- Export footage

### Phase 4: Management (Future)
- Retention policies
- Storage management
- Advanced audit reports

### Phase 5: Advanced Security (Future)
- Per-user VPN access
- Advanced permissions
- Privacy compliance features

---

## ✅ Phase 1 Success = You Can Answer "Yes" to These:

- [ ] Can I see the "Cameras" button on the Dashboard?
- [ ] Does the status LED show green when Video Agent is running?
- [ ] Can I click "Cameras" and see the status window connect?
- [ ] Does live video from Camera 1 appear within 3 seconds?
- [ ] Is the video smooth (not choppy)?
- [ ] Does the inactivity timeout work (5 min warning)?
- [ ] Can I close the page and video stops automatically?
- [ ] Are all camera views being logged in the audit system?

**If yes to all = Phase 1 is successful! 🎉**

---

## 💡 Key Takeaways

1. **Phase 1 is simple:** One camera, live view, local network only
2. **Setup is quick:** ~20-25 minutes total
3. **Minimal hardware:** Can run on existing GFC Web App PC
4. **Zero internet exposure:** Everything stays on LAN
5. **Automatic cleanup:** Streams stop when you close the page
6. **Full audit trail:** Every view is logged
7. **Safe by default:** No security risks introduced

**Phase 1 proves the concept works before building the full system.**

---

## ❓ Questions?

**"Do I need to buy new hardware?"**
- No, can use existing GFC Web App PC

**"Will this slow down my network?"**
- No, one camera uses minimal bandwidth (~500 Kbps)

**"Can I access cameras from home?"**
- Not in Phase 1 (that's Phase 2 with VPN)

**"What if the Video Agent PC crashes?"**
- Cameras still record to NVR
- Just restart the Video Agent service
- No data loss

**"Can multiple people view at once?"**
- Yes! Each user gets their own stream
- Each stream uses ~500 Kbps

**"What if I want to see all 16 cameras?"**
- That's Phase 2 (multi-camera grid)
- Phase 1 proves one camera works first

---

**Ready to start? Begin with the Pre-Implementation Checklist!**

See: `CAMERA_PHASE_1_PRE_IMPLEMENTATION_CHECKLIST.md`
