namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides the current controller mode (simulation vs real).
/// This is used by dependency injection to resolve the correct controller client.
/// </summary>
public interface IControllerModeProvider
{
    /// <summary>
    /// When true, controller operations should use real hardware via Agent PC.
    /// When false, controller operations should use simulation mode (no network packets).
    /// </summary>
    bool UseRealControllers { get; }
}

