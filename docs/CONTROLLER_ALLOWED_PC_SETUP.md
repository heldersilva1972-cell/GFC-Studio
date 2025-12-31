# Controller "Allowed PC" Configuration Guide

## Problem
The WG3000 controller has a security feature that only accepts commands from specific "Allowed PC" IP addresses. Your PC uses DHCP (automatic IP), so the IP can change, causing the controller to reject commands.

## Solution
Configure the controller to accept commands from ANY PC on your network.

## Steps Using N3000.exe Software

### 1. Launch N3000.exe
Located at: `ACTUAL CONTROLLER SOFTWARE\AccessControl.en\N3000.exe`

### 2. Connect to Controller
- Click "Search" or "Device" menu → "Search Device"
- The controller should appear: IP 192.168.1.72, SN 223213880
- Double-click to connect

### 3. Navigate to Network Settings
Look for one of these menu options:
- "Device" → "Network Settings"
- "System" → "Communication Settings"  
- "Tools" → "Controller Configuration"

### 4. Find "Allowed PC" or "PC IP Filter" Section
This might be called:
- "Allowed PC IP"
- "PC IP Filter"
- "Communication Security"
- "Authorized PCs"

### 5. Configure Allowed IPs

**Option A: Allow All PCs (Recommended for local network)**
- Set to: `0.0.0.0` or `255.255.255.255`
- Or look for a checkbox: "Allow all PCs" or "Disable IP filtering"

**Option B: Allow Your Subnet**
- Add: `192.168.1.0` with subnet mask `255.255.255.0`
- This allows any PC on your local network

**Option C: Add Specific IPs**
- Find your current IP: Open Command Prompt, type `ipconfig`
- Add your current IP (e.g., 192.168.1.77)
- Also add a few more IPs in case DHCP changes it (192.168.1.75, 192.168.1.76, 192.168.1.78, etc.)

### 6. Save Configuration
- Click "Save" or "Write to Controller"
- Wait for confirmation
- The controller may reboot

### 7. Test Connection
- Close N3000.exe
- Run the test script: `test_controller_simple.ps1`
- You should now get a response!

## Alternative: Check Current IP
If you want to see what IP the controller is currently configured to accept:
1. In N3000.exe, go to the same "Allowed PC" section
2. It should show the currently configured IP (probably 192.168.1.17)
3. This confirms why it's not responding to your current PC

## After Configuration
Once the controller is configured to accept your PC:
1. The test script should work
2. The GFC web application should show "CONNECTED"
3. All controller features will work properly

## Troubleshooting
If you can't find the "Allowed PC" setting:
- Check the manual: `ACTUAL CONTROLLER SOFTWARE\AccessControl.en\1.SoftwareManual.pdf`
- Look for "Communication Password" - this might also need to be blank or set correctly
- Try the "Advanced" or "Expert" mode in the software

## Important Notes
- The controller WILL respond to discovery/search commands even with IP filtering enabled
- But it will IGNORE all other commands (GetRunStatus, OpenDoor, etc.) from non-allowed IPs
- This is why Wireshark showed the controller was online but not responding to our commands
