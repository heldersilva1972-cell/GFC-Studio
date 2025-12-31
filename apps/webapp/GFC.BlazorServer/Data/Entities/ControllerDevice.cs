using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("Controllers")]
public class ControllerDevice
{
    public const int SimulatedControllerId = -1;
    private const uint SimulatedSerialValue = 0;

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public uint SerialNumber { get; set; }

    [Required, MaxLength(100)]
    public string IpAddress { get; set; } = string.Empty;
    public int DoorCount { get; set; } = 4; // Default to 4 doors

    public int Port { get; set; } = 60000;
    


    public bool IsEnabled { get; set; } = true;

    public ICollection<Door> Doors { get; set; } = new List<Door>();

    public ICollection<ControllerEvent> Events { get; set; } = new List<ControllerEvent>();

    public ControllerLastIndex? LastIndex { get; set; }

    /// <summary>
    /// Indicates this controller is runtime-only and should not persist.
    /// </summary>
    [NotMapped]
    public bool IsSimulated { get; set; }

    /// <summary>
    /// Friendly serial number text used for UI and simulated clients.
    /// </summary>
    [NotMapped]
    public string SerialNumberDisplay => SerialNumber.ToString();

    /// <summary>
    /// Returns the numeric serial used for comparisons when a simulated controller is needed.
    /// </summary>
    public static uint GetSimulatedSerialValue() => SimulatedSerialValue;

    /// <summary>
    /// Optional model label for UI context (non-persistent).
    /// </summary>
    [NotMapped]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Optional online indicator for UI context (non-persistent).
    /// </summary>
    [NotMapped]
    public bool IsOnline { get; set; }
}

