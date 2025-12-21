# Plan: Purge Simulation Mode from GFC Studio V2

## 🎯 Objective
Completely remove all simulation-related code, UI components, services, and data structures to ensure the application only interacts with real hardware.

## 🏗️ Technical Checklist

### 1. UI Purge & Visual Tracking
- [ ] **Implement Change Tracking UI**: Create global CSS classes `.gfc-modified-tag` and `.gfc-new-tag` in `app.css`.
- [ ] **Tag Modified Pages**: Any page that is modified (but not deleted) must have a `MODIFIED` tag at the top with a note of what changed (e.g., "Removed Simulation Toggle").
- [ ] **Tag New Elements**: Any new elements added to a page must be wrapped in a container with a `NEW` tag and a description.
- [ ] **Delete `apps/webapp/GFC.BlazorServer/Components/Pages/Simulation/`** (Entire folder).
- [ ] **Delete `wwwroot/css/simulation-mode.css`** and `simulation-dashboard-compact.css`.
- [ ] **Modify `Settings.razor`**: Remove the "Use Real Controllers" toggle. [**TAG: MODIFIED**]
- [ ] **Modify `CardAccessController.razor`**: Remove status badges and logic that references `SimulationControllerClient`. [**TAG: MODIFIED**]
- [ ] **Modify `NavMenu.razor`**: Remove the `/simulation` navigation link. [**TAG: MODIFIED**]

### 2. Service & Backend Purge
- [ ] **Delete `apps/webapp/GFC.BlazorServer/Services/Simulation/`** (Entire folder).
- [ ] **Delete `apps/webapp/GFC.BlazorServer/Services/SimulationReplay/`** (Entire folder).
- [ ] **Delete `DynamicControllerClient.cs`**: Replace with direct usage of `RealControllerClient`.
- [ ] **Delete `SimulationControllerClient.cs`** (both locations).
- [ ] **Delete `SimulationTraceService.cs`** and related extensions.
- [ ] **Delete `SimulationGuard.cs`** and `SimulationModeBlockedException.cs`.

### 3. Data & Persistence Purge
- [ ] **Delete `SimulationControllerTrace.cs`** entity.
- [ ] **Modify `GfcDbContext.cs`**: Remove `SimulationControllerTraces` DbSet and `UseRealControllers` property from `SystemSettings`.
- [ ] **Generate Migration**: Create a DB migration to drop simulation tables and columns.

### 4. Integration Cleanup
- [ ] **Modify `Program.cs`**: Remove simulation service registrations.
- [ ] **Modify `RealControllerClient.cs`**: Remove calls to `ISimulationGuard`.
- [ ] **Remove `AccessControlSimulationOptions.cs`** configuration.

## 🔄 Cross-Plan Impact (Update Required)
The removal of Simulation Mode affects the following in-process plans:
1. **Phase 3 (Auto-Open)**: Must be rewritten to use a new `HardwareAutomationService` instead of the `SimulationStateEngine`.
2. **Phase 4 (Advanced Modes)**: Interlock logic must be moved to the Real-Mode hardware logic.

## 🚀 Success Criteria
- [ ] All modified/new elements are clearly tagged with visible indicators.
- [ ] Application builds successfully without warnings.
- [ ] All hardware commands trigger `RealControllerClient` directly.
- [ ] No "Simulation Mode" UI elements remain in the app.
- [ ] Database is cleaned of simulation traces.
