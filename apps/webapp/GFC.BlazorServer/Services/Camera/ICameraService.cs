// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public interface ICameraService
    {
        Task<List<GFC.Core.Models.Camera>> GetAllCamerasAsync();
        Task<GFC.Core.Models.Camera> GetCameraByIdAsync(int id);
        Task<GFC.Core.Models.Camera> CreateCameraAsync(GFC.Core.Models.Camera camera);
        Task UpdateCameraAsync(GFC.Core.Models.Camera camera);
        Task DeleteCameraAsync(int id);
        Task<bool> SendPTZCommandAsync(int cameraId, PTZCommand command);
    }
}
