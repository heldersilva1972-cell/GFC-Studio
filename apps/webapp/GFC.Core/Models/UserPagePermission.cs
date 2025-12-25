using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

/// <summary>
/// Represents a user's permission to access a specific page.
/// </summary>
public class UserPagePermission
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public AppUser User { get; set; }

    [Required]
    public int PageId { get; set; }

    [ForeignKey("PageId")]
    public AppPage Page { get; set; }

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string GrantedBy { get; set; }
}
