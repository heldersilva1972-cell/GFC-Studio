using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Models;

namespace GFC.BlazorServer.Data.Entities;

[Table("UserDevices")]
public class UserDevice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public AppUser? User { get; set; }

    [Required]
    [MaxLength(256)]
    public string DeviceToken { get; set; } = string.Empty;

    [MaxLength(512)]
    public string? FcmDeviceToken { get; set; }

    [Required]
    [MaxLength(50)]
    public string Platform { get; set; } = "Android";

    [MaxLength(150)]
    public string? DeviceModel { get; set; }

    [MaxLength(50)]
    public string? OsVersion { get; set; }

    [MaxLength(50)]
    public string? AppVersion { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastActive { get; set; } = DateTime.UtcNow;
}
