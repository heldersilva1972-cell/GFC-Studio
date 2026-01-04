# ISSUE 4 — Windows One-Click Onboarding Package

## Overview
This document specifies the implementation of the Windows One-Click Onboarding package. The goal is to maximize success rates for non-technical users by automating the installation of WireGuard, the trust of the internal Root CA, and the configuration of the VPN tunnel.

## Components

### 1. PowerShell Bootstrap (`Install-GfcVpn.ps1`)
The primary engine for automation. It performs the following sequence:
- **Elevation**: Detects if running as Administrator and re-launches with `-Verb RunAs` if required.
- **WireGuard Sync**: Checks for `wireguard.exe`. If missing, it downloads the official installer and executes `/install /quiet`.
- **CA Trust**: Fetches the Root CA certificate from the GFC API and imports it into the `Cert:\LocalMachine\Root` store.
- **Profile Import**: Fetches the user's `.conf` file and registers it as a Windows Service using `wireguard.exe /installtunnelservice`.
- **Telemetry**: Attempts a connection test to the private health check and POSTs results back to the GFC Onboarding API.

### 2. Dynamic Script Generation (`OnboardingController.cs`)
The server provides the script via `GET /api/onboarding/windows-setup`.
- Security: Requires a valid onboarding token.
- Customization: The server reads the script template and replaces placeholders with the specific `token` and `ApiUrl` for that session.
- MIME Type: Served as `application/x-powershell`.

### 3. Gateway UI Integration
The Onboarding Gateway (`index.html`) detects the `Windows` platform and presents a high-visibility banner:
- **Button**: "Run One-Click Setup (Recommended)".
- **Feedback**: Redirects the user to the final confirmation step while the script runs in the background.

## User Experience (UX) Flow
1. User clicks the invite link.
2. Gateway detects Windows and shows the "One-Click" option.
3. User clicks "Run One-Click Setup".
4. Browser downloads `Setup-GFC-VPN.ps1`.
5. User runs the script (Double-click or Right-click -> Run with PowerShell).
6. A single Windows UAC prompt appears.
7. The script window shows progress.
8. Once finished, the Gateway UI automatically updates to show "Connected!".

## Security Considerations
- **Token Bound**: The generated script is only valid as long as the onboarding token is active.
- **Signed Code**: While currently unsigned (W1), transition to a signed `.exe` or `MSIX` (W2) is the recommended next step for production to avoid "Unknown Publisher" warnings.
- **Cleanup**: The script removes temporary config files from `$env:TEMP` immediately after import.
