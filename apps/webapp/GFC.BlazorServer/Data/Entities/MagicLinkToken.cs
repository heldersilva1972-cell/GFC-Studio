using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.BlazorServer.Data.Entities;

public class MagicLinkToken
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Token { get; set; } = string.Empty;
    
    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime ExpiresAtUtc { get; set; }
    
    public bool IsUsed { get; set; }
    
    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;
}
