# Cloudflare Tunnel Setup Guide
## Enable Trusted HTTPS for gfc.lovanow.com

**Status**: Implementation Guide  
**Priority**: High (Phase 3 - Secure External Access)  
**Labels**: security, infrastructure, https, cloudflare, blocking

---

## 📋 Overview

This guide provides step-by-step instructions to enable browser-trusted HTTPS for `gfc.lovanow.com` using Cloudflare Tunnel (formerly Argo Tunnel). This solution provides:

- ✅ **Trusted HTTPS** - No browser warnings
- ✅ **No Public Exposure** - Server remains private
- ✅ **No Inbound Ports** - Outbound-only connection
- ✅ **VPN-Safe** - Compatible with access controls
- ✅ **Auto-Managed TLS** - Cloudflare handles certificates

---

## ❗ Current Issues Being Resolved

1. **Self-Signed Certificate Warnings** - Browsers show "Not Secure"
2. **Service Worker Failures** - PWA registration blocked
3. **WebSocket Downgrade** - Blazor forced to long polling
4. **Login Failures** - Authentication broken outside Visual Studio
5. **Poor User Experience** - Non-technical users cannot onboard

---

## 🎯 Implementation Phases

### **Phase 1: Cloudflare Configuration**
### **Phase 2: Server Setup (Windows/IIS)**
### **Phase 3: Security Verification**
### **Phase 4: Production Deployment**

---

## 📝 Prerequisites

- [ ] Domain `lovanow.com` ownership verified
- [ ] Cloudflare account with access to DNS management
- [ ] Windows Server with IIS installed and running
- [ ] Administrator access to the server
- [ ] Current application running on `http://localhost:8080` (or note actual port)

---

## Phase 1: Cloudflare Configuration

### Step 1.1: Add Domain to Cloudflare

1. **Log in to Cloudflare Dashboard**
   - Navigate to: https://dash.cloudflare.com/

2. **Add Site**
   - Click "Add a Site"
   - Enter: `lovanow.com`
   - Select plan (Free tier is sufficient)
   - Click "Add Site"

3. **Update Nameservers**
   - Cloudflare will provide 2 nameservers (e.g., `ns1.cloudflare.com`, `ns2.cloudflare.com`)
   - Log in to your domain registrar (where you purchased `lovanow.com`)
   - Replace existing nameservers with Cloudflare's nameservers
   - **Wait 24-48 hours for DNS propagation** (usually faster)

4. **Verify Active Status**
   - Return to Cloudflare dashboard
   - Confirm site status shows "Active"

### Step 1.2: Create DNS Record

1. **Navigate to DNS Settings**
   - In Cloudflare dashboard, select `lovanow.com`
   - Click "DNS" in the left sidebar

2. **Add A Record (Placeholder)**
   - Click "Add record"
   - Type: `A`
   - Name: `gfc`
   - IPv4 address: `192.0.2.1` (placeholder - will be overridden by tunnel)
   - Proxy status: **Proxied** (orange cloud icon)
   - TTL: Auto
   - Click "Save"

   > **Note**: The actual IP doesn't matter - Cloudflare Tunnel will route traffic through the tunnel, not to this IP.

### Step 1.3: Enable SSL/TLS

1. **Navigate to SSL/TLS Settings**
   - Click "SSL/TLS" in the left sidebar
   - Select "Overview"

2. **Set Encryption Mode**
   - Choose: **"Full"** or **"Full (strict)"**
   - Recommended: **"Full"** (works with self-signed certs on origin)

3. **Enable Universal SSL**
   - Navigate to "SSL/TLS" → "Edge Certificates"
   - Confirm "Universal SSL" is **Enabled**
   - Wait 5-10 minutes for certificate provisioning

4. **Verify Certificate**
   - Open browser to: `https://gfc.lovanow.com`
   - You'll see an error (tunnel not configured yet), but check certificate:
     - Click padlock icon → Certificate
     - Issuer should be "Cloudflare" or "Let's Encrypt"
     - Valid for `gfc.lovanow.com` and `*.lovanow.com`

---

## Phase 2: Server Setup (Windows/IIS)

### Step 2.1: Install Cloudflared

1. **Download Cloudflared**
   - Open PowerShell as Administrator
   - Run:
     ```powershell
     # Create directory for cloudflared
     New-Item -ItemType Directory -Force -Path "C:\Program Files\cloudflared"
     
     # Download latest Windows binary
     Invoke-WebRequest -Uri "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe" -OutFile "C:\Program Files\cloudflared\cloudflared.exe"
     
     # Add to PATH
     $env:Path += ";C:\Program Files\cloudflared"
     [Environment]::SetEnvironmentVariable("Path", $env:Path, [EnvironmentVariableTarget]::Machine)
     ```

2. **Verify Installation**
   ```powershell
   cloudflared --version
   ```
   - Should output version number (e.g., `cloudflared version 2024.x.x`)

### Step 2.2: Authenticate Cloudflared

1. **Login to Cloudflare**
   ```powershell
   cloudflared tunnel login
   ```
   - A browser window will open
   - Select the domain: `lovanow.com`
   - Click "Authorize"
   - Certificate saved to: `C:\Users\<YourUser>\.cloudflared\cert.pem`

2. **Verify Authentication**
   ```powershell
   Test-Path "$env:USERPROFILE\.cloudflared\cert.pem"
   ```
   - Should return `True`

### Step 2.3: Create Named Tunnel

1. **Create Tunnel**
   ```powershell
   cloudflared tunnel create gfc-webapp
   ```
   - Output will show:
     ```
     Tunnel credentials written to: C:\Users\<YourUser>\.cloudflared\<TUNNEL-ID>.json
     Created tunnel gfc-webapp with id <TUNNEL-ID>
     ```
   - **Save the TUNNEL-ID** - you'll need it!

2. **Verify Tunnel Creation**
   ```powershell
   cloudflared tunnel list
   ```
   - Should show: `gfc-webapp` with status `INACTIVE`

### Step 2.4: Configure Tunnel Routing

1. **Route DNS to Tunnel**
   ```powershell
   cloudflared tunnel route dns gfc-webapp gfc.lovanow.com
   ```
   - Output: `Successfully created route for gfc.lovanow.com`

2. **Verify DNS Route**
   - In Cloudflare dashboard → DNS
   - The `gfc` A record should now show a CNAME to `<TUNNEL-ID>.cfargotunnel.com`

### Step 2.5: Create Tunnel Configuration File

1. **Determine IIS Port**
   - Open IIS Manager
   - Select your GFC site
   - Click "Bindings" in the right panel
   - Note the HTTP port (commonly `8080`, `80`, or `5000`)

2. **Create Config File**
   ```powershell
   # Create config directory
   New-Item -ItemType Directory -Force -Path "$env:USERPROFILE\.cloudflared"
   
   # Create config file
   @"
tunnel: <TUNNEL-ID>
credentials-file: C:\Users\<YourUser>\.cloudflared\<TUNNEL-ID>.json

ingress:
  - hostname: gfc.lovanow.com
    service: http://localhost:8080
    originRequest:
      noTLSVerify: true
  - service: http_status:404
"@ | Out-File -FilePath "$env:USERPROFILE\.cloudflared\config.yml" -Encoding UTF8
   ```

   **Replace**:
   - `<TUNNEL-ID>` with your actual tunnel ID
   - `<YourUser>` with your Windows username
   - `8080` with your actual IIS port

3. **Verify Config File**
   ```powershell
   Get-Content "$env:USERPROFILE\.cloudflared\config.yml"
   ```

### Step 2.6: Test Tunnel (Manual)

1. **Start Tunnel Manually**
   ```powershell
   cloudflared tunnel run gfc-webapp
   ```
   - Should output:
     ```
     INF Connection registered connIndex=0
     INF Connection registered connIndex=1
     INF Connection registered connIndex=2
     INF Connection registered connIndex=3
     ```

2. **Test in Browser**
   - Open: `https://gfc.lovanow.com`
   - Should load your application **without certificate warnings**
   - Press `Ctrl+C` to stop the tunnel

### Step 2.7: Install as Windows Service

1. **Install Service**
   ```powershell
   cloudflared service install
   ```
   - Output: `Service installed successfully`

2. **Start Service**
   ```powershell
   Start-Service cloudflared
   ```

3. **Verify Service Status**
   ```powershell
   Get-Service cloudflared
   ```
   - Status should be: `Running`

4. **Set Service to Auto-Start**
   ```powershell
   Set-Service -Name cloudflared -StartupType Automatic
   ```

5. **Verify Service Logs**
   ```powershell
   cloudflared tunnel info gfc-webapp
   ```

---

## Phase 3: Security Verification

### Step 3.1: HTTPS Certificate Validation

1. **Browser Test**
   - Navigate to: `https://gfc.lovanow.com`
   - Click padlock icon → "Connection is secure"
   - Verify:
     - ✅ No warnings
     - ✅ Issued by: Cloudflare/Let's Encrypt
     - ✅ Valid dates
     - ✅ Matches domain

2. **SSL Labs Test** (Optional)
   - Visit: https://www.ssllabs.com/ssltest/
   - Enter: `gfc.lovanow.com`
   - Wait for scan
   - Target grade: **A** or **A+**

### Step 3.2: Service Worker / PWA Verification

1. **Open Browser DevTools**
   - Press `F12`
   - Navigate to "Application" tab (Chrome) or "Storage" tab (Firefox)

2. **Check Service Worker**
   - Click "Service Workers" in left panel
   - Verify:
     - ✅ Service Worker registered
     - ✅ Status: "activated and running"
     - ✅ No errors in console

3. **Test PWA Installation**
   - Look for "Install" icon in address bar
   - Click to install
   - Verify app installs and launches

### Step 3.3: Blazor WebSocket Verification

1. **Open Browser DevTools**
   - Press `F12` → "Network" tab
   - Filter: "WS" (WebSocket)

2. **Refresh Page**
   - Verify WebSocket connection established
   - Look for: `_blazor?id=...` with status `101 Switching Protocols`
   - ✅ No fallback to long polling

3. **Check Console**
   - No errors like "WebSocket failed, falling back to long polling"

### Step 3.4: Authentication / Login Test

1. **Test Login Flow**
   - Navigate to login page
   - Enter credentials
   - Verify:
     - ✅ Login succeeds
     - ✅ Session persists
     - ✅ No CORS errors

2. **Test from External Device**
   - Use mobile phone (not on same network)
   - Navigate to `https://gfc.lovanow.com`
   - Verify login works

### Step 3.5: Firewall / Port Verification

1. **Check No Inbound Ports Open**
   ```powershell
   # List listening ports
   netstat -an | Select-String "LISTENING"
   ```
   - Verify port `8080` (or your IIS port) is **only** listening on `127.0.0.1` (localhost)
   - Should see: `127.0.0.1:8080` NOT `0.0.0.0:8080`

2. **External Port Scan** (Optional)
   - Use: https://www.yougetsignal.com/tools/open-ports/
   - Enter your public IP
   - Scan ports: `80`, `443`, `8080`
   - All should be: **Closed**

### Step 3.6: Tunnel Resilience Test

1. **Restart Tunnel Service**
   ```powershell
   Restart-Service cloudflared
   ```

2. **Verify Auto-Recovery**
   - Wait 10-15 seconds
   - Refresh browser: `https://gfc.lovanow.com`
   - Should reconnect automatically

3. **Check Tunnel Status**
   ```powershell
   cloudflared tunnel info gfc-webapp
   ```
   - Verify 4 connections active

---

## Phase 4: Production Deployment

### Step 4.1: Update Application Configuration

1. **Update `appsettings.json`**
   - Locate: `PublishGFCWebApp\appsettings.json`
   - Update base URL:
     ```json
     {
       "BaseUrl": "https://gfc.lovanow.com",
       "Kestrel": {
         "Endpoints": {
           "Http": {
             "Url": "http://localhost:8080"
           }
         }
       }
     }
     ```

2. **Update PWA Manifest** (if applicable)
   - Update `start_url` and `scope` to use `https://gfc.lovanow.com`

### Step 4.2: Configure Cloudflare Optimizations

1. **Enable HTTP/3**
   - Cloudflare dashboard → "Network"
   - Enable: "HTTP/3 (with QUIC)"

2. **Enable Auto Minify**
   - "Speed" → "Optimization"
   - Enable: JavaScript, CSS, HTML

3. **Enable Brotli Compression**
   - "Speed" → "Optimization"
   - Enable: "Brotli"

4. **Configure Caching**
   - "Caching" → "Configuration"
   - Browser Cache TTL: `4 hours` (or as needed)

### Step 4.3: Monitoring & Logging

1. **Enable Cloudflare Analytics**
   - Dashboard → "Analytics & Logs"
   - Review traffic patterns

2. **Monitor Tunnel Logs**
   ```powershell
   # View service logs
   Get-EventLog -LogName Application -Source cloudflared -Newest 50
   ```

3. **Set Up Alerts** (Optional)
   - Cloudflare dashboard → "Notifications"
   - Create alert for: "Tunnel Down"

### Step 4.4: Backup Configuration

1. **Backup Tunnel Credentials**
   ```powershell
   # Copy to secure location
   Copy-Item "$env:USERPROFILE\.cloudflared\*" -Destination "C:\Backups\cloudflared\" -Recurse
   ```

2. **Document Configuration**
   - Save tunnel ID, domain, and port mappings
   - Store in secure documentation

---

## 🔒 Security Best Practices

### Additional Hardening (Optional)

1. **Enable Cloudflare Access** (VPN-like control)
   - Dashboard → "Zero Trust" → "Access"
   - Create application for `gfc.lovanow.com`
   - Add authentication rules (email, Google, etc.)

2. **IP Allowlist** (if needed)
   - "Security" → "WAF" → "Tools"
   - Create firewall rule to allow only specific IPs

3. **Rate Limiting**
   - "Security" → "WAF" → "Rate limiting rules"
   - Protect login endpoints from brute force

4. **DDoS Protection**
   - Automatically enabled with Cloudflare proxy
   - Review: "Security" → "DDoS"

---

## 🧪 Testing Checklist

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

---

## 🐛 Troubleshooting

### Issue: "Tunnel not connecting"

**Symptoms**: `cloudflared tunnel run` shows no connections

**Solutions**:
1. Verify credentials file exists:
   ```powershell
   Test-Path "$env:USERPROFILE\.cloudflared\<TUNNEL-ID>.json"
   ```
2. Check config file syntax (YAML is indentation-sensitive)
3. Verify tunnel ID matches in config and credentials file
4. Check firewall allows outbound HTTPS (port 443)

### Issue: "502 Bad Gateway"

**Symptoms**: Browser shows Cloudflare 502 error

**Solutions**:
1. Verify IIS is running:
   ```powershell
   Get-Service W3SVC
   ```
2. Test local endpoint:
   ```powershell
   Invoke-WebRequest http://localhost:8080
   ```
3. Check `service:` URL in config.yml matches IIS port
4. Verify `noTLSVerify: true` is set (if using self-signed cert locally)

### Issue: "Service Worker still failing"

**Symptoms**: PWA won't install, SW registration errors

**Solutions**:
1. Hard refresh browser: `Ctrl+Shift+R`
2. Clear browser cache and service workers
3. Check browser console for specific errors
4. Verify `https://` (not `http://`) in all PWA manifest URLs

### Issue: "WebSocket still using long polling"

**Symptoms**: Network tab shows no WS connections

**Solutions**:
1. Verify Cloudflare WebSocket support is enabled:
   - Dashboard → "Network" → "WebSockets": **On**
2. Check Blazor hub configuration in application
3. Clear browser cache

### Issue: "Tunnel service won't start"

**Symptoms**: `Start-Service cloudflared` fails

**Solutions**:
1. Check service logs:
   ```powershell
   Get-EventLog -LogName Application -Source cloudflared -Newest 10
   ```
2. Verify config file path in service installation
3. Reinstall service:
   ```powershell
   cloudflared service uninstall
   cloudflared service install
   ```

---

## 📚 Additional Resources

- **Cloudflare Tunnel Docs**: https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/
- **Cloudflared GitHub**: https://github.com/cloudflare/cloudflared
- **Cloudflare Community**: https://community.cloudflare.com/
- **Windows Service Management**: https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create

---

## ✅ Acceptance Criteria

- [x] Documentation created
- [ ] Domain added to Cloudflare
- [ ] DNS configured with proxied record
- [ ] Universal SSL enabled and active
- [ ] `cloudflared` installed on server
- [ ] Tunnel authenticated and created
- [ ] Tunnel configured with correct port
- [ ] Tunnel running as Windows service
- [ ] `https://gfc.lovanow.com` shows valid HTTPS
- [ ] No browser security warnings
- [ ] Service Worker registers successfully
- [ ] PWA installation works
- [ ] Blazor uses WebSockets
- [ ] Login works from external networks
- [ ] No inbound firewall ports required
- [ ] Tunnel auto-starts on server reboot

---

## 📝 Notes

- **Estimated Time**: 2-3 hours (including DNS propagation wait)
- **Downtime**: None (tunnel runs alongside existing setup)
- **Reversibility**: High (can disable tunnel anytime)
- **Cost**: $0 (Cloudflare Free tier sufficient)

---

**Last Updated**: 2026-01-04  
**Author**: GFC Development Team  
**Status**: Ready for Implementation
