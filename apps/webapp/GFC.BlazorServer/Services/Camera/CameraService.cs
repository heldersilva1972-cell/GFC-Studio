// [NEW]
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraService : ICameraService
    {
        private readonly HttpClient _httpClient;
        private readonly string _videoAgentBaseUrl;

        public CameraService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _videoAgentBaseUrl = configuration["VideoAgent:BaseUrl"] ?? "https://localhost:5101";
        }

        public async Task<List<Camera>> GetAllCamerasAsync()
        {
            // Placeholder: In a real app, this would come from a database or config
            return await Task.FromResult(new List<Camera>
            {
                new Camera { Id = 1, Name = "Lobby" },
                new Camera { Id = 2, Name = "Warehouse" },
                new Camera { Id = 3, Name = "Office" },
                new Camera { Id = 4, Name = "Parking Lot" }
            });
        }

        public async Task<CameraCapabilities> GetCameraCapabilitiesAsync(int cameraId)
        {
            var response = await _httpClient.GetAsync($"{_videoAgentBaseUrl}/api/cameras/{cameraId}/capabilities");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CameraCapabilities>();
        }

        public async Task<bool> SendPTZCommandAsync(int cameraId, PTZCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_videoAgentBaseUrl}/api/cameras/{cameraId}/ptz", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<byte[]> CaptureSnapshotAsync(int cameraId)
        {
            return await _httpClient.GetByteArrayAsync($"{_videoAgentBaseUrl}/api/cameras/{cameraId}/snapshot");
        }

        public async Task<StreamQuality> GetStreamQualityAsync(int cameraId)
        {
            var response = await _httpClient.GetAsync($"{_videoAgentBaseUrl}/api/streams/{cameraId}/quality");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StreamQuality>();
        }

        public async Task SetStreamQualityAsync(int cameraId, StreamQuality quality)
        {
            await _httpClient.PostAsJsonAsync($"{_videoAgentBaseUrl}/api/streams/{cameraId}/quality", quality);
        }
    }
}
