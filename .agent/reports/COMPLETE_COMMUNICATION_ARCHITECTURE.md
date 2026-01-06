# GFC System - Complete Communication Architecture

**Generated**: 2026-01-05 20:15 EST  
**Purpose**: Explain ALL communication flows in the GFC system

---

## 📋 Table of Contents

1. [System Overview](#system-overview)
2. [User Login Flow](#user-login-flow)
3. [Web App ↔ Database Communication](#web-app--database-communication)
4. [Web App ↔ Controller Communication](#web-app--controller-communication)
5. [Network Architecture](#network-architecture)
6. [Port Usage](#port-usage)
7. [Data Flow Examples](#data-flow-examples)
8. [VPN Integration (Planned)](#vpn-integration-planned)

---

## 🏗️ System Overview

The GFC system has **THREE** main components:

```
┌─────────────────────────────────────────────────────────────────┐
│                         USER'S BROWSER                           │
│                    (Chrome, Safari, Edge)                        │
└────────────────────────┬────────────────────────────────────────┘
                         │ HTTPS (Port 443) or HTTP (Port 8080)
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│                      GFC WEB APPLICATION                         │
│                    (Blazor Server on IIS)                        │
│                                                                  │
│  Components:                                                     │
│  • Authentication Service                                        │
│  • Member Management                                             │
│  • Key Card Management                                           │
│  • Controller Communication                                      │
│  • Camera Access                                                 │
└──────────┬──────────────────────────────────┬───────────────────┘
           │                                  │
           │ SQL (Port 1433)                  │ UDP (Port 60000)
           ▼                                  ▼
┌──────────────────────┐         ┌──────────────────────────────┐
│   SQL SERVER         │         │   ACCESS CONTROLLER          │
│  (ClubMembership DB) │         │   (Mengqi N3000)             │
│                      │         │   IP: 192.168.1.72           │
│  Tables:             │         │   Serial: 223213880          │
│  • AppUsers          │         │                              │
│  • Members           │         │   Manages:                   │
│  • KeyCards          │         │   • Door locks               │
│  • MemberDoorAccess  │         │   • Card readers             │
│  • AuditLogs         │         │   • Access events            │
└──────────────────────┘         └──────────────────────────────┘
```

---

## 🔐 User Login Flow

### Step-by-Step: What Happens When a User Logs In

#### 1. **User Opens Browser**
```
User types: http://localhost:8080 (dev) or https://gfc.lovanow.com (prod)
```

#### 2. **Browser Connects to Web Server**
- **Protocol**: HTTP or HTTPS
- **Port**: 8080 (IIS) or 443 (Cloudflare Tunnel)
- **Connection**: Direct TCP connection
- **What happens**: Browser downloads HTML, CSS, JavaScript, Blazor runtime

#### 3. **Blazor Establishes SignalR Connection**
```
Browser → WebSocket Connection → IIS
Protocol: WebSocket (ws:// or wss://)
Purpose: Real-time communication for Blazor Server
```

**Why WebSocket?** Blazor Server uses SignalR to maintain a persistent connection. Every UI interaction (button click, form submit) sends a message through this WebSocket to the server, which processes it and sends back UI updates.

#### 4. **User Enters Credentials**
```
Username: admin
Password: Admin123!
```

User clicks "Login" button.

#### 5. **Login Request Flows Through System**

```
┌─────────────┐
│   Browser   │
└──────┬──────┘
       │ SignalR message: "LoginButtonClicked"
       ▼
┌─────────────────────────────┐
│   Blazor Server (IIS)       │
│   Login.razor component     │
└──────┬──────────────────────┘
       │ Calls AuthenticationService.LoginAsync()
       ▼
┌─────────────────────────────┐
│  AuthenticationService.cs   │
│  (GFC.Core)                 │
└──────┬──────────────────────┘
       │ Queries database
       ▼
┌─────────────────────────────┐
│   SQL Server                │
│   Query: SELECT * FROM      │
│   AppUsers WHERE            │
│   Username = 'admin'        │
└──────┬──────────────────────┘
       │ Returns user record
       ▼
┌─────────────────────────────┐
│  AuthenticationService.cs   │
│  • Verifies password hash   │
│  • Checks if user is active │
│  • Creates session          │
└──────┬──────────────────────┘
       │ Returns LoginResult
       ▼
┌─────────────────────────────┐
│   Login.razor               │
│   • Stores user in session  │
│   • Redirects to Dashboard  │
└──────┬──────────────────────┘
       │ SignalR: Navigate("/dashboard")
       ▼
┌─────────────┐
│   Browser   │
│   Shows     │
│   Dashboard │
└─────────────┘
```

#### 6. **Session is Established**
- **Where stored**: Server-side in memory (Blazor Server maintains state)
- **How identified**: SignalR connection ID
- **Duration**: Until user logs out or session times out

---

## 💾 Web App ↔ Database Communication

### Connection Details

**Connection String** (from appsettings.json):
```
Server=.;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;
```

**Breakdown**:
- `Server=.` = Local SQL Server instance
- `Database=ClubMembership` = Database name
- `Trusted_Connection=True` = Windows Authentication (no username/password)
- `TrustServerCertificate=True` = Accept self-signed certs
- `Encrypt=False` = No TLS encryption (localhost only)

**Protocol**: TDS (Tabular Data Stream) over TCP
**Port**: 1433 (SQL Server default)
**Authentication**: Windows Authentication (app pool identity)

### Example: User Updates Member Information

```
┌──────────────────────────────────────────────────────────────┐
│ SCENARIO: Admin changes member's phone number               │
└──────────────────────────────────────────────────────────────┘

1. Admin navigates to Members page
   Browser → SignalR → MembersPage.razor loads

2. Admin clicks "Edit" on member John Doe
   Browser → SignalR → EditMember.razor opens
   
3. Web app queries database for member details:
   
   MemberService.cs → SQL Server
   Query: SELECT * FROM Members WHERE MemberID = 123
   
   SQL Server → MemberService.cs
   Returns: { MemberID: 123, Name: "John Doe", Phone: "555-1234", ... }
   
4. EditMember.razor displays form with current data
   SignalR → Browser
   Browser shows form with "555-1234" in phone field

5. Admin changes phone to "555-9999" and clicks "Save"
   Browser → SignalR → EditMember.razor.OnSaveClicked()
   
6. Web app updates database:
   
   MemberService.cs → SQL Server
   Query: UPDATE Members 
          SET Phone = '555-9999', LastModified = GETUTCDATE()
          WHERE MemberID = 123
   
   SQL Server executes update
   SQL Server → MemberService.cs
   Returns: 1 row affected

7. Web app logs the change:
   
   AuditLogger.cs → SQL Server
   Query: INSERT INTO AuditLogs (Action, PerformedByUserId, Details, TimestampUtc)
          VALUES ('MemberUpdated', 1, 'Changed phone for MemberID 123', GETUTCDATE())
   
8. Web app shows success message
   MemberService.cs → EditMember.razor → SignalR → Browser
   Browser shows: "Member updated successfully"
```

**Key Points**:
- **Every database operation** goes through Entity Framework Core or direct SQL
- **Connection pooling**: Connections are reused for performance
- **Transactions**: Updates can be wrapped in transactions for consistency
- **Audit trail**: All changes are logged to AuditLogs table

---

## 🎛️ Web App ↔ Controller Communication

### Architecture: Two-Tier System

The GFC system uses a **two-tier architecture** for controller communication:

```
┌─────────────────────────────────────────────────────────────┐
│                    GFC WEB APPLICATION                       │
│                    (Blazor Server on IIS)                    │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ HTTP REST API (Port 5101)
                         │ Header: X-Agent-Key: GFC-ACCESS-CONTROL-SECRET-KEY-2025
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    AGENT API SERVICE                         │
│                    (localhost:5101)                          │
│                                                              │
│  Purpose: Middleware between web app and controller         │
│  Why: Isolates UDP communication complexity                 │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ UDP Packets (Port 60000)
                         │ Proprietary Mengqi protocol
                         ▼
┌─────────────────────────────────────────────────────────────┐
│              ACCESS CONTROLLER (Mengqi N3000)                │
│              IP: 192.168.1.72                                │
│              Serial: 223213880                               │
└─────────────────────────────────────────────────────────────┘
```

### Why Two Tiers?

1. **Complexity Isolation**: UDP communication with proprietary protocol is complex
2. **Reliability**: Agent API handles retries, timeouts, packet assembly
3. **Security**: API key authentication between web app and agent
4. **Scalability**: Multiple web app instances can share one agent

### Communication Protocols

#### Tier 1: Web App → Agent API
- **Protocol**: HTTP REST
- **Port**: 5101
- **Authentication**: API Key in `X-Agent-Key` header
- **Format**: JSON
- **Example Request**:
  ```http
  POST https://localhost:5101/api/controllers/223213880/door/1/open
  X-Agent-Key: GFC-ACCESS-CONTROL-SECRET-KEY-2025
  Content-Type: application/json
  
  {
    "DurationSec": 5
  }
  ```

#### Tier 2: Agent API → Controller
- **Protocol**: UDP (custom Mengqi protocol)
- **Port**: 60000 (both source and destination)
- **Format**: Binary packets (64 bytes)
- **Authentication**: Controller serial number + optional password
- **Example Packet** (Open Door command):
  ```
  Byte 0: 0x17 (Command: Open Door)
  Byte 1: 0x01 (Door number)
  Byte 2-3: CRC16 (calculated from source port + data)
  Byte 4-7: Serial number (223213880)
  Byte 8-63: Additional data
  ```

### Configuration (appsettings.json)

```json
{
  "AgentApi": {
    "BaseUrl": "https://localhost:5101",
    "ApiKey": "GFC-ACCESS-CONTROL-SECRET-KEY-2025",
    "RequestTimeoutSeconds": 10,
    "Controllers": [
      {
        "SerialNumber": 223213880,
        "IpAddress": "192.168.1.72",
        "UdpPort": 60000,
        "TcpPort": 60000,
        "CommPassword": ""
      }
    ]
  }
}
```

---

## 📊 Data Flow Examples

### Example 1: Admin Assigns Key Card to Member

```
┌──────────────────────────────────────────────────────────────┐
│ SCENARIO: Admin assigns card #12345 to member John Doe      │
└──────────────────────────────────────────────────────────────┘

STEP 1: Admin clicks "Assign Card" on Key Cards page
├─ Browser → SignalR → KeyCardsPage.razor
└─ Opens AssignCardDialog component

STEP 2: Admin selects member and enters card number
├─ Browser shows form
└─ Admin fills: Member = "John Doe", Card = "12345", Doors = [1, 2]

STEP 3: Admin clicks "Save"
├─ Browser → SignalR → AssignCardDialog.OnSave()
└─ Calls KeyCardService.AssignCardAsync()

STEP 4: Web app updates database
├─ KeyCardService → SQL Server
│  Query 1: INSERT INTO KeyCards (CardNumber, MemberID, IsActive, AssignedDate)
│            VALUES ('12345', 123, 1, GETUTCDATE())
│  
│  Query 2: INSERT INTO MemberDoorAccess (MemberId, DoorId, CardNumber, TimeProfileId)
│            VALUES (123, 1, '12345', 1), (123, 2, '12345', 1)
│
└─ SQL Server confirms inserts

STEP 5: Web app syncs to controller
├─ KeyCardService → AgentApiClient.AddOrUpdateCardAsync()
│  
│  AgentApiClient → HTTP POST to Agent API
│  URL: https://localhost:5101/api/controllers/223213880/cards
│  Body: {
│    "CardNumber": "12345",
│    "Doors": [
│      { "DoorNo": 1, "TimeProfileId": 1 },
│      { "DoorNo": 2, "TimeProfileId": 1 }
│    ]
│  }
│  
│  Agent API → UDP packet to controller (192.168.1.72:60000)
│  Command: 0x50 (Add/Update Card)
│  Data: Card number, door permissions, time profile
│  
│  Controller receives packet
│  Controller stores card in memory
│  Controller → UDP response packet
│  
│  Agent API receives response
│  Agent API → HTTP 200 OK to web app
│
└─ KeyCardService receives success

STEP 6: Web app logs the action
├─ AuditLogger → SQL Server
│  INSERT INTO AuditLogs (Action, PerformedByUserId, Details, TimestampUtc)
│  VALUES ('KeyCardAdded', 1, 'Card 12345 assigned to John Doe (MemberID 123)', GETUTCDATE())
│
└─ SQL Server confirms

STEP 7: Web app shows success message
├─ KeyCardService → AssignCardDialog → SignalR → Browser
└─ Browser shows: "Card assigned and synced to controller successfully"

STEP 8: Background sync updates status
├─ ControllerSyncService (background job) queries controller
│  AgentApiClient → Agent API → Controller
│  Verifies card is actually stored
│  
│  Updates database:
│  UPDATE MemberDoorAccess 
│  SET LastSyncedAt = GETUTCDATE(), LastSyncResult = 'Success'
│  WHERE CardNumber = '12345'
│
└─ Database updated
```

**Timeline**:
- Steps 1-3: ~1 second (user interaction)
- Step 4: ~50ms (database writes)
- Step 5: ~200ms (controller communication)
- Step 6: ~20ms (audit log)
- Step 7: ~50ms (UI update)
- Step 8: ~500ms (background verification)

**Total**: ~2 seconds from click to confirmation

---

### Example 2: Member Swipes Card at Door

```
┌──────────────────────────────────────────────────────────────┐
│ SCENARIO: John Doe swipes card #12345 at Door 1             │
└──────────────────────────────────────────────────────────────┘

STEP 1: Member swipes card at reader
├─ Card reader reads card number: 12345
└─ Card reader sends to controller via Wiegand protocol

STEP 2: Controller processes card
├─ Controller checks internal memory for card 12345
├─ Finds: Card 12345, Doors [1, 2], Time Profile 1
├─ Checks current time against Time Profile 1
├─ Decision: GRANT ACCESS (time is within allowed hours)
└─ Controller activates door relay for 5 seconds

STEP 3: Door unlocks
├─ Relay energizes
├─ Door lock releases
├─ Member opens door
└─ Door closes after 5 seconds

STEP 4: Controller logs event
├─ Controller stores event in internal buffer
│  Event: { CardNumber: 12345, Door: 1, Time: 2026-01-05 20:15:33, Result: Access Granted }
└─ Event counter increments

STEP 5: Web app polls for events (background job)
├─ ControllerEventService runs every 5 seconds
│  
│  AgentApiClient → HTTP GET to Agent API
│  URL: https://localhost:5101/api/controllers/223213880/events?lastIndex=1234
│  
│  Agent API → UDP packet to controller
│  Command: 0xB0 (Get Events)
│  Data: Last known event index
│  
│  Controller → UDP response with new events
│  Data: [{ Index: 1235, CardNumber: 12345, Door: 1, Time: ..., Result: 1 }]
│  
│  Agent API → HTTP 200 with JSON events
│  
│  ControllerEventService receives events
│
└─ Processes each event

STEP 6: Web app stores event in database
├─ ControllerEventService → SQL Server
│  INSERT INTO DoorActivityLog (ControllerId, DoorId, CardNumber, EventTime, EventType, MemberId)
│  VALUES (1, 1, '12345', '2026-01-05 20:15:33', 'AccessGranted', 123)
│
└─ SQL Server confirms

STEP 7: Web app updates UI (if admin is watching)
├─ If admin has Door Activity page open
│  ControllerEventService → SignalR Hub → All connected clients
│  Message: "NewDoorEvent"
│  Data: { Door: "Main Entrance", Member: "John Doe", Time: "8:15 PM", Result: "Granted" }
│  
│  Browser receives SignalR message
│  Browser updates Door Activity table
│  New row appears: "John Doe - Main Entrance - 8:15 PM - ✓ Granted"
│
└─ Real-time update complete
```

**Timeline**:
- Step 1: Instant (card swipe)
- Step 2: ~50ms (controller processing)
- Step 3: 5 seconds (door unlock duration)
- Step 4: ~10ms (controller logging)
- Step 5: 0-5 seconds (next poll cycle)
- Step 6: ~30ms (database insert)
- Step 7: ~50ms (SignalR broadcast)

**Total**: Event appears in web app within 5-6 seconds of card swipe

---

## 🌐 Network Architecture

### Current Setup (Local Network)

```
┌─────────────────────────────────────────────────────────────┐
│                    LOCAL NETWORK (192.168.1.x)               │
│                                                              │
│  ┌──────────────┐         ┌──────────────┐                  │
│  │  Admin PC    │         │  Server PC   │                  │
│  │  Browser     │────────▶│  IIS:8080    │                  │
│  │              │  HTTP   │  SQL:1433    │                  │
│  └──────────────┘         │  Agent:5101  │                  │
│                           └───────┬──────┘                  │
│                                   │ UDP:60000                │
│                                   ▼                          │
│                           ┌──────────────┐                  │
│                           │ Controller   │                  │
│                           │ 192.168.1.72 │                  │
│                           └──────────────┘                  │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

**Access**: Only works on local network (192.168.1.x)

### With Cloudflare Tunnel (Planned)

```
┌──────────────────┐
│  User's Phone    │
│  (Anywhere)      │
└────────┬─────────┘
         │ HTTPS
         ▼
┌─────────────────────────────────────────────────────────────┐
│              CLOUDFLARE EDGE NETWORK                         │
│              (Global CDN)                                    │
└────────────────────┬────────────────────────────────────────┘
                     │ Encrypted Tunnel (outbound from server)
                     ▼
┌─────────────────────────────────────────────────────────────┐
│                    LOCAL NETWORK (192.168.1.x)               │
│                                                              │
│  ┌──────────────────────────────────────┐                   │
│  │  Server PC                           │                   │
│  │  • cloudflared service (tunnel)      │                   │
│  │  • IIS:8080 (localhost only)         │                   │
│  │  • SQL:1433 (localhost only)         │                   │
│  │  • Agent:5101 (localhost only)       │                   │
│  └────────────────┬─────────────────────┘                   │
│                   │ UDP:60000 (local network)                │
│                   ▼                                          │
│           ┌──────────────┐                                  │
│           │ Controller   │                                  │
│           │ 192.168.1.72 │                                  │
│           └──────────────┘                                  │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

**Access**: Works from anywhere with internet connection

**Security**:
- No inbound ports open on server
- Controller still only accessible on local network
- Cloudflare provides DDoS protection

### With WireGuard VPN (Planned)

```
┌──────────────────┐
│  Director's      │
│  Phone           │
│  (Anywhere)      │
└────────┬─────────┘
         │ WireGuard VPN
         │ Encrypted tunnel to 10.20.0.1
         ▼
┌─────────────────────────────────────────────────────────────┐
│              WIREGUARD VPN SERVER                            │
│              (On GFC Server or separate box)                 │
│              10.20.0.1                                       │
└────────────────────┬────────────────────────────────────────┘
                     │ Routes to private network
                     ▼
┌─────────────────────────────────────────────────────────────┐
│                    LOCAL NETWORK (192.168.1.x)               │
│                                                              │
│  Director's device appears as 10.20.0.5                     │
│  Can access:                                                 │
│  • Web app (via Cloudflare or direct)                       │
│  • Cameras (via NVR)                                         │
│  • Other internal services                                   │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

**Access**: Requires VPN connection, then full network access

---

## 🔌 Port Usage

### Server Ports

| Port | Protocol | Service | Purpose | Accessibility |
|------|----------|---------|---------|---------------|
| **1433** | TCP | SQL Server | Database | Localhost only |
| **5101** | TCP | Agent API | Controller middleware | Localhost only |
| **8080** | TCP | IIS | Web application | Local network |
| **60000** | UDP | UDP Transport | Controller communication | Local network |
| **51820** | UDP | WireGuard | VPN server (planned) | Internet |

### Controller Ports

| Port | Protocol | Service | Purpose |
|------|----------|---------|---------|
| **60000** | UDP | Mengqi Protocol | Commands & events |
| **60000** | TCP | Mengqi Protocol | Firmware updates (unused) |

### Client Ports

| Port | Protocol | Service | Purpose |
|------|----------|---------|---------|
| **Dynamic** | TCP | Browser | HTTPS to web app |
| **Dynamic** | TCP | SignalR | WebSocket to Blazor |
| **Dynamic** | UDP | WireGuard | VPN tunnel (planned) |

---

## 🔄 Complete User Journey Example

### Scenario: Director Logs In and Views Door Activity

```
┌──────────────────────────────────────────────────────────────┐
│ COMPLETE FLOW: Director logs in and checks who entered today│
└──────────────────────────────────────────────────────────────┘

═══════════════════════════════════════════════════════════════
PHASE 1: NETWORK CONNECTION
═══════════════════════════════════════════════════════════════

1. Director opens browser on phone (connected to WiFi)
   Phone IP: 192.168.1.100
   
2. Director types: http://192.168.1.50:8080
   (Server IP on local network)
   
3. Browser sends HTTP request
   Source: 192.168.1.100:54321 (random port)
   Destination: 192.168.1.50:8080
   Protocol: TCP
   
4. IIS receives request
   Binds to: 0.0.0.0:8080 (all interfaces)
   Accepts connection from 192.168.1.100
   
5. IIS serves login page
   Sends: HTML, CSS, JavaScript, Blazor runtime
   Size: ~2 MB
   Time: ~500ms
   
6. Browser establishes SignalR WebSocket
   Upgrade: HTTP → WebSocket
   Connection: ws://192.168.1.50:8080/_blazor
   Purpose: Real-time UI updates

═══════════════════════════════════════════════════════════════
PHASE 2: AUTHENTICATION
═══════════════════════════════════════════════════════════════

7. Director enters credentials
   Username: director1
   Password: Director123!
   
8. Browser sends login via SignalR
   SignalR message → IIS → Login.razor → AuthenticationService
   
9. AuthenticationService queries database
   Connection: localhost:1433 (SQL Server)
   Protocol: TDS over TCP
   Auth: Windows Authentication (IIS app pool identity)
   
   Query:
   SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MfaEnabled
   FROM AppUsers
   WHERE Username = 'director1'
   
10. SQL Server returns user record
    Result: { UserId: 5, Username: "director1", PasswordHash: "$2a$...", IsActive: true }
    
11. AuthenticationService verifies password
    Uses BCrypt to compare password with hash
    Result: Match ✓
    
12. AuthenticationService creates session
    Stores user object in server memory
    Associated with SignalR connection ID
    
13. AuthenticationService logs login
    INSERT INTO LoginHistory (UserId, LoginDate, IpAddress, LoginSuccessful)
    VALUES (5, GETUTCDATE(), '192.168.1.100', 1)
    
14. AuthenticationService logs audit
    INSERT INTO AuditLogs (Action, PerformedByUserId, Details, TimestampUtc)
    VALUES ('LoginSuccessPassword', 5, 'IP: 192.168.1.100', GETUTCDATE())
    
15. Login.razor redirects to dashboard
    SignalR → Browser: Navigate("/dashboard")
    Browser loads Dashboard.razor

═══════════════════════════════════════════════════════════════
PHASE 3: LOADING DASHBOARD
═══════════════════════════════════════════════════════════════

16. Dashboard.razor loads
    Calls DashboardService.GetDashboardDataAsync()
    
17. DashboardService queries database (multiple queries)
    
    Query 1: Get member count
    SELECT COUNT(*) FROM Members WHERE IsActive = 1
    Result: 150
    
    Query 2: Get active key cards
    SELECT COUNT(*) FROM KeyCards WHERE IsActive = 1
    Result: 145
    
    Query 3: Get today's door activity
    SELECT COUNT(*) FROM DoorActivityLog 
    WHERE EventTime >= CAST(GETDATE() AS DATE)
    Result: 47
    
    Query 4: Get controller status
    SELECT * FROM Controllers
    Result: { ControllerId: 1, SerialNumber: 223213880, LastSeen: ... }
    
18. Dashboard displays stats
    SignalR → Browser: Update UI
    Browser shows: "150 Members | 145 Cards | 47 Entries Today"

═══════════════════════════════════════════════════════════════
PHASE 4: VIEWING DOOR ACTIVITY
═══════════════════════════════════════════════════════════════

19. Director clicks "Key Cards" → "Door Activity"
    Browser → SignalR → Navigate("/keycards/activity")
    
20. DoorActivityTab.razor loads
    Calls LoadActivityAsync()
    
21. Web app queries database
    Query:
    SELECT TOP 50 
      d.EventTime, d.CardNumber, d.EventType, d.DoorId,
      m.FirstName, m.LastName,
      door.DoorName
    FROM DoorActivityLog d
    LEFT JOIN KeyCards k ON d.CardNumber = k.CardNumber
    LEFT JOIN Members m ON k.MemberID = m.MemberID
    LEFT JOIN Doors door ON d.DoorId = door.DoorId
    ORDER BY d.EventTime DESC
    
22. SQL Server returns results
    Result: [
      { Time: "2026-01-05 20:15:33", Member: "John Doe", Door: "Main Entrance", Result: "Granted" },
      { Time: "2026-01-05 19:45:12", Member: "Jane Smith", Door: "Back Door", Result: "Granted" },
      ...
    ]
    
23. DoorActivityTab displays table
    SignalR → Browser: Render table
    Browser shows list of 50 recent entries

═══════════════════════════════════════════════════════════════
PHASE 5: REAL-TIME UPDATES (Background)
═══════════════════════════════════════════════════════════════

24. ControllerEventService runs (background job, every 5 seconds)
    
    Step A: Query Agent API for new events
    HTTP GET https://localhost:5101/api/controllers/223213880/events?lastIndex=1235
    Header: X-Agent-Key: GFC-ACCESS-CONTROL-SECRET-KEY-2025
    
    Step B: Agent API sends UDP packet to controller
    Source: localhost:60000
    Destination: 192.168.1.72:60000
    Packet: [0xB0, 0x00, CRC, SN, LastIndex, ...]
    
    Step C: Controller responds with events
    UDP packet back to localhost:60000
    Data: [{ Index: 1236, CardNumber: 67890, Door: 2, Time: ..., Result: 1 }]
    
    Step D: Agent API returns JSON to web app
    HTTP 200 OK
    Body: { "Success": true, "Data": { "Events": [...] } }
    
    Step E: ControllerEventService processes events
    For each event:
      - Insert into DoorActivityLog
      - Broadcast via SignalR to connected clients
    
25. Director's browser receives SignalR update
    Message: "NewDoorEvent"
    Data: { Door: "Back Door", Member: "Bob Johnson", Time: "8:20 PM", Result: "Granted" }
    
26. Browser updates table
    New row appears at top of table
    No page refresh needed

═══════════════════════════════════════════════════════════════
PHASE 6: DIRECTOR LOGS OUT
═══════════════════════════════════════════════════════════════

27. Director clicks "Logout"
    Browser → SignalR → Logout button clicked
    
28. Web app calls AuthenticationService.LogoutAsync()
    Clears session from memory
    Closes SignalR connection
    
29. Browser redirects to login page
    SignalR → Browser: Navigate("/login")
    WebSocket connection closes
    
30. Connection terminated
    TCP FIN packets exchanged
    Browser and server both close sockets
```

**Total Data Transferred**:
- Initial page load: ~2 MB
- Login: ~1 KB
- Dashboard data: ~5 KB
- Door activity: ~10 KB
- Real-time updates: ~500 bytes per event

**Total Time**: ~3 seconds from login to viewing door activity

---

## 🔐 VPN Integration (Planned)

### How VPN Would Change Communication

#### Without VPN (Current):
```
Director's Phone (192.168.1.100)
    ↓ Direct connection on local network
Web App (192.168.1.50:8080)
```

**Limitation**: Only works on local network

#### With Cloudflare Tunnel (Planned):
```
Director's Phone (Anywhere)
    ↓ HTTPS to Cloudflare
Cloudflare Edge
    ↓ Encrypted tunnel
cloudflared service (on server)
    ↓ localhost
Web App (localhost:8080)
```

**Benefit**: Works from anywhere, but no VPN needed

#### With WireGuard VPN (Planned):
```
Director's Phone (Anywhere)
    ↓ WireGuard VPN tunnel
WireGuard Server (10.20.0.1)
    ↓ Routes to private network
Director appears as 10.20.0.5
    ↓ Can access everything on network
Web App, Cameras, etc.
```

**Benefit**: Full network access, like being on-site

### VPN Communication Flow

```
1. Director activates WireGuard on phone
   WireGuard client connects to gfc.lovanow.com:51820
   
2. VPN tunnel established
   Encrypted UDP tunnel created
   Director's phone gets IP: 10.20.0.5
   
3. Director opens browser
   Types: http://192.168.1.50:8080
   OR: http://10.20.0.1:8080 (VPN gateway)
   
4. Traffic flows through VPN
   Phone (10.20.0.5) → VPN Server (10.20.0.1) → Web App (192.168.1.50)
   All traffic encrypted in VPN tunnel
   
5. Web app sees VPN IP
   Request appears to come from 10.20.0.5
   Logged in audit: "IP: 10.20.0.5"
   
6. Director can also access cameras
   Camera NVR: http://192.168.1.100:8000
   Accessible through VPN tunnel
```

---

## 📝 Summary

### Key Takeaways

1. **User Login**: Browser → SignalR → AuthenticationService → SQL Server → Session created

2. **Database Communication**: All data operations go through SQL Server on localhost:1433 using Windows Authentication

3. **Controller Communication**: Web App → Agent API (HTTP) → Controller (UDP) → Two-tier architecture for reliability

4. **Real-time Updates**: SignalR WebSocket keeps browser and server connected for instant UI updates

5. **Background Jobs**: Services poll controller every 5 seconds for new events and sync to database

6. **Network Access**: 
   - Current: Local network only (192.168.1.x)
   - Planned: Cloudflare Tunnel for HTTPS or WireGuard VPN for full network access

7. **Security**: Windows Authentication for SQL, API key for Agent API, session-based for web app

8. **Audit Trail**: Every action logged to AuditLogs table with user, timestamp, and details

---

**Document End**  
**Generated**: 2026-01-05 20:15 EST  
**For questions, see**: VPN_IMPLEMENTATION_STATUS_REPORT.md
