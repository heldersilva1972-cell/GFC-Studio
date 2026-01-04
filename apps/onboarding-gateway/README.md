# GFC Public Onboarding Gateway

A minimal, secure, public-facing gateway for bootstrapping new devices with VPN access to the GFC Private Network.

## 🎯 Purpose

This gateway serves **one purpose only**: to help new users set up WireGuard VPN access to the private GFC application. It:

- ✅ Is publicly accessible (no VPN required)
- ✅ Never exposes the private GFC web app
- ✅ Delivers signed onboarding artifacts
- ✅ Provides platform-specific setup instructions
- ❌ Does NOT authenticate users
- ❌ Does NOT access the database directly
- ❌ Does NOT contain business logic

## 📁 Structure

```
onboarding-gateway/
├── index.html          # Main landing page
├── css/
│   └── styles.css      # Minimal, modern styling
├── js/
│   └── setup.js        # OS detection, token validation
├── assets/
│   └── logo.svg        # GFC logo
└── README.md           # This file
```

## 🚀 Deployment

### Option 1: Cloudflare Pages (Recommended)

1. **Create a Cloudflare Pages project:**
   ```bash
   # From the GFC-Studio V2 root directory
   cd apps/onboarding-gateway
   ```

2. **Connect to Git:**
   - Push this directory to a Git repository
   - Connect Cloudflare Pages to the repository

3. **Configure build settings:**
   - Build command: (none - static site)
   - Build output directory: `/`
   - Root directory: `apps/onboarding-gateway`

4. **Set environment variables:**
   - `API_BASE_URL`: Your private GFC app URL (e.g., `https://gfc.lovanow.com`)

5. **Configure custom domain:**
   - Add `setup.gfc.lovanow.com` as a custom domain
   - Cloudflare will automatically provision SSL

### Option 2: Netlify

1. **Install Netlify CLI:**
   ```bash
   npm install -g netlify-cli
   ```

2. **Deploy:**
   ```bash
   cd apps/onboarding-gateway
   netlify deploy --prod
   ```

3. **Configure custom domain:**
   - Add `setup.gfc.lovanow.com` in Netlify dashboard
   - Update DNS records as instructed

### Option 3: IIS (Windows Server)

1. **Create a new IIS site:**
   - Site name: `GFC Onboarding Gateway`
   - Physical path: `C:\inetpub\gfc-onboarding`
   - Binding: HTTPS on port 443
   - Host name: `setup.gfc.lovanow.com`

2. **Copy files:**
   ```powershell
   Copy-Item -Path "apps\onboarding-gateway\*" -Destination "C:\inetpub\gfc-onboarding" -Recurse
   ```

3. **Configure SSL:**
   - Install SSL certificate for `setup.gfc.lovanow.com`
   - Bind certificate to the site

4. **Update API URL:**
   - Edit `js/setup.js`
   - Update `CONFIG.apiBaseUrl` to your private app URL

## 🔒 Security Configuration

### Required HTTP Headers

Add these headers to your web server configuration:

```
Content-Security-Policy: default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:;
X-Frame-Options: DENY
X-Content-Type-Options: nosniff
Referrer-Policy: no-referrer
Permissions-Policy: geolocation=(), microphone=(), camera=()
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

### Cloudflare Pages Headers

Create a `_headers` file in the root:

```
/*
  Content-Security-Policy: default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:;
  X-Frame-Options: DENY
  X-Content-Type-Options: nosniff
  Referrer-Policy: no-referrer
  Permissions-Policy: geolocation=(), microphone=(), camera=()
  Strict-Transport-Security: max-age=31536000; includeSubDomains
```

### IIS web.config

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name="Content-Security-Policy" value="default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:;" />
        <add name="X-Frame-Options" value="DENY" />
        <add name="X-Content-Type-Options" value="nosniff" />
        <add name="Referrer-Policy" value="no-referrer" />
        <add name="Permissions-Policy" value="geolocation=(), microphone=(), camera=()" />
      </customHeaders>
    </httpProtocol>
    <rewrite>
      <rules>
        <rule name="HTTPS Redirect" stopProcessing="true">
          <match url="(.*)" />
          <conditions>
            <add input="{HTTPS}" pattern="off" />
          </conditions>
          <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

## 🔧 Configuration

### API Base URL

Update the API base URL in `js/setup.js`:

```javascript
const CONFIG = {
    apiBaseUrl: 'https://gfc.lovanow.com',
    // ... other config
};
```

### Rate Limiting (Backend)

Configure rate limiting in the backend `Program.cs`:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("onboarding", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 5;
    });
});
```

## 🧪 Testing

### Local Testing

1. **Start a local web server:**
   ```bash
   # Python 3
   python -m http.server 8080
   
   # Node.js
   npx http-server -p 8080
   ```

2. **Access the gateway:**
   ```
   http://localhost:8080?token=YOUR_TEST_TOKEN
   ```

### Production Testing

1. **Without VPN:**
   - Access `https://setup.gfc.lovanow.com` → Should load
   - Access `https://gfc.lovanow.com` → Should fail (VPN required)

2. **With valid token:**
   - Visit `https://setup.gfc.lovanow.com?token=VALID_TOKEN`
   - Verify OS detection
   - Download config
   - Test connection

3. **With invalid token:**
   - Visit `https://setup.gfc.lovanow.com?token=INVALID`
   - Verify error message

## 📊 Monitoring

### Cloudflare Analytics

- Page views
- Unique visitors
- Bandwidth usage
- Error rates

### Backend Logs

Monitor these events in your GFC app logs:
- Token validation attempts
- Config downloads
- Failed validations
- Rate limit violations

## 🔄 Updates

To update the gateway:

1. **Cloudflare Pages:**
   - Push changes to Git
   - Cloudflare auto-deploys

2. **Netlify:**
   - Push changes to Git
   - Netlify auto-deploys

3. **IIS:**
   - Copy updated files to `C:\inetpub\gfc-onboarding`
   - No restart required

## 🆘 Troubleshooting

### Gateway not loading
- Check DNS records for `setup.gfc.lovanow.com`
- Verify SSL certificate is valid
- Check web server logs

### Token validation fails
- Verify `apiBaseUrl` in `setup.js` is correct
- Check CORS settings on backend
- Verify backend is accessible from gateway

### Config download fails
- Check backend logs for errors
- Verify WireGuard settings in SystemSettings
- Ensure user has valid VPN profile

### Connection test fails
- Verify WireGuard is running
- Check VPN subnet configuration
- Ensure health endpoint is accessible

## 📚 Related Documentation

- [WireGuard Documentation](https://www.wireguard.com/)
- [Cloudflare Pages Docs](https://developers.cloudflare.com/pages/)
- Backend API: `apps/webapp/GFC.BlazorServer/Controllers/OnboardingController.cs`
- VPN Service: `apps/webapp/GFC.BlazorServer/Services/Vpn/VpnConfigurationService.cs`

## 🔐 Security Notes

- This gateway is **intentionally minimal** to reduce attack surface
- Never add authentication or database access to this gateway
- All business logic must remain in the private GFC app
- Rate limiting is enforced at the backend API level
- Tokens are single-use and expire after 48 hours (configurable)

## 📝 License

Copyright © 2026 GFC System. All rights reserved.
