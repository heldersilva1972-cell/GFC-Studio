using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public interface IStudioEngineService
    {
        bool IsRunning { get; }
        string? LastError { get; }
        List<string> Logs { get; }
        Task StartAsync(string projectPath);
        Task StopAsync();
        event Action OnStatusChanged;
        event Action<string> OnLogReceived;
    }

    public class StudioEngineService : IStudioEngineService, IDisposable
    {
        private Process? _process;
        private readonly ILogger<StudioEngineService> _logger;
        private readonly List<string> _logs = new();
        private const int MaxLogs = 200;

        public bool IsRunning => _process != null && !_process.HasExited;
        public string? LastError { get; private set; }
        public List<string> Logs => _logs;

        public event Action? OnStatusChanged;
        public event Action<string>? OnLogReceived;

        public StudioEngineService(ILogger<StudioEngineService> logger)
        {
            _logger = logger;
        }

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public async Task StartAsync(string projectPath)
        {
            if (IsRunning) return;
            
            await _lock.WaitAsync();
            try
            {
                if (IsRunning) return;

                // 0. Clean up any ghost processes on port 3000
                ForceKillPort3000();

                // 1. Try to find the website engine. 
                // It could be in projectPath/apps/website (if opening workspace root)
                // or in ../../website relative to the Blazor app
                // or we look for it in the parent of the projectPath
                
                string websitePath = Path.Combine(projectPath, "apps", "website");

                if (!Directory.Exists(websitePath))
                {
                    // Fallback: Check if we are inside a subfolder and the engine is at the workspace root
                    var parent = Directory.GetParent(projectPath)?.FullName;
                    if (parent != null)
                    {
                        var parentWebsite = Path.Combine(parent, "apps", "website");
                        if (Directory.Exists(parentWebsite)) 
                            websitePath = parentWebsite;
                    }
                }

                // Fallback 2: Absolute known workspace locations
                if (!Directory.Exists(websitePath))
                {
                     var workspaceRoot = @"c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2";
                     var absoluteWebsite = Path.Combine(workspaceRoot, "apps", "website");
                     if (Directory.Exists(absoluteWebsite))
                         websitePath = absoluteWebsite;
                }

                if (!Directory.Exists(websitePath))
                {
                    LastError = $"Could not find Rendering Engine (apps/website). Searched in {projectPath}, its parent, and default workspace. Ensure 'apps/website' exists.";
                    _logger.LogError(LastError);
                    AddLog("ERROR: Rendering Engine folder not found.", true);
                    OnStatusChanged?.Invoke();
                    return;
                }

                AddLog($"Initializing Engine in {websitePath}...");
                _logs.Clear();
                LastError = null;

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c npm run dev",
                    WorkingDirectory = websitePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Bypass SSL certificate validation for development
                startInfo.EnvironmentVariables["NODE_TLS_REJECT_UNAUTHORIZED"] = "0";
                startInfo.EnvironmentVariables["NEXT_TELEMETRY_DISABLED"] = "1";

                _process = new Process { StartInfo = startInfo };
                _process.OutputDataReceived += (s, e) => {
                    if (e.Data != null) AddLog(e.Data);
                };
                _process.ErrorDataReceived += (s, e) => {
                    if (e.Data != null) AddLog(e.Data, true);
                };

                _process.Start();
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();

                _logger.LogInformation("Started Next.js Engine in {Path}", websitePath);
                OnStatusChanged?.Invoke();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                _logger.LogError(ex, "Failed to start Next.js Engine");
                OnStatusChanged?.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }

        private void ForceKillPort3000()
        {
            try
            {
                AddLog("Checking for ghost processes on port 3000...");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c for /f \"tokens=5\" %a in ('netstat -aon ^| findstr :3000') do taskkill /f /pid %a",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    }
                };
                process.Start();
                if (process.WaitForExit(3000))
                {
                     AddLog("Clean-up complete.");
                }
            }
            catch { /* Ignore errors if port already clear */ }
        }

        public async Task StopAsync()
        {
            if (_process != null && !_process.HasExited)
            {
                try
                {
                    // Kill the process tree (since cmd.exe spawns node)
                    _process.Kill(true);
                    await _process.WaitForExitAsync();
                }
                catch { }
            }
            _process = null;
            OnStatusChanged?.Invoke();
        }

        private void AddLog(string? data, bool isError = false)
        {
            if (string.IsNullOrEmpty(data)) return;

            lock (_logs)
            {
                _logs.Add(data);
                if (_logs.Count > MaxLogs) _logs.RemoveAt(0);
            }

            if (isError) _logger.LogWarning("Engine: {Data}", data);
            OnLogReceived?.Invoke(data);
        }

        public void Dispose()
        {
            StopAsync().Wait();
            _process?.Dispose();
        }
    }
}
