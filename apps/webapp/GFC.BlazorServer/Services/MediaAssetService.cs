// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class MediaAssetService : IMediaAssetService, GFC.Core.Interfaces.IMediaAssetService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaAssetService(IDbContextFactory<GfcDbContext> contextFactory, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MediaAsset> CreateAssetAsync(IBrowserFile file, string usage)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var originalFileName = $"{Guid.NewGuid()}_{file.Name}";
            var originalFilePath = Path.Combine(uploadsFolderPath, originalFileName);

            await using (var stream = file.OpenReadStream(long.MaxValue))
            await using (var fs = new FileStream(originalFilePath, FileMode.Create))
            {
                await stream.CopyToAsync(fs);
            }

            string finalFilePath = originalFilePath;
            string finalFileName = originalFileName;
            string finalContentType = file.ContentType;
            long finalFileSize = file.Size;

            if (file.ContentType.StartsWith("image/") && file.ContentType != "image/webp")
            {
                using var image = await Image.LoadAsync(originalFilePath);
                var webpFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}.webp";
                var webpFilePath = Path.Combine(uploadsFolderPath, webpFileName);

                await image.SaveAsync(webpFilePath, new WebpEncoder());

                finalFilePath = webpFilePath;
                finalFileName = webpFileName;
                finalContentType = "image/webp";
                finalFileSize = new FileInfo(webpFilePath).Length;

                File.Delete(originalFilePath);
            }

            var asset = new MediaAsset
            {
                FileName = file.Name,
                StoredFileName = finalFileName,
                ContentType = finalContentType,
                FileSize = finalFileSize,
                Usage = usage,
                UploadedAt = DateTime.UtcNow
            };

            if (file.ContentType.StartsWith("image/"))
            {
                await GenerateRenditionsAsync(asset, finalFilePath);
            }

            using var context = await _contextFactory.CreateDbContextAsync();
            context.MediaAssets.Add(asset);
            await context.SaveChangesAsync();

            return asset;
        }

        private async Task GenerateRenditionsAsync(MediaAsset asset, string originalFilePath)
        {
            using var image = await Image.LoadAsync(originalFilePath);

            // Desktop (1920px), Tablet (1024px), Mobile (640px)
            var sizes = new Dictionary<string, int>
            {
                { "desktop", 1920 },
                { "tablet", 1024 },
                { "mobile", 640 }
            };

            foreach (var size in sizes)
            {
                var renditionFileName = $"{Path.GetFileNameWithoutExtension(originalFilePath)}_{size.Key}.webp";
                var renditionFilePath = Path.Combine(Path.GetDirectoryName(originalFilePath), renditionFileName);

                var clone = image.Clone(ctx => ctx.Resize(new ResizeOptions
                {
                    Size = new Size(size.Value, 0),
                    Mode = ResizeMode.Max
                }));

                await clone.SaveAsync(renditionFilePath, new WebpEncoder());

                asset.Renditions.Add(new MediaRendition
                {
                    RenditionType = $"{size.Key}-webp",
                    Url = $"/uploads/{renditionFileName}",
                    Width = clone.Width,
                    Height = clone.Height
                });
            }
        }

        public async Task<List<MediaAsset>> GetAllAssetsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets.Include(a => a.Renditions).ToListAsync();
        }

        public async Task<MediaAsset> GetAssetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var asset = await context.MediaAssets.Include(a => a.Renditions).FirstOrDefaultAsync(a => a.Id == id);

            if (asset != null && !string.IsNullOrEmpty(asset.RequiredRole))
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.IsInRole(asset.RequiredRole))
                {
                    return null; // Or throw an exception, depending on desired behavior
                }
            }

            return asset;
        }

        public async Task UpdateAssetRoleAsync(int id, string? role)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var asset = await context.MediaAssets.FindAsync(id);
            if (asset != null)
            {
                asset.RequiredRole = role;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAssetAsync(int id)
        {
            await DeleteMediaAssetAsync(id);
        }

        // GFC.Core.Interfaces.IMediaAssetService Implementation

        public async Task<MediaAsset> CreateMediaAssetAsync(Stream fileStream, string fileName, string tag, string uploadedBy)
        {
            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fs);
            }

            var asset = new MediaAsset
            {
                FileName = fileName,
                StoredFileName = uniqueFileName,
                ContentType = "image/jpeg", // Simplified for now
                FileSize = new FileInfo(filePath).Length,
                Usage = tag,
                Tag = tag,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.UtcNow
            };

            await GenerateRenditionsAsync(asset, filePath);

            using var context = await _contextFactory.CreateDbContextAsync();
            context.MediaAssets.Add(asset);
            await context.SaveChangesAsync();

            return asset;
        }

        public async Task<IEnumerable<MediaAsset>> GetMediaAssetsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets.Include(a => a.Renditions).ToListAsync();
        }

        public async Task<IEnumerable<MediaAsset>> GetPublicWebsiteGalleryAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets
                .Include(a => a.Renditions)
                .Where(a => a.Tag == "Public Website Gallery")
                .ToListAsync();
        }

        public async Task DeleteMediaAssetAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var isAssetInUse = await context.StudioSectionAssets.AnyAsync(ssa => ssa.MediaAssetId == id);
            if (isAssetInUse)
            {
                throw new InvalidOperationException("This media asset is currently in use by a Studio Section and cannot be deleted.");
            }

            var asset = await context.MediaAssets.Include(a => a.Renditions).FirstOrDefaultAsync(a => a.Id == id);
            if (asset != null)
            {
                // Delete physical files
                foreach (var rendition in asset.Renditions)
                {
                    var filePath = Path.Combine(_env.WebRootPath, rendition.Url.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                var originalFilePath = Path.Combine(_env.WebRootPath, "uploads", asset.StoredFileName);
                if (File.Exists(originalFilePath))
                {
                    File.Delete(originalFilePath);
                }

                context.MediaAssets.Remove(asset);
                await context.SaveChangesAsync();
            }
        }
    }
}
