# Local Computer Setup - Ultra-Detailed Step-by-Step Guide

**For:** Non-technical users  
**Time:** 1-2 hours  
**Difficulty:** Easy (follow screenshots)  
**Result:** Fully configured Video Agent PC

---

## 📋 Before You Start

**What you need:**
- [ ] Windows PC at the club (will become Video Agent PC)
- [ ] Admin access to this PC
- [ ] Router admin login
- [ ] NVR admin login
- [ ] 1-2 hours of uninterrupted time

**What you'll install:**
- FFmpeg (video converter)
- .NET 8 SDK (for Video Agent)
- WireGuard (VPN server) - you already have this!

---

## 🎯 Overview: What We're Building

```
Your PC (Video Agent) will become:
┌─────────────────────────────────────────┐
│  Video Agent PC (192.168.1.200)         │
│  ┌───────────────────────────────────┐  │
│  │  WireGuard VPN Server             │  │
│  │  (Allows web app to connect)      │  │
│  └───────────────────────────────────┘  │
│  ┌───────────────────────────────────┐  │
│  │  Video Agent Service              │  │
│  │  (Converts camera streams)        │  │
│  └───────────────────────────────────┘  │
│  ┌───────────────────────────────────┐  │
│  │  FFmpeg (Video converter)         │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
         ↓
    Connects to NVR (192.168.1.64)
         ↓
    Gets camera streams
```

---

## STEP 1: Set Static IP Address (10 minutes)

### **Why:** So the PC always has the same IP address

### **What to do:**

**1.1 Open Network Settings**

Press `Windows Key + R` on your keyboard

You'll see this window:
```
┌─────────────────────────────┐
│  Run                    [X] │
├─────────────────────────────┤
│  Open: [              ]     │
│                             │
│  [OK] [Cancel] [Browse...]  │
└─────────────────────────────┘
```

Type: `ncpa.cpl`

Click `OK`

---

**1.2 Find Your Network Adapter**

You'll see a window like this:
```
┌──────────────────────────────────────────┐
│  Network Connections                     │
├──────────────────────────────────────────┤
│  [Icon] Ethernet                         │
│         Network cable unplugged          │
│                                          │
│  [Icon] Wi-Fi                            │
│         Connected                        │
│                                          │
│  [Icon] Bluetooth Network Connection     │
│         Not connected                    │
└──────────────────────────────────────────┘
```

**Right-click** the one that says "Connected" (usually "Ethernet" or "Wi-Fi")

Click `Properties`

---

**1.3 Open IPv4 Settings**

You'll see this window:
```
┌──────────────────────────────────────────┐
│  Ethernet Properties                 [X] │
├──────────────────────────────────────────┤
│  This connection uses the following:     │
│                                          │
│  ☑ Client for Microsoft Networks        │
│  ☑ File and Printer Sharing              │
│  ☑ Internet Protocol Version 6 (TCP/IPv6)│
│  ☑ Internet Protocol Version 4 (TCP/IPv4)│ ← Double-click this
│  ☐ Link-Layer Topology Discovery         │
│                                          │
│  [Install] [Uninstall] [Properties]      │
└──────────────────────────────────────────┘
```

**Double-click** `Internet Protocol Version 4 (TCP/IPv4)`

---

**1.4 Set Static IP**

You'll see this window:
```
┌──────────────────────────────────────────┐
│  Internet Protocol Version 4 Properties  │
├──────────────────────────────────────────┤
│  ○ Obtain an IP address automatically    │ ← Currently selected
│  ● Use the following IP address:         │ ← Click this
│                                          │
│  IP address:     [192.168.1.200]        │ ← Type this
│  Subnet mask:    [255.255.255.0]        │ ← Type this
│  Default gateway:[192.168.1.1]          │ ← Type this
│                                          │
│  ○ Obtain DNS server address automatically│
│  ● Use the following DNS server addresses:│ ← Click this
│                                          │
│  Preferred DNS:  [192.168.1.1]          │ ← Type this
│  Alternate DNS:  [8.8.8.8]              │ ← Type this (optional)
│                                          │
│  [OK] [Cancel]                           │
└──────────────────────────────────────────┘
```

**Click the radio button:** `Use the following IP address`

**Type these EXACT values:**
- IP address: `192.168.1.200`
- Subnet mask: `255.255.255.0`
- Default gateway: `192.168.1.1`

**Click the radio button:** `Use the following DNS server addresses`

**Type:**
- Preferred DNS: `192.168.1.1`
- Alternate DNS: `8.8.8.8` (optional)

**Click `OK`**

**Click `OK` again** on the Ethernet Properties window

---

**1.5 Verify It Worked**

Press `Windows Key + R`

Type: `cmd`

Click `OK`

In the black window, type:
```
ipconfig
```

Press `Enter`

You should see:
```
Ethernet adapter Ethernet:
   IPv4 Address. . . . . . . . . . . : 192.168.1.200  ← Should show this!
   Subnet Mask . . . . . . . . . . . : 255.255.255.0
   Default Gateway . . . . . . . . . : 192.168.1.1
```

**✅ If you see `192.168.1.200` - SUCCESS!**

**❌ If not - go back and check you typed it correctly**

---

## STEP 2: Configure Windows Firewall (5 minutes)

### **Why:** Allow VPN and Video Agent traffic

### **What to do:**

**2.1 Open PowerShell as Administrator**

Click the `Start` button (Windows logo)

Type: `powershell`

You'll see:
```
┌──────────────────────────────────────────┐
│  Best match                              │
│  [Icon] Windows PowerShell               │
│         App                              │
│                                          │
│  → Run as administrator                  │ ← Click this
│    Open file location                    │
│    Pin to Start                          │
└──────────────────────────────────────────┘
```

**Right-click** `Windows PowerShell`

**Click** `Run as administrator`

**Click `Yes`** if asked "Do you want to allow this app to make changes?"

---

**2.2 Run Firewall Commands**

You'll see a blue window like this:
```
Windows PowerShell
Copyright (C) Microsoft Corporation. All rights reserved.

PS C:\Windows\system32>
```

**Copy and paste this ENTIRE block** (right-click to paste):

```powershell
# Allow VPN port
New-NetFirewallRule -DisplayName "GFC Camera VPN" -Direction Inbound -Protocol UDP -LocalPort 51820 -Action Allow

# Allow Video Agent
New-NetFirewallRule -DisplayName "GFC Video Agent" -Direction Inbound -Protocol TCP -LocalPort 5100 -Action Allow

Write-Host "Firewall rules created successfully!" -ForegroundColor Green
```

Press `Enter`

You should see:
```
Name                  : {GUID}
DisplayName           : GFC Camera VPN
...

Name                  : {GUID}
DisplayName           : GFC Video Agent
...

Firewall rules created successfully!
```

**✅ If you see "successfully" - DONE!**

**Keep PowerShell open - we'll use it again**

---

## STEP 3: Install FFmpeg (5 minutes)

### **Why:** Converts camera video to web-friendly format

### **What to do:**

**3.1 Install via winget**

In the same PowerShell window, type:

```powershell
winget install ffmpeg
```

Press `Enter`

You'll see:
```
Found FFmpeg [Gyan.FFmpeg]
This application is licensed to you by its owner.
Microsoft is not responsible for, nor does it grant any licenses to, third-party packages.
Downloading https://...
  ██████████████████████████████  100%
Successfully verified installer hash
Starting package install...
Successfully installed
```

**Wait for "Successfully installed"**

---

**3.2 Verify FFmpeg Works**

In PowerShell, type:

```powershell
ffmpeg -version
```

Press `Enter`

You should see:
```
ffmpeg version 2024-11-20-git-... Copyright (c) 2000-2024 the FFmpeg developers
built with gcc ...
configuration: ...
```

**✅ If you see version info - SUCCESS!**

**❌ If you see "not recognized" - close and reopen PowerShell, try again**

---

## STEP 4: Install .NET 8 SDK (5 minutes)

### **Why:** Required to run Video Agent service

### **What to do:**

**4.1 Install via winget**

In PowerShell, type:

```powershell
winget install Microsoft.DotNet.SDK.8
```

Press `Enter`

You'll see:
```
Found Microsoft .NET SDK 8.0 [Microsoft.DotNet.SDK.8]
...
Downloading https://...
  ██████████████████████████████  100%
Successfully installed
```

**Wait for "Successfully installed"**

---

**4.2 Verify .NET Works**

**Close PowerShell** (type `exit` and press Enter)

**Open a NEW PowerShell** (as administrator, same as Step 2.1)

Type:

```powershell
dotnet --version
```

Press `Enter`

You should see:
```
8.0.404
```

**✅ If you see `8.0.x` - SUCCESS!**

---

## STEP 5: Configure WireGuard VPN Server (15 minutes)

### **Why:** Allows web app to connect securely

### **What to do:**

**5.1 Generate Server Keys**

In PowerShell, type:

```powershell
cd "C:\Program Files\WireGuard"
```

Press `Enter`

Then type:

```powershell
.\wg.exe genkey | Out-File -Encoding ASCII server_private.key
Get-Content server_private.key | .\wg.exe pubkey | Out-File -Encoding ASCII server_public.key
```

Press `Enter`

---

**5.2 View Your Keys**

Type:

```powershell
Write-Host "Server Private Key:"
Get-Content server_private.key
Write-Host "`nServer Public Key:"
Get-Content server_public.key
```

Press `Enter`

You'll see something like:
```
Server Private Key:
aBcDeFgHiJkLmNoPqRsTuVwXyZ1234567890ABCD=

Server Public Key:
XyZ1234567890ABCDeFgHiJkLmNoPqRsTuVwXyZ=
```

**IMPORTANT: Copy both keys to a text file!** You'll need them later.

---

**5.3 Create Server Config**

**Open Notepad**

Click `Start` → Type `notepad` → Press `Enter`

**Copy and paste this:**

```ini
[Interface]
PrivateKey = PASTE_YOUR_SERVER_PRIVATE_KEY_HERE
Address = 10.0.0.1/24
ListenPort = 51820

# Web app peer will be added here later
```

**Replace `PASTE_YOUR_SERVER_PRIVATE_KEY_HERE`** with your actual server private key from Step 5.2

**Save the file as:**
- Location: `C:\Program Files\WireGuard\`
- Filename: `wg0.conf`
- Save as type: `All Files (*.*)`

**Click `Save`**

---

**5.4 Import Config into WireGuard**

**Open WireGuard app** (look for icon in system tray, or Start menu)

You'll see:
```
┌──────────────────────────────────────────┐
│  WireGuard                           [X] │
├──────────────────────────────────────────┤
│  No tunnels                              │
│                                          │
│  [Import tunnel(s) from file]           │ ← Click this
│  [Add empty tunnel...]                   │
└──────────────────────────────────────────┘
```

**Click** `Import tunnel(s) from file`

**Navigate to:** `C:\Program Files\WireGuard\`

**Select:** `wg0.conf`

**Click** `Open`

You'll now see:
```
┌──────────────────────────────────────────┐
│  WireGuard                           [X] │
├──────────────────────────────────────────┤
│  wg0                                     │
│  Status: Inactive                        │
│                                          │
│  [Activate]                              │ ← Click this
└──────────────────────────────────────────┘
```

**Click** `Activate`

Status should change to:
```
Status: Active
```

**✅ VPN Server is now running!**

---

## STEP 6: Configure Router Port Forwarding (10 minutes)

### **Why:** Allows web app to connect from internet

### **What to do:**

**6.1 Find Your Router's IP**

Already done! It's `192.168.1.1` (from Step 1)

---

**6.2 Log Into Router**

**Open a web browser**

**Go to:** `http://192.168.1.1`

You'll see your router's login page

**Enter:**
- Username: (usually `admin`)
- Password: (check router label or manual)

**Click `Login`**

---

**6.3 Find Port Forwarding Section**

**Look for one of these menu items:**
- "Port Forwarding"
- "Virtual Server"
- "NAT"
- "Advanced" → "Port Forwarding"

**Click it**

---

**6.4 Add Port Forwarding Rule**

You'll see a form like this:
```
┌──────────────────────────────────────────┐
│  Add Port Forwarding Rule                │
├──────────────────────────────────────────┤
│  Service Name:    [GFC Camera VPN]       │
│  External Port:   [51820]                │
│  Internal IP:     [192.168.1.200]        │
│  Internal Port:   [51820]                │
│  Protocol:        [UDP ▼]                │
│  Status:          [☑ Enabled]            │
│                                          │
│  [Save] [Cancel]                         │
└──────────────────────────────────────────┘
```

**Fill in:**
- Service Name: `GFC Camera VPN`
- External Port: `51820`
- Internal IP: `192.168.1.200`
- Internal Port: `51820`
- Protocol: `UDP` (very important!)
- Status: `Enabled` (check the box)

**Click `Save` or `Apply`**

---

**6.5 Verify Port Forwarding**

**Open a new browser tab**

**Go to:** https://www.yougetsignal.com/tools/open-ports/

**Enter port:** `51820`

**Click** `Check`

You should see:
```
Port 51820 is open on [Your IP]
```

**✅ If "open" - SUCCESS!**

**❌ If "closed" - double-check router config**

---

## STEP 7: Set Up DuckDNS (10 minutes)

### **Why:** So web app can find you even if IP changes

### **What to do:**

**7.1 Create DuckDNS Account**

**Go to:** https://www.duckdns.org/

**Click:** `Sign in with Google` (or GitHub/Reddit)

**Authorize DuckDNS**

---

**7.2 Create Subdomain**

You'll see:
```
┌──────────────────────────────────────────┐
│  sub domains                             │
├──────────────────────────────────────────┤
│  [yourclub        ].duckdns.org          │
│  [add domain]                            │
└──────────────────────────────────────────┘
```

**Type your club name** (e.g., `mygfc`, `clubname`, etc.)

**Click** `add domain`

---

**7.3 Copy Your Token**

At the top of the page, you'll see:
```
token
aBcDeFgH-1234-5678-90AB-CDEFGHIJKLMN
```

**Copy this token!** You'll need it in the next step.

---

**7.4 Create Update Script**

**Go back to PowerShell** (as administrator)

**Type:**

```powershell
# Create Scripts folder
New-Item -Path "C:\Scripts" -ItemType Directory -Force

# Create update script (replace YOUR_TOKEN and yourclub)
$token = "PASTE_YOUR_DUCKDNS_TOKEN_HERE"
$domain = "yourclub"

$script = @"
`$token = '$token'
`$domain = '$domain'
Invoke-WebRequest -Uri "https://www.duckdns.org/update?domains=`$domain&token=`$token" -UseBasicParsing | Out-Null
"@

$script | Out-File -FilePath "C:\Scripts\update-duckdns.ps1" -Encoding ASCII

Write-Host "Script created at C:\Scripts\update-duckdns.ps1"
```

**BEFORE pressing Enter:**
- Replace `PASTE_YOUR_DUCKDNS_TOKEN_HERE` with your actual token
- Replace `yourclub` with your actual subdomain

**Press `Enter`**

---

**7.5 Create Scheduled Task**

**In PowerShell, type:**

```powershell
$action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-WindowStyle Hidden -File C:\Scripts\update-duckdns.ps1"
$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Minutes 5)
$principal = New-ScheduledTaskPrincipal -UserId "SYSTEM" -LogonType ServiceAccount
Register-ScheduledTask -TaskName "DuckDNS Update" -Action $action -Trigger $trigger -Principal $principal -Force

Write-Host "Scheduled task created! DuckDNS will update every 5 minutes."
```

**Press `Enter`**

---

**7.6 Test It Works**

**In PowerShell, type:**

```powershell
nslookup yourclub.duckdns.org
```

(Replace `yourclub` with your actual subdomain)

**Press `Enter`**

You should see your public IP address!

**✅ SUCCESS!**

---

## ✅ CHECKPOINT: What You've Done So Far

- ✅ Set static IP (192.168.1.200)
- ✅ Configured firewall
- ✅ Installed FFmpeg
- ✅ Installed .NET 8
- ✅ Configured VPN server
- ✅ Set up port forwarding
- ✅ Set up DuckDNS

**You're 70% done!**

**Next:** Install Video Agent and configure web app

---

**Continue to Part 2?** (Video Agent installation and web app configuration)
