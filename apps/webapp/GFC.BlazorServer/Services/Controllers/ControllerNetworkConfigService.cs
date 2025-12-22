using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Service for managing controller network configuration.
/// </summary>
public class ControllerNetworkConfigService
{
    private readonly GfcDbContext _dbContext;
    private readonly IControllerClient _controllerClient;
    private readonly IControllerModeProvider _modeProvider;
    private readonly ILogger<ControllerNetworkConfigService> _logger;


    public ControllerNetworkConfigService(
        GfcDbContext dbContext,
        IControllerClient controllerClient,
        IControllerModeProvider modeProvider,
        ILogger<ControllerNetworkConfigService> logger,

    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
        _modeProvider = modeProvider ?? throw new ArgumentNullException(nameof(modeProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }

    public async Task<ControllerNetworkConfig?> GetFromDbAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ControllerNetworkConfigs
            .Include(c => c.Controller)
            .FirstOrDefaultAsync(c => c.ControllerId == controllerId, cancellationToken);
    }

    public async Task SaveToDbAsync(ControllerNetworkConfig config, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.ControllerNetworkConfigs
            .FirstOrDefaultAsync(c => c.ControllerId == config.ControllerId, cancellationToken);

        if (existing != null)
        {
            existing.IpAddress = config.IpAddress;
            existing.SubnetMask = config.SubnetMask;
            existing.Gateway = config.Gateway;
            existing.Port = config.Port;
            existing.DhcpEnabled = config.DhcpEnabled;
            existing.AllowedPcIp = config.AllowedPcIp;
            existing.CommPasswordMasked = config.CommPasswordMasked;
            existing.UpdatedUtc = DateTime.UtcNow;
        }
        else
        {
            config.CreatedUtc = DateTime.UtcNow;
            _dbContext.ControllerNetworkConfigs.Add(config);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ControllerNetworkConfig?> ReadFromControllerAsync(uint controllerSerialNumber, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.SerialNumber == controllerSerialNumber, cancellationToken);

        if (controller == null)
        {
            _logger.LogWarning("Controller with serial number {Sn} not found", controllerSerialNumber);
            return null;
        }

        if (!_modeProvider.UseRealControllers)
        {
            // Simulation mode: return simulated data
            var simulated = await GetFromDbAsync(controller.Id, cancellationToken);
            if (simulated == null)
            {
                simulated = new ControllerNetworkConfig
                {
                    ControllerId = controller.Id,
                    IpAddress = "192.168.1.100",
                    SubnetMask = "255.255.255.0",
                    Gateway = "192.168.1.1",
                    Port = 60000,
                    DhcpEnabled = false,
                    AllowedPcIp = "192.168.1.50",
                    CommPasswordMasked = "****",
                    LastReadUtc = DateTime.UtcNow
                };
            }
            else
            {
                simulated.LastReadUtc = DateTime.UtcNow;
            }
            return simulated;
        }

        try
        {
            // Read network config
            var networkConfig = await _controllerClient.GetNetworkConfigAsync(controllerSerialNumber.ToString(), cancellationToken);
            
            // Read allowed PC and password (if available)
            AllowedPcAndPasswordRequestDto? allowedPcData = null;
            try
            {
                allowedPcData = await _controllerClient.GetAllowedPcSettingsAsync(controllerSerialNumber.ToString(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read allowed PC/password for controller {Sn}", controllerSerialNumber);
                // Continue without allowed PC data
            }

            var config = await GetFromDbAsync(controller.Id, cancellationToken);
            if (config == null)
            {
                config = new ControllerNetworkConfig
                {
                    ControllerId = controller.Id,
                    CreatedUtc = DateTime.UtcNow
                };
            }

            if (networkConfig != null)
            {
                config.IpAddress = networkConfig.IpAddress;
                config.SubnetMask = networkConfig.SubnetMask;
                config.Gateway = networkConfig.Gateway;
                config.Port = networkConfig.Port;
                config.DhcpEnabled = networkConfig.DhcpEnabled;
            }

            if (allowedPcData != null)
            {
                config.AllowedPcIp = allowedPcData.AllowedPcIp;
                // Password is never returned from controller for security, so we keep existing masked value
                // If we don't have a masked value yet, set one to indicate password exists
                if (string.IsNullOrWhiteSpace(config.CommPasswordMasked))
                {
                    config.CommPasswordMasked = "****"; // Indicate password is set
                }
            }

            config.LastReadUtc = DateTime.UtcNow;
            config.UpdatedUtc = DateTime.UtcNow;

            await SaveToDbAsync(config, cancellationToken);
            return config;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read network config from controller {Sn}", controllerSerialNumber);
            throw;
        }
    }

    public async Task SyncToControllerAsync(uint controllerSerialNumber, ControllerNetworkConfig config, string? newPassword = null, CancellationToken cancellationToken = default)
    {
        var isSimulated = !_modeProvider.UseRealControllers || controllerSerialNumber == ControllerDevice.GetSimulatedSerialValue();
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.SerialNumber == controllerSerialNumber, cancellationToken);

        if (controller == null)
        {
            throw new InvalidOperationException($"Controller with serial number {controllerSerialNumber} not found");
        }

        try
        {


            // Sync network config
            var networkDto = new NetworkConfigDto
            {
                IpAddress = config.IpAddress,
                SubnetMask = config.SubnetMask,
                Gateway = config.Gateway,
                Port = config.Port,
                DhcpEnabled = config.DhcpEnabled
            };

            if (isSimulated)
            {
                // Simulated sync: persist to DB only
                config.LastSyncUtc = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    config.CommPasswordMasked = "****";
                }
                await SaveToDbAsync(config, cancellationToken);
                return;
            }

            await _controllerClient.SetNetworkConfigAsync(controllerSerialNumber.ToString(), networkDto, cancellationToken);

            // Sync allowed PC and password if provided
            if (!string.IsNullOrWhiteSpace(config.AllowedPcIp) || !string.IsNullOrWhiteSpace(newPassword))
            {
                var allowedPcDto = new AllowedPcAndPasswordRequestDto
                {
                    AllowedPcIp = config.AllowedPcIp,
                    CommPassword = string.IsNullOrWhiteSpace(newPassword) ? null : newPassword
                };

                var result = await _controllerClient.SetAllowedPcSettingsAsync(controllerSerialNumber.ToString(), allowedPcDto, cancellationToken);
                if (!result.Success)
                {
                    throw new InvalidOperationException($"Failed to sync allowed PC/password: {result.Message}");
                }
            }

            config.LastSyncUtc = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                config.CommPasswordMasked = "****";
            }

            await SaveToDbAsync(config, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync network config to controller {Sn}", controllerSerialNumber);
            throw;
        }
    }
}
