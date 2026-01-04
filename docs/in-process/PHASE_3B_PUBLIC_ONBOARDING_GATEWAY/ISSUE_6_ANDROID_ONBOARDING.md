# ISSUE 6 — Android Onboarding Flow

## Overview
Unlike Windows or iOS, Android does not support a consolidated "One-Click" installer or profile for both VPN and CA Certificates without the use of an MDM. The Android onboarding flow is therefore optimized for clarity, guiding the user through the manual steps in the most logical order to minimize friction.

## UX Design Principles
1.  **Ordered Workflow**: The gateway enforces a "Certificate First" then "Config Second" approach to ensure the user has established trust before attempting to connect.
2.  **Visual Cues**: High-visibility buttons for the Google Play Store and the specific "Optimized Android Setup" banner.
3.  **Cross-Flavor Support**: Instructions are tailored for both vanilla Android (Pixels) and Samsung's One UI, accounting for the different locations of security settings.

## Onboarding Sequence

### 1. App Installation
- The user is directed to the official WireGuard entry on the Google Play Store.
- The gateway provides a "Continue to Certificate" button once the app is installed.

### 2. Root CA Trust (`GFC_Root_CA.cer`)
- **Action**: User downloads the certificate.
- **Complexity**: Android considers CA certificates a high-risk action. The script provides the exact navigation path:
    - **Pixel/Standard**: `Settings -> Security -> More Security Settings -> Encryption & credentials -> Install a certificate -> CA certificate`.
    - **Samsung**: `Settings -> Biometrics and security -> Other security settings -> Install from device storage`.
- **Result**: Once installed, Chrome and other apps trust `https://gfc.lovanow.com`.

### 3. VPN Configuration (`gfc-access.conf`)
- **Action**: User downloads the `.conf` file.
- **Import**: The script instructs the user to use the "Import from file" option within the WireGuard app.
- **Result**: The VPN tunnel is established.

### 4. Verification
- The user confirms activation in the WireGuard app and returns to the browser to run the connectivity test.

## Technical Implementation
- **Platform Detection**: `setup.js` detects the `Android` string in the User Agent and activates the `android-help-container`.
- **MIME Support**: Configuration files are served with the standard WireGuard headers to encourage Android to offer "Open with WireGuard" if the app supports the file association.

## Future Enhancements
- **QR Code Support**: Generating a WireGuard-compatible QR code on the server would eliminate the "Download -> Import" step for Step 3. This is planned for a future iteration of the gateway.
