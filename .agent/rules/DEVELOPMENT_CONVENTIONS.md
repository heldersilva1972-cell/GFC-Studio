---
trigger: always_on
---

# Development Rules
* **Plan First:** Always propose a concise implementation plan before editing any files.
* **Scope Control:** Do not refactor or "clean up" code outside the immediate scope of the task.
* **Iterative Workflow:** If a task is complex, break it into smaller sub-tasks and ask for review after each step.
* **Architecture Awareness:** Always check the primary database context, schema definitions, or existing models before proposing new data structures.
* **POS Versioning Protocol:** Whenever a functional or UI update is made to the **GFC-Pos-Standalone** suite:
    1. **Increment** the revision number in `PosVersionService.cs`.
    2. **Sync** to all `service-worker*.js` files.
    3. **Sync** to `GFC.Pos.Mobile.csproj` (`ApplicationDisplayVersion` and `ApplicationVersion`).
    4. **Annotate** the change in `PosVersionService.cs`.
* **Mobile Hub Versioning Protocol:** Whenever modifying **GFC-Mobile-Standalone**:
    1. **Increment** the version in `version.json`.
    2. **Sync** to the revision comment in `GFC.Mobile/wwwroot/service-worker.js`.
* **Webapp Versioning Protocol:** Whenever modifying the main **webapp** (GFC.BlazorServer):
    1. **Increment** the `Revision` in `appsettings.json`.