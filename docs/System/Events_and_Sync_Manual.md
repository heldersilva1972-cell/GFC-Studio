# GFC-Studio: Event & Sync System Manual

This document describes the technical architecture and data flow for Controller Events within the GFC system. 

## 1. Hardware Communication (UDP Protocol)
The system communicates with Mengqi/Wiegand-style controllers using UDP. Events are fetched using the `0x20` (or legacy `0xB0`) command.

### Event Packet Structure (64 Bytes)
When the controller returns an event record, the parser (`WgResponseParser.cs`) extracts data based on the following offsets:

| Offset | Size | Description | Value Example |
| :--- | :--- | :--- | :--- |
| **8-11** | 4 Bytes | **Raw Index** (Little Endian) | `97 01 00 00` -> 337 |
| **14** | 1 Byte | **Door/Reader Number** | `01` = Door 1, `02` = Door 2 |
| **15** | 1 Byte | **Event Type** | `01` = Granted, `05` = Denied |
| **16-19** | 4 Bytes | **Card Number** (Little Endian) | `...` |
| **20-26** | 7 Bytes | **Timestamp (BCD Format)** | `20 26 01 10 11 51 26` |

**Timestamp BCD Format**: `CC YY MM DD HH mm ss` (Century, Year, Month, Day, Hour, Minute, Second).

---

## 2. Database Mapping & Relationships
Events are stored in the `ControllerEvents` table. The mapping logic relies on two critical relationships:

### The Door Link
*   **Column `DoorOrReader`**: This is the RAW number from the hardware (1 or 2).
*   **Column `DoorId`**: This is a Foreign Key to the `Doors` table.
*   **Mapping Logic**: During sync, the system looks at the `Doors` table for a record where `ControllerId` matches AND `DoorIndex` matches the hardware's `DoorOrReader` value.

### The Sync Tracker
The table `ControllerLastIndexes` tracks the `LastRecordIndex` for each controller. The system will only request indices *greater* than this value during a refresh.

---

## 3. The Synchronization Process
The `ControllerEventService.SyncFromControllerAsync` method handles the data pull.

1.  **Backlog Handling**: If the hardware index is significantly ahead (e.g., > 2000), the system caps the sync to the most recent 500 events to prevent UI hang.
2.  **Performance**: The sync requests indices sequentially. Current performance is tuned to ~1000 events per pass with minimal 1ms delay.
3.  **Duplicate Prevention**: Before saving a batch, the system checks if a record with the same `RawIndex` already exists for that controller.
4.  **Automatic Sync**: When a "Momentary Unlock" command is sent, the UI triggers a background sync after a **2000ms delay** to allow the hardware time to commit the event to its internal log.

---

## 4. Troubleshooting & Verification

### Specialized Event Labels
*   **Momentary Door Unlock**: Generated when the Unlock button is clicked in the UI. Detected by tracking the RawIndex at the time of the unlock command - any event with an index within 3 of the recorded index is considered a system unlock. Card numbers are hidden for these events.
*   **Access Granted (`0x01`)**: Standard successful card swipe.
*   **Access Denied (`0x05-0x09`)**: Variety of denial reasons (Not Found, Timezone, etc.).

**Note**: The hardware sends identical event type codes (`0x01`) for both card swipes and momentary unlocks, so detection relies on RawIndex tracking rather than event type differentiation.

### How to verify raw hardware data
```sql
SELECT Id, DoorOrReader, RawData FROM ControllerEvents ORDER BY Id DESC;
```

### How to fix Mismatched Door Names
Every door on a single controller MUST have a unique `DoorIndex` matching its physical wiring (1, 2, 3, or 4).
```sql
SELECT Id, Name, DoorIndex FROM Doors;
```

### How to Reset Sync (Clean Slate)
If history is corrupted or a massive backlog is preventing new events from showing:
```sql
DELETE FROM ControllerEvents;
DELETE FROM ControllerLastIndexes;
```
