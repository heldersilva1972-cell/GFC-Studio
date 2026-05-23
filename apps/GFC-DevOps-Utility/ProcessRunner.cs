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

        public ProcessRunner(Action<string, bool> logCallback)
        {
            _logCallback = logCallback;
        }

        public async Task<int> RunAsync(string fileName, string arguments, string workingDirectory)
        {
            using (var process = new Process())
            {
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
                    return -1;
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();
                return process.ExitCode;
            }
        }
    }
}
