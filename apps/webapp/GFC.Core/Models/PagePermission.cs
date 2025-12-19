namespace GFC.Core.Models;

/// <summary>
/// Represents a page/route in the application that can have access controlled
/// </summary>
public class AppPage
{
    public int PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public string PageRoute { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool RequiresAdmin { get; set; } // If true, only admins can access regardless of permissions
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Represents a user's permission to access a specific page
/// </summary>
public class UserPagePermission
{
    public int PermissionId { get; set; }
    public int UserId { get; set; }
    public int PageId { get; set; }
    public bool CanAccess { get; set; }
    public DateTime GrantedDate { get; set; }
    public string? GrantedBy { get; set; }
    
    // Navigation properties
    public AppUser? User { get; set; }
    public AppPage? Page { get; set; }
}
