# Door Count Configuration and Hardware Initialization

## Overview
The GFC system now properly manages door counts for N3000 controllers, automatically disabling unused doors to prevent "ghost data" from appearing in the dashboard.

## When Door Disabling Commands (0x8E Mode 0) Are Sent

### 1. Initial Setup / Configuration
**When:** During the "Deep Reset & Recovery" operation in the Maintenance tab
**Why:** The N3000 stores door settings in non-volatile EEPROM. Once set, the controller remembers the configuration even after power loss.

**How it works:**
1. Navigate to Controllers → Maintenance tab
2. Click "Execute Deep Reset"
3. The system:
   - Clears all privileges (0x10)
   - Resets privilege counters (0x11)
   - Initializes doors 1 to `DoorCount` with Mode 3 (Controlled)
   - Disables doors `DoorCount+1` to 4 with Mode 0 (Not Configured)

**Example for 2-door controller:**
- Door 1: Mode 3 (Controlled), 5s unlock time
- Door 2: Mode 3 (Controlled), 5s unlock time
- Door 3: Mode 0 (Disabled)
- Door 4: Mode 0 (Disabled)

### 2. After Hardware Reset
**When:** Immediately after a physical hardware reset or firmware update
**Why:** A reset wipes the door configuration and may default all 4 ports back to Mode 3

**Action Required:**
Run "Deep Reset & Recovery" again to re-apply the door count configuration.

### 3. During Manual Door Configuration
**When:** In the Doors tab, when you manually set a door's Control Mode to "Not Configured (Disabled)"
**Why:** Allows you to manually disable specific doors without running a full reset

**How it works:**
1. Navigate to Controllers → Doors tab
2. Select a door (e.g., Door 3)
3. Set Control Mode to "Not Configured (Disabled)"
4. Click "Save Changes"
5. The system sends 0x8E command with Mode 0 to that specific door

## What "Update Identity" Does (Network Tab)

**When you click "Update Identity":**
- ✅ Updates Controller Name in database
- ✅ Updates Serial Number in database
- ✅ Updates Door Count in database
- ✅ Preserves IP Address (never changes it)
- ❌ Does NOT send any hardware commands
- ❌ Does NOT disable doors in hardware

**Why:** The door count is stored in the database for UI purposes (showing correct number of slots), but the hardware configuration is only changed during explicit operations like Deep Reset.

## Door Count Behavior

### Database (Software)
- Stored in `Controllers.DoorCount` column
- Controls how many door slots appear in the UI
- Controls relay port dropdown options (1-2 for 2-door, 1-4 for 4-door)
- Controls which doors show status in Controller Info tab

### Hardware (Controller EEPROM)
- Stored in controller's non-volatile memory
- Set via 0x8E commands during Deep Reset
- Persists across power cycles
- Only changes when explicitly commanded

## Verification

After running Deep Reset on a 2-door controller:

1. **Check Controller Info Tab:**
   - Doors 1-2 should show "Controlled" mode
   - Doors 3-4 should show "Not Configured" mode

2. **Check Doors Tab:**
   - Only 2 door slots should be visible
   - Relay port dropdown should only show "Port 1" and "Port 2"

3. **Run PowerShell Verification (Optional):**
   ```powershell
   .\docs\Scripts\VerifyDoorStatus.ps1
   ```
   Expected output:
   - Door 1: Mode 3 (Controlled)
   - Door 2: Mode 3 (Controlled)
   - Door 3: Mode 0 (Not Configured)
   - Door 4: Mode 0 (Not Configured)

## Technical Details

### Commands Used
- **0x10**: Reset Privileges (Clear All)
- **0x11**: Reset Privilege Index
- **0x8E**: Set Door Config
  - Mode 0: Not Configured/Disabled
  - Mode 3: Controlled (Normal operation)

### Packet Structure (0x8E)
```
Byte 0: 0x17 (Command packet type)
Byte 1: 0x8E (Set Door Config)
Byte 2-3: CRC-16 IBM (auto-calculated)
Byte 4-7: Serial Number (Little Endian)
Byte 8: Door Index (1-4)
Byte 9: Control Mode (0=Disabled, 3=Controlled)
Byte 10: Relay Delay (seconds)
Byte 11: Sensor Type
Byte 14: Interlock Group
```

### Storage Location
- **EEPROM**: Controller's non-volatile memory
- **Persistence**: Survives power loss and reboots
- **Reset**: Only cleared by hardware reset or 0x10 command

## Best Practices

1. **Set Door Count First**: Configure the door count in Network → Hardware Identity before running Deep Reset
2. **Run Deep Reset Once**: After initial setup, you don't need to run it again unless you do a hardware reset
3. **Don't Change Door Count Frequently**: The door count should match your physical hardware and rarely change
4. **Verify After Reset**: Always check Controller Info tab after Deep Reset to confirm doors are configured correctly

## Troubleshooting

**Problem:** Doors 3-4 still show data after setting door count to 2
**Solution:** Run "Deep Reset & Recovery" in the Maintenance tab

**Problem:** All 4 doors show as "Controlled" after hardware reset
**Solution:** This is expected. Run "Deep Reset & Recovery" to re-apply the door count configuration

**Problem:** Door count changed but UI still shows 4 doors
**Solution:** Refresh the page. The UI reads door count from the database on page load
