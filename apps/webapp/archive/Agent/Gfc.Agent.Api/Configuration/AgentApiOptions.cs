using System;
using System.Collections.Generic;

namespace Gfc.Agent.Api.Configuration;

public sealed class AgentApiOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public double RequestTimeoutSeconds { get; set; } = 10;

    public TimeSpan RequestTimeout => TimeSpan.FromSeconds(RequestTimeoutSeconds <= 0 ? 10 : RequestTimeoutSeconds);

    public IList<ControllerEndpointOptions> Controllers { get; set; } = new List<ControllerEndpointOptions>();
}

public sealed class ControllerEndpointOptions
{
    public uint SerialNumber { get; set; }

    public string IpAddress { get; set; } = "127.0.0.1";

    public int UdpPort { get; set; } = 60000;

    public int TcpPort { get; set; } = 60000;

    public string? CommPassword { get; set; }
}

