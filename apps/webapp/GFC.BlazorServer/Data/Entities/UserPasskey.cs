using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Models;

namespace GFC.BlazorServer.Data.Entities;

[Table("UserPasskeys")]
public class UserPasskey
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public AppUser? User { get; set; }

    [Required]
    [MaxLength(500)]
    public string CredentialId { get; set; } = null!;

    [Required]
    public string PublicKey { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string UserHandle { get; set; } = null!;

    public int SignatureCounter { get; set; }

    [MaxLength(50)]
    public string? AttestationFormat { get; set; }

    [MaxLength(200)]
    public string? FriendlyName { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? LastUsedUtc { get; set; }

    [MaxLength(200)]
    public string? DeviceDisplayName { get; set; }

    public Guid? AAGUID { get; set; }
}
