using System.ComponentModel.DataAnnotations;

namespace GFC.BlazorServer.Models;

/// <summary>
/// [NEW] NVR Configuration Model for camera auto-discovery
/// </summary>
public class NvrConfig
{
    [Required]
    public string NvrIpAddress { get; set; } = "192.168.1.64";
    
    public int NvrPort { get; set; } = 8000;
    
    [Required]
    public string Username { get; set; } = "admin";
    
    [Required]
    public string Password { get; set; } = "";
}
