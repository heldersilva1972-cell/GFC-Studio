namespace GFC.BlazorServer.Configuration;

/// <summary>
/// Options controlling whether access control operations run in simulation mode.
/// </summary>
public sealed class AccessControlSimulationOptions
{
    /// <summary>
    /// When true, controller operations use the simulation client instead of the real client.
    /// </summary>
    public bool UseAccessControlSimulation { get; set; } = false;
}
