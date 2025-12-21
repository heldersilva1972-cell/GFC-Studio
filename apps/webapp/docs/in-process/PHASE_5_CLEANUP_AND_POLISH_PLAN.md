# Phase 5: Cleanup & Project Polish

## 🎯 Objective
Transition the project from "Development" to "Production-Ready" by eliminating technical debt, purging legacy files, and refining the final UX details.

## 🏗️ Technical Requirements

### 1. Legacy File Purge
The modernization process resulted in redundant files. These must be safely removed:
*   **Duplicate Service Clients**: Delete redundant copies of `SimulationControllerClient.cs` and `RealControllerClient.cs` that are not in the `Services/Controllers` directory.
*   **Legacy Components**: Remove `ControllerStatusPage.razor` and other standalone pages replaced by the unified `CardAccessController.razor`.

### 2. Settings Consolidation
*   **Global Mode Toggle**: Move the "Use Real Controllers" switch from the hardware dashboard to the main `Settings.razor` page.
*   **Centralized Configuration**: Ensure all simulation-related settings (Replay, Presets) are consolidated in one section of the System Settings.

### 3. Namespace & Code Standards
*   **Namespace Alignment**: Audit all new hardware-related classes to ensure they use `GFC.BlazorServer.Services.Controllers`.
*   **Documentation Linkage**: Ensure all `.md` files in `/docs` are cross-referenced in the main `README.md`.

### 4. Final Build Verification
*   **Linting/Warnings**: Resolve any remaining compiler warnings or CSS linting issues.
*   **Performance Benchmark**: Verify that the "Dynamic Proxy" does not introduce measurable latency in high-traffic scenarios.

## 🚀 UX Goals
*   **Professionalism**: The app should feel "tight" and intentional, with no "ghost" menu items or broken links.
*   **Handoff Readiness**: A clean repository that another developer can pick up and understand instantly.
