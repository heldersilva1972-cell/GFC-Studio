namespace GFC.BlazorServer.Connectors.Mengqi.Models;

/// <summary>
/// Hardware-level configuration for a single door on a controller.
/// </summary>
public sealed class DoorHardwareConfig
{
    public int DoorIndex { get; set; }
    
    /// <summary>
    /// 0x01 = Controlled, 0x02 = Always Open, 0x03 = Always Locked
    /// </summary>
    public DoorControlMode ControlMode { get; set; }
    
    /// <summary>
    /// Unlock Duration in seconds (0-255)
    /// </summary>
    public byte UnlockDuration { get; set; }
    
    /// <summary>
    /// 0x00 = None, 0x01 = Interlocked
    /// </summary>
    public DoorInterlockMode Interlock { get; set; }

    /// <summary>
    /// 0x00 = None, 0x01 = NC, 0x02 = NO
    /// </summary>
    public byte SensorType { get; set; }

    /// <summary>
    /// Seconds before the alarm sounds if the door is held open.
    /// </summary>
    public byte DoorAjarTimeout { get; set; }

    /// <summary>
    /// 0x01 = Card Only, 0x02 = Card + PIN, 0x03 = PIN Only
    /// </summary>
    public DoorVerificationMode Verification { get; set; }
}

public enum DoorControlMode : byte
{
    Controlled = 0x01,
    AlwaysOpen = 0x02,
    AlwaysLocked = 0x03
}

public enum DoorInterlockMode : byte
{
    None = 0x00,
    Interlocked = 0x01
}

public enum DoorVerificationMode : byte
{
    CardOnly = 0x01,
    CardAndPin = 0x02,
    PinOnly = 0x03
}
