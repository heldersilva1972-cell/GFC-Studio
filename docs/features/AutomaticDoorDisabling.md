# Automatic Door Disabling Feature

## Overview
When you change the Door Count in the Hardware Identity section and click "Update Identity", the system now automatically disables unused doors in the controller hardware.

## What Happens Step-by-Step

### Example: Changing from 4 doors to 2 doors

1. **User Action:**
   - Navigate to Controllers → Network Settings
   - Change "Door Count" dropdown to "2 Doors"
   - Click "Update Identity"

2. **Database Update:**
   - Controller Name is saved
   - Serial Number is saved
   - **DoorCount = 2** is saved to the database
   - IP Address is PRESERVED (not changed)

3. **Hardware Commands Sent:**
   The system automatically sends Function 0x8E commands to disable doors 3 and 4:

   **For Door 3:**
   ```
   Packet Type: 0x17 (Command)
   Command Code: 0x8E (Set Door Config)
   Door Index: 3
   Control Mode: 0 (Not Configured/Disabled)
   Relay Delay: 0
   Sensor Type: 0
   Interlock: 0
   ```

   **For Door 4:**
   ```
   Packet Type: 0x17 (Command)
   Command Code: 0x8E (Set Door Config)
   Door Index: 4
   Control Mode: 0 (Not Configured/Disabled)
   Relay Delay: 0
   Sensor Type: 0
   Interlock: 0
   ```

4. **Result:**
   - Doors 3 and 4 are disabled in the controller's firmware
   - The UI only shows 2 door slots
   - The Controller Info tab only displays status for doors 1 and 2
   - Doors 3 and 4 show as "Not Configured" if queried
   - No more "ghost data" from uninitialized RAM

## Verification

After updating the door count, you can verify the change:

1. **In the UI:**
   - Go to Controllers → Doors tab
   - You should only see slots for doors 1 and 2
   - The relay port dropdown should only show "Port 1" and "Port 2"

2. **In Controller Info tab:**
   - Only doors 1 and 2 should show status
   - Doors 3 and 4 should show as "Not Configured" or be hidden

3. **Via PowerShell (Optional):**
   Run the `VerifyDoorStatus.ps1` script to query the controller:
   ```powershell
   .\docs\Scripts\VerifyDoorStatus.ps1
   ```
   Doors 3 and 4 should report Mode 0 (Not Configured)

## Safety Features

- **IP Address Protection:** The IP address is NEVER changed when updating identity
- **Controller Must Be Online:** Hardware commands are only sent if `Controller.IsEnabled` is true
- **Error Handling:** If hardware commands fail, the database is still updated and you get a clear error message
- **100ms Delay:** Small delay between commands to prevent overwhelming the controller

## Supported Door Counts

- **1 Door:** Disables doors 2, 3, and 4
- **2 Doors:** Disables doors 3 and 4
- **4 Doors:** No doors are disabled (default)

## Technical Details

- **Command:** Function 0x8E (Set Door Configuration)
- **Mode 0:** Not Configured/Disabled
- **Packet Size:** 64 bytes (standard N3000 UDP packet)
- **CRC:** Automatically calculated by `WgPacketBuilder`
- **Protocol:** UDP to controller IP on port 60000
