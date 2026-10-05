# GFC Connect: Release Signing, Sideloading & Distribution Guide

This guide provides end-to-end instructions for creating the release signing keystore, packaging the production APK, deploying it to your host server, and onboarding members.

---

## 1. Generating the Release Signing Keystore (`keytool`)

To sign the APK for production distribution outside the Google Play Store, generate a secure hardware-grade keystore using Java's `keytool`:

1. Open PowerShell or Command Prompt on your computer.
2. Run the following command:

```powershell
keytool -genkeypair -v `
  -keystore "gfc-connect-release.jks" `
  -alias "gfc-connect-key" `
  -keyalg RSA `
  -keysize 4096 `
  -validity 10000 `
  -dname "CN=Gloucester Fraternity Club, OU=IT, O=GFC, L=Local, ST=State, C=US"
```

3. Enter a strong password when prompted and save it securely in your password manager.
4. Place the generated `gfc-connect-release.jks` in a secure location (e.g., `C:\GFC_Storage\Keys\gfc-connect-release.jks`).

---

## 2. Compiling the Production Release APK

In Android Studio or via Gradle terminal inside `apps/GFC-Connect`:

```powershell
# Build signed production release APK
.\gradlew assembleRelease
```

The output APK will be generated at:
`apps/GFC-Connect/app/build/outputs/apk/release/app-release.apk`

Rename this file to **`gfc-connect.apk`**.

---

## 3. Deploying to the Host Server

Copy the compiled APK to your host server's private package storage:

* **File Location:** `C:\inetpub\GFCWebApp\App_Data\Packages\gfc-connect.apk`
* **Version Metadata:** Update `C:\inetpub\GFCWebApp\App_Data\Packages\version.json` if bumping the version:

```json
{
  "latestVersionCode": 100,
  "latestVersionName": "1.0.0",
  "downloadUrl": "/api/app/download/latest.apk",
  "mandatoryUpdate": false,
  "releaseNotes": "Initial release of GFC Connect with biometrics and push notifications.",
  "releasedAt": "2026-10-04T00:00:00Z"
}
```

---

## 4. End-User Onboarding & First-Time Installation

### Step A: Admin Generates Setup Code
1. Open your web app $\rightarrow$ navigate to **User Management** $\rightarrow$ click the **GFC Connect** tab.
2. Click **"Onboard Device"**, select the member, and click **"Generate Setup Code"**.
3. A **6-digit code** (e.g., `482-910`) and a **QR Code** appear with a 15-minute countdown.

### Step B: Member Downloads the APK
1. The member points their phone's camera at the QR code (or types `gfc.lovanow.com/app` into Chrome on their phone).
2. The phone displays the clean **GFC Connect** download screen.
3. The member enters their 6-digit code and taps **"VERIFY & DOWNLOAD APK"**.
4. The server validates the code and streams `gfc-connect.apk` directly to their downloads.

### Step C: One-Time Android Permission ("Install Unknown Apps")
1. The member taps the downloaded APK notification.
2. Android displays: *"For your security, your phone is not allowed to install unknown apps from this source."*
3. The member taps **Settings** $\rightarrow$ toggles **"Allow from this source"** for Chrome $\rightarrow$ taps Back $\rightarrow$ taps **Install**.

### Step D: Open & Biometric Activation
1. The member opens **GFC Connect**.
2. The app pairs with their account and prompts: *"Enable Fingerprint / Face Unlock?"*.
3. The member touches their fingerprint sensor.
4. **Done!** From then on, they access the app with 100% biometric security.

---

## 5. In-App Auto Updates (Subsequent Releases)

When you deploy a new version to `App_Data\Packages\`:
* Active GFC Connect apps check `GET /api/app/version` on launch.
* An update card appears automatically in the app: *"Update Available (v1.0.1)"*.
* The user taps **"Update"** $\rightarrow$ the app streams the new APK to its cache and launches the Android installer.
* The user taps **Update** on Android. No browser visit or settings toggle required!
