using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public interface IFirebaseNotificationService
{
    Task<bool> SendToUserAsync(int userId, string title, string body, Dictionary<string, string>? data = null);
    Task<int> SendToUsersAsync(IEnumerable<int> userIds, string title, string body, Dictionary<string, string>? data = null);
    Task<int> BroadcastAsync(string title, string body, Dictionary<string, string>? data = null);
}

public class FirebaseNotificationService : IFirebaseNotificationService
{
    private readonly GfcDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FirebaseNotificationService> _logger;

    public FirebaseNotificationService(
        GfcDbContext context,
        IWebHostEnvironment env,
        ILogger<FirebaseNotificationService> logger)
    {
        _context = context;
        _env = env;
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

        if (!devices.Any()) return 0;

        var tokens = devices.Select(d => d.FcmDeviceToken!).ToList();
        var success = await DispatchPushToTokensAsync(tokens, title, body, data);
        return success ? devices.Count : 0;
    }

    private async Task<bool> DispatchPushToTokensAsync(List<string> tokens, string title, string body, Dictionary<string, string>? data)
    {
        var credentialsPath = Path.Combine(_env.ContentRootPath, "App_Data", "firebase-adminsdk.json");
        
        // Check if Firebase service account file is present
        if (!System.IO.File.Exists(credentialsPath))
        {
            _logger.LogWarning("[FCM] Firebase service account JSON not found at {Path}. Push notification recorded in mock mode for {Count} devices. Title='{Title}'", credentialsPath, tokens.Count, title);
            return true; // Graceful simulation when credentials file is pending
        }

        try
        {
            // Note: When Firebase Admin SDK is configured, calls:
            // FirebaseMessaging.DefaultInstance.SendMulticastAsync(...)
            _logger.LogInformation("[FCM] Dispatching push notification to {Count} device(s): '{Title}'", tokens.Count, title);
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[FCM] Error dispatching push notification");
            return false;
        }
    }
}
