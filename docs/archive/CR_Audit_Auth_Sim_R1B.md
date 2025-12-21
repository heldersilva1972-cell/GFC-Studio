# Code Review Audit – Authentication & Simulation Mode (R1B)
Date: December 1, 2025  
Auditor: Cursor + ChatGPT

## Summary
Authentication relies on repository-backed credential validation with hashing and explicit login history logging; no simulation-specific bypass exists. However, claims issued during login never populate `ClaimTypes.Role`, so `[Authorize(Roles="Admin")]` endpoints remain unreachable, forcing components to fall back to manual checks. Simulation mode is stored in `SystemSettings` (database) but visual indicators and some checks rely on a singleton `SimulationModeService` that reads only configuration at startup, and the scoped DI factory for `IControllerClient` locks the chosen implementation for the lifetime of each Blazor circuit. These design choices explain why CR-002 reproduces: toggling “Use Real Controllers” rarely updates active sessions or UI indicators without reconnecting.

## AUTH-1: Login validation flow
- `Components/Pages/Login.razor` validates non-empty username/password, logs bypass flag status, and always delegates to `CustomAuthenticationStateProvider.LoginAsync`; there is no Simulation Mode branch or shortcut.

```88:155:GFC.BlazorServer/Components/Pages/Login.razor
    private async Task HandleLoginAsync()
    {
        _error = null;
        _isSubmitting = true;
        ...
        var result = await AuthStateProvider.LoginAsync(_model.Username, _model.Password);
        if (!result.Success)
        {
            _error = result.ErrorMessage ?? "Invalid username or password.";
            Logger.LogWarning("Login failed for {Username}: {Reason}", _model.Username, _error);
            return;
        }
        ...
        Navigation.NavigateTo("/", forceLoad: false);
    }
```

- `CustomAuthenticationStateProvider.LoginAsync` simply wraps `_authenticationService.LoginAsync`, promoting the user to a new `ClaimsPrincipal` only when the service returns success; failed attempts reset the principal to an empty identity.

```42:79:GFC.BlazorServer/Services/CustomAuthenticationStateProvider.cs
    private ClaimsPrincipal BuildPrincipal(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("IsAdmin", user.IsAdmin ? "true" : "false")
        };
        var identity = new ClaimsIdentity(claims, authenticationType: "GfcAuth");
        return new ClaimsPrincipal(identity);
    }

    public async Task<LoginResult> LoginAsync(string username, string password, string? ipAddress = null)
    {
        var result = await _authenticationService.LoginAsync(username, password, ipAddress);
        if (result.Success && result.User is not null)
        {
            _currentUser = result.User;
            _currentPrincipal = BuildPrincipal(result.User);
        }
        else
        {
            _currentUser = null;
            _currentPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
        return result;
    }
```

- `GFC.Core/Services/AuthenticationService.LoginAsync` fetches the user via `_userRepository`, enforces `IsActive`, compares hashes with `PasswordHelper.VerifyPassword`, logs every outcome through `_loginHistoryRepository`, and force-disables `AUTH_BYPASS_ENABLED`, so Simulation Mode never bypasses validation.

```24:158:GFC.Core/Services/AuthenticationService.cs
    public async Task<LoginResult> LoginAsync(string username, string password, string? ipAddress = null)
    {
        ...
        user = _userRepository.GetByUsername(username);
        if (user == null) { await SafeLogLogin(... "User not found"); return new LoginResult { Success = false, ErrorMessage = "Invalid username or password." }; }
        if (!user.IsActive) { ... return new LoginResult { Success = false, ErrorMessage = "This user account is inactive." }; }
        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash)) { ... return new LoginResult { Success = false, ErrorMessage = "Invalid username or password." }; }
        _currentUser = user;
        await SafeLogLogin(username, user.UserId, true, ipAddress, null);
        return new LoginResult { Success = true, User = user, PasswordChangeRequired = user.PasswordChangeRequired };
    }

    public bool IsBypassEnabled()
    {
        if (_settingsRepository.GetBoolSetting("AUTH_BYPASS_ENABLED", false))
        {
            _settingsRepository.SetBoolSetting("AUTH_BYPASS_ENABLED", false, "System");
        }
        return false;
    }
```

- Risk: [No issue found]

## AUTH-2: Invalid credentials behavior
- When `LoginAsync` returns `Success = false`, the page keeps user input, surfaces the returned error via `_error`, and shows it through `<ValidationSummary>` without clearing the form.

```123:155:GFC.BlazorServer/Components/Pages/Login.razor
        var result = await AuthStateProvider.LoginAsync(_model.Username, _model.Password);
        if (!result.Success)
        {
            _error = result.ErrorMessage ?? "Invalid username or password.";
            Logger.LogWarning("Login failed for {Username}: {Reason}", _model.Username, _error);
            return;
        }
```

- `AuthenticationService.LoginAsync` enumerates explicit error messages for missing credentials, missing users, inactive accounts, password mismatches, and unexpected exceptions, logging each attempt through `SafeLogLogin`, so callers always see a reason.

```37:131:GFC.Core/Services/AuthenticationService.cs
        if (user == null)
        {
            await SafeLogLogin(username, null, false, ipAddress, "User not found");
            return new LoginResult { Success = false, ErrorMessage = "Invalid username or password." };
        }
        if (!user.IsActive)
        {
            await SafeLogLogin(username, user.UserId, false, ipAddress, "Inactive user");
            return new LoginResult { Success = false, ErrorMessage = "This user account is inactive." };
        }
        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
        {
            await SafeLogLogin(username, user.UserId, false, ipAddress, "Invalid password");
            return new LoginResult { Success = false, ErrorMessage = "Invalid username or password." };
        }
```

- `CustomAuthenticationStateProvider.LoginAsync` ensures failed logins result in a blank `ClaimsPrincipal`, preventing a stale authenticated state after errors.

```57:78:GFC.BlazorServer/Services/CustomAuthenticationStateProvider.cs
        if (result.Success && result.User is not null)
        {
            _currentUser = result.User;
            _currentPrincipal = BuildPrincipal(result.User);
        }
        else
        {
            _currentUser = null;
            _currentPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
```

- Risk: [No issue found]

## AUTH-3: Role/claims loading
- `CustomAuthenticationStateProvider.BuildPrincipal` issues only `ClaimTypes.Name`, a custom `"UserId"`, and `"IsAdmin"` claims; it never emits `ClaimTypes.Role` (or any director/board roles), so the resulting principal fails role-based authorization checks.

```42:55:GFC.BlazorServer/Services/CustomAuthenticationStateProvider.cs
    private ClaimsPrincipal BuildPrincipal(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("IsAdmin", user.IsAdmin ? "true" : "false")
        };
        var identity = new ClaimsIdentity(claims, authenticationType: "GfcAuth");
        return new ClaimsPrincipal(identity);
    }
```

- Pages such as `Components/Pages/Controllers/SimulationPanel.razor` rely on `[Authorize(Roles = "Admin")]`, but because no `ClaimTypes.Role` exists, the attribute never succeeds; the page later injects `AuthStateProvider` to perform manual checks, resulting in inconsistent protection.

```1:44:GFC.BlazorServer/Components/Pages/Controllers/SimulationPanel.razor
@attribute [Authorize(Roles = "Admin")]
...
@inject CustomAuthenticationStateProvider AuthStateProvider
@if (SimulationModeService.UseRealControllers)
{
    <div class="alert alert-info">
        ...
    </div>
}
```

- `AppUser` only stores a boolean `IsAdmin`; there is no notion of director, board, or other roles to attach as claims, so special permissions cannot be represented in the identity.

```6:19:GFC.Core/Models/AppUser.cs
public class AppUser
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public int? MemberId { get; set; }
    ...
}
```

- Risk: [Medium risk incorrect behavior]

## SIM-1: Simulation Mode storage
- `Data/Entities/SystemSettings` creates a single-row table where `UseRealControllers` defines whether real hardware is active; it defaults to `false`.

```6:29:GFC.BlazorServer/Data/Entities/SystemSettings.cs
public class SystemSettings
{
    [Key]
    public int Id { get; set; } = 1;
    public bool UseRealControllers { get; set; } = false;
    public DateTime? LastUpdatedUtc { get; set; }
}
```

- `SystemSettingsService` reads/writes that row, and `Settings.razor` binds the toggle to `_useRealControllers`, persisting changes via `SetUseRealControllersAsync` while logging which authenticated user triggered the change.

```22:52:GFC.BlazorServer/Services/SystemSettingsService.cs
    public async Task<SystemSettings> GetAsync()
    {
        var settings = await _dbContext.SystemSettings.FindAsync(1);
        if (settings == null)
        {
            settings = new SystemSettings { Id = 1, UseRealControllers = false, LastUpdatedUtc = null };
            _dbContext.SystemSettings.Add(settings);
            await _dbContext.SaveChangesAsync();
        }
        return settings;
    }

    public async Task SetUseRealControllersAsync(bool value)
    {
        var settings = await GetAsync();
        settings.UseRealControllers = value;
        settings.LastUpdatedUtc = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }
```

```562:610:GFC.BlazorServer/Components/Pages/Settings.razor
    private async Task LoadControllerSettingsAsync()
    {
        var settings = await SystemSettingsService.GetAsync();
        _useRealControllers = settings.UseRealControllers;
    }
    ...
    private async Task SaveControllerSettings()
    {
        var oldSettings = await SystemSettingsService.GetAsync();
        var oldValue = oldSettings.UseRealControllers;
        await SystemSettingsService.SetUseRealControllersAsync(_useRealControllers);
        if (oldValue != _useRealControllers)
        {
            var currentUser = AuthStateProvider.GetCurrentUser();
            Logger.LogInformation("Controller mode changed by {UserName}: {OldMode} → {NewMode}", ...);
        }
    }
```

- UI badges and Simulation Panel gating rely on `SimulationModeService`, which is registered as a singleton and reads the `Controllers:UseRealControllers` appsetting once at startup; it never consults `SystemSettings`, so visual status can differ permanently from the stored mode.

```8:24:GFC.BlazorServer/Services/Simulation/SimulationModeService.cs
public class SimulationModeService : ISimulationModeService
{
    private readonly bool _useRealControllers;
    public SimulationModeService(IConfiguration configuration)
    {
        _useRealControllers = configuration.GetValue<bool>("Controllers:UseRealControllers", false);
    }
    public bool UseRealControllers => _useRealControllers;
    public bool ModeLabel => ...
}
```

```128:149:GFC.BlazorServer/Program.cs
// BEGIN Simulation/Real controller toggle wiring
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IControllerModeProvider, ControllerModeProvider>();
builder.Services.AddSingleton<GFC.BlazorServer.Services.Controllers.SimControllerStateStore>();
...
builder.Services.AddSingleton<GFC.BlazorServer.Services.Simulation.ISimulationModeService, GFC.BlazorServer.Services.Simulation.SimulationModeService>();
```

- Risk: [Medium risk incorrect behavior]

## SIM-2: IControllerClient binding & DI
- `Program.cs` registers `IControllerClient` via a scoped factory that reads `IControllerModeProvider.UseRealControllers`; it returns a scoped `SimulationControllerClient` when false, otherwise constructs a transient `RealControllerClient`. The `IMengqiControllerClient` binding follows the same pattern.

```154:210:GFC.BlazorServer/Program.cs
builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.IControllerClient>(sp =>
{
    var modeProvider = sp.GetRequiredService<IControllerModeProvider>();
    if (!modeProvider.UseRealControllers)
    {
        return sp.GetRequiredService<GFC.BlazorServer.Services.Controllers.SimulationControllerClient>();
    }
    var agent = sp.GetRequiredService<AgentApiClient>();
    var logger = sp.GetRequiredService<ILogger<GFC.BlazorServer.Services.Controllers.RealControllerClient>>();
    return new GFC.BlazorServer.Services.Controllers.RealControllerClient(agent, logger);
});
...
builder.Services.AddScoped<IMengqiControllerClient>(sp =>
{
    var modeProvider = sp.GetRequiredService<IControllerModeProvider>();
    if (!modeProvider.UseRealControllers)
    {
        return sp.GetRequiredService<SimulationControllerClient>();
    }
    return sp.GetRequiredService<RealControllerClient>();
});
```

- On any exception while resolving `IControllerModeProvider`, both factories default back to `SimulationControllerClient`, so transient database issues silently force simulation mode.

```172:209:GFC.BlazorServer/Program.cs
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Failed to determine controller mode, defaulting to simulation mode");
        return sp.GetRequiredService<GFC.BlazorServer.Services.Controllers.SimulationControllerClient>();
    }
```

- Because both `DoorConfigSyncService` and many other scoped services capture `IControllerClient` in their constructors, whichever implementation was active when the Blazor circuit was created remains in use; toggling `UseRealControllers` does not replace `_controllerClient` for existing sessions.

```14:89:GFC.BlazorServer/Services/DoorConfigSyncService.cs
public class DoorConfigSyncService
{
    private readonly IControllerClient _controllerClient;
    private readonly IControllerModeProvider _modeProvider;
    ...
    public DoorConfigSyncService(... IControllerClient controllerClient, IControllerModeProvider modeProvider, ...)
    {
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
        _modeProvider = modeProvider ?? throw new ArgumentNullException(nameof(modeProvider));
    }

    public async Task<bool> ReadFromControllerAsync(uint controllerSerialNumber, CancellationToken cancellationToken = default)
    {
        if (!_modeProvider.UseRealControllers)
        {
            _logger.LogInformation("Simulation mode: ReadFromController skipped for controller {SerialNumber}", controllerSerialNumber);
            return true;
        }
        var dto = await _controllerClient.GetDoorConfigAsync(controllerSerialNumber.ToString(), cancellationToken);
        ...
    }
```

- Risk: [Medium risk incorrect behavior]

## SIM-3: Simulation Mode influence on auth
- `MainLayout.razor` injects both `CustomAuthenticationStateProvider` and `ISimulationModeService` purely to show badges and navigation; login/logout logic never checks simulation state.

```5:210:GFC.BlazorServer/Components/Layout/MainLayout.razor
@inject CustomAuthenticationStateProvider AuthStateProvider
@inject GFC.BlazorServer.Services.Simulation.ISimulationModeService SimulationModeService
...
@if (_isAdmin || IsAdminUser())
{
    <button ...>
        @if (SimulationModeService.UseRealControllers)
        {
            <span class="badge bg-success ms-2"><i class="bi bi-hdd-network"></i> @SimulationModeService.ModeLabel</span>
        }
        else
        {
            <span class="badge bg-warning text-dark ms-2"><i class="bi bi-cpu"></i> @SimulationModeService.ModeLabel</span>
        }
    </button>
}
```

- `Settings.razor` only references `AuthStateProvider.GetCurrentUser()` to include the operator’s username in audit logs; Simulation Mode state does not bypass authentication.

```592:610:GFC.BlazorServer/Components/Pages/Settings.razor
        var oldSettings = await SystemSettingsService.GetAsync();
        ...
        if (oldValue != _useRealControllers)
        {
            var currentUser = AuthStateProvider.GetCurrentUser();
            var userName = currentUser?.Username ?? Environment.UserName ?? "System";
            Logger.LogInformation("Controller mode changed by {UserName}: {OldMode} → {NewMode}", ...);
        }
```

- Controller pages (e.g., `ControllersDashboard.razor`) inject both auth and simulation services but only to warn users when hardware calls are simulated; there is no branch that bypasses login when Simulation Mode is on.

```11:24:GFC.BlazorServer/Components/Pages/ControllersDashboard.razor
@inject CustomAuthenticationStateProvider AuthStateProvider
@inject GFC.BlazorServer.Services.Simulation.ISimulationModeService SimulationModeService
...
@if (!SimulationModeService.UseRealControllers)
{
    <span class="badge bg-warning text-dark mt-2">
        <i class="bi bi-cpu"></i> Simulation Mode – Using simulation client
    </span>
}
```

- Risk: [No issue found]

## SIM-4: Mode switching & caching
- `ControllerModeProvider` caches the boolean per scoped instance and pulls data via `_systemSettingsService.GetAsync()`; because both services (and the underlying `GfcDbContext`) are registered as scoped, each Blazor circuit tracks the first `SystemSettings` entity it loads. After toggling, existing circuits continue returning the cached `_cachedValue` until the circuit disconnects, preventing live mode changes.

```9:77:GFC.BlazorServer/Services/ControllerModeProvider.cs
public class ControllerModeProvider : IControllerModeProvider
{
    private bool? _cachedValue;
    private DateTime _cacheTime = DateTime.MinValue;
    public bool UseRealControllers
    {
        get
        {
            if (_cachedValue.HasValue && DateTime.UtcNow - _cacheTime < CacheDuration)
            {
                return _cachedValue.Value;
            }
            var settingsTask = _systemSettingsService.GetAsync();
            var completedTask = Task.WhenAny(settingsTask, Task.Delay(TimeSpan.FromSeconds(3))).GetAwaiter().GetResult();
            if (completedTask == settingsTask && settingsTask.IsCompletedSuccessfully)
            {
                var settings = settingsTask.GetAwaiter().GetResult();
                _cachedValue = settings.UseRealControllers;
                _cacheTime = DateTime.UtcNow;
                return _cachedValue.Value;
            }
            _cachedValue = false;
            return false;
        }
    }
}
```

- `Program.cs` registers both `AddDbContext`, `ISystemSettingsService`, and `IControllerModeProvider` as scoped, so every active Blazor circuit holds onto its own DbContext and cached settings; only disconnecting the circuit or recycling the server forces a reload.

```60:140:GFC.BlazorServer/Program.cs
builder.Services.AddDbContext<GfcDbContext>(options => options.UseSqlServer(efConnectionString));
...
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IControllerModeProvider, ControllerModeProvider>();
builder.Services.AddSingleton<GFC.BlazorServer.Services.Simulation.ISimulationModeService, GFC.BlazorServer.Services.Simulation.SimulationModeService>();
```

- The singleton `SimulationModeService` (config snapshot) and singleton `SimControllerStateStore` mean UI indicators and simulation state never refresh after toggling; even after flipping to real hardware, the badges continue to display “Simulation Mode,” and the in-memory controller state persists until manually cleared.

```8:24:GFC.BlazorServer/Services/Simulation/SimulationModeService.cs
public class SimulationModeService : ISimulationModeService
{
    private readonly bool _useRealControllers;
    public SimulationModeService(IConfiguration configuration)
    {
        _useRealControllers = configuration.GetValue<bool>("Controllers:UseRealControllers", false);
    }
    public bool UseRealControllers => _useRealControllers;
}
```

```10:36:GFC.BlazorServer/Services/Controllers/SimControllerStateStore.cs
public class SimControllerStateStore
{
    private readonly ConcurrentDictionary<string, SimControllerState> _controllers = new();
    public SimControllerState GetOrCreate(string controllerSn) => _controllers.GetOrAdd(controllerSn, _ => new SimControllerState { SerialNumber = controllerSn });
    public IEnumerable<string> GetAllSerialNumbers() => _controllers.Keys;
}
```

- Since scoped services such as `DoorConfigSyncService` capture `_controllerClient` and `_modeProvider` once, existing circuits continue to use the simulation implementation even after the admin flips the toggle. Users must reconnect (or the server must recycle scopes) to obtain the real client, which aligns with the CR-002 symptom.

```14:86:GFC.BlazorServer/Services/DoorConfigSyncService.cs
public class DoorConfigSyncService
{
    private readonly IControllerClient _controllerClient;
    private readonly IControllerModeProvider _modeProvider;
    ...
    if (!_modeProvider.UseRealControllers)
    {
        _logger.LogInformation("Simulation mode: ReadFromController skipped for controller {SerialNumber}", controllerSerialNumber);
        return true;
    }
    var dto = await _controllerClient.GetDoorConfigAsync(controllerSerialNumber.ToString(), cancellationToken);
```

- Risk: [Medium risk incorrect behavior]

## Issue cross-reference
| Issue | Status | Notes |
| --- | --- | --- |
| CR-001 – Simulation Mode allows login bypass | Not observed (needs more testing) | Login flow always delegates to `AuthenticationService` and never inspects simulation state; bypass setting is force-disabled (`GFC.Core/Services/AuthenticationService.cs`). |
| CR-002 – Cannot exit Simulation Mode reliably | Confirmed | Mode indicator relies on config-only `SimulationModeService`, and DI-scoped `IControllerClient` / `ControllerModeProvider` cache `UseRealControllers`, so existing circuits remain in simulation despite toggling (`Program.cs`, `ControllerModeProvider.cs`, `DoorConfigSyncService.cs`). |


## Deployment safeguards
- `DevAuth.AutoAdminBypassEnabled` is development-only; ensure it is removed or set to `false` and that the DevAuth auto-admin middleware is disabled before any production deployment.

