# GFC Internal CA - Renewal Process

## Overview
TLS certificates issued by the internal CA (e.g., for `gfc.lovanow.com`) typically expire every 2 years. The Root CA itself should be valid for 10-20 years.

## 1. Renewing the Leaf Certificate (Every 2 Years)
When the certificate for `gfc.lovanow.com` is nearing expiry:

1.  **Generate a new certificate** using the same Root CA:
    ```powershell
    # Run the generation script again (or use specific commands)
    .\infrastructure\scripts\Generate-GfcCerts.ps1
    ```
2.  **Install the new PFX in IIS**:
    - Open IIS Manager.
    - Go to "Server Certificates".
    - Import `infrastructure\ca\GFC_Server.pfx`.
    - Go to the GFC Website -> Bindings.
    - Edit the HTTPS binding and select the new certificate.
3.  **Restart the website**.

## 2. Renewing the Root CA (Every 10-20 Years)
This is a major event as it requires all clients to re-onboard or manually update their trusted roots.

1.  **Generate a new Root CA**.
2.  **Update the Onboarding Gateway**: Replace the old `GFC_Root_CA.cer` with the new one.
3.  **Notify Users**: Users will need to download and trust the new certificate.
4.  **Re-issue all leaf certificates**: Sign new server certs with the new Root CA.

## 3. Automation (Future Enhancement)
For larger deployments, consider implementing ACME (Automated Certificate Management Environment) using a tool like **Smallstep CA** or **Certify the Web** to automate renewals in IIS.

## 4. Emergency Revocation
If the Root CA private key is compromised:
1.  **Immediately delete** the compromised key.
2.  **Generate a new Root CA** and new server certificates.
3.  **Force re-onboarding** for all users to establish the new trust chain.
