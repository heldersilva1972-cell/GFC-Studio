# GFC CAMERA SYSTEM – REMOTE VIDEO VIEWING SECURITY
## MASTER IMPLEMENTATION PLAN v1.0

**Project Status:** Planning Complete - Ready for Implementation  
**Document Version:** 1.0  
**Created:** 2025-12-24  
**Last Updated:** 2025-12-24

---

## 📋 EXECUTIVE SUMMARY

This document defines the complete implementation plan for enabling **secure remote video viewing** in the GFC Web App. The system allows authorized users (Directors and designated members) to view live and recorded camera footage from anywhere in the world while maintaining bank-level security.

### Core Security Principles
1. **Zero Trust Architecture:** Remote access requires VPN authentication
2. **Sovereign Control:** All security managed in-house (no third-party dependencies)
3. **Automated UX:** Non-technical users can set up access in under 2 minutes
4. **Complete Auditability:** Every action is logged and traceable
5. **IP Protection:** Home/club IP address never exposed to the internet

---

## 🎯 PROJECT OBJECTIVES

### Primary Goals
- ✅ Enable secure remote camera viewing for authorized users
- ✅ Protect the club network, NVR, and web app from external threats
- ✅ Provide a "one-click" setup experience for non-technical users
- ✅ Give administrators complete visibility and control over access
- ✅ Support all devices (Windows, Mac, iPhone, Android, Linux)

### Success Criteria
- Any authorized user can complete VPN setup in under 2 minutes without IT help
- Zero exposure of NVR, Video Agent, or internal network to public internet
- Administrators can view real-time access logs and revoke permissions instantly
- System works identically whether user is on-site (LAN) or remote (VPN)

---

## 🏗️ SYSTEM ARCHITECTURE

### Network Topology

```
┌─────────────────────────────────────────────────────────────┐
│                    INTERNET (Public)                         │
│                                                              │
│  ┌──────────────────────────────────────────────────┐       │
│  │         Cloudflare Tunnel (IP Masking)           │       │
│  │  • Hides actual home IP                          │       │
│  │  • DDoS protection                               │       │
│  │  • Bot filtering                                 │       │
│  └────────────────┬─────────────────────────────────┘       │
│                   │ (Encrypted Tunnel)                       │
└───────────────────┼──────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────┐
│              HOME/CLUB NETWORK (Private LAN)                 │
│                                                              │
│  ┌──────────────────────────────────────────────────┐       │
│  │         Windows PC (Host Server)                 │       │
│  │  ┌────────────────────────────────────────────┐  │       │
│  │  │  GFC Web App (Blazor Server)               │  │       │
│  │  │  • Authentication & Authorization          │  │       │
│  │  │  • VPN Profile Generation                  │  │       │
│  │  │  • Network Location Detection              │  │       │
│  │  │  • Audit Logging                           │  │       │
│  │  └────────────────────────────────────────────┘  │       │
│  │                                                   │       │
│  │  ┌────────────────────────────────────────────┐  │       │
│  │  │  WireGuard VPN Server                      │  │       │
│  │  │  • Port: 51820 (UDP)                       │  │       │
│  │  │  • Subnet: 10.8.0.0/24                     │  │       │
│  │  │  • Per-user keypairs                       │  │       │
│  │  └────────────────────────────────────────────┘  │       │
│  │                                                   │       │
│  │  ┌────────────────────────────────────────────┐  │       │
│  │  │  Video Agent Service                       │  │       │
│  │  │  • HLS Stream Generation                   │  │       │
│  │  │  • Token Validation                        │  │       │
│  │  │  • Only accepts LAN + VPN traffic          │  │       │
│  │  └────────────────────────────────────────────┘  │       │
│  └───────────────────────┬───────────────────────────┘       │
│                          │ (LAN Only)                        │
│                          ▼                                   │
│  ┌──────────────────────────────────────────────────┐       │
│  │         NVR (Network Video Recorder)             │       │
│  │  • NEVER exposed to internet                     │       │
│  │  • Credentials stored only on Video Agent        │       │
│  │  • IP: 192.168.x.x (Private)                     │       │
│  └──────────────────────────────────────────────────┘       │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

### Access Flow Decision Tree

```
User Attempts to Access Video Page
         │
         ▼
    Is user logged in?
         │
    ┌────┴────┐
   NO        YES
    │          │
    ▼          ▼
 Login    Check IP Address
 Page          │
         ┌─────┴─────┐
         │           │
    LAN Range?   VPN Range?
         │           │
    ┌────┴────┐ ┌────┴────┐
   YES       NO YES       NO
    │         │  │         │
    ▼         │  ▼         ▼
 ALLOW        │ ALLOW   BLOCK
 VIDEO        │ VIDEO   VIDEO
              │           │
              │           ▼
              │    Show "Enable Secure
              │     Video Access" Flow
              │           │
              │           ▼
              │    1. Install WireGuard
              │    2. Download Profile
              │    3. Connect
              │    4. Verify
              │           │
              └───────────┘
```

---

## 🔐 SECURITY MODEL

### Layer 1: Network Isolation
- **NVR:** Never reachable from internet (192.168.x.x only)
- **Video Agent:** Only accepts connections from LAN (192.168.x.x) or VPN (10.8.0.x)
- **Web App:** Accessible via Cloudflare Tunnel (actual IP hidden)

### Layer 2: VPN Authentication
- **WireGuard:** Military-grade encryption (Curve25519, ChaCha20, Poly1305)
- **Per-User Keys:** Each user gets unique keypair (no shared credentials)
- **Silent Protocol:** VPN port appears "closed" to unauthorized scanners
- **Instant Revocation:** Removing user's public key kills their tunnel immediately

### Layer 3: Application Authorization
- **Role-Based Access:** Only Directors + Authorized Users can access video
- **Session Validation:** Every stream request validates user session + permissions
- **Token-Based Streaming:** Video URLs contain short-lived tokens (60 seconds)
- **Audit Logging:** Every view, download, and configuration change is logged

### Layer 4: Optional Enhancements (Configurable)
- **Two-Factor Authentication:** Extra code required for remote access
- **Device Management:** Revoke access for specific devices (lost phone)
- **IP Filtering:** Whitelist/blacklist specific countries or IP ranges
- **Geofencing:** Alert if user connects from unusual location
- **Watermarking:** Overlay username on video to deter unauthorized sharing

---

## 📱 USER EXPERIENCE FLOW

### Scenario 1: On-Site User (LAN)
```
1. User logs into Web App from club computer/WiFi
2. User clicks "Video" in navigation
3. System detects LAN IP (192.168.x.x)
4. ✅ Video page loads immediately
5. User selects cameras and views live/recorded footage
```

### Scenario 2: Remote User (First Time)
```
1. User logs into Web App from home
2. User clicks "Video" in navigation
3. System detects public IP (not LAN or VPN)
4. ❌ Video blocked - Redirect to "Enable Secure Video Access" page

   ┌─────────────────────────────────────────────────┐
   │  🔒 Enable Secure Video Access                  │
   │                                                  │
   │  For security, remote video viewing requires    │
   │  a secure connection. Follow these 3 steps:     │
   │                                                  │
   │  Step 1: Install Secure Access App              │
   │  ┌──────────────────────────────────────────┐   │
   │  │  [📱 Install WireGuard]                  │   │
   │  │  (Opens App Store/Play Store)            │   │
   │  └──────────────────────────────────────────┘   │
   │  ℹ️ This is a free, industry-standard VPN app   │
   │                                                  │
   │  Step 2: Download Your Profile                  │
   │  ┌──────────────────────────────────────────┐   │
   │  │  [📥 Download My Secure Profile]         │   │
   │  │  (Downloads .conf file)                  │   │
   │  └──────────────────────────────────────────┘   │
   │  ℹ️ This file is unique to you and expires in   │
   │     24 hours if not used                        │
   │                                                  │
   │  Step 3: Connect                                │
   │  ┌──────────────────────────────────────────┐   │
   │  │  1. Tap the downloaded file              │   │
   │  │  2. Choose "Open with WireGuard"         │   │
   │  │  3. Tap "Allow" to add VPN               │   │
   │  │  4. Toggle the switch to "Connected"     │   │
   │  └──────────────────────────────────────────┘   │
   │                                                  │
   │  ┌──────────────────────────────────────────┐   │
   │  │  [✅ I'm Connected - Verify Access]      │   │
   │  └──────────────────────────────────────────┘   │
   │                                                  │
   │  Need help? [📖 View Setup Guide with Photos]   │
   └─────────────────────────────────────────────────┘

5. User completes setup
6. User clicks "Verify Access"
7. System detects VPN IP (10.8.0.x)
8. ✅ Video page unlocks
9. User can now view cameras remotely
```

### Scenario 3: Remote User (Returning)
```
1. User logs into Web App from home
2. User opens WireGuard app and connects
3. User clicks "Video" in Web App
4. System detects VPN IP (10.8.0.x)
5. ✅ Video page loads immediately
```

---

## 🛠️ IMPLEMENTATION PHASES

### Phase 1: Foundation & Infrastructure (Week 1-2)
**Objective:** Set up core networking and security infrastructure

#### 1.1 Cloudflare Tunnel Setup
- [ ] Create setup guide document with screenshots
- [ ] Add "Cloudflare Tunnel Configuration" section to Settings page
- [ ] Implement tunnel status check (green/red indicator)
- [ ] Add "Test Connection" button with diagnostic output

#### 1.2 WireGuard Server Integration
- [ ] Install WireGuard on Windows host server
- [ ] Create C# wrapper for WireGuard management commands
- [ ] Implement keypair generation (Curve25519)
- [ ] Implement peer add/remove functionality
- [ ] Create configuration file templates

#### 1.3 Database Schema Updates
```sql
-- VPN Profiles Table
CREATE TABLE VpnProfiles (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    PublicKey NVARCHAR(255) NOT NULL UNIQUE,
    PrivateKey NVARCHAR(255) NOT NULL, -- Encrypted at rest
    AssignedIP NVARCHAR(50) NOT NULL UNIQUE, -- e.g., 10.8.0.15
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastUsedAt DATETIME2 NULL,
    RevokedAt DATETIME2 NULL,
    RevokedBy INT NULL FOREIGN KEY REFERENCES Users(Id),
    RevokedReason NVARCHAR(500) NULL,
    DeviceName NVARCHAR(255) NULL,
    DeviceType NVARCHAR(50) NULL -- iOS, Android, Windows, Mac, Linux
);

-- VPN Sessions Table (Active Connections)
CREATE TABLE VpnSessions (
    Id INT PRIMARY KEY IDENTITY,
    VpnProfileId INT NOT NULL FOREIGN KEY REFERENCES VpnProfiles(Id),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    ConnectedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    DisconnectedAt DATETIME2 NULL,
    ClientIP NVARCHAR(50) NOT NULL,
    BytesReceived BIGINT DEFAULT 0,
    BytesSent BIGINT DEFAULT 0
);

-- Video Access Audit Table
CREATE TABLE VideoAccessAudit (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AccessType NVARCHAR(50) NOT NULL, -- 'LiveView', 'Recording', 'Download', 'Snapshot'
    CameraId INT NULL FOREIGN KEY REFERENCES Cameras(Id),
    CameraName NVARCHAR(255) NULL,
    ConnectionType NVARCHAR(50) NOT NULL, -- 'LAN', 'VPN', 'Blocked'
    ClientIP NVARCHAR(50) NOT NULL,
    SessionStart DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SessionEnd DATETIME2 NULL,
    DurationSeconds INT NULL,
    RecordingFile NVARCHAR(500) NULL, -- If downloaded
    Notes NVARCHAR(MAX) NULL
);

-- Security Alerts Table
CREATE TABLE SecurityAlerts (
    Id INT PRIMARY KEY IDENTITY,
    AlertType NVARCHAR(100) NOT NULL, -- 'FailedLogin', 'UnusualLocation', 'ExpiredAccess', etc.
    Severity NVARCHAR(20) NOT NULL, -- 'Info', 'Warning', 'Critical'
    UserId INT NULL FOREIGN KEY REFERENCES Users(Id),
    Username NVARCHAR(255) NULL,
    ClientIP NVARCHAR(50) NULL,
    Details NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ReviewedAt DATETIME2 NULL,
    ReviewedBy INT NULL FOREIGN KEY REFERENCES Users(Id),
    Status NVARCHAR(50) DEFAULT 'New' -- 'New', 'Reviewed', 'Resolved', 'Dismissed'
);

-- System Settings Table (Add new columns)
ALTER TABLE SystemSettings ADD
    CloudflareTunnelToken NVARCHAR(MAX) NULL, -- Encrypted
    PublicDomain NVARCHAR(255) NULL, -- e.g., gfc-cameras.yourclub.com
    WireGuardPort INT DEFAULT 51820,
    WireGuardSubnet NVARCHAR(50) DEFAULT '10.8.0.0/24',
    MaxSimultaneousViewers INT DEFAULT 10,
    DirectorAccessExpiryDate DATETIME2 NULL,
    EnableTwoFactorAuth BIT DEFAULT 0,
    EnableSessionTimeout BIT DEFAULT 0,
    SessionTimeoutMinutes INT DEFAULT 30,
    EnableFailedLoginProtection BIT DEFAULT 1,
    MaxFailedLoginAttempts INT DEFAULT 5,
    LoginLockDurationMinutes INT DEFAULT 30,
    EnableIPFiltering BIT DEFAULT 0,
    IPFilterMode NVARCHAR(20) DEFAULT 'Blacklist', -- 'Whitelist' or 'Blacklist'
    EnableGeofencing BIT DEFAULT 0,
    EnableWatermarking BIT DEFAULT 0,
    WatermarkPosition NVARCHAR(20) DEFAULT 'BottomRight',
    EnableConnectionQualityAlerts BIT DEFAULT 1,
    MinimumBandwidthMbps DECIMAL(5,2) DEFAULT 1.0;
```

---

### Phase 2: Settings Page & Configuration UI (Week 2-3)
**Objective:** Build comprehensive admin interface for all security settings

#### 2.1 Remote Access Configuration Section
```
Settings > Security & Remote Access > Remote Access Configuration

┌─────────────────────────────────────────────────────────────┐
│ 📡 Remote Access Configuration                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ Step 1: Cloudflare Tunnel Setup                             │
│ ─────────────────────────────────────────────────────       │
│ ℹ️ What is this?                                            │
│ This protects your home IP address by routing traffic       │
│ through Cloudflare's secure network.                        │
│                                                              │
│ Tunnel Token 🔑                                             │
│ [________________________________________________]           │
│ 📘 Where do I get this?                                     │
│    Follow our [Cloudflare Setup Guide] (takes 5 minutes)   │
│                                                              │
│ Status: ✅ Connected | Last check: 2 minutes ago            │
│ [Test Connection]                                           │
│                                                              │
│ ─────────────────────────────────────────────────────       │
│                                                              │
│ Step 2: Public Domain                                       │
│ ─────────────────────────────────────────────────────       │
│ ℹ️ What is this?                                            │
│ The web address people will use to access the system        │
│ remotely.                                                   │
│                                                              │
│ Your Domain                                                 │
│ [________________________________________________]           │
│ 💡 Example: gfc-cameras.yourclub.com                        │
│ 📘 Where do I get this?                                     │
│    This is provided by Cloudflare after Step 1             │
│                                                              │
│ ─────────────────────────────────────────────────────       │
│                                                              │
│ Step 3: VPN Settings (Advanced)                             │
│ ─────────────────────────────────────────────────────       │
│ ℹ️ What is this?                                            │
│ These settings control the secure "tunnel" that allows      │
│ remote video viewing.                                       │
│                                                              │
│ VPN Port                                                    │
│ [51820]                                                     │
│ 📘 What does this do?                                       │
│    This is the "door number" WireGuard uses. The default   │
│    works for 99% of setups. Only change if you have        │
│    another service using port 51820.                        │
│                                                              │
│ VPN Subnet                                                  │
│ [10.8.0.0/24]                                               │
│ 📘 What does this do?                                       │
│    This is the "private address range" for VPN users.      │
│    Keep the default unless it conflicts with your home     │
│    network (unlikely).                                      │
│                                                              │
│ [💾 Save Configuration]                                     │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

#### 2.2 User & Permission Management
```
Settings > Security & Remote Access > User & Permission Management

┌─────────────────────────────────────────────────────────────┐
│ 👥 Director Access Management                               │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ ℹ️ What is this?                                            │
│ Controls when board members lose remote video access after  │
│ their term ends.                                            │
│                                                              │
│ Current Directors Access Expires                            │
│ [📅 MM/DD/YYYY] [Clear]                                     │
│                                                              │
│ 💡 Recommended: Set this 30 days after the new board takes  │
│    over to allow for transition period.                     │
│                                                              │
│ 📘 Why?                                                      │
│    This gives outgoing directors time to help with          │
│    transition while ensuring access doesn't stay open       │
│    forever.                                                 │
│                                                              │
│ Current Status:                                             │
│ • 5 Directors with active access                            │
│ • Access expires: 01/31/2026 (38 days)                      │
│                                                              │
├─────────────────────────────────────────────────────────────┤
│ 👤 Additional Authorized Users                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ ℹ️ What is this?                                            │
│ Grant video access to specific members who are not          │
│ directors (e.g., security staff, maintenance).              │
│                                                              │
│ [+ Add Authorized User]                                     │
│                                                              │
│ ┌──────────────────────────────────────────────────────┐    │
│ │ User          │ Access Level │ Expires    │ Actions  │    │
│ ├──────────────────────────────────────────────────────┤    │
│ │ John Smith    │ Full Access  │ Never      │ [Remove] │    │
│ │ Security Mgr  │              │            │          │    │
│ ├──────────────────────────────────────────────────────┤    │
│ │ Jane Doe      │ View Only    │ 03/15/2026 │ [Remove] │    │
│ │ Maintenance   │              │            │ [Extend] │    │
│ └──────────────────────────────────────────────────────┘    │
│                                                              │
│ 📘 Access Levels:                                           │
│    • Full Access: View live, recordings, download clips     │
│    • View Only: View live and recordings (no downloads)     │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

#### 2.3 Security Features (Optional Toggles)
```
Settings > Security & Remote Access > Security Features

┌─────────────────────────────────────────────────────────────┐
│ 🔐 Security Features                                         │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ [☐] Two-Factor Authentication (2FA)                         │
│     ℹ️ Require authenticator app code for remote access    │
│     📘 Recommended for maximum security                     │
│     ⚙️ Configure: [Authenticator App ▼] [SMS Backup ☐]    │
│                                                              │
│ [☑] Session Auto-Timeout                                    │
│     ℹ️ Force re-login after inactivity                     │
│     ⚙️ Timeout after: [30 minutes ▼]                       │
│     ⚙️ Apply to: [Remote Only ▼]                           │
│     📘 Prevents unattended access                           │
│                                                              │
│ [☑] Failed Login Protection                                 │
│     ℹ️ Auto-block after too many wrong passwords           │
│     ⚙️ Lock after: [5 ▼] failed attempts                   │
│     ⚙️ Lock duration: [30 minutes ▼]                       │
│     ⚙️ [☑] Notify admin via email                          │
│     📘 Prevents brute-force attacks                         │
│                                                              │
│ [☐] IP Filtering                                            │
│     ℹ️ Block or allow specific IP addresses/countries      │
│     ⚙️ Mode: [Blacklist ▼] (Block these IPs)               │
│     ⚙️ [Manage IP List...]                                 │
│     📘 Advanced: Use to block known malicious IPs           │
│                                                              │
│ [☐] Video Watermarking                                      │
│     ℹ️ Overlay username on video streams                   │
│     ⚙️ Display: [Username + Timestamp ▼]                   │
│     ⚙️ Position: [Bottom Right ▼]                          │
│     📘 Deters unauthorized recording/sharing                │
│                                                              │
│ [💾 Save Security Settings]                                 │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

#### 2.4 Monitoring & Alerts
```
Settings > Security & Remote Access > Monitoring & Alerts

┌─────────────────────────────────────────────────────────────┐
│ 📊 Monitoring & Alerts                                       │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ [☐] Geofencing Alerts                                       │
│     ℹ️ Alert if user connects from unusual location        │
│     ⚙️ Alert when user connects from new:                  │
│        [☑] Country  [☑] State  [☐] City                    │
│     📘 Example: "John connected from California (unusual)"  │
│                                                              │
│ [☑] Connection Quality Monitoring                           │
│     ℹ️ Notify users if their connection is poor            │
│     ⚙️ Show warning if bandwidth drops below:              │
│        [1.0 Mbps ▼]                                         │
│     ⚙️ [☑] Auto-reduce quality to maintain connection      │
│     📘 Prevents buffering and improves user experience      │
│                                                              │
│ [☑] Email Notifications                                     │
│     ℹ️ Send alerts to administrators                       │
│     ⚙️ Notify me when:                                     │
│        [☑] New user completes VPN setup                    │
│        [☑] User connects from new device                   │
│        [☑] Security alert triggered                        │
│        [☐] Daily summary report                            │
│        [☐] Weekly summary report                           │
│     ⚙️ Send to: [admin@yourclub.com]                       │
│                                                              │
│ [💾 Save Alert Settings]                                    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

#### 2.5 System Limits
```
Settings > Security & Remote Access > System Limits

┌─────────────────────────────────────────────────────────────┐
│ 🎛️ System Limits                                            │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ Max Simultaneous Users                                      │
│ [10 ▼]                                                      │
│ ℹ️ Maximum number of people who can watch video at once    │
│ 📘 Current usage: 3/10 users                                │
│ 💡 Recommended: Set based on your internet upload speed     │
│    (2-3 Mbps per user for HD quality)                       │
│                                                              │
│ ─────────────────────────────────────────────────────       │
│                                                              │
│ Bandwidth Management                                        │
│ ℹ️ Control video quality to prevent network congestion     │
│                                                              │
│ Remote Users (VPN):                                         │
│ • Max Quality: [HD 720p ▼]                                  │
│ • Limit per user: [3 Mbps ▼]                                │
│                                                              │
│ On-Site Users (LAN):                                        │
│ • Max Quality: [Full HD 1080p ▼]                            │
│ • Limit per user: [No Limit ▼]                              │
│                                                              │
│ 📘 Why different limits?                                    │
│    Remote users share your internet upload speed, so        │
│    limiting quality prevents buffering. On-site users use   │
│    local network which is much faster.                      │
│                                                              │
│ [☐] Enable "Waiting Room"                                   │
│     ℹ️ If max users reached, show queue instead of error   │
│     ⚙️ Max wait time: [5 minutes ▼]                        │
│                                                              │
│ [💾 Save Limit Settings]                                    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

#### 2.6 Advanced Settings
```
Settings > Security & Remote Access > Advanced

┌─────────────────────────────────────────────────────────────┐
│ 🛠️ Advanced Settings                                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ [☐] Granular Camera Permissions                             │
│     ℹ️ Control which users can see which cameras           │
│     📘 When disabled: All authorized users see all cameras  │
│     📘 When enabled: Customize per user                     │
│     ⚙️ [Manage Camera Permissions...]                      │
│                                                              │
│ ─────────────────────────────────────────────────────       │
│                                                              │
│ Backup Administrator                                        │
│ ℹ️ Designate someone who can manage the system if you're   │
│    unavailable                                              │
│                                                              │
│ Primary Admin: You (Admin)                                  │
│ Backup Admin: [Select User ▼] [None]                        │
│                                                              │
│ Backup can:                                                 │
│ [☑] Revoke user access                                      │
│ [☑] View audit logs                                         │
│ [☐] Modify security settings                                │
│ [☐] Add/remove authorized users                             │
│                                                              │
│ 📘 Recommended: Grant limited permissions to backup admin   │
│                                                              │
│ ─────────────────────────────────────────────────────       │
│                                                              │
│ 🚨 Emergency Kill Switch                                    │
│ ℹ️ Instantly disconnect ALL remote users and disable       │
│    remote access                                            │
│                                                              │
│ [🔴 ACTIVATE EMERGENCY LOCKDOWN]                            │
│                                                              │
│ ⚠️ WARNING: This will:                                      │
│    • Disconnect all VPN users immediately                   │
│    • Disable remote video access                            │
│    • Require manual re-enable                               │
│    • Send alert to all admins                               │
│                                                              │
│ Use only if you suspect:                                    │
│ • Security breach                                           │
│ • Stolen/compromised device                                 │
│ • Unauthorized access                                       │
│                                                              │
│ ─────────────────────────────────────────────────────       │
│                                                              │
│ Maintenance Mode Scheduler                                  │
│ ℹ️ Schedule system downtime for updates/maintenance        │
│                                                              │
│ [☐] Maintenance Mode Active                                 │
│                                                              │
│ Schedule Maintenance:                                       │
│ Start: [📅 MM/DD/YYYY] [🕐 HH:MM AM/PM]                    │
│ End:   [📅 MM/DD/YYYY] [🕐 HH:MM AM/PM]                    │
│                                                              │
│ Message to display:                                         │
│ [_________________________________________________]          │
│ Default: "System maintenance in progress. Video access      │
│          will resume at [end time]."                        │
│                                                              │
│ [☑] Auto-disconnect users 5 minutes before start            │
│ [☑] Send notification 24 hours in advance                   │
│                                                              │
│ [💾 Save Advanced Settings]                                 │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

### Phase 3: Video Access Monitoring Dashboard (Week 3-4)
**Objective:** Build comprehensive monitoring and audit interface

#### 3.1 Create New Page: VideoAccessMonitoring.razor
Location: `Components/Pages/Admin/VideoAccessMonitoring.razor`

#### 3.2 Tab 1: VPN Setup Status
- [ ] Display all users with video permissions
- [ ] Show setup completion status (Complete/Pending/Never)
- [ ] Show VPN profile creation date
- [ ] Show last connection date
- [ ] Actions: Revoke, Reset, Send Reminder
- [ ] Filters: All/Complete/Pending, Date range, Search

#### 3.3 Tab 2: Active Sessions
- [ ] Real-time display of connected users
- [ ] Show connection type (LAN/VPN)
- [ ] Show client IP address
- [ ] Show connected duration
- [ ] Show cameras being viewed
- [ ] Show bandwidth usage
- [ ] Action: Disconnect user immediately
- [ ] Export to CSV

#### 3.4 Tab 3: Access History (Audit Log)
- [ ] Complete historical record
- [ ] Columns: Date/Time, User, Action, Location, Camera(s), Duration, Details
- [ ] Actions: Viewed Live, Downloaded Recording, VPN Connected, Setup Complete
- [ ] Filters: Date range, User, Action type, Location
- [ ] Export: Excel, PDF, CSV
- [ ] Pagination (50 records per page)

#### 3.5 Tab 4: Security Alerts
- [ ] Display security events
- [ ] Alert types: Failed Logins, Unusual Location, Expired Access Attempt, New Device
- [ ] Severity levels: Info, Warning, Critical
- [ ] Status: New, Reviewed, Resolved, Dismissed
- [ ] Action: Mark as reviewed, Add notes
- [ ] Auto-highlight unreviewed alerts

---

### Phase 4: Network Location Detection (Week 4)
**Objective:** Implement IP-based access control

#### 4.1 Create NetworkLocationService
```csharp
public interface INetworkLocationService
{
    Task<LocationType> DetectLocationAsync(string ipAddress);
    Task<bool> IsLanAddressAsync(string ipAddress);
    Task<bool> IsVpnAddressAsync(string ipAddress);
    Task<bool> IsAuthorizedForVideoAsync(int userId, string ipAddress);
}

public enum LocationType
{
    LAN,        // 192.168.x.x or configured LAN range
    VPN,        // 10.8.0.x or configured VPN range
    Public,     // Everything else
    Unknown
}
```

#### 4.2 Implement IP Range Checking
- [ ] Parse LAN subnet from settings (e.g., 192.168.1.0/24)
- [ ] Parse VPN subnet from settings (e.g., 10.8.0.0/24)
- [ ] Implement CIDR notation support
- [ ] Handle IPv4 and IPv6
- [ ] Cache results for performance

#### 4.3 Create Authorization Middleware
- [ ] Intercept requests to `/cameras/*` routes
- [ ] Extract client IP (handle X-Forwarded-For from Cloudflare)
- [ ] Check location type
- [ ] Allow: LAN or VPN
- [ ] Block: Public (redirect to setup page)
- [ ] Log all attempts

---

### Phase 5: VPN Profile Generation & Management (Week 5-6)
**Objective:** Automate WireGuard configuration for users

#### 5.1 Create WireGuardManagementService
```csharp
public interface IWireGuardManagementService
{
    Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType);
    Task<string> GenerateConfigFileAsync(VpnProfile profile);
    Task<bool> ActivateProfileAsync(int profileId);
    Task<bool> RevokeProfileAsync(int profileId, int revokedByUserId, string reason);
    Task<List<VpnProfile>> GetUserProfilesAsync(int userId);
    Task<List<VpnSession>> GetActiveSessionsAsync();
    Task<bool> DisconnectSessionAsync(int sessionId);
}
```

#### 5.2 Implement Key Generation
- [ ] Use libsodium or BouncyCastle for Curve25519 keys
- [ ] Generate private key (32 bytes, base64 encoded)
- [ ] Derive public key from private key
- [ ] Store private key encrypted in database
- [ ] Never expose private key in logs

#### 5.3 Implement IP Assignment
- [ ] Track used IPs in VpnProfiles table
- [ ] Assign next available IP in subnet (e.g., 10.8.0.2, 10.8.0.3, etc.)
- [ ] Reserve 10.8.0.1 for server
- [ ] Handle IP recycling when profiles are revoked

#### 5.4 Generate .conf File
```ini
[Interface]
PrivateKey = <user_private_key>
Address = 10.8.0.15/32
DNS = 1.1.1.1

[Peer]
PublicKey = <server_public_key>
Endpoint = gfc-cameras.yourclub.com:51820
AllowedIPs = 10.8.0.0/24, 192.168.1.0/24
PersistentKeepalive = 25
```

#### 5.5 Implement Server-Side Peer Management
- [ ] Add peer to WireGuard interface when profile activated
- [ ] Command: `wg set wg0 peer <public_key> allowed-ips 10.8.0.15/32`
- [ ] Remove peer when profile revoked
- [ ] Command: `wg set wg0 peer <public_key> remove`
- [ ] Persist changes: `wg-quick save wg0`

---

### Phase 6: Guided Setup Flow (Week 6-7)
**Objective:** Build the "Enable Secure Video Access" user experience

#### 6.1 Create EnableSecureVideoAccess.razor
Location: `Components/Pages/Camera/EnableSecureVideoAccess.razor`

#### 6.2 Implement Device Detection
```csharp
public class DeviceDetectionService
{
    public DeviceInfo DetectDevice(HttpContext context)
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        
        return new DeviceInfo
        {
            Type = DetermineDeviceType(userAgent),
            OS = DetermineOS(userAgent),
            AppStoreUrl = GetAppStoreUrl(userAgent)
        };
    }
    
    private string GetAppStoreUrl(string userAgent)
    {
        if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
            return "https://apps.apple.com/app/wireguard/id1441195209";
        else if (userAgent.Contains("Android"))
            return "https://play.google.com/store/apps/details?id=com.wireguard.android";
        else if (userAgent.Contains("Mac"))
            return "https://apps.apple.com/app/wireguard/id1451685025";
        else if (userAgent.Contains("Windows"))
            return "https://download.wireguard.com/windows-client/wireguard-installer.exe";
        else
            return "https://www.wireguard.com/install/";
    }
}
```

#### 6.3 Step 1: Install WireGuard Button
- [ ] Detect device OS
- [ ] Show appropriate app store icon (Apple/Google/Windows)
- [ ] Open app store in new tab when clicked
- [ ] Track click event in audit log

#### 6.4 Step 2: Download Profile Button
- [ ] Check if user already has active profile
- [ ] If not, generate new profile automatically
- [ ] Create .conf file with user's keys
- [ ] Set Content-Disposition header for download
- [ ] Filename: `GFC-Video-Access-{Username}.conf`
- [ ] Track download event in audit log
- [ ] Show "Profile expires in 24 hours if not activated" warning

#### 6.5 Step 3: Connection Instructions
- [ ] Show platform-specific instructions
- [ ] iOS/Android: "Tap the file → Open with WireGuard → Allow → Toggle On"
- [ ] Windows: "Double-click the file → Import → Activate"
- [ ] Mac: "Double-click the file → Import → Activate"
- [ ] Include screenshots/GIFs for each platform

#### 6.6 Step 4: Verify Connection Button
- [ ] Re-check client IP address
- [ ] If now in VPN range (10.8.0.x):
  - Show success message
  - Mark profile as activated in database
  - Redirect to video page after 3 seconds
- [ ] If still public IP:
  - Show troubleshooting tips
  - Offer "Download Setup Guide PDF" button
  - Provide support contact info

---

### Phase 7: Video Stream Security (Week 7-8)
**Objective:** Implement token-based stream authentication

#### 7.1 Create StreamTokenService
```csharp
public interface IStreamTokenService
{
    Task<string> GenerateTokenAsync(int userId, int cameraId, int validitySeconds = 60);
    Task<bool> ValidateTokenAsync(string token, int cameraId);
    Task RevokeUserTokensAsync(int userId);
}
```

#### 7.2 Implement Token Generation
- [ ] Use JWT (JSON Web Token) format
- [ ] Claims: UserId, CameraId, IssuedAt, ExpiresAt
- [ ] Sign with secret key from configuration
- [ ] Default validity: 60 seconds
- [ ] Return token as query parameter: `/live/{cameraId}/index.m3u8?token={jwt}`

#### 7.3 Update Video Agent
- [ ] Add token validation middleware
- [ ] Extract token from query string
- [ ] Validate signature and expiration
- [ ] Check if user has permission for requested camera
- [ ] Return 403 Forbidden if invalid
- [ ] Return 401 Unauthorized if expired
- [ ] Log all validation attempts

#### 7.4 Update ViewCameras.razor
- [ ] Request token from Web App before loading stream
- [ ] Append token to HLS URL
- [ ] Refresh token every 45 seconds (before expiry)
- [ ] Handle token refresh failures gracefully

---

### Phase 8: System Health Dashboard (Week 8)
**Objective:** Real-time system status monitoring

#### 8.1 Create SystemHealthDashboard Component
Location: `Components/Shared/SystemHealthDashboard.razor`

#### 8.2 Implement Health Checks
```csharp
public class SystemHealthService
{
    public async Task<SystemHealth> GetHealthStatusAsync()
    {
        return new SystemHealth
        {
            VpnServer = await CheckVpnServerAsync(),
            VideoAgent = await CheckVideoAgentAsync(),
            CloudflareTunnel = await CheckCloudflareAsync(),
            DiskSpace = await CheckDiskSpaceAsync(),
            ActiveUsers = await GetActiveUserCountAsync(),
            CameraStatus = await GetCameraStatusAsync()
        };
    }
}
```

#### 8.3 Dashboard Display
```
┌─────────────────────────────────────────────────────────────┐
│ 📈 System Health Dashboard                                  │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ 🟢 VPN Server: Online                                       │
│    • Port 51820 listening                                   │
│    • 12 active peers                                        │
│    • Last restart: 3 days ago                               │
│                                                              │
│ 🟢 Video Agent: Online                                      │
│    • Streaming 3 cameras                                    │
│    • CPU: 15% | Memory: 2.1 GB                              │
│    • Uptime: 5 days                                         │
│                                                              │
│ 🟢 Cloudflare Tunnel: Connected                             │
│    • Latency: 45ms                                          │
│    • Last check: 30 seconds ago                             │
│                                                              │
│ ⚠️ Disk Space: 78% used (Warning)                           │
│    • 220 GB used / 1 TB total                               │
│    • Estimated days until full: 45                          │
│    • [Configure Cleanup Policy]                             │
│                                                              │
│ 🟢 Active Users: 3 / 10 max                                 │
│    • 2 remote (VPN)                                         │
│    • 1 on-site (LAN)                                        │
│                                                              │
│ ⚠️ Camera Status: 15 / 16 online                            │
│    • 🔴 Offline: Parking Lot Cam 2                          │
│    • Last seen: 2 hours ago                                 │
│    • [View Details]                                         │
│                                                              │
│ Last updated: 5 seconds ago | [🔄 Refresh Now]              │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

#### 8.4 Auto-Refresh
- [ ] Update every 30 seconds via SignalR
- [ ] Show "Last updated" timestamp
- [ ] Manual refresh button
- [ ] Pause auto-refresh when user is inactive

---

### Phase 9: Testing & Documentation (Week 9-10)
**Objective:** Comprehensive testing and user documentation

#### 9.1 Security Testing
- [ ] Penetration testing: Attempt to access video without VPN
- [ ] Token validation: Try expired/invalid tokens
- [ ] Brute force: Test failed login protection
- [ ] IP spoofing: Verify X-Forwarded-For validation
- [ ] Session hijacking: Test session timeout
- [ ] Revocation: Verify instant VPN disconnect

#### 9.2 Cross-Platform Testing
- [ ] iOS (iPhone/iPad): Setup flow and streaming
- [ ] Android: Setup flow and streaming
- [ ] Windows: Setup flow and streaming
- [ ] macOS: Setup flow and streaming
- [ ] Linux: Setup flow and streaming
- [ ] Multiple browsers: Chrome, Safari, Firefox, Edge

#### 9.3 Performance Testing
- [ ] Load test: 10 simultaneous users
- [ ] Bandwidth test: Measure actual usage per stream
- [ ] Latency test: Measure VPN overhead
- [ ] Stress test: Max users + 1 (waiting room)

#### 9.4 User Documentation
- [ ] **Cloudflare Setup Guide** (PDF with screenshots)
- [ ] **User Quick Start Guide** (How to set up VPN)
- [ ] **Troubleshooting Guide** (Common issues)
- [ ] **Admin Manual** (All settings explained)
- [ ] **Video Tutorials** (Optional: Screen recordings)

#### 9.5 Admin Training
- [ ] How to grant/revoke access
- [ ] How to monitor active sessions
- [ ] How to respond to security alerts
- [ ] How to use emergency kill switch
- [ ] How to schedule maintenance

---

## 📊 SUCCESS METRICS

### Security Metrics
- ✅ Zero unauthorized access attempts succeed
- ✅ 100% of video access attempts are logged
- ✅ Revoked users cannot connect (verified within 1 second)
- ✅ No NVR/Video Agent exposure to public internet
- ✅ All tokens expire within configured time (60 seconds)

### User Experience Metrics
- ✅ 95%+ of users complete setup without support
- ✅ Average setup time < 2 minutes
- ✅ Zero "it doesn't work" support tickets
- ✅ Video loads within 3 seconds of clicking camera

### System Performance Metrics
- ✅ Support 10 simultaneous users without buffering
- ✅ VPN adds < 50ms latency
- ✅ System health dashboard loads in < 1 second
- ✅ Audit log queries return in < 2 seconds

---

## 🚨 SECURITY CHECKLIST (Pre-Launch)

Before enabling remote access in production:

### Infrastructure
- [ ] Cloudflare Tunnel configured and tested
- [ ] WireGuard server installed and running
- [ ] Firewall rules configured (block all except Cloudflare IPs)
- [ ] SSL certificate valid and auto-renewing
- [ ] Backup admin designated and tested

### Application
- [ ] All database migrations applied
- [ ] Encryption keys generated and secured
- [ ] Session timeout enabled (30 minutes)
- [ ] Failed login protection enabled (5 attempts)
- [ ] Token expiration set to 60 seconds
- [ ] Audit logging verified working

### Testing
- [ ] Penetration test passed
- [ ] All platforms tested (iOS, Android, Windows, Mac)
- [ ] Emergency kill switch tested
- [ ] Revocation tested (instant disconnect verified)
- [ ] Load test passed (10+ users)

### Documentation
- [ ] User setup guide published
- [ ] Admin manual completed
- [ ] Troubleshooting guide available
- [ ] Support contact info displayed

### Monitoring
- [ ] Email alerts configured
- [ ] System health dashboard accessible
- [ ] Audit log retention policy set (90 days)
- [ ] Backup schedule verified

---

## 📞 SUPPORT & MAINTENANCE

### Ongoing Tasks
- **Weekly:** Review security alerts
- **Monthly:** Review audit logs for unusual patterns
- **Quarterly:** Update WireGuard and dependencies
- **Annually:** Rotate encryption keys

### Common Issues & Solutions

**Issue:** User can't connect to VPN  
**Solution:** 
1. Verify WireGuard app installed
2. Check profile was imported (not just downloaded)
3. Verify toggle is "On" in WireGuard app
4. Check firewall allows UDP port 51820

**Issue:** Video won't load after VPN connected  
**Solution:**
1. Click "Verify Connection" button
2. Check IP shows as 10.8.0.x in system
3. Verify user has video permissions
4. Check Video Agent is running

**Issue:** "Too many users" error  
**Solution:**
1. Check active sessions in monitoring dashboard
2. Disconnect idle users
3. Increase max users limit in settings
4. Enable waiting room feature

---

## 🎓 TRAINING MATERIALS

### For End Users
- **Title:** "How to Watch Cameras from Home"
- **Format:** 2-page PDF with screenshots
- **Content:** 
  1. Click Video in menu
  2. Click "Install WireGuard" button
  3. Click "Download Profile" button
  4. Tap file → Open with WireGuard
  5. Toggle switch to "On"
  6. Click "Verify Connection"
  7. Start watching!

### For Administrators
- **Title:** "Video Access Security Administration Guide"
- **Format:** 20-page PDF
- **Chapters:**
  1. Initial Setup (Cloudflare + WireGuard)
  2. Granting Access (Directors + Authorized Users)
  3. Monitoring Access (Dashboard + Audit Logs)
  4. Responding to Alerts
  5. Revoking Access
  6. Emergency Procedures
  7. Troubleshooting

---

## 📅 IMPLEMENTATION TIMELINE

| Phase | Duration | Dependencies | Deliverables |
|-------|----------|--------------|--------------|
| 1. Foundation | 2 weeks | None | Cloudflare + WireGuard + Database |
| 2. Settings UI | 1 week | Phase 1 | Complete admin interface |
| 3. Monitoring | 1 week | Phase 1 | Audit dashboard |
| 4. Location Detection | 1 week | Phase 1 | IP-based access control |
| 5. VPN Management | 2 weeks | Phase 1, 4 | Profile generation |
| 6. Setup Flow | 1 week | Phase 5 | User-facing wizard |
| 7. Stream Security | 1 week | Phase 5 | Token authentication |
| 8. Health Dashboard | 1 week | Phase 1 | System monitoring |
| 9. Testing & Docs | 2 weeks | All | Launch-ready system |

**Total Estimated Time:** 10-12 weeks (2.5-3 months)

---

## 🔄 FUTURE ENHANCEMENTS (Post-Launch)

### Phase 10: Mobile App (Optional)
- Native iOS/Android app
- Push notifications for alerts
- Faster streaming (native video players)
- Offline mode (cached credentials)

### Phase 11: Advanced Analytics (Optional)
- Usage reports (who watches most, when)
- Bandwidth optimization recommendations
- Predictive alerts (camera likely to fail)
- Compliance reports for insurance

### Phase 12: Multi-Site Support (Optional)
- Support multiple club locations
- Centralized management
- Cross-site access control

---

## ✅ FINAL CHECKLIST

Before marking this project complete:

- [ ] All 9 phases implemented and tested
- [ ] Security checklist 100% complete
- [ ] User documentation published
- [ ] Admin training completed
- [ ] Backup admin designated
- [ ] Emergency procedures documented
- [ ] Support contact info visible
- [ ] System health monitoring active
- [ ] Audit logging verified
- [ ] Performance metrics met
- [ ] Cross-platform testing passed
- [ ] Penetration testing passed
- [ ] User acceptance testing passed
- [ ] Production deployment successful
- [ ] Post-launch monitoring (1 week) completed

---

**Document Status:** ✅ READY FOR IMPLEMENTATION  
**Next Step:** Begin Phase 1 - Foundation & Infrastructure

**Questions or concerns?** Review this document with your team before starting implementation.
