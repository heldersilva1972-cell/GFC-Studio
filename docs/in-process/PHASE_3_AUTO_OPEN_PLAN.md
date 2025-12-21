# Phase 3: Auto-Open Schedules (Hardware Automation)

## 🎯 Objective
Implement the logic required to automatically unlock and lock doors based on a predefined Time Profile. This brings "Smart Building" capabilities to the GFC Studio real-mode environment.

### 🏷️ Visual Tracking Requirement
*   Any page modified by this phase must include a visible **[MODIFIED]** tag with a description.
*   New UI tabs (`AutoOpenTab`) or settings must be marked with a visible **[NEW]** tag.

## 🏗️ Technical Requirements

### 1. UI Extension: `AutoOpenTab.razor` [**TAG: NEW**]
- [ ] **Door Mapping**: A grid showing all doors on the selected controller.
- [ ] **Profile Linkage**: A dropdown for each door to select an existing Time Profile.
- [ ] **Manual Override**: A toggle to temporarily suspend the schedule without deleting it.
- [ ] **Day-of-Week Selector**: Visual chips for Mon-Sun recurring schedules.

### 2. REAL-MODE Automation Engine (`HardwareAutomationService.cs`) [**TAG: NEW**]
- [ ] **Background Monitor**: A low-frequency background service (every 60 seconds).
- [ ] **State Transition**: Check current real-world time against Door Schedules. If a door should be UNLOCKED, send the command to the `RealControllerClient`.
- [ ] **Safety Check**: Ensure the service only fires a command if the current door state (fetched from hardware) does not match the desired schedule state.
- [ ] **Log Integration**: All auto-open events must be logged in the system audit trail.

### 3. Hardware Synchronization
- [ ] **Verification**: Implement a "Sync Status" badge that confirms the real controller is responding to the automation commands.
- [ ] **Retry Logic**: Implement basic exponential backoff for failed commands sent to the Agent PC.

## 🚀 UX Goals
*   **Safety First**: Clearly indicate on the Dashboard when a door is held open by a schedule vs. a manual command.
*   **Visual Debugging**: Show the "Next Event" (e.g., "Auto-unlocking at 08:00 AM") in the status panel.
*   **Status Feedback**: UI shows real-time auto-open status per door.
