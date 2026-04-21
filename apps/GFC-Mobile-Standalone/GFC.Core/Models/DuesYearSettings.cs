namespace GFC.Core.Models;

/// <summary>
/// Represents the standard dues configuration for a specific year.
/// </summary>
public class DuesYearSettings
{
    public int Year { get; set; }
    public decimal StandardDues { get; set; }
    public bool GraceEndApplied { get; set; }
    public DateTime? GraceEndDate { get; set; }
}




