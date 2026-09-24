namespace GFC.BlazorServer.Auth;

/// <summary>
/// Central place for role names so attributes stay consistent.
/// </summary>
public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Director = "Director";
    public const string StudioUnlock = "StudioUnlock";
    public const string ReportingAnalyst = "ReportingAnalyst";
}

/// <summary>
/// Authorization policy names mapped to canonical requirements.
/// </summary>
public static class AppPolicies
{
    public const string RequireAdmin = "RequireAdmin";
    public const string CanForceUnlock = "CanForceUnlock";
    public const string CanAccessReportingApi = "CanAccessReportingApi";
}

