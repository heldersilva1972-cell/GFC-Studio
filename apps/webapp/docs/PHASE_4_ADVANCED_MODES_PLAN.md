# Phase 4: Advanced Door Modes

## 🎯 Objective
Unlock the full potential of the access control hardware by supporting professional security modes and interlocking logic.

## 🏗️ Technical Requirements

### 1. Extended Door Configuration (`DoorModal.razor`)
Add the following configuration options:
*   **Normally Open (Hold Open)**: The door remains unlocked by default. A valid card swipe relocks it (toggle behavior).
*   **Double Swipe/Two-Person Rule**: Requires two valid card reads within 5 seconds to unlock the door.
*   **Toggle Mode**: A card swipe switches the door state from Locked to Unlocked (and vice versa) permanently until the next swipe.

### 2. Interlocking Logic (Airlock Security)
*   **Group Definition**: Ability to group two or more doors into an "Interlock Group."
*   **Rule Enforcement**: If any door in the group is `Open`, all other doors in the group must reject `Unlock` commands (Visual/Audio warning in UI).
*   **Simulation Support**: Update `SimulationStateEngine` to enforce these hardware rules synchronously.

### 3. Visual Indicators
*   **Mode Badges**: Update the door grid to show badges like `AIRLOCK`, `TWO-PERSON`, or `TOGGLE` so users understand the active logic.
*   **Conflict UI**: If a door is blocked by an interlock, show a distinctive "Interlock Blocked" animation.

## 🚀 UX Goals
*   **Clarity**: Ensure that users don't get trapped in interlocks without clear visual feedback on *why* a door won't open.
*   **Safety**: Ensure that "Fire Alarm" overrides still work effectively regardless of the door mode.
