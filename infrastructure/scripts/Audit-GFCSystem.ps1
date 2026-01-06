# GFC System Audit Script
# Shows what's already installed and configured on this host

Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  GFC System Configuration Audit" -ForegroundColor Cyan
Write-Host "  Computer: $env:COMPUTERNAME" -ForegroundColor Cyan
Write-Host "  Date: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

$report = @{
    IIS = @{}
    DotNet = @{}
    CloudflareTunnel = @{}
    GFCApp = @{}
    Network = @{}
    Database = @{}
}

# ============================================================
# 1. IIS Configuration
# ============================================================
Write-Host "[1/7] Checking IIS..." -ForegroundColor Yellow

try {
    $iisService = Get-Service W3SVC -ErrorAction Stop
    $report.IIS.ServiceInstalled = $true
    $report.IIS.ServiceStatus = $iisService.Status
    $report.IIS.ServiceStartType = $iisService.StartType
    
    Write-Host "  IIS Service: $($iisService.Status) (StartType: $($iisService.StartType))" -ForegroundColor $(if ($iisService.Status -eq 'Running') { 'Green' } else { 'Yellow' })
    
    # Check IIS features
    $iisFeatures = Get-WindowsOptionalFeature -Online | Where-Object {$_.FeatureName -like "IIS-*" -and $_.State -eq 'Enabled'}
    $report.IIS.EnabledFeatures = $iisFeatures.Count
    Write-Host "  Enabled IIS Features: $($iisFeatures.Count)" -ForegroundColor Green
    
    # Check IIS sites
    Import-Module WebAdministration -ErrorAction SilentlyContinue
    $sites = Get-Website -ErrorAction SilentlyContinue
    
    if ($sites) {
        Write-Host "  IIS Sites:" -ForegroundColor White
        foreach ($site in $sites) {
            $bindings = $site.bindings.Collection | ForEach-Object { $_.bindingInformation }
            Write-Host "    - $($site.Name): $($site.State) | Bindings: $($bindings -join ', ')" -ForegroundColor $(if ($site.State -eq 'Started') { 'Green' } else { 'Yellow' })
            $report.IIS.Sites += @{
                Name = $site.Name
                State = $site.State
                PhysicalPath = $site.PhysicalPath
                Bindings = $bindings
            }
        }
    } else {
        Write-Host "  No IIS sites configured" -ForegroundColor Yellow
        $report.IIS.Sites = @()
    }
    
    # Check application pools
    $appPools = Get-ChildItem IIS:\AppPools -ErrorAction SilentlyContinue
    if ($appPools) {
        Write-Host "  Application Pools: $($appPools.Count)" -ForegroundColor White
        $report.IIS.AppPools = $appPools.Count
    }
    
} catch {
    Write-Host "  IIS NOT installed" -ForegroundColor Red
    $report.IIS.ServiceInstalled = $false
}

Write-Host ""

# ============================================================
# 2. .NET Runtime
# ============================================================
Write-Host "[2/7] Checking .NET Runtime..." -ForegroundColor Yellow

try {
    $dotnetVersion = dotnet --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  .NET SDK: $dotnetVersion" -ForegroundColor Green
        $report.DotNet.SDK = $dotnetVersion
    }
    
    # Check for ASP.NET Core Hosting Bundle
    $hostingBundlePath = "C:\Program Files\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll"
    if (Test-Path $hostingBundlePath) {
        $hostingBundleVersion = (Get-Item $hostingBundlePath).VersionInfo.FileVersion
        Write-Host "  ASP.NET Core Hosting Bundle: Installed (v$hostingBundleVersion)" -ForegroundColor Green
        $report.DotNet.HostingBundle = $hostingBundleVersion
    } else {
        Write-Host "  ASP.NET Core Hosting Bundle: NOT installed" -ForegroundColor Yellow
        $report.DotNet.HostingBundle = $null
    }
    
    # Check installed runtimes
    $runtimes = dotnet --list-runtimes 2>&1 | Out-String
    $aspNetCoreRuntimes = $runtimes -split "`n" | Where-Object { $_ -match "Microsoft.AspNetCore.App" }
    if ($aspNetCoreRuntimes) {
        Write-Host "  ASP.NET Core Runtimes:" -ForegroundColor White
        foreach ($runtime in $aspNetCoreRuntimes) {
            Write-Host "    $runtime" -ForegroundColor Gray
        }
    }
    
} catch {
    Write-Host "  .NET SDK: NOT installed" -ForegroundColor Red
    $report.DotNet.SDK = $null
}

Write-Host ""

# ============================================================
# 3. Cloudflare Tunnel
# ============================================================
Write-Host "[3/7] Checking Cloudflare Tunnel..." -ForegroundColor Yellow

$cloudflaredPaths = @(
    "C:\Program Files\cloudflared\cloudflared.exe",
    "C:\cloudflared\cloudflared.exe"
)

$cloudflaredPath = $null
foreach ($path in $cloudflaredPaths) {
    if (Test-Path $path) {
        $cloudflaredPath = $path
        break
    }
}

if ($cloudflaredPath) {
    Write-Host "  cloudflared.exe: Found at $cloudflaredPath" -ForegroundColor Green
    $report.CloudflareTunnel.Installed = $true
    $report.CloudflareTunnel.Path = $cloudflaredPath
    
    # Check version
    $env:Path += ";$(Split-Path $cloudflaredPath)"
    $version = & cloudflared --version 2>&1 | Out-String
    if ($version -match "cloudflared version ([\d\.]+)") {
        Write-Host "  Version: $($matches[1])" -ForegroundColor Green
        $report.CloudflareTunnel.Version = $matches[1]
    }
    
    # Check service
    try {
        $cfService = Get-Service cloudflared -ErrorAction Stop
        Write-Host "  Service: $($cfService.Status) (StartType: $($cfService.StartType))" -ForegroundColor $(if ($cfService.Status -eq 'Running') { 'Green' } else { 'Yellow' })
        $report.CloudflareTunnel.ServiceStatus = $cfService.Status
        $report.CloudflareTunnel.ServiceStartType = $cfService.StartType
    } catch {
        Write-Host "  Service: NOT installed" -ForegroundColor Yellow
        $report.CloudflareTunnel.ServiceStatus = "Not Installed"
    }
    
    # Check config
    $configPath = "$env:USERPROFILE\.cloudflared\config.yml"
    if (Test-Path $configPath) {
        Write-Host "  Config: Found at $configPath" -ForegroundColor Green
        $config = Get-Content $configPath -Raw
        $report.CloudflareTunnel.ConfigExists = $true
        
        # Parse config
        if ($config -match "hostname:\s*(.+)") {
            Write-Host "    Hostname: $($matches[1].Trim())" -ForegroundColor White
        }
        if ($config -match "service:\s*http://localhost:(\d+)") {
            Write-Host "    Target: http://localhost:$($matches[1])" -ForegroundColor White
        }
        if ($config -match "tunnel:\s*([a-f0-9-]+)") {
            Write-Host "    Tunnel ID: $($matches[1])" -ForegroundColor White
            $report.CloudflareTunnel.TunnelId = $matches[1]
        }
    } else {
        Write-Host "  Config: NOT found" -ForegroundColor Red
        $report.CloudflareTunnel.ConfigExists = $false
    }
    
    # List tunnels
    try {
        $tunnels = & cloudflared tunnel list 2>&1 | Out-String
        if ($tunnels -match "gfc-webapp") {
            Write-Host "  Tunnel 'gfc-webapp': Configured" -ForegroundColor Green
        }
    } catch {
        Write-Host "  Could not list tunnels" -ForegroundColor Yellow
    }
    
} else {
    Write-Host "  cloudflared.exe: NOT installed" -ForegroundColor Red
    $report.CloudflareTunnel.Installed = $false
}

Write-Host ""

# ============================================================
# 4. GFC Application
# ============================================================
Write-Host "[4/7] Checking GFC Application..." -ForegroundColor Yellow

$appPaths = @(
    "C:\inetpub\wwwroot\GFC",
    "C:\GFC",
    "C:\Apps\GFC"
)

$gfcAppPath = $null
foreach ($path in $appPaths) {
    if (Test-Path $path) {
        $gfcAppPath = $path
        break
    }
}

if ($gfcAppPath) {
    Write-Host "  App Path: $gfcAppPath" -ForegroundColor Green
    $report.GFCApp.Path = $gfcAppPath
    
    # Check for key files
    $dllPath = Join-Path $gfcAppPath "GFC.BlazorServer.dll"
    if (Test-Path $dllPath) {
        $dllInfo = Get-Item $dllPath
        Write-Host "  Main DLL: Found (Modified: $($dllInfo.LastWriteTime))" -ForegroundColor Green
        $report.GFCApp.LastDeployed = $dllInfo.LastWriteTime
    }
    
    $appSettingsPath = Join-Path $gfcAppPath "appsettings.json"
    if (Test-Path $appSettingsPath) {
        Write-Host "  appsettings.json: Found" -ForegroundColor Green
        $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
        
        # Check connection string
        if ($appSettings.ConnectionStrings.GFC) {
            $connStr = $appSettings.ConnectionStrings.GFC
            if ($connStr -match "Server=([^;]+)") {
                Write-Host "    Database Server: $($matches[1])" -ForegroundColor White
            }
            if ($connStr -match "Database=([^;]+)") {
                Write-Host "    Database Name: $($matches[1])" -ForegroundColor White
            }
        }
    }
    
} else {
    Write-Host "  GFC App: NOT deployed" -ForegroundColor Red
    $report.GFCApp.Path = $null
}

Write-Host ""

# ============================================================
# 5. Network & Ports
# ============================================================
Write-Host "[5/7] Checking Network Configuration..." -ForegroundColor Yellow

# Check listening ports
$listeningPorts = netstat -ano | Select-String "LISTENING" | Out-String

$commonPorts = @{
    "80" = "HTTP"
    "443" = "HTTPS"
    "8080" = "IIS/App"
    "5000" = "Kestrel"
    "5001" = "Kestrel HTTPS"
    "1433" = "SQL Server"
}

foreach ($port in $commonPorts.Keys) {
    $portPattern = ":$port\s"
    if ($listeningPorts -match $portPattern) {
        Write-Host "  Port $port ($($commonPorts[$port])): LISTENING" -ForegroundColor Green
        $report.Network["Port$port"] = "Listening"
    } else {
        Write-Host "  Port $port ($($commonPorts[$port])): Not listening" -ForegroundColor Gray
        $report.Network["Port$port"] = "Not listening"
    }
}

# Check DNS
try {
    $dns = Resolve-DnsName -Name "gfc.lovanow.com" -ErrorAction Stop
    $aRecords = $dns | Where-Object { $_.Type -eq 'A' } | Select-Object -ExpandProperty IPAddress
    if ($aRecords) {
        Write-Host "  DNS (gfc.lovanow.com): $($aRecords -join ', ')" -ForegroundColor Green
        $report.Network.DNS = $aRecords -join ', '
    }
} catch {
    Write-Host "  DNS (gfc.lovanow.com): Cannot resolve" -ForegroundColor Yellow
    $report.Network.DNS = "Not resolved"
}

Write-Host ""

# ============================================================
# 6. SQL Server
# ============================================================
Write-Host "[6/7] Checking SQL Server..." -ForegroundColor Yellow

try {
    $sqlService = Get-Service | Where-Object { $_.Name -like "MSSQL*" -or $_.Name -eq "SQLWriter" } | Select-Object -First 1
    
    if ($sqlService) {
        Write-Host "  SQL Server Service: $($sqlService.DisplayName) - $($sqlService.Status)" -ForegroundColor $(if ($sqlService.Status -eq 'Running') { 'Green' } else { 'Yellow' })
        $report.Database.ServiceStatus = $sqlService.Status
        
        # Try to connect
        try {
            $connStr = "Server=.;Database=master;Integrated Security=True;TrustServerCertificate=True"
            $conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
            $conn.Open()
            
            $cmd = $conn.CreateCommand()
            $cmd.CommandText = "SELECT @@VERSION"
            $version = $cmd.ExecuteScalar()
            
            if ($version -match "Microsoft SQL Server (\d+)") {
                Write-Host "  SQL Server Version: SQL Server $($matches[1])" -ForegroundColor Green
            }
            
            # Check for GFC database
            $cmd.CommandText = "SELECT name FROM sys.databases WHERE name = 'ClubMembership'"
            $dbExists = $cmd.ExecuteScalar()
            
            if ($dbExists) {
                Write-Host "  ClubMembership Database: EXISTS" -ForegroundColor Green
                $report.Database.GFCDatabaseExists = $true
            } else {
                Write-Host "  ClubMembership Database: NOT FOUND" -ForegroundColor Yellow
                $report.Database.GFCDatabaseExists = $false
            }
            
            $conn.Close()
            
        } catch {
            Write-Host "  Could not connect to SQL Server: $($_.Exception.Message)" -ForegroundColor Yellow
        }
        
    } else {
        Write-Host "  SQL Server: NOT installed" -ForegroundColor Red
        $report.Database.ServiceStatus = "Not Installed"
    }
} catch {
    Write-Host "  SQL Server: NOT found" -ForegroundColor Red
    $report.Database.ServiceStatus = "Not Found"
}

Write-Host ""

# ============================================================
# 7. Quick Health Check
# ============================================================
Write-Host "[7/7] Testing Endpoints..." -ForegroundColor Yellow

# Test localhost
$localPorts = @(80, 8080, 5000)
foreach ($port in $localPorts) {
    try {
        $response = Invoke-WebRequest "http://localhost:$port" -UseBasicParsing -TimeoutSec 3 -ErrorAction Stop
        Write-Host "  http://localhost:$port - HTTP $($response.StatusCode)" -ForegroundColor Green
    } catch {
        # Silent - port not responding
    }
}

# Test HTTPS
try {
    [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
    $response = Invoke-WebRequest "https://gfc.lovanow.com" -UseBasicParsing -TimeoutSec 10 -ErrorAction Stop
    Write-Host "  https://gfc.lovanow.com - HTTP $($response.StatusCode)" -ForegroundColor Green
    $report.Network.HTTPSWorking = $true
} catch {
    Write-Host "  https://gfc.lovanow.com - NOT responding" -ForegroundColor Red
    $report.Network.HTTPSWorking = $false
}

# ============================================================
# Summary
# ============================================================
Write-Host ""
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Summary" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

$ready = $true
$issues = @()

if (-not $report.IIS.ServiceInstalled) {
    $issues += "IIS is not installed"
    $ready = $false
}

if (-not $report.CloudflareTunnel.Installed) {
    $issues += "Cloudflare Tunnel is not installed"
    $ready = $false
}

if (-not $report.CloudflareTunnel.ConfigExists) {
    $issues += "Cloudflare Tunnel config is missing"
    $ready = $false
}

if (-not $report.GFCApp.Path) {
    $issues += "GFC Application is not deployed"
    $ready = $false
}

if (-not $report.Database.GFCDatabaseExists) {
    $issues += "ClubMembership database not found"
    $ready = $false
}

if ($ready) {
    Write-Host "System Status: READY" -ForegroundColor Green
    Write-Host ""
    Write-Host "All components are installed and configured!" -ForegroundColor Green
    Write-Host "You may just need to start services or fix configuration." -ForegroundColor Yellow
} else {
    Write-Host "System Status: INCOMPLETE" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Missing components:" -ForegroundColor Yellow
    foreach ($issue in $issues) {
        Write-Host "  - $issue" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "===========================================================" -ForegroundColor Cyan
