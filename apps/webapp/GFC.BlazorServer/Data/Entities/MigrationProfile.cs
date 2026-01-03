using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

public enum MigrationMode
{
    Guided = 0,
    Strict = 1
}

public class MigrationProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public MigrationMode Mode { get; set; } = MigrationMode.Guided;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public bool IsCompleted { get; set; }

    /// <summary>
    /// JSON string storing the state of each gate (e.g., {"VPN": true, "Backup": false})
    /// </summary>
    public string GatesStatusJson { get; set; } = "{}";

    public string? ReportContentTxt { get; set; }
    public DateTime? ReportGeneratedAtUtc { get; set; }
}

public class GateStatus
{
    public bool IsAutomated { get; set; }
    public bool Passed { get; set; }
    public DateTime? LastCheckedUtc { get; set; }
    public string? Message { get; set; }
}
