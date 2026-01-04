# Issue 13: Restricted Onboarding Artifacts

## Overview
This update hardens the security of onboarding artifacts (Root CA certificate, platform installers) by restricting access based on user roles and device state. It prevents anonymous or unauthorized access to sensitive downloads.

## Changes Implemented

### 1. Root CA Certificate Restrictions
- **Endpoint**: `/api/onboarding/ca-cert`
- **Previous State**: Publicly accessible without authentication.
- **New State**: Requires either:
  - **Admin Role**: Authenticated session with Admin role.
  - **Valid Token**: A valid, unexpired onboarding token passed via `?token=...` query parameter.
- **Impact**: Anonymous users/scanners cannot download the internal Root CA certificate.

### 2. Platform Installers Restrictions
- **UI Visibility**:
  - **First-Time Users (State 1)**: See ZERO download links.
  - **Managed Users (State 2)**: See "Reinstall" links for Windows Installer and Apple Profile.
  - **Admins (State 3)**: See "Reinstall" links AND Root CA download.
- **Mechanism**:
  - Reinstall links generate a short-lived (1 hour) onboarding token on-the-fly.
  - This token is appended to the download URL, allowing the backend to validate the request authorised by the frontend session.

### 3. Windows Setup Script Update
- The `Install-GfcVpn.ps1` script was updated to append the `$Token` to the CA certificate download URL.
- This ensures the automated setup process continues to work seamlessly with the new backend restrictions, using the legitimate onboarding token.

## Testing Verification

### Test 1: Anonymous Access (Fail)
- Open Incognito window.
- Navigate to `/api/onboarding/ca-cert`.
- **Expected**: 403 Forbidden (or login redirect if handled by middleware, but Controller returns 403 explicitly).

### Test 2: Admin Access (Pass)
- Log in as Admin.
- Navigate to `/api/onboarding/ca-cert`.
- **Expected**: File download starts (GFC_Root_CA.cer).

### Test 3: Managed User Reinstall (Pass)
- Log in as Director with existing devices.
- Go to "Secure Access & Devices".
- Click "Windows Installer" or "Apple Profile" in "Tools & Artifacts" section.
- **Expected**: File download starts.

### Test 4: First-Time User (Pass)
- Log in as Director with NO devices.
- Go to "Secure Access & Devices".
- **Expected**: No "Tools & Artifacts" section visible. No download buttons.

### Test 5: Setup Script (Pass)
- Generate Invite Link (Admin).
- Run Windows Setup with token.
- **Expected**: Script successfully downloads CA cert using the token.

## Security Benefits
- **Reduced Attack Surface**: Internal CA cert is no longer exposed to the public internet.
- **Controlled Distribution**: Artifacts are only available to authorized personnel or devices.
- **Audit Compliance**: All downloads are now tied to a user session or valid token, improving auditability.
