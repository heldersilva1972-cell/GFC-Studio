# Phase 3: Auto-Open Schedule System

## 🎯 Objective
Implement automated door unlocking based on time schedules for regular business hours.

### 🏷️ Visual Tracking Requirement
*   Any page modified by this phase must include a visible **[MODIFIED]** tag.
*   New components must be marked with a visible **[NEW]** tag.

## 🏗️ Technical Requirements

### 1. Auto-Open Configuration UI
- [ ] **Create AutoOpenSchedule.razor**: [**TAG: NEW**]
    - Configure which doors auto-unlock
    - Set time ranges for auto-unlock
    - Day-of-week selector
    - Enable/disable toggle per schedule

### 2. Backend Scheduler Service
- [ ] **Create AutoOpenSchedulerService**:
    - Background service that runs continuously
    - Checks schedules every minute
    - Triggers door unlock commands at scheduled times
    - Logs all auto-open events

### 3. Integration with Controllers
- [ ] **Controller Command Integration**:
    - Send unlock commands to real controllers
    - Handle controller offline scenarios
    - Retry logic for failed commands
    - Status feedback to UI

## 🚀 Success Criteria
- [ ] Doors unlock automatically at scheduled times
- [ ] Schedules persist across app restarts
- [ ] Failed unlocks are logged and retried
- [ ] UI shows current auto-open status
