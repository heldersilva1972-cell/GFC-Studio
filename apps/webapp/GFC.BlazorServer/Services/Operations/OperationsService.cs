using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFC.Data;
using GFC.Core.Interfaces;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Operations
{
    public class OperationsService : IOperationsService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseBackupService _backupService;
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<OperationsService> _logger;

        public OperationsService(
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IDatabaseBackupService backupService,
            IDbContextFactory<GfcDbContext> dbFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<OperationsService> logger)
        {
            _environment = environment;
            _configuration = configuration;
            _backupService = backupService;
            _dbFactory = dbFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<OperationsHealthInfo> GetHealthInfoAsync()
        {
            var info = new OperationsHealthInfo
            {
                EnvironmentName = _environment.EnvironmentName,
                AppVersion = GetType().Assembly.GetName().Version?.ToString() ?? "Unknown",
                BuildDate = File.GetLastWriteTimeUtc(GetType().Assembly.Location)
            };

            // Database Connectivity
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                info.DatabaseConnected = await db.Database.CanConnectAsync();
            }
            catch
            {
                info.DatabaseConnected = false;
            }

            // Disk Space (Drive where app is running)
            try
            {
                var drive = new DriveInfo(Path.GetPathRoot(_environment.ContentRootPath));
                long freeSpaceGb = drive.AvailableFreeSpace / 1024 / 1024 / 1024;
                long totalSpaceGb = drive.TotalSize / 1024 / 1024 / 1024;
                info.DiskSpaceMessage = $"{freeSpaceGb} GB free of {totalSpaceGb} GB";
            }
            catch (Exception ex)
            {
                info.DiskSpaceMessage = "Unknown";
                _logger.LogWarning(ex, "Failed to get disk space");
            }

            // Reverse Proxy / HTTPS
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                info.IsHttps = context.Request.IsHttps;
                info.IsReverseProxyDetected = context.Request.Headers.ContainsKey("X-Forwarded-Proto") || 
                                              context.Request.Headers.ContainsKey("X-Forwarded-Host");
            }

            // Cloudflared process check
            info.CloudflaredRunning = CheckIfProcessRunning("cloudflared") || CheckIfServiceRunning("cloudflared");

            info.IsHealthy = info.DatabaseConnected && info.CloudflaredRunning; // Basic health definition

            return info;
        }

        public async Task<PublicAccessInfo> GetPublicAccessInfoAsync()
        {
            var info = new PublicAccessInfo();
            
            // Get configured domain
            using var db = await _dbFactory.CreateDbContextAsync();
            var settings = await db.SystemSettings.FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(settings?.PrimaryDomain))
            {
                info.Domains.Add(settings.PrimaryDomain);
            }

            info.ConfigPath = @"C:\ProgramData\cloudflared\config.yml";
            info.IsCloudflaredInstalled = File.Exists(@"C:\Program Files (x86)\cloudflared\cloudflared.exe") || 
                                          File.Exists(@"C:\Program Files\cloudflared\cloudflared.exe");
            
            info.IsCloudflaredRunning = CheckIfServiceRunning("cloudflared");
            
            // Try to extract Tunnel ID from config if possible (requires read access)
            if (File.Exists(info.ConfigPath))
            {
                try
                {
                    var lines = await File.ReadAllLinesAsync(info.ConfigPath);
                    var tunnelLine = lines.FirstOrDefault(l => l.Contains("tunnel:"));
                    if (tunnelLine != null)
                    {
                        info.TunnelId = tunnelLine.Replace("tunnel:", "").Trim();
                    }
                }
                catch { /* Ignore access errors */ }
            }

            // DNS Check (Simple simulation)
            info.CanResolvePublicDns = true; // Placeholder for actual DNS lookup check

            return info;
        }

        public async Task<HostingInfo> GetHostingInfoAsync()
        {
            var info = new HostingInfo
            {
                PhysicalPath = _environment.ContentRootPath
            };

            // IIS Info is hard to get without Admin/PS, but we can infer some
            info.IisSiteName = Environment.GetEnvironmentVariable("APP_POOL_ID") ?? "Unknown (Not in IIS?)";
            info.AppPoolName = Environment.GetEnvironmentVariable("APP_POOL_ID") ?? "Unknown";

            // Hosting Bundle check
            info.DotNetHostingBundleVersion = Environment.Version.ToString();

            return Task.FromResult(info).Result;
        }

        public async Task<DatabaseRecoveryInfo> GetDatabaseRecoveryInfoAsync()
        {
            var info = new DatabaseRecoveryInfo();
            
            using var db = await _dbFactory.CreateDbContextAsync();
            var connStr = db.Database.GetConnectionString();
            
            if (string.IsNullOrEmpty(connStr)) return info;

            var builder = new SqlConnectionStringBuilder(connStr);
            info.InstanceName = builder.DataSource;
            info.DatabaseName = builder.InitialCatalog;
            info.ConnectionStringMasked = $"Server={builder.DataSource};Database={builder.InitialCatalog};User Id=***;Password=***;";

            // Get Backup Info (Assuming DatabaseBackupService exposes config logic, otherwise we read it manually or add a method to interface)
            // For now, let's just use what we can from a helper or assume defaults
            info.BackupLocation = "Configured Backup Path"; // Needs implementation in IBackupService to expose this
            
            // Query for last backup
             try
            {
                // This query works on SQL Server to get last backup time
                var sql = "SELECT MAX(backup_finish_date) FROM msdb.dbo.backupset WHERE database_name = @dbName AND type = 'D'";
                var cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;
                var p = cmd.CreateParameter();
                p.ParameterName = "@dbName";
                p.Value = info.DatabaseName;
                cmd.Parameters.Add(p);

                await db.Database.OpenConnectionAsync();
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    info.LastBackupTime = (DateTime)result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not retrieve last backup time from SQL Server");
            }

            return info;
        }

        public Task<NetworkSecurityInfo> GetNetworkSecurityInfoAsync()
        {
            return Task.FromResult(new NetworkSecurityInfo
            {
                OutboundPorts = new List<string> { "443 (HTTPS)", "7844 (Cloudflare QUIC)" },
                LocalDnsResolver = "System Default",
                TimeSyncStatus = true // Placeholder
            });
        }

        public async Task<List<DiagnosticEntry>> RunDiagnosticsAsync()
        {
            var logs = new List<DiagnosticEntry>();

            // Helper to record logs
            void Log(string component, string status, string msg, long ms = 0)
            {
                logs.Add(new DiagnosticEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Component = component,
                    Status = status,
                    Message = msg,
                    DurationMs = ms
                });
            }

            // 1. Host Connectivity (Google DNS Ping)
            var sw = Stopwatch.StartNew();
            try 
            {
                using var ping = new System.Net.NetworkInformation.Ping();
                var result = await ping.SendPingAsync("8.8.8.8", 2000);
                sw.Stop();
                if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                    Log("Internet", "Success", $"Outbound connectivity verified (Ping: {result.RoundtripTime}ms)", sw.ElapsedMilliseconds);
                else
                    Log("Internet", "Warning", $"Ping 8.8.8.8 failed: {result.Status}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("Internet", "Failed", $"Outbound connectivity error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 2. Database Functional Check
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var canConnect = await db.Database.CanConnectAsync();
                if (canConnect) 
                {
                    var cmd = db.Database.GetDbConnection().CreateCommand();
                    cmd.CommandText = "SELECT 1";
                    await db.Database.OpenConnectionAsync();
                    await cmd.ExecuteScalarAsync();
                    sw.Stop();
                    Log("Database", "Success", "Connection & Read Query Validated", sw.ElapsedMilliseconds);
                }
                else
                {
                    sw.Stop();
                    Log("Database", "Failed", "CanConnectAsync returned false", sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("Database", "Failed", $"DB Connection Error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 3. Local Web Server (Loopback)
            sw.Restart();
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(2);
                var response = await client.GetAsync("http://localhost/"); 
                sw.Stop();
                Log("LocalHost", "Success", $"Local Web Server responded: {response.StatusCode}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("LocalHost", "Warning", $"Local HTTP Check Failed: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 4. Cloudflared Service Status
            sw.Restart();
            var serviceRunning = CheckIfServiceRunning("cloudflared");
            if (serviceRunning)
            {
                Log("TunnelService", "Success", "Cloudflared Service is Running", 0);
            }
            else
            {
                Log("TunnelService", "Failed", "Cloudflared Service is NOT Running", 0);
            }

            // 5. DNS Resolution for Public Domain
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var settings = await db.SystemSettings.FirstOrDefaultAsync();
                var domain = settings?.PrimaryDomain ?? "gfc.lovanow.com";
                
                var addresses = await System.Net.Dns.GetHostAddressesAsync(domain);
                sw.Stop();
                if (addresses.Length > 0)
                {
                    var ipList = string.Join(", ", addresses.Select(a => a.ToString()));
                    Log("DNS Resolution", "Success", $"{domain} → {ipList}", sw.ElapsedMilliseconds);
                }
                else
                {
                    Log("DNS Resolution", "Failed", $"{domain} did not resolve", sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("DNS Resolution", "Failed", $"DNS lookup error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 6. Public Domain HTTPS Check
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var settings = await db.SystemSettings.FirstOrDefaultAsync();
                var domain = settings?.PrimaryDomain ?? "gfc.lovanow.com";
                
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                var response = await client.GetAsync($"https://{domain}/api/health");
                sw.Stop();
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Log("Public HTTPS", "Success", $"https://{domain}/api/health → {response.StatusCode}: {content}", sw.ElapsedMilliseconds);
                }
                else
                {
                    Log("Public HTTPS", "Warning", $"https://{domain}/api/health → {response.StatusCode}", sw.ElapsedMilliseconds);
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                sw.Stop();
                Log("Public HTTPS", "Failed", $"HTTPS request error: {ex.Message} | Inner: {ex.InnerException?.Message}", sw.ElapsedMilliseconds);
            }
            catch (TaskCanceledException)
            {
                sw.Stop();
                Log("Public HTTPS", "Failed", "Request timed out after 10 seconds", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("Public HTTPS", "Failed", $"Unexpected error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 7. Database Connection Details & Version
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var connStr = db.Database.GetConnectionString();
                var builder = new SqlConnectionStringBuilder(connStr);
                
                var cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "SELECT @@VERSION";
                await db.Database.OpenConnectionAsync();
                var version = await cmd.ExecuteScalarAsync();
                sw.Stop();
                
                var versionShort = version?.ToString()?.Split('\n')[0] ?? "Unknown";
                Log("DB Version", "Success", $"{builder.DataSource}\\{builder.InitialCatalog} - {versionShort}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("DB Version", "Failed", $"Cannot retrieve version: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 8. Database Table & Data Verification
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var userCount = await db.AuthorizedUsers.CountAsync();
                var settingsExists = await db.SystemSettings.AnyAsync();
                sw.Stop();
                
                Log("DB Schema", "Success", $"Users: {userCount}, SystemSettings: {(settingsExists ? "Configured" : "Missing")}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("DB Schema", "Failed", $"Schema check error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 9. Tunnel Configuration File Analysis
            sw.Restart();
            var configPath = @"C:\ProgramData\cloudflared\config.yml";
            if (File.Exists(configPath))
            {
                try
                {
                    var configContent = await File.ReadAllTextAsync(configPath);
                    var tunnelId = "Not found";
                    var url = "Not found";
                    
                    foreach (var line in configContent.Split('\n'))
                    {
                        if (line.Contains("tunnel:")) tunnelId = line.Split(':')[1].Trim();
                        if (line.Contains("url:")) url = line.Split(':')[1].Trim();
                    }
                    
                    Log("Tunnel Config", "Success", $"Tunnel: {tunnelId} → Target: {url}", 0);
                }
                catch (Exception ex)
                {
                    Log("Tunnel Config", "Warning", $"Config exists but unreadable: {ex.Message}", 0);
                }
            }
            else
            {
                Log("Tunnel Config", "Failed", $"Config not found at {configPath}", 0);
            }

            // 10. Request Context & Path Analysis
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                var scheme = context.Request.Scheme;
                var host = context.Request.Host.ToString();
                var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var forwardedProto = context.Request.Headers["X-Forwarded-Proto"].ToString();
                var forwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
                
                var pathInfo = $"{scheme}://{host} from {remoteIp}";
                if (!string.IsNullOrEmpty(forwardedProto))
                    pathInfo += $" | X-Forwarded-Proto: {forwardedProto}";
                if (!string.IsNullOrEmpty(forwardedFor))
                    pathInfo += $" | X-Forwarded-For: {forwardedFor}";
                
                var status = (scheme == "https" || forwardedProto == "https") ? "Success" : "Warning";
                Log("Request Path", status, pathInfo, 0);
            }
            else
            {
                Log("Request Path", "Warning", "No HTTP context (running outside web request)", 0);
            }

            return logs;
        }

        public async Task<byte[]> GenerateRecoveryPackAsync()
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                // 1. README_RECOVERY.md
                await AddFileToZipAsync(archive, "README_RECOVERY.md", GetReadmeContent());

                // 2. CHECKLIST.md
                await AddFileToZipAsync(archive, "CHECKLIST.md", GetChecklistContent());

                // 3. SYSTEM_SNAPSHOT.json
                var snapshot = new
                {
                    GeneratedAt = DateTime.UtcNow,
                    Health = await GetHealthInfoAsync(),
                    Hosting = await GetHostingInfoAsync(),
                    Database = await GetDatabaseRecoveryInfoAsync()
                };
                await AddFileToZipAsync(archive, "SYSTEM_SNAPSHOT.json", System.Text.Json.JsonSerializer.Serialize(snapshot, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                // 4. Scripts
                await AddFileToZipAsync(archive, "IIS_RESTORE.ps1", GetIisRestoreScript(snapshot.Hosting));
                await AddFileToZipAsync(archive, "SQL_BACKUP_RESTORE.ps1", GetSqlRestoreScript(snapshot.Database));
                await AddFileToZipAsync(archive, "CLOUDFLARED_RESTORE.ps1", GetCloudflaredRestoreScript());
                await AddFileToZipAsync(archive, "VERIFY_ONLINE.ps1", GetVerifyOnlineScript());
                
                // 5. Docs
                await AddFileToZipAsync(archive, "KNOWN_DEPENDENCIES.md", GetKnownDependenciesContent());
                await AddFileToZipAsync(archive, "TROUBLESHOOTING.md", GetTroubleshootingContent());
            }

            return memoryStream.ToArray();
        }

        private async Task AddFileToZipAsync(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName);
            using var entryStream = entry.Open();
            using var streamWriter = new StreamWriter(entryStream);
            await streamWriter.WriteAsync(content);
        }

        // Helpers
        private bool CheckIfProcessRunning(string processName)
        {
            return Process.GetProcessesByName(processName).Any();
        }

        private bool CheckIfServiceRunning(string serviceName)
        {
            try
            {
                // This is a rough check using SC command if Access Denied for ServiceController
                var startInfo = new ProcessStartInfo
                {
                    FileName = "sc",
                    Arguments = $"query \"{serviceName}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(startInfo);
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Contains("RUNNING");
            }
            catch
            {
                return false;
            }
        }

        // Content Generators
        private string GetReadmeContent() => 
@"# Disaster Recovery Pack

This pack contains the necessary scripts and documentation to restore the GFC Studio application stack from scratch.

## Contents
- **CHECKLIST.md**: Step-by-step restoration guide.
- **SYSTEM_SNAPSHOT.json**: The state of the system when this pack was generated.
- **Scripts**: PowerShell scripts to automate parts of the recovery.

## Usage
1. Unzip this pack to a secure location.
2. Open `CHECKLIST.md` and follow the instructions.
";

        private string GetChecklistContent() => 
@"# Restoration Checklist

- [ ] **1. Infrastructure Prep**
  - [ ] Install Windows Server / Windows 10/11
  - [ ] Set Time Zone & Sync Clock
  - [ ] Disable Sleep/Hibernate (Power Settings)
  - [ ] Install prerequisites (See KNOWN_DEPENDENCIES.md)

- [ ] **2. Database Restore**
  - [ ] Install SQL Server Express
  - [ ] Run `SQL_BACKUP_RESTORE.ps1`
  - [ ] Verify connectivity

- [ ] **3. Web App Hosting**
  - [ ] Install IIS & Hosting Bundle
  - [ ] Run `IIS_RESTORE.ps1`
  - [ ] Copy application files to physical path

- [ ] **4. Public Access**
  - [ ] Run `CLOUDFLARED_RESTORE.ps1`
  - [ ] Verify tunnel status

- [ ] **5. Final Verification**
  - [ ] Run `VERIFY_ONLINE.ps1`
";

        private string GetKnownDependenciesContent() =>
@"# Known Dependencies

1. **.NET Hosting Bundle**
   - Version: 8.0 (or matching app version)
   - Required for IIS hosting.

2. **Microsoft SQL Server Express**
   - Version: 2019 or later.
   - Authentication: Mixed Mode (SQL Auth + Windows Auth).

3. **Cloudflared (Cloudflare Tunnel)**
   - Required for public access without opening firewall ports.

4. **IIS (Internet Information Services)**
   - Enabled features: Web Server, WebSocket Protocol, ASP.NET 4.8 (if needed), Application Initialization.
";

        private string GetTroubleshootingContent() =>
@"# Troubleshooting

## Mixed Content / SSL Issues
- Ensure `X-Forwarded-Proto` header is being passed by Cloudflared.
- Ensure app `appsettings.json` has `ForwardedHeaders` config enabled.

## 502 Bad Gateway
- Check if the Web App is running in IIS.
- Check if the App Pool is started.
- Check `cloudflared` logs (`C:\ProgramData\cloudflared`).

## SQL Connection Errors
- Verify SQL Server Browser service is running if using Named Instance.
- Verify TCP/IP is enabled in SQL Server Configuration Manager.
";

        private string GetIisRestoreScript(HostingInfo info) =>
$@"# IIS Restore Script
# Generated for: {info.IisSiteName}
# Path: {info.PhysicalPath}

Write-Host ""Restoring IIS Configuration..."" -ForegroundColor Cyan

# Ensure IIS is installed
# Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole -All

# Create App Pool
$poolName = ""{info.AppPoolName}""
if (!(Get-WebAppPoolState -Name $poolName -ErrorAction SilentlyContinue)) {{
    Write-Host ""Creating App Pool: $poolName""
    New-WebAppPool -Name $poolName
    Set-ItemProperty ""IIS:\AppPools\$poolName"" -Name ""managedRuntimeVersion"" -Value """"
    Set-ItemProperty ""IIS:\AppPools\$poolName"" -Name ""startMode"" -Value ""AlwaysRunning""
}}

# Create Site
$siteName = ""{info.IisSiteName}""
$path = ""{info.PhysicalPath}""
if (!(Get-Website -Name $siteName -ErrorAction SilentlyContinue)) {{
    Write-Host ""Creating Site: $siteName""
    New-Website -Name $siteName -PhysicalPath $path -Port 80 -ApplicationPool $poolName
}}

Write-Host ""IIS Restore Complete."" -ForegroundColor Green
";

        private string GetSqlRestoreScript(DatabaseRecoveryInfo info) =>
$@"# SQL Restore Script
# Database: {info.DatabaseName}

Write-Host ""SQL Server Restore Guidance"" -ForegroundColor Cyan
Write-Host ""---------------------------""
Write-Host ""Target Instance: {info.InstanceName}""
Write-Host ""Target Database: {info.DatabaseName}""

Write-Host ""To restore, run the following SQL command in SSMS or sqlcmd:""
Write-Host ""RESTORE DATABASE [{info.DatabaseName}] FROM DISK = 'PATH_TO_BACKUP.bak' WITH REPLACE, RECOVERY"" -ForegroundColor Yellow

# TODO: Add automated restore logic if backup path is known and accessible.
";

        private string GetCloudflaredRestoreScript() =>
@"# Cloudflared Restore Script

Write-Host ""Restoring Cloudflare Tunnel..."" -ForegroundColor Cyan

# 1. Check for Config
if (!(Test-Path ""C:\ProgramData\cloudflared\config.yml"")) {
    Write-Warning ""Config file missing at C:\ProgramData\cloudflared\config.yml""
    Write-Host ""Please place your 'config.yml' and certificate file in the directory.""
}

# 2. Install Service
# cloudflared.exe service install

# 3. Start Service
# Start-Service cloudflared

Write-Host ""Check status with: Get-Service cloudflared""
";

        private string GetVerifyOnlineScript() =>
@"# Verify Online Script

$url = ""http://localhost""
try {
    $response = Invoke-WebRequest -Uri $url -Method Head -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host ""Local Application is ONLINE"" -ForegroundColor Green
    }
} catch {
    Write-Host ""Local Application Check FAILED"" -ForegroundColor Red
}
";

    }
}
