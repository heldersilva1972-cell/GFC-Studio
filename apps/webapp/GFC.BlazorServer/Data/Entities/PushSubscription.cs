using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Models;

namespace GFC.BlazorServer.Data.Entities;

[Table("PushSubscriptions")]
public class PushSubscription
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public AppUser? User { get; set; }

    [Required]
    public string Endpoint { get; set; } = string.Empty;

    [Required]
    public string P256dh { get; set; } = string.Empty;

    [Required]
    public string Auth { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? DeviceName { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
