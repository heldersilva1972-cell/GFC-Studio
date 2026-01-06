# HTTPS Fix - Production Deployment Checklist

**Issue**: Fix "Not secure" on https://gfc.lovanow.com  
**Status**: ✅ Code Complete - Ready for Testing  
**Date**: 2026-01-05

---

## Pre-Deployment Checklist

- [x] ✅ Forwarded headers configured in `Program.cs`
- [x] ✅ Secure cookies enforced
- [x] ✅ HTTPS redirection enabled
- [x] ✅ HSTS enabled for production
- [x] ✅ No hardcoded HTTP URLs in codebase
- [x] ✅ PWA manifest uses relative URLs
- [x] ✅ Service worker uses relative URLs
- [x] ✅ Documentation created
- [x] ✅ Verification script created

---

## Deployment Steps

### 1. Verify Cloudflare Configuration

- [ ] **DNS Record**: `gfc` is CNAME to `<tunnel-id>.cfargotunnel.com` with **Proxied** (orange cloud)
- [ ] **SSL/TLS Mode**: Set to **Full** or **Full (strict)**
- [ ] **Universal SSL**: **Active** for `*.lovanow.com`
- [ ] **Always Use HTTPS**: **On** (recommended)
- [ ] **Automatic HTTPS Rewrites**: **On** (recommended)

**How to check**:
1. Log in to Cloudflare dashboard
2. Select `lovanow.com` domain
3. Go to SSL/TLS → Overview → Verify mode is "Full"
4. Go to SSL/TLS → Edge Certificates → Verify Universal SSL is "Active"
5. Go to DNS → Verify `gfc` record is proxied

### 2. Verify Cloudflare Tunnel

- [ ] **Tunnel running**: `cloudflared tunnel info gfc-webapp` shows active
- [ ] **Tunnel config**: Points to `http://localhost:8080`
- [ ] **Windows service**: `Get-Service cloudflared` shows "Running"
- [ ] **Auto-start**: Service startup type is "Automatic"

**How to check**:
```powershell
# Check tunnel status
cloudflared tunnel info gfc-webapp

# Check Windows service
Get-Service cloudflared

# Verify auto-start
Get-Service cloudflared | Select-Object StartType
```

### 3. Verify IIS Configuration

- [ ] **IIS running**: `Get-Service W3SVC` shows "Running"
- [ ] **Site running**: GFC site is started
- [ ] **Port binding**: Site bound to `http://localhost:8080`
- [ ] **App pool running**: Application pool is started

**How to check**:
```powershell
# Check IIS service
Get-Service W3SVC

# Test local endpoint
Invoke-WebRequest http://localhost:8080 -UseBasicParsing
```

### 4. Deploy Application

- [ ] **Build**: `dotnet build --configuration Release`
- [ ] **Publish**: `dotnet publish --configuration Release`
- [ ] **Copy files**: Deploy to IIS wwwroot
- [ ] **Restart app pool**: Recycle application pool
- [ ] **Verify startup**: Check application logs for errors

**How to deploy**:
```powershell
# Build and publish
cd "apps/webapp/GFC.BlazorServer"
dotnet publish --configuration Release --output "C:\inetpub\wwwroot\GFC"

# Restart IIS app pool (replace 'GFC' with your app pool name)
Restart-WebAppPool -Name "GFC"
```

---

## Post-Deployment Testing

### Automated Tests

- [ ] **Run verification script**:
  ```powershell
  cd "infrastructure/scripts"
  .\Verify-HttpsConfiguration.ps1
  ```
- [ ] **Expected**: All tests pass (6/6)

### Manual Browser Tests

#### Test 1: Security Lock Icon
- [ ] Open Chrome/Edge
- [ ] Navigate to `https://gfc.lovanow.com`
- [ ] **Expected**: 🔒 Lock icon in address bar
- [ ] **Expected**: "Secure" or "Connection is secure"
- [ ] **Not Expected**: ⚠️ "Not secure" warning

#### Test 2: DevTools Console
- [ ] Press `F12` → Console tab
- [ ] Refresh page (`Ctrl+R`)
- [ ] **Expected**: No mixed content warnings
- [ ] **Not Expected**: Warnings about "Mixed Content" or "insecure resource"

#### Test 3: DevTools Network
- [ ] Press `F12` → Network tab
- [ ] Clear network log
- [ ] Refresh page
- [ ] Filter by `http://` (not `https://`)
- [ ] **Expected**: No requests to `http://gfc.lovanow.com`
- [ ] **Expected**: All requests use `https://`

#### Test 4: DevTools Security
- [ ] Press `F12` → Security tab
- [ ] **Expected**: "This page is secure (valid HTTPS)"
- [ ] **Expected**: Certificate issued by Cloudflare/Let's Encrypt
- [ ] **Expected**: No mixed content warnings
- [ ] **Not Expected**: "This page is not secure"

#### Test 5: WebSocket Connection
- [ ] Press `F12` → Network tab → WS filter
- [ ] Refresh page
- [ ] **Expected**: WebSocket to `wss://gfc.lovanow.com/_blazor`
- [ ] **Expected**: Status "101 Switching Protocols"
- [ ] **Not Expected**: WebSocket to `ws://...` (insecure)

#### Test 6: Cookies
- [ ] Press `F12` → Application tab → Cookies
- [ ] Find authentication cookies
- [ ] **Expected**: `Secure` flag is ✓ checked
- [ ] **Expected**: `SameSite` is "Lax"

#### Test 7: PWA Installation
- [ ] Look for install icon in address bar
- [ ] Click install (if available)
- [ ] **Expected**: App installs successfully
- [ ] **Expected**: Installed app works normally

### Functional Tests

- [ ] **Login**: Log in with admin credentials
- [ ] **Navigation**: Navigate to different pages
- [ ] **Key Cards**: View key cards page
- [ ] **Members**: View members page
- [ ] **Logout**: Log out successfully
- [ ] **Re-login**: Log in again
- [ ] **Session**: Session persists correctly

### Mobile Tests (Optional but Recommended)

- [ ] **iPhone Safari**: Open `https://gfc.lovanow.com`
- [ ] **Android Chrome**: Open `https://gfc.lovanow.com`
- [ ] **Add to Home Screen**: Install PWA
- [ ] **Launch from home**: Open installed app
- [ ] **Login**: Log in from mobile
- [ ] **Navigation**: Navigate pages

---

## Troubleshooting

### Issue: "Not secure" warning persists

**Steps**:
1. Hard refresh: `Ctrl+Shift+R`
2. Clear cookies: DevTools → Application → Clear storage
3. Check Cloudflare SSL mode: Should be "Full"
4. Check DNS: `nslookup gfc.lovanow.com` → Should show Cloudflare IP
5. Check tunnel: `cloudflared tunnel info gfc-webapp`
6. Check IIS: `Invoke-WebRequest http://localhost:8080`
7. Review logs: Check application logs for errors

### Issue: Login fails

**Steps**:
1. Check cookies: DevTools → Application → Cookies → Verify `Secure` flag
2. Verify HTTPS: Ensure using `https://` not `http://`
3. Clear cookies: DevTools → Application → Clear storage
4. Check session: Verify session timeout settings
5. Check logs: Review application logs

### Issue: WebSocket fails

**Steps**:
1. Check WebSocket: DevTools → Network → WS → Should be `wss://`
2. Verify middleware order: `UseForwardedHeaders()` before `MapBlazorHub()`
3. Check Cloudflare: WebSockets should be enabled
4. Check tunnel: Verify tunnel supports WebSockets
5. Review logs: Check for SignalR errors

### Issue: Mixed content warnings

**Steps**:
1. Check console: Note which resources are HTTP
2. Search codebase: `grep -r "http://" --include="*.cs" --include="*.razor"`
3. Fix URLs: Change to relative URLs or HTTPS
4. Rebuild: `dotnet build`
5. Redeploy: Copy files to IIS
6. Test: Verify warnings are gone

---

## Rollback Plan

If issues occur:

1. **Stop IIS**: `Stop-WebAppPool -Name "GFC"`
2. **Restore previous version**: Copy backup files to wwwroot
3. **Start IIS**: `Start-WebAppPool -Name "GFC"`
4. **Verify**: Test `http://localhost:8080`
5. **Investigate**: Review logs and error messages
6. **Fix**: Address issues and redeploy

---

## Success Criteria

- [x] ✅ All pre-deployment checks passed
- [ ] ⏳ Application deployed successfully
- [ ] ⏳ Automated tests pass (6/6)
- [ ] ⏳ Manual browser tests pass (7/7)
- [ ] ⏳ Functional tests pass
- [ ] ⏳ Lock icon appears on `https://gfc.lovanow.com`
- [ ] ⏳ No mixed content warnings
- [ ] ⏳ All functionality works normally

---

## Sign-Off

**Deployed By**: ___________________  
**Date**: ___________________  
**Time**: ___________________  

**Verified By**: ___________________  
**Date**: ___________________  

**Issues Found**: ___________________  
**Resolution**: ___________________  

---

## Documentation

- **Comprehensive Guide**: `docs/hosting/CLOUDFLARE_TUNNEL_HTTPS_FIX.md`
- **Quick Summary**: `.agent/reports/HTTPS_FIX_SUMMARY.md`
- **Verification Script**: `infrastructure/scripts/Verify-HttpsConfiguration.ps1`
- **This Checklist**: `.agent/reports/HTTPS_DEPLOYMENT_CHECKLIST.md`

---

**Status**: Ready for Deployment  
**Estimated Time**: 30-60 minutes  
**Risk Level**: Low (all code changes already implemented)
