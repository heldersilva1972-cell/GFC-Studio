# GFC Event Sync Deployment Instructions

## Problem Summary
The ControllerEventSyncService is not fetching events from the controller.
All code fixes have been made on the dev PC but are NOT deployed to the host.

## Files Modified (All fixes applied):
1. WgResponseParser.cs - Event data offset corrected (byte 8)
2. MengqiControllerClient.cs - Request payload offset corrected (byte 20)
3. RealControllerClient.cs - RawIndex calculation fixed
4. ControllerEventService.cs - LastRecordIndex update logic fixed
5. WgPayloadFactory.cs - Time sync payload offset corrected (byte 20)
6. ControllerStatusMonitor.cs - Auto time sync added
7. ControllerEventSyncService.cs - Enhanced logging added

## Deployment Steps:

### 1. On DEV PC (this computer):
```cmd
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"
dotnet publish -c Release -o "C:\Temp\GFCDeploy"
```

### 2. Copy files to HOST:
- Copy entire contents of `C:\Temp\GFCDeploy` to host's `C:\inetpub\GFCWebApp`
- Overwrite all existing files

### 3. On HOST PC:
```cmd
iisreset
```

### 4. Verify Deployment:
- Check file dates in `C:\inetpub\GFCWebApp` - should be TODAY (1/9/2026)
- Check logs for: "=== CONTROLLER EVENT SYNC SERVICE STARTING ==="
- Hardware Traffic Monitor should show GETNEWEVENTS (0xB0) commands

## Database Cleanup (if needed):
```sql
USE ClubMembership;
DELETE FROM ControllerLastIndexes WHERE ControllerId = 2;
DELETE FROM ControllerEvents WHERE ControllerId = 2;
```

## Expected Behavior After Deployment:
1. Service starts with CRITICAL log messages
2. After 10 seconds, begins syncing
3. Fetches last 1000 events from controller (index 150,938,168 to 150,939,168)
4. Events appear in UI with current timestamps (Jan 9, 2026)
5. Hardware Traffic Monitor shows GETNEWEVENTS commands every 30 seconds
