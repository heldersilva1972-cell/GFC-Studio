# Plan: Camera System Integration - Phase 2: Modern UI & Premium Grid
**Project:** GFC Camera Viewer  
**Status:** Multi-Phase Design (Deferred until Phase 1 Success)  

## 🎯 Objective
Transform the proof-of-concept viewer into a world-class, 16-channel "Control Room" interface with glassmorphism design and high-performance video scaling.

## 🏗️ Technical Checklist

### 1. UI/UX Design & Visual Tracking
- [ ] **Design Camera Grid UI**: Implement a flexible CSS Grid for 1, 4, 9, or 16 cameras. [**TAG: NEW**]
- [ ] **Apply Glassmorphism**: Use frosted-glass panels and blur-backdrops for the camera selector and layout controls.
- [ ] **Smooth Transitions**: Use C# / CSS transitions for resizing camera windows.
- [ ] **Snapshots**: Add a "Snapshot" icon to each video frame. [**TAG: NEW**]
    - Method A: Client-side canvas capture (Instant).
    - Method B: Agent-side raw frame capture (High Detail).

### 2. Video Agent Scaling
- [ ] **Implement Multi-Channel Logic**: Update Agent to handle up to 16 concurrent RTSP-to-HLS streams.
- [ ] **Dual-Stream Optimization**: 
    - Always use **Sub-streams** for the Grid (low resolution/bandwidth).
    - Switch to **Main-stream** (HD) only when a camera is clicked for "Focus View."
- [ ] **Off-screen Auto-Pause**: Automatically tell the Agent to stop transcoding cameras that are not currently visible on the user's monitor.

### 3. Management Logic
- [ ] **Camera Settings Panel**: A simple sidebar to name cameras (e.g., "Main Gate," "Back Alley"). [**TAG: NEW**]
- [ ] **Hardware Status Overlays**: Show "No Signal" or "Connecting..." on specific grid squares if an individual camera fails.

## 🚀 Success Criteria
- [ ] **Visual Hierarchy**: Modern aesthetics (vibrant dark mode, premium typography).
- [ ] **Performance**: 16 substreams running concurrently without crashing the browser or PC.
- [ ] **Responsiveness**: UI layout adapts smoothly to Desktop, Tablet, and Full-Screen modes.
- [ ] **Functional Snapshots**: User can save a JPEG of any camera feed with one click.
