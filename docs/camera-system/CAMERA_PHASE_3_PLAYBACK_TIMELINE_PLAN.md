# Plan: Camera System Integration - Phase 3: Playback & Timeline
**Project:** GFC Camera Viewer  
**Status:** Planning / In-Process  

## 🎯 Objective
Implement video playback controls with timeline scrubbing for reviewing recorded footage.

## 🏗️ Technical Checklist

### 1. Playback Controls
- [ ] **Create PlaybackController.razor**: [**TAG: NEW**]
    - Play/Pause/Stop buttons
    - Speed controls (0.5x, 1x, 2x, 4x)
    - Frame-by-frame stepping (forward/backward)
    - Jump to specific timestamp
- [ ] **Timeline Scrubber**: [**TAG: NEW**]
    - Visual timeline bar showing recording duration
    - Drag to scrub through footage
    - Thumbnail preview on hover
    - Event markers (motion detection, alerts)

### 2. Recording Retrieval
- [ ] **NVR Recording API Integration**:
    - Query available recordings by camera and date range
    - Retrieve recording metadata (start time, duration, file size)
    - Stream recorded video via HLS
- [ ] **Date/Time Picker**: [**TAG: NEW**]
    - Calendar view for selecting date
    - Time range selector
    - Quick presets (Last Hour, Last 24h, Last Week)

### 3. Advanced Features
- [ ] **Multi-Camera Sync Playback**:
    - Synchronize playback across multiple cameras
    - Show all cameras at same timestamp
- [ ] **Export Functionality**:
    - Clip selection (start/end time)
    - Download video segment
    - Export as MP4 with metadata

## 🚀 Success Criteria
- [ ] **Visual Tracking**: All new components tagged
- [ ] **Smooth Playback**: No stuttering or buffering
- [ ] **Accurate Scrubbing**: Timeline matches video position
- [ ] **Event Markers**: Motion events clearly visible on timeline
- [ ] **Export Works**: Downloaded clips play in standard media players
