# Public Onboarding Gateway - Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         INTERNET (Public Access)                         │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ HTTPS
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                    setup.gfc.lovanow.com (Public)                        │
│  ┌───────────────────────────────────────────────────────────────────┐  │
│  │                    Static HTML/CSS/JS Gateway                     │  │
│  │                                                                   │  │
│  │  ┌─────────────┐  ┌──────────────┐  ┌────────────────────┐      │  │
│  │  │  OS Detect  │  │ Token Valid  │  │  Config Download   │      │  │
│  │  │  (Client)   │  │  (API Call)  │  │    (API Call)      │      │  │
│  │  └─────────────┘  └──────────────┘  └────────────────────┘      │  │
│  │                                                                   │  │
│  │  Security:                                                        │  │
│  │  • HTTPS Only                                                     │  │
│  │  • No Database Access                                             │  │
│  │  • No Authentication                                              │  │
│  │  • Minimal JS (OS detection only)                                 │  │
│  └───────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ HTTPS API Calls
                                    │ (Rate Limited: 10/min)
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                      API Endpoints (Public Access)                       │
│  ┌───────────────────────────────────────────────────────────────────┐  │
│  │  /api/onboarding/validate?token=ABC123                           │  │
│  │  /api/onboarding/config?token=ABC123                             │  │
│  │  /api/health/vpn-check                                            │  │
│  └───────────────────────────────────────────────────────────────────┘  │
│                                                                          │
│  Security:                                                               │
│  • Rate Limiting (10 req/min per IP)                                    │
│  • CORS (setup.gfc.lovanow.com only)                                    │
│  • Token Validation                                                      │
│  • Audit Logging                                                         │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ Internal
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                   Private GFC App (VPN Required)                         │
│  ┌───────────────────────────────────────────────────────────────────┐  │
│  │                    VpnConfigurationService                        │  │
│  │  • ValidateOnboardingTokenAsync()                                 │  │
│  │  • GenerateConfigForUserAsync()                                   │  │
│  │  • CreateOnboardingTokenAsync()                                   │  │
│  └───────────────────────────────────────────────────────────────────┘  │
│                                    │                                     │
│                                    ▼                                     │
│  ┌───────────────────────────────────────────────────────────────────┐  │
│  │                         Database                                  │  │
│  │  ┌─────────────────────────────────────────────────────────────┐ │  │
│  │  │  VpnOnboardingTokens                                         │ │  │
│  │  │  • Id, UserId, Token                                         │ │  │
│  │  │  • CreatedAtUtc, ExpiresAtUtc                                │ │  │
│  │  │  • IsUsed, DeviceInfo                                        │ │  │
│  │  │  • Indexes: Token, UserId, ExpiresAtUtc                      │ │  │
│  │  └─────────────────────────────────────────────────────────────┘ │  │
│  │  ┌─────────────────────────────────────────────────────────────┐ │  │
│  │  │  SystemSettings                                              │ │  │
│  │  │  • EnableOnboarding, SafeModeEnabled                         │ │  │
│  │  │  • OnboardingGatewayUrl                                      │ │  │
│  │  │  • OnboardingTokenExpiryHours                                │ │  │
│  │  │  • WireGuardServerPublicKey, WireGuardPort                   │ │  │
│  │  └─────────────────────────────────────────────────────────────┘ │  │
│  └───────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ VPN Tunnel
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                         WireGuard VPN Server                             │
│  • Subnet: 10.8.0.0/24                                                  │
│  • Port: 51820                                                           │
│  • Allowed IPs: 10.8.0.0/24, 192.168.1.0/24                             │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## User Flow Diagram

```
┌──────────────┐
│  New User    │
│  (No VPN)    │
└──────┬───────┘
       │
       │ 1. Receives invite link from admin
       │    https://setup.gfc.lovanow.com?token=ABC123
       │
       ▼
┌──────────────────────────────────────────┐
│  Public Gateway (setup.gfc.lovanow.com)  │
│                                          │
│  Step 1: Download WireGuard App          │
│  ┌────────────────────────────────────┐  │
│  │ • Detects OS (Windows/Mac/iOS/etc) │  │
│  │ • Shows platform-specific link     │  │
│  │ • User downloads WireGuard app     │  │
│  └────────────────────────────────────┘  │
│                                          │
│  Step 2: Import Configuration            │
│  ┌────────────────────────────────────┐  │
│  │ • Validates token via API          │  │
│  │ • Generates WireGuard config       │  │
│  │ • Downloads .conf file             │  │
│  │ • User imports into WireGuard      │  │
│  └────────────────────────────────────┘  │
│                                          │
│  Step 3: Test Connection                 │
│  ┌────────────────────────────────────┐  │
│  │ • User enables VPN in WireGuard    │  │
│  │ • Clicks "Test Connection"         │  │
│  │ • Gateway pings VPN endpoint       │  │
│  │ • Shows success/failure            │  │
│  └────────────────────────────────────┘  │
└──────────────────────────────────────────┘
       │
       │ 2. VPN Connected
       │
       ▼
┌──────────────────────────────────────────┐
│  Private GFC App (gfc.lovanow.com)       │
│  • Now accessible via VPN                │
│  • Full authentication required          │
│  • All features available                │
└──────────────────────────────────────────┘
```

---

## Security Boundaries

```
┌─────────────────────────────────────────────────────────────────┐
│                        PUBLIC ZONE                               │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  setup.gfc.lovanow.com                                    │  │
│  │  • No VPN required                                        │  │
│  │  • No authentication                                      │  │
│  │  • No database access                                     │  │
│  │  • Rate limited                                           │  │
│  │  • HTTPS only                                             │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              │ Firewall
                              │ (API endpoints only)
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      SEMI-PUBLIC ZONE                            │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  /api/onboarding/*                                        │  │
│  │  /api/health/vpn-check                                    │  │
│  │  • Rate limited (10/min)                                  │  │
│  │  • Token validation only                                  │  │
│  │  • No sensitive data                                      │  │
│  │  • Audit logged                                           │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              │ VPN Required
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                       PRIVATE ZONE                               │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  gfc.lovanow.com                                          │  │
│  │  • VPN required                                           │  │
│  │  • Full authentication                                    │  │
│  │  • Database access                                        │  │
│  │  • All business logic                                     │  │
│  │  • Sensitive data                                         │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Data Flow

```
1. Token Generation (Admin → Database)
   ┌───────┐         ┌──────────┐         ┌──────────┐
   │ Admin │ ──────> │ GFC App  │ ──────> │ Database │
   └───────┘  Create  └──────────┘  INSERT └──────────┘
              Token      Token              VpnOnboardingTokens

2. Token Validation (User → Gateway → API → Database)
   ┌──────┐    ┌─────────┐    ┌─────────┐    ┌──────────┐
   │ User │ -> │ Gateway │ -> │ API     │ -> │ Database │
   └──────┘    └─────────┘    └─────────┘    └──────────┘
     Click      Validate       Check Token    SELECT Token
     Link       via API        Expiry/Used    WHERE Token=?

3. Config Generation (User → Gateway → API → Service)
   ┌──────┐    ┌─────────┐    ┌─────────┐    ┌──────────────┐
   │ User │ -> │ Gateway │ -> │ API     │ -> │ VpnConfig    │
   └──────┘    └─────────┘    └─────────┘    │ Service      │
     Click      Download       Generate       └──────────────┘
     Button     Config         WireGuard           │
                               Config              │
                                                   ▼
                               ┌─────────────────────────────┐
                               │ [Interface]                 │
                               │ PrivateKey = xxx            │
                               │ Address = 10.8.0.X/32       │
                               │                             │
                               │ [Peer]                      │
                               │ PublicKey = SERVER_KEY      │
                               │ AllowedIPs = 10.8.0.0/24    │
                               │ Endpoint = vpn.gfc.com:51820│
                               └─────────────────────────────┘

4. Connection Test (User → Gateway → VPN Endpoint)
   ┌──────┐    ┌─────────┐    ┌──────────────┐
   │ User │ -> │ Gateway │ -> │ /api/health/ │
   └──────┘    └─────────┘    │ vpn-check    │
     Click      Test           └──────────────┘
     Test       Connection           │
                                     ▼
                               Check if request
                               from VPN subnet
                               (10.8.0.0/24)
```

---

## Threat Model

```
┌─────────────────────────────────────────────────────────────┐
│                      THREATS MITIGATED                       │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  1. Token Enumeration                                       │
│     ✓ Rate limiting (10/min)                                │
│     ✓ Random tokens (GUID-based)                            │
│     ✓ Audit logging                                         │
│                                                             │
│  2. Unauthorized Access                                     │
│     ✓ Token expiry (48 hours)                               │
│     ✓ Single-use tokens (optional)                          │
│     ✓ VPN required for main app                             │
│                                                             │
│  3. Man-in-the-Middle                                       │
│     ✓ HTTPS only (no HTTP)                                  │
│     ✓ HSTS header                                           │
│     ✓ SSL certificate validation                            │
│                                                             │
│  4. XSS Attacks                                             │
│     ✓ CSP headers                                           │
│     ✓ Minimal JavaScript                                    │
│     ✓ No user input stored                                  │
│                                                             │
│  5. Clickjacking                                            │
│     ✓ X-Frame-Options: DENY                                 │
│     ✓ CSP frame-ancestors 'none'                            │
│                                                             │
│  6. Information Disclosure                                  │
│     ✓ No database access from gateway                       │
│     ✓ No sensitive data in responses                        │
│     ✓ Generic error messages                                │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

This architecture ensures:
- ✅ **Minimal Attack Surface** - Gateway has no database access
- ✅ **Defense in Depth** - Multiple security layers
- ✅ **Principle of Least Privilege** - Gateway only validates tokens
- ✅ **Separation of Concerns** - Public gateway isolated from private app
- ✅ **Fail Secure** - Errors default to denying access
