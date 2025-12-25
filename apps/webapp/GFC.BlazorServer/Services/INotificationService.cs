// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface INotificationService
    {
        Task DispatchNotificationAsync(SystemNotification notification);
        void EnabbleMasterKillSwitch();
        void DisableMasterKillSwitch();
        bool IsMasterKillSwitchEnabled();
        
        // Rental notification methods
        Task SendRentalConfirmationEmailAsync(HallRentalRequest request);
        Task SendRentalDenialEmailAsync(HallRentalRequest request, string reason);

        // General email sending
        Task SendEmailAsync(string email, string subject, string body);
    }
}
