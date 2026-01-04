# ISSUE 8 — Firewall & Access Hardening (Final Lockdown)

## Overview
This final stage of the Private Network integration ensures that the app is unreachable via the public internet or the local LAN, enforcing a strict "VPN-Only" access policy. This prevents unauthorized users on the premises or on the web from even reaching the login screen.

## Hardening Components

### 1. Network Segmentation (Windows Firewall)
The application host is configured with explicit firewall rules:
- **WireGuard (UDP 51820)**: Remained open to `Any` source to allow initial tunnel establishment.
- **Web Traffic (TCP 80/443)**: Restricted exclusively to the `10.20.0.0/24` subnet.
- **Explicit Block**: Generic IIS/Web rules are disabled, creating a "Default Deny" posture for any source IP outside the VPN range.

### 2. Provider Consistency
The system settings and onboarding generators have been aligned to use the `10.20.0.0/24` subnet:
- **DHCP**: `VpnConfigurationService` allocates IPs starting from `10.20.0.2`.
- **Profiles**: Apple `.mobileconfig` and official `.conf` files now default to the `10.20.0.0/24` allowed range.

### 3. Application-Level Enforcement
While the firewall handles the network layer, the application still performs location checks:
- **GFC Code**: `MainLayout.razor` checks `UserConnectionService.LocationType`.
- **Policy**: If `EnforceVpn` is enabled in `SystemSettings`, users coming from `Public` or `LAN` IPs are redirected to a `vpn-required` explanation page, providing a second layer of defense.

## Verification Checklist
- [ ] **LAN Test**: Attempting to reach the server IP from a computer on the same physical network should result in a "Connection Timed Out".
- [ ] **Public Test**: Attempting to reach `gfc.lovanow.com` without the VPN active should fail.
- [ ] **VPN Test**: Active WireGuard connection should allow immediate access to the dashboard.
- [ ] **DNS Test**: `nslookup gfc.lovanow.com` should resolve correctly to the server's private address while on VPN.

## Rollback Procedure
If an administrator is accidentally locked out:
1. Access the server console physically or via out-of-band management.
2. Run: `Disable-NetFirewallRule -Name "GFC-Secure-Web-Inbound"`
3. Enable default web rules: `Enable-NetFirewallRule -DisplayGroup "World Wide Web Services (HTTP Traffic-In)"`
