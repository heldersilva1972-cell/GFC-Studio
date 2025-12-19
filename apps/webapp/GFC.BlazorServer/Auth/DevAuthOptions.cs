using System.Security.Claims;

namespace GFC.BlazorServer.Auth;

public class DevAuthOptions
{
    public bool AutoAdminBypassEnabled { get; set; }
}

public static class DevAuthDefaults
{
    public const string AuthenticationScheme = "GfcAuth";
    public const string DevBypassClaimType = "DevAuth.Bypass";
    public const string DevPrincipalItemKey = "DevAuth.Principal";
}

public static class DevAuthIdentityFactory
{
    public static ClaimsPrincipal CreateDevAdminPrincipal()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "DEV-ADMIN"),
            new Claim(ClaimTypes.Name, "Dev Admin"),
            new Claim("UserId", "-1"),
            new Claim("IsAdmin", "true"),
            new Claim(ClaimTypes.Role, AppRoles.Admin),
            new Claim(DevAuthDefaults.DevBypassClaimType, "true")
        };

        var identity = new ClaimsIdentity(claims, DevAuthDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}

