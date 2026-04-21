namespace GFC.Core.DTOs;

/// <summary>
/// Parameters for querying members for list/grid views.
/// </summary>
public class MemberFilterOptions
{
    public string? SearchText { get; set; }
    public string? Status { get; set; }
    public bool? NonPortugueseOnly { get; set; }
}

