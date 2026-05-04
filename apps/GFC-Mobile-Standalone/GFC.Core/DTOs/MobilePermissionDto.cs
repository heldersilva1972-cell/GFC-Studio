namespace GFC.Core.DTOs;

public class MobilePermissionDto
{
    public int PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public string PageRoute { get; set; } = string.Empty;
    public string? Category { get; set; }
    public bool CanAccess { get; set; } = true;
    public bool CanEdit { get; set; }
    public bool ReceivePush { get; set; }
}
