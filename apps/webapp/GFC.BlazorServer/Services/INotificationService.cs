// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface INotificationService
    {
        Task DispatchNotificationAsync(SystemNotification notification);
        void EnabbleMasterKillSwitch();
        void DisableMasterKillSwitch();
        bool IsMasterKillSwitchEnabled();
        Task NotifySystemAlertAsync(string title, string body, string? url = null);
        
        // Push Notifications
        Task SubscribeToPushAsync(int userId, string endpoint, string p256dh, string auth, string? deviceName);
        Task UnsubscribeFromPushAsync(int userId, string endpoint);
        Task SendPushNotificationAsync(int userId, string title, string body, string? url = null);
        
        // Notification Preferences
        Task<GFC.BlazorServer.Data.Entities.UserNotificationPreferences?> GetUserPreferencesAsync(int userId);
        Task SaveUserPreferencesAsync(GFC.BlazorServer.Data.Entities.UserNotificationPreferences preferences);
        Task<List<GFC.BlazorServer.Data.Entities.UserNotificationPreferences>> GetAllPreferencesAsync();
        
        // Rental notification methods
        Task SendRentalConfirmationEmailAsync(HallRentalRequest request);
        Task SendRentalDenialEmailAsync(HallRentalRequest request, string reason);

        // General email sending
        Task<int> GetPushSubscriptionCountAsync(int userId);
        Task<string?> GetVapidPublicKeyAsync();
        Task SendEmailAsync(string email, string subject, string body);
        Task<List<SystemNotification>> GetActiveNotificationsAsync();
    }
}
