// [MODIFIED]
// NOTE: This service uses Playwright for browser automation.
// The target deployment environment must have Playwright's browser binaries installed.
// Run `pwsh bin/Debug/netX/playwright.ps1 install` in the project directory.
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class ContentIngestionService : IContentIngestionService
    {
        private readonly ILogger<ContentIngestionService> _logger;
        private readonly IMediaStorageService _mediaStorageService;
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;

        public ContentIngestionService(
            ILogger<ContentIngestionService> logger,
            IMediaStorageService mediaStorageService,
            IDbContextFactory<GfcDbContext> dbContextFactory)
        {
            _logger = logger;
            _mediaStorageService = mediaStorageService;
            _dbContextFactory = dbContextFactory;
        }

        public async Task CreateRedirectAsync(string oldUrl, string newUrlSlug)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var existingRedirect = await dbContext.UrlRedirects.FirstOrDefaultAsync(r => r.OldUrl == oldUrl);
            if (existingRedirect != null)
            {
                _logger.LogInformation("Redirect for {OldUrl} already exists. Updating to point to {NewUrlSlug}", oldUrl, newUrlSlug);
                existingRedirect.NewUrl = newUrlSlug;
            }
            else
            {
                _logger.LogInformation("Creating new 301 redirect from {OldUrl} to {NewUrlSlug}", oldUrl, newUrlSlug);
                var newRedirect = new UrlRedirect
                {
                    OldUrl = oldUrl,
                    NewUrl = newUrlSlug,
                    RedirectType = 301
                };
                await dbContext.UrlRedirects.AddAsync(newRedirect);
            }
            await dbContext.SaveChangesAsync();
        }

        private async Task<bool> IsUrlPubliclyRoutable(Uri uri)
        {
            try
            {
                var hostAddresses = await Dns.GetHostAddressesAsync(uri.DnsSafeHost);
                if (!hostAddresses.Any())
                {
                    _logger.LogWarning("SSRF check failed: No IP address found for host {Host}", uri.DnsSafeHost);
                    return false;
                }

                foreach (var ipAddress in hostAddresses)
                {
                    if (IPAddress.IsLoopback(ipAddress) || IsPrivate(ipAddress))
                    {
                        _logger.LogWarning("SSRF check failed: Host {Host} resolved to a non-public IP address: {IP}", uri.DnsSafeHost, ipAddress);
                        return false;
                    }
                }

                _logger.LogInformation("SSRF check passed for host {Host}", uri.DnsSafeHost);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SSRF check failed for host {Host} with an exception.", uri.DnsSafeHost);
                return false;
            }
        }

        private bool IsPrivate(IPAddress ipAddress)
        {
            var bytes = ipAddress.GetAddressBytes();
            switch (bytes[0])
            {
                case 10:
                    return true;
                case 172:
                    return bytes[1] >= 16 && bytes[1] < 32;
                case 192:
                    return bytes[1] == 168;
                default:
                    return false;
            }
        }

        public async Task<List<StudioSection>> ScrapeUrlAsync(string url)
        {
            var sections = new List<StudioSection>();
            if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                _logger.LogWarning("Invalid URL provided for scraping: {Url}", url);
                return sections;
            }

            if (!await IsUrlPubliclyRoutable(uri))
            {
                throw new ArgumentException("The provided URL must resolve to a public IP address for security reasons.");
            }

            try
            {
                _logger.LogInformation("Starting intelligent scrape of URL: {Url}", url);

                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync();
                var page = await browser.NewPageAsync();
                await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

                var html = await page.ContentAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // --- Text Normalization: Remove inline styles and unwanted tags ---
                var nodesWithStyle = doc.DocumentNode.SelectNodes("//*[@style]");
                if (nodesWithStyle != null)
                {
                    foreach (var node in nodesWithStyle)
                    {
                        node.Attributes.Remove("style");
                    }
                }
                // Optionally remove other tags like <font>, <b>, <i> if desired
                // This example focuses on inline styles as requested.

                var contentNodes = doc.DocumentNode.SelectNodes("//body//*[self::h1 or self::h2 or self::h3 or self::p or self::img or self::div[count(img) > 2]]");

                if (contentNodes == null)
                {
                    _logger.LogWarning("No mappable content (headings, p, img, image divs) found at URL: {Url}", url);
                    return sections;
                }

                foreach (var node in contentNodes)
                {
                    StudioSection newSection = null;

                    // --- Intelligent Block Mapping ---
                    if (node.Name == "div" && node.SelectNodes(".//img")?.Count > 2)
                    {
                        // Map to ImageGallery
                        var imageUrls = new List<string>();
                        foreach (var imgNode in node.SelectNodes(".//img"))
                        {
                            var src = imgNode.GetAttributeValue("src", "");
                            if (!string.IsNullOrEmpty(src))
                            {
                                var absoluteSrc = new Uri(uri, src).ToString();
                                // In a real scenario, you'd download these. For now, we'll just store the URLs.
                                imageUrls.Add(absoluteSrc);
                            }
                        }
                        if (imageUrls.Any())
                        {
                            newSection = new StudioSection
                            {
                                Title = "Image Gallery",
                                ComponentType = StudioComponentType.ImageGallery,
                                PageIndex = sections.Count,
                                properties = new Dictionary<string, object> { { "images", imageUrls } }
                            };
                            newSection.SyncPropertiesToData();
                        }
                    }
                    else if (node.Name == "img")
                    {
                        var src = node.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(src))
                        {
                            var absoluteSrc = new Uri(uri, src).ToString();
                            string localImagePath = "";
                            bool isBroken = false;

                            // --- Media Recovery ---
                            try
                            {
                                localImagePath = await _mediaStorageService.SaveImageFromUrlAsync(absoluteSrc);
                                if (string.IsNullOrEmpty(localImagePath))
                                {
                                    isBroken = true;
                                    _logger.LogWarning("Failed to save image from {ImageUrl}, flagging as broken.", absoluteSrc);
                                }
                            }
                            catch (Exception imgEx)
                            {
                                isBroken = true;
                                _logger.LogError(imgEx, "Exception while saving image from {ImageUrl}, flagging as broken.", absoluteSrc);
                            }

                            var altText = node.GetAttributeValue("alt", "");
                            var title = !string.IsNullOrWhiteSpace(altText) ? altText : "Scraped Image";

                            newSection = new StudioSection
                            {
                                Title = title,
                                ComponentType = StudioComponentType.Image,
                                PageIndex = sections.Count,
                                properties = new Dictionary<string, object>
                                {
                                    { "src", localImagePath },
                                    { "alt", altText },
                                    { "isBroken", isBroken } // Flag for the UI
                                }
                            };
                            newSection.SyncPropertiesToData();
                        }
                    }
                    else if (node.Name.StartsWith("h"))
                    {
                        newSection = new StudioSection
                        {
                            Title = node.InnerText.Trim(),
                            Content = node.OuterHtml,
                            ComponentType = StudioComponentType.Heading,
                            PageIndex = sections.Count
                        };
                    }
                    else if (node.Name == "p")
                    {
                        // Map to RichTextBlock for normalized content
                        newSection = new StudioSection
                        {
                            Title = "Text Block",
                            Content = node.InnerHtml, // Use InnerHtml to preserve links etc., but OuterHtml for headings
                            ComponentType = StudioComponentType.RichTextBlock,
                            PageIndex = sections.Count
                        };
                    }

                    if (newSection != null)
                    {
                        sections.Add(newSection);
                    }
                }
                _logger.LogInformation("Successfully performed intelligent scrape of {Url}, found {Count} sections.", url, sections.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during intelligent scrape of {Url}", url);
                throw;
            }

            return sections;
        }
    }
}
