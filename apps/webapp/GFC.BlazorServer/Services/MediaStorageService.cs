// [MODIFIED]
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GFC.BlazorServer.Services
{
    public class MediaStorageService : IMediaStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly ILogger<MediaStorageService> _logger;

        public MediaStorageService(IWebHostEnvironment webHostEnvironment, HttpClient httpClient, ILogger<MediaStorageService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> SaveImageFromUrlAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) || !Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
            {
                _logger.LogWarning("Invalid image URL provided: {ImageUrl}", imageUrl);
                return null;
            }

            try
            {
                var response = await _httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                using var imageStream = await response.Content.ReadAsStreamAsync();
                using var image = await Image.LoadAsync(imageStream);

                var fileName = Path.GetFileNameWithoutExtension(new Uri(imageUrl).AbsolutePath);
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}.webp";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "studio", uniqueFileName);

                await image.SaveAsync(filePath, new WebpEncoder());

                var relativePath = $"/uploads/studio/{uniqueFileName}";
                _logger.LogInformation("Successfully downloaded, optimized, and saved image from {ImageUrl} to {RelativePath}", imageUrl, relativePath);
                return relativePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download or process image from {ImageUrl}", imageUrl);
                return null;
            }
        }
    }
}
