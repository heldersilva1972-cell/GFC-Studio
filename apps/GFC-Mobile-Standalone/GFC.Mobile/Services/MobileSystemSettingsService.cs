using GFC.Core.Interfaces;
using GFC.Core.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GFC.Mobile.Services
{
    public class MobileSystemSettingsService : ISystemSettingsService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string CacheKey = "gfc_system_settings";
        private SystemSettings? _cachedSettings;

        public MobileSystemSettingsService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<SystemSettings> GetAsync()
        {
            if (_cachedSettings != null)
            {
                return _cachedSettings;
            }

            // 1. Try local cache
            try
            {
                var cachedJson = await _js.InvokeAsync<string>("localStorage.getItem", CacheKey);
                if (!string.IsNullOrEmpty(cachedJson))
                {
                    _cachedSettings = JsonSerializer.Deserialize<SystemSettings>(cachedJson);
                }
            }
            catch { }

            // 2. If nothing cached, try API, otherwise return new default
            if (_cachedSettings == null)
            {
                try
                {
                    using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(5));
                    var settings = await _http.GetFromJsonAsync<SystemSettings>("api/mobile-reporting/settings", cts.Token);
                    if (settings != null)
                    {
                        _cachedSettings = settings;
                        await SaveToCacheAsync(settings);
                    }
                }
                catch
                {
                    // Fallback to default
                    _cachedSettings = new SystemSettings();
                }
            }
            else
            {
                // Background refresh
                _ = RefreshSettingsBackgroundAsync();
            }

            return _cachedSettings ?? new SystemSettings();
        }

        private async Task RefreshSettingsBackgroundAsync()
        {
            try
            {
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(5));
                var settings = await _http.GetFromJsonAsync<SystemSettings>("api/mobile-reporting/settings", cts.Token);
                if (settings != null)
                {
                    _cachedSettings = settings;
                    await SaveToCacheAsync(settings);
                }
            }
            catch { }
        }

        private async Task SaveToCacheAsync(SystemSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings);
                await _js.InvokeVoidAsync("localStorage.setItem", CacheKey, json);
            }
            catch { }
        }

        public Task<SystemSettings> GetSettingsAsync() => GetAsync();

        public async Task UpdateAsync(SystemSettings settings)
        {
            _cachedSettings = settings;
            await SaveToCacheAsync(settings);

            try
            {
                await _http.PostAsJsonAsync("api/mobile-reporting/settings", settings);
            }
            catch
            {
                // Standalone/Offline sync outbox could be handled here or by MobileReportingService
            }
        }

        public Task SaveSettingsAsync(SystemSettings settings) => UpdateAsync(settings);

        public async Task<int> GetTrustedDeviceDurationDaysAsync() => (await GetAsync()).TrustedDeviceDurationDays;
        public async Task<bool> GetSafeModeEnabledAsync() => (await GetAsync()).SafeModeEnabled;
        public async Task<bool> GetEnableTwoFactorAuthAsync() => (await GetAsync()).EnableTwoFactorAuth;
        public async Task<string?> GetTwilioAccountSidAsync() => (await GetAsync()).TwilioAccountSid;
        public async Task<string?> GetTwilioAuthTokenAsync() => (await GetAsync()).TwilioAuthToken;
        public async Task<string?> GetTwilioFromNumberAsync() => (await GetAsync()).TwilioFromNumber;
        public async Task<string> GetPreferredMfaMethodAsync() => "SMS"; // Default / fallback
        public async Task<bool> GetSmsEnabledAsync() => (await GetAsync()).SmsEnabled;
        public async Task<bool> GetEmailEnabledAsync() => (await GetAsync()).EmailEnabled;
    }
}
