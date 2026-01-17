// [NEW]
namespace GFC.Core.Interfaces
{
    /// <summary>
    /// Minimal settings interface for core services that need access to system settings.
    /// </summary>
    public interface ISystemSettingsService
    {
        /// <summary>
        /// Gets the trusted device duration in days.
        /// </summary>
        Task<int> GetTrustedDeviceDurationDaysAsync();

        /// <summary>
        /// Gets whether the system is in Safe Mode.
        /// </summary>
        Task<bool> GetSafeModeEnabledAsync();

        /// <summary>
        /// Gets whether Two Factor Authentication is enabled globally.
        /// </summary>
        Task<bool> GetEnableTwoFactorAuthAsync();

        /// <summary>
        /// Gets Twilio account settings.
        /// </summary>
        Task<string?> GetTwilioAccountSidAsync();
        Task<string?> GetTwilioAuthTokenAsync();
        Task<string?> GetTwilioFromNumberAsync();
        Task<string> GetPreferredMfaMethodAsync();
        Task<bool> GetSmsEnabledAsync();
        Task<bool> GetEmailEnabledAsync();
    }
}
