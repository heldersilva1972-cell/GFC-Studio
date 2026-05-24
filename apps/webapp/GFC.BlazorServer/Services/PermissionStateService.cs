namespace GFC.BlazorServer.Services;

/// <summary>
/// Scoped per-Blazor-circuit service.
/// NavMenu subscribes to OnPermissionsChanged and reloads its permission list
/// whenever an admin saves changes in UserPagePermissionsModal.
/// </summary>
public class PermissionStateService
{
    /// <summary>Fired after permissions are saved for any user.</summary>
    public event Action? OnPermissionsChanged;

    /// <summary>
    /// Call this after successfully saving page permissions.
    /// NavMenu will immediately reload and re-render.
    /// </summary>
    public void NotifyPermissionsChanged() => OnPermissionsChanged?.Invoke();
}
