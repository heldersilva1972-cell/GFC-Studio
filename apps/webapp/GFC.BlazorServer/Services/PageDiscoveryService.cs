using System.Reflection;
using Microsoft.AspNetCore.Components;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service that automatically discovers and synchronizes Razor pages with the AppPages database table.
/// Runs on application startup to ensure the permission system always has the latest page list.
/// </summary>
public class PageDiscoveryService
{
    private readonly IPagePermissionRepository _pagePermissionRepository;
    private readonly ILogger<PageDiscoveryService> _logger;

    public PageDiscoveryService(
        IPagePermissionRepository pagePermissionRepository,
        ILogger<PageDiscoveryService> logger)
    {
        _pagePermissionRepository = pagePermissionRepository;
        _logger = logger;
    }

    /// <summary>
    /// Discovers all Razor pages in the application and syncs them with the database.
    /// Should be called during application startup.
    /// </summary>
    public async Task DiscoverAndSyncPagesAsync()
    {
        _logger.LogInformation("Starting page discovery and synchronization...");

        try
        {
            var discoveredPages = DiscoverRazorPages();
            var existingPages = _pagePermissionRepository.GetAllPages().ToList();

            int added = 0, updated = 0, deactivated = 0;

            foreach (var page in discoveredPages)
            {
                var existing = existingPages.FirstOrDefault(p => p.PageRoute == page.PageRoute);
                
                if (existing == null)
                {
                    // Brand new page found in code - add it as a placeholder
                    page.Category = "UNSPECIFIED";
                    _pagePermissionRepository.AddPage(page);
                    added++;
                }
                else if (!existing.IsActive)
                {
                    // Just reactivate it if it was deleted
                    existing.IsActive = true;
                    _pagePermissionRepository.UpdatePage(existing);
                }
                
                // WE NO LONGER OVERWRITE NAMES OR CATEGORIES HERE.
                // The database is now the master record.
            }

            // Deactivate pages that no longer exist in the codebase
            var discoveredRoutes = discoveredPages.Select(p => p.PageRoute).ToHashSet();
            var removedPages = existingPages.Where(p => !discoveredRoutes.Contains(p.PageRoute) && p.IsActive);
            
            foreach (var removed in removedPages)
            {
                removed.IsActive = false;
                _pagePermissionRepository.UpdatePage(removed);
                deactivated++;
                _logger.LogWarning($"Deactivated removed page: {removed.PageName} ({removed.PageRoute})");
            }

            _logger.LogInformation($"Page sync complete: {added} added, {updated} updated, {deactivated} deactivated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during page discovery and synchronization");
        }
    }

    /// <summary>
    /// Discovers all Razor pages in the application using reflection.
    /// </summary>
    private List<AppPage> DiscoverRazorPages()
    {
        var pages = new List<AppPage>();
        var assembly = Assembly.GetExecutingAssembly();

        // Find all types that are Razor components with @page directive
        var componentTypes = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ComponentBase)) && 
                       t.Namespace?.Contains("Components.Pages") == true);

        int displayOrder = 0;

        foreach (var type in componentTypes)
        {
            // Get the @page route from RouteAttribute
            var routeAttr = type.GetCustomAttribute<RouteAttribute>();
            if (routeAttr == null) continue;

            var route = routeAttr.Template;
            
            // Determine category from namespace
            var category = DetermineCategoryFromNamespace(type.Namespace);
            
            // Check if page requires admin (you can use a custom attribute for this)
            var requiresAdmin = DetermineIfRequiresAdmin(type, route, category);

            pages.Add(new AppPage
            {
                PageName = GetFriendlyPageName(type.Name),
                PageRoute = route,
                Description = GetPageDescription(type),
                Category = category,
                RequiresAdmin = requiresAdmin,
                IsActive = true,
                DisplayOrder = displayOrder++
            });
        }

        return pages.OrderBy(p => p.Category).ThenBy(p => p.DisplayOrder).ToList();
    }

    private string DetermineCategoryFromNamespace(string? ns)
    {
        if (ns == null) return "DASHBOARD";

        // Map namespace folders to the exact Sidebar Group Titles
        var upperNs = ns.ToUpperInvariant();

        if (upperNs.Contains("PAGES.ADMIN") || upperNs.Contains("PAGES.USERS")) 
            return "ADMINISTRATION";
            
        if (upperNs.Contains("PAGES.MEMBERS") || upperNs.Contains("PAGES.MEMBERSHIP") || upperNs.Contains("PAGES.DIRECTORS")) 
            return "MEMBERSHIP";
            
        if (upperNs.Contains("PAGES.CONTROLLERS") || upperNs.Contains("PAGES.HARDWARE")) 
            return "CONTROLLERS";
            
        if (upperNs.Contains("PAGES.FINANCE") || upperNs.Contains("PAGES.SALES") || upperNs.Contains("PAGES.REIMBURSEMENTS")) 
            return "FINANCE";
            
        if (upperNs.Contains("PAGES.WEBSITE") || upperNs.Contains("PAGES.CMS")) 
            return "WEBSITE";
            
        if (upperNs.Contains("PAGES.RENTALS") || upperNs.Contains("PAGES.HALL")) 
            return "HALL RENTALS";
            
        if (upperNs.Contains("PAGES.SYSTEM") || upperNs.Contains("PAGES.INFRASTRUCTURE")) 
            return "SYSTEM";
            
        if (upperNs.Contains("PAGES.CAMERA")) 
            return "CAMERA SYSTEM";
            
        if (upperNs.Contains("PAGES.STUDIO")) 
            return "GFC STUDIO";

        return "DASHBOARD";
    }

    private bool DetermineIfRequiresAdmin(Type type, string route, string category)
    {
        // Check for [Authorize(Policy = AppPolicies.RequireAdmin)] attribute
        var authorizeAttr = type.GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        if (authorizeAttr?.Policy == "RequireAdmin")
            return true;

        // Admin categories
        if (category == "Administration" || category == "System")
            return true;

        // Admin routes
        if (route.StartsWith("/admin/") || route.StartsWith("/users"))
            return true;

        return false;
    }

    private string GetFriendlyPageName(string typeName)
    {
        // Convert "MemberManagement" to "Member Management"
        return System.Text.RegularExpressions.Regex.Replace(typeName, "([a-z])([A-Z])", "$1 $2");
    }

    private string GetPageDescription(Type type)
    {
        // You could use a custom [PageDescription] attribute here
        // For now, return a generic description
        return $"{GetFriendlyPageName(type.Name)} page";
    }
}
