using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Provides access to per-year dues configuration values.
/// </summary>
public interface IDuesYearSettingsRepository
{
    DuesYearSettings? GetSettingsForYear(int year);
}

