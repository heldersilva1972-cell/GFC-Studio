# Cloudflare Tunnel - Quick Reference Guide

## 🚀 Quick Start Commands

### Installation (One-Time Setup)
```powershell
# Run as Administrator
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\infrastructure\scripts"
.\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com
```

### Verification
```powershell
# Run anytime to check status
.\Verify-CloudflareTunnel.ps1 -Hostname gfc.lovanow.com
```

---

## 📋 Common Commands

### Service Management
```powershell
# Check service status
Get-Service cloudflared

# Start service
Start-Service cloudflared

# Stop service
Stop-Service cloudflared

# Restart service
Restart-Service cloudflared

# View service logs
Get-EventLog -LogName Application -Source cloudflared -Newest 20
```

### Tunnel Information
```powershell
# List all tunnels
cloudflared tunnel list

# Get tunnel details
cloudflared tunnel info gfc-webapp

# Test tunnel manually (Ctrl+C to stop)
cloudflared tunnel run gfc-webapp
```

### Configuration
```powershell
# View config file
notepad "$env:USERPROFILE\.cloudflared\config.yml"

# View credentials
notepad "$env:USERPROFILE\.cloudflared\<TUNNEL-ID>.json"

# Validate config
cloudflared tunnel ingress validate
```

---

## 🔧 Troubleshooting Guide

### Problem: Service won't start

**Symptoms:**
- `Start-Service cloudflared` fails
- Service shows "Stopped" status

**Solutions:**
1. Check config file syntax:
   ```powershell
   cloudflared tunnel ingress validate
   ```

2. Verify credentials file exists:
   ```powershell
   dir "$env:USERPROFILE\.cloudflared\*.json"
   ```

3. Check service logs:
   ```powershell
   Get-EventLog -LogName Application -Source cloudflared -Newest 10
   ```

4. Reinstall service:
   ```powershell
   cloudflared service uninstall
   cloudflared service install
   Start-Service cloudflared
   ```

---

### Problem: 502 Bad Gateway

**Symptoms:**
- Browser shows Cloudflare 502 error
- "Bad Gateway" message

**Solutions:**
1. Verify IIS is running:
   ```powershell
   Get-Service W3SVC
   iisreset /status
   ```

2. Test local endpoint:
   ```powershell
   Invoke-WebRequest http://localhost:8080
   ```

3. Check port in config matches IIS:
   ```powershell
   notepad "$env:USERPROFILE\.cloudflared\config.yml"
   # Verify: service: http://localhost:8080
   ```

4. Restart both services:
   ```powershell
   iisreset /restart
   Restart-Service cloudflared
   ```

---

### Problem: Certificate warnings persist

**Symptoms:**
- Browser still shows "Not Secure"
- Certificate errors

**Solutions:**
1. Clear browser cache:
   - Chrome: `Ctrl+Shift+Delete` → Clear cached images and files
   - Edge: Same as Chrome

2. Hard refresh page:
   - `Ctrl+Shift+R` or `Ctrl+F5`

3. Verify DNS is using Cloudflare:
   ```powershell
   Resolve-DnsName gfc.lovanow.com
   # Should show CNAME to *.cfargotunnel.com
   ```

4. Check Cloudflare SSL mode:
   - Dashboard → SSL/TLS → Overview
   - Should be: **Full** or **Full (strict)**

5. Wait for SSL propagation (5-10 minutes after setup)

---

### Problem: Service Worker won't register

**Symptoms:**
- PWA won't install
- Console error: "Service Worker registration failed"

**Solutions:**
1. Verify HTTPS is working (no certificate warnings)

2. Check browser console (F12) for specific errors

3. Verify Service Worker file is accessible:
   ```
   https://gfc.lovanow.com/service-worker.js
   ```

4. Clear Service Workers:
   - Chrome DevTools → Application → Service Workers
   - Click "Unregister" on old workers
   - Refresh page

5. Check manifest.json is valid:
   ```
   https://gfc.lovanow.com/manifest.json
   ```

---

### Problem: WebSocket connections fail

**Symptoms:**
- Blazor falls back to long polling
- Console: "WebSocket connection failed"

**Solutions:**
1. Enable WebSockets in Cloudflare:
   - Dashboard → Network → WebSockets: **On**

2. Verify tunnel config allows WebSockets (should by default)

3. Check browser DevTools → Network → WS tab
   - Should see `_blazor?id=...` with status 101

4. Restart tunnel service:
   ```powershell
   Restart-Service cloudflared
   ```

---

### Problem: Tunnel shows "INACTIVE"

**Symptoms:**
- `cloudflared tunnel list` shows INACTIVE
- Site not accessible

**Solutions:**
1. Check if service is running:
   ```powershell
   Get-Service cloudflared
   ```

2. Start service if stopped:
   ```powershell
   Start-Service cloudflared
   ```

3. Verify config file path:
   ```powershell
   Test-Path "$env:USERPROFILE\.cloudflared\config.yml"
   ```

4. Check tunnel routing:
   ```powershell
   cloudflared tunnel route dns gfc-webapp gfc.lovanow.com
   ```

---

### Problem: DNS not resolving

**Symptoms:**
- "DNS_PROBE_FINISHED_NXDOMAIN"
- Site not found

**Solutions:**
1. Verify domain is active in Cloudflare:
   - Dashboard → Websites → lovanow.com
   - Status should be: **Active**

2. Check nameservers at registrar:
   - Should point to Cloudflare's nameservers
   - Example: `ns1.cloudflare.com`, `ns2.cloudflare.com`

3. Verify DNS record exists:
   - Dashboard → DNS → Records
   - Should see: `gfc` → CNAME → `<tunnel-id>.cfargotunnel.com`

4. Wait for DNS propagation (up to 48 hours, usually < 1 hour)

5. Test DNS from external tool:
   - https://dnschecker.org
   - Enter: `gfc.lovanow.com`

---

### Problem: Login fails from external network

**Symptoms:**
- Login works locally but not remotely
- Authentication errors

**Solutions:**
1. Verify HTTPS is working (no certificate warnings)

2. Check CORS settings in application

3. Verify cookies are being set:
   - Browser DevTools → Application → Cookies
   - Should see authentication cookies for `gfc.lovanow.com`

4. Check if Cloudflare is blocking requests:
   - Dashboard → Security → Events
   - Look for blocked requests

5. Verify application base URL is set correctly:
   ```json
   // appsettings.json
   "BaseUrl": "https://gfc.lovanow.com"
   ```

---

## 🔍 Diagnostic Commands

### Check tunnel connectivity
```powershell
# Detailed tunnel info
cloudflared tunnel info gfc-webapp

# Test ingress rules
cloudflared tunnel ingress validate

# List active connections
cloudflared tunnel list
```

### Network diagnostics
```powershell
# Check DNS resolution
Resolve-DnsName gfc.lovanow.com

# Test HTTPS endpoint
Invoke-WebRequest https://gfc.lovanow.com -UseBasicParsing

# Check local IIS
Invoke-WebRequest http://localhost:8080 -UseBasicParsing

# List listening ports
netstat -an | Select-String "LISTENING" | Select-String "8080"
```

### Service diagnostics
```powershell
# Service status
Get-Service cloudflared | Format-List *

# Service logs (last 50 entries)
Get-EventLog -LogName Application -Source cloudflared -Newest 50

# IIS status
iisreset /status
Get-Service W3SVC
```

---

## 📊 Performance Monitoring

### Check tunnel latency
```powershell
# Measure response time
Measure-Command { Invoke-WebRequest https://gfc.lovanow.com -UseBasicParsing }
```

### Monitor active connections
```powershell
# View tunnel connections
cloudflared tunnel info gfc-webapp

# Should show 4 active connections for optimal performance
```

### Cloudflare Analytics
- Dashboard → Analytics & Logs → Traffic
- Monitor:
  - Requests per second
  - Bandwidth usage
  - Response time
  - Error rates

---

## 🔒 Security Checks

### Verify no public ports exposed
```powershell
# Check local bindings
Get-NetTCPConnection -State Listen | Where-Object { $_.LocalPort -eq 8080 }

# Should only show 127.0.0.1 (localhost), NOT 0.0.0.0
```

### Test external port scan
- Visit: https://www.yougetsignal.com/tools/open-ports/
- Enter your public IP
- Test ports: 80, 443, 8080
- All should be: **Closed** ✓

### Verify HTTPS certificate
```powershell
# PowerShell certificate check
$url = "https://gfc.lovanow.com"
$request = [System.Net.HttpWebRequest]::Create($url)
$response = $request.GetResponse()
$cert = $request.ServicePoint.Certificate
$cert2 = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 $cert
$cert2 | Format-List Subject, Issuer, NotBefore, NotAfter
```

---

## 📁 Important File Locations

### Cloudflared Files
```
C:\Program Files\cloudflared\cloudflared.exe
```

### Configuration Files
```
%USERPROFILE%\.cloudflared\
├── cert.pem                    # Cloudflare authentication certificate
├── config.yml                  # Tunnel configuration
└── <tunnel-id>.json           # Tunnel credentials
```

### Application Files
```
c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\
├── infrastructure\
│   ├── CLOUDFLARE_TUNNEL_SETUP.md
│   └── scripts\
│       ├── Install-CloudflareTunnel.ps1
│       └── Verify-CloudflareTunnel.ps1
└── PublishGFCWebApp\
    └── appsettings.json        # Update BaseUrl here
```

---

## 🔄 Maintenance Tasks

### Weekly
- [ ] Check service status: `Get-Service cloudflared`
- [ ] Review tunnel logs for errors
- [ ] Verify HTTPS certificate is valid

### Monthly
- [ ] Update cloudflared to latest version
- [ ] Review Cloudflare analytics for anomalies
- [ ] Test backup/restore of tunnel config

### Update Cloudflared
```powershell
# Stop service
Stop-Service cloudflared

# Download latest version
Invoke-WebRequest -Uri "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe" -OutFile "C:\Program Files\cloudflared\cloudflared.exe"

# Start service
Start-Service cloudflared

# Verify version
cloudflared --version
```

---

## 🆘 Emergency Procedures

### Rollback to Direct Access (Disable Tunnel)
```powershell
# Stop tunnel service
Stop-Service cloudflared
Set-Service -Name cloudflared -StartupType Disabled

# Users can access via: http://<server-ip>:8080
# (Only works on local network)
```

### Re-enable Tunnel
```powershell
# Enable and start service
Set-Service -Name cloudflared -StartupType Automatic
Start-Service cloudflared

# Verify status
Get-Service cloudflared
```

### Complete Reinstall
```powershell
# Uninstall service
cloudflared service uninstall

# Backup config
Copy-Item "$env:USERPROFILE\.cloudflared" -Destination "C:\Backups\cloudflared-backup" -Recurse

# Delete tunnel
cloudflared tunnel delete gfc-webapp

# Re-run installation script
.\Install-CloudflareTunnel.ps1
```

---

## 📞 Support Resources

### Cloudflare Documentation
- Tunnel Docs: https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/
- Troubleshooting: https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/troubleshooting/

### Cloudflare Community
- Forum: https://community.cloudflare.com/
- Discord: https://discord.cloudflare.com/

### GFC Project Documentation
- Setup Guide: `infrastructure\CLOUDFLARE_TUNNEL_SETUP.md`
- Installation Script: `infrastructure\scripts\Install-CloudflareTunnel.ps1`
- Verification Script: `infrastructure\scripts\Verify-CloudflareTunnel.ps1`

---

## ✅ Health Check Checklist

Run this checklist weekly to ensure tunnel is healthy:

- [ ] Service is running: `Get-Service cloudflared`
- [ ] HTTPS loads without warnings: `https://gfc.lovanow.com`
- [ ] Certificate is valid (check expiry date)
- [ ] 4 tunnel connections active: `cloudflared tunnel info gfc-webapp`
- [ ] No errors in service logs
- [ ] Login works from external network
- [ ] Service Worker registers successfully
- [ ] WebSockets are working (not long polling)
- [ ] No 502 errors in Cloudflare dashboard
- [ ] Response time < 500ms

---

**Last Updated**: 2026-01-04  
**Version**: 1.0  
**Maintainer**: GFC Development Team
