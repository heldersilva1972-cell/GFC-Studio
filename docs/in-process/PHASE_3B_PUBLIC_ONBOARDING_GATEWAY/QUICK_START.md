# Quick Start: Onboarding Gateway

## For Administrators

### How to Generate an Onboarding Link

#### Option 1: Via Code (Temporary - Until UI is Built)

Add this temporary endpoint to test token generation:

```csharp
// In OnboardingController.cs, add this method:

/// <summary>
/// TEMPORARY: Generate an onboarding token for a user.
/// TODO: Move this to admin UI
/// </summary>
[HttpPost("generate-token")]
[Authorize(Policy = AppPolicies.RequireAdmin)]
public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
{
    try
    {
        var token = await _vpnConfigService.CreateOnboardingTokenAsync(
            request.UserId, 
            request.ExpiryHours ?? 48);
        
        var settings = await _systemSettingsService.GetSystemSettingsAsync();
        var gatewayUrl = settings?.OnboardingGatewayUrl ?? "https://setup.gfc.lovanow.com";
        var link = $"{gatewayUrl}?token={token}";
        
        _logger.LogInformation(
            "Onboarding token generated for user {UserId} by admin", 
            request.UserId);
        
        return Ok(new
        {
            token,
            link,
            expiresAt = DateTime.UtcNow.AddHours(request.ExpiryHours ?? 48),
            userId = request.UserId
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating onboarding token");
        return StatusCode(500, new { error = "Failed to generate token" });
    }
}

public class GenerateTokenRequest
{
    public int UserId { get; set; }
    public int? ExpiryHours { get; set; }
}
```

Then call it via Postman or curl:

```bash
curl -X POST https://gfc.lovanow.com/api/onboarding/generate-token \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -d '{"userId": 1, "expiryHours": 48}'
```

#### Option 2: Direct Database Insert (Quick Test)

```sql
DECLARE @Token NVARCHAR(256) = CONVERT(NVARCHAR(256), NEWID());
DECLARE @UserId INT = 1; -- Change to actual user ID

INSERT INTO VpnOnboardingTokens (UserId, Token, CreatedAtUtc, ExpiresAtUtc, IsUsed)
VALUES (@UserId, @Token, GETUTCDATE(), DATEADD(HOUR, 48, GETUTCDATE()), 0);

SELECT 
    'https://setup.gfc.lovanow.com?token=' + @Token AS OnboardingLink,
    @Token AS Token,
    DATEADD(HOUR, 48, GETUTCDATE()) AS ExpiresAt;
```

---

## For End Users

### How to Set Up VPN Access

1. **Receive your invite link** from an administrator
   - Link format: `https://setup.gfc.lovanow.com?token=ABC123...`

2. **Click the link** on the device you want to connect
   - Works on Windows, macOS, iOS, Android, Linux

3. **Follow the 3-step wizard:**
   - **Step 1:** Download WireGuard app (if not installed)
   - **Step 2:** Download your secure access profile
   - **Step 3:** Test your connection

4. **You're done!** You can now access the GFC private network

---

## Troubleshooting

### "Invalid or Expired Invite"
- Token may have expired (default: 48 hours)
- Token may have already been used
- Request a new invite from your administrator

### "Onboarding Temporarily Unavailable"
- System administrator has paused onboarding
- Contact support for assistance

### "Config Download Failed"
- Check your internet connection
- Try again in a few moments
- Contact support if problem persists

### "Connection Test Failed"
- Ensure WireGuard app is installed
- Make sure the VPN toggle is ON in WireGuard
- Wait a few seconds and try again
- Restart WireGuard app if needed

---

## Security Notes

- **Tokens are single-use** (configurable)
- **Tokens expire** after 48 hours (configurable)
- **Rate limited** to prevent abuse
- **HTTPS only** - no HTTP access
- **No sensitive data** exposed on public gateway

---

## Admin Checklist

Before sending onboarding links to users:

- [ ] Verify `EnableOnboarding` is `true` in SystemSettings
- [ ] Verify `SafeModeEnabled` is `false` in SystemSettings
- [ ] Verify WireGuard server is running
- [ ] Verify `WireGuardServerPublicKey` is set in SystemSettings
- [ ] Verify DNS record for `setup.gfc.lovanow.com` is configured
- [ ] Verify SSL certificate is valid
- [ ] Test the gateway with a test token

---

## Monitoring

### Check Token Status

```sql
-- View all active tokens
SELECT 
    Id,
    UserId,
    Token,
    CreatedAtUtc,
    ExpiresAtUtc,
    IsUsed,
    DeviceInfo
FROM VpnOnboardingTokens
WHERE ExpiresAtUtc > GETUTCDATE()
  AND IsUsed = 0
ORDER BY CreatedAtUtc DESC;
```

### Check Recent Onboarding Activity

```sql
-- View tokens used in last 7 days
SELECT 
    Id,
    UserId,
    CreatedAtUtc,
    ExpiresAtUtc,
    IsUsed,
    DeviceInfo
FROM VpnOnboardingTokens
WHERE CreatedAtUtc > DATEADD(DAY, -7, GETUTCDATE())
ORDER BY CreatedAtUtc DESC;
```

### Clean Up Expired Tokens

```sql
-- Delete tokens expired more than 30 days ago
DELETE FROM VpnOnboardingTokens
WHERE ExpiresAtUtc < DATEADD(DAY, -30, GETUTCDATE());
```

---

## Future UI Integration

### Recommended Admin Panel Features

1. **Token Management Page**
   - List all active tokens
   - Generate new tokens
   - Revoke tokens
   - View usage history

2. **User Management Integration**
   - "Send Onboarding Invite" button on user profile
   - Auto-email onboarding link
   - Track onboarding status

3. **Analytics Dashboard**
   - Onboarding completion rate
   - Average time to complete
   - Platform distribution
   - Failed attempts

4. **Email Templates**
   - Welcome email with onboarding link
   - Reminder email if not completed
   - Success confirmation

---

## Configuration

### SystemSettings Fields

- `EnableOnboarding` (bool) - Master switch for onboarding
- `SafeModeEnabled` (bool) - Pause onboarding during maintenance
- `OnboardingGatewayUrl` (string) - Gateway URL (default: https://setup.gfc.lovanow.com)
- `OnboardingTokenExpiryHours` (int) - Token expiry (default: 48)
- `OnboardingRateLimitPerMinute` (int) - Rate limit (default: 10)
- `WireGuardServerPublicKey` (string) - Server public key
- `WireGuardPort` (int) - Server port (default: 51820)
- `WireGuardSubnet` (string) - VPN subnet (default: 10.8.0.0/24)
- `WireGuardAllowedIPs` (string) - Allowed IPs (default: 10.8.0.0/24, 192.168.1.0/24)

---

## Support Contacts

- **Technical Issues:** [Your IT Contact]
- **Access Requests:** [Your Admin Contact]
- **Emergency:** [Your Emergency Contact]
