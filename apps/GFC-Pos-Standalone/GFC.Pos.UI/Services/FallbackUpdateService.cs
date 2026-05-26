using System;
using System.Linq;
using System.Threading.Tasks;
using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services;

public class FallbackUpdateService : IUpdateService
{
    private readonly IPosTerminalService _terminalService;
    private readonly IVersionService _versionService;

    public string CurrentVersion => _versionService.GetRevision();
    public string ServerVersion { get; private set; } = "Offline";
    public bool IsUpdateAvailable { get; private set; }
    public bool IsAndroidPlatform => false;

    public FallbackUpdateService(IPosTerminalService terminalService, IVersionService versionService)
    {
        _terminalService = terminalService;
        _versionService = versionService;
    }

    public async Task<bool> CheckForUpdatesAsync()
    {
        try
        {
            var serverVer = await _terminalService.GetServerVersionAsync();
            ServerVersion = serverVer;
            if (string.IsNullOrEmpty(serverVer) || serverVer == "Offline")
            {
                IsUpdateAvailable = false;
                return false;
            }

            IsUpdateAvailable = IsServerNewer(serverVer, CurrentVersion);
            return IsUpdateAvailable;
        }
        catch
        {
            IsUpdateAvailable = false;
            return false;
        }
    }

    public Task<bool> DownloadAndInstallUpdateAsync(Action<double>? progressCallback = null)
    {
        // No native package installation on standard browser or Windows fallbacks.
        // We return true to let the UI know it can perform a standard browser reload.
        return Task.FromResult(true);
    }

    private bool IsServerNewer(string serverFullVersion, string clientRev)
    {
        try
        {
            var parts = serverFullVersion.Split(new[] { "Revision ", "v.", "v" }, StringSplitOptions.RemoveEmptyEntries);
            var serverRev = parts.LastOrDefault()?.Trim();
            
            if (string.IsNullOrEmpty(serverRev) || string.IsNullOrEmpty(clientRev)) return false;
            if (serverRev.Contains(" ")) serverRev = serverRev.Split(' ')[0];

            if (serverRev.StartsWith("3.") && clientRev.StartsWith("2.")) return false;

            var sParts = serverRev.Split('.').Select(p => int.TryParse(p, out int v) ? v : 0).ToArray();
            var cParts = clientRev.Split('.').Select(p => int.TryParse(p, out int v) ? v : 0).ToArray();

            for (int i = 0; i < Math.Max(sParts.Length, cParts.Length); i++)
            {
                int s = i < sParts.Length ? sParts[i] : 0;
                int c = i < cParts.Length ? cParts[i] : 0;
                if (s > c) return true;
                if (s < c) return false;
            }
            return false;
        }
        catch { return false; }
    }
}
