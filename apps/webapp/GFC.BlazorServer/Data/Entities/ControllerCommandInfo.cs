using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ControllerCommandInfos")]
public class ControllerCommandInfo
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Phase { get; set; } = string.Empty;

    [MaxLength(250)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string? LongDescription { get; set; }

    public int RiskLevel { get; set; }

    public bool IsReadOperation { get; set; }

    public bool IsWriteOperation { get; set; }

    [MaxLength(500)]
    public string? ProtocolInfo { get; set; }

    public bool Enabled { get; set; } = true;
}

