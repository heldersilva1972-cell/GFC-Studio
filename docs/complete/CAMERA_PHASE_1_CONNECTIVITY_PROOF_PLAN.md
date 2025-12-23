# Plan: Camera System Integration - Phase 1: Connectivity Proof
**Project:** GFC Camera Viewer  
**Status:** Planning / In-Process  

## 🎯 Objective
Prove reliable connectivity between the GFC Web App and the NVR (192.168.1.64) via the Local Video Agent. Establish a real-time status bridge with plain-English feedback for the user.

## 🏗️ Technical Checklist

### 1. UI Integration & Visual Tracking
- [ ] **Implement Change Tracking UI**: Ensure `.gfc-modified-tag` and `.gfc-new-tag` are available in `app.css`.
- [ ] **Modify Dashboard.razor**: Add a "Cameras" button to the top action bar. [**TAG: NEW**]
    - Include a status LED component (🟢/🟡/🔴) indicating the heartbeat status of the Video Agent.
- [ ] **Create CameraViewer.razor**: A new page for testing live connectivity. [**TAG: NEW**]
    - Add a "Plain-English Status Window" for connection logs.
    - Implement a single video player (HLS.js) for Camera 1.
- [ ] **Modify NavMenu.razor**: Add a temporary test link to the "Cameras" page under the Controllers section. [**TAG: MODIFIED**]

### 2. Video Agent (Phase 1 MVP)
- [ ] **Create Video Agent Service (C#)**:
    - Implement an HTTP endpoint for the "Heartbeat".
    - Store NVR credentials securely (Windows DPAPI).
    - Implement the "Automated NVR Probe" (Login -> Discovery -> RTSP Check).
    - Use FFmpeg to transcode RTSP Stream 1 (Sub-stream) to HLS.

### 3. Logic & Security
- [ ] **Implement "Still Working?" Popup**: A Blazor modal that triggers after 5 minutes of inactivity on the viewer page.
- [ ] **Implement Auto-Kill Logic**: Ensure the web app calls the Agent to terminate FFmpeg processes when the user leaves the page or the timeout occurs.
- [ ] **Handshake Security**: Implement basic API Key validation from Web App to Local Agent.

## 🚀 Success Criteria
- [ ] **Visual Tracking**: All new/modified elements on the Dashboard or NavMenu are clearly tagged.
- [ ] **Connection**: The Dashboard LED glows Green when the Agent is running.
- [ ] **Automation**: Clicking "Cameras" automatically logs into the NVR and shows live video.
- [ ] **Human-Readable**: The status window displays "NVR Found," "Login Valid," and "Video Active" during the handshake.
- [ ] **Cleanup**: FFmpeg processes are confirmed dead after the page is closed.
