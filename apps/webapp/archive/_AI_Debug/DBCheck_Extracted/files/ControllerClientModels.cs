using System.Text.Json.Serialization;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Represents a card privilege targeted to a specific controller/door.
/// </summary>
public sealed class CardPrivilegeModel
{
    /// <summary>
    /// Target controller identifier from the database.
    /// </summary>
    [JsonIgnore]
    public int ControllerId { get; set; }

    /// <summary>
    /// Optional door record identifier; used to resolve the door index when not provided.
    /// </summary>
    [JsonIgnore]
    public int? DoorId { get; set; }

    /// <summary>
    /// Optional explicit door index (1-4). If not supplied, the door index is resolved from DoorId.
    /// </summary>
    public int? DoorIndex { get; set; }

    public long CardNumber { get; set; }

    public int? TimeProfileIndex { get; set; }

    public bool Enabled { get; set; } = true;

    public DateTime? ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }
}

/// <summary>
/// Minimal runtime status returned from controllers.
/// </summary>
public sealed class RunStatusModel
{
    public bool IsOnline { get; set; }

    public DateTime? ControllerTimeUtc { get; set; }

    public uint? LastRecordIndex { get; set; }

    public IReadOnlyList<DoorStatus> Doors { get; set; } = Array.Empty<DoorStatus>();

    public sealed class DoorStatus
    {
        public int DoorIndex { get; set; }
        public bool IsDoorOpen { get; set; }
        public bool IsRelayOn { get; set; }
        public bool IsSensorActive { get; set; }
    }
}

/// <summary>
/// Minimal event log item returned from controllers.
/// </summary>
public sealed class EventLogModel
{
    public long Index { get; set; }

    public DateTime TimestampUtc { get; set; }

    public int ControllerId { get; set; }

    public int? DoorId { get; set; }

    public int? DoorNumber { get; set; }

    public long? CardNumber { get; set; }

    public int EventType { get; set; }

    public int? ReasonCode { get; set; }

    public bool IsByCard { get; set; }

    public bool IsByButton { get; set; }
}
