# ISSUE 3 — WireGuard Peer Lifecycle Management

## Overview
This document specifies the server-side lifecycle management for WireGuard peers (VPN Profiles). Each user (Director) is assigned a unique, persistent VPN profile that allows for secure, auditable access to the GFC Private Network.

## Peer Statuses
- **Active**: Profile is valid and the public key is registered on the WireGuard server.
- **Revoked**: Access has been terminated. The peer is removed from the server configuration.
- **Expired**: Typically used for temporary access (not standard for Director profiles).

## Peer Data Structure (VpnProfiles Table)
| Field | Description |
|-------|-------------|
| `Id` | Primary Key |
| `UserId` | Owner of the profile |
| `PublicKey` | Base64 WireGuard Public Key |
| `PrivateKey` | Base64 WireGuard Private Key (Encrypted at rest) |
| `AssignedIP` | Specific /32 IP within the VPN subnet (e.g., 10.8.0.5) |
| `DeviceName` | Metadata (e.g., "iPhone 15", "Windows Desktop") |
| `CreatedAt` | Timestamp of creation |
| `RevokedAt` | Timestamp of revocation (NULL = Active) |
| `RevokedBy` | User ID who performed the revocation |
| `RevokedReason` | Reason for revocation |

## Lifecycle Processes

### 1. Peer Generation (Registration)
Triggered during the first successful onboarding for a user.
1.  Verify the user doesn't already have an **Active** profile.
2.  Generate a unique Curve25519 keypair.
3.  Allocate the next available IP in the 10.8.0.0/24 range.
4.  Store metadata and keys in the `VpnProfiles` table.
5.  Generate the `.conf` file and deliver it to the user.

### 2. IP Allocation Strategy
- **Reservations**: 10.8.0.1 is reserved for the VPN Server.
- **Dynamic Pool**: 10.8.0.2 to 10.8.0.254.
- **Assignment**: Sequential allocation based on the highest existing octet in the `VpnProfiles` table.

### 3. Revocation (Offboarding)
1.  Admin marks the profile as `Revoked`.
2.  System sets `RevokedAt`, `RevokedBy`, and `RevokedReason`.
3.  The WireGuard Sync Service (external) detects the change and removes the peer from the `wg0` interface.
4.  Optionally, all other profiles for the same user can be revoked simultaneously.

### 4. Key Rotation
1.  Generate a new keypair for an existing profile.
2.  Update the `VpnProfiles` table.
3.  User must re-download and import the new configuration.

## Auditability
All peer creation, modification, and revocation events are logged in the application logs with the associated User IDs.

## Server Integration
The WireGuard server should periodically sync its peer list with the `VpnProfiles` table where `RevokedAt` is NULL. This can be achieved via a cron job or a web hook.
