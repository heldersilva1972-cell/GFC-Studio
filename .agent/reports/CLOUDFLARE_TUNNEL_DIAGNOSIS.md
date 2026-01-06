# Cloudflare Tunnel Diagnosis and Fix

**Date**: 2026-01-05 21:35 EST  
**Issue**: SSL/TLS connection fails to https://gfc.lovanow.com  
**Root Cause**: Cloudflare Tunnel not running or not configured

---

## Current Status

### ✅ What's Working
- DNS resolves correctly to Cloudflare IPs:
  - `104.21.49.122`
  - `172.67.145.55`
  - `2606:4700:3030::6815:317a` (IPv6)
  - `2606:4700:3035::ac43:9137` (IPv6)

### ❌ What's NOT Working
- **SSL/TLS connection fails**: "Could not establish trust relationship for the SSL/TLS secure channel"
- **Cloudflare Tunnel not found**: `cloudflared` command not in PATH
- **Service worker not accessible**: Cannot reach https://gfc.lovanow.com

---

## Root Cause

The error "Could not establish trust relationship for the SSL/TLS secure channel" combined with Cloudflare DNS resolution means:

1. **DNS is correct** - Points to Cloudflare
2. **Cloudflare Tunnel is NOT running** - No backend to serve requests
3. **Cloudflare returns an error** - Likely 502 Bad Gateway or connection timeout

---

## Diagnosis Steps

### Step 1: Check if Cloudflare Tunnel is Installed

```powershell
# Check if cloudflared.exe exists
Test-Path "C:\Program Files\cloudflared\cloudflared.exe"

# If not found, check alternate locations
Get-ChildItem -Path "C:\" -Filter "cloudflared.exe" -Recurse -ErrorAction SilentlyContinue | Select-Object FullName
```

**Expected**: Should find `cloudflared.exe` somewhere
**If not found**: Cloudflare Tunnel is not installed

### Step 2: Check if Cloudflare Tunnel Service is Running

```powershell
# Check Windows service
Get-Service -Name "cloudflared" -ErrorAction SilentlyContinue

# Check all services with "cloud" in name
Get-Service | Where-Object {$_.Name -like "*cloud*"}
```

**Expected**: Service named `cloudflared` with status "Running"
**If not found**: Service not installed

### Step 3: Check IIS Status

```powershell
# Check if IIS is running
Get-Service W3SVC

# Check if site is running on port 8080
netstat -ano | findstr :8080

# Test local endpoint
Invoke-WebRequest http://localhost:8080 -UseBasicParsing
```

**Expected**: IIS running, port 8080 listening, local site accessible
**If fails**: IIS or application not running

### Step 4: Check Cloudflare Dashboard

1. Log in to https://dash.cloudflare.com
2. Select `lovanow.com` domain
3. Go to **Zero Trust** → **Networks** → **Tunnels**
4. Look for tunnel named `gfc-webapp`
5. Check status:
   - **HEALTHY** (green) = Tunnel connected
   - **DOWN** (red) = Tunnel not connected
   - **Not found** = Tunnel not created

---

## Solution Paths

### Option A: Cloudflare Tunnel is NOT Installed

**You need to install it first:**

1. **Run the installation script**:
   ```powershell
   cd "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\infrastructure\scripts"
   .\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com
   ```

2. **Follow the interactive prompts**:
   - Script will download `cloudflared.exe`
   - Browser will open for Cloudflare authentication
   - Tunnel will be created and configured
   - Windows service will be installed

3. **Verify installation**:
   ```powershell
   Get-Service cloudflared
   cloudflared tunnel info gfc-webapp
   ```

### Option B: Cloudflare Tunnel is Installed but NOT Running

**Start the service:**

```powershell
# Find cloudflared.exe
$cloudflaredPath = "C:\Program Files\cloudflared\cloudflared.exe"

# If service exists but stopped
Start-Service cloudflared

# If service doesn't exist, install it
& $cloudflaredPath service install

# Start the service
Start-Service cloudflared

# Verify it's running
Get-Service cloudflared
```

### Option C: Cloudflare Tunnel is Running but NOT Configured

**Check and fix configuration:**

```powershell
# Check if config file exists
Test-Path "$env:USERPROFILE\.cloudflared\config.yml"

# View config
Get-Content "$env:USERPROFILE\.cloudflared\config.yml"
```

**Expected config.yml**:
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

**If config is wrong or missing**:
1. Stop service: `Stop-Service cloudflared`
2. Fix config file
3. Start service: `Start-Service cloudflared`

---

## Quick Fix Commands

### If Tunnel is NOT Installed:

```powershell
# Navigate to scripts folder
cd "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\infrastructure\scripts"

# Run installation script
.\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com
```

### If Tunnel is Installed but Stopped:

```powershell
# Start the service
Start-Service cloudflared

# Set to auto-start
Set-Service -Name cloudflared -StartupType Automatic

# Verify
Get-Service cloudflared
```

### If Tunnel Service Doesn't Exist:

```powershell
# Find cloudflared.exe
$cloudflaredPath = (Get-ChildItem -Path "C:\" -Filter "cloudflared.exe" -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1).FullName

# Install service
& $cloudflaredPath service install

# Start service
Start-Service cloudflared
```

---

## Verification After Fix

### Step 1: Verify Tunnel is Running

```powershell
# Check service
Get-Service cloudflared

# Should show:
# Status: Running
# StartType: Automatic
```

### Step 2: Verify Tunnel Connection

```powershell
# Add cloudflared to PATH if needed
$env:Path += ";C:\Program Files\cloudflared"

# Check tunnel info
cloudflared tunnel info gfc-webapp

# Should show tunnel details and status
```

### Step 3: Test HTTPS Connection

```powershell
# Wait 30 seconds for tunnel to stabilize
Start-Sleep -Seconds 30

# Test connection (ignore cert validation for now)
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
$response = Invoke-WebRequest -Uri "https://gfc.lovanow.com" -UseBasicParsing
$response.StatusCode

# Should return: 200
```

### Step 4: Test in Browser

1. Open Chrome/Edge
2. Navigate to: `https://gfc.lovanow.com`
3. **Expected**: Page loads (may show "Not secure" initially)
4. **Not Expected**: Connection timeout or 502 Bad Gateway

---

## Common Issues

### Issue: "cloudflared: command not found"

**Cause**: `cloudflared.exe` not in PATH

**Solution**:
```powershell
# Add to PATH temporarily
$env:Path += ";C:\Program Files\cloudflared"

# Or use full path
& "C:\Program Files\cloudflared\cloudflared.exe" tunnel info gfc-webapp
```

### Issue: "Service 'cloudflared' not found"

**Cause**: Windows service not installed

**Solution**:
```powershell
# Install service
cloudflared service install

# Start service
Start-Service cloudflared
```

### Issue: "502 Bad Gateway" in browser

**Cause**: Tunnel running but IIS not responding

**Solution**:
```powershell
# Check IIS
Get-Service W3SVC

# Test local endpoint
Invoke-WebRequest http://localhost:8080 -UseBasicParsing

# Restart IIS if needed
iisreset
```

### Issue: "Tunnel shows HEALTHY but site doesn't load"

**Cause**: DNS cache or browser cache

**Solution**:
```powershell
# Clear DNS cache
ipconfig /flushdns

# In browser: Hard refresh (Ctrl+Shift+R)
# Or: Clear browser cache
```

---

## Next Steps

1. **Determine which scenario applies**:
   - [ ] Tunnel not installed → Run `Install-CloudflareTunnel.ps1`
   - [ ] Tunnel installed but stopped → Run `Start-Service cloudflared`
   - [ ] Tunnel running but misconfigured → Fix `config.yml`

2. **After fixing, re-run verification**:
   ```powershell
   cd "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\infrastructure\scripts"
   .\Verify-HttpsConfiguration.ps1
   ```

3. **Expected results**:
   - ✅ DNS Resolution
   - ✅ HTTPS Connection
   - ✅ SSL Certificate
   - ✅ Tunnel Status
   - ✅ Service Worker
   - ✅ Manifest

---

## Documentation References

- **Installation Guide**: `infrastructure/CLOUDFLARE_TUNNEL_SETUP.md`
- **Quick Reference**: `infrastructure/CLOUDFLARE_TUNNEL_QUICK_REFERENCE.md`
- **HTTPS Fix Guide**: `docs/hosting/CLOUDFLARE_TUNNEL_HTTPS_FIX.md`

---

**Status**: Awaiting Tunnel Installation/Configuration  
**Priority**: High - Site is currently inaccessible via HTTPS
