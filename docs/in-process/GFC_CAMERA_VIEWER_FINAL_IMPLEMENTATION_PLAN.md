# GFC Camera Viewer - Final Implementation Plan

## ✅ APPROVED DESIGN

### **Key Decisions:**
1. **Agent-PC Pattern** - Distributed architecture with Local Video Agent handling heavy transport (RTSP → HLS) behind the firewall.
2. **NVR Subnet (192.168.1.64)** - Direct communication only between Agent PC and NVR; credentials NEVER leave the Local Agent.
3. **16-Camera Support** - Dual-stream optimization (Sub-streams for grid, Main-stream for focus).
4. **Auto-Kill Safety** - FFmpeg processes on Agent PC must be force-killed if session idle > 5 mins or page exited.
5. **Distinct Identity** - "GFC Camera Viewer" is treated as a premium, distinct but integrated suite.
6. **Visual Tracking** - Every modified page must include a visible **[MODIFIED]** tag. New components (like the camera grid) must be tagged/wrapped with a visible **[NEW]** tag.

---

## 📋 IMPLEMENTATION PHASES

### **Phase 1: Connectivity & Heartbeat (MVP)**
*   **Infrastructure:** Establish WebApp-to-Agent handshake (API Key auth).
*   **Heartbeat UI:** Add Status LED to Dashboard top bar. [**TAG: NEW**]
*   **Automation:** Clicking "Cameras" on Dashboard triggers automated NVR probe.
*   **Safety:** Implement "Still Working?" popup and server-side connection timeout.

### **Phase 2: Ultra-Modern UI & Grid**
*   **Design:** Premium 16-channel "Control Center" using Glassmorphism (frosted glass) and micro-animations.
*   **Tech:** Flex-Grid with 1, 4, 9, 16 camera layouts. [**TAG: NEW**]
*   **Optimization:** Off-screen auto-pause for HLS fragments to save bandwidth.
*   **Snapshots:** One-click frame capture saved to local downloads.

### **Phase 3: Playback & Timeline Integration**
*   **UI:** Interactive scrubbing timeline with event markers for door swipes. [**TAG: NEW**]
*   **Speed:** Variable playback speeds (1/4x to 16x).
*   **Logic:** Synchronized playback across multiple grid windows.
*   **Backend:** Agent-side HLS seek logic for arbitrary timestamps.

### **Phase 4: Audit Archive (Visual Evidence)**
*   **Automation:** Auto-export 30s clips (15s pre/post) for every valid door swipe.
*   **Storage:** 500GB FIFO storage cap managed on Agent PC disc.
*   **Log Integration:** "Play Evidence" link added directly to Member Activity logs. [**TAG: MODIFIED**]
*   **Precision:** Mandatory NTP sync between NVR and GFC Server.

### **Phase 5: Management & Security Suite**
*   **Admin UI:** Card-based camera configuration page (link Doors to Cameras). [**TAG: NEW**]
*   **Security:** Expiring One-Time Tokens (OTT) for video URLs (60s lifetime).
*   **RBAC:** Strict "Security Admin" role enforcement for Archive and Settings.
*   **Vitals:** HDD usage Gauges and high-priority offline alerts on Dashboard.

---

## 📊 TESTING CHECKLIST
- [ ] LED Status: Green = Agent Online, Red = Agent/NVR Missing.
- [ ] Auto-Kill Test: Monitor `ffmpeg.exe` instances after tab close.
- [ ] Sync Test: Door swipe timestamp matches video event marker within 1s.
- [ ] Privacy Test: Inspect network traffic to verify NVR credentials are NOT sent to browser.
- [ ] Grid Test: 16 concurrent low-res streams do not exceed 30% CPU on Agent PC.

---

## 🚀 DEPLOYMENT ORDER
1. **Agent PC Setup** (Windows Service + FFmpeg + API).
2. **Phase 1 Integration** (Dashboard heartbeats + single stream).
3. **Phase 2 UI Upgrade** (Grid + Aesthetics).
4. **Phase 3/4 Security Features** (Timeline + Evidence Archive).
5. **Phase 5 Hardening** (RBAC + OTT).

**Status:** **FINALIZED / APPROVED**  
**Estimated Timeline:** 8-10 weeks (Cumulative)  
**Lead AI:** Antigravity  
