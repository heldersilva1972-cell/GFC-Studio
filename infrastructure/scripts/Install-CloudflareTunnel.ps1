<#
.SYNOPSIS
    Installs and configures Cloudflare Tunnel for GFC Web App

.DESCRIPTION
    This script automates the installation of cloudflared and initial setup
    for enabling trusted HTTPS via Cloudflare Tunnel.

.PARAMETER IISPort
    The local port where IIS is serving the application (default: 8080)

.PARAMETER TunnelName
    Name for the Cloudflare tunnel (default: gfc-webapp)

.PARAMETER Hostname
    The public hostname for the application (default: gfc.lovanow.com)

.EXAMPLE
    .\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com

.NOTES
    Requires Administrator privileges
    Must be run from an elevated PowerShell session
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [int]$IISPort = 8080,
    
    [Parameter(Mandatory=$false)]
    [string]$TunnelName = "gfc-webapp",
    
    [Parameter(Mandatory=$false)]
    [string]$Hostname = "gfc.lovanow.com"
)

# Requires Administrator
#Requires -RunAsAdministrator

$ErrorActionPreference = "Stop"

# Color output functions
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Step {
    param([string]$Message)
    Write-Host "`n═══════════════════════════════════════════════════════" -ForegroundColor Magenta
    Write-Host "  $Message" -ForegroundColor Magenta
    Write-Host "═══════════════════════════════════════════════════════`n" -ForegroundColor Magenta
}

# Main installation function
function Install-Cloudflared {
    Write-Step "Step 1: Installing Cloudflared"
    
    $installPath = "C:\Program Files\cloudflared"
    $executablePath = Join-Path $installPath "cloudflared.exe"
    
    # Check if already installed
    if (Test-Path $executablePath) {
        Write-Warning "Cloudflared already installed at: $executablePath"
        $version = & $executablePath --version 2>&1
        Write-Info "Current version: $version"
        
        $response = Read-Host "Do you want to reinstall/update? (y/N)"
        if ($response -ne 'y' -and $response -ne 'Y') {
            Write-Info "Skipping installation"
            return $executablePath
        }
    }
    
    # Create directory
    Write-Info "Creating installation directory: $installPath"
    New-Item -ItemType Directory -Force -Path $installPath | Out-Null
    
    # Download latest version
    Write-Info "Downloading latest cloudflared for Windows..."
    $downloadUrl = "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe"
    
    try {
        Invoke-WebRequest -Uri $downloadUrl -OutFile $executablePath -UseBasicParsing
        Write-Success "Downloaded cloudflared successfully"
    }
    catch {
        Write-Error "Failed to download cloudflared: $_"
        throw
    }
    
    # Add to PATH
    Write-Info "Adding cloudflared to system PATH..."
    $currentPath = [Environment]::GetEnvironmentVariable("Path", [EnvironmentVariableTarget]::Machine)
    
    if ($currentPath -notlike "*$installPath*") {
        $newPath = "$currentPath;$installPath"
        [Environment]::SetEnvironmentVariable("Path", $newPath, [EnvironmentVariableTarget]::Machine)
        $env:Path = $newPath
        Write-Success "Added to PATH"
    }
    else {
        Write-Info "Already in PATH"
    }
    
    # Verify installation
    $version = & $executablePath --version 2>&1
    Write-Success "Cloudflared installed successfully: $version"
    
    return $executablePath
}

function Initialize-CloudflareTunnel {
    param([string]$CloudflaredPath)
    
    Write-Step "Step 2: Authenticating with Cloudflare"
    
    $certPath = Join-Path $env:USERPROFILE ".cloudflared\cert.pem"
    
    if (Test-Path $certPath) {
        Write-Warning "Certificate already exists at: $certPath"
        $response = Read-Host "Do you want to re-authenticate? (y/N)"
        if ($response -ne 'y' -and $response -ne 'Y') {
            Write-Info "Using existing certificate"
            return $certPath
        }
    }
    
    Write-Info "Opening browser for Cloudflare authentication..."
    Write-Warning "Please select 'lovanow.com' in the browser and click Authorize"
    
    try {
        & $CloudflaredPath tunnel login
        
        if (Test-Path $certPath) {
            Write-Success "Authentication successful! Certificate saved to: $certPath"
            return $certPath
        }
        else {
            throw "Certificate file not found after authentication"
        }
    }
    catch {
        Write-Error "Authentication failed: $_"
        throw
    }
}

function New-CloudflareTunnel {
    param(
        [string]$CloudflaredPath,
        [string]$TunnelName
    )
    
    Write-Step "Step 3: Creating Cloudflare Tunnel"
    
    # Check if tunnel already exists
    Write-Info "Checking for existing tunnels..."
    $tunnelList = & $CloudflaredPath tunnel list 2>&1 | Out-String
    
    if ($tunnelList -match $TunnelName) {
        Write-Warning "Tunnel '$TunnelName' already exists"
        
        # Extract tunnel ID from list
        $tunnelId = ($tunnelList -split "`n" | Where-Object { $_ -match $TunnelName } | Select-Object -First 1) -replace '.*?([a-f0-9-]{36}).*', '$1'
        
        if ($tunnelId) {
            Write-Info "Found existing tunnel ID: $tunnelId"
            return $tunnelId
        }
    }
    
    # Create new tunnel
    Write-Info "Creating new tunnel: $TunnelName"
    
    try {
        $output = & $CloudflaredPath tunnel create $TunnelName 2>&1 | Out-String
        Write-Success "Tunnel created successfully"
        
        # Extract tunnel ID
        if ($output -match "([a-f0-9-]{36})") {
            $tunnelId = $matches[1]
            Write-Success "Tunnel ID: $tunnelId"
            
            # Verify credentials file
            $credentialsPath = Join-Path $env:USERPROFILE ".cloudflared\$tunnelId.json"
            if (Test-Path $credentialsPath) {
                Write-Success "Credentials saved to: $credentialsPath"
            }
            else {
                Write-Warning "Credentials file not found at expected location"
            }
            
            return $tunnelId
        }
        else {
            throw "Could not extract tunnel ID from output"
        }
    }
    catch {
        Write-Error "Failed to create tunnel: $_"
        throw
    }
}

function Set-TunnelDNS {
    param(
        [string]$CloudflaredPath,
        [string]$TunnelName,
        [string]$Hostname
    )
    
    Write-Step "Step 4: Configuring DNS Routing"
    
    Write-Info "Routing $Hostname to tunnel $TunnelName..."
    
    try {
        $output = & $CloudflaredPath tunnel route dns $TunnelName $Hostname 2>&1 | Out-String
        
        if ($output -match "success" -or $output -match "already exists") {
            Write-Success "DNS route configured successfully"
            Write-Info "DNS record created: $Hostname → $TunnelName"
        }
        else {
            Write-Warning "Unexpected output: $output"
        }
    }
    catch {
        Write-Error "Failed to configure DNS: $_"
        throw
    }
}

function New-TunnelConfig {
    param(
        [string]$TunnelId,
        [string]$Hostname,
        [int]$IISPort
    )
    
    Write-Step "Step 5: Creating Tunnel Configuration"
    
    $configDir = Join-Path $env:USERPROFILE ".cloudflared"
    $configPath = Join-Path $configDir "config.yml"
    $credentialsPath = Join-Path $configDir "$TunnelId.json"
    
    # Ensure directory exists
    if (-not (Test-Path $configDir)) {
        New-Item -ItemType Directory -Force -Path $configDir | Out-Null
    }
    
    # Create config content
    $configContent = @"
tunnel: $TunnelId
credentials-file: $credentialsPath

ingress:
  - hostname: $Hostname
    service: http://localhost:$IISPort
    originRequest:
      noTLSVerify: true
  - service: http_status:404
"@
    
    # Write config file
    Write-Info "Writing configuration to: $configPath"
    $configContent | Out-File -FilePath $configPath -Encoding UTF8 -Force
    
    Write-Success "Configuration file created"
    Write-Info "Configuration:"
    Write-Host $configContent -ForegroundColor Gray
    
    return $configPath
}

function Test-TunnelConnection {
    param(
        [string]$CloudflaredPath,
        [string]$TunnelName,
        [string]$Hostname
    )
    
    Write-Step "Step 6: Testing Tunnel Connection"
    
    Write-Info "Starting tunnel in test mode..."
    Write-Warning "Press Ctrl+C to stop the test after verifying connection"
    Write-Info "Test URL in browser: https://$Hostname"
    
    try {
        & $CloudflaredPath tunnel run $TunnelName
    }
    catch {
        Write-Warning "Tunnel stopped (this is normal if you pressed Ctrl+C)"
    }
}

function Install-TunnelService {
    param([string]$CloudflaredPath)
    
    Write-Step "Step 7: Installing Windows Service"
    
    # Check if service already exists
    $service = Get-Service -Name "cloudflared" -ErrorAction SilentlyContinue
    
    if ($service) {
        Write-Warning "Service 'cloudflared' already exists"
        $response = Read-Host "Do you want to reinstall? (y/N)"
        
        if ($response -eq 'y' -or $response -eq 'Y') {
            Write-Info "Uninstalling existing service..."
            & $CloudflaredPath service uninstall
            Start-Sleep -Seconds 2
        }
        else {
            Write-Info "Skipping service installation"
            return
        }
    }
    
    # Install service
    Write-Info "Installing cloudflared as Windows service..."
    
    try {
        & $CloudflaredPath service install
        Write-Success "Service installed successfully"
        
        # Set to automatic startup
        Write-Info "Configuring automatic startup..."
        Set-Service -Name "cloudflared" -StartupType Automatic
        Write-Success "Service set to start automatically"
        
        # Start service
        Write-Info "Starting service..."
        Start-Service -Name "cloudflared"
        Start-Sleep -Seconds 3
        
        # Verify service status
        $serviceStatus = Get-Service -Name "cloudflared"
        if ($serviceStatus.Status -eq "Running") {
            Write-Success "Service is running!"
        }
        else {
            Write-Warning "Service status: $($serviceStatus.Status)"
        }
    }
    catch {
        Write-Error "Failed to install/start service: $_"
        throw
    }
}

function Show-Summary {
    param(
        [string]$Hostname,
        [string]$TunnelId,
        [int]$IISPort
    )
    
    Write-Step "Installation Complete!"
    
    Write-Host @"

╔════════════════════════════════════════════════════════════════╗
║                   CLOUDFLARE TUNNEL SUMMARY                    ║
╚════════════════════════════════════════════════════════════════╝

  Public URL:       https://$Hostname
  Tunnel ID:        $TunnelId
  Local Service:    http://localhost:$IISPort
  Service Status:   Running (Auto-start enabled)

╔════════════════════════════════════════════════════════════════╗
║                        NEXT STEPS                              ║
╚════════════════════════════════════════════════════════════════╝

  1. Open browser to: https://$Hostname
  2. Verify HTTPS certificate is valid (no warnings)
  3. Test Service Worker / PWA installation
  4. Test login from external network
  5. Verify WebSocket connections (not long polling)

╔════════════════════════════════════════════════════════════════╗
║                    USEFUL COMMANDS                             ║
╚════════════════════════════════════════════════════════════════╝

  Check service status:
    Get-Service cloudflared

  View tunnel info:
    cloudflared tunnel info gfc-webapp

  View service logs:
    Get-EventLog -LogName Application -Source cloudflared -Newest 20

  Restart service:
    Restart-Service cloudflared

  Stop service:
    Stop-Service cloudflared

╔════════════════════════════════════════════════════════════════╗
║                   CONFIGURATION FILES                          ║
╚════════════════════════════════════════════════════════════════╝

  Config:       $env:USERPROFILE\.cloudflared\config.yml
  Credentials:  $env:USERPROFILE\.cloudflared\$TunnelId.json
  Certificate:  $env:USERPROFILE\.cloudflared\cert.pem

"@ -ForegroundColor Cyan

    Write-Success "Setup complete! Your application is now accessible via trusted HTTPS."
}

# Main execution
try {
    Write-Host @"

╔════════════════════════════════════════════════════════════════╗
║           CLOUDFLARE TUNNEL INSTALLATION WIZARD                ║
║                    GFC Web Application                         ║
╚════════════════════════════════════════════════════════════════╝

"@ -ForegroundColor Magenta

    Write-Info "Configuration:"
    Write-Host "  Hostname:    $Hostname" -ForegroundColor Gray
    Write-Host "  IIS Port:    $IISPort" -ForegroundColor Gray
    Write-Host "  Tunnel Name: $TunnelName`n" -ForegroundColor Gray
    
    $response = Read-Host "Continue with installation? (Y/n)"
    if ($response -eq 'n' -or $response -eq 'N') {
        Write-Warning "Installation cancelled"
        exit 0
    }
    
    # Step 1: Install cloudflared
    $cloudflaredPath = Install-Cloudflared
    
    # Step 2: Authenticate
    $certPath = Initialize-CloudflareTunnel -CloudflaredPath $cloudflaredPath
    
    # Step 3: Create tunnel
    $tunnelId = New-CloudflareTunnel -CloudflaredPath $cloudflaredPath -TunnelName $TunnelName
    
    # Step 4: Configure DNS
    Set-TunnelDNS -CloudflaredPath $cloudflaredPath -TunnelName $TunnelName -Hostname $Hostname
    
    # Step 5: Create config file
    $configPath = New-TunnelConfig -TunnelId $tunnelId -Hostname $Hostname -IISPort $IISPort
    
    # Step 6: Ask about test
    Write-Host "`n"
    $testResponse = Read-Host "Do you want to test the tunnel before installing as service? (Y/n)"
    if ($testResponse -ne 'n' -and $testResponse -ne 'N') {
        Test-TunnelConnection -CloudflaredPath $cloudflaredPath -TunnelName $TunnelName -Hostname $Hostname
    }
    
    # Step 7: Install service
    Write-Host "`n"
    $serviceResponse = Read-Host "Install cloudflared as Windows service? (Y/n)"
    if ($serviceResponse -ne 'n' -and $serviceResponse -ne 'N') {
        Install-TunnelService -CloudflaredPath $cloudflaredPath
    }
    
    # Show summary
    Show-Summary -Hostname $Hostname -TunnelId $tunnelId -IISPort $IISPort
    
}
catch {
    Write-Host "`n"
    Write-Error "Installation failed: $_"
    Write-Host "`nStack Trace:" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    exit 1
}
