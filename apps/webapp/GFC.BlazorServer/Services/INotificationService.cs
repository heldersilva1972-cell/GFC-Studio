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
    }
}
