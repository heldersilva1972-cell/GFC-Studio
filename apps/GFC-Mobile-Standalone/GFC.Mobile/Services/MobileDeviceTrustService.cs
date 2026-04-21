using GFC.Core.Interfaces;
using GFC.Core.Models.Security;
using GFC.Core.DTOs;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace GFC.Mobile.Services;

public class MobileDeviceTrustService : IDeviceTrustService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _jsRuntime;

    public MobileDeviceTrustService(HttpClient http, IJSRuntime jsRuntime)
    {
        _http = http;
        _jsRuntime = jsRuntime;
    }

    public async Task<TrustedDevice?> GetDeviceByTokenAsync(string token)
    {
        try {
            return await _http.GetFromJsonAsync<TrustedDevice>($"/api/mobile-reporting/device?token={token}");
        } catch { return null; }
    }

    public async Task<int?> ValidateStationAutoLoginAsync(string stationToken, string username)
    {
        try {
            var response = await _http.GetAsync($"/api/mobile-reporting/device/auto-login?token={stationToken}&username={username}");
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                return int.Parse(text);
            }
            return null;
        } catch { return null; }
    }

    // STUBS FOR INTERFACE COMPLIANCE (Not used via WASM UI directly)
    public Task<bool> ValidateDeviceTokenAsync(string token, int userId) => throw new NotImplementedException();
    public Task<string> CreateDeviceTokenAsync(int userId, string userAgent, string ipAddress, int durationDays) => throw new NotImplementedException();
    public Task<string> CreateStationTokenAsync(int authorizedByUserId, string userAgent, string ipAddress, int durationDays, string? stationName = null, string? loginMode = null, string? authorizedUserIdsCsv = null) => throw new NotImplementedException();
    public Task<List<TrustedDevice>> GetAllActiveStationsAsync() => throw new NotImplementedException();
    public Task<bool> IsStationTokenAsync(string token) => throw new NotImplementedException();
    public Task<bool> RevokeDeviceTokenAsync(string token) => throw new NotImplementedException();
    public Task RevokeAllUserDevicesAsync(int userId) => throw new NotImplementedException();
    public Task CleanupExpiredTokensAsync() => throw new NotImplementedException();
    public Task RevokeAllGlobalSessionsAsync() => throw new NotImplementedException();
    public bool ValidateToken(string token) => throw new NotImplementedException();
    public Task<bool> ValidateTokenAsync(string token) => throw new NotImplementedException();
    public Task<int?> GetUserIdByTokenAsync(string token) => throw new NotImplementedException();
    public Task<List<TrustedDevice>> GetDevicesForUserAsync(int userId) => throw new NotImplementedException();
    public Task<List<DeviceSessionDto>> GetAllActiveDevicesAsync() => throw new NotImplementedException();
    public Task ResetMobileSetupAsync(int userId) => throw new NotImplementedException();
    public void InvalidateUserSession(int userId) => throw new NotImplementedException();
    public void InvalidateTokenSession(string token) => throw new NotImplementedException();
    public void InvalidateAllUserSessions() => throw new NotImplementedException();
    public Task UpdateDeviceAsync(TrustedDevice device) => throw new NotImplementedException();
    public Task<string?> GenerateSetupCodeAsync(string deviceToken) => throw new NotImplementedException();
    public Task<string?> ValidateSetupCodeAsync(string code) => throw new NotImplementedException();
}
