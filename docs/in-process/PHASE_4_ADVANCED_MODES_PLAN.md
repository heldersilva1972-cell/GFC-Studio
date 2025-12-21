# Phase 4: Advanced Door Modes & Security (Real Hardware)

## 🎯 Objective
Unlock the full potential of the access control hardware by supporting professional security modes (Lockdown, Visitor, Interlock) and real-mode guarding logic.

### 🏷️ Visual Tracking Requirement
*   Any page modified by this phase must include a visible **[MODIFIED]** tag with a description (e.g., in `DoorModal.razor`).
*   Any new UI indicators (e.g., `AIRLOCK` or `LOCKDOWN` badges) must be marked with a visible **[NEW]** tag.

## 🏗️ Technical Requirements

### 1. Extended Door Configuration (`DoorModal.razor`) [**TAG: MODIFIED**]
- [ ] **Normally Open (Hold Open)**: The door remains unlocked by default. A card swipe relocks it (toggle logic).
- [ ] **Double Swipe/Two-Person Rule**: Requires two valid card reads within 5 seconds for access.
- [ ] **Toggle Mode**: Card swipe switches the door state from Locked to Unlocked (and vice versa) until the next swipe.
- [ ] **Visitor Mode**: Generate time-limited access codes and assign them to specific doors.

### 2. Lockdown & Emergency Systems
- [ ] **Emergency Lockdown**: A high-priority global command to lock ALL doors immediately, overriding all schedules.
- [ ] **Admin Override**: Require high-level admin authentication to disable a lockdown state.
- [ ] **Audit Integration**: Log lockdown triggers with the user ID and timestamp.

### 3. Interlocking Logic (Real-Mode Guards)
- [ ] **Door Interlock Groups**: Configure pairs of doors that cannot be open simultaneously (Airlock/Security).
- [ ] **Software Guards**: Implement logic in `RealControllerClient` that blocks unlock commands if interlock conditions are not met.
- [ ] **State Awareness**: Poll hardware states to ensure interlock rules are respected globally.

### 4. Advanced Scheduling
- [ ] **Holiday Schedules**: Define a calendar of holidays that override normal recurring schedules.

## 🚀 UX Goals
- [ ] **Mode Badges**: Grid shows clear badges like `LOCKDOWN`, `AIRLOCK`, or `VISITOR`.
- [ ] **Conflict Feedback**: If a door is blocked by an interlock, show a "Blocked" animation.
- [ ] **Performance**: Lockdown activates all doors within 2 seconds.
