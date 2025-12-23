# Plan: Camera System Integration - Phase 4: Audit Archive (Visual Evidence)
**Project:** GFC Camera Viewer  
**Status:** Core Security Feature  

## 🎯 Objective
Automatically create and store permanent video evidence of every door activation. Ensure these clips remain accessible even after the NVR has overwritten its local hard drive.

## 🏗️ Technical Checklist

### 1. Automation Logic (Agent-Side)
- [ ] **Implement Auto-Export Trigger**:
    - The Web App notifies the Agent of a Door Swipe (Member ID, Door ID, Timestamp).
    - The Agent schedules a high-res MP4 download (15s pre-event, 15s post-event).
- [ ] **Batch Processing Service**: Move downloads to "off-peak" hours if network traffic is high.
- [ ] **Storage Management (FIFO)**: 
    - Implement a 500GB (or user-defined) storage cap for the archive folder.
    - Automatically delete the oldest day's clips when the cap is reached.

### 2. UI Integration & Visual Tracking
- [ ] **Modify Audit Logs**: Add a "Play Evidence" icon next to every door access event. [**TAG: MODIFIED**]
- [ ] **Implement Evidence Pop-out**: A high-speed video player that loads clips directly from the local Archive (bypassing the NVR for speed). [**TAG: NEW**]
- [ ] **Visual "Overwritten" Indicator**: Dim the play button if the clip was never archived and the NVR loop has naturally expired.

### 3. Data Integrity
- [ ] **NTP Clock Sync**: Implement a check to ensure NVR and GFC System clocks are synced to the same second.
- [ ] **De-Duplication**: If two swipes occur at the same door within 5-10 seconds, only save one 45-second clip to conserve space.

## 🚀 Success Criteria
- [ ] **Evidence Loop**: Every door swipe results in a saved MP4 in the `C:\GFC_Security_Archive` folder.
- [ ] **Speed**: Playing an archived clip from the hard drive is near-instant compared to live NVR fetching.
- [ ] **Reliability**: Clips are verified as playable after the NVR has looped over the original footage.
- [ ] **Sustainability**: Hard drive usage remains within the specified cap (Set-and-Forget).
