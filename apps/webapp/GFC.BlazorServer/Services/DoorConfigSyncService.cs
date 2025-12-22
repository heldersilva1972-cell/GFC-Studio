using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for syncing door configuration between database and controllers via Agent API.
/// </summary>
public class DoorConfigSyncService
{
    private readonly GfcDbContext _dbContext;
    private readonly IDoorConfigService _doorConfigService;
    private readonly ControllerRegistryService _controllerRegistryService;
    private readonly IControllerClient _controllerClient;

    private readonly ILogger<DoorConfigSyncService> _logger;


    public DoorConfigSyncService(
        GfcDbContext dbContext,
        IDoorConfigService doorConfigService,
        ControllerRegistryService controllerRegistryService,
        IControllerClient controllerClient,

        ILogger<DoorConfigSyncService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _doorConfigService = doorConfigService ?? throw new ArgumentNullException(nameof(doorConfigService));
        _controllerRegistryService = controllerRegistryService ?? throw new ArgumentNullException(nameof(controllerRegistryService));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }

    /// <summary>
    /// Reads door configuration from a controller and saves/updates it in the database.
    /// </summary>
    public async Task<bool> ReadFromControllerAsync(uint controllerSerialNumber, CancellationToken cancellationToken = default)
    {
        // Simulation mode removed - always use real controllers

        try
        {
            var dto = await _controllerClient.GetDoorConfigAsync(controllerSerialNumber.ToString(), cancellationToken);
            if (dto == null)
            {
                _logger.LogWarning("Failed to read door config from controller {SerialNumber}", controllerSerialNumber);
                return false;
            }

            var controller = await _controllerRegistryService.GetControllerBySerialNumberAsync(controllerSerialNumber, cancellationToken);
            if (controller == null)
            {
                _logger.LogWarning("Controller {SerialNumber} not found in database", controllerSerialNumber);
                return false;
            }

            var doors = controller.Doors.ToList();
            var configs = _doorConfigService.MapFromReadDto(dto, doors);

            await _doorConfigService.SaveConfigsAsync(controller.Id, configs);

            _logger.LogInformation("Successfully read and saved door config from controller {SerialNumber}", controllerSerialNumber);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading door config from controller {SerialNumber}", controllerSerialNumber);
            throw;
        }
    }

    /// <summary>
    /// Syncs door configuration from database to a controller.
    /// </summary>
    public async Task<bool> SyncToControllerAsync(uint controllerSerialNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var isSimulated = controllerSerialNumber == ControllerDevice.GetSimulatedSerialValue();

            var controller = await _controllerRegistryService.GetControllerBySerialNumberAsync(controllerSerialNumber, cancellationToken);
            if (controller == null)
            {
                _logger.LogWarning("Controller {SerialNumber} not found in database", controllerSerialNumber);
                return false;
            }



            var configs = await _doorConfigService.GetConfigsForControllerAsync(controller.Id, cancellationToken);
            if (configs.Count == 0)
            {
                _logger.LogWarning("No door configs found for controller {SerialNumber}", controllerSerialNumber);
                return false;
            }

            var writeDto = _doorConfigService.MapToWriteDto(configs);
            try
            {
                await _controllerClient.WriteDoorConfigAsync(controller.SerialNumberDisplay, writeDto, cancellationToken);
                _logger.LogInformation("Successfully synced door config to controller {SerialNumber}", controller.SerialNumberDisplay);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to sync door config to controller {SerialNumber}: {Message}", controller.SerialNumberDisplay, ex.Message);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing door config to controller {SerialNumber}", controllerSerialNumber);
            throw;
        }
    }
}
