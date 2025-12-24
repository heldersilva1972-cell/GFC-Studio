# SETUP GUIDE 1B: HOME COMPUTER CONFIGURATION
## Complete Step-by-Step Guide for Your Home Computer

**Purpose:** This guide will walk you through setting up your home Windows computer that will host the GFC Web App and connect to the club computer.

**Time Required:** 45-60 minutes  
**Skill Level:** Beginner-friendly (copy/paste commands provided)  
**Prerequisites:** 
- Completed Setup Guide 1A (Club Computer)
- Windows 10 or Windows 11 computer with admin access
- Club server public key and public IP from Guide 1A

---

## 📋 WHAT YOU'LL INSTALL ON THIS COMPUTER

1. **WireGuard** - VPN client to connect to the club computer
2. **GFC Web App** - The main application (already installed)
3. **Cloudflare Tunnel** - To hide your home IP (covered in Guide 2)

---

## ⚠️ IMPORTANT: YOUR ROLE IN THE SYSTEM

```
┌─────────────────────────────────────────┐
│  COMPUTER A (Club - Already Set Up)    │
│  • WireGuard Server                     │
│  • Video Agent                          │
│  • VPN IP: 10.8.0.1                     │
└─────────────────────────────────────────┘
                    ↑
                    │ VPN Tunnel
                    │ (You're setting this up now)
                    ↓
┌─────────────────────────────────────────┐
│  COMPUTER B (Home - This Guide)         │
│  • WireGuard Client                     │
│  • GFC Web App                          │
│  • VPN IP: 10.8.0.2                     │
└─────────────────────────────────────────┘
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

### Step 1.3: Gather Information from Guide 1A
You'll need these from the club computer setup:

```
From Guide 1A Configuration Sheet:
- Club Server Public Key: _________________________________
- Club Public IP: _________________
- Club VPN Interface IP: 10.8.0.1 (standard)
```

⚠️ **STOP:** If you don't have this information, go back to Guide 1A and complete it first!

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

## PART 3: CREATE WIREGUARD CLIENT CONFIGURATION

### Step 3.1: Generate Client Keys
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
   wg genkey > home_client_private.key
   ```
8. Press Enter
9. Type:
   ```cmd
   type home_client_private.key | wg pubkey > home_client_public.key
   ```
10. Press Enter

### Step 3.2: View Your Client Keys
1. In the same Command Prompt, type:
   ```cmd
   type home_client_private.key
   ```
2. Press Enter
3. **Copy the output** (it will be a long string of letters/numbers)
4. **Paste it here and save it:**
   ```
   Home Client Private Key: _________________________________
   ```

5. Now type:
   ```cmd
   type home_client_public.key
   ```
6. Press Enter
7. **Copy the output**
8. **Paste it here and save it:**
   ```
   Home Client Public Key: _________________________________
   ```

⚠️ **IMPORTANT:** You'll need the Home Client Public Key in the next step!

### Step 3.3: Create Client Configuration File
1. Click the **WireGuard icon** in your system tray
2. Click **"Add Tunnel"**
3. Click **"Add empty tunnel..."**
4. You'll see a text editor window
5. **Delete everything** in the window
6. **Copy and paste** the following template:

```ini
[Interface]
PrivateKey = YOUR_HOME_CLIENT_PRIVATE_KEY_HERE
Address = 10.8.0.2/32
DNS = 1.1.1.1

[Peer]
PublicKey = YOUR_CLUB_SERVER_PUBLIC_KEY_HERE
Endpoint = YOUR_CLUB_PUBLIC_IP_HERE:51820
AllowedIPs = 10.8.0.0/24
PersistentKeepalive = 25
```

7. **Replace the placeholders:**
   - `YOUR_HOME_CLIENT_PRIVATE_KEY_HERE` → Your Home Client Private Key (from Step 3.2)
   - `YOUR_CLUB_SERVER_PUBLIC_KEY_HERE` → Club Server Public Key (from Guide 1A)
   - `YOUR_CLUB_PUBLIC_IP_HERE` → Club Public IP (from Guide 1A)

8. **Example of completed config:**
```ini
[Interface]
PrivateKey = aBcDeFgHiJkLmNoPqRsTuVwXyZ1234567890ABCD=
Address = 10.8.0.2/32
DNS = 1.1.1.1

[Peer]
PublicKey = XyZ9876543210ZyXwVuTsRqPoNmLkJiHgFeDcBa=
Endpoint = 123.45.67.89:51820
AllowedIPs = 10.8.0.0/24
PersistentKeepalive = 25
```

9. At the top of the window, change the name from "Untitled" to: `GFC-Home-to-Club-VPN`

10. Click **"Save"**

---

## PART 4: ADD HOME COMPUTER AS PEER ON CLUB SERVER

⚠️ **IMPORTANT:** You need to do this step **on the club computer** (remotely or in person).

### Step 4.1: Update Club Server Configuration
1. On the **club computer**, click the WireGuard icon
2. Right-click **"GFC-Club-VPN-Server"**
3. Click **"Edit"**
4. At the bottom of the file, add this section:

```ini
# Peer: Home Computer
[Peer]
PublicKey = YOUR_HOME_CLIENT_PUBLIC_KEY_HERE
AllowedIPs = 10.8.0.2/32
```

5. **Replace** `YOUR_HOME_CLIENT_PUBLIC_KEY_HERE` with your Home Client Public Key (from Step 3.2 of this guide)

6. **Example:**
```ini
[Interface]
PrivateKey = (existing club server key)
Address = 10.8.0.1/24
ListenPort = 51820
SaveConfig = false

# Peer: Home Computer
[Peer]
PublicKey = aBcDeFgHiJkLmNoPqRsTuVwXyZ1234567890ABCD=
AllowedIPs = 10.8.0.2/32
```

7. Click **"Save"**
8. Click **"Deactivate"** then **"Activate"** to reload the configuration

---

## PART 5: TEST THE VPN CONNECTION

### Step 5.1: Activate the VPN on Home Computer
1. On your **home computer**, click the WireGuard icon
2. Find **"GFC-Home-to-Club-VPN"** in the list
3. Click **"Activate"**
4. The status should change to **"Active"**
   - ✅ If it says "Active", continue to next step
   - ❌ If you see an error, see Troubleshooting section below

### Step 5.2: Verify Connection
1. Click on **"GFC-Home-to-Club-VPN"** to see details
2. You should see:
   - **Interface:** `10.8.0.2`
   - **Status:** Active
   - **Peer:** Shows the club server public key
   - **Latest handshake:** Should show a recent time (e.g., "5 seconds ago")
   - **Transfer:** Should show some data (RX/TX)

✅ **If you see "Latest handshake" with a recent time, the tunnel is working!**

### Step 5.3: Test Connectivity to Club Computer
1. Open Command Prompt (as admin)
2. Type:
   ```cmd
   ping 10.8.0.1
   ```
3. Press Enter
4. **Expected result:**
   ```
   Reply from 10.8.0.1: bytes=32 time=12ms TTL=64
   Reply from 10.8.0.1: bytes=32 time=11ms TTL=64
   Reply from 10.8.0.1: bytes=32 time=13ms TTL=64
   ```
   - ✅ If you see "Reply from 10.8.0.1", the VPN is working perfectly!
   - ❌ If you see "Request timed out", see Troubleshooting section

---

## PART 6: CONFIGURE GFC WEB APP

### Step 6.1: Update Web App Configuration
1. Navigate to your GFC Web App folder:
   ```
   C:\Users\[YourName]\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer
   ```
2. Find the file: `appsettings.json`
3. **Right-click** it and open with **Notepad**
4. Find the section that says `"VideoAgent"`
5. Update the `BaseUrl` to use the club computer's VPN IP:
   ```json
   "VideoAgent": {
     "BaseUrl": "http://10.8.0.1:5001",
     "HealthCheckIntervalSeconds": 30
   }
   ```
6. Save the file

### Step 6.2: Test Web App Connection to Video Agent
1. Make sure the VPN is **Active**
2. Open your web browser
3. Go to: `http://10.8.0.1:5001/health`
4. **Expected result:** You should see a JSON response like:
   ```json
   {
     "status": "Healthy",
     "timestamp": "2025-12-24T01:30:00Z"
   }
   ```
   - ✅ If you see this, the Web App can reach the Video Agent!
   - ❌ If you see an error, verify the Video Agent is running on the club computer

---

## PART 7: CONFIGURE AUTO-START AND AUTO-RECONNECT

### Step 7.1: Set VPN to Start Automatically
1. Press `Windows Key + R`
2. Type: `services.msc`
3. Press Enter
4. Scroll down to find **"WireGuardTunnel$GFC-Home-to-Club-VPN"**
5. **Right-click** it and select **"Properties"**
6. Change **"Startup type"** to **"Automatic"**
7. Click **"Apply"**
8. Click **"Start"** if it's not already running
9. Click **"OK"**

### Step 7.2: Verify Computer Stays On
1. Press `Windows Key + R`
2. Type: `powercfg.cpl`
3. Press Enter
4. Set **"Put the computer to sleep"** to **"Never"**
5. Set **"Turn off the display"** to your preference (e.g., 30 minutes)
6. Click **"Save changes"**

⚠️ **CRITICAL:** This computer must stay on 24/7 for the Web App to be accessible!

---

## PART 8: VERIFY EVERYTHING IS WORKING

### Step 8.1: Check VPN Status
1. Click the WireGuard icon in system tray
2. Verify "GFC-Home-to-Club-VPN" shows **"Active"**
3. Click on it to see details
4. Verify "Latest handshake" shows a recent time

### Step 8.2: Check Web App Can Reach Video Agent
1. Open Command Prompt
2. Type:
   ```cmd
   curl http://10.8.0.1:5001/health
   ```
3. You should see the health check JSON response

### Step 8.3: Test Full System
1. Start the GFC Web App (if not already running)
2. Open browser and go to `http://localhost:5000`
3. Log in
4. Navigate to the Video/Camera page
5. ✅ If cameras load, everything is working!

---

## PART 9: DOCUMENT YOUR CONFIGURATION

Fill in this information sheet and keep it safe:

```
=================================================================
GFC CAMERA SYSTEM - HOME COMPUTER CONFIGURATION SHEET
=================================================================

COMPUTER INFORMATION:
- Computer Name: _______________________
- Windows Version: _____________________
- Home LAN IP Address: _________________

WIREGUARD CONFIGURATION:
- Client Private Key: __________________________________
- Client Public Key: ___________________________________
- VPN Interface IP: 10.8.0.2
- Club Server Public Key: ______________________________
- Club Public IP: ______________________
- VPN Status: [ ] Active  [ ] Inactive

CONNECTION TEST:
- Ping to 10.8.0.1: [ ] Success  [ ] Failed
- Video Agent Health Check: [ ] Success  [ ] Failed
- Latest Handshake: _____________ (should be recent)

GFC WEB APP:
- Installation Path: ___________________________________
- VideoAgent BaseUrl: http://10.8.0.1:5001
- Service Status: [ ] Running  [ ] Stopped

SERVICE CONFIGURATION:
- [✓] VPN service set to start automatically
- [✓] Computer set to never sleep
- [✓] Web App configured to use VPN IP

NEXT STEPS:
- [ ] Complete Setup Guide 2 (Cloudflare Tunnel)
- [ ] Test remote access from external network
- [ ] Configure monitoring and alerts

=================================================================
```

---

## 🔧 TROUBLESHOOTING

### Problem: VPN won't activate
**Solution:**
1. Verify you entered the club server public key correctly
2. Verify you entered the club public IP correctly
3. Check if club computer's VPN is active
4. Check if port 51820 is open on club router

### Problem: "Latest handshake" never appears
**Solution:**
1. Verify club computer's firewall allows UDP 51820
2. Verify club router port forwarding is configured
3. Verify club computer's VPN is active
4. Try restarting both VPN services

### Problem: Ping to 10.8.0.1 times out
**Solution:**
1. Verify VPN shows "Active" on both computers
2. Check "Latest handshake" is recent (< 1 minute)
3. Verify club computer added your public key as a peer
4. Check Windows Firewall on club computer allows ICMP

### Problem: Can't reach Video Agent (http://10.8.0.1:5001)
**Solution:**
1. Verify ping to 10.8.0.1 works first
2. Verify Video Agent is running on club computer
3. Verify Video Agent is listening on port 5001
4. Check Video Agent's allowed networks includes 10.8.0.0/24

### Problem: VPN disconnects randomly
**Solution:**
1. Check `PersistentKeepalive = 25` is in your config
2. Verify club computer stays on (check power settings)
3. Check club internet connection is stable
4. Review Windows Event Viewer for errors

---

## ✅ COMPLETION CHECKLIST

Before moving to the next guide, verify:

- [ ] WireGuard is installed on home computer
- [ ] Client keys are generated and saved
- [ ] Client configuration file created and activated
- [ ] Club server updated with home computer as peer
- [ ] VPN connection is active
- [ ] "Latest handshake" shows recent time
- [ ] Ping to 10.8.0.1 succeeds
- [ ] Video Agent health check succeeds
- [ ] GFC Web App configured to use VPN IP
- [ ] VPN service set to auto-start
- [ ] Computer set to never sleep
- [ ] Configuration sheet filled out and saved

**If all boxes are checked, you're ready for Setup Guide 2: Cloudflare Tunnel!**

---

## 📞 NEED HELP?

If you get stuck:
1. Take a screenshot of the error
2. Note which step you're on
3. Check the Troubleshooting section above
4. Verify club computer is on and VPN is active
5. Contact your system administrator

---

**Next Guide:** [SETUP_GUIDE_2_CLOUDFLARE_TUNNEL.md](./SETUP_GUIDE_2_CLOUDFLARE_TUNNEL.md)

---

**Your home computer is now connected to the club computer via encrypted VPN! Next, you'll set up Cloudflare to allow external access.**
