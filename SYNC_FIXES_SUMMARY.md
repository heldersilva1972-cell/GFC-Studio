# Event Sync Verification Summary

## All Bugs Fixed:

### 1. ✅ Request Payload (MengqiControllerClient.cs, Line 191-192)
**Issue**: Event index was sent at byte 8, controller expected it at byte 20
**Fix**: Added 12 bytes padding to shift index to byte 20
```csharp
var payload = new byte[16];
BitConverter.GetBytes(eventIndex).CopyTo(payload, 12);
```

### 2. ✅ Response Parser (WgResponseParser.cs, Line 202)
**Issue**: Parser was reading event data from byte 24, actual data starts at byte 8
**Fix**: Changed slice offset from 24 to 8
```csharp
var eventData = packet.Slice(8, 20);
```

### 3. ✅ RawIndex Calculation (RealControllerClient.cs, Line 374)
**Issue**: Was calculating `RawIndex = lastIndex + i + 1` when fetching single event at `lastIndex`
**Fix**: Use `lastIndex` directly since we fetch one event at that exact index
```csharp
RawIndex = lastIndex,
```

### 4. ✅ LastRecordIndex Update (ControllerEventService.cs, Line 256)
**Issue**: Always set to `currentIndex` even if some events failed to save
**Fix**: Set to actual last successfully saved index
```csharp
uint newIndex = totalSaved > 0 
    ? startSyncIndex + (uint)totalSaved - 1 
    : lastReadIndex;
```

### 5. ✅ Time Sync Payload (WgPayloadFactory.cs, Line 35)
**Issue**: Time data sent at byte 8, controller expected it at byte 20
**Fix**: Added 12 bytes padding
```csharp
var payload = Allocate(profile, 20);
int baseOffset = 12;
payload[baseOffset + 0] = ToBcd(now.Year / 100);
```

### 6. ✅ Auto Time Sync (ControllerStatusMonitor.cs, Line 115)
**Issue**: Controller time was never synced, causing old timestamps
**Fix**: Added automatic time sync when controller is online
```csharp
if (isOnline)
{
    await controllerClient.SyncTimeAsync(controller.Id, CancellationToken.None);
}
```

## Database Schema Fixed:
- Added missing columns to SystemSettings table (TwilioAccountSid, BackupStoragePath, etc.)

## Verification Steps:
1. Clean database: `CleanAndResync.sql` executed
2. All code changes applied
3. Ready for restart

## Expected Behavior After Restart:
1. Status Monitor connects and syncs time to Jan 9, 2026
2. Event Sync Service starts after 10s delay
3. Fetches last 1000 events from controller
4. Events appear in UI with correct timestamps and card numbers
5. New events (door opens) appear immediately with current date
