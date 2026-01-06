# GFC Communication Flow - Quick Visual Reference

## 🎯 The Big Picture

```
┌─────────────┐
│   BROWSER   │ ← User interacts here
└──────┬──────┘
       │ HTTP/HTTPS + WebSocket (SignalR)
       │ Port: 8080 or 443
       ▼
┌─────────────────────────┐
│   WEB APPLICATION       │ ← Blazor Server (IIS)
│   • Authentication      │
│   • Business Logic      │
│   • UI Rendering        │
└──────┬──────────┬───────┘
       │          │
       │          │ HTTP REST + API Key
       │          │ Port: 5101
       │          ▼
       │   ┌──────────────┐
       │   │  AGENT API   │ ← Middleware
       │   └──────┬───────┘
       │          │ UDP Binary Protocol
       │          │ Port: 60000
       │          ▼
       │   ┌──────────────┐
       │   │ CONTROLLER   │ ← Door locks & card readers
       │   │ 192.168.1.72 │
       │   └──────────────┘
       │
       │ SQL/TDS Protocol
       │ Port: 1433
       ▼
┌─────────────┐
│ SQL SERVER  │ ← All data stored here
│ ClubMembership
└─────────────┘
```

## 🔑 Key Protocols

| Connection | Protocol | Port | Purpose |
|------------|----------|------|---------|
| Browser → Web App | HTTP/HTTPS | 8080/443 | Page delivery |
| Browser ↔ Web App | WebSocket | 8080/443 | Real-time UI (SignalR) |
| Web App → SQL | TDS (SQL) | 1433 | Database queries |
| Web App → Agent | HTTP REST | 5101 | Controller commands |
| Agent → Controller | UDP | 60000 | Low-level hardware control |

## 📊 Example: User Logs In

```
1. Browser sends: POST /login { username, password }
   ↓
2. Web App queries: SELECT * FROM AppUsers WHERE Username = ?
   ↓
3. SQL returns: { UserId: 5, PasswordHash: "...", IsActive: true }
   ↓
4. Web App verifies password (BCrypt)
   ↓
5. Web App creates session (in memory)
   ↓
6. Web App logs: INSERT INTO AuditLogs (Action, UserId, ...)
   ↓
7. Browser receives: { success: true, redirect: "/dashboard" }
```

**Time**: ~200ms

## 📊 Example: Admin Assigns Card

```
1. Browser: "Assign card 12345 to John Doe for doors 1,2"
   ↓
2. Web App: INSERT INTO KeyCards (CardNumber, MemberID, ...)
   ↓
3. Web App: INSERT INTO MemberDoorAccess (MemberId, DoorId, ...)
   ↓
4. Web App → Agent API: POST /api/controllers/223213880/cards
   ↓
5. Agent API → Controller: UDP packet [0x50, card data, ...]
   ↓
6. Controller: Stores card in memory, sends ACK
   ↓
7. Agent API → Web App: { success: true }
   ↓
8. Web App: INSERT INTO AuditLogs (Action: "KeyCardAdded", ...)
   ↓
9. Browser: "Card assigned successfully"
```

**Time**: ~2 seconds

## 📊 Example: Member Swipes Card

```
1. Member swipes card at door
   ↓
2. Card reader → Controller (Wiegand protocol)
   ↓
3. Controller checks: Is card 12345 allowed on door 1?
   ↓
4. Controller: YES → Unlock door for 5 seconds
   ↓
5. Controller logs event in internal buffer
   ↓
6. Web App polls (every 5 sec): GET /api/controllers/.../events
   ↓
7. Agent API → Controller: UDP packet [0xB0, get events, ...]
   ↓
8. Controller → Agent API: UDP response with events
   ↓
9. Agent API → Web App: JSON with events
   ↓
10. Web App: INSERT INTO DoorActivityLog (CardNumber, EventTime, ...)
    ↓
11. Web App → Browser (SignalR): "New event: John Doe entered"
    ↓
12. Browser updates table (no refresh needed)
```

**Time**: Event appears in web app within 5-6 seconds of swipe

## 🔐 Security Layers

```
Layer 1: User Authentication
├─ Username/password (BCrypt hashed)
├─ Optional MFA (TOTP)
└─ Session management (server-side)

Layer 2: Database Security
├─ Windows Authentication (no passwords in config)
├─ Localhost-only access
└─ Audit logging (all changes tracked)

Layer 3: Controller Security
├─ API key authentication (Web App → Agent)
├─ Serial number validation (Agent → Controller)
└─ Optional communication password

Layer 4: Network Security (Planned)
├─ Cloudflare Tunnel (HTTPS, no open ports)
└─ WireGuard VPN (encrypted tunnel)
```

## 🌐 Network Topology

### Current (Local Only)
```
192.168.1.50 (Server)
    ├─ IIS:8080 (Web App)
    ├─ SQL:1433 (Database)
    └─ Agent:5101 (Controller API)
        └─ UDP:60000 → 192.168.1.72 (Controller)

Access: Only from 192.168.1.x network
```

### With Cloudflare Tunnel (Planned)
```
Internet
    ↓ HTTPS
Cloudflare Edge
    ↓ Encrypted tunnel (outbound from server)
Server (cloudflared)
    ↓ localhost
IIS:8080
    ↓ localhost
SQL:1433

Access: From anywhere with internet
Security: No inbound ports, DDoS protection
```

### With WireGuard VPN (Planned)
```
Internet
    ↓ WireGuard (UDP:51820)
VPN Server (10.20.0.1)
    ↓ Routes to private network
Director (10.20.0.5)
    ↓ Full network access
Web App, Cameras, etc.

Access: From anywhere, requires VPN client
Security: Encrypted tunnel, full network access
```

## 📝 Port Summary

**Server Ports:**
- 1433: SQL Server (localhost only)
- 5101: Agent API (localhost only)
- 8080: IIS Web App (local network)
- 60000: UDP Controller (local network)
- 51820: WireGuard VPN (internet, planned)

**Controller Ports:**
- 60000: UDP/TCP for commands

**Client Ports:**
- Dynamic: Browser HTTP/WebSocket
- Dynamic: WireGuard VPN (planned)

## 🔄 Data Flow Patterns

### Pattern 1: User Action → Database
```
Browser → SignalR → Blazor Component → Service → SQL Server
```
Example: Update member phone number

### Pattern 2: User Action → Controller
```
Browser → SignalR → Blazor Component → Service → Agent API → Controller
```
Example: Open door remotely

### Pattern 3: User Action → Both
```
Browser → SignalR → Blazor Component → Service
    ├─→ SQL Server (store intent)
    └─→ Agent API → Controller (execute action)
```
Example: Assign key card

### Pattern 4: Controller → Database (Background)
```
Controller → Agent API → ControllerEventService → SQL Server → SignalR → Browser
```
Example: Card swipe event

## 🎯 Key Concepts

1. **SignalR = Real-time**: Browser stays connected via WebSocket for instant updates

2. **Two-tier Controller**: Web App → Agent API → Controller (for reliability)

3. **Polling**: Web app polls controller every 5 seconds for new events

4. **Audit Everything**: All actions logged to AuditLogs table

5. **Session-based**: User state stored server-side, tied to SignalR connection

6. **Windows Auth**: SQL Server uses Windows Authentication (no passwords)

7. **API Key**: Agent API requires X-Agent-Key header for security

---

**For complete details, see**: COMPLETE_COMMUNICATION_ARCHITECTURE.md
