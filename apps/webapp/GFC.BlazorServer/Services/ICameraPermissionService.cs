// [OBSOLETE - DO NOT USE]
// This interface has been moved to Services/Camera/ICameraPermissionService.cs
// This file exists only to prevent build errors during transition
// TODO: Delete this file after confirming no references exist

using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.OBSOLETE
{
    [System.Obsolete("Use GFC.BlazorServer.Services.Camera.ICameraPermissionService instead")]
    public interface ICameraPermissionService
    {
        Task<bool> UserHasAnyCameraPermissionAsync(int userId);
    }
}
