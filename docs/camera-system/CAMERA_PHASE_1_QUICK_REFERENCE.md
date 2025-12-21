# Phase 1 - Quick Reference Card

**Print this page and keep it handy during setup!**

---

## 🎯 Goal
Set up camera viewing with local AND remote access in 1.5 hours

---

## 📋 What You Need

### **Hardware:**
- [ ] Windows PC at club (8GB RAM, 4 cores)
- [ ] Access to router admin panel
- [ ] NVR at 192.168.1.64

### **Information:**
- [ ] NVR username: _______________
- [ ] NVR password: _______________
- [ ] Router admin login: _______________
- [ ] Public IP or DDNS hostname: _______________

---

## 🚀 Setup Steps (Copy-Paste Commands)

### **1. Set Static IP (10 min)**
```
IP: 192.168.1.100
Subnet: 255.255.255.0
Gateway: 192.168.1.1
DNS: 192.168.1.1
```

### **2. Firewall Rules (2 min)**
```powershell
New-NetFirewallRule -DisplayName "WireGuard VPN" -Direction Inbound -Protocol UDP -LocalPort 51820 -Action Allow
New-NetFirewallRule -DisplayName "GFC Video Agent" -Direction Inbound -Protocol TCP -LocalPort 5100 -Action Allow
New-NetFirewallRule -DisplayName "GFC Web App" -Direction Inbound -Protocol TCP -LocalPort 5000 -Action Allow
```

### **3. Install Software (20 min)**
```powershell
winget install ffmpeg
winget install WireGuard.WireGuard
winget install Microsoft.DotNet.SDK.8
```

### **4. Router Port Forwarding (5 min)**
```
External Port: 51820 (UDP)
Internal IP: 192.168.1.100
Internal Port: 51820 (UDP)
```

### **5. Generate API Key**
```powershell
[guid]::NewGuid().ToString()
```
**Copy this:** _________________________________

---

## 🔧 Configuration Files

### **Video Agent Config**
Location: `C:\GFC\VideoAgent\appsettings.json`
```json
{
  "NVR": {
    "IpAddress": "192.168.1.64",
    "Username": "admin",
    "Password": "YOUR_PASSWORD"
  },
  "Server": {
    "Port": 5100,
    "ApiKey": "YOUR_API_KEY"
  }
}
```

### **Web App Config**
Location: `C:\GFC\GFC-Studio\appsettings.json`
```json
{
  "VideoAgent": {
    "BaseUrl": "http://localhost:5100",
    "ApiKey": "YOUR_API_KEY"
  }
}
```

---

## ✅ Testing Checklist

### **Local Access:**
- [ ] Open: `http://localhost:5000`
- [ ] Login works
- [ ] Dashboard shows 🟢 green LED
- [ ] Click "Cameras" → Video plays

### **Remote Access:**
- [ ] VPN connects (green status)
- [ ] Can ping 192.168.1.100
- [ ] Open: `http://192.168.1.100:5000`
- [ ] Login works
- [ ] Cameras work

---

## 🐛 Quick Troubleshooting

| Problem | Fix |
|---------|-----|
| Red LED 🔴 | Restart Video Agent service |
| Yellow LED 🟡 | Check NVR is on, ping 192.168.1.64 |
| VPN won't connect | Check port 51820 forwarded |
| Can't access web app | Check firewall, restart web app |
| No video | Check Video Agent logs |

---

## 📞 Important Paths

```
Video Agent Logs: C:\GFC\VideoAgent\logs\
Video Agent Config: C:\GFC\VideoAgent\appsettings.json
WireGuard Config: C:\Program Files\WireGuard\wg0.conf
Web App Config: C:\GFC\GFC-Studio\appsettings.json
```

---

## 🎯 Daily Usage

### **At Club:**
1. Browser → `http://192.168.1.100:5000`
2. Login → Cameras

### **Remote:**
1. VPN → Activate
2. Browser → `http://192.168.1.100:5000`
3. Login → Cameras

---

## 📱 Client Config Template

```ini
[Interface]
PrivateKey = CLIENT_PRIVATE_KEY
Address = 10.0.0.2/24
DNS = 192.168.1.1

[Peer]
PublicKey = SERVER_PUBLIC_KEY
Endpoint = yourclub.ddns.net:51820
AllowedIPs = 192.168.1.0/24
PersistentKeepalive = 25
```

---

## ⏱️ Time Breakdown

- Prepare PC: 10 min
- Install software: 20 min
- Configure VPN: 15 min
- Router setup: 5 min
- DDNS: 10 min
- Video Agent: 15 min
- Client configs: 10 min
- Web app: 5 min
- Testing: 10 min

**Total: ~1.5 hours**

---

## 🔐 Security Checklist

- [ ] Only port 51820 forwarded
- [ ] NVR ports (554, 8000) NOT forwarded
- [ ] Credentials encrypted
- [ ] VPN using strong keys
- [ ] DDNS configured
- [ ] Firewall rules active

---

**Full Guide:** `CAMERA_PHASE_1_LOCAL_PC_SETUP.md`  
**Help:** Check logs in `C:\GFC\VideoAgent\logs\`
