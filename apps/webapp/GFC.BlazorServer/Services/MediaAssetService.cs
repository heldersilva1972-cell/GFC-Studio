// [VERIFIED FIX + DIAGNOSTICS]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
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

        private string PrepareStoredFileName(string originalName)
        {
            var guid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(originalName);
            var baseName = Path.GetFileNameWithoutExtension(originalName);
            
            // Limit base name to 150 chars to stay safe within 255 char limit (36 for GUID + 1 for underscore + extension)
            if (baseName.Length > 150)
            {
                baseName = baseName.Substring(0, 150);
            }
            
            return $"{guid}_{baseName}{extension}";
        }

        private string TruncateFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "unnamed_file";
            return name.Length > 250 ? name.Substring(0, 250) : name;
        }

        public async Task<MediaAsset> CreateAssetAsync(IBrowserFile file, string usage)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath)) Directory.CreateDirectory(uploadsFolderPath);

            var uniqueFileName = PrepareStoredFileName(file.Name);
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            await using (var stream = file.OpenReadStream(long.MaxValue))
            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fs);
            }

            var asset = new MediaAsset
            {
                FileName = TruncateFileName(file.Name),
                StoredFileName = uniqueFileName,
                ContentType = file.ContentType,
                FileSize = file.Size,
                Size = file.Size, // Map to 'Size' column
                Url = $"/uploads/{uniqueFileName}",
                Usage = string.IsNullOrWhiteSpace(usage) ? "General" : usage,
                Tag = string.IsNullOrWhiteSpace(usage) ? null : usage,
                UploadedBy = "System", // Required
                CreatedAt = DateTime.UtcNow,
                UploadedAt = DateTime.UtcNow
            };

            using var context = await _contextFactory.CreateDbContextAsync();
            try 
            {
                context.MediaAssets.Add(asset);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database Save Failed (CreateAsset): {innerMessage}");
            }

            return asset;
        }

        public async Task<MediaAsset> CreateMediaAssetAsync(Stream fileStream, string fileName, string tag, string uploadedBy)
        {
            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath)) Directory.CreateDirectory(uploadsFolderPath);

            var uniqueFileName = PrepareStoredFileName(fileName);
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fs);
            }

            var asset = new MediaAsset
            {
                FileName = TruncateFileName(fileName),
                StoredFileName = uniqueFileName,
                ContentType = "image/jpeg",
                FileSize = new FileInfo(filePath).Length,
                Size = new FileInfo(filePath).Length, // Map to 'Size' column
                Url = $"/uploads/{uniqueFileName}",
                Usage = string.IsNullOrWhiteSpace(tag) ? "LiquorReference" : tag,
                Tag = tag,
                UploadedBy = "System", // Required
                CreatedAt = DateTime.UtcNow,
                UploadedAt = DateTime.UtcNow
            };

            using var context = await _contextFactory.CreateDbContextAsync();
            try 
            {
                context.MediaAssets.Add(asset);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database Save Failed (CreateMediaAsset): {innerMessage}");
            }

            return asset;
        }

        public async Task<List<MediaAsset>> GetAllAssetsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets.ToListAsync();
        }

        public async Task<MediaAsset> GetAssetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<MediaAsset>> GetMediaAssetsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets.ToListAsync();
        }

        public async Task<IEnumerable<MediaAsset>> GetPublicWebsiteGalleryAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MediaAssets
                .Where(a => a.Usage == "Public Website Gallery")
                .ToListAsync();
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

        public async Task DeleteMediaAssetAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var asset = await context.MediaAssets.FirstOrDefaultAsync(a => a.Id == id);
            if (asset != null)
            {
                var originalFilePath = Path.Combine(_env.WebRootPath, "uploads", asset.StoredFileName);
                if (File.Exists(originalFilePath)) File.Delete(originalFilePath);

                context.MediaAssets.Remove(asset);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAssetAsync(int id) => await DeleteMediaAssetAsync(id);
    }
}


