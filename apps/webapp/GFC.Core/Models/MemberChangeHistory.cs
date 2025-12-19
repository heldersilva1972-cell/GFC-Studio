namespace GFC.Core.Models;

/// <summary>
/// Represents an audit entry tracking a single change to a member record.
/// </summary>
public class MemberChangeHistory
{
    public DateTime ChangeDate { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? ChangedBy { get; set; }
}

