# Network Migration Wizard - Feature Specification

## Overview
A guided UI wizard that helps administrators seamlessly migrate controllers between network configurations (Same-LAN ↔ VPN) with zero downtime and automatic validation.

## User Story
**As an administrator**, I want to easily switch my access controller from local LAN to VPN (or back) **without technical knowledge**, so that I can relocate hardware or change network topology without breaking the system.

## UI Location
**Admin → Controllers → [Select Controller] → Network Settings → "Change Network Configuration"**

## Wizard Flow

### Step 1: Current Configuration Detection
```
┌─────────────────────────────────────────────────┐
│  Network Migration Wizard                       │
├─────────────────────────────────────────────────┤
│                                                 │
│  Current Configuration:                         │
│  ┌───────────────────────────────────────────┐ │
│  │ 🏠 Same LAN                               │ │
│  │ Controller: Main Door Controller          │ │
│  │ IP Address: 192.168.0.100                 │ │
│  │ Port: 60000                               │ │
│  │ Status: ✅ Connected (Last seen: 2s ago)  │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  Where do you want to move this controller?     │
│                                                 │
│  ○ Keep on Same LAN (change IP only)           │
│  ● Move to VPN Network                          │
│  ○ Move to Different LAN (via VPN)             │
│                                                 │
│            [Cancel]  [Next: Configure VPN →]    │
└─────────────────────────────────────────────────┘
```

### Step 2: VPN Configuration (if VPN selected)
```
┌─────────────────────────────────────────────────┐
│  VPN Configuration                              │
├─────────────────────────────────────────────────┤
│                                                 │
│  Select or Create VPN Profile:                  │
│  ┌───────────────────────────────────────────┐ │
│  │ ● Create New WireGuard Profile            │ │
│  │ ○ Use Existing: "Remote Site VPN"        │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  New VPN Profile Details:                       │
│  Profile Name: [Remote Controller VPN____]      │
│  VPN Type: [WireGuard ▼]                        │
│                                                 │
│  WireGuard Configuration:                       │
│  Server IP: [10.99.0.1_______________]          │
│  Server Port: [51820____]                       │
│  Peer IP: [10.99.0.2_______________]            │
│                                                 │
│  🔑 Keys (Auto-generated):                      │
│  [✓] Generate new key pair                      │
│  [ ] Use existing keys                          │
│                                                 │
│  ℹ️ Keys will be generated and displayed for    │
│     configuration on the remote device          │
│                                                 │
│       [← Back]  [Next: Test Connection →]       │
└─────────────────────────────────────────────────┘
```

### Step 3: Connection Test
```
┌─────────────────────────────────────────────────┐
│  Test VPN Connection                            │
├─────────────────────────────────────────────────┤
│                                                 │
│  Testing connection to controller via VPN...    │
│                                                 │
│  ┌───────────────────────────────────────────┐ │
│  │ ✅ VPN Tunnel Established                 │ │
│  │ ✅ Can ping 10.99.0.2                     │ │
│  │ ⏳ Testing UDP port 60000...              │ │
│  │ ❌ Controller not responding              │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  ⚠️ Controller is not responding on VPN IP      │
│                                                 │
│  Possible issues:                               │
│  • Controller not yet configured with VPN       │
│  • Firewall blocking UDP port 60000            │
│  • Controller powered off                       │
│                                                 │
│  What would you like to do?                     │
│                                                 │
│  [← Back]  [Skip Test]  [Retry Test]  [Next →] │
└─────────────────────────────────────────────────┘
```

### Step 4: Migration Plan Review
```
┌─────────────────────────────────────────────────┐
│  Review Migration Plan                          │
├─────────────────────────────────────────────────┤
│                                                 │
│  The following changes will be made:            │
│                                                 │
│  Controller: Main Door Controller               │
│                                                 │
│  FROM (Current):                                │
│  ┌───────────────────────────────────────────┐ │
│  │ Network: Same LAN                         │ │
│  │ IP: 192.168.0.100:60000                   │ │
│  │ VPN: None                                 │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  TO (New):                                      │
│  ┌───────────────────────────────────────────┐ │
│  │ Network: VPN (WireGuard)                  │ │
│  │ IP: 10.99.0.2:60000                       │ │
│  │ VPN Profile: Remote Controller VPN        │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  ⚠️ Important:                                  │
│  • Old IP will be kept as backup for 24 hours  │
│  • You can rollback if VPN connection fails    │
│  • Door access will continue working           │
│                                                 │
│       [← Back]  [Apply Migration]               │
└─────────────────────────────────────────────────┘
```

### Step 5: Migration in Progress
```
┌─────────────────────────────────────────────────┐
│  Migrating Controller...                        │
├─────────────────────────────────────────────────┤
│                                                 │
│  ✅ Saved old configuration as backup           │
│  ✅ Created VPN profile                         │
│  ✅ Updated controller IP address               │
│  ⏳ Testing connection on new IP...             │
│  ⏳ Syncing card data to controller...          │
│  ⬜ Verifying door access...                    │
│                                                 │
│  [████████████░░░░░░░░░░] 60%                   │
│                                                 │
│  Please wait...                                 │
│                                                 │
└─────────────────────────────────────────────────┘
```

### Step 6: Success / Rollback Option
```
┌─────────────────────────────────────────────────┐
│  ✅ Migration Successful!                       │
├─────────────────────────────────────────────────┤
│                                                 │
│  Controller successfully migrated to VPN        │
│                                                 │
│  New Configuration:                             │
│  ┌───────────────────────────────────────────┐ │
│  │ Network: VPN (WireGuard)                  │ │
│  │ IP: 10.99.0.2:60000                       │ │
│  │ Status: ✅ Connected                       │ │
│  │ Last Sync: Just now                       │ │
│  │ Cards Synced: 150/150                     │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  🔑 WireGuard Configuration for Remote Device:  │
│  ┌───────────────────────────────────────────┐ │
│  │ [Interface]                               │ │
│  │ PrivateKey = ABC123...                    │ │
│  │ Address = 10.99.0.2/24                    │ │
│  │                                           │ │
│  │ [Peer]                                    │ │
│  │ PublicKey = XYZ789...                     │ │
│  │ Endpoint = your-server.com:51820          │ │
│  │ AllowedIPs = 10.99.0.0/24                 │ │
│  └───────────────────────────────────────────┘ │
│                                                 │
│  [📋 Copy Config]  [📥 Download QR Code]        │
│                                                 │
│  ⚠️ Backup available for 24 hours              │
│  [Rollback to Old IP]  [Done]                   │
│                                                 │
└─────────────────────────────────────────────────┘
```

## Backend Implementation

### New Service: `INetworkMigrationService`

```csharp
public interface INetworkMigrationService
{
    // Detect current network configuration
    Task<NetworkConfiguration> DetectCurrentConfigAsync(int controllerId);
    
    // Validate new configuration
    Task<ValidationResult> ValidateNewConfigAsync(NetworkMigrationRequest request);
    
    // Test VPN connection
    Task<ConnectionTestResult> TestVpnConnectionAsync(VpnProfile vpnProfile, string targetIp, int port);
    
    // Execute migration with rollback capability
    Task<MigrationResult> ExecuteMigrationAsync(NetworkMigrationRequest request);
    
    // Rollback to previous configuration
    Task<bool> RollbackMigrationAsync(int controllerId);
    
    // Generate WireGuard config
    Task<WireGuardConfig> GenerateWireGuardConfigAsync(VpnProfile profile);
}
```

### Database Changes

```sql
-- Add to Controllers table
ALTER TABLE Controllers ADD 
    NetworkType NVARCHAR(50) DEFAULT 'LAN',  -- 'LAN', 'VPN', 'Remote'
    VpnProfileId INT NULL,
    BackupIpAddress NVARCHAR(50) NULL,
    BackupExpiresUtc DATETIME NULL,
    LastMigrationUtc DATETIME NULL;

-- VPN Profiles table (already exists)
-- Just ensure it has these fields
ALTER TABLE VpnProfiles ADD
    WireGuardPublicKey NVARCHAR(MAX) NULL,
    WireGuardPrivateKey NVARCHAR(MAX) NULL,
    WireGuardPeerPublicKey NVARCHAR(MAX) NULL;
```

### Key Features

1. **Auto-Detection**: Automatically detects if controller is on LAN or VPN
2. **Guided Wizard**: Step-by-step process with validation
3. **Connection Testing**: Tests VPN before committing changes
4. **Rollback Safety**: Keeps old config for 24 hours
5. **Zero Downtime**: Controller keeps working during migration
6. **QR Code Export**: Easy WireGuard config for mobile devices
7. **Validation**: Checks firewall, ports, connectivity before applying

## User Benefits

✅ **No Technical Knowledge Required** - Wizard guides through everything
✅ **Safe Migration** - Automatic rollback if anything fails
✅ **Visual Feedback** - See exactly what's happening
✅ **Copy-Paste Config** - WireGuard config ready to use
✅ **Test Before Apply** - Validate connection before committing
✅ **Audit Trail** - All migrations logged for compliance

## Next Steps

1. Create `NetworkMigrationWizard.razor` component
2. Implement `NetworkMigrationService.cs`
3. Add WireGuard key generation utility
4. Create migration database schema
5. Add to Controllers page as "Change Network" button

**Should I proceed with implementing this wizard?**
