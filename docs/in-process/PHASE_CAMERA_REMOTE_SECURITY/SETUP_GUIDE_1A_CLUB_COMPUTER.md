# SETUP GUIDE 1A: CLUB COMPUTER CONFIGURATION
## Complete Step-by-Step Guide for the Computer at Club Location

**Purpose:** This guide will walk you through setting up the Windows computer at your club that connects to the cameras and runs the Video Agent.

**Time Required:** 60-90 minutes  
**Skill Level:** Beginner-friendly (copy/paste commands provided)  
**Prerequisites:** Windows 10 or Windows 11 computer with admin access at the club

---

## 📋 WHAT YOU'LL INSTALL ON THIS COMPUTER

1. **WireGuard** - VPN server to allow your home computer to connect
2. **Video Agent** - Service that streams camera footage (already installed)
3. **Windows Firewall Rules** - To allow VPN traffic

---

## ⚠️ IMPORTANT: UNDERSTAND YOUR SETUP

You have **TWO computers** in this system:

```
┌─────────────────────────────────────────┐
│  COMPUTER A (This Guide)                │
│  Location: At the club                  │
│  Purpose: Connect to cameras            │
│  Runs: Video Agent + WireGuard Server   │
│  Must be: Always on                     │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│  COMPUTER B (Next Guide)                │
│  Location: Your home                    │
│  Purpose: Host the Web App              │
│  Runs: Web App + WireGuard Client       │
│  Must be: Always on                     │
└─────────────────────────────────────────┘

These two computers will connect via an encrypted VPN tunnel
so the Web App can talk to the Video Agent securely.
```

---

## PART 1: VERIFY COMPUTER REQUIREMENTS

### Step 1.1: Check Windows Version
1. Press `Windows Key + R`
2. Type: `winver`
3. Press Enter
4. **Verify:** You see "Windows 10" or "Windows 11"
   - ✅ If yes, continue
   - ❌ If no, you need to upgrade Windows first

### Step 1.2: Check if You're an Administrator
1. Press `Windows Key + X`
2. Click "Windows PowerShell (Admin)" or "Terminal (Admin)"
3. If you see a blue/black window, you're good!
   - ✅ Continue to next step
   - ❌ If you see "Access Denied", contact your IT person

### Step 1.3: Check Your Local IP Address
1. In the PowerShell window, type:
   ```powershell
   ipconfig
   ```
2. Press Enter
3. Look for "IPv4 Address" under your network adapter
4. **Write this down:** It will look like `192.168.1.100`
   - This is your **Club LAN IP Address**
   - Example: `192.168.1.100`

**Write down your club LAN IP:**
```
Club Computer LAN IP: _________________
```

### Step 1.4: Identify Your Network Range
From the IP address above, determine your network range:
- If your IP is `192.168.1.100`, your range is `192.168.1.0/24`
- If your IP is `192.168.0.50`, your range is `192.168.0.0/24`
- If your IP is `10.0.0.25`, your range is `10.0.0.0/24`

**Write down your club network range:**
```
Club Network Range: _________________
```

---

## PART 2: INSTALL WIREGUARD

### Step 2.1: Download WireGuard
1. Open your web browser
2. Go to: `https://www.wireguard.com/install/`
3. Click **"Windows 10/11"**
4. Click the blue **"Download Windows Installer"** button
5. Save the file (it will be named `wireguard-installer.exe`)

### Step 2.2: Install WireGuard
1. Go to your Downloads folder
2. **Right-click** on `wireguard-installer.exe`
3. Click **"Run as administrator"**
4. Click **"Yes"** when Windows asks for permission
5. Click **"Next"** in the installer
6. Click **"Install"**
7. Wait for installation to complete (about 30 seconds)
8. Click **"Finish"**

### Step 2.3: Verify WireGuard is Installed
1. Look in your system tray (bottom-right corner, near the clock)
2. You should see a new icon that looks like a tunnel or circle
3. **Right-click** the WireGuard icon
4. You should see a menu with options
   - ✅ If you see the menu, installation successful!
   - ❌ If you don't see the icon, restart your computer and check again

---

## PART 3: CREATE WIREGUARD SERVER CONFIGURATION

### Step 3.1: Generate Server Keys
1. Click the **Start Menu**
2. Type: `cmd`
3. **Right-click** "Command Prompt"
4. Click **"Run as administrator"**
5. Type the following command **exactly** (copy/paste recommended):
   ```cmd
   cd "C:\Program Files\WireGuard"
   ```
6. Press Enter
7. Now type:
   ```cmd
   wg genkey > club_server_private.key
   ```
8. Press Enter
9. Type:
   ```cmd
   type club_server_private.key | wg pubkey > club_server_public.key
   ```
10. Press Enter

### Step 3.2: View Your Server Keys
1. In the same Command Prompt, type:
   ```cmd
   type club_server_private.key
   ```
2. Press Enter
3. **Copy the output** (it will be a long string of letters/numbers)
4. **Paste it here and save it:**
   ```
   Club Server Private Key: _________________________________
   ```

5. Now type:
   ```cmd
   type club_server_public.key
   ```
6. Press Enter
7. **Copy the output**
8. **Paste it here and save it:**
   ```
   Club Server Public Key: _________________________________
   ```

⚠️ **IMPORTANT:** Keep these keys safe! The private key is like a password.

### Step 3.3: Create Server Configuration File
1. Click the **WireGuard icon** in your system tray
2. Click **"Add Tunnel"**
3. Click **"Add empty tunnel..."**
4. You'll see a text editor window
5. **Delete everything** in the window
6. **Copy and paste** the following (we'll customize it next):

```ini
[Interface]
PrivateKey = YOUR_CLUB_SERVER_PRIVATE_KEY_HERE
Address = 10.8.0.1/24
ListenPort = 51820
SaveConfig = false

# Peer: Home Computer (will be added in Guide 1B)
# This section will be filled in after you set up the home computer
```

7. **Replace** `YOUR_CLUB_SERVER_PRIVATE_KEY_HERE` with your actual Club Server Private Key from Step 3.2

8. At the top of the window, change the name from "Untitled" to: `GFC-Club-VPN-Server`

9. Click **"Save"**

### Step 3.4: Activate the VPN Server
1. In the WireGuard window, you should see "GFC-Club-VPN-Server" in the list
2. Click the **"Activate"** button
3. The status should change to **"Active"**
   - ✅ If it says "Active", success!
   - ❌ If you see an error, see Troubleshooting section below

---

## PART 4: CONFIGURE WINDOWS FIREWALL

### Step 4.1: Allow WireGuard Through Firewall
1. Press `Windows Key + R`
2. Type: `firewall.cpl`
3. Press Enter
4. Click **"Allow an app or feature through Windows Defender Firewall"** (left side)
5. Click **"Change settings"** button (top)
6. Click **"Allow another app..."** button (bottom)
7. Click **"Browse..."**
8. Navigate to: `C:\Program Files\WireGuard\wireguard.exe`
9. Click **"Open"**
10. Click **"Add"**
11. **Check both boxes** next to "WireGuard" (Private and Public)
12. Click **"OK"**

### Step 4.2: Create Inbound Rule for VPN Port
1. Press `Windows Key + R`
2. Type: `wf.msc`
3. Press Enter (this opens Windows Defender Firewall with Advanced Security)
4. Click **"Inbound Rules"** in the left panel
5. Click **"New Rule..."** in the right panel
6. Select **"Port"**
7. Click **"Next"**
8. Select **"UDP"**
9. In "Specific local ports", type: `51820`
10. Click **"Next"**
11. Select **"Allow the connection"**
12. Click **"Next"**
13. **Check all three boxes:** Domain, Private, Public
14. Click **"Next"**
15. Name: `GFC Club VPN Server - WireGuard`
16. Description: `Allows incoming VPN connections from home computer`
17. Click **"Finish"**

---

## PART 5: CONFIGURE ROUTER PORT FORWARDING

⚠️ **IMPORTANT:** This step requires access to your club's router admin panel.

### Step 5.1: Find Your Router's IP Address
1. Open Command Prompt (as admin)
2. Type:
   ```cmd
   ipconfig
   ```
3. Look for **"Default Gateway"** under your network adapter
4. **Write it down:** It will look like `192.168.1.1` or `192.168.0.1`
   ```
   Club Router IP: _________________
   ```

### Step 5.2: Access Router Admin Panel
1. Open your web browser
2. In the address bar, type your Router IP (from above)
3. Press Enter
4. You'll see a login page
5. Enter your router username/password
   - Common defaults: `admin` / `admin` or `admin` / `password`
   - If you don't know it, check the sticker on your router

### Step 5.3: Create Port Forward Rule
⚠️ **Note:** Every router brand has a different interface. Look for these menu names:
- "Port Forwarding"
- "Virtual Server"
- "NAT Forwarding"
- "Applications & Gaming"

**General Steps:**
1. Find the Port Forwarding section
2. Click **"Add New"** or **"Create Rule"**
3. Fill in these details:
   - **Service Name:** `GFC-Club-VPN`
   - **External Port:** `51820`
   - **Internal Port:** `51820`
   - **Protocol:** `UDP`
   - **Internal IP:** Your club computer's LAN IP (from Part 1, Step 1.3)
   - **Enabled:** `Yes` or `On`
4. Click **"Save"** or **"Apply"**

### Step 5.4: Verify Port Forward
1. Go to: `https://www.yougetsignal.com/tools/open-ports/`
2. In "Remote Address", leave it as "your IP"
3. In "Port Number", type: `51820`
4. Click **"Check"**
5. **Result:**
   - ✅ "Port 51820 is open" = Success!
   - ❌ "Port 51820 is closed" = Double-check your port forward settings

---

## PART 6: FIND YOUR CLUB'S PUBLIC IP ADDRESS

### Step 6.1: Get Your Current Public IP
1. Go to: `https://www.whatismyip.com/`
2. You'll see a number like `123.45.67.89`
3. **Write it down:**
   ```
   Club Public IP: _________________
   ```

⚠️ **IMPORTANT:** You'll need this IP when setting up the home computer (Guide 1B).

---

## PART 7: CONFIGURE VIDEO AGENT SECURITY

### Step 7.1: Update Video Agent to Only Accept Secure Connections
1. Navigate to your Video Agent folder:
   ```
   C:\Users\[YourName]\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\services\GFC.VideoAgent
   ```
2. Find the file: `appsettings.json`
3. **Right-click** it and open with **Notepad**
4. Find the section that says `"AllowedHosts"` or similar
5. Add this configuration:
   ```json
   "AllowedNetworks": [
     "192.168.1.0/24",  // Club LAN (adjust to your network)
     "10.8.0.0/24"      // VPN subnet (home computer will use this)
   ]
   ```
6. Save the file
7. Restart the Video Agent service

---

## PART 8: VERIFY EVERYTHING IS WORKING

### Step 8.1: Check WireGuard Status
1. Click the WireGuard icon in system tray
2. Verify "GFC-Club-VPN-Server" shows **"Active"**
3. Click on "GFC-Club-VPN-Server" to see details
4. You should see:
   - Interface: `10.8.0.1`
   - Listening port: `51820`
   - Peers: `0` (this is normal - home computer will appear here after Guide 1B)

### Step 8.2: Check Windows Services
1. Press `Windows Key + R`
2. Type: `services.msc`
3. Press Enter
4. Scroll down to find **"WireGuardTunnel$GFC-Club-VPN-Server"**
5. Verify **Status** shows **"Running"**
6. Verify **Startup Type** shows **"Automatic"**
   - ✅ If both are correct, you're good!
   - ❌ If not running:
     - Right-click the service
     - Click "Start"

### Step 8.3: Verify Computer Stays On
1. Press `Windows Key + R`
2. Type: `powercfg.cpl`
3. Press Enter
4. Set **"Put the computer to sleep"** to **"Never"**
5. Set **"Turn off the display"** to your preference (e.g., 30 minutes)
6. Click **"Save changes"**

⚠️ **CRITICAL:** This computer must stay on 24/7 for remote video access to work!

---

## PART 9: DOCUMENT YOUR CONFIGURATION

Fill in this information sheet and keep it safe:

```
=================================================================
GFC CAMERA SYSTEM - CLUB COMPUTER CONFIGURATION SHEET
=================================================================

COMPUTER INFORMATION:
- Computer Name: _______________________
- Windows Version: _____________________
- Club LAN IP Address: _________________
- Club Network Range: __________________

WIREGUARD CONFIGURATION:
- Server Private Key: __________________________________
- Server Public Key: ___________________________________
- VPN Interface IP: 10.8.0.1
- VPN Subnet: 10.8.0.0/24
- Listen Port: 51820

NETWORK INFORMATION:
- Club Router IP: ______________________
- Club Public IP: ______________________
- Port Forward Rule: UDP 51820 → [Club LAN IP]:51820

VIDEO AGENT:
- Installation Path: ___________________________________
- Service Status: [ ] Running  [ ] Stopped
- Allowed Networks: 
  • Club LAN: _________________
  • VPN: 10.8.0.0/24

FIREWALL RULES:
- [✓] WireGuard allowed through Windows Firewall
- [✓] Inbound rule created for UDP 51820
- [✓] Router port forward configured

POWER SETTINGS:
- [✓] Computer set to never sleep
- [✓] Service set to start automatically

NEXT STEPS:
- [ ] Complete Setup Guide 1B (Home Computer)
- [ ] Complete Setup Guide 3 (Connect the two computers)
- [ ] Test VPN tunnel connection

=================================================================
```

---

## 🔧 TROUBLESHOOTING

### Problem: WireGuard won't activate
**Solution:**
1. Check if another VPN is running (Cisco, OpenVPN, etc.)
2. Disable other VPNs
3. Restart WireGuard
4. If still failing, check Windows Event Viewer for errors

### Problem: Port forward test shows "closed"
**Solution:**
1. Verify your computer's LAN IP hasn't changed
2. Check router port forward rule is enabled
3. Try restarting your router
4. Some ISPs block VPN ports - contact your ISP if issue persists

### Problem: Can't access router admin panel
**Solution:**
1. Verify router IP is correct (check Default Gateway)
2. Try common IPs: `192.168.1.1`, `192.168.0.1`, `10.0.0.1`
3. Check router manual or sticker for default login
4. Reset router to factory settings (last resort)

### Problem: Computer goes to sleep
**Solution:**
1. Open Power Options (powercfg.cpl)
2. Set sleep to "Never"
3. Check BIOS settings if still sleeping
4. Disable "Fast Startup" in Power Options

---

## ✅ COMPLETION CHECKLIST

Before moving to the next guide, verify:

- [ ] WireGuard is installed
- [ ] Server keys are generated and saved
- [ ] Server configuration file created and activated
- [ ] Windows Firewall rules configured
- [ ] Router port forwarding configured
- [ ] Port forward test shows "open"
- [ ] Club public IP address documented
- [ ] Video Agent configured for secure access
- [ ] Computer set to never sleep
- [ ] Configuration sheet filled out and saved

**If all boxes are checked, you're ready for Setup Guide 1B: Home Computer!**

---

## 📞 NEED HELP?

If you get stuck:
1. Take a screenshot of the error
2. Note which step you're on
3. Check the Troubleshooting section above
4. Contact your system administrator

---

**Next Guide:** [SETUP_GUIDE_1B_HOME_COMPUTER.md](./SETUP_GUIDE_1B_HOME_COMPUTER.md)

---

**This computer is now configured as the VPN server. Next, you'll set up your home computer to connect to it.**
