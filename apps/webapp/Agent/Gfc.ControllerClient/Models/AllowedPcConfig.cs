using System;
using System.Collections.Generic;
using System.Net;

namespace Gfc.ControllerClient.Models;

public sealed class AllowedPcConfig
{
    public IReadOnlyList<IPAddress> AllowedIPs { get; set; } = Array.Empty<IPAddress>();

    public string? CommunicationPassword { get; set; }

    public string? LegacyUsername { get; set; }

    public string? LegacyPassword { get; set; }
}

