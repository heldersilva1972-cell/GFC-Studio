using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("VpnOnboardingTokens")]
public class VpnOnboardingToken
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(256)]
    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime ExpiresAtUtc { get; set; }
    
    public bool IsUsed { get; set; }
    
    [MaxLength(500)]
    public string? DeviceInfo { get; set; }
}
