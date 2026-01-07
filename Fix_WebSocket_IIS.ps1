# Fix IIS WebSocket configuration for Blazor Server
# Run this on the production server AS ADMINISTRATOR

Write-Host "=== Fixing IIS WebSocket Configuration ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check if WebSocket feature is installed
Write-Host "[1] Checking WebSocket feature..." -ForegroundColor Green
$wsFeature = Get-WindowsFeature -Name "Web-WebSockets" -ErrorAction SilentlyContinue
if ($wsFeature) {
    if ($wsFeature.Installed) {
        Write-Host "  ✓ WebSocket feature is installed" -ForegroundColor Green
    } else {
        Write-Host "  ✗ WebSocket feature is NOT installed" -ForegroundColor Red
        Write-Host "  Installing WebSocket feature..." -ForegroundColor Yellow
        Install-WindowsFeature -Name "Web-WebSockets"
        Write-Host "  ✓ Installed" -ForegroundColor Green
    }
} else {
    Write-Host "  Checking via DISM..." -ForegroundColor Gray
    $dism = dism /online /get-featureinfo /featurename:IIS-WebSockets
    if ($dism -match "State : Enabled") {
        Write-Host "  ✓ WebSocket feature is enabled" -ForegroundColor Green
    } else {
        Write-Host "  ✗ WebSocket feature is NOT enabled" -ForegroundColor Red
        Write-Host "  Enabling WebSocket feature..." -ForegroundColor Yellow
        dism /online /enable-feature /featurename:IIS-WebSockets /all
        Write-Host "  ✓ Enabled" -ForegroundColor Green
    }
}

# 2. Enable WebSocket in IIS site
Write-Host ""
Write-Host "[2] Enabling WebSocket in IIS site..." -ForegroundColor Green
Import-Module WebAdministration
$siteName = "GFCWebApp"

try {
    # Enable WebSocket protocol
    Set-WebConfigurationProperty -PSPath "IIS:\Sites\$siteName" -Filter "system.webServer/webSocket" -Name "enabled" -Value $true
    Write-Host "  ✓ WebSocket enabled for site" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to enable WebSocket: $_" -ForegroundColor Red
}

# 3. Check web.config for WebSocket settings
Write-Host ""
Write-Host "[3] Checking web.config..." -ForegroundColor Green
$webConfig = "C:\inetpub\GFCWebApp\web.config"
if (Test-Path $webConfig) {
    $config = [xml](Get-Content $webConfig)
    
    # Ensure webSocket element exists
    $webSocket = $config.configuration.'system.webServer'.webSocket
    if (-not $webSocket) {
        Write-Host "  Adding WebSocket configuration to web.config..." -ForegroundColor Yellow
        
        $webSocketElement = $config.CreateElement("webSocket")
        $webSocketElement.SetAttribute("enabled", "true")
        $config.configuration.'system.webServer'.AppendChild($webSocketElement) | Out-Null
        
        $config.Save($webConfig)
        Write-Host "  ✓ WebSocket configuration added" -ForegroundColor Green
    } else {
        Write-Host "  ✓ WebSocket configuration exists" -ForegroundColor Green
    }
}

# 4. Restart IIS
Write-Host ""
Write-Host "[4] Restarting IIS..." -ForegroundColor Green
iisreset /restart
Write-Host "  ✓ IIS restarted" -ForegroundColor Green

Write-Host ""
Write-Host "=== Fix Complete ===" -ForegroundColor Cyan
Write-Host "Wait 30 seconds for IIS to fully restart, then test the site." -ForegroundColor Yellow
Start-Sleep -Seconds 30
Write-Host "Ready to test: https://gfc.lovanow.com" -ForegroundColor Green
