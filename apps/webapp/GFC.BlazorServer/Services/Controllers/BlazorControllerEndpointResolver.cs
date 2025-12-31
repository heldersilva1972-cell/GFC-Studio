using System;
using System.Linq;
using System.Net;
using GFC.BlazorServer.Connectors.Mengqi.Abstractions;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;
using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Services.Controllers;

public class BlazorControllerEndpointResolver : IControllerEndpointResolver
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly IOptionsMonitor<AgentApiOptions> _options;

    public BlazorControllerEndpointResolver(
        IDbContextFactory<GfcDbContext> dbFactory,
        IOptionsMonitor<AgentApiOptions> options)
    {
        _dbFactory = dbFactory;
        _options = options;
    }

    public ControllerEndpoint Resolve(uint controllerSn)
    {
        // 0 is used for broadcast discovery
        if (controllerSn == 0)
        {
            return new ControllerEndpoint(IPAddress.Broadcast, 60000, 60000, null);
        }

        // 1. Try to resolve from database (Primary source of truth)
        using (var context = _dbFactory.CreateDbContext())
        {
            var controller = context.Controllers
                .AsNoTracking()
                .FirstOrDefault(c => c.SerialNumber == controllerSn);

            if (controller != null && !string.IsNullOrWhiteSpace(controller.IpAddress))
            {
                if (IPAddress.TryParse(controller.IpAddress, out var ip))
                {
                    // Look up matching config in appsettings for the password and default ports
                    var appSettingsConfig = _options.CurrentValue.Controllers
                        .FirstOrDefault(c => c.SerialNumber == controllerSn);
                    
                    var password = appSettingsConfig?.CommPassword;
                    var udpPort = controller.Port > 0 ? controller.Port : (appSettingsConfig?.UdpPort ?? 60000);
                    var tcpPort = controller.Port > 0 ? controller.Port : (appSettingsConfig?.TcpPort ?? 60000);

                    return new ControllerEndpoint(ip, udpPort, tcpPort, password);
                }
            }
        }

        // 2. Fallback to appsettings.json if not in DB or DB IP is invalid
        var config = _options.CurrentValue.Controllers.FirstOrDefault(c => c.SerialNumber == controllerSn);
        if (config == null)
        {
            throw new InvalidOperationException($"Controller {controllerSn} is not configured in database or appsettings.");
        }

        if (!IPAddress.TryParse(config.IpAddress, out var fallbackIp))
        {
             // Last resort fallback
             fallbackIp = IPAddress.Loopback;
        }

        return new ControllerEndpoint(fallbackIp, config.UdpPort, config.TcpPort, config.CommPassword);
    }
}
