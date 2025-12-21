using System;
using Gfc.ControllerClient.Configuration;
using Microsoft.Extensions.Configuration;

namespace Gfc.Agent.Api.Configuration;

internal static class ControllerClientOptionsFactory
{
    public static ControllerClientOptions Create(IConfiguration configuration)
    {
        var options = new ControllerClientOptions
        {
            LocalControllerSn = configuration.GetValue<uint?>("ControllerClient:LocalControllerSn") ?? 0,
            DriverVersion = configuration.GetValue<byte?>("ControllerClient:DriverVersion") ?? 0x01,
            ProtocolMarker = configuration.GetValue<byte?>("ControllerClient:ProtocolMarker") ?? 0x00,
            MaxRetries = configuration.GetValue<int?>("ControllerClient:MaxRetries") ?? 2,
            SendTimeout = TimeSpan.FromMilliseconds(configuration.GetValue<double?>("ControllerClient:SendTimeoutMilliseconds") ?? 2500),
            ReceiveTimeout = TimeSpan.FromMilliseconds(configuration.GetValue<double?>("ControllerClient:ReceiveTimeoutMilliseconds") ?? 2500),
            CommandProfiles = CommandProfileFactory.CreateDefaults()
        };

        return options;
    }
}

