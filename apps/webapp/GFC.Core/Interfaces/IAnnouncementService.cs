// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IAnnouncementService
    {
        Task<List<SystemNotification>> GetActiveAnnouncementsAsync();
    }
}
