using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("DoorConfigs")]
public class DoorConfig
{
    public int Id { get; set; }

    // FK to Door
    public int DoorId { get; set; }
    public Door Door { get; set; } = null!;

    // The fields the UI expects
    public int OpenTimeSeconds { get; set; }

    public int LockDelaySeconds { get; set; }

    public bool AlarmEnabled { get; set; }
    
    // Hardware Parameters (0x8E command)
    public byte ControlMode { get; set; } = 3; // 1=AlwaysOpen, 2=AlwaysClosed, 3=Controlled
    public byte SensorType { get; set; } = 1;  // 0=None, 1=NC, 2=NO
    public byte Interlock { get; set; } = 0;   // 0=None, 1=2Door, 2=3Door, 3=4Door

    public DateTime CreatedUtc { get; set; }
    public DateTime? UpdatedUtc { get; set; }
}

