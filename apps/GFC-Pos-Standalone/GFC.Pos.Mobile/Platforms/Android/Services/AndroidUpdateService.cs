using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Pos.UI.Services;

#if ANDROID
using Android.Content;
using Android.Net;
using Microsoft.Maui.ApplicationModel;
#endif

namespace GFC.Pos.Mobile.Services;

public class AndroidUpdateService : IUpdateService
{
    private readonly HttpClient _http;
    private readonly IPosTerminalService _terminalService;
    private readonly IVersionService _versionService;

    public string CurrentVersion => _versionService.GetRevision();
    public string ServerVersion { get; private set; } = "Offline";
    public bool IsUpdateAvailable { get; private set; }
    public bool IsAndroidPlatform => true;

    public AndroidUpdateService(HttpClient http, IPosTerminalService terminalService, IVersionService versionService)
    {
        _http = http;
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

    public async Task<bool> DownloadAndInstallUpdateAsync(Action<double>? progressCallback = null)
    {
#if ANDROID
        try
        {
            Console.WriteLine("[GFC UPDATE] Starting native Android APK downloader...");
            
            // 1. Resolve Server Domain / Address (Always download APK from the POS domain, not the API/WebApp domain)
            string domain = "https://pos.lovanow.com";
            var apkUrl = $"{domain}/Download/GFC_POS_Mobile.apk?v={DateTime.UtcNow.Ticks}";
            var tempApkPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.CacheDirectory, "update.apk");

            if (File.Exists(tempApkPath))
            {
                File.Delete(tempApkPath);
            }

            // 2. Download APK with Progress (Use a clean HttpClient with a standard mobile User-Agent to bypass Cloudflare bot blocks)
            using (var cleanHttp = new HttpClient())
            {
                cleanHttp.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36");
                
                using (var response = await cleanHttp.GetAsync(apkUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var contentLength = response.Content.Headers.ContentLength;

                    using (var downloadStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(tempApkPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        var buffer = new byte[8192];
                        long totalRead = 0;
                        int bytesRead;

                        while ((bytesRead = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;

                            if (contentLength.HasValue && progressCallback != null)
                            {
                                double progress = (double)totalRead / contentLength.Value;
                                progressCallback.Invoke(progress);
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"[GFC UPDATE] Download complete. Saved to: {tempApkPath}");

            // 3. Launch System Package Installer using FileProvider
            var context = Android.App.Application.Context;
            var file = new Java.IO.File(tempApkPath);
            
            // Safe URI sharing using whitelisted fileprovider defined in AndroidManifest.xml
            var apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(
                context, 
                "com.gfc.pos.mobile.fileprovider", 
                file
            );

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(apkUri, "application/vnd.android.package-archive");
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.NewTask);

            context.StartActivity(intent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GFC UPDATE ERROR] Native Android installation failed: {ex.Message}");
            return false;
        }
#else
        return await Task.FromResult(false);
#endif
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
