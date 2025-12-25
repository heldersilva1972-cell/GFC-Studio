// [MODIFIED]
// NOTE: This service uses Playwright for browser automation.
// The target deployment environment must have Playwright's browser binaries installed.
// Run `pwsh bin/Debug/netX/playwright.ps1 install` in the project directory.
using GFC.Core.Models;
using HtmlAgilityPack;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class ContentIngestionService : IContentIngestionService
    {
        private readonly ILogger<ContentIngestionService> _logger;
        private readonly IMediaStorageService _mediaStorageService;

        public ContentIngestionService(ILogger<ContentIngestionService> logger, IMediaStorageService mediaStorageService)
        {
            _logger = logger;
            _mediaStorageService = mediaStorageService;
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
                _logger.LogInformation("Attempting to scrape URL with Playwright: {Url}", url);

                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync();
                var page = await browser.NewPageAsync();
                await page.GotoAsync(url);

                var html = await page.ContentAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var nodes = doc.DocumentNode.SelectNodes("//h1|//h2|//h3|//p|//img");

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        StudioSection newSection = null;
                        if (node.Name == "img")
                        {
                            var src = node.GetAttributeValue("src", "");
                            if (!string.IsNullOrEmpty(src))
                            {
                                var absoluteSrc = new Uri(uri, src).ToString();
                                var localImagePath = await _mediaStorageService.SaveImageFromUrlAsync(absoluteSrc);

                                if (!string.IsNullOrEmpty(localImagePath))
                                {
                                    var altText = node.GetAttributeValue("alt", "");
                                    var title = !string.IsNullOrWhiteSpace(altText)
                                        ? altText
                                        : Path.GetFileNameWithoutExtension(new Uri(absoluteSrc).AbsolutePath);

                                    newSection = new StudioSection
                                    {
                                        Title = title,
                                        Content = localImagePath,
                                        ComponentType = StudioComponentType.Image,
                                        PageIndex = sections.Count
                                    };
                                }
                                else
                                {
                                    _logger.LogWarning("Failed to download and save image from {ImageUrl}", absoluteSrc);
                                }
                            }
                        }
                        else // Text-based nodes
                        {
                            var innerText = node.InnerText.Trim();
                            var title = node.Name.StartsWith("h")
                                ? innerText
                                : string.Join(" ", innerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Take(5)) + "...";

                            newSection = new StudioSection
                            {
                                Title = title,
                                Content = node.OuterHtml,
                                ComponentType = node.Name.StartsWith("h") ? StudioComponentType.Heading : StudioComponentType.TextBlock,
                                PageIndex = sections.Count
                            };
                        }

                        if (newSection != null)
                        {
                            sections.Add(newSection);
                        }
                    }
                    _logger.LogInformation("Successfully scraped {Count} sections from {Url}", sections.Count, url);
                }
                else
                {
                    _logger.LogWarning("No h1, h2, h3, p, or img tags found at URL: {Url}", url);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while scraping {Url} with Playwright", url);
                throw;
            }

            return sections;
        }
    }
}
