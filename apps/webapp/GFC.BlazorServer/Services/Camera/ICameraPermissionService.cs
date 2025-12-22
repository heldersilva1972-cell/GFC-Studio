// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public interface ICameraPermissionService
    {
        Task<List<CameraPermission>> GetPermissionsForCameraAsync(int cameraId);
        Task<List<CameraPermission>> GetPermissionsForUserAsync(string userId);
        Task AddPermissionAsync(CameraPermission permission);
        Task RemovePermissionAsync(int permissionId);
        Task<bool> HasPermissionAsync(string userId, int cameraId, CameraAccessLevel accessLevel);
    }
}
