// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;

namespace GFC.BlazorServer.Services
{
    public class MediaAssetService : IMediaAssetService
    {
        private readonly GfcDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaAssetService(GfcDbContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MediaAsset> CreateMediaAssetAsync(Stream fileStream, string fileName, string tag, string uploadedBy)
        {
            if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));

            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads", "media");
            Directory.CreateDirectory(uploadsFolderPath);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var originalFilePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var image = await Image.LoadAsync(fileStream))
            {
                // Save original
                await image.SaveAsync(originalFilePath);

                // Create renditions
                await CreateRenditionAsync(image, "desktop", 1920, originalFilePath);
                await CreateRenditionAsync(image, "tablet", 1024, originalFilePath);
                await CreateRenditionAsync(image, "mobile", 640, originalFilePath);
            }

            var mediaAsset = new MediaAsset
            {
                FileName = uniqueFileName,
                FilePath = $"/uploads/media/{uniqueFileName}",
                Tag = tag,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.UtcNow
            };

            _context.MediaAssets.Add(mediaAsset);
            await _context.SaveChangesAsync();

            return mediaAsset;
        }

        private async Task CreateRenditionAsync(Image sourceImage, string size, int width, string originalPath)
        {
            var renditionPath = Path.ChangeExtension(originalPath, $".{size}.webp");

            using var clonedImage = sourceImage.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(width, 0),
                Mode = ResizeMode.Max
            }));

            await clonedImage.SaveAsync(renditionPath, new WebpEncoder { Quality = 80 });
        }

        public async Task DeleteMediaAssetAsync(int id)
        {
            var mediaAsset = await _context.MediaAssets.FindAsync(id);
            if (mediaAsset == null) return;

            // Delete the original file and all renditions
            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads", "media");
            var originalPath = Path.Combine(uploadsFolderPath, mediaAsset.FileName);

            File.Delete(originalPath);
            File.Delete(Path.ChangeExtension(originalPath, ".desktop.webp"));
            File.Delete(Path.ChangeExtension(originalPath, ".tablet.webp"));
            File.Delete(Path.ChangeExtension(originalPath, ".mobile.webp"));

            _context.MediaAssets.Remove(mediaAsset);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MediaAsset>> GetMediaAssetsAsync()
        {
            return await _context.MediaAssets.OrderByDescending(m => m.UploadedAt).ToListAsync();
        }

        public async Task<IEnumerable<MediaAsset>> GetPublicWebsiteGalleryAsync()
        {
            return await _context.MediaAssets
                .Where(m => m.Tag == "Public Website Gallery")
                .OrderByDescending(m => m.UploadedAt)
                .ToListAsync();
        }
    }
}
