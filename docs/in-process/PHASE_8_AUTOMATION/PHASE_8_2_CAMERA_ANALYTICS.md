# PHASE 8-2: CAMERA ANALYTICS INTEGRATION

**Objective:** Connect the NVR visual data with the operational dashboard.

## 1. Motion Alerts
- [ ] **Event Listener**: Subscribe to ONVIF "MotionDetected" events from the NVR.
- [ ] **Dashboard Widget**: Visual indicator in the "Mission Control" when motion is detected in sensitive areas (e.g., Office, Stock Room) after hours.
- [ ] **Snapshot Capture**: Automatically save a still image to the `CameraAuditLogs` on motion triggers.

## 2. Smart Linking
- [ ] **Access + Video**: Auto-play a 10-second clip when a "Door Forced Open" alarm is triggered.
