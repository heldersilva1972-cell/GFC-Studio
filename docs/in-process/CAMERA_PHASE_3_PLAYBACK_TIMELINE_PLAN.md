# Plan: Camera System Integration - Phase 3: Playback & Timeline
**Project:** GFC Camera Viewer  
**Status:** Future Phase  

## 🎯 Objective
Enable users to search, rewind, and analyze recorded footage directly from the GFC Web App using a professional, high-precision timeline.

## 🏗️ Technical Checklist

### 1. Interactive Timeline UI
- [ ] **Create Visual Timeline Component**: A scrollable browser component at the bottom of the viewer. [**TAG: NEW**]
- [ ] **Data Density Bar**: Show colored indicators (Green = Video, Orange = Motion) fetched from the NVR.
- [ ] **Event Markers**: Small "Key" icons on the timeline representing door swipes or system alerts. [**TAG: NEW**]

### 2. Playback Controls & Logic
- [ ] **Implement Variable Speed**:
    - **Fast Forward**: 2x, 4x, 8x, 16x.
    - **Slow Motion**: 1/2x, 1/4x.
- [ ] **Precision Jump Buttons**: Dedicated buttons for "-10s", "-5s", "+5s", and "+10s".
- [ ] **Frame-by-Frame Seek**: Use NVR API to fetch specific frame sequences for detailed analysis.
- [ ] **Sync Playback**: Allow "Grid Mode" playback where all visible cameras move to the same timestamp.

### 3. Backend (Agent-Side) Enhancement
- [ ] **NVR Recording Probe**: Implement API calls to query NVR for available files by date/time/camera.
- [ ] **HLS Seek Logic**: Update Agent to start transcoding from any arbitrary timestamp requested by the user.

## 🚀 Success Criteria
- [ ] **Precision**: Timeline "Scrubbing" follows the mouse/drag smoothly.
- [ ] **Context**: Hovering over an event marker on the timeline displays the name of the person who swiped the door.
- [ ] **Control**: Variable speeds are responsive (NVR speed commands are executed correctly).
- [ ] **Sync**: Replayed video remains synchronized across multiple camera windows.
