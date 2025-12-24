// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface ICameraPermissionService
    {
        Task<bool> UserHasAnyCameraPermissionAsync(int userId);
    }
}
