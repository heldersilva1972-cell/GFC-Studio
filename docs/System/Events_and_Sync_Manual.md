# GFC Access Control: Event Sync & Display System

## 1. Overview

The Event Sync system pulls access logs from the N3000 hardware controller and displays them in real-time in the web application's "Recent Events" panel.

### Key Features
*   **Auto-Sync**: Background sync every 3 seconds (silent, only updates UI when new events detected)
*   **Real-Time Display**: Events appear automatically without manual refresh
*   **Accurate Parsing**: Correctly identifies door number, access result, and event type
*   **Duplicate Prevention**: Unique constraint on RawIndex prevents duplicate event storage

---

## 2. Hardware Event Packet Structure

### Event Packet Format (64 bytes)
The controller sends event data in fixed 64-byte UDP packets with the following structure:

| Byte Range | Field | Description |
|:---|:---|:---|
| 0 | Packet Type | `0x17` (Request) / `0x18` (Reply) |
| 1 | Function Code | `0x20` (Run Status) / `0xB0` (Get Events) |
| 2-3 | Checksum | CRC16-IBM |
| 4-7 | Serial Number | Controller SN (Little Endian) |
| 8-11 | **Event Index** | Unique event number (uint32 LE) |
| 12-15 | **Metadata Block** | Contains result flag and door number |
| 16-19 | **Card Number** | Card ID (uint32 LE) |
| 20-26 | **Timestamp** | BCD format: YY YY MM DD HH mm ss |
| 27-63 | Additional Data | Event-specific data |

### Critical Metadata Block (Bytes 12-15)
```
Byte 12: Constant (usually 0x01)
Byte 13: Result Flag (0x01 = Granted, 0x00 = Denied)
Byte 14: Door Number (0x01 = Door 1, 0x02 = Door 2)
Byte 15: Type (usually 0x01)
```

**Examples:**
- `01 01 01 01` = Access Granted on Door 1
- `01 01 02 01` = Access Granted on Door 2
- `01 00 01 01` = Access Denied on Door 1
- `01 00 02 01` = Access Denied on Door 2

---

## 3. Event Type Mapping

### Access Result Codes (Byte 13)
| Code | Display Label |
|:---|:---|
| `0x00` | Access Denied |
| `0x01` | Access Granted |

### Specialized Event Labels
*   **Momentary Door Unlock**: Detected by card number `906285819` (system-generated unlock command from UI)
*   **Access Granted**: Standard successful card swipe (byte 13 = `0x01`)
*   **Access Denied**: Card swipe rejected (byte 13 = `0x00`)

### Display Rules
1. **Momentary Unlock** (card #906285819): `Door X - Momentary Door Unlock` (no card number shown)
2. **Access Granted** (byte 13 = 0x01): `Door X - Access Granted - Card #[number]`
3. **Access Denied** (byte 13 = 0x00): `Door X - Access Denied - Card #[number]`

---

## 4. Auto-Sync System

### Background Sync Timer
- **Frequency**: Every 3 seconds
- **Behavior**: Silent sync that only updates UI when new events are detected
- **Method**: `SilentBackgroundSync()` in `OverviewTab.razor`

### Sync Process
1. Timer triggers every 3 seconds
2. Calls `EventService.SyncFromControllerAsync()`
3. Returns count of new events synced
4. If `newCount > 0`, reloads events and updates UI
5. If `newCount = 0`, no UI update (prevents flashing)

### Manual Refresh
- User can click refresh button for immediate sync
- Shows status messages during sync
- Uses `SyncAndLoadEvents()` method

---

## 5. Database Schema

### ControllerEvents Table
| Column | Type | Description |
|:---|:---|:---|
| Id | int | Primary key |
| ControllerId | int | FK to Controllers |
| DoorId | int? | FK to Doors (nullable) |
| RawIndex | int | **Unique** hardware event index |
| DoorOrReader | int | Raw door number from hardware |
| CardNumber | long? | Card ID |
| EventType | int | Result code (0=denied, 1=granted) |
| TimestampUtc | DateTime | Event timestamp |
| RawData | string | Full 64-byte packet (hex) |
| IsByCard | bool | Card swipe event |
| IsByButton | bool | Button/system event |
| CreatedUtc | DateTime | DB insert time |

### Unique Constraint
```sql
CREATE UNIQUE INDEX IX_ControllerEvents_ControllerId_RawIndex 
ON ControllerEvents(ControllerId, RawIndex);
```
This prevents duplicate events from being stored.

---

## 6. Code Architecture

### Key Components

**Parser**: `WgResponseParser.cs`
- Reads byte 13 for result flag (granted/denied)
- Reads byte 14 for door number
- Reads bytes 8-11 for event index
- Reads bytes 16-19 for card number
- Reads bytes 20-26 for timestamp (BCD)

**Sync Service**: `ControllerEventService.cs`
- `SyncFromControllerAsync()`: Pulls new events from hardware
- Returns count of new events synced
- Prevents duplicates using RawIndex check

**UI Component**: `OverviewTab.razor`
- `SilentBackgroundSync()`: Background timer (every 3 seconds)
- `SyncAndLoadEvents()`: Manual refresh with status messages
- `GetEventDescription()`: Formats event display text

---

## 7. Troubleshooting

### Events not appearing
1. Check if auto-sync timer is running (should see events within 3 seconds)
2. Verify controller is online
3. Check browser console for errors
4. Manually click refresh button

### Duplicate events
1. Run `ADD_UNIQUE_CONSTRAINT_RAWINDEX.sql` to add unique constraint
2. Restart application

### Wrong door number displayed
1. Verify door mapping in database (`Doors` table)
2. Check `DoorOrReader` value in `ControllerEvents` table
3. Confirm hardware is sending correct byte 14 value

### Access Granted/Denied incorrect
1. Check `EventType` column in database (should be 0 or 1)
2. Verify parser is reading byte 13 correctly
3. Examine `RawData` field to see actual hardware bytes

---

## 8. Testing Checklist

✅ **Event Display**
- [ ] Door 1 card swipe shows "Door 1"
- [ ] Door 2 card swipe shows "Door 2"
- [ ] Access granted shows "Access Granted - Card #..."
- [ ] Access denied shows "Access Denied - Card #..."
- [ ] Momentary unlock shows "Momentary Door Unlock" (no card #)

✅ **Auto-Sync**
- [ ] Events appear within 3 seconds without manual refresh
- [ ] Page doesn't flash when no new events
- [ ] Page updates smoothly when new events arrive

✅ **Data Integrity**
- [ ] No duplicate events in database
- [ ] RawIndex increments sequentially
- [ ] Timestamps are correct
- [ ] Card numbers are accurate
