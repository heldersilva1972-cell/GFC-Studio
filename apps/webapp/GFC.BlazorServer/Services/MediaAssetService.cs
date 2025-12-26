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

        public async Task<MediaAsset> CreateAssetAsync(IBrowserFile file, string usage)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.Name}";
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            await using (var stream = file.OpenReadStream(long.MaxValue))
            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fs);
            }

            var asset = new MediaAsset
            {
                FileName = file.Name,
                StoredFileName = uniqueFileName,
                ContentType = file.ContentType,
                FileSize = file.Size,
                Usage = usage,
                UploadedAt = DateTime.UtcNow
            };

            if (file.ContentType.StartsWith("image/"))
            {
                await GenerateRenditionsAsync(asset, filePath);
            }

            _context.MediaAssets.Add(asset);
            await _context.SaveChangesAsync();

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
            return await _context.MediaAssets.Include(a => a.Renditions).ToListAsync();
        }

        public async Task<MediaAsset> GetAssetByIdAsync(int id)
        {
            var asset = await _context.MediaAssets.Include(a => a.Renditions).FirstOrDefaultAsync(a => a.Id == id);

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

        public async Task UpdateAssetRoleAsync(int assetId, string? role)
        {
            var asset = await _context.MediaAssets.FindAsync(assetId);
            if (asset != null)
            {
                asset.RequiredRole = role;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAssetAsync(int id)
        {
            var asset = await GetAssetByIdAsync(id);
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


                _context.MediaAssets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }
    }
}
