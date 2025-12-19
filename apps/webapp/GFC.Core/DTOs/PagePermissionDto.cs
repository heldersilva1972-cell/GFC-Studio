namespace GFC.Core.DTOs;

/// <summary>
/// DTO for displaying page permissions in the UI
/// </summary>
public class PagePermissionDto
{
    public int PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public string PageRoute { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool RequiresAdmin { get; set; }
    public bool HasAccess { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO for user with page permissions summary
/// </summary>
public class UserWithPermissionsDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public int PermissionCount { get; set; }
    public List<string> PermittedPages { get; set; } = new();
}
