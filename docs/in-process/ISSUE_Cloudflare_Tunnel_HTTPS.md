# ISSUE 1 — Enable Trusted HTTPS via Cloudflare Tunnel (No Public Exposure)

**Title**: Enable trusted HTTPS for gfc.lovanow.com using Cloudflare Tunnel (VPN-safe, no inbound ports)

**Labels**: `security` `infrastructure` `https` `cloudflare` `blocking` `phase-3`

**Milestone**: Secure External Access (Phase 3)

**Depends on**: None (foundational)

---

## ❗ Problem

The GFC Web App currently uses a self-signed TLS certificate, causing:

- ❌ Browser "Not Secure" warnings
- ❌ Service Worker / PWA registration failures
- ❌ Blazor WebSocket downgrade to long polling
- ❌ Login failures outside Visual Studio
- ❌ Impossible onboarding for non-technical users (especially mobile)

Modern browsers will not trust self-signed certificates, and installing root certs on user devices is not acceptable.

---

## 🎯 Goal

Provide browser-trusted HTTPS for `https://gfc.lovanow.com` without exposing the server publicly, without opening inbound ports, and without requiring client-side configuration.

---

## ✅ Solution Overview

Use **Cloudflare Tunnel** (cloudflared) to:

1. ✅ Terminate TLS at Cloudflare (public trust)
2. ✅ Create an outbound-only encrypted tunnel to IIS
3. ✅ Keep the IIS server private (no NAT, no port forwarding)
4. ✅ Preserve VPN-only or Access-controlled behavior

---

## 🔧 Implementation Tasks

### Cloudflare Configuration

- [ ] Add `lovanow.com` to Cloudflare (DNS-only initially)
- [ ] Create `gfc.lovanow.com` DNS record (proxied)
- [ ] Enable Cloudflare Universal SSL
- [ ] Confirm valid certificate chain via browser

### Server (Windows / IIS)

- [ ] Install `cloudflared` on server
- [ ] Authenticate tunnel with Cloudflare account
- [ ] Create named tunnel (e.g., `gfc-webapp`)
- [ ] Configure tunnel to forward: `https://gfc.lovanow.com` → `http://localhost:8080`
- [ ] Run tunnel as a Windows service
- [ ] Verify no inbound ports are open

### Security Verification

- [ ] Confirm site loads over HTTPS without warnings
- [ ] Confirm Service Worker registers successfully
- [ ] Confirm Blazor uses WebSockets (no fallback)
- [ ] Confirm site unreachable without Cloudflare path

---

## 🔒 Security Notes

- ✅ No inbound firewall rules required
- ✅ No public exposure of IIS
- ✅ Tunnel is outbound-only
- ✅ TLS managed automatically by Cloudflare
- ✅ Compatible with later VPN-only or Access enforcement

---

## ✔ Acceptance Criteria

- [ ] `https://gfc.lovanow.com` shows valid HTTPS
- [ ] No browser warnings
- [ ] PWA & Service Worker load
- [ ] Login works from browser (not just Visual Studio)
- [ ] No client-side cert installation required
- [ ] Tunnel service auto-starts on server reboot
- [ ] Documentation complete and verified

---

## 📚 Implementation Resources

All necessary documentation and scripts have been created in the repository:

### 1. **Comprehensive Setup Guide**
   - **Location**: `infrastructure/CLOUDFLARE_TUNNEL_SETUP.md`
   - **Contents**: Step-by-step instructions for complete setup
   - **Phases**: 
     - Phase 1: Cloudflare Configuration
     - Phase 2: Server Setup (Windows/IIS)
     - Phase 3: Security Verification
     - Phase 4: Production Deployment

### 2. **Automated Installation Script**
   - **Location**: `infrastructure/scripts/Install-CloudflareTunnel.ps1`
   - **Purpose**: Automates cloudflared installation and configuration
   - **Features**:
     - Interactive wizard with prompts
     - Automatic download and installation
     - Tunnel creation and DNS routing
     - Windows service installation
     - Comprehensive error handling
   - **Usage**:
     ```powershell
     # Run as Administrator
     cd "infrastructure\scripts"
     .\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com
     ```

### 3. **Verification Script**
   - **Location**: `infrastructure/scripts/Verify-CloudflareTunnel.ps1`
   - **Purpose**: Comprehensive testing of tunnel setup
   - **Tests**:
     - Cloudflared installation
     - Authentication status
     - Tunnel configuration
     - Windows service status
     - Local IIS endpoint
     - HTTPS certificate validation
     - DNS resolution
     - Port security
     - Tunnel connectivity
     - PWA readiness
   - **Usage**:
     ```powershell
     .\Verify-CloudflareTunnel.ps1 -Hostname gfc.lovanow.com
     ```

### 4. **Quick Reference Guide**
   - **Location**: `infrastructure/CLOUDFLARE_TUNNEL_QUICK_REFERENCE.md`
   - **Contents**:
     - Common commands
     - Troubleshooting guide
     - Diagnostic procedures
     - Maintenance tasks
     - Emergency procedures
     - Health check checklist

---

## 🚀 Implementation Steps

### Step 1: Review Documentation
```powershell
# Open the comprehensive setup guide
notepad "infrastructure\CLOUDFLARE_TUNNEL_SETUP.md"
```

### Step 2: Run Installation Script
```powershell
# Open PowerShell as Administrator
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\infrastructure\scripts"

# Run installation wizard
.\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com
```

The script will:
1. Download and install cloudflared
2. Open browser for Cloudflare authentication
3. Create tunnel and configure DNS
4. Generate configuration files
5. Install and start Windows service

### Step 3: Verify Installation
```powershell
# Run verification script
.\Verify-CloudflareTunnel.ps1 -Hostname gfc.lovanow.com
```

Review the output for any failures or warnings.

### Step 4: Test in Browser
1. Open browser to: `https://gfc.lovanow.com`
2. Verify:
   - ✅ No certificate warnings
   - ✅ Padlock icon shows "Secure"
   - ✅ Certificate issued by Cloudflare/Let's Encrypt

### Step 5: Test Service Worker
1. Open browser DevTools (F12)
2. Navigate to "Application" tab
3. Check "Service Workers" section
4. Verify registration is successful

### Step 6: Test Login
1. Log out of application
2. Log in from browser (not Visual Studio)
3. Verify login succeeds
4. Test from external network/mobile device

### Step 7: Monitor Service
```powershell
# Check service status
Get-Service cloudflared

# View recent logs
Get-EventLog -LogName Application -Source cloudflared -Newest 20
```

---

## 🐛 Troubleshooting

If you encounter issues, refer to:

1. **Quick Reference Guide**: `infrastructure/CLOUDFLARE_TUNNEL_QUICK_REFERENCE.md`
   - Contains solutions for common problems
   - Diagnostic commands
   - Emergency procedures

2. **Setup Guide**: `infrastructure/CLOUDFLARE_TUNNEL_SETUP.md`
   - Detailed troubleshooting section
   - Step-by-step problem resolution

3. **Verification Script**: Run to identify specific issues
   ```powershell
   .\Verify-CloudflareTunnel.ps1
   ```

---

## 📊 Testing Checklist

### Pre-Implementation
- [ ] IIS is running and accessible on `http://localhost:8080`
- [ ] Domain `lovanow.com` is registered and accessible
- [ ] Cloudflare account created
- [ ] Administrator access to server

### Post-Implementation
- [ ] HTTPS loads without warnings
- [ ] Certificate is valid and trusted
- [ ] Service Worker registers successfully
- [ ] PWA installs on mobile devices
- [ ] Blazor uses WebSockets (not long polling)
- [ ] Login works from external networks
- [ ] No inbound firewall ports required
- [ ] Tunnel service auto-starts on reboot
- [ ] Site unreachable via direct IP (only via Cloudflare)
- [ ] Performance is acceptable (latency < 100ms)

### Security Verification
- [ ] Port 8080 only bound to localhost (not 0.0.0.0)
- [ ] External port scan shows all ports closed
- [ ] HTTPS certificate chain is valid
- [ ] No browser security warnings
- [ ] HSTS header present (optional but recommended)

---

## 📝 Configuration Details

### Tunnel Configuration
```yaml
tunnel: <TUNNEL-ID>
credentials-file: C:\Users\<YourUser>\.cloudflared\<TUNNEL-ID>.json

ingress:
  - hostname: gfc.lovanow.com
    service: http://localhost:8080
    originRequest:
      noTLSVerify: true
  - service: http_status:404
```

### DNS Configuration
- **Type**: CNAME
- **Name**: `gfc`
- **Target**: `<TUNNEL-ID>.cfargotunnel.com`
- **Proxy**: Enabled (orange cloud)

### SSL/TLS Mode
- **Cloudflare Setting**: Full or Full (strict)
- **Origin Certificate**: Self-signed (acceptable with "Full" mode)

---

## 🔄 Maintenance

### Regular Checks (Weekly)
```powershell
# Run health check
.\Verify-CloudflareTunnel.ps1

# Check service status
Get-Service cloudflared

# Review logs for errors
Get-EventLog -LogName Application -Source cloudflared -Newest 50
```

### Updates (Monthly)
```powershell
# Update cloudflared to latest version
Stop-Service cloudflared
Invoke-WebRequest -Uri "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe" -OutFile "C:\Program Files\cloudflared\cloudflared.exe"
Start-Service cloudflared
```

---

## 📞 Support

### Documentation
- Setup Guide: `infrastructure/CLOUDFLARE_TUNNEL_SETUP.md`
- Quick Reference: `infrastructure/CLOUDFLARE_TUNNEL_QUICK_REFERENCE.md`

### Cloudflare Resources
- Tunnel Docs: https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/
- Community: https://community.cloudflare.com/

### Scripts
- Installation: `infrastructure/scripts/Install-CloudflareTunnel.ps1`
- Verification: `infrastructure/scripts/Verify-CloudflareTunnel.ps1`

---

## 💡 Notes

- **Estimated Time**: 2-3 hours (including DNS propagation)
- **Downtime**: None (tunnel runs alongside existing setup)
- **Reversibility**: High (can disable tunnel anytime)
- **Cost**: $0 (Cloudflare Free tier is sufficient)
- **Prerequisites**: Domain ownership, Cloudflare account, Administrator access

---

## ✅ Definition of Done

- [ ] All implementation tasks completed
- [ ] All acceptance criteria met
- [ ] All tests passing
- [ ] Documentation reviewed and accurate
- [ ] Service running and auto-starting
- [ ] External users can access via HTTPS
- [ ] No security warnings or errors
- [ ] Monitoring and maintenance procedures in place

---

**Created**: 2026-01-04  
**Priority**: High  
**Complexity**: Medium  
**Estimated Effort**: 2-3 hours  
**Status**: Ready for Implementation
