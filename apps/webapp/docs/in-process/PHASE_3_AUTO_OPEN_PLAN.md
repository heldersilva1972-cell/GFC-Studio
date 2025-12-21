# Phase 3: Auto-Open Schedules (Hardware Automation)

## 🎯 Objective
Implement the logic required to automatically unlock and lock doors based on a predefined Time Profile. This brings "Smart Building" capabilities to the GFC Studio.

## 🏗️ Technical Requirements

### 1. UI Extension: `AutoOpenTab.razor`
*   **Door Mapping**: A grid showing all doors on the selected controller.
*   **Profile Linkage**: A dropdown for each door to select an existing Time Profile.
*   **Manual Override**: A toggle to temporarily suspend the schedule without deleting it.

### 2. Simulation Engine Update (`SimulationStateEngine.cs`)
*   **Background Monitor**: A low-frequency task (every 30-60 seconds) that checks the current "Simulation Time."
*   **State Transition**: If current time enters a "Granted" window for an Auto-Open door, trigger the `OpenDoorAsync` logic with an "Auto-Unlock" reason.
*   **Lock Transition**: When the window ends, automatically trigger a "Lock" command.

### 3. Real Mode Synchronization
*   **Agent PC Command**: Ensure the `WriteAutoOpenAsync` method correctly transmits the schedule to the physical Agent PC.
*   **Verification**: Implement a "Sync Verification" badge that confirms the hardware has acknowledged and loaded the schedule.

## 🚀 UX Goals
*   **Safety First**: Clearly indicate on the Dashboard when a door is held open by a schedule vs. a manual command.
*   **Visual Debugging**: Show the "Next Event" (e.g., "Auto-unlocking at 08:00 AM") in the status panel.
