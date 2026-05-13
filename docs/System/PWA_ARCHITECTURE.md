# GFC PWA Architecture & Maintenance Guide

## Overview
The GFC application uses a Progressive Web App (PWA) architecture to provide a native-like experience on mobile devices (Android/iOS). This includes standalone display (no browser bar), offline support, and push notifications.

## ⚠️ Critical "Golden Rules"
To prevent breaking the installation flow (especially on Android 16+), these rules MUST be followed:

1. **HTTPS Enforcement**: PWAs require HTTPS. The `UrlHelperService.cs` is configured to always generate `https://` links for device setup. Do not revert this.
2. **Start URL Stability**: The `manifest.json` `start_url` and `id` must remain `"/"`. Changing these causes Chrome to treat the app as a "new" identity, blocking updates or causing "shortcut" downgrades.
3. **Single Event Listener**: Do NOT add multiple `beforeinstallprompt` listeners. The primary listener is in the `<head>` of `_Host.cshtml`.
4. **No pwa-installer.js**: Avoid using the legacy `/js/pwa-installer.js` script in host pages. It conflicts with the modern "Early Catch" logic.

## Technical Architecture

### 1. The "Early Catch" Logic (`_Host.cshtml`)
Because mobile browsers fire the `beforeinstallprompt` very early, we capture it in the `<head>` before Blazor even starts:
```javascript
window.addEventListener('beforeinstallprompt', (e) => {
    e.preventDefault();
    window._pwaBeforeInstallPrompt = e;
});
```

### 2. Service Worker Readiness
Android 16+ often rejects installation if the Service Worker isn't fully active. Our `GFC.triggerInstall` function includes a "wait-loop" that ensures the worker is ready before showing the prompt.

### 3. Connectivity Shield Protection
The GFC mobile "Connectivity Shield" (which reloads the page on signal return) is programmed to **pause reloads** if `window._pwaInstalling` is true. This protects the 30-60 second "WebAPK Generation" process on Android.

## How to Trigger Installation in Code

If you create a new Setup or Admin page and want to add an "Install" button, use the `window.GFC` bridge:

### In Blazor (C#):
```csharp
// Check if installable
bool canInstall = await JSRuntime.InvokeAsync<bool>("GFC.canInstall");

// Trigger the prompt
bool success = await JSRuntime.InvokeAsync<bool>("GFC.triggerInstall");
```

### In JavaScript:
```javascript
// Check
if (window.GFC.canInstall()) { ... }

// Trigger
await window.GFC.triggerInstall();
```

## Maintenance Checklist
- **Updating the App**: If you update the Service Worker logic, increment the version tag in `_Host.cshtml` (`/service-worker.js?v=9`) and the `CACHE_NAME` inside the worker.
- **Icon Changes**: If changing icons, update `manifest.json`. Ensure icons are available in both `any` and `maskable` purposes to satisfy Android requirements.
- **Redirects**: Ensure the flow always ends on a page within the manifest `scope` (default is `/`).

## Troubleshooting "Ghost Installs"
If the app installs as a "Shortcut" (with Chrome bar) instead of a "WebAPK" (standalone):
1. Clear Chrome Site Data for the domain.
2. Verify `manifest.json` passes the "PWA Diagnostic" page.
3. Ensure no `location.reload()` is firing during the installation process.
