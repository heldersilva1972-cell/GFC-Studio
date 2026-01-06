# Complete GFC Deployment Script
# Installs IIS, deploys app, configures Cloudflare Tunnel, and sets up HTTPS
# Run as Administrator

param(
    [string]$Hostname = "gfc.lovanow.com",
    [int]$Port = 8080,
    [string]$AppPath = "C:\inetpub\wwwroot\GFC",
    [switch]$SkipIIS,
    [switch]$SkipDeploy,
    [switch]$SkipTunnel
)

$ErrorActionPreference = "Stop"

Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  GFC Complete Deployment Script" -ForegroundColor Cyan
Write-Host "  Hostname: $Hostname" -ForegroundColor Cyan
Write-Host "  Port: $Port" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

# Check if running as Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "ERROR: This script must be run as Administrator!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Right-click PowerShell and select 'Run as Administrator', then run this script again." -ForegroundColor Yellow
    exit 1
}

# Step 1: Install IIS
if (-not $SkipIIS) {
    Write-Host "[1/5] Installing IIS..." -ForegroundColor Yellow
    
    # Check if IIS is already installed
    $iisInstalled = (Get-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole).State -eq 'Enabled'
    
    if ($iisInstalled) {
        Write-Host "  IIS is already installed" -ForegroundColor Green
    } else {
        Write-Host "  Installing IIS features (this may take 5-10 minutes)..." -ForegroundColor Yellow
        
        try {
            # Install IIS using DISM (faster than Enable-WindowsOptionalFeature)
            $dismResult = dism /online /enable-feature /featurename:IIS-WebServerRole /featurename:IIS-WebServer /featurename:IIS-CommonHttpFeatures /featurename:IIS-ASPNET45 /featurename:IIS-NetFxExtensibility45 /featurename:IIS-ManagementConsole /featurename:IIS-HttpErrors /featurename:IIS-HttpRedirect /featurename:IIS-ApplicationDevelopment /featurename:IIS-Security /featurename:IIS-RequestFiltering /featurename:IIS-HealthAndDiagnostics /featurename:IIS-HttpLogging /featurename:IIS-Performance /featurename:IIS-HttpCompressionStatic /featurename:IIS-StaticContent /featurename:IIS-DefaultDocument /featurename:IIS-DirectoryBrowsing /featurename:IIS-ISAPIExtensions /featurename:IIS-ISAPIFilter /all /norestart
            
            if ($LASTEXITCODE -eq 0 -or $LASTEXITCODE -eq 3010) {
                Write-Host "  IIS installed successfully" -ForegroundColor Green
                
                # Start IIS service
                Start-Service W3SVC -ErrorAction SilentlyContinue
                Set-Service W3SVC -StartupType Automatic
                
                Write-Host "  IIS service started" -ForegroundColor Green
            } else {
                Write-Host "  WARNING: IIS installation returned code $LASTEXITCODE" -ForegroundColor Yellow
            }
        } catch {
            Write-Host "  ERROR installing IIS: $($_.Exception.Message)" -ForegroundColor Red
            Write-Host "  You may need to install IIS manually via Server Manager or Windows Features" -ForegroundColor Yellow
        }
    }
    
    # Check for ASP.NET Core Hosting Bundle
    Write-Host "  Checking for ASP.NET Core Hosting Bundle..." -ForegroundColor Yellow
    $hostingBundleInstalled = Test-Path "C:\Program Files\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll"
    
    if (-not $hostingBundleInstalled) {
        Write-Host "  WARNING: ASP.NET Core Hosting Bundle not detected" -ForegroundColor Yellow
        Write-Host "  You need to install it manually:" -ForegroundColor Yellow
        Write-Host "    1. Download from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor White
        Write-Host "    2. Look for 'Hosting Bundle' under ASP.NET Core Runtime" -ForegroundColor White
        Write-Host "    3. Install and run: iisreset" -ForegroundColor White
        Write-Host ""
        Write-Host "  Press Enter to continue (or Ctrl+C to abort and install it first)..." -ForegroundColor Yellow
        Read-Host
    } else {
        Write-Host "  ASP.NET Core Hosting Bundle is installed" -ForegroundColor Green
    }
} else {
    Write-Host "[1/5] Skipping IIS installation (--SkipIIS specified)" -ForegroundColor Gray
}

Write-Host ""

# Step 2: Deploy Application
if (-not $SkipDeploy) {
    Write-Host "[2/5] Deploying GFC Application..." -ForegroundColor Yellow
    
    $projectPath = Join-Path $PSScriptRoot "..\..\apps\webapp\GFC.BlazorServer"
    
    if (-not (Test-Path $projectPath)) {
        Write-Host "  ERROR: Project not found at: $projectPath" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "  Project path: $projectPath" -ForegroundColor White
    Write-Host "  Publish path: $AppPath" -ForegroundColor White
    Write-Host ""
    
    # Create publish directory
    if (-not (Test-Path $AppPath)) {
        New-Item -ItemType Directory -Path $AppPath -Force | Out-Null
        Write-Host "  Created directory: $AppPath" -ForegroundColor Green
    }
    
    # Publish the application
    Write-Host "  Publishing application (this may take 2-3 minutes)..." -ForegroundColor Yellow
    
    Push-Location $projectPath
    try {
        $publishOutput = dotnet publish -c Release -o $AppPath 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  Application published successfully" -ForegroundColor Green
        } else {
            Write-Host "  ERROR: Publish failed" -ForegroundColor Red
            Write-Host $publishOutput -ForegroundColor Red
            exit 1
        }
    } finally {
        Pop-Location
    }
    
    # Create/Update IIS Site
    Write-Host "  Configuring IIS site..." -ForegroundColor Yellow
    
    Import-Module WebAdministration -ErrorAction SilentlyContinue
    
    # Check if site exists
    $siteExists = Get-Website -Name "GFC" -ErrorAction SilentlyContinue
    
    if ($siteExists) {
        Write-Host "  Updating existing site 'GFC'" -ForegroundColor Yellow
        Stop-Website -Name "GFC" -ErrorAction SilentlyContinue
        Remove-Website -Name "GFC" -ErrorAction SilentlyContinue
    }
    
    # Create new site
    New-Website -Name "GFC" -Port $Port -PhysicalPath $AppPath -ApplicationPool "DefaultAppPool" -Force | Out-Null
    
    # Configure application pool for .NET Core
    Set-ItemProperty "IIS:\AppPools\DefaultAppPool" -Name "managedRuntimeVersion" -Value ""
    
    # Start the site
    Start-Website -Name "GFC"
    
    Write-Host "  IIS site 'GFC' created and started on port $Port" -ForegroundColor Green
    
    # Test local endpoint
    Write-Host "  Testing local endpoint..." -ForegroundColor Yellow
    Start-Sleep -Seconds 3
    
    try {
        $testResponse = Invoke-WebRequest "http://localhost:$Port" -UseBasicParsing -TimeoutSec 10
        Write-Host "  Local site responds: HTTP $($testResponse.StatusCode)" -ForegroundColor Green
    } catch {
        Write-Host "  WARNING: Local site not responding yet" -ForegroundColor Yellow
        Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host "  This may be normal - the app might need more time to start" -ForegroundColor Yellow
    }
} else {
    Write-Host "[2/5] Skipping application deployment (--SkipDeploy specified)" -ForegroundColor Gray
}

Write-Host ""

# Step 3: Configure Cloudflare Tunnel
if (-not $SkipTunnel) {
    Write-Host "[3/5] Configuring Cloudflare Tunnel..." -ForegroundColor Yellow
    
    # Check if cloudflared is installed
    $cloudflaredPath = "C:\Program Files\cloudflared\cloudflared.exe"
    
    if (-not (Test-Path $cloudflaredPath)) {
        Write-Host "  ERROR: cloudflared.exe not found at: $cloudflaredPath" -ForegroundColor Red
        Write-Host "  Please install Cloudflare Tunnel first" -ForegroundColor Yellow
        exit 1
    }
    
    # Create config directory
    $configDir = "$env:USERPROFILE\.cloudflared"
    if (-not (Test-Path $configDir)) {
        New-Item -ItemType Directory -Path $configDir -Force | Out-Null
        Write-Host "  Created config directory: $configDir" -ForegroundColor Green
    }
    
    # Check if tunnel already exists
    Write-Host "  Checking for existing tunnel..." -ForegroundColor Yellow
    
    $env:Path += ";C:\Program Files\cloudflared"
    $tunnelList = & cloudflared tunnel list 2>&1 | Out-String
    
    $tunnelId = $null
    if ($tunnelList -match "gfc-webapp\s+([a-f0-9-]+)") {
        $tunnelId = $matches[1]
        Write-Host "  Found existing tunnel: gfc-webapp ($tunnelId)" -ForegroundColor Green
    } else {
        Write-Host "  No existing tunnel found - you need to create one" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "  Run this command to create a tunnel:" -ForegroundColor Cyan
        Write-Host "    cloudflared tunnel create gfc-webapp" -ForegroundColor White
        Write-Host ""
        Write-Host "  Then run this script again" -ForegroundColor Yellow
        exit 1
    }
    
    # Create config.yml
    $configPath = Join-Path $configDir "config.yml"
    $credentialsPath = Join-Path $configDir "$tunnelId.json"
    
    if (-not (Test-Path $credentialsPath)) {
        Write-Host "  ERROR: Credentials file not found: $credentialsPath" -ForegroundColor Red
        Write-Host "  Run: cloudflared tunnel create gfc-webapp" -ForegroundColor Yellow
        exit 1
    }
    
    Write-Host "  Creating config.yml..." -ForegroundColor Yellow
    
    $configContent = @"
tunnel: $tunnelId
credentials-file: $credentialsPath

ingress:
  - hostname: $Hostname
    service: http://localhost:$Port
    originRequest:
      noTLSVerify: true
  - service: http_status:404
"@
    
    $configContent | Out-File -FilePath $configPath -Encoding UTF8 -Force
    Write-Host "  Config file created: $configPath" -ForegroundColor Green
    
    # Restart cloudflared service
    Write-Host "  Restarting cloudflared service..." -ForegroundColor Yellow
    
    try {
        Restart-Service cloudflared -ErrorAction Stop
        Write-Host "  Service restarted successfully" -ForegroundColor Green
    } catch {
        Write-Host "  WARNING: Could not restart service: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host "  Try manually: Restart-Service cloudflared" -ForegroundColor Yellow
    }
    
    Write-Host "  Waiting for tunnel to stabilize (30 seconds)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 30
    
} else {
    Write-Host "[3/5] Skipping Cloudflare Tunnel configuration (--SkipTunnel specified)" -ForegroundColor Gray
}

Write-Host ""

# Step 4: Configure DNS (informational)
Write-Host "[4/5] DNS Configuration Check..." -ForegroundColor Yellow

try {
    $dns = Resolve-DnsName -Name $Hostname -ErrorAction Stop
    $aRecords = $dns | Where-Object { $_.Type -eq 'A' } | Select-Object -ExpandProperty IPAddress
    
    if ($aRecords) {
        Write-Host "  DNS resolves to: $($aRecords -join ', ')" -ForegroundColor Green
        
        # Check if it's Cloudflare
        if ($aRecords[0] -match "^(104\.21\.|172\.67\.|10\.20\.)") {
            Write-Host "  Appears to be Cloudflare/VPN IP - Good!" -ForegroundColor Green
        } else {
            Write-Host "  WARNING: IP doesn't look like Cloudflare" -ForegroundColor Yellow
            Write-Host "  Expected: 104.21.x.x or 172.67.x.x" -ForegroundColor Yellow
        }
    } else {
        Write-Host "  WARNING: No A records found" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  WARNING: Could not resolve DNS: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "  Make sure DNS is configured in Cloudflare dashboard" -ForegroundColor Yellow
}

Write-Host ""

# Step 5: Verify HTTPS
Write-Host "[5/5] Testing HTTPS Connection..." -ForegroundColor Yellow

Write-Host "  Waiting 10 seconds for everything to stabilize..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

try {
    # Ignore certificate validation for this test
    [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
    
    $response = Invoke-WebRequest -Uri "https://$Hostname" -UseBasicParsing -TimeoutSec 30
    
    if ($response.StatusCode -eq 200) {
        Write-Host "  SUCCESS: HTTPS connection works! (Status: $($response.StatusCode))" -ForegroundColor Green
    } else {
        Write-Host "  WARNING: Got response but status is $($response.StatusCode)" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  ERROR: HTTPS connection failed" -ForegroundColor Red
    Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "  This might be normal if:" -ForegroundColor Yellow
    Write-Host "    - DNS hasn't propagated yet (wait 5-15 minutes)" -ForegroundColor White
    Write-Host "    - Tunnel is still connecting (check: Get-Service cloudflared)" -ForegroundColor White
    Write-Host "    - Cloudflare SSL mode is wrong (should be 'Full')" -ForegroundColor White
}

# Final Summary
Write-Host ""
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Deployment Complete!" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "  1. Open browser to: https://$Hostname" -ForegroundColor White
Write-Host "  2. Verify lock icon appears (Secure)" -ForegroundColor White
Write-Host "  3. Log in and test functionality" -ForegroundColor White
Write-Host ""

Write-Host "If site doesn't load:" -ForegroundColor Cyan
Write-Host "  1. Check IIS: Get-Service W3SVC" -ForegroundColor White
Write-Host "  2. Check tunnel: Get-Service cloudflared" -ForegroundColor White
Write-Host "  3. Check local: http://localhost:$Port" -ForegroundColor White
Write-Host "  4. Wait 5-15 minutes for DNS propagation" -ForegroundColor White
Write-Host ""

Write-Host "Verification script:" -ForegroundColor Cyan
Write-Host "  .\Verify-HttpsConfiguration.ps1" -ForegroundColor White
Write-Host ""

Write-Host "===========================================================" -ForegroundColor Cyan
