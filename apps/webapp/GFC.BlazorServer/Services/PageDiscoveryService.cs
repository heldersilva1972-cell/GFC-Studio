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
                    // Brand new page found in code - add it
                    _pagePermissionRepository.AddPage(page);
                    added++;
                }
                else 
                {
                    bool changed = false;
                    if (!existing.IsActive)
                    {
                        existing.IsActive = true;
                        changed = true;
                    }

                    // [REORG] Explicitly move specific pages to requested categories
                    // This overrides whatever might be in the database or namespace mapping
                    string targetedCategory = GetExplicitCategoryOverride(page.PageRoute) ?? page.Category;

                    // AUTO-FIX CATEGORY: If the existing page is in DASHBOARD or has no category,
                    // OR if we have a targeted override, update it.
                    if (string.IsNullOrEmpty(existing.Category) || 
                        existing.Category == "DASHBOARD" || 
                        existing.Category == "UNSPECIFIED" ||
                        targetedCategory != page.Category) // page.Category here is from DetermineCategory...
                    {
                        if (!string.IsNullOrEmpty(targetedCategory) && existing.Category != targetedCategory)
                        {
                            existing.Category = targetedCategory;
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        _pagePermissionRepository.UpdatePage(existing);
                        updated++;
                    }
                }
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
    /// Returns explicit category overrides for specific routes to ensure menu organization
    /// matches user expectations regardless of file location.
    /// </summary>
    private string? GetExplicitCategoryOverride(string route)
    {
        var normalized = route.TrimStart('/').ToLowerInvariant();

        // [MEMBERSHIP] - Root pages or misc routes
        if (normalized == "members" || normalized == "dues" || normalized == "keycards" || 
            normalized == "life-eligibility" || normalized == "np-queue" || normalized == "bylaws" ||
            normalized == "directors" || normalized == "physicalkeys")
            return "MEMBERSHIP";

        // [FINANCE]
        if (normalized == "admin/bar-sales" || normalized == "reimbursements/manage" || 
            normalized == "finance/insights" || normalized == "finance/lottery-analytics" || 
            normalized == "finance/bar-lottery-sales")
            return "FINANCE";

        // [BARTENDERS]
        if (normalized == "operations/end-of-shift" || normalized == "admin/staff-shifts" || 
            normalized == "mobile/manage-schedule" || normalized == "mobile/liquor/manage")
            return "BARTENDERS";

        // [WEBSITE]
        if (normalized == "admin/pages" || normalized == "admin/reviews" || normalized == "admin/event-promotions" || 
            normalized == "admin/nav-menu-editor" || normalized == "admin/website-settings" || 
            normalized == "admin/form-builder" || normalized == "admin/form-submissions" || 
            normalized == "controllers/schedules/specialevents")
            return "WEBSITE";

        // [HALL RENTALS]
        if (normalized == "admin/hall-management" || normalized == "admin/hall-rental-settings")
            return "HALL RENTALS";

        // [SYSTEM]
        if (normalized == "admin/operations" || normalized == "admin/system/communications" || 
            normalized == "admin/system/alerts")
            return "SYSTEM";

        // [LIQUOR]
        if (normalized.StartsWith("liquor/") || normalized == "liquor" || normalized == "mobile/liquor/manage")
            return "LIQUOR";

        // [MOBILE]
        if (normalized == "mobile" || normalized == "hub" || normalized.StartsWith("mobile/"))
        {
            if (normalized == "mobile/manage-schedule")
                return "BARTENDERS";
            return "MOBILE";
        }

        return null;
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
            
            // Determine category from namespace and route
            var category = GetExplicitCategoryOverride(route) ?? DetermineCategoryFromNamespace(type.Namespace, route);
            
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

    private string DetermineCategoryFromNamespace(string? ns, string route)
    {
        if (ns == null) return "DASHBOARD";

        var normalizedRoute = route.TrimStart('/').ToLowerInvariant();
        var upperNs = ns.ToUpperInvariant();

        // Heuristic 1: Explicit Route Markers (Highest Priority after Overrides)
        if (normalizedRoute.StartsWith("admin/"))
        {
            if (normalizedRoute.Contains("hall")) return "HALL RENTALS";
            if (normalizedRoute.Contains("website") || normalizedRoute.Contains("page")) return "WEBSITE";
            if (normalizedRoute.Contains("operation") || normalizedRoute.Contains("system")) return "SYSTEM";
            return "ADMINISTRATION";
        }

        if (normalizedRoute.StartsWith("members") || normalizedRoute.StartsWith("dues") || 
            normalizedRoute.StartsWith("directors") || normalizedRoute.StartsWith("keycards"))
            return "MEMBERSHIP";

        if (normalizedRoute.StartsWith("finance/") || normalizedRoute.StartsWith("finance-"))
            return "FINANCE";

        if (normalizedRoute.StartsWith("cameras/"))
            return "CAMERA SYSTEM";

        if (normalizedRoute.StartsWith("controllers/"))
            return "CONTROLLERS";

        // Heuristic 2: Namespace Folders
        if (upperNs.Contains("PAGES.ADMIN") || upperNs.Contains("PAGES.USERS")) 
            return "ADMINISTRATION";
            
        if (upperNs.Contains("PAGES.MEMBERS") || upperNs.Contains("PAGES.MEMBERSHIP") || upperNs.Contains("PAGES.DIRECTORS")) 
            return "MEMBERSHIP";
            
        if (upperNs.Contains("PAGES.CONTROLLERS") || upperNs.Contains("PAGES.HARDWARE")) 
            return "CONTROLLERS";
            
        if (upperNs.Contains("PAGES.FINANCE") || upperNs.Contains("PAGES.SALES") || upperNs.Contains("PAGES.REIMBURSEMENTS")) 
            return "FINANCE";

        if (upperNs.Contains("PAGES.BARTENDERS") || upperNs.Contains("PAGES.OPERATIONS")) 
            return "BARTENDERS";
            
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

        if (upperNs.Contains("PAGES.MOBILE")) 
            return "MOBILE";

        // Fallback for root pages - if it's in the root GFC.BlazorServer.Components.Pages
        // and didn't match anything above, it's likely a Membership page or Dashboard.
        if (ns == "GFC.BlazorServer.Components.Pages")
        {
            if (normalizedRoute == "" || normalizedRoute == "dashboard" || normalizedRoute == "home")
                return "DASHBOARD";
            return "MEMBERSHIP";
        }

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
        if (category == "ADMINISTRATION" || category == "SYSTEM")
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
