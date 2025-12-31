using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

/// <summary>
///     Represents a single privilege row stored on the controller.
/// </summary>
public sealed class CardPrivilegeModel
{
    public long CardNumber { get; set; }

    public byte DoorMask { get; set; }

    public IReadOnlyCollection<int> DoorList { get; set; } = Array.Empty<int>();

    public IReadOnlyList<byte> TimeZones { get; set; } = Array.Empty<byte>();

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public CardPrivilegeFlags Flags { get; set; } = CardPrivilegeFlags.Normal;

    public string? HolderName { get; set; }
}


