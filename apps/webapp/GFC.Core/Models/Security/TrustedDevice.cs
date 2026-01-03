// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models.Security;

public class TrustedDevice
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public AppUser? User { get; set; }

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
