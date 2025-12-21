# GFC Studio V2: Unified Hardware & Access Roadmap

This document outlines the progression of the GFC Studio V2 project, consolidating the separate simulation and hardware management components into a unified, professional-grade platform.

---

## 🏗️ Phase 1: Consolidation & Hybrid Architecture (COMPLETED)
*Focus: Infrastructure, Universal Search, and modern UI implementation.*

- [x] **Universal Dashboard**: Created `CardAccessController.razor` as the single hub for all hardware.
- [x] **Hybrid Client Architecture**: Implemented `DynamicControllerClient` and `SimulationStateEngine` to allow real-time switching between Real and Simulated modes.
- [x] **Theme & Design System**: Implemented a global CSS variable system with High-Contrast Dark and Soft Light modes (WCAG AAA compliant).
- [x] **Heartbeat Sync**: Fixed the "Offline" status bug; all controllers now report live state upon login.
- [x] **Unified Settings**: Integrated Scanner Proxy and Mode toggles into the core data models.

---

## 📅 Phase 2: Visual Interval Builder (PENDING)
*Focus: Graphical UI for complex access schedules.*

- [ ] **Interval Picker Component**: Create a reusable Blazor component for selecting Start/End times.
- [ ] **Day-of-Week Toggles**: Graphical selector for Mon-Sun recurring schedules.
- [ ] **Visual Profile Modal**: Implement a drag-and-drop or checklist-based interval list in the Time Profile manager.
- [ ] **Backend Persistence**: Map the visual ranges to the `TimeProfileIntervals` database schema.

---

## 🤖 Phase 3: Auto-Open Schedules (PENDING)
*Focus: Per-door hardware automation.*

- [ ] **Schedule Assignment**: Allow assigning a Time Profile to a specific door for "Auto-Unlock" functionality.
- [ ] **Simulator Logic**: Update the Simulation Engine to automatically transition doors to "Unlocked" when a schedule is active.
- [ ] **Real Mode Sync**: Ensure the Agent PC receives and executes the auto-open commands based on the local clock.

---

## 🔐 Phase 4: Advanced Door Modes (PENDING)
*Focus: Professional security features and logic.*

- [ ] **Normally Open Mode**: Support for doors that stay unlocked until a card is swiped.
- [ ] **Interlock Groups**: Logic to prevent Door B from opening if Door A is currently Open (Airlock/Security).
- [ ] **Toggle Mode**: Card swipe toggles the door between "Locked" and "Stay Unlocked".
- [ ] **Extended Configuration**: Support for firmware-level door settings (Lock Delay, Sensor Sensitivity).

---

## 🧹 Phase 5: System Cleanup & Final Polish (PENDING)
*Focus: Maintenance and final verification.*

- [ ] **Legacy File Purge**: Remove duplicate/obsolete service files (e.g., redundant `SimulationControllerClient` copies).
- [ ] **Settings Consolidation**: Move the "Use Real Controllers" toggle from the dashboard into the Global System Settings page.
- [ ] **Service Refactoring**: Ensure all hardware services live under the `GFC.BlazorServer.Services.Controllers` namespace.
- [ ] **Final Build Audit**: Verify zero warnings and consistent performance in both mode toggles.

---

## 📈 Milestone Checklist
- [x] **Phase 1** (Infrastructure & Theme)
- [ ] **Phase 2** (Visual Builder)
- [ ] **Phase 3** (Auto-Open Logic)
- [ ] **Phase 4** (Advanced Modes)
- [ ] **Phase 5** (Project Cleanup)
