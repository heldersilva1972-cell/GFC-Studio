# Temporary IP Change Helper
# This script helps you temporarily change your IP to configure the controller

Write-Host "=== Temporary IP Configuration Helper ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "This will help you temporarily set your IP to 192.168.1.17" -ForegroundColor Yellow
Write-Host "so you can configure the controller's Allowed PC settings." -ForegroundColor Yellow
Write-Host ""

# Get network adapters
$adapters = Get-NetAdapter | Where-Object {$_.Status -eq "Up" -and $_.InterfaceDescription -notlike "*Bluetooth*" -and $_.InterfaceDescription -notlike "*Virtual*"}

if ($adapters.Count -eq 0) {
    Write-Host "No active network adapters found!" -ForegroundColor Red
    exit
}

Write-Host "Active Network Adapters:" -ForegroundColor Cyan
for ($i = 0; $i -lt $adapters.Count; $i++) {
    $adapter = $adapters[$i]
    $ipConfig = Get-NetIPAddress -InterfaceIndex $adapter.InterfaceIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue
    $currentIP = if ($ipConfig) { $ipConfig.IPAddress } else { "No IP" }
    Write-Host "  [$i] $($adapter.Name) - $currentIP" -ForegroundColor Gray
}

Write-Host ""
$selection = Read-Host "Select adapter number (or 'q' to quit)"

if ($selection -eq 'q') {
    Write-Host "Cancelled." -ForegroundColor Yellow
    exit
}

$selectedAdapter = $adapters[[int]$selection]
Write-Host ""
Write-Host "Selected: $($selectedAdapter.Name)" -ForegroundColor Green
Write-Host ""

# Save current configuration
$currentConfig = Get-NetIPConfiguration -InterfaceIndex $selectedAdapter.InterfaceIndex
$currentIP = $currentConfig.IPv4Address.IPAddress
$currentGateway = $currentConfig.IPv4DefaultGateway.NextHop
$isDHCP = (Get-NetIPInterface -InterfaceIndex $selectedAdapter.InterfaceIndex -AddressFamily IPv4).Dhcp

Write-Host "Current Configuration:" -ForegroundColor Cyan
Write-Host "  IP: $currentIP" -ForegroundColor Gray
Write-Host "  Gateway: $currentGateway" -ForegroundColor Gray
Write-Host "  DHCP: $isDHCP" -ForegroundColor Gray
Write-Host ""

Write-Host "IMPORTANT: This script requires Administrator privileges!" -ForegroundColor Yellow
Write-Host ""
Write-Host "What would you like to do?" -ForegroundColor Cyan
Write-Host "  [1] Set temporary IP (192.168.1.17)" -ForegroundColor Gray
Write-Host "  [2] Restore to automatic (DHCP)" -ForegroundColor Gray
Write-Host "  [q] Quit" -ForegroundColor Gray
Write-Host ""

$action = Read-Host "Select action"

if ($action -eq '1') {
    Write-Host ""
    Write-Host "Setting temporary IP..." -ForegroundColor Cyan
    
    try {
        # Remove existing IP
        Remove-NetIPAddress -InterfaceIndex $selectedAdapter.InterfaceIndex -Confirm:$false -ErrorAction SilentlyContinue
        Remove-NetRoute -InterfaceIndex $selectedAdapter.InterfaceIndex -Confirm:$false -ErrorAction SilentlyContinue
        
        # Set new IP
        New-NetIPAddress -InterfaceIndex $selectedAdapter.InterfaceIndex `
                        -IPAddress "192.168.1.17" `
                        -PrefixLength 24 `
                        -DefaultGateway "192.168.1.1" `
                        -ErrorAction Stop | Out-Null
        
        Write-Host "✓ IP changed to 192.168.1.17" -ForegroundColor Green
        Write-Host ""
        Write-Host "Next steps:" -ForegroundColor Cyan
        Write-Host "  1. Run: configure_controller_allowedpc.ps1" -ForegroundColor Gray
        Write-Host "  2. After configuration, run this script again and select option [2]" -ForegroundColor Gray
        Write-Host "  3. This will restore your automatic IP" -ForegroundColor Gray
    }
    catch {
        Write-Host "✗ Error: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
        Write-Host "Make sure you run PowerShell as Administrator!" -ForegroundColor Yellow
    }
}
elseif ($action -eq '2') {
    Write-Host ""
    Write-Host "Restoring automatic IP (DHCP)..." -ForegroundColor Cyan
    
    try {
        # Remove static IP
        Remove-NetIPAddress -InterfaceIndex $selectedAdapter.InterfaceIndex -Confirm:$false -ErrorAction SilentlyContinue
        Remove-NetRoute -InterfaceIndex $selectedAdapter.InterfaceIndex -Confirm:$false -ErrorAction SilentlyContinue
        
        # Enable DHCP
        Set-NetIPInterface -InterfaceIndex $selectedAdapter.InterfaceIndex -Dhcp Enabled
        
        Write-Host "✓ DHCP enabled" -ForegroundColor Green
        Write-Host "  Waiting for IP..." -ForegroundColor Gray
        
        Start-Sleep -Seconds 3
        
        $newConfig = Get-NetIPConfiguration -InterfaceIndex $selectedAdapter.InterfaceIndex
        $newIP = $newConfig.IPv4Address.IPAddress
        
        Write-Host "✓ New IP: $newIP" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "Cancelled." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
