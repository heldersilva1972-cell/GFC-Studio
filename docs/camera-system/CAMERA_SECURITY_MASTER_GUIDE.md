# GFC Camera System – Secure Remote Access & Anti-Hacking Master Guide

**Scope:** Safest possible method to access cameras locally and remotely  
**Applies to:** Club Wi-Fi, NVR, Video Agent, Stand-alone Camera Viewer, GFC Web App  
**Security Priority:** MAXIMUM (prevent hacking, spying, lateral network access)  
**Status:** Master Reference Document

---

## 1) 🎯 CORE SECURITY GOAL

Provide reliable camera viewing (local + future remote) **WITHOUT:**
- Exposing the NVR to the internet
- Weakening club Wi-Fi security
- Creating new attack paths into the GFC Web App
- Allowing unauthorized viewing, spying, or data exfiltration

### Security must protect against:
- External hackers
- Insider misuse
- Credential leaks
- Firmware exploits
- Cloud/vendor compromise

---

## 2) 🏗️ THE SINGLE SAFEST ARCHITECTURE (NON-NEGOTIABLE)

**THIS IS THE ONLY ARCHITECTURE RECOMMENDED:**

```
Remote User
   ↓ (Encrypted VPN Tunnel – WireGuard)
GFC Network (LAN)
   ↓
GFC Web App
   ↓
Video Agent Service
   ↓
NVR (LAN-only, private IP, never exposed)
```

### Key Principle:
> **The NVR MUST NEVER be reachable from the public internet.**  
> **Not now. Not later. Ever.**

---

## 3) ✅ WHAT MUST ALWAYS HAPPEN (REQUIRED RULES)

### A) NETWORK-LEVEL RULES
- ✅ NVR uses a private IP only (192.168.x.x)
- ✅ **NO** router port forwarding to:
  - RTSP (554)
  - HTTP/API (8000)
  - Any other NVR port
- ✅ Disable UPnP on the router
- ✅ Cloud/DDNS features disabled once VPN is active
- ✅ NVR preferably on wired Ethernet
- ✅ Optional (future hardening): isolate NVR on its own VLAN

### B) ACCESS CONTROL RULES
- ✅ All camera access goes through: **Video Agent → Web App**
- ✅ Browsers **NEVER** connect directly to the NVR
- ✅ VPN required for **ALL** remote camera access
- ✅ No VPN = no camera access (outside the LAN)
- ✅ One VPN key per user/device
- ✅ VPN access revoked immediately when staff changes

### C) CREDENTIAL HANDLING RULES
- ✅ NVR credentials stored **ONLY** in the Video Agent
- ✅ Credentials encrypted at rest (Windows DPAPI)
- ✅ Credentials **NEVER:**
  - Stored in browser
  - Embedded in JavaScript
  - Sent to clients
- ✅ NVR admin password changed from installer default
- ✅ Password rotation policy documented

### D) APPLICATION SECURITY RULES
- ✅ HTTPS only for web app
- ✅ Login required before camera viewer loads
- ✅ Session-based authorization
- ✅ Stream URLs are short-lived and user-bound
- ✅ Streams cannot be copied/shared externally
- ✅ Rate limiting on video endpoints
- ✅ Security headers enabled (CSP, X-Frame-Options, etc.)

### E) AUDIT & ACCOUNTABILITY RULES

**Log ALL of the following:**
- Who viewed which camera
- Viewing start and stop times
- Remote vs local access
- Who downloaded clips
- Time ranges downloaded

> **Audit logs must be immutable and admin-visible.**

---

## 4) ❌ WHAT MUST NEVER HAPPEN (ABSOLUTE PROHIBITIONS)

These actions create serious security risk and **MUST NOT** be allowed:

- ❌ Port forwarding ANY NVR port to the internet
- ❌ Exposing RTSP streams publicly
- ❌ Browser → NVR direct connections
- ❌ Embedding camera credentials in web pages
- ❌ Allowing anonymous or guest camera viewing
- ❌ Relying on vendor cloud/DDNS long-term
- ❌ Allowing full-day unrestricted downloads
- ❌ Sharing VPN credentials between users
- ❌ Allowing camera access without audit logging

> **Any of the above materially increases hacking risk.**

---

## 5) 🔐 WHY VPN (WIREGUARD) IS THE SAFEST OPTION

### WireGuard VPN advantages:
- ✅ Strong modern encryption
- ✅ Very small attack surface
- ✅ No exposed services to the public internet
- ✅ Makes remote devices behave as if they are onsite
- ✅ No changes needed to camera or streaming logic
- ✅ Industry-standard for high-security environments

### VPN model:
1. Remote user authenticates FIRST
2. Then accesses the web app normally
3. Cameras never know the internet exists

---

## 6) 📊 COMPARISON OF REMOTE ACCESS OPTIONS

### OPTION 1: VPN + Web App (✅ RECOMMENDED)
- **Security:** EXCELLENT
- **NVR exposed to internet:** NO
- **Control & auditing:** FULL
- **Risk level:** LOWEST POSSIBLE

### OPTION 2: Public Web App Camera Access
- **Security:** GOOD if hardened
- **NVR exposed:** NO
- **Attack surface:** MEDIUM
- **Requires:** Heavy ongoing security work

### OPTION 3: Vendor Cloud / DDNS
- **Security:** UNKNOWN / vendor-dependent
- **NVR exposed:** INDIRECTLY
- **Control:** LIMITED
- **Risk level:** MODERATE

### OPTION 4: Port Forwarding (❌ NEVER)
- **Security:** POOR
- **NVR exposed:** YES
- **Risk level:** EXTREME

---

## 7) 🚀 PHASED EXECUTION PLAN (SECURITY-FIRST)

### PHASE 1 – CURRENT (SAFE BY DEFAULT)
- LAN-only stand-alone viewer
- No remote access
- Prove video works
- No cloud reliance

### PHASE 2 – SECURE REMOTE ACCESS
- Deploy WireGuard VPN
- Issue per-user keys
- Restrict camera access to VPN users only
- Disable vendor cloud/DDNS

### PHASE 3 – WEB APP INTEGRATION
- Merge camera viewer into GFC Web App
- Keep Video Agent unchanged
- Maintain VPN requirement for cameras
- Enforce roles and permissions

---

## 8) 🛡️ CLUB WI-FI PROTECTION CONSIDERATIONS

### This approach PROTECTS club Wi-Fi because:
- ✅ Cameras never open inbound ports
- ✅ No guest Wi-Fi access to cameras
- ✅ VPN users authenticate separately
- ✅ Video Agent isolates camera credentials
- ✅ Web app access does not expose LAN internals

### Optional enhancements:
- Separate VLAN for cameras
- Firewall rules allowing ONLY Video Agent → NVR
- Guest Wi-Fi blocked from camera VLAN

---

## 9) ⚖️ PRIVACY & LEGAL SAFETY ADD-ONS (RECOMMENDED)

- ✅ Clear retention policy
- ✅ Download permissions restricted
- ✅ Watermark overlays on video/downloads
- ✅ Signage: "Video Recording in Use"
- ✅ Audio recording disabled unless explicitly required

---

## 10) 🏆 FINAL SECURITY VERDICT

### THIS IS THE SAFEST POSSIBLE APPROACH:

- ✅ NVR remains LAN-only forever
- ✅ No public exposure of camera services
- ✅ All remote access via VPN
- ✅ Video Agent acts as security boundary
- ✅ Web app never handles raw camera access
- ✅ Full auditability and control

### This approach minimizes:
- Hacking risk
- Insider misuse
- Credential leaks
- Liability exposure
- Long-term maintenance burden

---

## 📋 COMPLIANCE CHECKLIST

Use this checklist to verify security compliance at each phase:

- [ ] NVR has private IP only (no public exposure)
- [ ] Router port forwarding disabled for all NVR ports
- [ ] UPnP disabled on router
- [ ] NVR cloud/DDNS features disabled
- [ ] Video Agent uses Windows DPAPI for credential storage
- [ ] NVR admin password changed from default
- [ ] Web app requires authentication before camera access
- [ ] HTTPS enabled on web app
- [ ] Audit logging implemented for all camera access
- [ ] VPN configured for remote access (Phase 2+)
- [ ] Per-user VPN keys issued
- [ ] Security headers configured (CSP, X-Frame-Options)
- [ ] Rate limiting enabled on video endpoints
- [ ] Privacy signage posted
- [ ] Retention policy documented

---

**Document Version:** 1.0  
**Last Updated:** 2025-12-21  
**Review Frequency:** Quarterly or after any security incident
