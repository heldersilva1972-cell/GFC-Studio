# Phase 4: Advanced Door Modes (Real Hardware)

## 🎯 Objective
Unlock the full potential of the access control hardware by supporting professional security modes and real-mode interlocking logic.

### 🏷️ Visual Tracking Requirement
*   Any page modified by this phase must include a visible **[MODIFIED]** tag with a description (e.g., in `DoorModal.razor`).
*   Any new UI indicators (e.g., `AIRLOCK` badges) must be marked with a visible **[NEW]** tag.

## 🏗️ Technical Requirements

### 1. Extended Door Configuration (`DoorModal.razor`) [**TAG: MODIFIED**]
Add the following configuration options:
*   **Normally Open (Hold Open)**: The door remains unlocked by default. A valid card swipe relocks it (toggle behavior).
*   **Double Swipe/Two-Person Rule**: Requires two valid card reads within 5 seconds to unlock the door.
*   **Toggle Mode**: A card swipe switches the door state from Locked to Unlocked (and vice versa) permanently until the next swipe.

### 2. Interlocking Logic (Real-Mode Guards)
*   **Software Interlocks**: Implement logic in `RealControllerClient` that checks the state of other doors in the "Interlock Group" before allowing an unlock command to proceed.
*   **State Awareness**: The system must frequently poll door states to ensure interlock rules are respected globally.

### 3. Visual Indicators [**TAG: NEW**]
*   **Mode Badges**: Update the door grid to show badges like `AIRLOCK`, `TWO-PERSON`, or `TOGGLE`.
*   **Conflict UI**: If a door is blocked by an interlock, show a distinctive "Interlock Blocked" animation.

## 🚀 UX Goals
*   **Clarity**: Ensure that users don't get trapped in interlocks without clear visual feedback.
*   **Safety**: Ensure that "Fire Alarm" overrides still work effectively regardless of the software door mode.
