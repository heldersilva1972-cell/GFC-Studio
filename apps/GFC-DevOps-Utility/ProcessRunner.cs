using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GFCDevOpsUtility
{
    public class ProcessRunner
    {
        private readonly Action<string, bool> _logCallback;
        private Process? _currentProcess;

        public ProcessRunner(Action<string, bool> logCallback)
        {
            _logCallback = logCallback;
        }

        public void Abort()
        {
            try
            {
                if (_currentProcess != null && !_currentProcess.HasExited)
                {
                    _logCallback?.Invoke(">>> [ABORT] Sending termination signal to process tree...", false);
                    _currentProcess.Kill(true); // Kill process and all its child processes recursively
                }
            }
            catch (Exception ex)
            {
                _logCallback?.Invoke($"[ABORT WARNING] Failed to cleanly terminate process: {ex.Message}", true);
            }
        }

        public async Task<int> RunAsync(string fileName, string arguments, string workingDirectory)
        {
            using (var process = new Process())
            {
                _currentProcess = process;
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        _logCallback?.Invoke(e.Data, false);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        _logCallback?.Invoke(e.Data, true);
                    }
                };

                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    _logCallback?.Invoke($"CRITICAL ERROR: Failed to start process '{fileName}'. {ex.Message}", true);
                    _currentProcess = null;
                    return -1;
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                try
                {
                    await process.WaitForExitAsync();
                }
                catch (TaskCanceledException)
                {
                    // Handle cancellation gracefully
                }
                finally
                {
                    _currentProcess = null;
                }

                return process.ExitCode;
            }
        }
    }
}
