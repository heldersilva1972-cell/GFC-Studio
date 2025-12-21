# Camera Phase 1 - Pre-Implementation Security Checklist

**Purpose:** Verify security posture BEFORE starting Phase 1 implementation  
**Status:** Required - Must complete before writing any code  
**Time Required:** ~30 minutes

---

## ✅ STEP 1: Network Security Verification

### Router Configuration
- [ ] Log into router admin panel
- [ ] Navigate to Port Forwarding section
- [ ] **VERIFY**: No port forwarding rules exist for:
  - [ ] Port 554 (RTSP)
  - [ ] Port 8000 (NVR HTTP API)
  - [ ] Port 80/443 (NVR Web Interface)
  - [ ] Any other ports pointing to 192.168.1.64
- [ ] Navigate to UPnP settings
- [ ] **VERIFY**: UPnP is DISABLED
- [ ] Take screenshot of port forwarding page (for documentation)
- [ ] Take screenshot of UPnP settings (for documentation)

**Result:** ✅ PASS = No port forwarding, UPnP disabled  
**Action if FAIL:** Remove all port forwarding rules, disable UPnP

---

## ✅ STEP 2: NVR Security Audit

### NVR Network Configuration
- [ ] Log into NVR web interface (http://192.168.1.64:8000)
- [ ] Navigate to Network Settings
- [ ] **VERIFY**: IP Address is 192.168.1.64 (static, private)
- [ ] **VERIFY**: Gateway is your router's LAN IP (e.g., 192.168.1.1)
- [ ] **VERIFY**: DNS servers are local or trusted (not public if avoidable)
- [ ] Take screenshot of network settings

### NVR Cloud/DDNS Settings
- [ ] Navigate to Cloud/DDNS settings
- [ ] **DOCUMENT**: Current cloud service status (enabled/disabled)
- [ ] **DOCUMENT**: Current DDNS status (enabled/disabled)
- [ ] **NOTE**: Will disable in Phase 2 after VPN is set up
- [ ] Take screenshot of cloud/DDNS settings

### NVR User Accounts
- [ ] Navigate to User Management
- [ ] **VERIFY**: Admin password is NOT the installer default
- [ ] If still default password:
  - [ ] Change admin password to strong password (16+ chars)
  - [ ] Store new password in password manager
  - [ ] Document password location
- [ ] **VERIFY**: No unnecessary user accounts exist
- [ ] **VERIFY**: Guest account is disabled
- [ ] Take screenshot of user list (hide passwords)

### NVR Firmware
- [ ] Navigate to System Info
- [ ] **DOCUMENT**: Current firmware version
- [ ] **DOCUMENT**: Last firmware update date
- [ ] Check manufacturer website for latest firmware
- [ ] **PLAN**: Schedule firmware update if outdated (after Phase 1 testing)

---

## ✅ STEP 3: Camera Discovery & Documentation

### Camera Inventory
- [ ] Navigate to Camera Management in NVR
- [ ] **DOCUMENT** for each camera:
  - [ ] Camera ID/Number
  - [ ] Camera Name/Location
  - [ ] IP Address (if applicable)
  - [ ] Main stream RTSP URL
  - [ ] Sub-stream RTSP URL
  - [ ] Resolution (main/sub)
  - [ ] Status (online/offline)

**Create spreadsheet or document with:**
```
Camera 1: Front Entrance
- ID: 1
- IP: 192.168.1.101
- Main: rtsp://192.168.1.64:554/cam1/main
- Sub: rtsp://192.168.1.64:554/cam1/sub
- Resolution: 1920x1080 (main), 640x480 (sub)
- Status: Online
```

---

## ✅ STEP 4: Test Connectivity (Manual)

### NVR Reachability
- [ ] Open Command Prompt
- [ ] Run: `ping 192.168.1.64`
- [ ] **VERIFY**: Successful ping response
- [ ] **DOCUMENT**: Average ping time (should be < 5ms on LAN)

### RTSP Stream Test (Optional - requires VLC)
- [ ] Open VLC Media Player
- [ ] Media → Open Network Stream
- [ ] Enter RTSP URL for Camera 1 sub-stream
- [ ] **VERIFY**: Video plays successfully
- [ ] **VERIFY**: Latency is acceptable (< 3 seconds)
- [ ] Close VLC

---

## ✅ STEP 5: Development Environment Preparation

### Video Agent Development PC
- [ ] **VERIFY**: PC is on same LAN as NVR (192.168.1.x)
- [ ] **VERIFY**: .NET 8 SDK installed
- [ ] **VERIFY**: Visual Studio 2022 installed
- [ ] **VERIFY**: FFmpeg installed and in PATH
  - [ ] Run: `ffmpeg -version`
  - [ ] Should show version info
- [ ] **VERIFY**: Git installed and configured

### Web App Development Environment
- [ ] **VERIFY**: GFC Web App builds successfully
- [ ] **VERIFY**: Can run locally (dotnet run)
- [ ] **VERIFY**: Can log in with test account
- [ ] **VERIFY**: Dashboard loads correctly

---

## ✅ STEP 6: Security Documentation

### Create Security Baseline Document
- [ ] Create file: `CAMERA_SECURITY_BASELINE.md`
- [ ] Document current state:
  - [ ] Router port forwarding status
  - [ ] UPnP status
  - [ ] NVR IP configuration
  - [ ] NVR cloud/DDNS status
  - [ ] NVR firmware version
  - [ ] Admin password changed (yes/no)
  - [ ] Number of cameras
  - [ ] Network topology diagram
- [ ] Store screenshots in: `docs/security/baseline/`

### Create Password Documentation
- [ ] Store NVR admin credentials in password manager
- [ ] **NEVER** commit passwords to Git
- [ ] **NEVER** store passwords in plain text
- [ ] Document password manager location

---

## ✅ STEP 7: Compliance Verification

### Review Security Master Guide
- [ ] Read `CAMERA_SECURITY_MASTER_GUIDE.md` completely
- [ ] Understand all "MUST NEVER HAPPEN" prohibitions
- [ ] Understand all "MUST ALWAYS HAPPEN" requirements
- [ ] Confirm Phase 1 is LAN-only (no remote access)

### Sign-Off
- [ ] All checklist items completed
- [ ] All screenshots taken and stored
- [ ] All documentation created
- [ ] Security baseline documented
- [ ] Ready to begin Phase 1 implementation

---

## 📋 Checklist Summary

**Total Items:** 50+  
**Estimated Time:** 30-45 minutes  
**Required for:** Phase 1 implementation start

### Quick Status Check:
- [ ] **Network Security:** No port forwarding, UPnP disabled
- [ ] **NVR Security:** Strong password, documented configuration
- [ ] **Camera Inventory:** All cameras documented with RTSP URLs
- [ ] **Connectivity:** NVR pingable, RTSP streams accessible
- [ ] **Dev Environment:** All tools installed and working
- [ ] **Documentation:** Security baseline created
- [ ] **Compliance:** Security guide reviewed and understood

---

## 🚨 STOP CONDITIONS

**DO NOT proceed with Phase 1 implementation if:**
- ❌ Port forwarding exists to NVR
- ❌ NVR admin password is still default
- ❌ Cannot ping NVR from development PC
- ❌ FFmpeg not installed or not working
- ❌ Security master guide not reviewed

**Fix these issues FIRST before writing any code.**

---

## ✅ Ready to Proceed?

Once all items are checked:
1. Save this checklist with completion date
2. Store all screenshots in `docs/security/baseline/`
3. Commit security baseline document to Git
4. Begin Phase 1 implementation following `CAMERA_PHASE_1_CONNECTIVITY_PROOF_PLAN.md`

---

**Checklist Completed By:** ___________________  
**Date:** ___________________  
**Verified By:** ___________________  
**Date:** ___________________
