// [NEW]
namespace GFC.Core.Interfaces;

/// <summary>
/// Service for handling legacy content migration and data export.
/// </summary>
public interface IMigrationService
{
    /// <summary>
    /// Starts the process of scraping content from the legacy website.
    /// </summary>
    Task StartLegacyScrapeAsync();

    /// <summary>
    /// Exports the entire site to a static HTML format.
    /// </summary>
    Task ExportStaticSiteAsync();

    /// <summary>
    /// Generates 301 redirects for old URLs.
    /// </summary>
    Task GenerateRedirectsAsync();
}
