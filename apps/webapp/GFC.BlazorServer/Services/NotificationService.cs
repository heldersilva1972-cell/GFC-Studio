// [NEW]
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly GfcDbContext _context;
        private bool _masterKillSwitchEnabled = false;

        public NotificationService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task DispatchNotificationAsync(SystemNotification notification)
        {
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();
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

            _context.SystemNotifications.Add(notification);
            await _context.SaveChangesAsync();
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

            var usersToNotify = await _context.UserNotificationPreferences
                .Where(p => p.SystemAlertNotifyEmail || p.SystemAlertNotifyPush)
                .ToListAsync();

            foreach (var pref in usersToNotify)
            {
                // Note: In a larger app, we'd join this, but for alerts it's fine for now
                var user = await _context.AppUsers.FindAsync(pref.UserId);
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
            if (_masterKillSwitchEnabled)
            {
                // Email blocked by kill switch
                return;
            }

            // TODO: Implement actual email sending logic
            // For now, just create a notification record
            var notification = new SystemNotification
            {
                RecipientEmail = request.RequesterEmail,
                Subject = "Hall Rental Request Approved",
                Message = $"Your rental request for {request.EventDate:MMMM dd, yyyy} has been approved.",
                Channel = "Email",
                Status = "Sent",
                SentAt = DateTime.UtcNow
            };

            _context.SystemNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendRentalDenialEmailAsync(HallRentalRequest request, string reason)
        {
            if (_masterKillSwitchEnabled)
            {
                // Email blocked by kill switch
                return;
            }

            // TODO: Implement actual email sending logic
            // For now, just create a notification record
            var notification = new SystemNotification
            {
                RecipientEmail = request.RequesterEmail,
                Subject = "Hall Rental Request Denied",
                Message = $"Your rental request for {request.EventDate:MMMM dd, yyyy} has been denied. Reason: {reason}",
                Channel = "Email",
                Status = "Sent",
                SentAt = DateTime.UtcNow
            };

            _context.SystemNotifications.Add(notification);
            await _context.SaveChangesAsync();
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
                Status = "Sent",
                SentAt = DateTime.UtcNow
            };

            _context.SystemNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SystemNotification>> GetActiveNotificationsAsync()
        {
            return await _context.SystemNotifications
                                 .Where(n => n.Status == "Sent")
                                 .ToListAsync();
        }

        public async Task SubscribeToPushAsync(int userId, string endpoint, string p256dh, string auth, string? deviceName)
        {
            var existing = await _context.PushSubscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == endpoint);

            if (existing == null)
            {
                _context.PushSubscriptions.Add(new PushSubscription
                {
                    UserId = userId,
                    Endpoint = endpoint,
                    P256dh = p256dh,
                    Auth = auth,
                    DeviceName = deviceName,
                    CreatedAtUtc = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnsubscribeFromPushAsync(int userId, string endpoint)
        {
            var existing = await _context.PushSubscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == endpoint);

            if (existing != null)
            {
                _context.PushSubscriptions.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendPushNotificationAsync(int userId, string title, string body, string? url = null)
        {
            if (_masterKillSwitchEnabled) return;

            var subscriptions = await _context.PushSubscriptions
                .Where(s => s.UserId == userId)
                .ToListAsync();

            foreach (var sub in subscriptions)
            {
                // In a real production app, we would use WebPush library here
                // For now, we log the intent and store it in system notifications
                var notification = new SystemNotification
                {
                    RecipientEmail = $"User:{userId}",
                    Subject = title,
                    Message = $"{body} (URL: {url ?? "/"})",
                    Channel = "Push",
                    Status = "Sent",
                    SentAt = DateTime.UtcNow
                };
                _context.SystemNotifications.Add(notification);
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<GFC.BlazorServer.Data.Entities.UserNotificationPreferences?> GetUserPreferencesAsync(int userId)
        {
            return await _context.UserNotificationPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task SaveUserPreferencesAsync(GFC.BlazorServer.Data.Entities.UserNotificationPreferences preferences)
        {
            var existing = await _context.UserNotificationPreferences
                .FirstOrDefaultAsync(p => p.UserId == preferences.UserId);

            if (existing == null)
            {
                _context.UserNotificationPreferences.Add(preferences);
            }
            else
            {
                preferences.Id = existing.Id; // Ensure ID matches for update
                _context.Entry(existing).CurrentValues.SetValues(preferences);
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<GFC.BlazorServer.Data.Entities.UserNotificationPreferences>> GetAllPreferencesAsync()
        {
            return await _context.UserNotificationPreferences.ToListAsync();
        }
    }
}
