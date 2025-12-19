# GFC Simulation Mode — UI Refactoring & Enhancement Plan

## 1. Dashboard Layout Modularization
**Status:** 🚧 Needs Refactoring
**Goal:** Break the large `Dashboard.razor` into manageable, single-purpose components.

- [ ] **Create `SimulationStatusTile.razor`**
    - Responsibility: Show "Simulation Active" vs "Live" status.
    - Show Clock/Time offset.
    - Show Controller Connection status (Online/Offline).
- [ ] **Create `SimulationControlBench.razor`**
    - Responsibility: Group "Time Controls" (Advance Time) and "Event Injection" logic.
    - Consolidate "Time" card and "Inject Event" card into one logical panel.
- [ ] **Create `SimulationTraceLog.razor`**
    - Responsibility: Handle the "Recent Traces" table.
    - Support auto-refresh toggling.
    - Handle "View Details" modal/slide-out execution.
- [ ] **Create `ControllerVisualizer.razor`**
    - Responsibility: Replace the text-based "Controller State" table.
    - Display visual "LED" style indicators for Door Locks/Relays.
    - Interactive "Reset" button.

## 2. Replay & Timeline UI (The "Missing Link")
**Status:** ❌ Not Started
**Goal:** Allow admins to "watch" a simulation session.

- [ ] **Build `ReplayTimeline.razor`**
    - Visual scrub bar (slider) representing the simulation session duration.
    - Play/Pause/Stop control buttons.
    - Bind to `ISimulationTraceService` to fetch historical events.
- [ ] **Connect Replay to Visualization**
    - When scrubbing timeline, update the `ControllerVisualizer` state to match that historical moment.

## 3. Presets & Scenarios
**Status:** 🟡 Stubbed (Backend exists, Logic empty)
**Goal:** Make "Busy Friday Night" actually do something.

- [ ] **Implement `SimulationPresetService` Logic**
    - Wire up "Quiet Weekday": Set door modes to "Card Only", silence alarms.
    - Wire up "Busy Friday": Start a background task that injects random `CardGranted` events every 2-5 seconds.
    - Wire up "Door Held Open": Trigger a specific sequence (Open -> Wait 5m -> Alarm).
- [ ] **Add "Stop Scenario" Button**
    - Ensure running scenarios can be cancelled immediately.

## 4. Visual Polish & UX
**Status:** 🟡 Needs Improvement
**Goal:** Make it look like a professional studio tool.

- [ ] **Standardize Badges**
    - Unified color scheme for Open (Green/Warning), Closed (Gray), Locked (Red).
- [ ] **Responsive Fixes**
    - Ensure "Test Bench" stacks correctly on smaller screens (already started with Flex changes).
- [ ] **Tracing Detail View**
    - Improve the JSON viewer for event payloads (syntax highlighting or collapsible tree).

## 5. Integration
- [ ] **Verify `MainLayout` Links**
    - Ensure side-nav link to `/simulation` is visible ONLY to admins.
    - Verify `SimulationModeService` correctly intercepts real controller calls globally.
