<#
.SYNOPSIS
    GFC Private Network One-Click Onboarding for Windows.
    This script automates WireGuard installation, Root CA trust, and VPN configuration.
#>

Param(
    [Parameter(Mandatory=$true)]
    [string]$Token,
    
    [Parameter(Mandatory=$false)]
    [string]$ApiUrl = "https://gfc.lovanow.com"
)

$ErrorActionPreference = "Stop"

# 1. Check for Administrative Privileges
$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "Requesting administrative privileges..." -ForegroundColor Cyan
    Start-Process powershell.exe -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`" -Token `"$Token`" -ApiUrl `"$ApiUrl`"" -Verb RunAs
    exit
}

Write-Host "==========================================" -ForegroundColor Green
Write-Host "   GFC Private Network Windows Setup      " -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
Write-Host ""

# 2. Check/Install WireGuard
$wgPath = Join-Path $env:ProgramFiles "WireGuard\wireguard.exe"
if (-not (Test-Path $wgPath)) {
    Write-Host "[1/4] WireGuard not found. Installing..." -ForegroundColor Cyan
    $wgUrl = "https://download.wireguard.com/windows-client/wireguard-installer.exe"
    $wgInstaller = Join-Path $env:TEMP "wireguard-setup.exe"
    
    Write-Host "      Downloading installer..."
    Invoke-WebRequest -Uri $wgUrl -OutFile $wgInstaller
    
    Write-Host "      Running installer..."
    Start-Process -FilePath $wgInstaller -ArgumentList "/install", "/quiet" -Wait
    
    if (-not (Test-Path $wgPath)) {
        Write-Host "ERROR: WireGuard installation failed." -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "[1/4] WireGuard is already installed." -ForegroundColor Green
}

# 3. Download and Install Root CA
Write-Host "[2/4] Configuring Security Trust (GFC Root CA)..." -ForegroundColor Cyan
$caUrl = "$ApiUrl/api/onboarding/ca-cert?token=$Token"
$caPath = Join-Path $env:TEMP "GFC_Root_CA.cer"

try {
    Invoke-WebRequest -Uri $caUrl -OutFile $caPath
    Import-Certificate -FilePath $caPath -CertStoreLocation Cert:\LocalMachine\Root
    Write-Host "      GFC Root CA installed to Trusted Root store." -ForegroundColor Green
} catch {
    Write-Host "WARNING: Could not install Root CA. You may see browser warnings." -ForegroundColor Yellow
}

# 4. Download and Import VPN Configuration
Write-Host "[3/4] Downloading VPN Configuration..." -ForegroundColor Cyan
$configUrl = "$ApiUrl/api/onboarding/config?token=$Token"
$configPath = Join-Path $env:TEMP "gfc-access.conf"

try {
    Invoke-WebRequest -Uri $configUrl -OutFile $configPath
    
    Write-Host "      Importing tunnel to WireGuard..."
    & $wgPath /installtunnelservice $configPath
    
    Write-Host "      VPN Tunnel 'gfc-access' installed." -ForegroundColor Green
} catch {
    Write-Host "ERROR: Failed to download or import configuration." -ForegroundColor Red
    exit 1
}

# 5. Connection Test
Write-Host "[4/4] Testing connection..." -ForegroundColor Cyan
Write-Host "      Waiting for tunnel to initialize..."
Start-Sleep -Seconds 5

$testUrl = "$ApiUrl/api/health/vpn-check"
try {
    $response = Invoke-WebRequest -Uri $testUrl -UseBasicParsing -TimeoutSec 10
    if ($response.StatusCode -eq 200) {
        Write-Host "SUCCESS: You are securely connected to the GFC Private Network." -ForegroundColor Green
        
        # Notify backend of success
        $completeUrl = "$ApiUrl/api/onboarding/complete?token=$Token"
        $body = @{
            deviceInfo = "$env:COMPUTERNAME - Windows $((Get-ComputerInfo).WindowsProductName)"
            platform = "Windows"
            testPassed = $true
        } | ConvertTo-Json
        Invoke-RestMethod -Uri $completeUrl -Method Post -Body $body -ContentType "application/json"
    }
} catch {
    Write-Host "NOTE: Setup complete, but tunnel verification failed. Please check the WireGuard app." -ForegroundColor Yellow
}

# Cleanup
Remove-Item $caPath -ErrorAction SilentlyContinue
Remove-Item $configPath -ErrorAction SilentlyContinue
if (Test-Path $wgInstaller) { Remove-Item $wgInstaller -ErrorAction SilentlyContinue }

Write-Host ""
Write-Host "Setup finished! You can now access https://gfc.lovanow.com" -ForegroundColor Green
Read-Host "Press Enter to exit..."
