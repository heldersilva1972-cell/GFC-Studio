namespace GFC.Core.DTOs;

/// <summary>
/// DTO representing a pending EF Core migration.
/// </summary>
public class PendingMigrationDto
{
    /// <summary>
    /// Migration ID/name.
    /// </summary>
    public string MigrationId { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description if available.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether this migration appears to be destructive (drop table/column).
    /// </summary>
    public bool IsDestructive { get; set; }

    /// <summary>
    /// Estimated risk level: "Low", "Medium", "High"
    /// </summary>
    public string RiskLevel { get; set; } = "Low";
}
