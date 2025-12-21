namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Service interface for controlling the simulation engine.
/// </summary>
public interface ISimulationEngineControlService
{
    Task AdvanceTimeAsync(TimeSpan delta);
    Task ResetControllerStateAsync(int controllerId, CancellationToken cancellationToken = default);
    Task InjectEventAsync(int controllerId, int doorIndex, long? cardNumber, int eventType, DateTime timestamp, CancellationToken cancellationToken = default);
    Task ConfigureScenarioAsync(int controllerId, SimulationScenarioConfig config, CancellationToken cancellationToken = default);
}
