using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for syncing member privileges to controllers via Agent API.
/// </summary>
public class MemberPrivilegeSyncService : IMemberPrivilegeSyncService
{
    private readonly GfcDbContext _dbContext;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberKeycardRepository _memberKeycardRepository;
    private readonly ControllerRegistryService _controllerRegistryService;
    private readonly IScheduleService _scheduleService;
    private readonly IControllerClient _controllerClient;
    private readonly ILogger<MemberPrivilegeSyncService> _logger;

    public MemberPrivilegeSyncService(
        GfcDbContext dbContext,
        IMemberRepository memberRepository,
        IMemberKeycardRepository memberKeycardRepository,
        ControllerRegistryService controllerRegistryService,
        IScheduleService scheduleService,
        IControllerClient controllerClient,
        ILogger<MemberPrivilegeSyncService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberKeycardRepository = memberKeycardRepository ?? throw new ArgumentNullException(nameof(memberKeycardRepository));
        _controllerRegistryService = controllerRegistryService ?? throw new ArgumentNullException(nameof(controllerRegistryService));
        _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<EffectiveAccessResult> ComputeEffectiveAccessAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            throw new InvalidOperationException($"Member {memberId} not found.");
        }

        var cardAssignment = _memberKeycardRepository.GetCurrentAssignmentForMember(memberId);
        var cardNumber = cardAssignment?.KeyCard?.CardNumber;
        var hasActiveCard = !string.IsNullOrWhiteSpace(cardNumber);

        var accessRecords = await _dbContext.MemberDoorAccesses
            .Include(m => m.Door)
                .ThenInclude(d => d!.Controller)
            .Include(m => m.TimeProfile)
            .Where(m => m.MemberId == memberId && m.IsEnabled)
            .ToListAsync(cancellationToken);

        var controllers = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);
        var controllerLinks = new Dictionary<int, List<ControllerTimeProfileLink>>();

        var result = new EffectiveAccessResult
        {
            MemberId = memberId,
            CardNumber = cardNumber,
            HasActiveCard = hasActiveCard
        };

        foreach (var controller in controllers)
        {
            var links = await _scheduleService.GetControllerLinksAsync(controller.Id, cancellationToken);
            controllerLinks[controller.Id] = links;

            var controllerGroup = new ControllerAccessGroup
            {
                ControllerId = controller.Id,
                ControllerName = controller.Name,
                SerialNumber = controller.SerialNumber
            };

            foreach (var door in controller.Doors)
            {
                var access = accessRecords.FirstOrDefault(a => a.DoorId == door.Id);
                if (access == null || !access.IsEnabled) continue;

                int? timeProfileIndex = null;
                if (access.TimeProfileId.HasValue)
                {
                    var link = links.FirstOrDefault(l => l.TimeProfileId == access.TimeProfileId.Value && l.IsEnabled);
                    timeProfileIndex = link?.ControllerProfileIndex;
                }

                controllerGroup.Doors.Add(new DoorAccessConfig
                {
                    DoorId = door.Id,
                    DoorIndex = door.DoorIndex,
                    DoorName = door.Name,
                    IsEnabled = access.IsEnabled,
                    TimeProfileIndex = timeProfileIndex
                });
            }

            if (controllerGroup.Doors.Count > 0)
            {
                result.Controllers.Add(controllerGroup);
            }
        }

        return result;
    }

    public async Task<SyncResult> SyncMemberPrivilegesAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var result = new SyncResult();
        var effectiveAccess = await ComputeEffectiveAccessAsync(memberId, cancellationToken);

        if (!effectiveAccess.HasActiveCard || string.IsNullOrWhiteSpace(effectiveAccess.CardNumber))
        {
            result.Success = false;
            result.Message = "Member has no active key card.";
            return result;
        }

        if (!long.TryParse(effectiveAccess.CardNumber, out var cardNumber))
        {
            result.Success = false;
            result.Message = "Invalid card number.";
            return result;
        }

        foreach (var controllerGroup in effectiveAccess.Controllers)
        {
            result.ControllersProcessed++;
            try
            {
                foreach (var door in controllerGroup.Doors)
                {
                    var privilege = new CardPrivilegeModel
                    {
                        ControllerId = controllerGroup.ControllerId,
                        DoorId = door.DoorId,
                        DoorIndex = door.DoorIndex,
                        CardNumber = cardNumber,
                        TimeProfileIndex = door.TimeProfileIndex,
                        Enabled = door.IsEnabled
                    };

                    await _controllerClient.AddOrUpdatePrivilegeAsync(privilege, cancellationToken);
                }

                result.ControllersSucceeded++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync privileges to controller {ControllerId}", controllerGroup.ControllerId);
                result.Errors.Add($"Controller {controllerGroup.ControllerName}: {ex.Message}");
            }
        }

        result.Success = result.Errors.Count == 0;
        result.Message = result.Success
            ? $"Synced privileges to {result.ControllersSucceeded} controller(s)."
            : $"Synced to {result.ControllersSucceeded} controller(s) with {result.Errors.Count} error(s).";

        return result;
    }

    public async Task<SyncResult> RemoveCardFromControllersAsync(int memberId, string cardNumber, CancellationToken cancellationToken = default)
    {
        var result = new SyncResult();

        var controllers = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);

        if (!long.TryParse(cardNumber, out var cardNumberValue))
        {
            result.Success = false;
            result.Message = "Invalid card number format.";
            return result;
        }

        result.ControllersProcessed = controllers.Count;

        try
        {
            await _controllerClient.DeletePrivilegeAsync(cardNumberValue, cancellationToken);
            result.ControllersSucceeded = controllers.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove card privileges for member {MemberId}", memberId);
            result.Errors.Add(ex.Message);
        }

        result.Success = result.Errors.Count == 0;
        result.Message = result.Success
            ? $"Removed card from {result.ControllersSucceeded} controller(s)."
            : $"Removed from {result.ControllersSucceeded} controller(s) with {result.Errors.Count} error(s).";

        return result;
    }
}
