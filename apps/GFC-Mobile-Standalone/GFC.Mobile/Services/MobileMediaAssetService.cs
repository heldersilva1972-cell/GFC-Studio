using GFC.Core.Interfaces;
using GFC.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GFC.Mobile.Services
{
    public class MobileMediaAssetService : IMediaAssetService
    {
        private readonly HttpClient _http;

        public MobileMediaAssetService(HttpClient http)
        {
            _http = http;
        }

        public async Task<MediaAsset> CreateMediaAssetAsync(Stream fileStream, string fileName, string tag, string uploadedBy)
        {
            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "file", fileName);
            content.Add(new StringContent(tag), "tag");
            content.Add(new StringContent(uploadedBy), "uploadedBy");

            var response = await _http.PostAsync("api/assets/upload", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MediaAsset>() ?? new MediaAsset();
            }
            return new MediaAsset { StoredFileName = fileName };
        }

        public async Task<IEnumerable<MediaAsset>> GetMediaAssetsAsync()
        {
            return await _http.GetFromJsonAsync<List<MediaAsset>>("api/assets") ?? new List<MediaAsset>();
        }

        public async Task<IEnumerable<MediaAsset>> GetPublicWebsiteGalleryAsync()
        {
            return await _http.GetFromJsonAsync<List<MediaAsset>>("api/assets/gallery") ?? new List<MediaAsset>();
        }

        public async Task DeleteMediaAssetAsync(int id)
        {
            await _http.DeleteAsync($"api/assets/{id}");
        }

        public async Task UpdateAssetRoleAsync(int id, string? role)
        {
            await _http.PostAsJsonAsync($"api/assets/{id}/role", role);
        }
    }
}
