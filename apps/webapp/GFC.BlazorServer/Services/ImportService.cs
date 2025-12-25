// [NEW]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
// Note: This service has a dependency on the HtmlSanitizer library.
using Ganss.Xss;

namespace GFC.BlazorServer.Services
{
    public class ImportService : IImportService
    {
        private readonly GfcDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly IStudioService _studioService;
        private readonly HtmlSanitizer _htmlSanitizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DomAnalysisService _domAnalysisService;
        private static readonly HashSet<string> _allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp",
            ".css",
            ".woff", ".woff2", ".ttf", ".otf", ".eot"
        };

        public ImportService(GfcDbContext dbContext, IStudioService studioService, HttpClient httpClient, IWebHostEnvironment webHostEnvironment, DomAnalysisService domAnalysisService)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
            _studioService = studioService;
            _htmlSanitizer = new HtmlSanitizer();
            _webHostEnvironment = webHostEnvironment;
            _domAnalysisService = domAnalysisService;
        }

        public async Task ImportPageFromUrlAsync(string url, Action<string> onLog)
        {
            onLog($"Starting import from URL: {url}");

            try
            {
                var htmlContent = await ScrapeHtmlContentAsync(url, onLog);
                if (string.IsNullOrEmpty(htmlContent))
                {
                    onLog("HTML content was empty. This could be because the page is a Single Page Application (SPA) or requires JavaScript to render. Aborting import.");
                    return;
                }

                var document = await new HtmlParser().ParseDocumentAsync(htmlContent);
                var pageTitle = document.Title ?? $"Imported - {url}";

                var studioSections = await ParseHtmlToStudioSectionsAsync(document.Body, new Uri(url), onLog);

                var newPage = new StudioPage
                {
                    Title = pageTitle,
                    Slug = GenerateUniqueSlug(pageTitle),
                    Sections = studioSections
                };

                _dbContext.StudioPages.Add(newPage);
                await _dbContext.SaveChangesAsync();

                onLog($"Successfully imported page from {url} into StudioPage with ID {newPage.Id}");
            }
            catch (Exception ex)
            {
                onLog($"An error occurred while importing page from URL: {url}. Error: {ex.Message}");
            }
        }

        private async Task<string> ScrapeHtmlContentAsync(string url, Action<string> onLog)
        {
            try
            {
                onLog("Requesting fully rendered HTML from Playwright worker...");
                var scraperUrl = "http://localhost:3001/scrape";
                var payload = new { url };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(scraperUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ScrapeResult>();
                    onLog("Successfully received HTML from Playwright worker.");
                    return result.html;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    onLog($"Failed to get HTML from Playwright worker. Status: {response.StatusCode}. Error: {error}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                onLog($"Failed to connect to the Playwright worker. Please ensure it is running. Error: {ex.Message}");
                return null;
            }
        }

        private async Task<List<StudioSection>> ParseHtmlToStudioSectionsAsync(IElement body, Uri baseUrl, Action<string> onLog)
        {
            onLog("Performing DOM analysis to map HTML to Studio components...");

            await HarvestAssetsAsync(body, baseUrl, onLog);

            var sections = _domAnalysisService.Analyze(body);

            // Sanitize and set order
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i].Content = _htmlSanitizer.Sanitize(sections[i].Content);
                sections[i].OrderIndex = i;
            }

            if (!sections.Any())
            {
                onLog("Could not map any elements to specific components. The entire body will be imported as a single RawHtml section.");
                var sanitizedContent = _htmlSanitizer.Sanitize(body.InnerHtml);
                sections.Add(new StudioSection
                {
                    ComponentType = "RawHtml",
                    Content = sanitizedContent,
                    OrderIndex = 0
                });
            }

            return sections;
        }

        private async Task HarvestAssetsAsync(IElement element, Uri baseUrl, Action<string> onLog)
        {
            var mediaFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "imported_media");
            Directory.CreateDirectory(mediaFolderPath);

            var images = element.QuerySelectorAll("img");
            foreach (var img in images)
            {
                await ProcessImageAsset(img, baseUrl, mediaFolderPath, onLog);
            }

            var stylesheets = element.QuerySelectorAll("link[rel='stylesheet']");
            foreach (var stylesheet in stylesheets)
            {
                await ProcessStylesheetAsset(stylesheet, baseUrl, mediaFolderPath, onLog);
            }
        }

        private async Task ProcessImageAsset(IElement img, Uri baseUrl, string mediaFolderPath, Action<string> onLog)
        {
            var src = img.GetAttribute("src");
            if (string.IsNullOrEmpty(src) || src.StartsWith("data:")) return;

            var absoluteUrl = new Uri(baseUrl, src);
            onLog($"Found image asset: {absoluteUrl}");
            await DownloadAndReplaceUrlAsync(absoluteUrl, mediaFolderPath, (newPath) => img.SetAttribute("src", newPath), onLog);
        }

        private async Task ProcessStylesheetAsset(IElement stylesheet, Uri baseUrl, string mediaFolderPath, Action<string> onLog)
        {
            var href = stylesheet.GetAttribute("href");
            if (string.IsNullOrEmpty(href)) return;

            var absoluteUrl = new Uri(baseUrl, href);
            onLog($"Found stylesheet asset: {absoluteUrl}");
            await DownloadAndReplaceUrlAsync(absoluteUrl, mediaFolderPath, (newPath) => stylesheet.SetAttribute("href", newPath), onLog, (css, url) => ProcessCssContent(css, url, onLog));
        }

        private async Task<string> ProcessCssContent(string cssContent, Uri cssBaseUrl, Action<string> onLog)
        {
            var fontUrlRegex = new Regex(@"url\((?!['""]?data:)([^)]+)\)");
            var mediaFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "imported_media");
            var matches = fontUrlRegex.Matches(cssContent);

            foreach (Match match in matches)
            {
                var fontUrl = match.Groups[1].Value.Trim('\'', '"');
                var absoluteFontUrl = new Uri(cssBaseUrl, fontUrl);
                string newFontPath = null;
                await DownloadAndReplaceUrlAsync(absoluteFontUrl, mediaFolderPath, (newPath) => newFontPath = newPath, onLog);
                if(newFontPath != null)
                {
                    cssContent = cssContent.Replace(fontUrl, newFontPath);
                }
            }

            return cssContent;
        }

        private async Task DownloadAndReplaceUrlAsync(Uri url, string mediaFolderPath, Action<string> setAttributeAction, Action<string> onLog, Func<string, Uri, Task<string>> contentProcessor = null)
        {
            try
            {
                var extension = Path.GetExtension(url.AbsolutePath);
                if (!_allowedExtensions.Contains(extension))
                {
                    onLog($"Skipping asset with disallowed extension: {url}");
                    return;
                }

                var fileContent = await _httpClient.GetByteArrayAsync(url);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var relativePath = Path.Combine("imported_media", fileName);
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (contentProcessor != null)
                {
                    var originalContent = System.Text.Encoding.UTF8.GetString(fileContent);
                    var processedContent = await contentProcessor(originalContent, url);
                    await File.WriteAllTextAsync(fullPath, processedContent);
                }
                else
                {
                    await File.WriteAllBytesAsync(fullPath, fileContent);
                }

                var newPath = $"/{relativePath.Replace("\\", "/")}";
                setAttributeAction(newPath);
                onLog($"Successfully downloaded and saved asset to {newPath}");
            }
            catch (Exception ex)
            {
                onLog($"Failed to download or save asset from {url}. Error: {ex.Message}");
            }
        }

        private string GenerateUniqueSlug(string title)
        {
            var slug = title.ToLower().Replace(" ", "-").Replace("&", "and");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", ""); // Remove invalid chars
            slug = Regex.Replace(slug, @"-+", "-").Trim('-'); // Remove duplicate/trailing dashes
            return $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        private class ScrapeResult
        {
            public string html { get; set; }
        }
    }
}
