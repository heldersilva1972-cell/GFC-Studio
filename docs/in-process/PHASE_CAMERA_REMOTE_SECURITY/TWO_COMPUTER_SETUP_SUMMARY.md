# 🎯 TWO-COMPUTER SETUP - QUICK SUMMARY

**Version:** 1.0.1  
**Last Updated:** December 26, 2025  
**Status:** ✅ INFRASTRUCTURE COMPLETED

## 📜 REVISION HISTORY

| Date | Version | Author | Description |
|:---|:---|:---|:---|
| 2025-12-24 | 1.0.0 | Jules (AI Agent) | Initial quick summary creation |
| 2025-12-26 | 1.0.1 | Jules (AI Agent) | Updated status and moved to complete folder |

---

---

## 📍 COMPUTER A: Club Location

**Location:** At the club/business  
**Purpose:** Connect to cameras and stream video  
**Must be:** Always on, always connected to club network

**Runs:**
- ✅ Video Agent (streams camera footage)
- ✅ WireGuard VPN Server (accepts connections from home)
- ✅ VPN IP: 10.8.0.1

**Setup Guide:** [SETUP_GUIDE_1A_CLUB_COMPUTER.md](./SETUP_GUIDE_1A_CLUB_COMPUTER.md)

---

## 🏠 COMPUTER B: Your Home

**Location:** Your home  
**Purpose:** Host the Web App, allow users to log in  
**Must be:** Always on, always connected to internet

**Runs:**
- ✅ GFC Web App (user login, permissions, UI)
- ✅ WireGuard VPN Client (connects to club)
- ✅ Cloudflare Tunnel (hides your home IP)
- ✅ VPN IP: 10.8.0.2

**Setup Guides:**
1. [SETUP_GUIDE_1B_HOME_COMPUTER.md](./SETUP_GUIDE_1B_HOME_COMPUTER.md)
2. [SETUP_GUIDE_2_CLOUDFLARE_TUNNEL.md](./SETUP_GUIDE_2_CLOUDFLARE_TUNNEL.md)

---

## 🔐 How They Connect

```
┌──────────────────────────────────────────────────────────┐
│                      INTERNET                            │
│                                                          │
│  Users connect here ↓                                    │
│  https://cameras.yourclub.com                            │
│         ↓                                                │
│  ┌─────────────────────────────────────────────┐         │
│  │  Cloudflare (hides your home IP)            │         │
│  └──────────────────┬──────────────────────────┘         │
│                     │                                    │
└─────────────────────┼────────────────────────────────────┘
                      │
                      ↓
┌──────────────────────────────────────────────────────────┐
│              YOUR HOME (Computer B)                      │
│                                                          │
│  ┌────────────────────────────────────────────────┐      │
│  │  GFC Web App                                   │      │
│  │  • Users log in here                           │      │
│  │  • Manages permissions                         │      │
│  │  • Generates user VPN profiles                 │      │
│  └────────────────────────────────────────────────┘      │
│                     │                                    │
│                     │ Encrypted VPN Tunnel               │
│                     │ (Site-to-Site)                     │
│  ┌──────────────────▼───────────────────────────┐        │
│  │  WireGuard Client (10.8.0.2)                 │        │
│  └──────────────────────────────────────────────┘        │
│                                                          │
└──────────────────────┬───────────────────────────────────┘
                       │
                       │ Always-On VPN Connection
                       │
                       ↓
┌──────────────────────────────────────────────────────────┐
│              CLUB LOCATION (Computer A)                  │
│                                                          │
│  ┌──────────────────────────────────────────────┐        │
│  │  WireGuard Server (10.8.0.1)                 │        │
│  └──────────────────┬───────────────────────────┘        │
│                     │                                    │
│  ┌──────────────────▼───────────────────────────┐        │
│  │  Video Agent                                 │        │
│  │  • Streams camera footage                    │        │
│  │  • Only accepts LAN + VPN connections        │        │
│  └──────────────────┬───────────────────────────┘        │
│                     │                                    │
│  ┌──────────────────▼───────────────────────────┐        │
│  │  NVR + Cameras                               │        │
│  │  • Never exposed to internet                 │        │
│  └──────────────────────────────────────────────┘        │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## 🔒 Security Layers

**10 Layers of Protection:**

1. **Cloudflare DDoS Protection** - Blocks attacks before they reach you
2. **Cloudflare Tunnel** - Hides your home IP address
3. **Web App Authentication** - Username/password login
4. **Network Location Detection** - Checks if user is on VPN
5. **User VPN (WireGuard)** - Remote users connect via VPN
6. **Role-Based Permissions** - Only authorized users see video
7. **Site-to-Site VPN** - Encrypted tunnel between home & club ← **NEW!**
8. **Stream Token Validation** - 60-second expiring tokens
9. **Video Agent IP Filtering** - Only accepts LAN + VPN traffic
10. **NVR Isolation** - Never exposed to internet

---

## 📊 Setup Sequence

### Phase 0: Infrastructure Setup (3-4 hours total)

**Step 1: Club Computer (60-90 minutes)**
- Follow SETUP_GUIDE_1A_CLUB_COMPUTER.md
- Install WireGuard as VPN server
- Configure firewall and port forwarding
- Keep computer on 24/7

**Step 2: Home Computer (45-60 minutes)**
- Follow SETUP_GUIDE_1B_HOME_COMPUTER.md
- Install WireGuard as VPN client
- Connect to club computer
- Configure Web App to use VPN

**Step 3: Cloudflare Tunnel (30-45 minutes)**
- Follow SETUP_GUIDE_2_CLOUDFLARE_TUNNEL.md
- Set up free account
- Choose domain option
- Enable external access

**Step 4: Test Everything (15 minutes)**
- Verify VPN tunnel is connected
- Test Web App can reach Video Agent
- Test external access via Cloudflare
- Verify cameras load

---

## ✅ Success Criteria

After setup, you should have:

- ✅ Club computer always on, VPN active
- ✅ Home computer always on, VPN connected
- ✅ "Latest handshake" shows recent time (< 1 minute)
- ✅ Ping from home to club (10.8.0.1) works
- ✅ Web App can reach Video Agent health check
- ✅ External users can access via Cloudflare URL
- ✅ Cameras load in the Web App

---

## 🎯 Dashboard Monitoring

Once implemented, your main dashboard will show:

```
📡 SYSTEM STATUS

🟢 Club Computer: ONLINE
   Uptime: 3d 14h 23m
   Last Check: 5 seconds ago

🟢 Video Agent: STREAMING
   Active Cameras: 15/16
   Active Viewers: 2

🟢 VPN Tunnel: CONNECTED
   Latency: 12ms
   Bandwidth: 2.4 Mbps
```

**If club computer goes offline:**
```
🔴 ALERT: Club Computer OFFLINE
   Last Seen: 15 minutes ago
   Impact: Video access unavailable
   [Troubleshoot] [View History]
```

---

## 💡 Key Differences from Single-Computer Setup

| Aspect | Single Computer | Two Computers (Your Setup) |
|--------|----------------|----------------------------|
| **Complexity** | Simple | Moderate |
| **Security** | 9 layers | 10 layers (more secure!) |
| **Flexibility** | Limited | High (can move Web App) |
| **Reliability** | Single point of failure | Distributed |
| **Cost** | $0-15/year | $0-15/year (same!) |
| **Setup Time** | 2 hours | 3-4 hours |

---

## 🔄 What Happens When...

### Club Computer Goes Offline
- ❌ Video access stops working
- ✅ Web App still accessible (login works)
- ✅ Dashboard shows "Club Computer OFFLINE"
- ✅ Email alert sent to admin
- ✅ Users see "Video temporarily unavailable"

### Home Computer Goes Offline
- ❌ Web App stops working (can't log in)
- ❌ Video access stops working
- ✅ Club computer keeps running (ready for when home comes back)

### VPN Tunnel Disconnects
- ✅ Auto-reconnect attempts every 25 seconds
- ✅ Dashboard shows "VPN Tunnel: RECONNECTING"
- ✅ Usually reconnects within 1 minute
- ❌ Video access unavailable during reconnection

### Internet Goes Down at Club
- ❌ VPN tunnel disconnects
- ❌ Video access stops working
- ✅ Dashboard shows "Club Computer OFFLINE"
- ✅ Automatically reconnects when internet returns

### Internet Goes Down at Home
- ❌ Web App inaccessible from outside
- ❌ VPN tunnel disconnects
- ✅ Automatically reconnects when internet returns

---

## 📞 Quick Troubleshooting

### VPN Won't Connect
1. Check both computers are on
2. Check "Latest handshake" on both sides
3. Verify port 51820 is open on club router
4. Restart WireGuard on both computers

### Can't Ping 10.8.0.1
1. Verify VPN shows "Active" on both computers
2. Check firewall on club computer
3. Verify club computer added home as peer

### Video Agent Unreachable
1. Verify ping to 10.8.0.1 works
2. Check Video Agent is running on club computer
3. Verify Web App config uses http://10.8.0.1:5001

---

## 🎉 Benefits of This Setup

✅ **More Secure** - Extra VPN layer between computers  
✅ **More Flexible** - Can move Web App to cloud later  
✅ **Better Monitoring** - See status of both computers  
✅ **Easier Troubleshooting** - Can isolate issues  
✅ **Professional** - Industry-standard architecture  

---

**Ready to begin? Start with SETUP_GUIDE_1A_CLUB_COMPUTER.md!**
