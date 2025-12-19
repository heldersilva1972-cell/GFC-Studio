using System;
using System.Linq;
using System.Net;
using Gfc.Agent.Api.Configuration;
using Gfc.ControllerClient.Abstractions;
using Gfc.ControllerClient.Configuration;
using Microsoft.Extensions.Options;

namespace Gfc.Agent.Api.Services;

internal sealed class AppSettingsControllerEndpointResolver : IControllerEndpointResolver
{
    private readonly IOptionsMonitor<AgentApiOptions> _options;

    public AppSettingsControllerEndpointResolver(IOptionsMonitor<AgentApiOptions> options)
    {
        _options = options;
    }

    public ControllerEndpoint Resolve(uint controllerSn)
    {
        var config = _options.CurrentValue.Controllers.FirstOrDefault(c => c.SerialNumber == controllerSn);
        if (config is null)
        {
            throw new InvalidOperationException($"Controller {controllerSn} is not configured on this Agent.");
        }

        if (!IPAddress.TryParse(config.IpAddress, out var ip))
        {
            throw new InvalidOperationException($"Configured IP '{config.IpAddress}' for controller {controllerSn} is invalid.");
        }

        return new ControllerEndpoint(ip, config.UdpPort, config.TcpPort, config.CommPassword);
    }
}

