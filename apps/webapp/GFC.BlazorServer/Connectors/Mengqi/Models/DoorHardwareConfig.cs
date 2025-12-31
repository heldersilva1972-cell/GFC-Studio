namespace GFC.BlazorServer.Connectors.Mengqi.Models;

/// <summary>
/// Hardware-level configuration for a single door on a controller.
/// </summary>
public sealed class DoorHardwareConfig
{
    public int DoorIndex { get; set; }
    
    /// <summary>
    /// 0=Disabled, 1=AlwaysOpen, 2=AlwaysClosed, 3=Controlled
    /// </summary>
    public byte ControlMode { get; set; }
    
    /// <summary>
    /// Full seconds (1-255)
    /// </summary>
    public byte RelayDelay { get; set; }
    
    /// <summary>
    /// 0=None, 1=NC, 2=NO
    /// </summary>
    public byte SensorType { get; set; }
    
    /// <summary>
    /// 0=None, 1=2Door, 2=3Door, 3=4Door
    /// </summary>
    public byte Interlock { get; set; }
}
