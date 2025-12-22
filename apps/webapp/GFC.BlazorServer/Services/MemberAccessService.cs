using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MemberDoorAccess = GFC.Core.Models.MemberDoorAccess;
using EffectiveAccessResult = GFC.BlazorServer.Services.EffectiveAccessResult;
using ControllerAccessGroup = GFC.BlazorServer.Services.ControllerAccessGroup;
using DoorAccessConfig = GFC.BlazorServer.Services.DoorAccessConfig;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing member door access privileges.
/// </summary>
public class MemberAccessService : IMemberAccessService
{
    private readonly GfcDbContext _dbContext;
    private readonly IMemberRepository _memberRepository;
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly IMemberKeycardRepository _memberKeycardRepository;
    private readonly KeyCardService _keyCardService;
    private readonly ControllerRegistryService _controllerRegistryService;
    private readonly IScheduleService _scheduleService;
    private readonly CommandInfoService _commandInfoService;
    private readonly IControllerClient _controllerClient;

    private readonly IMemberHistoryService _historyService;
    private readonly ILogger<MemberAccessService> _logger;

    private readonly IAuditLogger _auditLogger;

    public MemberAccessService(
        GfcDbContext dbContext,
        IMemberRepository memberRepository,
        IKeyCardRepository keyCardRepository,
        IMemberKeycardRepository memberKeycardRepository,
        KeyCardService keyCardService,
        ControllerRegistryService controllerRegistryService,
        IScheduleService scheduleService,
        CommandInfoService commandInfoService,
        IControllerClient controllerClient,

        IMemberHistoryService historyService,
        ILogger<MemberAccessService> logger,

        IAuditLogger auditLogger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _keyCardRepository = keyCardRepository ?? throw new ArgumentNullException(nameof(keyCardRepository));
        _memberKeycardRepository = memberKeycardRepository ?? throw new ArgumentNullException(nameof(memberKeycardRepository));
        _keyCardService = keyCardService ?? throw new ArgumentNullException(nameof(keyCardService));
        _controllerRegistryService = controllerRegistryService ?? throw new ArgumentNullException(nameof(controllerRegistryService));
        _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
        _commandInfoService = commandInfoService ?? throw new ArgumentNullException(nameof(commandInfoService));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));

        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
    }

    public async Task<MemberDoorAccessResponse> GetMemberDoorAccessAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            throw new InvalidOperationException($"Member {memberId} not found.");
        }

        var eligibility = _keyCardService.GetKeyCardEligibility(memberId, DateTime.Today.Year);
        var isEligible = eligibility.Eligible;

        // TODO: Re-enable when MemberDoorAccess table exists in the database.
        // var accessRecords = await _dbContext.MemberDoorAccesses
        //     .Include(m => m.Door)
        //         .ThenInclude(d => d!.Controller)
        //     .Include(m => m.TimeProfile)
        //     .Where(m => m.MemberId == memberId)
        //     .ToListAsync(cancellationToken);
        var accessRecords = new List<MemberDoorAccess>();

        var allDoors = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);
        var allDoorsList = allDoors.SelectMany(c => c.Doors).ToList();

        var result = new List<MemberDoorAccessDto>();

        foreach (var door in allDoorsList)
        {
            var access = accessRecords.FirstOrDefault(a => a.DoorId == door.Id);
            var cardAssignment = _memberKeycardRepository.GetCurrentAssignmentForMember(memberId);
            var cardNumber = cardAssignment?.KeyCard?.CardNumber ?? string.Empty;

            var dto = new MemberDoorAccessDto
            {
                Id = access?.Id ?? 0,
                MemberId = memberId,
                CardNumber = access?.CardNumber ?? cardNumber,
                DoorId = door.Id,
                DoorName = door.Name,
                ControllerId = door.ControllerId,
                ControllerName = door.Controller?.Name ?? "Unknown",
                ControllerSerialNumber = door.Controller?.SerialNumber ?? 0,
                TimeProfileId = access?.TimeProfileId,
                TimeProfileName = access?.TimeProfile?.Name,
                IsEnabled = access?.IsEnabled ?? false,
                LastSyncedAt = access?.LastSyncedAt,
                LastSyncResult = access?.LastSyncResult,
                IsEligible = isEligible,
                StatusText = GetStatusText(access, isEligible)
            };

            result.Add(dto);
        }

        return new MemberDoorAccessResponse
        {
            Access = result,
            Eligibility = eligibility
        };
    }

    public async Task UpdateMemberDoorAccessAsync(int memberId, IEnumerable<MemberDoorAccessUpdateDto> updates, CancellationToken cancellationToken = default)
    {
        // Placeholder: MemberDoorAccess table is not yet available; skip persisting changes.
        await Task.CompletedTask;

        // Original logic retained for future enablement when the table exists.
        // var member = _memberRepository.GetMemberById(memberId);
        // if (member == null)
        // {
        //     throw new InvalidOperationException($"Member {memberId} not found.");
        // }
        //
        // var eligibility = _keyCardService.GetKeyCardEligibility(memberId, DateTime.Today.Year);
        // if (!eligibility.Eligible)
        // {
        //     throw new InvalidOperationException("Member is not eligible for door access. Member must be eligible for key cards (paid/waived dues, REGULAR/LIFE status).");
        // }
        //
        // var updatesList = updates.ToList();
        // var doorIds = updatesList.Select(u => u.DoorId).Distinct().ToList();
        //
        // var existingAccess = await _dbContext.MemberDoorAccesses
        //     .Where(m => m.MemberId == memberId && doorIds.Contains(m.DoorId))
        //     .ToListAsync(cancellationToken);
        //
        // var cardAssignment = _memberKeycardRepository.GetCurrentAssignmentForMember(memberId);
        // if (cardAssignment == null || cardAssignment.KeyCard == null)
        // {
        //     throw new InvalidOperationException("Member does not have an active key card assignment.");
        // }
        //
        // var cardNumber = cardAssignment.KeyCard.CardNumber;
        //
        // foreach (var update in updatesList)
        // {
        //     var existing = existingAccess.FirstOrDefault(a => a.DoorId == update.DoorId && a.CardNumber == update.CardNumber);
        //     
        //     if (existing != null)
        //     {
        //         existing.IsEnabled = update.IsEnabled;
        //         existing.TimeProfileId = update.TimeProfileId;
        //     }
        //     else
        //     {
        //         var newAccess = new MemberDoorAccess
        //         {
        //             MemberId = memberId,
        //             CardNumber = update.CardNumber,
        //             DoorId = update.DoorId,
        //             IsEnabled = update.IsEnabled,
        //             TimeProfileId = update.TimeProfileId
        //         };
        //         _dbContext.MemberDoorAccesses.Add(newAccess);
        //     }
        // }
        //
        // await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncMemberPrivilegesAsync(int memberId, CancellationToken cancellationToken = default)
    {
        // Placeholder: MemberDoorAccess table is not available; skip sync operations.
        await Task.CompletedTask;

        // Original implementation retained for future enablement.
        // var member = _memberRepository.GetMemberById(memberId);
        // if (member == null)
        // {
        //     throw new InvalidOperationException($"Member {memberId} not found.");
        // }
        //
        // var eligibility = _keyCardService.GetKeyCardEligibility(memberId, DateTime.Today.Year);
        // var cardAssignment = _memberKeycardRepository.GetCurrentAssignmentForMember(memberId);
        // 
        // if (cardAssignment == null || cardAssignment.KeyCard == null)
        // {
        //     throw new InvalidOperationException("Member does not have an active key card assignment.");
        // }
        //
        // var cardNumber = cardAssignment.KeyCard.CardNumber;
        // var accessRecords = await _dbContext.MemberDoorAccesses
        //     .Include(m => m.Door)
        //         .ThenInclude(d => d!.Controller)
        //     .Where(m => m.MemberId == memberId)
        //     .ToListAsync(cancellationToken);
        //
        // var controllers = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);
        // var useRealControllers = _modeProvider.UseRealControllers;
        //
        // var syncResults = new List<string>();
        //
        // foreach (var controller in controllers)
        // {
        //     if (!controller.IsEnabled)
        //     {
        //         continue;
        //     }
        //
        //     var controllerDoors = controller.Doors.Where(d => d.IsEnabled).ToList();
        //     var controllerAccessRecords = accessRecords.Where(a => controllerDoors.Any(d => d.Id == a.DoorId)).ToList();
        //
        //     foreach (var door in controllerDoors)
        //     {
        //         var access = controllerAccessRecords.FirstOrDefault(a => a.DoorId == door.Id && a.CardNumber == cardNumber);
        //         var shouldHaveAccess = eligibility.Eligible && access?.IsEnabled == true;
        //
        //         if (shouldHaveAccess)
        //         {
        //             if (access == null)
        //             {
        //                 continue;
        //             }
        //
        //             if (useRealControllers)
        //             {
        //                 var timeProfileIndex = await GetTimeProfileIndexForController(controller.Id, access.TimeProfileId, cancellationToken);
        //                 var cardRequest = new AddOrUpdateCardRequestDto
        //                 {
        //                     CardNumber = cardNumber,
        //                     DoorIndex = door.DoorIndex,
        //                     TimeProfileIndex = timeProfileIndex,
        //                     Enabled = true
        //                 };
        //
        //                 var result = await _controllerClient.AddOrUpdateCardAsync(controller.SerialNumberDisplay, cardRequest, cancellationToken);
        //                 access.LastSyncedAt = DateTime.UtcNow;
        //                 access.LastSyncResult = result.Success ? "Success" : result.Message;
        //                 syncResults.Add($"{controller.Name}/{door.Name}: {(result.Success ? "Synced" : $"Failed: {result.Message}")}");
        //             }
        //             else
        //             {
        //                 access.LastSyncedAt = DateTime.UtcNow;
        //                 access.LastSyncResult = "Simulated (no real controller changes)";
        //                 syncResults.Add($"{controller.Name}/{door.Name}: Simulated");
        //             }
        //         }
        //         else
        //         {
        //             if (useRealControllers)
        //             {
        //                 var result = await _controllerClient.DeleteCardAsync(controller.SerialNumberDisplay, cardNumber, cancellationToken);
        //                 if (access != null)
        //                 {
        //                     access.IsEnabled = false;
        //                     access.LastSyncedAt = DateTime.UtcNow;
        //                     access.LastSyncResult = result.Success ? "Removed" : result.Message;
        //                 }
        //                 syncResults.Add($"{controller.Name}/{door.Name}: {(result.Success ? "Removed" : $"Failed: {result.Message}")}");
        //             }
        //             else
        //             {
        //                 if (access != null)
        //                 {
        //                     access.IsEnabled = false;
        //                     access.LastSyncedAt = DateTime.UtcNow;
        //                     access.LastSyncResult = "Simulated removal";
        //                 }
        //                 syncResults.Add($"{controller.Name}/{door.Name}: Simulated removal");
        //             }
        //         }
        //     }
        // }
        //
        // await _dbContext.SaveChangesAsync(cancellationToken);
        //
        // var currentUser = Environment.UserName ?? "System";
        // _historyService.LogChange(memberId, "DoorAccess", null, $"Synced privileges: {string.Join("; ", syncResults)}", currentUser);
    }

    public async Task RemoveCardAsync(int memberId, string cardNumber, string? performedByUserName = null, int? performedByUserId = null, CancellationToken cancellationToken = default)
    {
        // Placeholder: MemberDoorAccess table is not available; skip removal logic.
        await Task.CompletedTask;

        // Original implementation retained for future enablement.
        // var member = _memberRepository.GetMemberById(memberId);
        // if (member == null)
        // {
        //     throw new InvalidOperationException($"Member {memberId} not found.");
        // }
        //
        // var accessRecords = await _dbContext.MemberDoorAccesses
        //     .Include(m => m.Door)
        //         .ThenInclude(d => d!.Controller)
        //     .Where(m => m.MemberId == memberId && m.CardNumber == cardNumber)
        //     .ToListAsync(cancellationToken);
        //
        // var controllers = accessRecords
        //     .Select(a => a.Door?.Controller)
        //     .Where(c => c != null)
        //     .Distinct()
        //     .ToList();
        //
        // var useRealControllers = _modeProvider.UseRealControllers;
        //
        // foreach (var controller in controllers)
        // {
        //     if (controller == null || !controller.IsEnabled)
        //     {
        //         continue;
        //     }
        //
        //     if (useRealControllers)
        //     {
        //         var result = await _controllerClient.DeleteCardAsync(controller.SerialNumberDisplay, cardNumber, cancellationToken);
        //         _logger.LogInformation("DeleteCard for controller {ControllerId} ({SerialNumber}): {Success} - {Message}",
        //             controller.Id, controller.SerialNumber, result.Success, result.Message);
        //     }
        //     else
        //     {
        //         _logger.LogInformation("DeleteCard simulated for controller {ControllerId} ({SerialNumber})",
        //             controller.Id, controller.SerialNumber);
        //     }
        // }
        //
        // foreach (var access in accessRecords)
        // {
        //     access.IsEnabled = false;
        //     access.LastSyncedAt = DateTime.UtcNow;
        //     access.LastSyncResult = useRealControllers ? "Removed" : "Simulated removal";
        // }
        //
        // await _dbContext.SaveChangesAsync(cancellationToken);
        //
        // var performedBy = performedByUserName ?? Environment.UserName ?? "System";
        // _historyService.LogChange(memberId, "DoorAccess", null, $"Removed card {cardNumber} from controllers", performedBy);
        // var details = $"Disabled card {cardNumber} and removed from controllers";
        // _auditLogger.Log(
        //     AuditLogActions.KeyCardDisabled,
        //     performedByUserId,
        //     null,
        //     details);
    }

    public async Task ClearAllPrivilegesAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        var controller = await _controllerRegistryService.GetControllerByIdAsync(controllerId, cancellationToken);
        if (controller == null)
        {
            throw new InvalidOperationException($"Controller {controllerId} not found.");
        }

        var commandInfo = await _commandInfoService.GetByKeyAsync("ClearAllCards", cancellationToken);
        if (commandInfo == null)
        {
            throw new InvalidOperationException("ClearAllCards command info not found.");
        }

        var isSimulatedController = controller.Id == ControllerDevice.SimulatedControllerId || controller.IsSimulated;



        await _controllerClient.ClearAllCardsAsync(controller.Id, cancellationToken);

        _logger.LogWarning(
            "ClearAllCards executed for controller {ControllerId} ({SerialNumber})",
            controller.Id,
            controller.SerialNumberDisplay);

        var currentUser = Environment.UserName ?? "System";
        _logger.LogWarning("DANGEROUS OPERATION: ClearAllPrivileges executed for controller {ControllerId} by {User}",
            controllerId, currentUser);
    }

    private string GetStatusText(MemberDoorAccess? access, bool isEligible)
    {
        if (!isEligible)
        {
            return "Not Eligible";
        }

        if (access == null)
        {
            return "No Access";
        }

        if (!access.IsEnabled)
        {
            return "Disabled";
        }

        if (access.LastSyncedAt.HasValue)
        {
            return $"Synced {access.LastSyncedAt.Value:g}";
        }

        return "Pending Sync";
    }

    private async Task<int?> GetTimeProfileIndexForController(int controllerId, int? timeProfileId, CancellationToken cancellationToken)
    {
        if (!timeProfileId.HasValue)
        {
            return null;
        }

        var link = await _dbContext.ControllerTimeProfileLinks
            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && l.TimeProfileId == timeProfileId.Value, cancellationToken);

        return link?.ControllerProfileIndex;
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

        // TODO: Re-enable when MemberDoorAccess table is available.
        // var accessRecords = await _dbContext.MemberDoorAccesses
        //     .Include(m => m.Door)
        //         .ThenInclude(d => d!.Controller)
        //     .Include(m => m.TimeProfile)
        //     .Where(m => m.MemberId == memberId && m.IsEnabled)
        //     .ToListAsync(cancellationToken);
        var accessRecords = new List<MemberDoorAccess>();

        var controllers = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);

        var result = new EffectiveAccessResult
        {
            MemberId = memberId,
            CardNumber = cardNumber,
            HasActiveCard = hasActiveCard
        };

        foreach (var controller in controllers)
        {
            var links = await _scheduleService.GetControllerLinksAsync(controller.Id, cancellationToken);

            var controllerGroup = new ControllerAccessGroup
            {
                ControllerId = controller.Id,
                ControllerName = controller.Name,
                SerialNumber = controller.SerialNumber,
                SerialNumberDisplay = controller.SerialNumberDisplay
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
}
