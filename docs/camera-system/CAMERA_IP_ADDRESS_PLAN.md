# GFC Camera System - IP Address Plan

**Purpose:** Standardized IP addressing for all camera system components  
**Last Updated:** 2025-12-21

---

## 📋 Reserved IP Addresses

### **Local Network (192.168.1.x)**

| IP Address | Device/Service | Purpose | Status |
|------------|----------------|---------|--------|
| `192.168.1.1` | Router | Network gateway | Existing |
| `192.168.1.64` | NVR | Network Video Recorder | Existing |
| `192.168.1.200` | Video Agent PC | Camera stream conversion + VPN server | **Phase 1** |
| `192.168.1.201` | Reserved | Future expansion (e.g., backup Video Agent) | Reserved |
| `192.168.1.202` | Reserved | Future expansion | Reserved |
| `192.168.1.203` | Reserved | Future expansion | Reserved |

### **VPN Network (10.0.0.x)**

| IP Address | Device/Service | Purpose | Status |
|------------|----------------|---------|--------|
| `10.0.0.1` | VPN Server | WireGuard VPN endpoint (on 192.168.1.200) | **Phase 1** |
| `10.0.0.2` | Web App Server | Remote web app VPN client | **Phase 1** |
| `10.0.0.3` | Reserved | Future VPN client | Reserved |
| `10.0.0.4-10` | Reserved | Future VPN clients | Reserved |

---

## 🔧 Port Assignments

### **External (Public-Facing)**

| Port | Protocol | Service | Forwarded To | Purpose |
|------|----------|---------|--------------|---------|
| `51820` | UDP | WireGuard VPN | 192.168.1.200:51820 | Secure remote access |

**IMPORTANT:** Only port 51820 should be forwarded. All other ports stay closed!

### **Internal (LAN-Only)**

| Port | Protocol | Service | Device | Purpose |
|------|----------|---------|--------|---------|
| `554` | TCP | RTSP | 192.168.1.64 (NVR) | Camera streams |
| `8000` | TCP | HTTP API | 192.168.1.64 (NVR) | NVR management |
| `5100` | TCP | Video Agent API | 192.168.1.200 | Stream management |
| `51820` | UDP | WireGuard | 192.168.1.200 | VPN server |

---

## 🌐 DNS/Hostnames

| Hostname | Points To | Purpose | Provider |
|----------|-----------|---------|----------|
| `yourclub.ddns.net` | Your public IP | VPN endpoint | No-IP / DuckDNS |
| `192.168.1.200` | Video Agent PC | Local access | Static IP |
| `10.0.0.1` | VPN Server | VPN network access | VPN config |

---

## 📝 Configuration Examples

### **Router Port Forwarding**
```
Service Name: GFC Camera VPN
External Port: 51820 (UDP)
Internal IP: 192.168.1.200
Internal Port: 51820 (UDP)
Protocol: UDP
Status: Enabled
```

### **Video Agent PC Network Config**
```
IP Address: 192.168.1.200
Subnet Mask: 255.255.255.0
Default Gateway: 192.168.1.1
DNS Server: 192.168.1.1
```

### **VPN Server Config (wg0.conf)**
```ini
[Interface]
PrivateKey = <server_private_key>
Address = 10.0.0.1/24
ListenPort = 51820
```

### **Web App VPN Client Config**
```ini
[Interface]
PrivateKey = <client_private_key>
Address = 10.0.0.2/24

[Peer]
PublicKey = <server_public_key>
Endpoint = yourclub.ddns.net:51820
AllowedIPs = 192.168.1.0/24, 10.0.0.0/24
```

### **Web App appsettings.json**
```json
{
  "VideoAgent": {
    "BaseUrl": "http://10.0.0.1:5100",
    "ApiKey": "<your_api_key>"
  }
}
```

---

## 🔒 Security Rules

### **What's Accessible from Internet:**
- ✅ Port 51820 (VPN only) → 192.168.1.200

### **What's NOT Accessible from Internet:**
- ❌ Port 554 (RTSP) - NVR streams
- ❌ Port 8000 (HTTP) - NVR management
- ❌ Port 5100 (HTTP) - Video Agent API
- ❌ Any other ports

### **Access Flow:**
```
Internet → Port 51820 (VPN) → 192.168.1.200 (VPN Server)
                                     ↓
                              VPN Tunnel (10.0.0.x)
                                     ↓
                              Video Agent (10.0.0.1:5100)
                                     ↓
                              NVR (192.168.1.64)
```

---

## 🚀 Future Expansion

### **Phase 2+: Additional Services**

If you add more services in the future, use these reserved IPs:

| IP Address | Suggested Use |
|------------|---------------|
| `192.168.1.201` | Backup/Failover Video Agent |
| `192.168.1.202` | Dedicated recording server |
| `192.168.1.203` | Analytics/AI processing server |
| `10.0.0.3-10` | Additional VPN clients |

---

## ✅ Verification Commands

### **Check Local IP**
```cmd
ipconfig
```
Look for: `IPv4 Address: 192.168.1.200`

### **Check VPN Server**
```powershell
# In WireGuard app, should show:
# Interface: wg0
# Public key: <your_key>
# Listening port: 51820
```

### **Test NVR Connectivity**
```cmd
ping 192.168.1.64
```

### **Test Video Agent**
```cmd
curl http://192.168.1.200:5100/api/heartbeat
```

### **Test VPN from Web App Server**
```bash
ping 10.0.0.1
curl http://10.0.0.1:5100/api/heartbeat
```

---

## 📊 Network Diagram

```
┌─────────────────────────────────────────────────────────────┐
│  Internet                                                   │
│  ↓ Port 51820 (VPN)                                        │
└─────────────────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────────────────┐
│  Router (192.168.1.1)                                       │
│  - Port forwarding: 51820 → 192.168.1.200                  │
└─────────────────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────────────────┐
│  Local Network (192.168.1.x)                                │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Video Agent PC (192.168.1.200)                     │   │
│  │  - WireGuard VPN Server (10.0.0.1)                  │   │
│  │  - Video Agent Service (port 5100)                  │   │
│  └─────────────────────────────────────────────────────┘   │
│                    ↓                                        │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  NVR (192.168.1.64)                                 │   │
│  │  - RTSP (port 554)                                  │   │
│  │  - HTTP API (port 8000)                             │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔄 Change Log

| Date | Change | Reason |
|------|--------|--------|
| 2025-12-21 | Standardized Video Agent PC to 192.168.1.200 | Avoid DHCP conflicts, future-proof |
| 2025-12-21 | Reserved 192.168.1.201-203 | Future expansion |
| 2025-12-21 | Defined VPN network as 10.0.0.0/24 | Separate VPN addressing |

---

## 📞 Quick Reference

**Video Agent PC:** `192.168.1.200`  
**VPN Server (internal):** `10.0.0.1`  
**NVR:** `192.168.1.64`  
**VPN Port:** `51820` (UDP)  
**Video Agent API:** `5100` (TCP)  

**Remember:** Only port 51820 is exposed to the internet!

---

**Use this document as the single source of truth for all IP addressing in the GFC Camera System.**
