// [NEW]
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities.Security;

[Table("TrustedDevices")]
[Index(nameof(DeviceToken), IsUnique = true)]
public class TrustedDevice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;

    [Required]
    [MaxLength(128)]
    public string DeviceToken { get; set; } = null!;

    [MaxLength(256)]
    public string? UserAgent { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }

    public DateTime LastUsedUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public bool IsRevoked { get; set; }
}
