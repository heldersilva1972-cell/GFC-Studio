# Deployment Checklist - Public Onboarding Gateway

## Pre-Deployment

### 1. Database Setup
- [ ] Run `Manual_OnboardingGateway_Schema.sql`
- [ ] Verify `VpnOnboardingTokens` table exists
- [ ] Verify indexes created successfully
- [ ] Verify SystemSettings columns added
- [ ] Set default values in SystemSettings

### 2. Backend Configuration
- [ ] Rate limiting added to Program.cs
- [ ] CORS policy updated with gateway URL
- [ ] OnboardingController.cs deployed
- [ ] HealthController.cs deployed
- [ ] VpnConfigurationService tested

### 3. Gateway Files
- [ ] index.html reviewed
- [ ] styles.css reviewed
- [ ] setup.js reviewed
- [ ] API URL configured in setup.js
- [ ] Logo SVG included
- [ ] Security headers configured

---

## Deployment Steps

### Option A: Cloudflare Pages

#### Step 1: Prepare Repository
```bash
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2"
git add apps/onboarding-gateway/
git commit -m "feat: add public onboarding gateway"
git push origin main
```

#### Step 2: Create Cloudflare Pages Project
- [ ] Log in to Cloudflare Dashboard
- [ ] Go to Pages → Create a project
- [ ] Connect to Git repository
- [ ] Select repository and branch
- [ ] Configure build settings:
  - Build command: (none)
  - Build output directory: `/`
  - Root directory: `apps/onboarding-gateway`

#### Step 3: Configure Custom Domain
- [ ] Go to Pages → Your Project → Custom domains
- [ ] Add custom domain: `setup.gfc.lovanow.com`
- [ ] Copy CNAME record details
- [ ] Update DNS (see DNS Configuration below)
- [ ] Wait for SSL provisioning (automatic)

#### Step 4: Verify Deployment
- [ ] Visit `https://setup.gfc.lovanow.com`
- [ ] Verify page loads correctly
- [ ] Check browser console for errors
- [ ] Verify security headers (use securityheaders.com)

---

### Option B: IIS Deployment

#### Step 1: Prepare Server
- [ ] Create directory: `C:\inetpub\gfc-onboarding`
- [ ] Copy files from `apps/onboarding-gateway/` to server
- [ ] Verify all files copied correctly

#### Step 2: Create IIS Site
- [ ] Open IIS Manager
- [ ] Right-click Sites → Add Website
- [ ] Site name: `GFC Onboarding Gateway`
- [ ] Physical path: `C:\inetpub\gfc-onboarding`
- [ ] Binding: Type = HTTPS, Port = 443
- [ ] Host name: `setup.gfc.lovanow.com`
- [ ] Select SSL certificate

#### Step 3: Configure Site
- [ ] Set Application Pool to `No Managed Code`
- [ ] Enable Static Content feature
- [ ] Verify web.config is present
- [ ] Test site in IIS

#### Step 4: Update Configuration
- [ ] Edit `js/setup.js`
- [ ] Update `CONFIG.apiBaseUrl` to production URL
- [ ] Save and verify

---

## DNS Configuration

### Add CNAME Record

**For Cloudflare Pages:**
```
Type: CNAME
Name: setup
Target: [your-project].pages.dev
Proxy: Yes (orange cloud)
TTL: Auto
```

**For IIS:**
```
Type: A
Name: setup
Target: [Your Server IP]
TTL: 3600
```

### Verify DNS
```bash
# Windows
nslookup setup.gfc.lovanow.com

# Expected output:
# Name: setup.gfc.lovanow.com
# Address: [IP or CNAME target]
```

---

## SSL Certificate

### Cloudflare Pages
- [ ] SSL automatically provisioned
- [ ] Verify certificate in browser
- [ ] Check expiry date

### IIS
- [ ] Obtain SSL certificate (Let's Encrypt, DigiCert, etc.)
- [ ] Install certificate in Windows Certificate Store
- [ ] Bind certificate to IIS site
- [ ] Verify HTTPS works
- [ ] Set up auto-renewal

---

## Backend Configuration

### Update SystemSettings

```sql
UPDATE SystemSettings
SET 
    EnableOnboarding = 1,
    SafeModeEnabled = 0,
    OnboardingGatewayUrl = 'https://setup.gfc.lovanow.com',
    OnboardingTokenExpiryHours = 48,
    OnboardingRateLimitPerMinute = 10,
    WireGuardServerPublicKey = '[YOUR_SERVER_PUBLIC_KEY]',
    WireGuardPort = 51820,
    WireGuardSubnet = '10.8.0.0/24',
    WireGuardAllowedIPs = '10.8.0.0/24, 192.168.1.0/24'
WHERE Id = 1;
```

### Restart Backend Application
- [ ] Stop GFC.BlazorServer
- [ ] Verify Program.cs changes deployed
- [ ] Start GFC.BlazorServer
- [ ] Check logs for errors
- [ ] Verify rate limiting active

---

## Testing

### Test 1: Gateway Accessibility
- [ ] Access `https://setup.gfc.lovanow.com` (no VPN)
- [ ] Verify page loads
- [ ] Check for HTTPS (lock icon)
- [ ] Verify no console errors

### Test 2: Invalid Token
- [ ] Visit `https://setup.gfc.lovanow.com?token=INVALID`
- [ ] Verify error message displayed
- [ ] Verify no backend errors

### Test 3: Valid Token Flow
```sql
-- Generate test token
DECLARE @Token NVARCHAR(256) = CONVERT(NVARCHAR(256), NEWID());
INSERT INTO VpnOnboardingTokens (UserId, Token, CreatedAtUtc, ExpiresAtUtc, IsUsed)
VALUES (1, @Token, GETUTCDATE(), DATEADD(HOUR, 48, GETUTCDATE()), 0);
SELECT 'https://setup.gfc.lovanow.com?token=' + @Token AS TestLink;
```

- [ ] Visit generated link
- [ ] Verify OS detected correctly
- [ ] Click "I have the app installed"
- [ ] Click "Download Secure Access Profile"
- [ ] Verify .conf file downloaded
- [ ] Open .conf file, verify contents
- [ ] Import into WireGuard
- [ ] Enable VPN
- [ ] Click "Test Connection"
- [ ] Verify success message

### Test 4: Rate Limiting
```bash
# Send 15 requests rapidly
for i in {1..15}; do
  curl -s -o /dev/null -w "%{http_code}\n" \
    "https://gfc.lovanow.com/api/onboarding/validate?token=test"
done

# Expected: First 10 return 404, remaining return 429
```

### Test 5: Security Headers
- [ ] Visit https://securityheaders.com
- [ ] Enter `https://setup.gfc.lovanow.com`
- [ ] Verify A or A+ rating
- [ ] Check all required headers present

### Test 6: Cross-Platform
- [ ] Test on Windows desktop
- [ ] Test on macOS desktop
- [ ] Test on iOS mobile
- [ ] Test on Android mobile
- [ ] Verify correct instructions per platform

---

## Monitoring Setup

### Application Insights (Optional)
```javascript
// Add to setup.js if using Application Insights
var appInsights = window.appInsights || function(config) {
    // ... Application Insights code
}({
    instrumentationKey: "YOUR_KEY"
});
```

### Backend Logging
- [ ] Verify logs capturing:
  - Token validation attempts
  - Config downloads
  - Failed validations
  - Rate limit violations

### Alerts
- [ ] Set up alert for high error rate
- [ ] Set up alert for rate limit violations
- [ ] Set up alert for SSL expiry

---

## Post-Deployment

### Documentation
- [ ] Update internal wiki with deployment info
- [ ] Document how to generate tokens
- [ ] Create user guide
- [ ] Train support staff

### Communication
- [ ] Notify administrators
- [ ] Send test invite to pilot users
- [ ] Gather feedback
- [ ] Iterate as needed

### Maintenance
- [ ] Schedule monthly token cleanup
- [ ] Schedule quarterly security review
- [ ] Schedule SSL renewal check
- [ ] Monitor usage metrics

---

## Rollback Plan

### If Issues Occur

1. **Disable Onboarding:**
```sql
UPDATE SystemSettings SET EnableOnboarding = 0 WHERE Id = 1;
```

2. **Remove DNS Record:**
   - Delete CNAME for `setup.gfc.lovanow.com`

3. **Revert Backend:**
   - Remove rate limiting from Program.cs
   - Remove OnboardingController.cs
   - Restart application

4. **Investigate:**
   - Check application logs
   - Check IIS/Cloudflare logs
   - Check database for errors
   - Review security headers

---

## Success Criteria

- [ ] Gateway accessible without VPN
- [ ] Main app NOT accessible without VPN
- [ ] Valid tokens work correctly
- [ ] Invalid tokens show proper errors
- [ ] Config downloads successfully
- [ ] Connection test works
- [ ] Rate limiting prevents abuse
- [ ] Security headers all present
- [ ] SSL certificate valid
- [ ] No errors in logs
- [ ] Users can complete setup successfully

---

## Support Contacts

- **Deployment Issues:** [Your DevOps Contact]
- **DNS/SSL Issues:** [Your Network Admin]
- **Application Issues:** [Your Dev Team]
- **User Support:** [Your Help Desk]

---

## Completion Sign-Off

- [ ] Deployment completed by: _________________ Date: _______
- [ ] Testing verified by: _________________ Date: _______
- [ ] Documentation updated by: _________________ Date: _______
- [ ] Go-live approved by: _________________ Date: _______

---

**Status:** Ready for deployment ✅
