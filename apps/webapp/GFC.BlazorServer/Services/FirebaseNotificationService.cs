using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public interface IFirebaseNotificationService
{
    Task<bool> SendToUserAsync(int userId, string title, string body, Dictionary<string, string>? data = null);
    Task<int> SendToUsersAsync(IEnumerable<int> userIds, string title, string body, Dictionary<string, string>? data = null);
    Task<int> BroadcastAsync(string title, string body, Dictionary<string, string>? data = null);
    Task<bool> SendToSingleDeviceTokenAsync(string token, string title, string body, Dictionary<string, string>? data = null);
}

public class FirebaseNotificationService : IFirebaseNotificationService
{
    private readonly GfcDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FirebaseNotificationService> _logger;

    private static string? _cachedOAuthToken;
    private static DateTime _tokenExpiresAtUtc = DateTime.MinValue;
    private static readonly SemaphoreSlim _tokenLock = new(1, 1);

    public FirebaseNotificationService(
        GfcDbContext context,
        IWebHostEnvironment env,
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        ILogger<FirebaseNotificationService> logger)
    {
        _context = context;
        _env = env;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<bool> SendToUserAsync(int userId, string title, string body, Dictionary<string, string>? data = null)
    {
        var devices = await _context.UserDevices
            .Where(d => d.UserId == userId && d.IsActive && !string.IsNullOrEmpty(d.FcmDeviceToken))
            .ToListAsync();

        if (!devices.Any())
        {
            _logger.LogInformation("[FCM] No active FCM devices found for UserId={UserId}", userId);
            return false;
        }

        var tokens = devices.Select(d => d.FcmDeviceToken!).ToList();
        return await DispatchPushToTokensAsync(tokens, title, body, data);
    }

    public async Task<int> SendToUsersAsync(IEnumerable<int> userIds, string title, string body, Dictionary<string, string>? data = null)
    {
        var targetIds = userIds.Distinct().ToList();
        var devices = await _context.UserDevices
            .Where(d => targetIds.Contains(d.UserId) && d.IsActive && !string.IsNullOrEmpty(d.FcmDeviceToken))
            .ToListAsync();

        if (!devices.Any()) return 0;

        var tokens = devices.Select(d => d.FcmDeviceToken!).ToList();
        var success = await DispatchPushToTokensAsync(tokens, title, body, data);
        return success ? devices.Count : 0;
    }

    public async Task<int> BroadcastAsync(string title, string body, Dictionary<string, string>? data = null)
    {
        var devices = await _context.UserDevices
            .Where(d => d.IsActive && !string.IsNullOrEmpty(d.FcmDeviceToken))
            .ToListAsync();

        if (!devices.Any())
        {
            _logger.LogInformation("[FCM] Broadcast skipped: No active FCM devices registered in UserDevices table.");
            return 0;
        }

        var tokens = devices.Select(d => d.FcmDeviceToken!).ToList();
        var success = await DispatchPushToTokensAsync(tokens, title, body, data);
        return success ? devices.Count : 0;
    }

    public async Task<bool> SendToSingleDeviceTokenAsync(string token, string title, string body, Dictionary<string, string>? data = null)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;
        return await DispatchPushToTokensAsync(new List<string> { token }, title, body, data);
    }

    private async Task<bool> DispatchPushToTokensAsync(List<string> tokens, string title, string body, Dictionary<string, string>? data)
    {
        if (!tokens.Any()) return false;

        var category = data != null && data.TryGetValue("category", out var catVal) ? catVal : "general";
        var channelId = category switch
        {
            "rental" => "gfc_hall_rentals",
            "inquiry" => "gfc_hall_rentals",
            "rental_payment" => "gfc_hall_rentals",
            "door" => "gfc_door_access",
            "urgent" => "gfc_urgent_alerts",
            _ => "gfc_general_notifications"
        };

        // 1. Check for Google Service Account credentials (FCM HTTP v1 - Current Standard)
        var credentialsPath = _config["Firebase:CredentialsPath"] 
            ?? Path.Combine(_env.ContentRootPath, "App_Data", "firebase-adminsdk.json");

        if (!File.Exists(credentialsPath))
        {
            var fallbackPath = Path.Combine(_env.ContentRootPath, "App_Data", "service-account.json");
            if (File.Exists(fallbackPath)) credentialsPath = fallbackPath;
        }

        if (File.Exists(credentialsPath))
        {
            try
            {
                return await SendViaHttpV1Async(credentialsPath, tokens, title, body, channelId, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FCM-v1] Error dispatching push via Firebase HTTP v1 API");
            }
        }

        // 2. Fallback to Legacy Server Key (Legacy HTTP API)
        var serverKey = _config["Firebase:ServerKey"] 
            ?? Environment.GetEnvironmentVariable("FIREBASE_SERVER_KEY");

        if (!string.IsNullOrWhiteSpace(serverKey))
        {
            try
            {
                return await SendViaLegacyHttpAsync(serverKey, tokens, title, body, channelId, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FCM-Legacy] Error dispatching push via legacy HTTP API");
            }
        }

        // 3. Neither credentials available -> Log simulated broadcast mode
        _logger.LogWarning("[FCM] Push Notification NOT sent over network: No 'firebase-adminsdk.json' found in App_Data/ and no 'Firebase:ServerKey' configured. Title='{Title}', Devices={Count}, Channel='{Channel}'", title, tokens.Count, channelId);
        return false;
    }

    private async Task<bool> SendViaHttpV1Async(string credentialsPath, List<string> tokens, string title, string body, string channelId, Dictionary<string, string>? data)
    {
        var json = await File.ReadAllTextAsync(credentialsPath);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var projectId = root.GetProperty("project_id").GetString();
        var clientEmail = root.GetProperty("client_email").GetString();
        var privateKeyPem = root.GetProperty("private_key").GetString();

        if (string.IsNullOrWhiteSpace(projectId) || string.IsNullOrWhiteSpace(clientEmail) || string.IsNullOrWhiteSpace(privateKeyPem))
        {
            _logger.LogError("[FCM-v1] Invalid service account json: missing project_id, client_email, or private_key.");
            return false;
        }

        var accessToken = await GetGoogleOAuthTokenAsync(clientEmail, privateKeyPem);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            _logger.LogError("[FCM-v1] Failed to obtain Google OAuth2 access token.");
            return false;
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var endpoint = $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send";
        int successCount = 0;

        foreach (var token in tokens.Distinct())
        {
            var messagePayload = new
            {
                message = new
                {
                    token = token,
                    notification = new
                    {
                        title = title,
                        body = body
                    },
                    android = new
                    {
                        priority = "high",
                        notification = new
                        {
                            channel_id = channelId,
                            sound = "default",
                            notification_priority = "PRIORITY_HIGH",
                            default_sound = true,
                            default_vibrate_timings = true
                        }
                    },
                    data = data ?? new Dictionary<string, string>()
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(messagePayload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                successCount++;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("[FCM-v1] FCM send failure for token {TokenPrefix}...: HTTP {Code}: {Body}", 
                    token.Length > 10 ? token[..10] : token, (int)response.StatusCode, errorBody);
            }
        }

        _logger.LogInformation("[FCM-v1] Push notification dispatched to {SuccessCount}/{Total} device(s): '{Title}'", successCount, tokens.Count, title);
        return successCount > 0;
    }

    private async Task<bool> SendViaLegacyHttpAsync(string serverKey, List<string> tokens, string title, string body, string channelId, Dictionary<string, string>? data)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");

        var fcmPayload = new
        {
            registration_ids = tokens,
            priority = "high",
            notification = new
            {
                title = title,
                body = body,
                sound = "default",
                android_channel_id = channelId
            },
            data = data ?? new Dictionary<string, string>()
        };

        var content = new StringContent(JsonSerializer.Serialize(fcmPayload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://fcm.googleapis.com/fcm/send", content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("[FCM-Legacy] Delivered push notification to {Count} device(s): '{Title}'", tokens.Count, title);
            return true;
        }

        var err = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("[FCM-Legacy] FCM HTTP Gateway returned {StatusCode}: {Error}", response.StatusCode, err);
        return false;
    }

    private async Task<string?> GetGoogleOAuthTokenAsync(string clientEmail, string privateKeyPem)
    {
        if (!string.IsNullOrEmpty(_cachedOAuthToken) && DateTime.UtcNow < _tokenExpiresAtUtc.AddMinutes(-5))
        {
            return _cachedOAuthToken;
        }

        await _tokenLock.WaitAsync();
        try
        {
            if (!string.IsNullOrEmpty(_cachedOAuthToken) && DateTime.UtcNow < _tokenExpiresAtUtc.AddMinutes(-5))
            {
                return _cachedOAuthToken;
            }

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var jwtHeader = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(new { alg = "RS256", typ = "JWT" }));
            var jwtClaimSet = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(new
            {
                iss = clientEmail,
                scope = "https://www.googleapis.com/auth/firebase.messaging",
                aud = "https://oauth2.googleapis.com/token",
                exp = now + 3600,
                iat = now
            }));

            var unsignedJwt = $"{jwtHeader}.{jwtClaimSet}";
            var unsignedBytes = Encoding.UTF8.GetBytes(unsignedJwt);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);
            var signatureBytes = rsa.SignData(unsignedBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var signedJwt = $"{unsignedJwt}.{Base64UrlEncode(signatureBytes)}";

            var client = _httpClientFactory.CreateClient();
            var requestParams = new Dictionary<string, string>
            {
                { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { "assertion", signedJwt }
            };

            var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(requestParams));
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                _logger.LogError("[FCM-Auth] Google OAuth2 token exchange error: HTTP {Code}: {Err}", (int)response.StatusCode, err);
                return null;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var resDoc = JsonDocument.Parse(responseJson);
            if (resDoc.RootElement.TryGetProperty("access_token", out var tokenProp))
            {
                _cachedOAuthToken = tokenProp.GetString();
                int expiresIn = resDoc.RootElement.TryGetProperty("expires_in", out var expProp) ? expProp.GetInt32() : 3600;
                _tokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(expiresIn);
                return _cachedOAuthToken;
            }

            return null;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}

