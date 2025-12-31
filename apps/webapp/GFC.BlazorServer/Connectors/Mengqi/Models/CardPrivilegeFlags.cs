using System;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

[Flags]
public enum CardPrivilegeFlags : ushort
{
    None = 0,
    Normal = 1 << 0,
    Vip = 1 << 1,
    Patrol = 1 << 2,
    AntiPassback = 1 << 3,
    FirstCard = 1 << 4,
    DisableOpenButton = 1 << 5,
    Visitor = 1 << 6,
    Blacklisted = 1 << 7
}


