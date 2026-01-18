using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Models;

namespace GFC.BlazorServer.Data.Entities;

[Table("DeviceInviteTokens")]
public class DeviceInviteToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(128)]
    public string Token { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    public AppUser? User { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; }

    public bool IsRevoked { get; set; }

    [MaxLength(100)]
    public string? TargetDeviceName { get; set; }

    public bool IsValid => !IsRevoked && UsedAtUtc == null && ExpiresAtUtc > DateTime.UtcNow;
}
