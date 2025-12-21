using System;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Exception thrown when a dangerous controller operation is blocked because the app is running in Simulation Mode.
/// </summary>
public class SimulationModeBlockedException : Exception
{
    public SimulationModeBlockedException(string message)
        : base(message)
    {
    }
}

