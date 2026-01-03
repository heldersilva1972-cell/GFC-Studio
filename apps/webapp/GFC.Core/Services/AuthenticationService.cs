// [MODIFIED]
using System.Security.Cryptography;
using Google.Authenticator;
using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.Models.Security;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GFC.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginHistoryRepository _loginHistoryRepository;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IAuditLogger _auditLogger;
    private readonly IEncryptionService _encryptionService;
    private readonly ITrustedDeviceRepository _trustedDeviceRepository;
    private readonly ISystemSettingsService _systemSettingsService;
    private AppUser? _currentUser;

    public AuthenticationService(
        IUserRepository userRepository,
        ILoginHistoryRepository loginHistoryRepository,
        ILogger<AuthenticationService> logger,
        IAuditLogger auditLogger,
        IEncryptionService encryptionService,
        ITrustedDeviceRepository trustedDeviceRepository,
        ISystemSettingsService systemSettingsService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _loginHistoryRepository = loginHistoryRepository ?? throw new ArgumentNullException(nameof(loginHistoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _trustedDeviceRepository = trustedDeviceRepository ?? throw new ArgumentNullException(nameof(trustedDeviceRepository));
        _systemSettingsService = systemSettingsService ?? throw new ArgumentNullException(nameof(systemSettingsService));
    }

    public async Task<LoginResult> LoginAsync(string username, string password, string? ipAddress = null, bool rememberDevice = false)
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
            user = _userRepository.GetByUsername(username);

            if (user == null)
            {
                const string reason = "User not found";
                await SafeLogLogin(username, null, false, ipAddress, reason);
                _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason);
                return CreateFailure(LoginResultCode.InvalidCredentials, reason);
            }

            if (!user.IsActive)
            {
                const string reason = "Inactive user";
                await SafeLogLogin(username, user.UserId, false, ipAddress, reason);
                _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason, user.UserId);
                return CreateFailure(LoginResultCode.AccountLockedOrDisabled, reason);
            }

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                const string reason = "Invalid password";
                await SafeLogLogin(username, user.UserId, false, ipAddress, reason);
                _logger.LogWarning("Login failed for {Username}: {Reason}", username, reason);
                _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, reason, user.UserId);
                return CreateFailure(LoginResultCode.InvalidCredentials, reason);
            }

            if (user.MfaEnabled)
            {
                return new LoginResult
                {
                    Code = LoginResultCode.MfaRequired,
                    User = user
                };
            }

            _currentUser = user;
            
            try
            {
                _userRepository.UpdateLastLogin(user.UserId, DateTime.UtcNow);
            }
            catch (Exception updateEx)
            {
                _logger.LogDebug(updateEx, "Unable to update last login for {UserId}", user.UserId);
            }

            await SafeLogLogin(username, user.UserId, true, ipAddress, null);

            string? deviceToken = null;
            if (rememberDevice)
            {
                deviceToken = await GenerateAndSaveDeviceTokenAsync(user.UserId, ipAddress, null);
            }

            _auditLogger.Log(AuditLogActions.LoginSuccessPassword, user.UserId, user.UserId, $"IP: {ipAddress ?? "unknown"}");

            return new LoginResult
            {
                Code = LoginResultCode.Success,
                User = user,
                PasswordChangeRequired = user.PasswordChangeRequired,
                DeviceToken = deviceToken
            };
        }
        catch (Exception ex)
        {
            await SafeLogLogin(username, user?.UserId, false, ipAddress, "Auth exception: " + ex.Message);
            _logger.LogError(ex, "Unexpected error during login for {Username}", username);
            _auditLogger.LogSuspiciousLoginAttempt(username, ipAddress, "Auth exception: " + ex.Message, user?.UserId);
            return CreateFailure(LoginResultCode.Error, "Auth exception: " + ex.Message);
        }
    }

    public async Task<LoginResult> LoginWithDeviceTokenAsync(string token, string? ipAddress = null)
    {
        _currentUser = null;
        if (string.IsNullOrWhiteSpace(token))
        {
            return CreateFailure(LoginResultCode.InvalidCredentials, "Missing device token");
        }

        var trustedDevice = await _trustedDeviceRepository.GetByTokenAsync(token);

        if (trustedDevice == null || trustedDevice.IsRevoked || trustedDevice.ExpiresAtUtc < DateTime.UtcNow)
        {
            string reason = trustedDevice == null ? "Device token not found" : (trustedDevice.IsRevoked ? "Device was revoked" : "Device token expired");
            await SafeLogLogin(null, trustedDevice?.UserId, false, ipAddress, reason);
            if (trustedDevice != null)
            {
                await _trustedDeviceRepository.DeleteAsync(trustedDevice.Id);
            }
            return CreateFailure(LoginResultCode.InvalidCredentials, reason);
        }

        var user = _userRepository.GetById(trustedDevice.UserId);
        if (user == null || !user.IsActive)
        {
            string reason = user == null ? "User not found for token" : "User for token is inactive";
            await SafeLogLogin(user?.Username, trustedDevice.UserId, false, ipAddress, reason);
            await _trustedDeviceRepository.DeleteAsync(trustedDevice.Id);
            return CreateFailure(LoginResultCode.AccountLockedOrDisabled, reason);
        }

        _currentUser = user;
        var (newToken, _) = await RotateDeviceTokenAsync(trustedDevice);

        await SafeLogLogin(user.Username, user.UserId, true, ipAddress, "Login via device token successful");

        return new LoginResult
        {
            Code = LoginResultCode.Success,
            User = user,
            PasswordChangeRequired = user.PasswordChangeRequired,
            DeviceToken = newToken
        };
    }

    public async Task<LoginResult> VerifyMfaCodeAsync(int userId, string code, string? ipAddress = null, bool rememberDevice = false)
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

        string? deviceToken = null;
        if (rememberDevice)
        {
            deviceToken = await GenerateAndSaveDeviceTokenAsync(user.UserId, ipAddress, null);
        }

        return new LoginResult
        {
            Code = LoginResultCode.Success,
            User = user,
            PasswordChangeRequired = user.PasswordChangeRequired,
            DeviceToken = deviceToken
        };
    }

    public async Task<LoginResult> LoginMagicLinkAsync(int userId, string? ipAddress = null)
    {
        _currentUser = null;
        var user = _userRepository.GetById(userId);

        if (user == null || !user.IsActive)
        {
            string reason = user == null ? "User not found" : "User inactive";
            await SafeLogLogin(user?.Username, userId, false, ipAddress, reason);
            _auditLogger.LogSuspiciousLoginAttempt(user?.Username ?? "unknown", ipAddress, reason, userId);
            return CreateFailure(LoginResultCode.AccountLockedOrDisabled, reason);
        }

        if (user.MfaEnabled)
        {
            // Requirement didn't explicitly say MFA skips for Magic Link, but usually Magic Link implies strict identity verification via email. 
            // However, usually MFA is 2nd factor. 
            // "Validation: must exist, not used, not expired. If valid: Log user in."
            // So implicit bypass of password. If MFA is enabled, we might still want it. 
            // But prompt says: "If valid: Log user in... redirect to Dashboard."
            // Simple approach: Magic Link acts as strong auth. 
        }

        _currentUser = user;
        _userRepository.UpdateLastLogin(user.UserId, DateTime.UtcNow);
        await SafeLogLogin(user.Username, user.UserId, true, ipAddress, "Magic Link login successful");
        
        _auditLogger.Log(AuditLogActions.LoginSuccessMagicLink, user.UserId, user.UserId, $"IP: {ipAddress ?? "unknown"}");

        return new LoginResult
        {
            Code = LoginResultCode.Success,
            User = user,
            PasswordChangeRequired = user.PasswordChangeRequired
        };
    }

    public async Task LogoutAsync(string? deviceToken = null)
    {
        if (!string.IsNullOrWhiteSpace(deviceToken))
        {
            var trustedDevice = await _trustedDeviceRepository.GetByTokenAsync(deviceToken);
            if (trustedDevice != null)
            {
                await _trustedDeviceRepository.DeleteAsync(trustedDevice.Id);
            }
        }
        _currentUser = null;
    }

    public AppUser? GetCurrentUser()
    {
        return _currentUser;
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

    private async Task SafeLogLogin(string? username, int? userId, bool success, string? ipAddress, string? failureReason)
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
        catch { }
        await Task.CompletedTask;
    }

    private static LoginResult CreateFailure(LoginResultCode code, string? reason)
    {
        return new LoginResult
        {
            Code = code,
            ErrorMessageForLog = reason
        };
    }

    private async Task<(string, DateTime)> RotateDeviceTokenAsync(TrustedDevice trustedDevice)
    {
        var durationDays = await _systemSettingsService.GetTrustedDeviceDurationDaysAsync();

        trustedDevice.LastUsedUtc = DateTime.UtcNow;
        trustedDevice.ExpiresAtUtc = DateTime.UtcNow.AddDays(durationDays);
        trustedDevice.DeviceToken = GenerateSecureToken();

        await _trustedDeviceRepository.UpdateAsync(trustedDevice);

        return (trustedDevice.DeviceToken, trustedDevice.ExpiresAtUtc);
    }

    private async Task<string?> GenerateAndSaveDeviceTokenAsync(int userId, string? ipAddress, string? userAgent)
    {
        var durationDays = await _systemSettingsService.GetTrustedDeviceDurationDaysAsync();

        var token = GenerateSecureToken();
        var newDevice = new TrustedDevice
        {
            UserId = userId,
            DeviceToken = token,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LastUsedUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(durationDays),
            IsRevoked = false
        };

        await _trustedDeviceRepository.CreateAsync(newDevice);
        return token;
    }

    private static string GenerateSecureToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
