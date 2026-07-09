using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Pos.UI.Services;

#if ANDROID
using Android.App;
using Android.Content;
using Android.Content.PM;
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

            // 2. Resumable Download Loop with 30-Minute Timeout and 50 Retries
            using (var cleanHttp = new HttpClient())
            {
                cleanHttp.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36");
                cleanHttp.Timeout = TimeSpan.FromMinutes(30);

                long totalRead = 0;
                long? contentLength = null;
                int retries = 50;
                bool downloadComplete = false;

                // Query total file size first via HEAD or a short GET request
                try
                {
                    using (var headRequest = new HttpRequestMessage(HttpMethod.Head, apkUrl))
                    {
                        using (var headResponse = await cleanHttp.SendAsync(headRequest))
                        {
                            if (headResponse.IsSuccessStatusCode)
                            {
                                contentLength = headResponse.Content.Headers.ContentLength;
                            }
                        }
                    }
                }
                catch { }

                if (!contentLength.HasValue)
                {
                    try
                    {
                        using (var response = await cleanHttp.GetAsync(apkUrl, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();
                            contentLength = response.Content.Headers.ContentLength;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[GFC UPDATE ERROR] Initial connection failed: {ex.Message}");
                        return false;
                    }
                }

                if (!contentLength.HasValue || contentLength.Value <= 0)
                {
                    Console.WriteLine("[GFC UPDATE ERROR] Could not determine server APK size.");
                    return false;
                }

                // If existing local file is corrupted/larger than the server size, delete it
                if (File.Exists(tempApkPath))
                {
                    var fileInfo = new FileInfo(tempApkPath);
                    if (fileInfo.Length >= contentLength.Value)
                    {
                        File.Delete(tempApkPath);
                    }
                    else
                    {
                        totalRead = fileInfo.Length;
                        Console.WriteLine($"[GFC UPDATE] Found existing partial download. Resuming from {totalRead} bytes.");
                    }
                }

                while (retries > 0 && !downloadComplete)
                {
                    try
                    {
                        using (var request = new HttpRequestMessage(HttpMethod.Get, apkUrl))
                        {
                            if (totalRead > 0)
                            {
                                request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(totalRead, null);
                            }

                            using (var response = await cleanHttp.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                            {
                                // If server returns 200 OK instead of 206 PartialContent when range is specified, restart from 0
                                if (response.StatusCode == System.Net.HttpStatusCode.OK && totalRead > 0)
                                {
                                    totalRead = 0;
                                    if (File.Exists(tempApkPath)) File.Delete(tempApkPath);
                                }
                                else
                                {
                                    response.EnsureSuccessStatusCode();
                                }

                                using (var downloadStream = await response.Content.ReadAsStreamAsync())
                                using (var fileStream = new FileStream(tempApkPath, totalRead > 0 ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                                {
                                    var buffer = new byte[8192];
                                    int bytesRead;

                                    while ((bytesRead = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                    {
                                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                                        totalRead += bytesRead;

                                        if (progressCallback != null)
                                        {
                                            double progress = (double)totalRead / contentLength.Value;
                                            progressCallback.Invoke(progress);
                                        }
                                    }
                                }
                            }
                        }

                        if (totalRead == contentLength.Value)
                        {
                            downloadComplete = true;
                        }
                        else
                        {
                            throw new IOException($"Download stream closed prematurely. Received {totalRead} of {contentLength.Value} bytes.");
                        }
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        Console.WriteLine($"[GFC UPDATE] Download failed: {ex.Message}. Retries remaining: {retries}");
                        if (retries > 0)
                        {
                            await Task.Delay(2000); // Wait 2 seconds before retrying
                        }
                        else
                        {
                            return false; // Retries exhausted
                        }
                    }
                }
            }

            Console.WriteLine($"[GFC UPDATE] Download complete. Verified {new FileInfo(tempApkPath).Length} bytes. Saved to: {tempApkPath}");

            // 3. Silent Installation via PackageInstaller API
            var context = Android.App.Application.Context;
            var packageManager = context.PackageManager;
            if (packageManager == null) return false;

            var packageInstaller = packageManager.PackageInstaller;
            if (packageInstaller == null) return false;

            var sessionParams = new PackageInstaller.SessionParams(PackageInstallMode.FullInstall);
            int sessionId = -1;
            PackageInstaller.Session? session = null;

            try
            {
                sessionId = packageInstaller.CreateSession(sessionParams);
                session = packageInstaller.OpenSession(sessionId);

                using (var apkStream = new FileStream(tempApkPath, FileMode.Open, FileAccess.Read))
                using (var sessionStream = session.OpenWrite("package_update", 0, apkStream.Length))
                {
                    byte[] buffer = new byte[65536];
                    int bytesRead;
                    while ((bytesRead = apkStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        sessionStream.Write(buffer, 0, bytesRead);
                    }
                    session.Fsync(sessionStream);
                }

                const string action = "com.gfc.pos.mobile.SESSION_API_PACKAGE_INSTALLED";
                var intent = new Intent(context, typeof(PackageInstallReceiver));
                intent.SetAction(action);

                var pendingIntentFlags = PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable;
                var pendingIntent = PendingIntent.GetBroadcast(context, sessionId, intent, pendingIntentFlags);

                session.Commit(pendingIntent!.IntentSender);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GFC UPDATE ERROR] Silent install session failed: {ex.Message}");
                session?.Abandon();
                return false;
            }
            finally
            {
                session?.Dispose();
            }
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

#if ANDROID
[BroadcastReceiver(Name = "com.gfc.pos.mobile.PackageInstallReceiver", Exported = true)]
[IntentFilter(new[] { "com.gfc.pos.mobile.SESSION_API_PACKAGE_INSTALLED" })]
public class PackageInstallReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent == null) return;
        var status = (PackageInstallStatus)intent.GetIntExtra(PackageInstaller.ExtraStatus, (int)PackageInstallStatus.Failure);

        switch (status)
        {
            case PackageInstallStatus.Success:
                Console.WriteLine("[GFC POS UPDATE] Silent update completed successfully.");
                break;
            case PackageInstallStatus.PendingUserAction:
                Console.WriteLine("[GFC POS UPDATE] Pending user action status received. Setting Device Owner mode might have failed.");
                var confirmIntent = (Intent?)intent.GetParcelableExtra(Intent.ExtraIntent);
                if (confirmIntent != null)
                {
                    confirmIntent.AddFlags(ActivityFlags.NewTask);
                    context.StartActivity(confirmIntent);
                }
                break;
            default:
                var message = intent.GetStringExtra(PackageInstaller.ExtraStatusMessage);
                Console.WriteLine($"[GFC POS UPDATE ERROR] Silent install failed: {status} - {message}");
                break;
        }
    }
}
#endif
