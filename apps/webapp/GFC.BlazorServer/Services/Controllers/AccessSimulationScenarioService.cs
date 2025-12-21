using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

public interface IAccessSimulationScenarioService
{
    Task RunAssignAndSwipeScenarioAsync(CancellationToken ct = default);

    Task RunReplaceCardScenarioAsync(CancellationToken ct = default);

    Task RunScheduleAllowDenyScenarioAsync(CancellationToken ct = default);
}

public sealed class AccessSimulationScenarioService : IAccessSimulationScenarioService
{
    private const int DefaultControllerId = 99001;
    private const int DefaultDoorId = 1;
    private const long AssignSwipeCard = 9900000001;
    private const long OldCardNumber = 9900000010;
    private const long NewCardNumber = 9900000011;

    private readonly IControllerClient _controllerClient;
    private readonly ILogger<AccessSimulationScenarioService> _logger;

    public AccessSimulationScenarioService(IControllerClient controllerClient, ILogger<AccessSimulationScenarioService> logger)
    {
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RunAssignAndSwipeScenarioAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Running Assign and Swipe simulation scenario.");

        var privilege = new CardPrivilegeModel
        {
            ControllerId = DefaultControllerId,
            DoorId = DefaultDoorId,
            DoorIndex = DefaultDoorId,
            CardNumber = AssignSwipeCard
        };

        await _controllerClient.AddOrUpdatePrivilegeAsync(privilege, ct);
        await _controllerClient.OpenDoorAsync(DefaultControllerId, DefaultDoorId, ct);
        await _controllerClient.GetEventsByIndexAsync(DefaultControllerId, fromIndex: 0, maxCount: 5, ct);
    }

    public async Task RunReplaceCardScenarioAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Running Replace Card simulation scenario.");

        var oldPrivilege = new CardPrivilegeModel
        {
            ControllerId = DefaultControllerId,
            DoorId = DefaultDoorId,
            DoorIndex = DefaultDoorId,
            CardNumber = OldCardNumber
        };
        await _controllerClient.AddOrUpdatePrivilegeAsync(oldPrivilege, ct);
        await _controllerClient.DeletePrivilegeAsync(OldCardNumber, ct);

        var newPrivilege = new CardPrivilegeModel
        {
            ControllerId = DefaultControllerId,
            DoorId = DefaultDoorId,
            DoorIndex = DefaultDoorId,
            CardNumber = NewCardNumber
        };
        await _controllerClient.AddOrUpdatePrivilegeAsync(newPrivilege, ct);
        await _controllerClient.OpenDoorAsync(DefaultControllerId, DefaultDoorId, ct);
    }

    public async Task RunScheduleAllowDenyScenarioAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Running Schedule Allow/Deny simulation scenario.");

        await _controllerClient.GetRunStatusAsync(DefaultControllerId, ct);
        await _controllerClient.OpenDoorAsync(DefaultControllerId, DefaultDoorId, ct);
        await _controllerClient.OpenDoorAsync(DefaultControllerId, DefaultDoorId + 1, ct);
    }
}
