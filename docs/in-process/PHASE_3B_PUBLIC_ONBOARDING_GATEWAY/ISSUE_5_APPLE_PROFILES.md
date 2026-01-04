# ISSUE 5 — iOS / macOS Profile-Based Onboarding

## Overview
This document specifies the implementation of the Apple-native onboarding via `.mobileconfig` profiles. This allows iOS and macOS users to configure their VPN tunnel and trust the GFC internal Root CA in a single installation flow, significantly improving the user experience on Apple devices.

## Components

### 1. Apple Configuration Profile (`GFC-Access.mobileconfig`)
A standard Apple plist-based configuration file delivered with the `application/x-apple-aspen-config` MIME type. It contains two primary payloads:

#### **A. Root Certificate Payload (`com.apple.security.root`)**
- Contains the Base64-encoded `GFC_Root_CA.cer`.
- Automatically installs the certificate into the system/user keychain and prompts for trust.
- Eliminates the manual "Download -> Settings -> Profile Downloaded -> Install -> About -> Certificate Trust" multi-step process.

#### **B. VPN Payload (`com.apple.vpn.managed`)**
- **Type**: WireGuard (`com.wireguard.ios`).
- **Authentication**: Key-based (Private Key included).
- **DNS**: Configured to use the GFC internal DNS (`10.20.0.1`).
- **On-Demand**: Configured to "Connect" by default when network activity is detected (optional but recommended for directors).

### 2. Dynamic Profile Engine (`VpnConfigurationService.cs`)
The server generates the XML on-the-fly:
- **UUIDs**: Generates unique `PayloadUUID` and `PayloadVersion` for every request to ensure iOS treats it as a fresh profile.
- **Injected Data**: Injects the user's specific WireGuard private key, assigned IP, and the server's public key/endpoint.
- **Root CA**: Reads the `GFC_Root_CA.cer` from the infrastructure folder and embeds it as a data blob.

### 3. Signing (Production Note)
While the current implementation serves raw XML (which Windows/macOS/iOS will label as "Unsigned"), production environments should sign the mobileconfig using the `GFC_Root_CA.pfx` or a dedicated Apple Developer certificate.
- **Method**: CMS (PKCS#7) signing.
- **Tooling**: Can be achieved via BouncyCastle or `openssl smime`.

## User Experience (UX) Flow
1. User clicks the invite link on an iPhone or Mac.
2. Gateway detects the platform and shows **"Apple One-Click Setup"**.
3. User clicks **"Install Access Profile"**.
4. Safari prompts: "This website is trying to download a configuration profile. Do you want to allow this?"
5. User clicks **Allow**.
6. **iOS**: User goes to Settings -> Profile Downloaded -> Install.
7. **macOS**: System Settings -> Profiles -> Install.
8. Once installed, the VPN is fully configured and the Root CA is trusted.

## Security Considerations
- **Token Bound**: The endpoint `GET /api/onboarding/apple-profile` requires a valid onboarding token.
- **Ephemeral**: Profiles contain the private key at rest (standard for Apple profiles); the device's passcode/biometrics protect the profile after install.
