using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginHistoryRepository _loginHistoryRepository;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IAuditLogger _auditLogger;
    private AppUser? _currentUser;

    public AuthenticationService(
        IUserRepository userRepository,
        ILoginHistoryRepository loginHistoryRepository,
        ILogger<AuthenticationService> logger,
        IAuditLogger auditLogger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _loginHistoryRepository = loginHistoryRepository ?? throw new ArgumentNullException(nameof(loginHistoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
    }

    public async Task<LoginResult> LoginAsync(string username, string password, string? ipAddress = null)
    {
        username = username?.Trim() ?? string.Empty;
        _currentUser = null;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            const string reason = "Missing username or password";
            await SafeLogLogin(username, null, false, ipAddress, reason);
            _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason);
            return CreateFailure(LoginResultCode.InvalidCredentials, reason);
        }

        AppUser? user = null;

        try
        {
            // Get user from repository
            user = _userRepository.GetByUsername(username);

            if (user == null)
            {
                const string reason = "User not found";
                await SafeLogLogin(username, null, false, ipAddress, reason);
                _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason);
                return CreateFailure(LoginResultCode.InvalidCredentials, reason);
            }

            // Check if user is active
            if (!user.IsActive)
            {
                const string reason = "Inactive user";
                await SafeLogLogin(username, user.UserId, false, ipAddress, reason);
                _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason, user.UserId);
                return CreateFailure(LoginResultCode.AccountLockedOrDisabled, reason);
            }

            // PASSWORD CHECK – use existing hashing helper
            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                const string reason = "Invalid password";
                await SafeLogLogin(username, user.UserId, false, ipAddress, reason);
                _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason, user.UserId);
                return CreateFailure(LoginResultCode.InvalidCredentials, reason);
            }

            // SUCCESS
            _currentUser = user;
            
            // Update last login
            try
            {
                _userRepository.UpdateLastLogin(user.UserId, DateTime.UtcNow);
            }
            catch (Exception updateEx)
            {
                // Ignore errors updating last login, but trace for diagnostics
                _logger.LogDebug(updateEx, "Unable to update last login for {UserId}", user.UserId);
            }

            await SafeLogLogin(username, user.UserId, true, ipAddress, null);

            return new LoginResult
            {
                Code = LoginResultCode.Success,
                User = user,
                PasswordChangeRequired = user.PasswordChangeRequired
            };
        }
        catch (Exception ex)
        {
            // On any unexpected error, do NOT authenticate the user
            await SafeLogLogin(username, user?.UserId, false, ipAddress, "Auth exception: " + ex.Message);
            _logger.LogError(ex, "Unexpected error during login for {Username}", username);
            _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, "Auth exception: " + ex.Message, user?.UserId);
            return CreateFailure(LoginResultCode.Error, "Auth exception: " + ex.Message);
        }
    }

    // Helper method to safely log login attempts without breaking the login flow
    private async Task SafeLogLogin(string username, int? userId, bool success, string? ipAddress, string? failureReason)
    {
        try
        {
            _loginHistoryRepository.LogLogin(new LoginHistory
            {
                UserId = userId,
                Username = username,
                LoginDate = DateTime.UtcNow,
                IpAddress = ipAddress,
                LoginSuccessful = success,
                FailureReason = failureReason
            });
        }
        catch
        {
            // Never let login history failures break login flow
        }
        await Task.CompletedTask;
    }

    public void Logout()
    {
        _currentUser = null;
    }

    public AppUser? GetCurrentUser()
    {
        return _currentUser;
    }

    private static LoginResult CreateFailure(LoginResultCode code, string? reason)
    {
        return new LoginResult
        {
            Code = code,
            ErrorMessageForLog = reason
        };
    }

}

