using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services.Models;

public class ExtendedDoorConfigWriteDto
{
    public IReadOnlyList<DoorExtendedConfig> Doors { get; set; } = Array.Empty<DoorExtendedConfig>();

    public sealed class DoorExtendedConfig
    {
        public int DoorNumber { get; set; }
        public byte LockDelaySeconds { get; set; }
        public bool NormallyOpenMode { get; set; }
        public bool DoubleLock { get; set; }
        public bool AllowButtonOpen { get; set; }
    }
}

public class ExtendedConfigDto
{
    public byte FirmwareMajor { get; init; }
    public byte FirmwareMinor { get; init; }
    public byte BoardType { get; init; }
    public bool AntiPassbackEnabled { get; init; }
    public bool InterlockEnabled { get; init; }
    public IReadOnlyList<DoorExtendedConfig> Doors { get; init; } = Array.Empty<DoorExtendedConfig>();

    public sealed class DoorExtendedConfig
    {
        public int DoorNumber { get; init; }
        public byte LockDelaySeconds { get; init; }
        public bool NormallyOpenMode { get; init; }
        public bool DoubleLock { get; init; }
        public bool AllowButtonOpen { get; init; }
    }
}

