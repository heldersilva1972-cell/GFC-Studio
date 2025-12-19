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

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}

