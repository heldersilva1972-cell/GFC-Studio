using System;
using System.Threading.Tasks;
using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services
{
    public interface IUpdateService
    {
        Task<bool> CheckForUpdatesAsync();
        Task<bool> DownloadAndInstallUpdateAsync(Action<double>? progressCallback = null);
        string CurrentVersion { get; }
        string ServerVersion { get; }
        bool IsUpdateAvailable { get; }
        bool IsAndroidPlatform { get; }
    }

    public class UpdateServiceMock : IUpdateService
    {
        private readonly IVersionService _versionService;

        public UpdateServiceMock(IVersionService versionService)
        {
            _versionService = versionService;
        }

        public string CurrentVersion => _versionService.GetRevision();
        public string ServerVersion => "Offline";
        public bool IsUpdateAvailable => false;
        public bool IsAndroidPlatform => false;

        public Task<bool> CheckForUpdatesAsync() => Task.FromResult(false);
        public Task<bool> DownloadAndInstallUpdateAsync(Action<double>? progressCallback = null) => Task.FromResult(true);
    }
}
