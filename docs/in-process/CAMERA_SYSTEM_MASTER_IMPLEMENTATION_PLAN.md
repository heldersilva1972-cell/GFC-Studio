# GFC Camera Viewer - Master Implementation Plan

**Project:** GFC Camera System Integration  
**Status:** Approved / Finalized  
**Architecture:** Agent-PC Pattern (Distributed)  
**Target:** 16-Camera High-Performance Security Suite  

## 🎯 Strategic Objective
Establish a world-class, ultra-modern camera surveillance system integrated directly into the GFC Web App. The system must prove reliability through a phased rollout, starting with basic connectivity and culminating in a full security management suite with localized video transport.

---

## 🏗️ Core Architecture: The "Agent-PC" Pattern
To ensure performance and security, the system is split into two distinct layers:
1. **GFC Web App (Cloud/Server)**: Coordinates identity, permissions, audit logging, and UI state.
2. **Local Video Agent (Agent PC)**: Sits behind the local firewall, communicates with the NVR (192.168.1.64), and handles heavy video transcoding (FFmpeg) to HLS for the Web App.

**Key Principle:** The Web App NEVER stores NVR credentials or contacts cameras directly. It sends heartbeats and requests to the Local Agent.

---

## 📅 Phased Rollout Schedule

### **Phase 1: Connectivity Proof (MVP)**
*   **Goal:** Establish the handshake between Web App and Local Agent.
*   **Key Features:** Dashboard status LED, single-camera probe, "Still Working?" safety timeout.
*   **Document:** `CAMERA_PHASE_1_CONNECTIVITY_PROOF_PLAN.md`

### **Phase 2: Ultra-Modern UI & Grid**
*   **Goal:** Create a "WOW" factor interface for daily monitoring.
*   **Key Features:** 4x4 Responsive Grid, Glassmorphism aesthetics, Micro-animations, Full-screen transitions.
*   **Document:** `CAMERA_PHASE_2_MODERN_UI_PLAN.md`

### **Phase 3: Playback & Timeline Integration**
*   **Goal:** Allow users to travel back in time through NVR records.
*   **Key Features:** Interactive scrubbing timeline, multi-camera synchronized playback, "Speed UI" (1x, 2x, 4x, 16x).
*   **Document:** `CAMERA_PHASE_3_PLAYBACK_TIMELINE_PLAN.md`

### **Phase 4: Audit & Archive Engine**
*   **Goal:** Formalize video evidence and audit trails.
*   **Key Features:** "Save to Archive" workflow, watermarked downloads, automated cleanup policy, access audit logs.
*   **Document:** `CAMERA_PHASE_4_AUDIT_ARCHIVE_PLAN.md`

### **Phase 5: Management & Security Suite**
*   **Goal:** A centralized command center for system vitals.
*   **Key Features:** Door/Camera linking, HDD storage monitors, One-Time Tokens (OTT) for video URLs, automated health alerts.
*   **Document:** `CAMERA_PHASE_5_MANAGEMENT_SECURITY_PLAN.md`

---

## 🛡️ Security & Performance Standards
- **NVR Isolation:** NVR remains on the isolated local subnet (192.168.1.x).
- **Auto-Kill FFmpeg:** The Local Agent must terminate all FFmpeg streams if the Web App session expires or the user leaves the page.
- **DPAPI Encryption:** Agent-side credentials must be encrypted using Windows Data Protection API.
- **Visual Tracking:** Every modified page must include the `[MODIFIED]` tag, and new components must include the `[NEW]` tag as per documentation standards.

---

## 📊 Success Metrics
- **Zero Configuration:** Plugging in the Agent PC should auto-discovery by the Web App.
- **Sub-Second Latency:** Live sub-streams must load in under 500ms.
- **Total Visibility:** Dashboard LED must accurately reflect NVR and Agent health 24/7.

---
**Status:** **READY FOR EXECUTION**  
**Lead:** Antigravity AI  
**Revision:** R1.0 (Post-Finalization)
