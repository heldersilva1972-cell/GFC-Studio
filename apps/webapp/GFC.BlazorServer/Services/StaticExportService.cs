// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace GFC.BlazorServer.Services
{
    public class StaticExportService : IStaticExportService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<StaticExportService> _logger;

        public StaticExportService(
            IDbContextFactory<GfcDbContext> dbContextFactory,
            IWebHostEnvironment env,
            ILogger<StaticExportService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _env = env;
            _logger = logger;
        }

        public async Task<byte[]> ExportSiteAsZipAsync()
        {
            _logger.LogInformation("Starting static site export process.");
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var pages = await dbContext.StudioPages
                .Include(p => p.Sections)
                .Where(p => p.Status == "Published" && !p.IsDeleted)
                .ToListAsync();

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                // 1. Generate Static HTML for each page
                foreach (var page in pages)
                {
                    var htmlContent = await RenderPageToHtmlAsync(page);
                    var entry = archive.CreateEntry($"{page.Slug}.html", CompressionLevel.Fastest);
                    using var entryStream = entry.Open();
                    using var streamWriter = new StreamWriter(entryStream, Encoding.UTF8);
                    await streamWriter.WriteAsync(htmlContent);
                }
                _logger.LogInformation("Generated HTML for {PageCount} pages.", pages.Count);

                // 2. Create backup.json with site structure
                var backupData = new { Pages = pages };
                var jsonContent = JsonSerializer.Serialize(backupData, new JsonSerializerOptions { WriteIndented = true });
                var jsonEntry = archive.CreateEntry("backup.json", CompressionLevel.Fastest);
                using (var entryStream = jsonEntry.Open())
                using (var streamWriter = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    await streamWriter.WriteAsync(jsonContent);
                }
                _logger.LogInformation("Created backup.json with site structure.");

                // 3. Bundle the uploads/ folder
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                if (Directory.Exists(uploadsPath))
                {
                    foreach (var filePath in Directory.GetFiles(uploadsPath, "*.*", SearchOption.AllDirectories))
                    {
                        var relativePath = Path.GetRelativePath(uploadsPath, filePath);
                        archive.CreateEntryFromFile(filePath, Path.Combine("uploads", relativePath));
                    }
                    _logger.LogInformation("Bundled assets from the uploads/ folder.");
                }
                else
                {
                    _logger.LogWarning("Uploads directory not found at {UploadsPath}. Skipping asset bundling.", uploadsPath);
                }
            }

            _logger.LogInformation("Static site export completed successfully.");
            return memoryStream.ToArray();
        }

        private async Task<string> RenderPageToHtmlAsync(StudioPage page)
        {
            // This is a simplified renderer. A real implementation might use a Razor Component renderer
            // or a more sophisticated templating engine to achieve 1:1 visual parity.
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine($"    <meta charset=\"UTF-8\">");
            sb.AppendLine($"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine($"    <title>{page.Title}</title>");
            sb.AppendLine("    <style>body { font-family: sans-serif; line-height: 1.6; padding: 2em; } .component { border: 1px solid #eee; padding: 1em; margin-bottom: 1em; border-radius: 5px; }</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine($"    <h1>{page.Title}</h1>");

            foreach (var section in page.Sections.OrderBy(s => s.OrderIndex))
            {
                sb.AppendLine($"<div class=\"component component-{section.ComponentType.ToLower()}\">");
                sb.AppendLine($"    <h2>{section.ComponentType}</h2>");

                // Deserialize the data to render it
                section.SyncDataToProperties();

                switch (section.ComponentType)
                {
                    case StudioComponentType.Heading:
                        sb.AppendLine(section.Content);
                        break;
                    case StudioComponentType.RichTextBlock:
                        sb.AppendLine(section.Content);
                        break;
                    case StudioComponentType.Image:
                        if(section.properties.TryGetValue("src", out var src) && src is not null)
                        {
                            sb.AppendLine($"    <img src=\"{src.ToString().Replace("~/", "")}\" alt=\"{section.properties.GetValueOrDefault("alt", "")}\" style=\"max-width: 100%;\">");
                        }
                        break;
                    // Add more cases for other component types as needed
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}
