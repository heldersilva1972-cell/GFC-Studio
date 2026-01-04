# ISSUE 1 — Public Onboarding Gateway (setup.gfc.lovanow.com)

**Type:** Feature / Security  
**Priority:** High  
**Phase:** 3B – Onboarding Infrastructure  
**Status:** In Progress

---

## Goal

Create a minimal, public, non-app onboarding gateway used only to bootstrap new devices with secure access.

This gateway must:
- ✅ Be reachable without VPN
- ✅ Never expose the private GFC web app
- ✅ Deliver signed onboarding artifacts
- ✅ Provide platform-specific instructions

---

## Scope

### ✅ In scope:
- Static site at `https://setup.gfc.lovanow.com`
- OS detection (Windows / iOS / Android / macOS / Linux)
- One primary action: "Secure Access Setup"
- Optional invite token / access code gate
- Platform-specific artifact delivery
- Minimal, secure JavaScript (OS detection + token validation)

### ❌ Out of scope:
- Hosting or proxying the private web app
- Authentication into the GFC app
- Business logic
- User management
- Database operations from the gateway

---

## Requirements

### Security
1. **HTTPS only** - No HTTP access permitted
2. **No reverse proxy to private services** - Gateway is isolated
3. **No open ports beyond HTTPS (443)** - Minimal attack surface
4. **Token-based access** - Optional invite code/token validation
5. **Rate limiting** - Prevent abuse of token validation endpoint
6. **CORS restrictions** - Only allow requests from authorized domains

### Functionality
1. **Platform detection** - Automatically detect user's OS
2. **Artifact delivery** - Serve WireGuard configs with correct MIME types
3. **Token validation** - Verify invite codes via secure API
4. **Clear instructions** - Step-by-step guidance per platform
5. **Error handling** - Graceful handling of invalid/expired tokens

### Performance
1. **Fast loading** - Minimal JS, optimized assets
2. **CDN-ready** - Static assets can be cached
3. **Mobile-optimized** - Responsive design for all devices

---

## Architecture

### Components

```
┌─────────────────────────────────────────┐
│  setup.gfc.lovanow.com (Public)         │
│  ┌───────────────────────────────────┐  │
│  │  Static HTML/CSS/JS               │  │
│  │  - index.html                     │  │
│  │  - styles.css                     │  │
│  │  - setup.js                       │  │
│  └───────────────────────────────────┘  │
│              │                           │
│              ▼                           │
│  ┌───────────────────────────────────┐  │
│  │  API Endpoint (Token Validation)  │  │
│  │  /api/onboarding/validate         │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
              │
              ▼ (HTTPS only)
┌─────────────────────────────────────────┐
│  Private GFC App (VPN-protected)        │
│  - Token validation service             │
│  - Config generation service            │
│  - Audit logging                        │
└─────────────────────────────────────────┘
```

### Flow

1. **User receives invite link** → `https://setup.gfc.lovanow.com?token=ABC123`
2. **Gateway validates token** → API call to private app
3. **OS detection** → JavaScript determines platform
4. **Instructions displayed** → Platform-specific steps
5. **Config download** → Signed WireGuard config delivered
6. **Token marked as used** → Prevent reuse (optional)

---

## Implementation Details

### 1. Static Gateway Site

**Location:** `apps/onboarding-gateway/`

**Files:**
```
apps/onboarding-gateway/
├── index.html          # Main landing page
├── css/
│   └── styles.css      # Minimal, modern styling
├── js/
│   └── setup.js        # OS detection, token validation
├── assets/
│   ├── logo.svg        # GFC logo
│   └── icons/          # Platform icons
└── README.md           # Deployment instructions
```

### 2. API Endpoint (Private App)

**New Controller:** `OnboardingApiController.cs`

**Endpoints:**
- `GET /api/onboarding/validate?token={token}` - Validate token, return user info
- `GET /api/onboarding/config?token={token}` - Generate and download WireGuard config
- `POST /api/onboarding/complete?token={token}` - Mark onboarding as complete

**Security:**
- Rate limiting: 10 requests per minute per IP
- CORS: Only allow `setup.gfc.lovanow.com`
- Token expiry: 48 hours default
- Single-use tokens (optional, configurable)

### 3. Database Schema

**Existing:** `VpnOnboardingTokens` table (already exists)

**Fields:**
- `Id` (int, PK)
- `UserId` (int, FK to Users)
- `Token` (string, unique, indexed)
- `CreatedAtUtc` (DateTime)
- `ExpiresAtUtc` (DateTime)
- `IsUsed` (bool)
- `DeviceInfo` (string, nullable)

**New Index:**
```sql
CREATE INDEX IX_VpnOnboardingTokens_Token ON VpnOnboardingTokens(Token);
```

### 4. SystemSettings Updates

**New Fields:**
- `OnboardingGatewayUrl` (string) - Default: `https://setup.gfc.lovanow.com`
- `OnboardingTokenExpiryHours` (int) - Default: 48
- `OnboardingRateLimitPerMinute` (int) - Default: 10

---

## Acceptance Criteria

### ✅ Gateway Accessibility
- [ ] New device (no VPN) can reach `https://setup.gfc.lovanow.com`
- [ ] Gateway loads in under 2 seconds
- [ ] Gateway is mobile-responsive

### ✅ Platform Detection
- [ ] Correctly detects Windows, macOS, iOS, Android, Linux
- [ ] Displays platform-specific instructions
- [ ] Provides correct download links for WireGuard app

### ✅ Token Validation
- [ ] Valid token allows access to setup flow
- [ ] Expired token shows clear error message
- [ ] Used token (if single-use) shows error
- [ ] Invalid token shows error

### ✅ Config Download
- [ ] Clicking "Download Config" generates WireGuard config
- [ ] Config file has correct MIME type (`application/x-wireguard-profile`)
- [ ] Config contains valid WireGuard settings
- [ ] Config is unique per user

### ✅ Security
- [ ] Private GFC app remains unreachable without VPN
- [ ] No sensitive services exposed
- [ ] Rate limiting prevents abuse
- [ ] HTTPS enforced (no HTTP)

### ✅ Error Handling
- [ ] Missing token shows friendly error
- [ ] Network errors handled gracefully
- [ ] Clear instructions for troubleshooting

---

## Testing Plan

### Manual Testing
1. **Without VPN:**
   - Access `https://setup.gfc.lovanow.com` → Should load
   - Access `https://gfc.lovanow.com` → Should fail (VPN required)

2. **With Valid Token:**
   - Visit `https://setup.gfc.lovanow.com?token=VALID_TOKEN`
   - Verify OS detection
   - Download config
   - Verify config contents

3. **With Invalid Token:**
   - Visit `https://setup.gfc.lovanow.com?token=INVALID`
   - Verify error message

4. **Platform Testing:**
   - Test on Windows, macOS, iOS, Android
   - Verify correct instructions per platform

### Automated Testing
1. **API Tests:**
   - Token validation endpoint
   - Config generation endpoint
   - Rate limiting

2. **Security Tests:**
   - HTTPS enforcement
   - CORS restrictions
   - Token expiry

---

## Deployment

### Hosting Options

**Option 1: Cloudflare Pages (Recommended)**
- Static site hosting
- Free SSL
- Global CDN
- Easy deployment from Git

**Option 2: Netlify**
- Similar to Cloudflare Pages
- Free tier available

**Option 3: Separate IIS Site**
- Host on same server as main app
- Different port/subdomain
- Manual SSL setup

### DNS Configuration
```
setup.gfc.lovanow.com → CNAME → [Cloudflare Pages URL]
```

### Environment Variables
- `API_BASE_URL` - URL of private GFC app API
- `RATE_LIMIT_PER_MINUTE` - Default: 10

---

## Security Considerations

### Threats Mitigated
1. **Unauthorized access** - Token-based validation
2. **Token enumeration** - Rate limiting
3. **MITM attacks** - HTTPS only
4. **XSS attacks** - Minimal JS, CSP headers
5. **Clickjacking** - X-Frame-Options header

### Security Headers
```
Content-Security-Policy: default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline';
X-Frame-Options: DENY
X-Content-Type-Options: nosniff
Referrer-Policy: no-referrer
Permissions-Policy: geolocation=(), microphone=(), camera=()
```

---

## Future Enhancements

1. **QR Code Support** - Generate QR codes for mobile setup
2. **Multi-language Support** - Internationalization
3. **Analytics** - Track setup completion rates
4. **Email Notifications** - Notify admins of new device setups
5. **Device Fingerprinting** - Enhanced security

---

## References

- [WireGuard Documentation](https://www.wireguard.com/)
- [Cloudflare Pages Docs](https://developers.cloudflare.com/pages/)
- Existing implementation: `apps/webapp/GFC.BlazorServer/Components/Pages/Setup/Onboarding.razor`
- Token service: `apps/webapp/GFC.BlazorServer/Services/Vpn/VpnConfigurationService.cs`

---

## Notes

- This gateway is **intentionally minimal** to reduce attack surface
- The gateway should **never** authenticate users or access the database directly
- All business logic remains in the private GFC app
- The gateway is a **bootstrap mechanism only**
