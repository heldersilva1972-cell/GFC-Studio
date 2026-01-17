using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IPagePermissionRepository
{
    // Page management
    IEnumerable<AppPage> GetAllPages();
    IEnumerable<AppPage> GetActivePages();
    IEnumerable<AppPage> GetPagesByCategory(string category);
    AppPage? GetPageById(int pageId);
    AppPage? GetPageByRoute(string route);
    void AddPage(AppPage page);
    void UpdatePage(AppPage page);
    
    // Permission management
    IEnumerable<UserPagePermission> GetUserPermissions(int userId);
    IEnumerable<UserPagePermission> GetPagePermissions(int pageId);
    UserPagePermission? GetPermission(int userId, int pageId);
    bool HasPermission(int userId, string pageRoute);
    
    // CRUD operations
    void GrantPermission(int userId, int pageId, string grantedBy);
    void RevokePermission(int userId, int pageId);
    void SetUserPermissions(int userId, IEnumerable<int> pageIds, string grantedBy);
    void ClearUserPermissions(int userId);
    
    // Bulk operations
    void GrantAllPermissions(int userId, string grantedBy);
    void CopyPermissions(int sourceUserId, int targetUserId, string grantedBy);

    // Default Permissions
    IEnumerable<int> GetDefaultPageIds();
    void SetDefaultPageIds(IEnumerable<int> pageIds);
}
