using System;
using System.Threading.Tasks;

namespace GFC.Pos.UI.Services;

public interface IUpdateService
{
    Task<bool> CheckForUpdatesAsync();
    Task<bool> DownloadAndInstallUpdateAsync(Action<double>? progressCallback = null);
    string CurrentVersion { get; }
    string ServerVersion { get; }
    bool IsUpdateAvailable { get; }
    bool IsAndroidPlatform { get; }
}
