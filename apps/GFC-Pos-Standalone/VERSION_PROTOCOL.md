# GFC POS Versioning Protocol

To ensure the "Update Available" banner works and the terminal doesn't get stuck with old styles, the following files must be updated on **every** revision change.

### 1. The Source of Truth (Code)
These define the version number inside the app itself.
*   **`PosVersionService.cs`**: Updates the UI display and maintains the revision history log.
*   **`PosVersion.props`**: Updates the build system (DLL versioning).

### 2. The Update Trigger (Server)
This is what the running terminal "looks at" to see if it should show the **Update Available** banner.
*   **`version.txt`**: The **Primary Authority**. If this is stale, the banner will never appear.
*   **`version.json`**: The secondary authority (used by some mobile check-ins).

### 3. The Cache-Busters (Deployment)
These ensure the browser doesn't load old `pos-terminal.css` or `app.js` from its memory.
*   **`index.html`**: Update the `?v=X.X.X` strings on CSS/JS links.
*   **`service-worker.published.js`**: Update the `cacheName` so the browser wipes the old offline cache.
*   **`service-worker.js`**: Update the revision comment for developer tracking.

### 4. Mobile App (App Store)
*   **`GFC.Pos.Mobile.csproj`**: Updates the Android/iOS package versions.

---

> [!TIP]
> **Why so many?** 
> Blazor WASM apps are "sticky" in browser memory. We have to scream at the browser in multiple ways (HTML, Service Worker, and Static Files) to get it to actually drop the old code and grab your new UI changes.

> [!IMPORTANT]
> **If the "Update Available" banner isn't showing up:** 
> Always check `version.txt`. It must be higher than the version the terminal is currently running.
