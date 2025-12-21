# Remote Camera Access - Simple Setup Guide

**Goal:** View cameras from anywhere (home, office, phone) WITHOUT being on club LAN  
**Method:** VPN (WireGuard) - The safest approach  
**Web App Location:** Hosted at the club (you access it remotely)  
**Time to Setup:** ~1 hour

---

## 🎯 What You're Building

```
┌─────────────────────────────────────────────────────────────────┐
│                         AT HOME / REMOTE                        │
│                                                                 │
│  📱 Your Phone          💻 Your Laptop        🖥️ Your Desktop  │
│     │                       │                      │            │
│     └───────────────────────┴──────────────────────┘            │
│                             │                                   │
│                             │ WireGuard VPN                     │
│                             │ (Encrypted Tunnel)                │
│                             ▼                                   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              │ INTERNET
                              │
┌─────────────────────────────▼───────────────────────────────────┐
│                      AT THE CLUB (LAN)                          │
│                                                                 │
│  ┌──────────────────┐                                          │
│  │  VPN Server      │ ◄─── You connect here first             │
│  │  (WireGuard)     │                                          │
│  └────────┬─────────┘                                          │
│           │                                                     │
│           │ Now you're "inside" the club network               │
│           ▼                                                     │
│  ┌──────────────────┐      ┌──────────────┐                   │
│  │  GFC Web App     │ ◄──► │ Video Agent  │                   │
│  │  (Hosted at club)│      │              │                   │
│  └──────────────────┘      └──────┬───────┘                   │
│                                   │                            │
│                                   ▼                            │
│                            ┌──────────────┐                    │
│                            │     NVR      │                    │
│                            │ 192.168.1.64 │                    │
│                            │  📹📹📹📹  │                    │
│                            └──────────────┘                    │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

**Key Point:** Once connected to VPN, your device acts like it's AT the club. Then you just open the web app in your browser like normal!

---

## 📦 What Needs to Be Installed (At the Club)

### 1. **GFC Web App** (Already installed at club, right?)
- Hosted on club server/PC
- Accessible via local IP (e.g., http://192.168.1.50:5000)
- No changes needed for remote access
- You'll access this through VPN

### 2. **Video Agent Service** (At the club)
- Installed on a club PC (same as Phase 1)
- Runs 24/7 as Windows service
- Connects to NVR and converts streams
- **Required:** Yes, this is essential

### 3. **WireGuard VPN Server** (At the club) ⭐ NEW
- Installed on club router OR dedicated PC
- Creates secure tunnel from internet to club LAN
- Allows remote devices to connect
- **This is the key piece for remote access**

---

## 🔧 Installation Options for VPN Server

You have **3 options** for where to run the VPN server:

### **Option 1: On Your Router** ✅ BEST (if supported)

**Pros:**
- No additional hardware needed
- Always on (router is always running)
- Best performance
- Centralized management

**Cons:**
- Not all routers support WireGuard
- May need router firmware update (e.g., DD-WRT, OpenWrt)

**Supported Routers:**
- UniFi Dream Machine / UDM Pro
- pfSense / OPNsense
- Mikrotik routers
- ASUS routers with Merlin firmware
- GL.iNet routers (built-in WireGuard)

**Setup Time:** 30 minutes

---

### **Option 2: Dedicated Raspberry Pi** ✅ RECOMMENDED (if router doesn't support)

**Pros:**
- Cheap (~$50 for Pi + case + power)
- Low power consumption (~5W)
- Easy to set up
- Can run 24/7
- Doesn't affect other systems

**Cons:**
- Requires buying hardware (if you don't have one)
- One more device to manage

**What you need:**
- Raspberry Pi 4 (2GB or 4GB model)
- MicroSD card (16GB+)
- Power supply
- Ethernet cable

**Setup Time:** 45 minutes

---

### **Option 3: Windows PC at Club** ⚠️ OK (but not ideal)

**Pros:**
- No new hardware if you have a PC running 24/7
- Can use same PC as Video Agent

**Cons:**
- PC must run 24/7
- Higher power consumption
- If PC reboots, VPN goes down

**Setup Time:** 30 minutes

---

## 🚀 Recommended Setup (Easiest Path)

### **At the Club (One-Time Setup):**

#### Step 1: Install WireGuard VPN Server (Choose one option above)

**If using Raspberry Pi (recommended):**
```bash
# 1. Install Raspberry Pi OS Lite
# 2. SSH into Pi
# 3. Run this one-liner:
curl -L https://install.pivpn.io | bash

# Follow the wizard:
# - Choose WireGuard (not OpenVPN)
# - Set port: 51820
# - Choose DNS: 192.168.1.1 (your router)
# - Done!
```

**If using Windows PC:**
- Download WireGuard for Windows: https://www.wireguard.com/install/
- Install as administrator
- Configure server (see detailed steps below)

**If using router:**
- Log into router admin panel
- Find VPN section
- Enable WireGuard
- Follow router-specific instructions

---

#### Step 2: Configure Router Port Forwarding

**You need to forward ONE port:**
- **Port:** 51820 (UDP)
- **Forward to:** IP of VPN server (Pi or PC)
- **Protocol:** UDP only

**Example:**
```
External Port: 51820 (UDP)
Internal IP: 192.168.1.100 (your Pi/PC)
Internal Port: 51820 (UDP)
```

**This is the ONLY port forwarding you need!**
- NVR ports stay closed ✅
- Web app ports stay closed ✅
- Only VPN port is open ✅

---

#### Step 3: Get Your Public IP or Set Up DDNS

**Option A: Static Public IP** (if you have one)
- Note your public IP: https://whatismyipaddress.com/
- Use this in VPN client config

**Option B: Dynamic DNS** (if IP changes)
- Sign up for free DDNS: https://www.noip.com/ or https://duckdns.org/
- Create hostname: `yourclub.ddns.net`
- Configure router to update DDNS
- Use hostname in VPN client config

---

#### Step 4: Create VPN Client Configs (One per device/person)

**On VPN server, create a config for each user:**

```bash
# On Raspberry Pi with PiVPN:
pivpn add

# Enter name: john-laptop
# QR code and config file generated!
```

**You'll get a config file like:**
```
[Interface]
PrivateKey = [auto-generated]
Address = 10.6.0.2/24
DNS = 192.168.1.1

[Peer]
PublicKey = [auto-generated]
Endpoint = yourclub.ddns.net:51820
AllowedIPs = 192.168.1.0/24
PersistentKeepalive = 25
```

**Create one config for:**
- Your laptop
- Your phone
- Your tablet
- Each staff member who needs access

---

### **On Your Devices (Remote Access):**

#### For Windows/Mac/Linux:

1. **Install WireGuard Client:**
   - Download: https://www.wireguard.com/install/
   - Install (takes 2 minutes)

2. **Import Config:**
   - Open WireGuard app
   - Click "Add Tunnel"
   - Select the `.conf` file you got from club
   - Click "Activate"

3. **Connect:**
   - Click "Activate" button
   - Status shows "Active"
   - **You're now "inside" the club network!**

4. **Access GFC Web App:**
   - Open browser
   - Go to: `http://192.168.1.50:5000` (your club's web app IP)
   - Log in normally
   - Click "Cameras"
   - **Watch live video from anywhere!**

---

#### For iPhone/Android:

1. **Install WireGuard App:**
   - iPhone: App Store → "WireGuard"
   - Android: Play Store → "WireGuard"

2. **Import Config:**
   - **Option A:** Scan QR code (easiest)
     - Open WireGuard app
     - Tap "+"
     - Tap "Create from QR code"
     - Scan QR code from VPN server
   
   - **Option B:** Import file
     - Email yourself the `.conf` file
     - Open in WireGuard app

3. **Connect:**
   - Tap the toggle switch
   - Status shows "Active"
   - **You're connected!**

4. **Access GFC Web App:**
   - Open Safari/Chrome
   - Go to: `http://192.168.1.50:5000`
   - Log in
   - View cameras!

---

## 🔒 Security: Why This Is Safe

### **What's Protected:**

✅ **NVR is NEVER exposed to internet**
- Only VPN port (51820) is open
- NVR ports (554, 8000) stay closed
- No direct access to NVR from internet

✅ **Encrypted tunnel**
- All traffic encrypted with modern cryptography
- Same encryption used by banks and military
- Cannot be intercepted or spied on

✅ **Per-user access control**
- Each person has their own VPN key
- Revoke access instantly by deleting their config
- Know who's connected at all times

✅ **No cloud services**
- No third-party involved
- No vendor can be hacked
- You control everything

✅ **Full audit trail**
- VPN logs who connects when
- Web app logs who views cameras
- Complete accountability

---

### **What's NOT Exposed:**

❌ NVR web interface (port 8000) - CLOSED  
❌ RTSP streams (port 554) - CLOSED  
❌ GFC Web App (port 5000) - CLOSED  
❌ Video Agent (port 5100) - CLOSED  

**Only VPN port (51820) is open** - and it requires your private key to connect!

---

## 📊 Complete Setup Checklist

### **At the Club (One-Time Setup):**

- [ ] **Install Video Agent** (if not already done)
  - On club PC
  - Configure NVR credentials
  - Verify working locally

- [ ] **Install WireGuard VPN Server**
  - Choose: Router, Raspberry Pi, or Windows PC
  - Install WireGuard
  - Configure server settings

- [ ] **Configure Router**
  - Forward port 51820 (UDP) to VPN server
  - Set up DDNS (if no static IP)
  - Test port is open: https://www.yougetsignal.com/tools/open-ports/

- [ ] **Create VPN Client Configs**
  - One per device/person
  - Save `.conf` files securely
  - Generate QR codes for mobile devices

- [ ] **Test Locally**
  - Connect to VPN from club Wi-Fi
  - Access web app through VPN
  - Verify cameras work

- [ ] **Test Remotely**
  - Disconnect from club Wi-Fi (use cellular)
  - Connect to VPN
  - Access web app
  - Verify cameras work

---

### **On Your Devices (Each Device):**

- [ ] **Install WireGuard Client**
  - Download from official site
  - Install app

- [ ] **Import VPN Config**
  - Use QR code or `.conf` file
  - Save tunnel

- [ ] **Test Connection**
  - Activate VPN
  - Verify "Active" status
  - Ping club network: `ping 192.168.1.50`

- [ ] **Access Web App**
  - Open browser
  - Go to club web app IP
  - Log in
  - View cameras

---

## 🎬 Day-to-Day Usage (After Setup)

### **From Home/Remote:**

1. **Open WireGuard app** on your device
2. **Click "Activate"** (connects VPN)
3. **Open browser**
4. **Go to:** `http://192.168.1.50:5000` (your club's IP)
5. **Log in** to GFC Web App
6. **Click "Cameras"**
7. **Watch live video!**

**When done:**
1. Close browser
2. Deactivate VPN (optional, but saves battery)

**That's it!** Same experience as being at the club.

---

## 💰 Cost Breakdown

### **Option 1: Router with WireGuard Support**
- **Cost:** $0 (if you already have compatible router)
- **Or:** $150-300 (if buying new router like UniFi Dream Machine)

### **Option 2: Raspberry Pi** ✅ RECOMMENDED
- **Raspberry Pi 4 (4GB):** $55
- **MicroSD Card (32GB):** $10
- **Power Supply:** $8
- **Case:** $7
- **Total:** ~$80

### **Option 3: Use Existing PC**
- **Cost:** $0 (but higher electricity cost)

### **VPN Software:**
- **WireGuard:** FREE (open-source)

### **DDNS Service:**
- **No-IP / DuckDNS:** FREE

**Total Cost: $0-80** (one-time)

---

## 🐛 Troubleshooting

### **Can't connect to VPN:**

1. **Check port forwarding:**
   - Port 51820 (UDP) forwarded to VPN server?
   - Test: https://www.yougetsignal.com/tools/open-ports/

2. **Check VPN server is running:**
   - Raspberry Pi: `sudo systemctl status wg-quick@wg0`
   - Windows: Check WireGuard app

3. **Check firewall:**
   - Windows Firewall allowing port 51820?
   - Router firewall allowing port 51820?

4. **Check DDNS:**
   - Is hostname resolving? `nslookup yourclub.ddns.net`
   - Is IP correct?

---

### **VPN connects, but can't access web app:**

1. **Check you're using correct IP:**
   - What's the GFC Web App IP on club LAN?
   - Try: `http://192.168.1.50:5000`

2. **Check web app is running:**
   - At club, can you access it locally?

3. **Check VPN routing:**
   - Can you ping web app? `ping 192.168.1.50`
   - If not, check VPN `AllowedIPs` setting

---

### **VPN works, web app works, but cameras won't load:**

1. **Check Video Agent is running:**
   - At club, check Windows Services
   - "GFC Video Agent" should be "Running"

2. **Check from club first:**
   - Does it work when at club?
   - If not, fix local issue first

3. **Check VPN bandwidth:**
   - Slow connection? Try lower quality stream

---

## 📱 Mobile Access Tips

### **For Best Mobile Experience:**

✅ **Use Wi-Fi when possible:**
- VPN works on cellular, but uses data
- One camera = ~500 Kbps = ~225 MB/hour

✅ **Disconnect VPN when done:**
- Saves battery
- Saves data

✅ **Use "On-Demand" VPN:**
- Auto-connect when accessing club IPs
- Auto-disconnect when done
- Configure in WireGuard app

✅ **Add web app to home screen:**
- Makes it feel like a native app
- Quick access

---

## 🔐 Security Best Practices

### **VPN Key Management:**

✅ **One key per device:**
- john-laptop
- john-phone
- mary-tablet

✅ **Revoke immediately when:**
- Device is lost/stolen
- Employee leaves
- Key is compromised

✅ **Never share keys:**
- Each person gets their own
- No "shared" keys

✅ **Rotate keys periodically:**
- Every 6-12 months
- Generate new configs
- Delete old ones

---

### **Access Control:**

✅ **VPN access ≠ Camera access:**
- VPN gets you to network
- Still need web app login
- Still need camera permissions

✅ **Use strong web app passwords:**
- VPN is one layer
- Web app auth is second layer
- Two-factor if possible (future)

✅ **Monitor VPN connections:**
- Check who's connected
- Review connection logs
- Investigate suspicious activity

---

## ✅ Success Criteria

You're done when you can:

- [ ] Connect to VPN from home
- [ ] Access GFC Web App through VPN
- [ ] Log in to web app
- [ ] View live cameras
- [ ] Video plays smoothly
- [ ] Can do this from laptop
- [ ] Can do this from phone
- [ ] Can disconnect and reconnect easily
- [ ] Other staff can connect with their own keys

---

## 📊 What You End Up With

### **At the Club:**
- GFC Web App (already there)
- Video Agent service (already there)
- VPN server (new - Raspberry Pi or router)
- Port 51820 forwarded (only port open)

### **On Your Devices:**
- WireGuard app (free)
- VPN config file (one per device)

### **Usage:**
1. Activate VPN (1 click)
2. Open browser
3. Go to club web app
4. View cameras

**Same experience from anywhere in the world!**

---

## 🎯 Summary

**What you need at the club:**
- ✅ Video Agent (Windows service)
- ✅ WireGuard VPN server (Raspberry Pi recommended)
- ✅ Port forwarding (only port 51820)
- ✅ DDNS (if no static IP)

**What you need on your devices:**
- ✅ WireGuard app (free)
- ✅ VPN config file

**What you DON'T need:**
- ❌ Web app installed locally
- ❌ Any special software besides WireGuard
- ❌ Port forwarding for NVR
- ❌ Cloud services

**Cost:** $0-80 (one-time)  
**Setup time:** ~1 hour  
**Daily usage:** 2 clicks (activate VPN, open browser)

---

**Ready to set this up?** Start with the Raspberry Pi option - it's the easiest and most reliable! 🚀

**Questions?** See `CAMERA_SECURITY_MASTER_GUIDE.md` for security details.
