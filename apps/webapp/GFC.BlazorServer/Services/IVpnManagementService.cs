// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IVpnManagementService
    {
        Task RevokeUserAccessAsync(int userId);
        Task DisconnectAllUsersAsync();
    }
}
