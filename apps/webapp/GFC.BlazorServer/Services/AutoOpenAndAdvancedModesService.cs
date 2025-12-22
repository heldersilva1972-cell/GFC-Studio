using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public class AutoOpenAndAdvancedModesService
{
    private readonly GfcDbContext _dbContext;
    private readonly IScheduleService _scheduleService;
    private readonly ILogger<AutoOpenAndAdvancedModesService> _logger;


    public AutoOpenAndAdvancedModesService(
        GfcDbContext dbContext,
        IScheduleService scheduleService,
        ILogger<AutoOpenAndAdvancedModesService> logger,

    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }

    public async Task<List<DoorAutoOpenViewModel>> GetDoorAutoOpenForControllerAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        var doors = await _dbContext.Doors
            .Include(d => d.Controller)
            .Where(d => d.ControllerId == controllerId)
            .OrderBy(d => d.DoorIndex)
            .ToListAsync(cancellationToken);

        var schedules = await _dbContext.DoorAutoOpenSchedules
            .Include(s => s.TimeProfile)
            .Where(s => doors.Select(d => d.Id).Contains(s.DoorId))
            .ToListAsync(cancellationToken);

        var links = await _scheduleService.GetControllerLinksAsync(controllerId, cancellationToken);

        var result = new List<DoorAutoOpenViewModel>();
        foreach (var door in doors)
        {
            var schedule = schedules.FirstOrDefault(s => s.DoorId == door.Id);
            var link = schedule?.TimeProfileId != null
                ? links.FirstOrDefault(l => l.TimeProfileId == schedule.TimeProfileId)
                : null;

            result.Add(new DoorAutoOpenViewModel
            {
                DoorId = door.Id,
                DoorName = door.Name,
                ControllerName = door.Controller?.Name ?? "",
                DoorIndex = door.DoorIndex,
                TimeProfileId = schedule?.TimeProfileId,
                TimeProfileName = schedule?.TimeProfile?.Name,
                IsActive = schedule?.IsActive ?? false,
                ControllerProfileIndex = link?.ControllerProfileIndex ?? schedule?.ControllerProfileIndex,
                ControllerStatusState = ControllerStatusState.Unknown
            });
        }

        return result;
    }

    public async Task SaveDoorAutoOpenAsync(int controllerId, IEnumerable<DoorAutoOpenViewModel> models, CancellationToken cancellationToken = default)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var doors = await _dbContext.Doors
                .Where(d => d.ControllerId == controllerId)
                .ToListAsync(cancellationToken);

            var doorIds = doors.Select(d => d.Id).ToList();
            var existingSchedules = await _dbContext.DoorAutoOpenSchedules
                .Where(s => doorIds.Contains(s.DoorId))
                .ToListAsync(cancellationToken);

            foreach (var model in models)
            {
                var existing = existingSchedules.FirstOrDefault(s => s.DoorId == model.DoorId);
                if (existing != null)
                {
                    existing.TimeProfileId = model.TimeProfileId;
                    existing.IsActive = model.IsActive;
                    existing.Description = null; // Can be extended later
                    existing.UpdatedUtc = DateTime.UtcNow;
                }
                else
                {
                    var newSchedule = new DoorAutoOpenSchedule
                    {
                        DoorId = model.DoorId,
                        TimeProfileId = model.TimeProfileId,
                        IsActive = model.IsActive,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _dbContext.DoorAutoOpenSchedules.Add(newSchedule);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<List<DoorAdvancedModeViewModel>> GetDoorAdvancedModesForControllerAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        var doors = await _dbContext.Doors
            .Include(d => d.Controller)
            .Where(d => d.ControllerId == controllerId)
            .OrderBy(d => d.DoorIndex)
            .ToListAsync(cancellationToken);

        var options = await _dbContext.DoorBehaviorOptions
            .Where(o => doors.Select(d => d.Id).Contains(o.DoorId))
            .ToListAsync(cancellationToken);

        var result = new List<DoorAdvancedModeViewModel>();
        foreach (var door in doors)
        {
            var option = options.FirstOrDefault(o => o.DoorId == door.Id);
            result.Add(new DoorAdvancedModeViewModel
            {
                DoorId = door.Id,
                DoorName = door.Name,
                ControllerName = door.Controller?.Name ?? "",
                DoorIndex = door.DoorIndex,
                FirstCardOpenEnabled = option?.FirstCardOpenEnabled ?? false,
                DoorAsSwitchEnabled = option?.DoorAsSwitchEnabled ?? false,
                OpenTooLongWarnEnabled = option?.OpenTooLongWarnEnabled ?? false,
                Invalid3CardsWarnEnabled = option?.Invalid3CardsWarnEnabled ?? false,
                ControllerStatusState = ControllerStatusState.Unknown
            });
        }

        return result;
    }

    public async Task SaveDoorAdvancedModesAsync(int controllerId, IEnumerable<DoorAdvancedModeViewModel> models, CancellationToken cancellationToken = default)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var doors = await _dbContext.Doors
                .Where(d => d.ControllerId == controllerId)
                .ToListAsync(cancellationToken);

            var doorIds = doors.Select(d => d.Id).ToList();
            var existingOptions = await _dbContext.DoorBehaviorOptions
                .Where(o => doorIds.Contains(o.DoorId))
                .ToListAsync(cancellationToken);

            foreach (var model in models)
            {
                var existing = existingOptions.FirstOrDefault(o => o.DoorId == model.DoorId);
                if (existing != null)
                {
                    existing.FirstCardOpenEnabled = model.FirstCardOpenEnabled;
                    existing.DoorAsSwitchEnabled = model.DoorAsSwitchEnabled;
                    existing.OpenTooLongWarnEnabled = model.OpenTooLongWarnEnabled;
                    existing.Invalid3CardsWarnEnabled = model.Invalid3CardsWarnEnabled;
                    existing.UpdatedUtc = DateTime.UtcNow;
                }
                else
                {
                    var newOption = new DoorBehaviorOptions
                    {
                        DoorId = model.DoorId,
                        FirstCardOpenEnabled = model.FirstCardOpenEnabled,
                        DoorAsSwitchEnabled = model.DoorAsSwitchEnabled,
                        OpenTooLongWarnEnabled = model.OpenTooLongWarnEnabled,
                        Invalid3CardsWarnEnabled = model.Invalid3CardsWarnEnabled,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _dbContext.DoorBehaviorOptions.Add(newOption);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ControllerBehaviorViewModel?> GetControllerBehaviorAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);

        if (controller == null) return null;

        var options = await _dbContext.ControllerBehaviorOptions
            .FirstOrDefaultAsync(o => o.ControllerId == controllerId, cancellationToken);

        return new ControllerBehaviorViewModel
        {
            ControllerId = controller.Id,
            Name = controller.Name,
            SerialNumber = controller.SerialNumber,
            ValidSwipeGapSeconds = options?.ValidSwipeGapSeconds ?? 3,
            ControllerStatusState = ControllerStatusState.Unknown
        };
    }

    public async Task SaveControllerBehaviorAsync(ControllerBehaviorViewModel model, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.ControllerBehaviorOptions
            .FirstOrDefaultAsync(o => o.ControllerId == model.ControllerId, cancellationToken);

        if (existing != null)
        {
            existing.ValidSwipeGapSeconds = model.ValidSwipeGapSeconds;
            existing.UpdatedUtc = DateTime.UtcNow;
        }
        else
        {
            var newOption = new ControllerBehaviorOptions
            {
                ControllerId = model.ControllerId,
                ValidSwipeGapSeconds = model.ValidSwipeGapSeconds,
                CreatedUtc = DateTime.UtcNow
            };
            _dbContext.ControllerBehaviorOptions.Add(newOption);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public AutoOpenConfigDto BuildAutoOpenConfigDto(int controllerId, List<DoorAutoOpenViewModel> models, List<ControllerTimeProfileLink> links)
    {
        var tasks = new List<AutoOpenConfigDto.AutoOpenTask>();

        foreach (var model in models.Where(m => m.IsActive && m.TimeProfileId.HasValue))
        {
            var link = links.FirstOrDefault(l => l.TimeProfileId == model.TimeProfileId && l.IsEnabled);
            if (link != null)
            {
                tasks.Add(new AutoOpenConfigDto.AutoOpenTask
                {
                    DoorNumber = model.DoorIndex,
                    TimeZoneIndex = link.ControllerProfileIndex,
                    IsEnabled = model.IsActive
                });
            }
        }

        return new AutoOpenConfigDto { Tasks = tasks };
    }

    public AdvancedDoorModesDto BuildAdvancedDoorModesDto(List<DoorAdvancedModeViewModel> doorModels, ControllerBehaviorViewModel? controllerModel)
    {
        var doors = doorModels.Select(d => new AdvancedDoorModesDto.DoorAdvancedMode
        {
            DoorNumber = d.DoorIndex,
            FirstCardOpenEnabled = d.FirstCardOpenEnabled,
            DoorAsSwitchEnabled = d.DoorAsSwitchEnabled,
            OpenTooLongWarnEnabled = d.OpenTooLongWarnEnabled,
            Invalid3CardsWarnEnabled = d.Invalid3CardsWarnEnabled
        }).ToList();

        var controllerOptions = controllerModel != null
            ? new AdvancedDoorModesDto.ControllerAdvancedOptions
            {
                ValidSwipeGapSeconds = controllerModel.ValidSwipeGapSeconds
            }
            : null;

        return new AdvancedDoorModesDto
        {
            Doors = doors,
            ControllerOptions = controllerOptions
        };
    }

    public async Task<SyncStatusReport> RefreshFromControllerAsync(int controllerId, GFC.BlazorServer.Services.Controllers.IControllerClient controllerClient, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);

        if (controller == null)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller not found",
                CommandKeys = new List<string>()
            };
        }

        var report = new SyncStatusReport { CommandKeys = new List<string>() };

        try
        {
            // Refresh Auto-Open
            var autoOpenDto = await controllerClient.GetAutoOpenAsync(controller.SerialNumberDisplay, cancellationToken);
            if (autoOpenDto != null)
            {
                var doors = await _dbContext.Doors
                    .Where(d => d.ControllerId == controllerId)
                    .ToListAsync(cancellationToken);

                foreach (var task in autoOpenDto.Tasks)
                {
                    var door = doors.FirstOrDefault(d => d.DoorIndex == task.DoorNumber);
                    if (door != null)
                    {
                        // Find time profile by controller profile index
                        var links = await _scheduleService.GetControllerLinksAsync(controllerId, cancellationToken);
                        var link = links.FirstOrDefault(l => l.ControllerProfileIndex == task.TimeZoneIndex && l.IsEnabled);
                        
                        var existing = await _dbContext.DoorAutoOpenSchedules
                            .FirstOrDefaultAsync(s => s.DoorId == door.Id, cancellationToken);

                        if (existing != null)
                        {
                            existing.TimeProfileId = link?.TimeProfileId;
                            existing.IsActive = task.IsEnabled;
                            existing.UpdatedUtc = DateTime.UtcNow;
                        }
                        else
                        {
                            _dbContext.DoorAutoOpenSchedules.Add(new DoorAutoOpenSchedule
                            {
                                DoorId = door.Id,
                                TimeProfileId = link?.TimeProfileId,
                                IsActive = task.IsEnabled,
                                CreatedUtc = DateTime.UtcNow
                            });
                        }
                    }
                }
                report.CommandKeys.Add("SyncAutoOpen");
            }

            // Refresh Advanced Door Modes
            var advancedModesDto = await controllerClient.GetAdvancedDoorModesAsync(controller.SerialNumberDisplay, cancellationToken);
            if (advancedModesDto != null)
            {
                var doors = await _dbContext.Doors
                    .Where(d => d.ControllerId == controllerId)
                    .ToListAsync(cancellationToken);

                foreach (var doorMode in advancedModesDto.Doors)
                {
                    var door = doors.FirstOrDefault(d => d.DoorIndex == doorMode.DoorNumber);
                    if (door != null)
                    {
                        var existing = await _dbContext.DoorBehaviorOptions
                            .FirstOrDefaultAsync(o => o.DoorId == door.Id, cancellationToken);

                        if (existing != null)
                        {
                            existing.FirstCardOpenEnabled = doorMode.FirstCardOpenEnabled;
                            existing.DoorAsSwitchEnabled = doorMode.DoorAsSwitchEnabled;
                            existing.OpenTooLongWarnEnabled = doorMode.OpenTooLongWarnEnabled;
                            existing.Invalid3CardsWarnEnabled = doorMode.Invalid3CardsWarnEnabled;
                            existing.UpdatedUtc = DateTime.UtcNow;
                        }
                        else
                        {
                            _dbContext.DoorBehaviorOptions.Add(new DoorBehaviorOptions
                            {
                                DoorId = door.Id,
                                FirstCardOpenEnabled = doorMode.FirstCardOpenEnabled,
                                DoorAsSwitchEnabled = doorMode.DoorAsSwitchEnabled,
                                OpenTooLongWarnEnabled = doorMode.OpenTooLongWarnEnabled,
                                Invalid3CardsWarnEnabled = doorMode.Invalid3CardsWarnEnabled,
                                CreatedUtc = DateTime.UtcNow
                            });
                        }
                    }
                }

                // Refresh Controller Behavior Options
                if (advancedModesDto.ControllerOptions != null)
                {
                    var existing = await _dbContext.ControllerBehaviorOptions
                        .FirstOrDefaultAsync(o => o.ControllerId == controllerId, cancellationToken);

                    if (existing != null)
                    {
                        existing.ValidSwipeGapSeconds = advancedModesDto.ControllerOptions.ValidSwipeGapSeconds;
                        existing.UpdatedUtc = DateTime.UtcNow;
                    }
                    else
                    {
                        _dbContext.ControllerBehaviorOptions.Add(new ControllerBehaviorOptions
                        {
                            ControllerId = controllerId,
                            ValidSwipeGapSeconds = advancedModesDto.ControllerOptions.ValidSwipeGapSeconds,
                            CreatedUtc = DateTime.UtcNow
                        });
                    }
                }

                report.CommandKeys.Add("SyncAdvancedDoorModes");
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            report.Success = true;
            report.Message = "Configuration refreshed from controller";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh from controller {ControllerId}", controllerId);
            report.Success = false;
            report.Message = $"Failed to refresh: {ex.Message}";
        }

        return report;
    }

    public async Task<SyncStatusReport> SyncAutoOpenToControllerAsync(int controllerId, GFC.BlazorServer.Services.Controllers.IControllerClient controllerClient, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);

        if (controller == null)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller not found",
                CommandKeys = new List<string>()
            };
        }

        if (!controller.IsEnabled)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller must be enabled before syncing",
                CommandKeys = new List<string>()
            };
        }

        var report = new SyncStatusReport { CommandKeys = new List<string> { "SyncAutoOpen" } };

        try
        {


            var autoOpenModels = await GetDoorAutoOpenForControllerAsync(controllerId, cancellationToken);
            var links = await _scheduleService.GetControllerLinksAsync(controllerId, cancellationToken);
            var autoOpenDto = BuildAutoOpenConfigDto(controllerId, autoOpenModels, links);
            await controllerClient.WriteAutoOpenAsync(controller.SerialNumberDisplay, autoOpenDto, cancellationToken);

            report.Success = true;
            report.Message = "Auto-open configuration synced to controller successfully";
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync auto-open to controller {ControllerId}", controllerId);
            report.Success = false;
            report.Message = $"Failed to sync: {ex.Message}";
        }

        return report;
    }

    public async Task<SyncStatusReport> SyncAdvancedModesToControllerAsync(int controllerId, GFC.BlazorServer.Services.Controllers.IControllerClient controllerClient, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);

        if (controller == null)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller not found",
                CommandKeys = new List<string>()
            };
        }

        if (!controller.IsEnabled)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller must be enabled before syncing",
                CommandKeys = new List<string>()
            };
        }

        var report = new SyncStatusReport { CommandKeys = new List<string> { "SyncAdvancedDoorModes" } };

        try
        {


            var doorModes = await GetDoorAdvancedModesForControllerAsync(controllerId, cancellationToken);
            var controllerBehavior = await GetControllerBehaviorAsync(controllerId, cancellationToken);
            var advancedModesDto = BuildAdvancedDoorModesDto(doorModes, controllerBehavior);
            await controllerClient.WriteAdvancedDoorModesAsync(controller.SerialNumberDisplay, advancedModesDto, cancellationToken);

            report.Success = true;
            report.Message = "Advanced door modes configuration synced to controller successfully";
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync advanced modes to controller {ControllerId}", controllerId);
            report.Success = false;
            report.Message = $"Failed to sync: {ex.Message}";
        }

        return report;
    }

    public async Task<SyncStatusReport> SyncDbToControllerAsync(int controllerId, GFC.BlazorServer.Services.Controllers.IControllerClient controllerClient, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);

        if (controller == null)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller not found",
                CommandKeys = new List<string>()
            };
        }

        if (!controller.IsEnabled)
        {
            return new SyncStatusReport
            {
                Success = false,
                Message = "Controller must be enabled before syncing",
                CommandKeys = new List<string>()
            };
        }

        var report = new SyncStatusReport { CommandKeys = new List<string>() };

        try
        {

            // Sync Auto-Open
            var autoOpenModels = await GetDoorAutoOpenForControllerAsync(controllerId, cancellationToken);
            var links = await _scheduleService.GetControllerLinksAsync(controllerId, cancellationToken);
            var autoOpenDto = BuildAutoOpenConfigDto(controllerId, autoOpenModels, links);
            await controllerClient.WriteAutoOpenAsync(controller.SerialNumberDisplay, autoOpenDto, cancellationToken);
            report.CommandKeys.Add("SyncAutoOpen");


            // Sync Advanced Door Modes
            var doorModes = await GetDoorAdvancedModesForControllerAsync(controllerId, cancellationToken);
            var controllerBehavior = await GetControllerBehaviorAsync(controllerId, cancellationToken);
            var advancedModesDto = BuildAdvancedDoorModesDto(doorModes, controllerBehavior);
            await controllerClient.WriteAdvancedDoorModesAsync(controller.SerialNumberDisplay, advancedModesDto, cancellationToken);
            report.CommandKeys.Add("SyncAdvancedDoorModes");

            report.Success = true;
            report.Message = "Configuration synced to controller successfully";
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync to controller {ControllerId}", controllerId);
            report.Success = false;
            report.Message = $"Failed to sync: {ex.Message}";
        }

        return report;
    }
}

