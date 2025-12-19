namespace GFC.BlazorServer.Models;

/// <summary>
/// DTO for displaying member door access information.
/// </summary>
public class MemberDoorAccessDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public int DoorId { get; set; }
    public string DoorName { get; set; } = string.Empty;
    public int ControllerId { get; set; }
    public string ControllerName { get; set; } = string.Empty;
    public uint ControllerSerialNumber { get; set; }
    public int? TimeProfileId { get; set; }
    public string? TimeProfileName { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public string? LastSyncResult { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public bool IsEligible { get; set; }
}

/// <summary>
/// DTO for updating member door access.
/// </summary>
public class MemberDoorAccessUpdateDto
{
    public int DoorId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int? TimeProfileId { get; set; }
}

/// <summary>
/// Request DTO for adding or updating a card on a controller.
/// </summary>
public class AddOrUpdateCardRequestDto
{
    public string CardNumber { get; set; } = string.Empty;
    public int DoorIndex { get; set; }
    public int? TimeProfileIndex { get; set; }
    public bool Enabled { get; set; } = true;
}
