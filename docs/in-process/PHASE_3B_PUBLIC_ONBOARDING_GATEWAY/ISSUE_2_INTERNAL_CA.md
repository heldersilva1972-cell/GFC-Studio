# ISSUE 2 — Internal Certificate Authority & HTTPS Trust Chain

## Overview
To provide a premium and secure experience, GFC uses an internal Certificate Authority (CA) to sign TLS certificates for private services like `gfc.lovanow.com`. This eliminates "Not Secure" browser warnings when connected via VPN without requiring a public/expensive TLS setup that might expose internal hostnames.

## Goals
- Eliminate browser HTTPS warnings for internal GFC services.
- Automate the trust process as part of the onboarding flow.
- Maintain a private, secure trust chain.

## Requirements
- **Root CA**: Private self-signed certificate.
- **Leaf Certificate**: Domain-specific (e.g., `gfc.lovanow.com`) signed by the Root CA.
- **Browser Trust**: Achieved by installing the Root CA on client devices during onboarding.
- **Security**: Root CA private key must NEVER be stored in public directories or exposed to the frontend.

## Architecture

```
[ GFC Root CA ] (Private Key Securely Stored)
       │
       └─── [ TLS Leaf Cert: gfc.lovanow.com ] (Installed on IIS)
```

## Implementation Plan

### 1. Certificate Generation
- Create a Root CA using OpenSSL or PowerShell.
- Generate a Certificate Signing Request (CSR) for `gfc.lovanow.com`.
- Sign the CSR with the Root CA.
- Export the Root CA public certificate (`.crt` or `.pem`) for distribution.
- Export the Leaf Certificate with Private Key (`.pfx`) for IIS.

### 2. Onboarding Gateway Integration
- Add "Trust GFC Network" step to the onboarding wizard.
- Provide a clear download for the `GFC-Root-CA.crt`.
- Platform-specific instructions for installation:
  - **Windows**: Install to "Trusted Root Certification Authorities".
  - **iOS/macOS**: Download profile and enable full trust in Settings.
  - **Android**: Install as "CA Certificate" in security settings.

### 3. Backend Implementation
- New endpoint: `GET /api/onboarding/ca-cert`
- Serves the public Root CA certificate with the correct MIME type (`application/x-x509-ca-cert`).

### 4. IIS Configuration
- Bind the new TLS certificate to the `gfc.lovanow.com` site on port 443.
- Ensure only HTTPS is used for the private application.

## Acceptance Criteria
- [ ] Browser shows a green lock/no warnings for `https://gfc.lovanow.com` when connected via VPN.
- [ ] Root CA is successfully downloaded during onboarding.
- [ ] Renewal process is clearly documented.
- [ ] Private keys are securely separated from public assets.

## Security Considerations
- The Root CA private key (`.key`) should be stored offline or in a secure, non-web-accessible directory.
- The Public Onboarding Gateway only serves the public portion of the Root CA.
