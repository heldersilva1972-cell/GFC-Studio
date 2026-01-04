# ISSUE 7 — Private Web App “Device Setup” Page (Director-Only)

## Overview
As users complete their initial onboarding via the public gateway, ongoing management of their secure access credentials must transition to the private, authenticated environment. The "Device Setup" page provides a self-service portal for Directors and Admins to manage their WireGuard profiles.

## Core Features

### 1. Unified Device Dashboard
- **Access**: Role-protected (`Director`, `Admin`) and only accessible over VPN/LAN.
- **Visibility**: Lists all active VPN profiles associated with the logged-in user.
- **Status Monitoring**: Displays the assigned IP address and "Last Seen" timestamp (handshake), allowing users to verify if their device is successfully connecting.

### 2. Artifact Re-delivery
- **Manual Config**: Users can re-download their `.conf` file at any time if they lose it or switch apps.
- **Tooling Links**: Provides direct links to the Windows One-Click script, Apple Profiles, and Root CA, though these still require a valid onboarding context for full automation.

### 3. Self-Service Management
- **Key Rotation**: Users can trigger a key rotation for a specific device. This generates a new private/public key pair and resets the creation date, requiring the user to update their WireGuard app with the new config.
- **Revocation**: Directors can self-revoke access for their own devices (e.g., if a phone is lost). This permanently invalidates the profile in the database.

## Technical Implementation
- **Component**: `DeviceSetup.razor` located in `Components/Pages/User/`.
- **Service Integration**: Uses `IVpnConfigurationService` for all profile operations (`GetUserProfilesAsync`, `RotateKeysAsync`, `RevokeProfileAsync`).
- **Security**: 
    - `Authorize` attribute ensures only authorized roles view the page.
    - Logic ensures users can only see/action profiles matching their own `UserId` (enforced by the service call).

## Auditability
All actions performed on this page (Rotation, Revocation, Download) are logged via the `ILogger` and recorded in the `VpnProfile` entity fields (`RevokedBy`, `RevokedReason`, `LastUsedAt`).

## Navigation
A new "Device Setup" link is added to the primary `NavMenu` for easy access by directors, appearing prominently in the secure control center.
