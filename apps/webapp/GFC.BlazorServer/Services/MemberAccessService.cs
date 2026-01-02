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
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
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
        IDbContextFactory<GfcDbContext> contextFactory,
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
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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

        var currentYear = DateTime.Today.Year;
        var eligibility = _keyCardService.GetKeyCardEligibility(memberId, currentYear);
        var isEligible = eligibility.Eligible;

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var accessRecords = await dbContext.MemberDoorAccesses
            .Include(m => m.Door)
                .ThenInclude(d => d!.Controller)
            .Include(m => m.TimeProfile)
            .Where(m => m.MemberId == memberId)
            .ToListAsync(cancellationToken);

        var allDoors = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);
        var allDoorsList = allDoors.SelectMany(c => c.Doors).ToList();

        var result = new List<MemberDoorAccessDto>();

        foreach (var door in allDoorsList)
        {
            var access = accessRecords.FirstOrDefault(a => a.DoorId == door.Id);
            var activeCard = _keyCardRepository.GetActiveMemberCard(memberId);
            var cardNumber = activeCard?.CardNumber ?? string.Empty;

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
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            throw new InvalidOperationException($"Member {memberId} not found.");
        }

        var currentYear = DateTime.Today.Year;
        var eligibility = _keyCardService.GetKeyCardEligibility(memberId, currentYear);
        if (!eligibility.Eligible)
        {
            throw new InvalidOperationException("Member is not eligible for door access.");
        }

        var updatesList = updates.ToList();
        var doorIds = updatesList.Select(u => u.DoorId).Distinct().ToList();

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var existingAccess = await dbContext.MemberDoorAccesses
            .Where(m => m.MemberId == memberId && doorIds.Contains(m.DoorId))
            .ToListAsync(cancellationToken);

        var activeCard = _keyCardRepository.GetActiveMemberCard(memberId);
        if (activeCard == null || !activeCard.IsActive)
        {
            throw new InvalidOperationException("Member does not have an active key card assignment.");
        }
 
        var cardNumber = activeCard.CardNumber;

        foreach (var update in updatesList)
        {
            var existing = existingAccess.FirstOrDefault(a => a.DoorId == update.DoorId);
            
            if (existing != null)
            {
                existing.IsEnabled = update.IsEnabled;
                existing.TimeProfileId = update.TimeProfileId;
                existing.CardNumber = cardNumber; // Sync card number if changed
            }
            else
            {
                var newAccess = new GFC.BlazorServer.Data.Entities.MemberDoorAccess
                {
                    MemberId = memberId,
                    CardNumber = cardNumber,
                    DoorId = update.DoorId,
                    IsEnabled = update.IsEnabled,
                    TimeProfileId = update.TimeProfileId
                };
                dbContext.MemberDoorAccesses.Add(newAccess);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Updated door access permissions for member {MemberId} across {Count} doors", memberId, updatesList.Count);
    }

    public async Task SyncMemberPrivilegesAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            throw new InvalidOperationException($"Member {memberId} not found.");
        }

        var currentYear = DateTime.Today.Year;
        var eligibility = _keyCardService.GetKeyCardEligibility(memberId, currentYear);
        var activeCard = _keyCardRepository.GetActiveMemberCard(memberId);
        
        if (activeCard == null || !activeCard.IsActive)
        {
            _logger.LogWarning("Member {MemberId} has no active card assigned for sync", memberId);
            return;
        }
 
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var cardNumber = activeCard.CardNumber;
        var accessRecords = await dbContext.MemberDoorAccesses
            .Include(m => m.Door)
                .ThenInclude(d => d!.Controller)
            .Where(m => m.MemberId == memberId)
            .ToListAsync(cancellationToken);

        var controllers = await _controllerRegistryService.GetControllersAsync(includeDoors: true, cancellationToken);
        var syncResults = new List<string>();

        foreach (var controller in controllers)
        {
            if (!controller.IsEnabled) continue;
 
            var controllerDoors = controller.Doors.Where(d => d.IsEnabled).ToList();
            var controllerDoorIds = controllerDoors.Select(d => d.Id).ToList();
 
            // Find all access records for this member on this controller
            var controllerAccess = accessRecords.Where(a => controllerDoorIds.Contains(a.DoorId) && a.CardNumber == cardNumber).ToList();
            
            // Only doors where the user is eligible AND has enabled access
            var doorsWithAccess = controllerDoors
                .Where(d => eligibility.Eligible && controllerAccess.Any(a => a.DoorId == d.Id && a.IsEnabled))
                .ToList();
 
            try 
            {
                if (doorsWithAccess.Any())
                {
                    // Use the time profile from the first door with access (or first overall if we want to be safe)
                    // Note: N3000 typical models use one time profile index for all doors in a single 0x50 hit,
                    // or separate bytes if we use the full MjRegisterCard layout.
                    var firstAccess = controllerAccess.FirstOrDefault(a => a.IsEnabled) ?? controllerAccess.First();
                    var timeProfileIndex = await GetTimeProfileIndexForController(controller.Id, firstAccess.TimeProfileId, cancellationToken);
 
                    var cardRequest = new AddOrUpdateCardRequestDto
                    {
                        CardNumber = cardNumber,
                        DoorIndexes = doorsWithAccess.Select(d => d.DoorIndex).ToList(),
                        TimeProfileIndex = timeProfileIndex,
                        Enabled = true
                    };
 
                    var result = await _controllerClient.AddOrUpdateCardAsync(controller.SerialNumber.ToString(), cardRequest, cancellationToken);
                    
                    foreach (var access in controllerAccess)
                    {
                        access.LastSyncedAt = DateTime.UtcNow;
                        access.LastSyncResult = result.Success ? "Success" : result.Message;
                    }
                    
                    var doorNames = string.Join(", ", doorsWithAccess.Select(d => d.Name));
                    syncResults.Add($"{controller.Name}: {(result.Success ? $"✓ Activated: {doorNames}" : $"Failed: {result.Message}")}");
                }
                else
                {
                    // No enabled doors or member not eligible - send update with empty door list (Revoke All)
                    var cardRequest = new AddOrUpdateCardRequestDto
                    {
                        CardNumber = cardNumber,
                        DoorIndexes = new List<int>(), // Empty list = all doors denied
                        TimeProfileIndex = 1,
                        Enabled = false
                    };
                    
                    var result = await _controllerClient.AddOrUpdateCardAsync(controller.SerialNumber.ToString(), cardRequest, cancellationToken);
                    
                    foreach (var access in controllerAccess)
                    {
                        access.LastSyncedAt = DateTime.UtcNow;
                        access.LastSyncResult = result.Success ? "Access Revoked" : result.Message;
                    }

                    if (result.Success)
                    {
                         syncResults.Add($"{controller.Name}: ✗ Revoked All Access");
                    }
                    else 
                    {
                         syncResults.Add($"{controller.Name}: Sync Revoke Failed: {result.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to sync member {MemberId} to controller {Sn}", memberId, controller.SerialNumber);
                foreach (var access in controllerAccess)
                {
                    access.LastSyncedAt = DateTime.UtcNow;
                    access.LastSyncResult = "Sync Failed: Offline or Timeout";
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Sync results for member {MemberId}: {Results}", memberId, string.Join("; ", syncResults));
    }

    public async Task RemoveCardAsync(int memberId, string cardNumber, string? performedByUserName = null, int? performedByUserId = null, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var accessRecords = await dbContext.MemberDoorAccesses
            .Include(m => m.Door)
                .ThenInclude(d => d!.Controller)
            .Where(m => m.MemberId == memberId && m.CardNumber == cardNumber)
            .ToListAsync(cancellationToken);

        var controllers = accessRecords
            .Select(a => a.Door?.Controller)
            .Where(c => c != null)
            .Distinct()
            .ToList();

        foreach (var controller in controllers)
        {
            if (controller == null || !controller.IsEnabled) continue;

            var result = await _controllerClient.DeleteCardAsync(controller.SerialNumber.ToString(), cardNumber, cancellationToken);
            _logger.LogInformation("DeleteCard for controller {SerialNumber}: {Success}", controller.SerialNumber, result.Success);
        }

        foreach (var access in accessRecords)
        {
            access.IsEnabled = false;
            access.LastSyncedAt = DateTime.UtcNow;
            access.LastSyncResult = "Removed";
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        var performedBy = performedByUserName ?? "System";
        _historyService.LogChange(memberId, "DoorAccess", null, $"Removed card {cardNumber} from controllers", performedBy);
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

    private string GetStatusText(GFC.BlazorServer.Data.Entities.MemberDoorAccess? access, bool isEligible)
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

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var link = await dbContext.ControllerTimeProfileLinks
            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && l.TimeProfileId == timeProfileId.Value, cancellationToken);

        return link?.ControllerProfileIndex;
    }

    public async Task<EffectiveAccessResult> ComputeEffectiveAccessAsync(int memberId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            throw new InvalidOperationException($"Member {memberId} not found.");
        }

        var currentYear = DateTime.Today.Year;
        var activeCard = _keyCardRepository.GetActiveMemberCard(memberId);
        var cardNumber = activeCard?.CardNumber;
        var hasActiveCard = activeCard?.IsActive == true;

        var accessRecords = await dbContext.MemberDoorAccesses
            .Include(m => m.Door)
                .ThenInclude(d => d!.Controller)
            .Include(m => m.TimeProfile)
            .Where(m => m.MemberId == memberId && m.IsEnabled)
            .ToListAsync(cancellationToken);

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

    public async Task GrantDefaultAccessAsync(int memberId, string cardNumber, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        // Find all controllers that have at least one door
        var controllers = await dbContext.Controllers
            .Include(c => c.Doors)
            .Where(c => c.IsEnabled)
            .ToListAsync(cancellationToken);

        foreach (var controller in controllers)
        {
            // Default to granting access to Door 1 (Main entrance usually)
            var mainDoor = controller.Doors.FirstOrDefault(d => d.DoorIndex == 1 && d.IsEnabled);
            if (mainDoor == null) continue;

            // Check if access already exists
            var exists = await dbContext.MemberDoorAccesses
                .AnyAsync(a => a.MemberId == memberId && a.CardNumber == cardNumber && a.DoorId == mainDoor.Id, cancellationToken);

            if (!exists)
            {
                dbContext.MemberDoorAccesses.Add(new GFC.BlazorServer.Data.Entities.MemberDoorAccess
                {
                    MemberId = memberId,
                    CardNumber = cardNumber,
                    DoorId = mainDoor.Id,
                    IsEnabled = true,
                    TimeProfileId = null // Default to 24/7
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Granted default Door 1 access for card {CardNumber} to member {MemberId}", cardNumber, memberId);
    }
}
