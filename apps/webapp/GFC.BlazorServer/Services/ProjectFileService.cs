using GFC.Core.Interfaces;
using GFC.Core.Models;
using System.Text.Json;
using System.Text;

namespace GFC.BlazorServer.Services
{
    public class ProjectFileService : IProjectFileService
    {
        private readonly ILogger<ProjectFileService> _logger;

        public ProjectFileService(ILogger<ProjectFileService> logger)
        {
            _logger = logger;
        }

        public async Task InitializeProjectStructureAsync(string projectRoot)
        {
            if (string.IsNullOrEmpty(projectRoot)) return;

            try
            {
                string[] folders = { "metadata", "metadata/pages", "metadata/sections", "public", "public/assets", "src", "src/components" };
                
                foreach (var folder in folders)
                {
                    var path = Path.Combine(projectRoot, folder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        _logger.LogInformation("Created project folder: {Path}", path);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize project structure at {Root}", projectRoot);
                throw;
            }
        }

        public async Task SavePageDataAsync(string projectRoot, StudioPage page, List<StudioSection> sections)
        {
            if (string.IsNullOrEmpty(projectRoot) || page == null) return;

            try
            {
                await InitializeProjectStructureAsync(projectRoot);

                var pageData = new
                {
                    Page = page,
                    Sections = sections,
                    LastModified = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(pageData, new JsonSerializerOptions { WriteIndented = true });
                var fileName = string.IsNullOrEmpty(page.Slug) ? $"page-{page.Id}.json" : $"{page.Slug}.json";
                var filePath = Path.Combine(projectRoot, "metadata/pages", fileName);

                await File.WriteAllTextAsync(filePath, json);
                _logger.LogInformation("Saved page data to {Path}", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save page data for {PageId}", page?.Id);
                throw;
            }
        }

        public async Task GeneratePageFileAsync(string projectRoot, StudioPage page, List<StudioSection> sections)
        {
            // For now, let's generate a basic HTML preview file in the src folder
            if (string.IsNullOrEmpty(projectRoot) || page == null) return;

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine($"<title>{page.Title}</title>");
                sb.AppendLine("<style>body { font-family: sans-serif; padding: 40px; } .section { margin-bottom: 20px; }</style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine($"<h1>{page.Title}</h1>");

                foreach (var section in sections.OrderBy(s => s.PageIndex))
                {
                    sb.AppendLine($"<div class='section' data-type='{section.ComponentType}'>");
                    // Simplified content rendering
                    sb.AppendLine(section.Content ?? "");
                    sb.AppendLine("</div>");
                }

                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                var fileName = string.IsNullOrEmpty(page.Slug) ? $"page-{page.Id}.html" : $"{page.Slug}.html";
                var filePath = Path.Combine(projectRoot, "src", fileName);

                await File.WriteAllTextAsync(filePath, sb.ToString());
                _logger.LogInformation("Generated page file at {Path}", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate page file for {PageId}", page?.Id);
            }
        }

        public async Task SyncAssetsAsync(string projectRoot, string sourceUploadsFolder)
        {
             if (string.IsNullOrEmpty(projectRoot) || !Directory.Exists(sourceUploadsFolder)) return;

             try 
             {
                 var targetPath = Path.Combine(projectRoot, "public/assets");
                 if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

                 foreach (var file in Directory.GetFiles(sourceUploadsFolder))
                 {
                     var destFile = Path.Combine(targetPath, Path.GetFileName(file));
                     File.Copy(file, destFile, true);
                 }
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, "Failed to sync assets to {Root}", projectRoot);
             }
        }
    }
}
