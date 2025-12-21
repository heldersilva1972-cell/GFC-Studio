namespace GFC.BlazorServer.Configuration;

/// <summary>
/// Configuration options for the simulation controller engine.
/// </summary>
public class SimControllerOptions
{
    /// <summary>
    /// Tick interval in milliseconds. Default is 1000ms (1 second).
    /// </summary>
    public int TickIntervalMs { get; set; } = 1000;

    /// <summary>
    /// Enable random synthetic card swipes for testing.
    /// </summary>
    public bool RandomCardSwipesEnabled { get; set; } = false;

    /// <summary>
    /// Probability of a synthetic card swipe per tick per door (0.0 to 1.0).
    /// Only used when RandomCardSwipesEnabled is true.
    /// </summary>
    public double SyntheticSwipeChance { get; set; } = 0.001;
}
