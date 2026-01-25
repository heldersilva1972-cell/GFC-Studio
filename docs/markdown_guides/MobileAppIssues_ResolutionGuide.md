# Mobile App Issues - Resolution Guide

## Issue 1: Notification Setup Getting Stuck ✅ FIXED

**Problem:** After enabling notifications, users get stuck on the notification screen instead of seeing the "Go to Dashboard" button.

**Root Cause:** The UI wasn't updating after `_isSubscribed` was set to `true`.

**Fix Applied:**
- Added `StateHasChanged()` call immediately after setting `_isSubscribed = true`
- Added early `NextStep()` calls for edge cases (no user, no VAPID key, denied permission)

**File Modified:** `DeviceWizard.razor` (lines 366-420)

---

## Issue 2: How to Terminate Someone's Session ✅ DOCUMENTED

**Solution:** Run the SQL script `TerminateUserSession.sql`

**Options Available:**
1. **Revoke Device Tokens** - Forces re-login on all devices
2. **Revoke Passkeys** - Removes biometric access
3. **Force Password Change** - Requires new password on next login
4. **Deactivate Account** - Completely disables the account

**Usage:**
```sql
DECLARE @UserId INT = 2; -- Change to target user
-- Run the script sections you need
```

**File Created:** `DatabaseScripts/TerminateUserSession.sql`

---

## Issue 3: Mobile App Should Not Show Desktop Dashboard ✅ FIXED

**Problem:** Mobile users could access the desktop dashboard, which isn't optimized for mobile.

**Fix Applied:**
- Removed "Main Dashboard" card from Mobile Hub
- Removed Dashboard access check from `HasAccessTo()` method
- Mobile users now stay in the mobile-optimized interface

**File Modified:** `MobileHub.razor` (lines 90-106, 379-390)

**Mobile Portal Structure:**
- `/mobile` - Mobile Hub (landing page)
- `/mobile/shift-report` - Shift reporting
- `/mobile/analytics` - Mobile analytics
- Settings and Support links remain available

---

## Issue 4: User Permissions Not Showing Mobile Pages ⚠️ NEEDS INVESTIGATION

**Problem:** After granting mobile page permissions to a user, the pages don't appear.

**Diagnostic Steps:**

### Step 1: Verify Mobile Pages Exist in Database
Run: `DatabaseScripts/DiagnosticQueries_MobilePermissions.sql` (Section 1)

Expected output:
```
PageId | PageName          | PageRoute
-------|-------------------|-------------------
XX     | Mobile Hub        | /mobile
XX     | Shift Report      | /mobile/shift-report
XX     | Analytics         | /mobile/analytics
```

If pages are missing, run: `DatabaseScripts/20260119_RegisterMobilePages.sql`

### Step 2: Check User's Permissions
Run: `DiagnosticQueries_MobilePermissions.sql` (Section 2)

Change `@UserId` to the affected user's ID.

### Step 3: Verify Permission Count
Run: `DiagnosticQueries_MobilePermissions.sql` (Section 3)

Should show `TotalPermissions > 0` if permissions exist.

### Step 4: Grant Permissions (If Missing)
Uncomment and run Section 4 of `DiagnosticQueries_MobilePermissions.sql`

**Common Causes:**
1. Mobile pages not registered in `AppPages` table
2. Permissions granted but user hasn't logged out/in
3. Case-sensitive route mismatch (`/Mobile` vs `/mobile`)
4. User cache not refreshed

**Quick Fix:**
```sql
-- Force user to re-login
DELETE FROM TrustedDevices WHERE UserId = [USER_ID];
```

---

## Testing Checklist

### Notification Setup
- [ ] User enables notifications
- [ ] "Notifications Enabled" message appears
- [ ] "Go to Dashboard" button shows immediately
- [ ] Clicking button navigates to dashboard

### Session Termination
- [ ] Run `TerminateUserSession.sql` for test user
- [ ] User is logged out on next page load
- [ ] User must re-login to access system

### Mobile Portal
- [ ] Mobile users land on `/mobile` hub
- [ ] Only mobile-specific cards show
- [ ] No desktop dashboard link visible
- [ ] Navigation stays within mobile routes

### Mobile Permissions
- [ ] Admin grants mobile permissions to user
- [ ] User logs out and back in
- [ ] Mobile pages appear in their portal
- [ ] Clicking cards navigates correctly

---

## Files Modified

1. `GFC.BlazorServer/Components/Pages/Setup/DeviceWizard.razor`
   - Fixed notification setup stuck issue

2. `GFC.BlazorServer/Components/Pages/Mobile/MobileHub.razor`
   - Removed desktop dashboard link
   - Cleaned up access checks

3. `DatabaseScripts/TerminateUserSession.sql` (NEW)
   - Session termination tool

4. `DatabaseScripts/DiagnosticQueries_MobilePermissions.sql` (NEW)
   - Permission troubleshooting tool

---

## Next Steps

1. **Restart the application** to load the fixes
2. **Test notification flow** with a new user
3. **Run diagnostic queries** to check mobile permissions
4. **Verify mobile portal** shows only mobile pages
5. **Test session termination** on a test account

---

## Support Notes

**For Users Stuck on Notification Screen:**
- This is now fixed - restart the app
- Users can also click "Skip" if they don't want notifications

**For Granting Mobile Access:**
1. Ensure mobile pages are registered (run registration script)
2. Grant permissions via admin UI or SQL
3. User must log out and back in
4. Check diagnostic queries if still not working

**For Emergency Session Termination:**
- Use `TerminateUserSession.sql`
- Change `@UserId` to target user
- Run desired sections (device tokens, passkeys, etc.)
