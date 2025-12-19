using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Models;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Services;

public sealed class AgentApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;
    private readonly ILogger<AgentApiClient> _logger;
    private readonly IOptions<AgentApiOptions> _options;

    public AgentApiClient(HttpClient httpClient, ILogger<AgentApiClient> logger, IOptions<AgentApiOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options;
    }

    public async Task<AgentRunStatusDto?> GetRunStatusAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/run-status");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent run-status request failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<AgentRunStatusDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<bool> OpenDoorAsync(uint controllerSn, int doorNo, int? durationSeconds, CancellationToken cancellationToken = default)
    {
        var payload = new OpenDoorRequestDto { DurationSec = durationSeconds };
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/door/{doorNo}/open");
        request.Content = JsonContent.Create(payload);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Agent open door failed ({Status}): {Body}", response.StatusCode, body);
            return false;
        }

        return true;
    }

    public async Task SyncTimeAsync(uint controllerSn, DateTime serverTimeUtc, CancellationToken cancellationToken = default)
    {
        var payload = new SyncTimeRequestDto { ServerTimeUtc = serverTimeUtc };
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/sync-time");
        request.Content = JsonContent.Create(payload, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Agent sync time failed ({Status}): {Body}", response.StatusCode, body);
            throw new InvalidOperationException($"Agent returned {response.StatusCode}: {body}");
        }
    }

    public async Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var request = CreateRequest(HttpMethod.Get, "/api/health");
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode) return true;
            
            request = CreateRequest(HttpMethod.Get, "/");
            response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<TimeScheduleDto?> GetTimeSchedulesAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/time-schedules");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get time schedules failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<TimeScheduleDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> SyncTimeSchedulesAsync(uint controllerSn, TimeScheduleWriteDto schedule, CancellationToken cancellationToken = default)
    {
        var payload = new TimeScheduleSyncRequest { Schedule = schedule };
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/time-schedules/sync");
        request.Content = JsonContent.Create(payload, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent sync time schedules failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Sync completed"
        };
    }

    public async Task<ExtendedConfigDto?> GetDoorConfigAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/door-config");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get door config failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<ExtendedConfigDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> SyncDoorConfigAsync(uint controllerSn, ExtendedDoorConfigWriteDto config, CancellationToken cancellationToken = default)
    {
        var payload = new DoorConfigSyncRequest { Config = config };
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/door-config/sync");
        request.Content = JsonContent.Create(payload, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent sync door config failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Sync completed"
        };
    }

    public async Task<AutoOpenConfigDto?> GetAutoOpenAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/auto-open");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get auto-open failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<AutoOpenConfigDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> SyncAutoOpenAsync(uint controllerSn, AutoOpenConfigDto config, CancellationToken cancellationToken = default)
    {
        var payload = new AutoOpenSyncRequest { Config = config };
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/auto-open/sync");
        request.Content = JsonContent.Create(payload, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent sync auto-open failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Auto-open sync completed"
        };
    }

    public async Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/advanced-door-modes");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get advanced door modes failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<AdvancedDoorModesDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> SyncAdvancedDoorModesAsync(uint controllerSn, AdvancedDoorModesDto config, CancellationToken cancellationToken = default)
    {
        var payload = new AdvancedDoorModesSyncRequest { Config = config };
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/advanced-door-modes/sync");
        request.Content = JsonContent.Create(payload, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent sync advanced door modes failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Advanced door modes sync completed"
        };
    }

    public async Task<ApiResult> AddOrUpdateCardAsync(uint controllerSn, AddOrUpdateCardRequestDto card, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/cards");
        request.Content = JsonContent.Create(card, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent add/update card failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Card added/updated successfully"
        };
    }

    public async Task<ControllerEventsResultDto?> GetEventsAsync(uint controllerSn, uint lastIndex, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/events?lastIndex={lastIndex}");
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get events failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<ControllerEventsResultDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> DeleteCardAsync(uint controllerSn, string cardNumber, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Delete, $"/api/controllers/{controllerSn}/cards/{Uri.EscapeDataString(cardNumber)}");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent delete card failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Card deleted successfully"
        };
    }

    public async Task<ApiResult> ClearAllCardsAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/cards/clear-all");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent clear all cards failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "All cards cleared successfully"
        };
    }

    public async Task<NetworkConfigDto?> GetNetworkConfigAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/network-config");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get network config failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<NetworkConfigDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> SetNetworkConfigAsync(uint controllerSn, NetworkConfigRequestDto config, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/network-config");
        request.Content = JsonContent.Create(config, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent set network config failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Network config updated"
        };
    }

    public async Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcAndPasswordAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, $"/api/controllers/{controllerSn}/allowed-pc");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent get allowed PC and password failed: {Status}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<AllowedPcAndPasswordRequestDto>>(JsonOptions, cancellationToken)
            .ContinueWith(t => t.Result?.Data, cancellationToken);
    }

    public async Task<ApiResult> SetAllowedPcAndCommPasswordAsync(uint controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, $"/api/controllers/{controllerSn}/allowed-pc");
        request.Content = JsonContent.Create(dto, options: JsonOptions);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Agent set allowed PC and password failed ({Status}): {Body}", response.StatusCode, body);
            return new ApiResult { Success = false, Message = $"Agent returned {response.StatusCode}: {body}" };
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(body, JsonOptions);
        return new ApiResult
        {
            Success = apiResponse?.Success ?? true,
            Message = apiResponse?.Message ?? "Allowed PC and password updated"
        };
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string relativePath)
    {
        var options = _options.Value;
        if (!string.IsNullOrWhiteSpace(options.BaseUrl) && _httpClient.BaseAddress?.AbsoluteUri != options.BaseUrl)
        {
            _httpClient.BaseAddress = new Uri(options.BaseUrl);
        }

        var request = new HttpRequestMessage(method, relativePath);
        if (!string.IsNullOrWhiteSpace(options.ApiKey))
        {
            request.Headers.TryAddWithoutValidation("X-Agent-Key", options.ApiKey);
        }
        return request;
    }

    // Matches Gfc.Agent.Api.Models.ApiResponse<T> structure
    private sealed class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}

