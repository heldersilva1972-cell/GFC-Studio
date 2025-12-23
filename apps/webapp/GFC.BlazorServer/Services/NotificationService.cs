// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
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
    }
}
