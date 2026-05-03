// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPush;
using System.Text.Json;

namespace GFC.BlazorServer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly GFC.Core.Interfaces.IEmailService _emailService;
        private bool _masterKillSwitchEnabled = false;

        public NotificationService(IDbContextFactory<GfcDbContext> dbFactory, GFC.Core.Interfaces.IEmailService emailService)
        {
            _dbFactory = dbFactory;
            _emailService = emailService;
        }

        public async Task DispatchNotificationAsync(SystemNotification notification)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var settings = await db.SystemSettings.FirstOrDefaultAsync();
            bool isBlocked = _masterKillSwitchEnabled;

            if (notification.Channel == "Email" && settings?.EmailEnabled == false) isBlocked = true;
            if (notification.Channel == "SMS" && settings?.SmsEnabled == false) isBlocked = true;
            if (notification.Channel == "Push" && settings?.PushEnabled == false) isBlocked = true;

            if (isBlocked)
            {
                notification.Status = "Blocked by Policy";
            }
            else
            {
                notification.Status = "Sent";
            }

            db.SystemNotifications.Add(notification);
            await db.SaveChangesAsync();
        }

        public void EnabbleMasterKillSwitch()
        {
            _masterKillSwitchEnabled = true;
        }

        public void DisableMasterKillSwitch()
        {
            _masterKillSwitchEnabled = false;
        }

        public bool IsMasterKillSwitchEnabled()
        {
            return _masterKillSwitchEnabled;
        }

        public async Task NotifySystemAlertAsync(string title, string body, string? url = null)
        {
            if (_masterKillSwitchEnabled) return;

            using var db = await _dbFactory.CreateDbContextAsync();
            var usersToNotify = await db.UserNotificationPreferences
                .Where(p => p.SystemAlertNotifyEmail || p.SystemAlertNotifyPush)
                .ToListAsync();

            foreach (var pref in usersToNotify)
            {
                var user = await db.AppUsers.FindAsync(pref.UserId);
                if (user == null) continue;

                if (pref.SystemAlertNotifyPush)
                {
                    await SendPushNotificationAsync(user.UserId, title, body, url);
                }

                if (pref.SystemAlertNotifyEmail && !string.IsNullOrEmpty(user.Email))
                {
                    await SendEmailAsync(user.Email, title, body);
                }
            }
        }

        public async Task SendRentalConfirmationEmailAsync(HallRentalRequest request)
        {
            if (_masterKillSwitchEnabled) return;

            var notification = new SystemNotification
            {
                RecipientEmail = request.RequesterEmail,
                Subject = "Hall Rental Request Approved",
                Message = $"Your rental request for {request.EventDate:MMMM dd, yyyy} has been approved.",
                Channel = "Email",
                Status = "Sent",
                SentAt = DateTime.UtcNow
            };

            using (var db = await _dbFactory.CreateDbContextAsync())
            {
                db.SystemNotifications.Add(notification);
                await db.SaveChangesAsync();
            }

            await SendEmailAsync(request.RequesterEmail, notification.Subject, notification.Message);
        }

        public async Task SendRentalDenialEmailAsync(HallRentalRequest request, string reason)
        {
            if (_masterKillSwitchEnabled) return;

            var notification = new SystemNotification
            {
                RecipientEmail = request.RequesterEmail,
                Subject = "Hall Rental Request Denied",
                Message = $"Your rental request for {request.EventDate:MMMM dd, yyyy} has been denied. Reason: {reason}",
                Channel = "Email",
                Status = "Sent",
                SentAt = DateTime.UtcNow
            };

            using (var db = await _dbFactory.CreateDbContextAsync())
            {
                db.SystemNotifications.Add(notification);
                await db.SaveChangesAsync();
            }

            await SendEmailAsync(request.RequesterEmail, notification.Subject, notification.Message);
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            if (_masterKillSwitchEnabled) return;

            var notification = new SystemNotification
            {
                RecipientEmail = email,
                Subject = subject,
                Message = body,
                Channel = "Email",
                Status = "Pending",
                SentAt = DateTime.UtcNow
            };

            using (var db = await _dbFactory.CreateDbContextAsync())
            {
                db.SystemNotifications.Add(notification);
                await db.SaveChangesAsync();

                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                    notification.Status = "Sent";
                    db.Entry(notification).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    notification.Status = "Failed";
                    db.Entry(notification).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    throw;
                }
            }
        }

        public async Task<List<SystemNotification>> GetActiveNotificationsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.SystemNotifications
                                 .Where(n => n.Status == "Sent")
                                 .ToListAsync();
        }

        public async Task<int> GetPushSubscriptionCountAsync(int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.PushSubscriptions
                .CountAsync(s => s.UserId == userId);
        }

        public async Task<string?> GetVapidPublicKeyAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var settings = await db.SystemSettings.FirstOrDefaultAsync();
            return settings?.VapidPublicKey;
        }

        public async Task SubscribeToPushAsync(int userId, string endpoint, string p256dh, string auth, string? deviceName)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.PushSubscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == endpoint);

            if (existing == null)
            {
                db.PushSubscriptions.Add(new GFC.BlazorServer.Data.Entities.PushSubscription
                {
                    UserId = userId,
                    Endpoint = endpoint,
                    P256dh = p256dh,
                    Auth = auth,
                    DeviceName = deviceName,
                    CreatedAtUtc = DateTime.UtcNow
                });
                await db.SaveChangesAsync();
            }
        }

        public async Task UnsubscribeFromPushAsync(int userId, string endpoint)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.PushSubscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == endpoint);

            if (existing != null)
            {
                db.PushSubscriptions.Remove(existing);
                await db.SaveChangesAsync();
            }
        }

        public async Task SendPushNotificationAsync(int userId, string title, string body, string? url = null)
        {
            if (_masterKillSwitchEnabled) throw new InvalidOperationException("Notifications are currently disabled by the Master Kill Switch.");

            using var db = await _dbFactory.CreateDbContextAsync();
            var settings = await db.SystemSettings.FirstOrDefaultAsync();
            if (settings == null) throw new InvalidOperationException("System settings not found.");
            if (!settings.PushEnabled) throw new InvalidOperationException("Web Push notifications are disabled in System Settings.");
            if (string.IsNullOrEmpty(settings.VapidPublicKey) || string.IsNullOrEmpty(settings.VapidPrivateKey))
                throw new InvalidOperationException("VAPID keys are missing. Please configure them in Communications Setup.");

            var subscriptions = await db.PushSubscriptions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAtUtc)
                .ToListAsync();

            if (!subscriptions.Any()) throw new InvalidOperationException("User has no registered push devices. They must enable notifications in 'My Security'.");

            var uniqueSubscriptions = subscriptions
                .GroupBy(s => s.Endpoint)
                .Select(g => g.First())
                .ToList();

            var vapidSubject = !string.IsNullOrEmpty(settings.VapidSubject) ? settings.VapidSubject : "mailto:admin@gfc.com";
            var vapidDetails = new VapidDetails(vapidSubject, settings.VapidPublicKey, settings.VapidPrivateKey);
            var webPushClient = new WebPushClient();

            var payload = JsonSerializer.Serialize(new
            {
                title = title,
                body = body,
                url = url ?? "/"
            });

            foreach (var sub in uniqueSubscriptions)
            {
                try
                {
                    var pushSubscription = new WebPush.PushSubscription(sub.Endpoint, sub.P256dh, sub.Auth);
                    await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);

                    var notification = new SystemNotification
                    {
                        RecipientEmail = $"User:{userId}",
                        Subject = title,
                        Message = body,
                        Channel = "Push",
                        Status = "Sent",
                        SentAt = DateTime.UtcNow
                    };
                    db.SystemNotifications.Add(notification);
                }
                catch (Exception ex)
                {
                    var notification = new SystemNotification
                    {
                        RecipientEmail = $"User:{userId}",
                        Subject = title,
                        Message = $"{body} (FAILED: {ex.Message})",
                        Channel = "Push",
                        Status = "Failed",
                        SentAt = DateTime.UtcNow
                    };
                    db.SystemNotifications.Add(notification);
                }
            }
            
            await db.SaveChangesAsync();
        }

        public async Task<GFC.BlazorServer.Data.Entities.UserNotificationPreferences?> GetUserPreferencesAsync(int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.UserNotificationPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task SaveUserPreferencesAsync(GFC.BlazorServer.Data.Entities.UserNotificationPreferences preferences)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.UserNotificationPreferences
                .FirstOrDefaultAsync(p => p.UserId == preferences.UserId);

            if (existing == null)
            {
                db.UserNotificationPreferences.Add(preferences);
            }
            else
            {
                preferences.Id = existing.Id;
                db.Entry(existing).CurrentValues.SetValues(preferences);
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
        }

        public async Task<List<GFC.BlazorServer.Data.Entities.UserNotificationPreferences>> GetAllPreferencesAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.UserNotificationPreferences.ToListAsync();
        }
    }
}


