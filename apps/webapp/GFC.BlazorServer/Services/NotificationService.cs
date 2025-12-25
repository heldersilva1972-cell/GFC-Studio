// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
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
            if (_masterKillSwitchEnabled)
            {
                notification.Status = "Blocked by Master Kill Switch";
            } else
            {
                // In a real application, this would integrate with an email/SMS gateway
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
    }
}
