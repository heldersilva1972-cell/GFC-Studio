using System;
using GFC.BlazorServer.Services;

namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Service that provides information about the current simulation mode.
/// </summary>
public class SimulationModeService : ISimulationModeService
{
    private readonly IControllerModeProvider _modeProvider;

    public SimulationModeService(IControllerModeProvider modeProvider)
        => _modeProvider = modeProvider ?? throw new ArgumentNullException(nameof(modeProvider));

    public bool UseRealControllers => _modeProvider.UseRealControllers;

    public bool IsSimulation => !_modeProvider.UseRealControllers;
    
    public string ModeLabel => UseRealControllers ? "Live Controllers" : "Simulation Mode";
}
