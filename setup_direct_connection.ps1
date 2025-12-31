# Direct PC-to-Controller Network Setup
# For when controller is plugged directly into PC (no router)

Write-Host "=== Direct Controller Connection Setup ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "This will configure your PC to communicate with the controller" -ForegroundColor Yellow
Write-Host "when connected directly (no router/switch)." -ForegroundColor Yellow
Write-Host ""

# Get network adapters
$adapters = Get-NetAdapter | Where-Object {$_.Status -eq "Up" -and $_.InterfaceDescription -notlike "*Bluetooth*" -and $_.InterfaceDescription -notlike "*Virtual*" -and $_.InterfaceDescription -notlike "*Wi-Fi*"}

if ($adapters.Count -eq 0) {
    Write-Host "No wired network adapters found!" -ForegroundColor Red
    Write-Host "Make sure the controller is plugged into your Ethernet port." -ForegroundColor Yellow
    exit
}

Write-Host "Wired Network Adapters:" -ForegroundColor Cyan
for ($i = 0; $i -lt $adapters.Count; $i++) {
    $adapter = $adapters[$i]
    $ipConfig = Get-NetIPAddress -InterfaceIndex $adapter.InterfaceIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue
    $currentIP = if ($ipConfig) { $ipConfig.IPAddress } else { "No IP" }
    Write-Host "  [$i] $($adapter.Name) - $currentIP" -ForegroundColor Gray
}

Write-Host ""
$selection = Read-Host "Select the adapter connected to the controller (or 'q' to quit)"

if ($selection -eq 'q') {
    exit
}

$selectedAdapter = $adapters[[int]$selection]
Write-Host ""
Write-Host "Selected: $($selectedAdapter.Name)" -ForegroundColor Green
Write-Host ""

Write-Host "WG3000 Controller Default Network Settings:" -ForegroundColor Cyan
Write-Host "  Controller Default IP: 192.168.1.200" -ForegroundColor Yellow
Write-Host "  Subnet Mask: 255.255.255.0" -ForegroundColor Yellow
Write-Host ""
Write-Host "We'll configure your PC as:" -ForegroundColor Cyan
Write-Host "  PC IP: 192.168.1.100" -ForegroundColor Green
Write-Host "  Subnet: 255.255.255.0" -ForegroundColor Green
Write-Host "  (No gateway needed for direct connection)" -ForegroundColor Gray
Write-Host ""

$confirm = Read-Host "Configure this adapter? (y/n)"

if ($confirm -ne 'y') {
    Write-Host "Cancelled." -ForegroundColor Yellow
    exit
}

Write-Host ""
Write-Host "Configuring network adapter..." -ForegroundColor Cyan

try {
    # Remove existing IP configuration
    Remove-NetIPAddress -InterfaceIndex $selectedAdapter.InterfaceIndex -Confirm:$false -ErrorAction SilentlyContinue
    Remove-NetRoute -InterfaceIndex $selectedAdapter.InterfaceIndex -Confirm:$false -ErrorAction SilentlyContinue
    
    # Set static IP
    New-NetIPAddress -InterfaceIndex $selectedAdapter.InterfaceIndex `
                    -IPAddress "192.168.1.100" `
                    -PrefixLength 24 `
                    -ErrorAction Stop | Out-Null
    
    Write-Host "✓ Network configured successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "  Your PC IP: 192.168.1.100" -ForegroundColor Cyan
    Write-Host "  Controller IP: 192.168.1.200 (default)" -ForegroundColor Cyan
    Write-Host ""
    
    # Test connectivity
    Write-Host "Testing controller connectivity..." -ForegroundColor Yellow
    Start-Sleep -Seconds 2
    
    $ping = Test-Connection -ComputerName "192.168.1.200" -Count 2 -Quiet
    
    if ($ping) {
        Write-Host "✓✓✓ SUCCESS! Controller is responding at 192.168.1.200!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Next steps:" -ForegroundColor Cyan
        Write-Host "  1. Update your database/config with controller IP: 192.168.1.200" -ForegroundColor Gray
        Write-Host "  2. Run your GFC application" -ForegroundColor Gray
        Write-Host "  3. The controller should now show as CONNECTED" -ForegroundColor Gray
    } else {
        Write-Host "⚠ Controller not responding at 192.168.1.200" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "The controller might be using a different default IP." -ForegroundColor Yellow
        Write-Host "Common defaults:" -ForegroundColor Cyan
        Write-Host "  - 192.168.1.200" -ForegroundColor Gray
        Write-Host "  - 192.168.1.201" -ForegroundColor Gray
        Write-Host "  - 192.168.0.200" -ForegroundColor Gray
        Write-Host ""
        Write-Host "Trying to discover controller..." -ForegroundColor Cyan
        
        # Try discovery on this network
        & "$PSScriptRoot\discover_controllers.ps1"
    }
}
catch {
    Write-Host "✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Make sure you run PowerShell as Administrator!" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
