# Phase 1 Setup - Correct Architecture (Remote Web App)

**Scenario:** GFC Web App is hosted remotely (cloud/external server), needs to access cameras at club  
**Goal:** Set up Video Agent at club + VPN for web app to connect  
**Time Required:** 1-2 hours

---

## 🎯 Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                    REMOTE (Cloud/External Server)               │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  GFC Web App (Blazor Server)                             │  │
│  │  - Hosted remotely (not at club)                         │  │
│  │  - Connects to Video Agent via VPN                       │  │
│  └──────────────────────────────────────────────────────────┘  │
│                             │                                   │
│                             │ WireGuard VPN                     │
│                             │ (Encrypted Tunnel)                │
└─────────────────────────────┼───────────────────────────────────┘
                              │
                              │ INTERNET
                              │
┌─────────────────────────────▼───────────────────────────────────┐
│                      AT THE CLUB (LAN)                          │
│                                                                 │
│  ┌──────────────────┐                                          │
│  │  VPN Server      │ ◄─── Web app connects here              │
│  │  (WireGuard)     │                                          │
│  │  Port 51820      │                                          │
│  └────────┬─────────┘                                          │
│           │                                                     │
│           │ VPN makes web app "inside" club network            │
│           ▼                                                     │
│  ┌──────────────────┐                                          │
│  │  Video Agent     │ ◄─── Web app calls this via VPN         │
│  │  Port 5100       │                                          │
│  │  (Local PC)      │                                          │
│  └────────┬─────────┘                                          │
│           │                                                     │
│           │ RTSP (LAN-only)                                    │
│           ▼                                                     │
│  ┌──────────────────┐                                          │
│  │  NVR             │                                          │
│  │  192.168.1.64    │                                          │
│  │  📹📹📹📹      │                                          │
│  └──────────────────┘                                          │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────┐
│                    USERS (Viewing Cameras)                      │
│                                                                 │
│  User → Web App (remote server) → VPN → Video Agent → NVR      │
│                                                                 │
│  Users DON'T need VPN - they just access the web app normally  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🎯 Key Points

**What's at the club:**
- ✅ Video Agent (Windows service on local PC)
- ✅ WireGuard VPN Server (on same local PC)
- ✅ NVR (192.168.1.64)

**What's remote:**
- ✅ GFC Web App (your existing hosting)
- ✅ Web app has VPN client config (connects to club)

**What users do:**
- ✅ Just access the web app normally (no VPN needed for users!)
- ✅ Web app handles VPN connection to club behind the scenes

---

## 📋 What Gets Installed Where

### **At the Club (Local PC):**
1. ✅ **FFmpeg** - Video converter
2. ✅ **WireGuard VPN Server** - Allows web app to connect
3. ✅ **Video Agent** - Converts camera streams to HLS

### **On Web App Server (Remote):**
1. ✅ **WireGuard VPN Client** - Connects to club VPN
2. ✅ **Updated GFC Web App** - With camera viewer pages

### **On User Devices:**
- ❌ **Nothing!** Users just access web app normally

---

## 🚀 Setup Guide

---

## PART 1: At the Club - Local PC Setup (1 hour)

### Step 1.1: Set Static IP Address

**IMPORTANT:** Use this specific IP for the Video Agent PC: `192.168.1.200`

1. Press `Windows + R`
2. Type: `ncpa.cpl` and press Enter
3. Right-click network adapter → **Properties**
4. Double-click **Internet Protocol Version 4 (TCP/IPv4)**
5. Select **Use the following IP address:**
   ```
   IP address: 192.168.1.200
   Subnet mask: 255.255.255.0
   Default gateway: 192.168.1.1
   DNS: 192.168.1.1
   ```
6. Click **OK**

**Why 192.168.1.200?**
- High enough to avoid DHCP conflicts
- Easy to remember
- Reserved for GFC Camera System
- Room for expansion (.201, .202 for future services)

---

### Step 1.2: Create Firewall Rules

**Open PowerShell as Administrator:**

```powershell
# Allow VPN port (for web app to connect)
New-NetFirewallRule -DisplayName "WireGuard VPN" -Direction Inbound -Protocol UDP -LocalPort 51820 -Action Allow

# Allow Video Agent (for web app to call via VPN)
New-NetFirewallRule -DisplayName "GFC Video Agent" -Direction Inbound -Protocol TCP -LocalPort 5100 -Action Allow
```

---

### Step 1.3: Install FFmpeg

```powershell
# Using winget (easiest)
winget install ffmpeg

# Verify
ffmpeg -version
```

---

### Step 1.4: Install WireGuard

1. Download: https://download.wireguard.com/windows-client/wireguard-installer.exe
2. Run installer
3. Click through wizard
4. Click **Finish**

---

### Step 1.5: Configure WireGuard VPN Server

**Generate server keys:**

```powershell
cd "C:\Program Files\WireGuard"

# Generate server private key
.\wg.exe genkey | Out-File -Encoding ASCII server_private.key

# Generate server public key
Get-Content server_private.key | .\wg.exe pubkey | Out-File -Encoding ASCII server_public.key

# Display keys
Write-Host "Server Private Key:"
Get-Content server_private.key
Write-Host "`nServer Public Key:"
Get-Content server_public.key
```

**Copy both keys to a text file.**

---

**Create server config:** `C:\Program Files\WireGuard\wg0.conf`

```ini
[Interface]
PrivateKey = YOUR_SERVER_PRIVATE_KEY_HERE
Address = 10.0.0.1/24
ListenPort = 51820

# This will be populated with web app peer config
```

**Import into WireGuard:**
1. Open WireGuard app
2. Click **Import tunnel(s) from file**
3. Select `wg0.conf`
4. Click **Activate**

---

### Step 1.6: Configure Router Port Forwarding

**Log into router and add:**

```
Service Name: GFC Camera VPN
External Port: 51820
Internal IP: 192.168.1.200
Internal Port: 51820
Protocol: UDP
Status: Enabled
```

**Test:** https://www.yougetsignal.com/tools/open-ports/ (port 51820 should be open)

---

### Step 1.7: Set Up DDNS

**Option A: No-IP**
1. Go to: https://www.noip.com/
2. Create free account
3. Create hostname: `yourclub.ddns.net`
4. Download DUC client
5. Install and configure

**Option B: DuckDNS**
1. Go to: https://www.duckdns.org/
2. Create subdomain: `yourclub.duckdns.org`
3. Set up update script (see previous guide)

---

### Step 1.8: Install Video Agent

**Create folder:** `C:\GFC\VideoAgent\`

**Create config:** `C:\GFC\VideoAgent\appsettings.json`

```json
{
  "NVR": {
    "IpAddress": "192.168.1.64",
    "Port": 8000,
    "RtspPort": 554,
    "Username": "admin",
    "Password": "YOUR_NVR_PASSWORD"
  },
  "Server": {
    "Port": 5100,
    "ApiKey": "GENERATE_RANDOM_GUID"
  },
  "FFmpeg": {
    "ExecutablePath": "C:\\ffmpeg\\bin\\ffmpeg.exe",
    "OutputPath": "C:\\GFC\\VideoAgent\\streams\\"
  }
}
```

**Generate API Key:**
```powershell
[guid]::NewGuid().ToString()
```

**Save this API key - you'll need it for the web app!**

---

**Install as Windows Service:**

```powershell
# Download NSSM
Invoke-WebRequest -Uri "https://nssm.cc/release/nssm-2.24.zip" -OutFile "C:\Temp\nssm.zip"
Expand-Archive -Path "C:\Temp\nssm.zip" -DestinationPath "C:\Temp\"
Copy-Item "C:\Temp\nssm-2.24\win64\nssm.exe" -Destination "C:\Windows\System32\"

# Install service (assuming you have the Video Agent executable)
nssm install "GFC Video Agent" "C:\GFC\VideoAgent\GFC.VideoAgent.exe"
nssm set "GFC Video Agent" AppDirectory "C:\GFC\VideoAgent"
nssm set "GFC Video Agent" Start SERVICE_AUTO_START
nssm start "GFC Video Agent"
```

---

### Step 1.9: Generate VPN Config for Web App Server

**Generate web app server keys:**

```powershell
cd "C:\Program Files\WireGuard"

# Generate client private key for web app server
.\wg.exe genkey | Out-File -Encoding ASCII webapp_private.key

# Generate client public key
Get-Content webapp_private.key | .\wg.exe pubkey | Out-File -Encoding ASCII webapp_public.key

# Display keys
Write-Host "Web App Client Private Key:"
Get-Content webapp_private.key
Write-Host "`nWeb App Client Public Key:"
Get-Content webapp_public.key
```

---

**Create web app VPN config:** `webapp-client.conf`

```ini
[Interface]
PrivateKey = WEBAPP_CLIENT_PRIVATE_KEY_HERE
Address = 10.0.0.2/24

[Peer]
PublicKey = YOUR_SERVER_PUBLIC_KEY_HERE
Endpoint = yourclub.ddns.net:51820
AllowedIPs = 192.168.1.0/24, 10.0.0.0/24
PersistentKeepalive = 25
```

**Save this file - you'll install it on the web app server!**

---

**Add web app peer to server config:**

Edit `C:\Program Files\WireGuard\wg0.conf` and add:

```ini
[Peer]
PublicKey = WEBAPP_CLIENT_PUBLIC_KEY_HERE
AllowedIPs = 10.0.0.2/32
```

**Restart WireGuard:**
1. Deactivate
2. Activate

---

## PART 2: On Web App Server (Remote) (30 minutes)

### Step 2.1: Install WireGuard VPN Client

**On Linux (Ubuntu/Debian):**
```bash
sudo apt update
sudo apt install wireguard
```

**On Windows Server:**
- Download: https://download.wireguard.com/windows-client/wireguard-installer.exe
- Install

---

### Step 2.2: Import VPN Config

**On Linux:**
```bash
# Copy webapp-client.conf to server
sudo cp webapp-client.conf /etc/wireguard/wg0.conf

# Start VPN
sudo wg-quick up wg0

# Enable on boot
sudo systemctl enable wg-quick@wg0
```

**On Windows:**
- Open WireGuard app
- Import `webapp-client.conf`
- Activate

---

### Step 2.3: Test VPN Connection

```bash
# Ping the Video Agent
ping 10.0.0.1

# Test Video Agent API
curl http://10.0.0.1:5100/api/heartbeat
```

Should get response from Video Agent!

---

### Step 2.4: Update Web App Configuration

**Edit `appsettings.json` on web app server:**

```json
{
  "VideoAgent": {
    "BaseUrl": "http://10.0.0.1:5100",
    "ApiKey": "YOUR_API_KEY_FROM_STEP_1.8",
    "HeartbeatIntervalSeconds": 10
  }
}
```

**Note:** Using VPN IP `10.0.0.1` (not `192.168.1.200`)

---

### Step 2.5: Restart Web App

```bash
# On Linux with systemd
sudo systemctl restart gfc-webapp

# On Windows
Restart-Service "GFC Web App"

# Or if using dotnet directly
# Stop and restart the process
```

---

## PART 3: Testing (10 minutes)

### Step 3.1: Test from Web App Server

**On the web app server:**

```bash
# Test VPN
ping 10.0.0.1

# Test Video Agent
curl http://10.0.0.1:5100/api/heartbeat

# Should return JSON with status
```

---

### Step 3.2: Test from User Browser

1. **Open browser** (anywhere in the world)
2. **Go to your web app** (e.g., https://yourapp.com)
3. **Log in**
4. **Check Dashboard** - should see 🟢 green status LED
5. **Click "Cameras"**
6. **Should see live video!**

---

## 📊 What You End Up With

### **At the Club:**
- Local PC running:
  - WireGuard VPN Server (port 51820)
  - Video Agent (port 5100)
- Router forwarding port 51820

### **On Web App Server:**
- WireGuard VPN Client (connected to club)
- Web app configured to use Video Agent via VPN

### **For Users:**
- Just access web app normally
- No VPN needed
- No special software
- Works from anywhere

---

## 🔐 Security Benefits

✅ **NVR never exposed** - stays on LAN  
✅ **Only VPN port open** - port 51820  
✅ **Web app connects securely** - encrypted VPN tunnel  
✅ **Users don't need VPN** - web app handles it  
✅ **Centralized access control** - manage in web app  

---

## 🐛 Troubleshooting

### Web App Can't Connect to Video Agent

**Check on web app server:**
```bash
# Is VPN connected?
sudo wg show

# Can you ping Video Agent?
ping 10.0.0.1

# Can you reach Video Agent API?
curl http://10.0.0.1:5100/api/heartbeat
```

**If VPN not connected:**
```bash
# Check VPN config
sudo cat /etc/wireguard/wg0.conf

# Check VPN logs
sudo journalctl -u wg-quick@wg0
```

---

### Users See Red LED

**Means web app can't reach Video Agent.**

**Check:**
1. Is VPN connected on web app server?
2. Is Video Agent running at club?
3. Is API key correct in web app config?

---

## ✅ Success Checklist

**At the Club:**
- [ ] Local PC has static IP (192.168.1.200)
- [ ] WireGuard VPN server running
- [ ] Port 51820 forwarded on router
- [ ] DDNS configured and updating
- [ ] Video Agent service running
- [ ] Can access NVR (192.168.1.64)

**On Web App Server:**
- [ ] WireGuard VPN client installed
- [ ] VPN config imported
- [ ] VPN connected (can ping 10.0.0.1)
- [ ] Web app config updated with Video Agent URL
- [ ] Web app restarted

**User Testing:**
- [ ] Users can access web app
- [ ] Dashboard shows green LED
- [ ] Cameras page loads
- [ ] Live video plays

---

## 💡 Future: Moving Web App to Local Network

**When you move the web app to a server at the club:**

1. **Remove VPN from web app server** (no longer needed)
2. **Update web app config:**
   ```json
   {
     "VideoAgent": {
       "BaseUrl": "http://192.168.1.200:5100",
       ...
     }
   }
   ```
3. **Keep VPN for user remote access** (users will need VPN to access web app)

**Or keep current setup** - works either way!

---

## 📞 Summary

**What's installed at club:**
- FFmpeg
- WireGuard VPN Server
- Video Agent

**What's installed on web app server:**
- WireGuard VPN Client

**What users need:**
- Nothing! Just a browser

**Total setup time:** ~1.5 hours  
**Cost:** $0 (using existing hardware)

---

**This is the correct architecture for your scenario!** 🎉
