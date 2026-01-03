using System.Security.Cryptography;
using Google.Authenticator;
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
    private readonly IEncryptionService _encryptionService;
    private AppUser? _currentUser;

    public AuthenticationService(
        IUserRepository userRepository,
        ILoginHistoryRepository loginHistoryRepository,
        ILogger<AuthenticationService> logger,
        IAuditLogger auditLogger,
        IEncryptionService encryptionService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _loginHistoryRepository = loginHistoryRepository ?? throw new ArgumentNullException(nameof(loginHistoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
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
            if (user.MfaEnabled)
            {
                return new LoginResult
                {
                    Code = LoginResultCode.MfaRequired,
                    User = user
                };
            }

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
            _auditLogger.Log(user.UserId, AuditLogActions.LoginSuccess, $"User {user.Username} logged in successfully from {ipAddress}.");

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

    public async Task<LoginResult> VerifyMfaCodeAsync(int userId, string code, string? ipAddress = null)
    {
        var user = _userRepository.GetById(userId);

        if (user == null || !user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretKey))
        {
            return CreateFailure(LoginResultCode.Error, "MFA not enabled for user");
        }

        var tfa = new TwoFactorAuthenticator();
        var decryptedSecretKey = _encryptionService.Decrypt(user.MfaSecretKey);
        var isValid = tfa.ValidateTwoFactorPIN(decryptedSecretKey, code);

        if (!isValid)
        {
            await SafeLogLogin(user.Username, user.UserId, false, ipAddress, "Invalid MFA code");
            return CreateFailure(LoginResultCode.InvalidCredentials, "Invalid MFA code");
        }

        _currentUser = user;
        _userRepository.UpdateLastLogin(user.UserId, DateTime.UtcNow);
        await SafeLogLogin(user.Username, user.UserId, true, ipAddress, "MFA successful");

        return new LoginResult
        {
            Code = LoginResultCode.Success,
            User = user,
            PasswordChangeRequired = user.PasswordChangeRequired
        };
    }

    public MfaSetupInfo GenerateMfaSetup(AppUser user)
    {
        var tfa = new TwoFactorAuthenticator();
        var secretKeyBytes = new byte[10];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(secretKeyBytes);
        }
        var secretKey = Google.Authenticator.Base32Encoding.ToString(secretKeyBytes);
        var setupInfo = tfa.GenerateSetupCode("GFC", user.Username, secretKey, false, 3);

        return new MfaSetupInfo
        {
            SecretKey = secretKey,
            EncryptedSecretKey = _encryptionService.Encrypt(secretKey),
            QrCodeImageUrl = setupInfo.QrCodeSetupImageUrl
        };
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

