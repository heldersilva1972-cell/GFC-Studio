# PWA Implementation - Testing & Verification Guide

## Overview
This document provides step-by-step instructions to verify that the GFC Web App is properly configured as an installable Progressive Web App (PWA).

## Implementation Summary

### Files Created/Modified:
1. **`wwwroot/manifest.json`** - Updated with complete PWA metadata
2. **`wwwroot/service-worker.js`** - NEW: Minimal service worker for installability
3. **`wwwroot/images/pwa-icon-192.png`** - NEW: 192x192 app icon
4. **`wwwroot/images/pwa-icon-512.png`** - NEW: 512x512 app icon
5. **`Pages/_Host.cshtml`** - Added PWA meta tags and service worker registration
6. **`Program.cs`** - Configured static file serving with proper MIME types

## Pre-Deployment Checklist

### 1. Verify Files Exist
Ensure the following files are present:
- [ ] `wwwroot/manifest.json`
- [ ] `wwwroot/service-worker.js`
- [ ] `wwwroot/images/pwa-icon-192.png`
- [ ] `wwwroot/images/pwa-icon-512.png`

### 2. Build and Deploy
```bash
dotnet build
dotnet run
```

## Testing Instructions

### A. Desktop Testing (Chrome/Edge)

1. **Open the app** in Chrome or Edge
2. **Check Console** (F12 → Console tab):
   - Look for: `[PWA] Service Worker registered successfully`
   - Should NOT see any manifest errors
   
3. **Check Application Tab** (F12 → Application):
   - **Manifest**: Should show "GFC App" with icons
   - **Service Workers**: Should show registered worker at `/service-worker.js`
   
4. **Install Prompt**:
   - Look for install icon in address bar (⊕ or computer icon)
   - Click to install
   - App should open in standalone window

5. **Lighthouse Audit** (F12 → Lighthouse):
   - Run PWA audit
   - Should pass "Installable" checks
   - May show warnings about offline support (expected)

### B. iOS Testing (iPhone/iPad)

1. **Open Safari** and navigate to the app
2. **Tap Share button** (square with arrow)
3. **Look for "Add to Home Screen"** option
4. **Tap "Add to Home Screen"**
5. **Verify**:
   - Icon appears on home screen
   - Tapping icon opens app full-screen
   - No Safari UI visible when running

### C. Android Testing

1. **Open Chrome** and navigate to the app
2. **Look for install banner** at bottom of screen
   - OR tap menu (⋮) → "Install app" / "Add to Home screen"
3. **Install the app**
4. **Verify**:
   - Icon appears in app drawer
   - Tapping icon opens app full-screen
   - No browser UI visible when running

## Verification Checklist

### ✅ Core PWA Requirements
- [ ] Manifest.json is accessible at `/manifest.json`
- [ ] Service worker is registered successfully
- [ ] HTTPS is enabled (required for PWA)
- [ ] Icons are loading without 404 errors
- [ ] No console errors related to manifest or service worker

### ✅ Installability
- [ ] Desktop: Install icon appears in address bar
- [ ] iOS: "Add to Home Screen" option available
- [ ] Android: Install prompt or menu option available
- [ ] Installed app opens in standalone mode (no browser chrome)

### ✅ Functionality After Install
- [ ] App loads correctly when launched from home screen/desktop
- [ ] Authentication still works
- [ ] SignalR connections work (Blazor Server real-time updates)
- [ ] VPN-only access is still enforced
- [ ] No regressions in existing features

### ✅ Network Behavior
- [ ] Service worker uses network-first strategy
- [ ] SignalR connections are NOT cached
- [ ] App works normally with active internet connection
- [ ] No aggressive caching breaking Blazor Server

## Troubleshooting

### Issue: "Add to Home Screen" not appearing on iOS
**Solution**: 
- Ensure you're using Safari (not Chrome on iOS)
- Verify HTTPS is enabled
- Check that manifest.json is loading without errors

### Issue: Install prompt not showing on Desktop
**Solution**:
- Check Chrome flags: `chrome://flags/#enable-desktop-pwas`
- Verify Lighthouse PWA audit passes
- Clear browser cache and reload

### Issue: Service Worker registration fails
**Solution**:
- Check console for specific error
- Verify `/service-worker.js` is accessible
- Ensure HTTPS is enabled
- Check for CORS issues

### Issue: App breaks after installing
**Solution**:
- Check if SignalR connections are being cached
- Verify service worker is using network-first for `/_blazor`
- Check console for WebSocket errors

### Issue: Icons not loading
**Solution**:
- Verify files exist in `wwwroot/images/`
- Check file permissions
- Clear browser cache
- Verify manifest.json paths are correct

## Production Deployment Notes

### HTTPS Requirement
- PWA **requires HTTPS** in production
- Service workers will NOT register over HTTP (except localhost)
- Ensure SSL certificate is valid

### VPN-Only Access
- PWA works perfectly with VPN-only access
- Users must be connected to VPN to access the app
- Install process requires VPN connection
- Once installed, app still requires VPN to function

### Cache Strategy
- Current implementation uses **network-first**
- This ensures Blazor Server SignalR works correctly
- No offline functionality (by design)
- Manifest and service worker are set to no-cache

## Success Criteria

The PWA implementation is successful when:

1. ✅ Directors can install the app on their devices
2. ✅ App opens full-screen without browser UI
3. ✅ App icon appears on home screen/desktop
4. ✅ VPN-only access remains enforced
5. ✅ No regressions in authentication or real-time features
6. ✅ Lighthouse PWA audit shows "Installable" as passing

## Next Steps

After successful PWA deployment:

1. **User Training**: Create simple instructions for directors on how to install
2. **Onboarding Flow**: Integrate install prompt into first-time user experience
3. **Monitoring**: Track install metrics and user feedback
4. **Future Enhancements**: 
   - Add offline fallback page (optional)
   - Implement push notifications (if needed)
   - Add app shortcuts for common actions

## Support

If issues persist after following this guide:
1. Check browser console for specific errors
2. Run Lighthouse PWA audit for detailed diagnostics
3. Verify all files are deployed correctly
4. Test on multiple devices/browsers to isolate the issue
