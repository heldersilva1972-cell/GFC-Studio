// [MODIFIED]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetManagerController : ControllerBase
    {
        private readonly GfcDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AssetManagerController(GfcDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/AssetManager/folders
        [HttpGet("folders")]
        public async Task<IActionResult> GetFolders()
        {
            var folders = await _context.AssetFolders
                .Include(f => f.SubFolders)
                .Where(f => f.ParentFolderId == null) // Start with root folders
                .ToListAsync();
            return Ok(folders);
        }

        // GET: api/AssetManager/assets/{folderId}
        [HttpGet("assets/{folderId?}")]
        public async Task<IActionResult> GetAssets(int? folderId)
        {
            var assets = await _context.MediaAssets
                .Where(a => a.AssetFolderId == folderId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return Ok(assets);
        }

        // POST: api/AssetManager/folders
        [HttpPost("folders")]
        public async Task<IActionResult> CreateFolder([FromBody] AssetFolder folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AssetFolders.Add(folder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFolders), new { id = folder.Id }, folder);
        }

        // POST: api/AssetManager/upload/{folderId}
        [HttpPost("upload/{folderId?}")]
        public async Task<IActionResult> Upload(IFormFile file, int? folderId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Security: Validate file size (e.g., 5MB limit)
            const long maxFileSize = 5 * 1024 * 1024;
            if (file.Length > maxFileSize)
                return BadRequest($"File size exceeds the limit of {maxFileSize / 1024 / 1024} MB.");

            // Security: Validate file type
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                return BadRequest("Invalid file type. Only JPEG and PNG are allowed.");

            var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads", "assets");
            Directory.CreateDirectory(uploadsFolderPath);

            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var uniqueFileNameSuffix = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            var fileExtension = ".webp"; // We convert everything to WebP

            var asset = new MediaAsset
            {
                FileName = file.FileName,
                FileType = "image/webp", // Standardize on WebP
                FileSize = file.Length, // Original size
                AssetFolderId = folderId,
                CreatedAt = DateTime.UtcNow
            };

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                var sizes = new Dictionary<string, int>
                {
                    { "xl", 1920 }, { "lg", 1280 }, { "md", 768 }, { "sm", 375 }
                };

                foreach (var size in sizes)
                {
                    var newFileName = $"{originalFileName}_{size.Key}_{uniqueFileNameSuffix}{fileExtension}";
                    var fullPath = Path.Combine(uploadsFolderPath, newFileName);
                    var relativePath = $"/uploads/assets/{newFileName}";

                    // Clone the original image to avoid progressive quality loss
                    using (var clonedImage = image.Clone(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(size.Value),
                        Mode = ResizeMode.Max
                    })))
                    {
                        await clonedImage.SaveAsWebpAsync(fullPath);
                    }

                    switch (size.Key)
                    {
                        case "xl": asset.FilePath_xl = relativePath; break;
                        case "lg": asset.FilePath_lg = relativePath; break;
                        case "md": asset.FilePath_md = relativePath; break;
                        case "sm": asset.FilePath_sm = relativePath; break;
                    }
                }
                asset.FilePath = asset.FilePath_xl; // Set main path to the largest version
            }

            _context.MediaAssets.Add(asset);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssets), new { folderId = asset.AssetFolderId }, asset);
        }

        // DELETE: api/AssetManager/assets/{id}
        [HttpDelete("assets/{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var asset = await _context.MediaAssets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            // Delete files from storage
            var paths = new[] { asset.FilePath, asset.FilePath_xl, asset.FilePath_lg, asset.FilePath_md, asset.FilePath_sm };
            foreach (var path in paths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var fullPath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            _context.MediaAssets.Remove(asset);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
