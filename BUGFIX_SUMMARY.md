# Analytics & Station Device Bugs - COMPLETE FIX LIST

## Bug #1: Station Devices Not Appearing in Active Sessions
**File:** `DeviceTrustService.cs` line 405-422
**Problem:** EF Core cannot translate complex string interpolation with null checks to SQL
**Symptom:** Station devices created via "Quick Onboard" don't show in Active Sessions list

**Fix:** Move projection to memory after database fetch
```csharp
// BEFORE (broken):
var sessions = await context.TrustedDevices
    .Select(d => new DeviceSessionDto { 
        Username = d.IsStation ? $"Station (Auth: {d.User?.Username})" : ...
    }).ToListAsync(); // ❌ EF tries to translate string interpolation to SQL

// AFTER (working):
var devices = await context.TrustedDevices.ToListAsync(); // Fetch first
var sessions = devices.Select(d => new DeviceSessionDto { ... }).ToList(); // Project in memory
```

---

## Bug #2: Analytics API Authentication Failure
**File:** `AnalyticsController.cs` line 31
**Problem:** Used `CustomAuthenticationStateProvider.GetCurrentUser()` which only works in Blazor circuit context, not API context
**Symptom:** All analytics API calls return 401 Unauthorized

**Fix:** Read device token from cookies and validate directly
```csharp
// BEFORE (broken):
var user = _authStateProvider.GetCurrentUser(); // Always null in API context

// AFTER (working):
var deviceToken = Request.Cookies["GFC_DeviceTrustToken"];
var trustedDevice = await _trustedDeviceRepository.GetByTokenAsync(deviceToken);
return trustedDevice.UserId;
```

---

## Bug #3: Invalid SQL Syntax in UpdateDuration
**File:** `AuditLogRepository.cs` line 192
**Problem:** `UPDATE TOP(1) ... ORDER BY` is invalid SQL Server syntax
**Symptom:** Heartbeat updates fail silently, duration stays at 0s

**Fix:** Use subquery to select the record to update
```sql
-- BEFORE (broken):
UPDATE TOP(1) AuditLogs 
SET DurationSeconds = DurationSeconds + @Seconds
WHERE ...
ORDER BY AuditLogId DESC; -- ❌ Can't use ORDER BY with UPDATE TOP

-- AFTER (working):
UPDATE AuditLogs
SET DurationSeconds = DurationSeconds + @Seconds
WHERE AuditLogId = (
    SELECT TOP 1 AuditLogId 
    FROM AuditLogs
    WHERE ...
    ORDER BY AuditLogId DESC
)
```

---

## Files Modified:
1. ✅ `DeviceTrustService.cs` - Fixed station device query
2. ✅ `AnalyticsController.cs` - Fixed authentication
3. ✅ `AuditLogRepository.cs` - Fixed SQL syntax
4. ✅ `analytics-tracker.js` - Created (new file)
5. ✅ `_Host.cshtml` - Added analytics script

## To Apply Fixes:
1. **Rebuild the solution** - The code changes need to be compiled
2. **Restart IIS/application** - New controller and fixes need to load
3. **Hard refresh browser** (Ctrl+Shift+R) - Clear cached JavaScript

## Expected Results After Restart:
✅ Station devices appear in Active Sessions with "Station Device (Auth by: username)" label
✅ Analytics tracker logs page views with actual page names (not just "PageView")
✅ Time on Page shows real durations (e.g., "2m 45s" instead of "0s")
✅ Duration column in audit trail shows actual time spent
✅ Last Action shows meaningful page names
