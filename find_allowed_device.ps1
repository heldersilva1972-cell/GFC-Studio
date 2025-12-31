# Find Device with IP 192.168.1.17
# This script scans your network to find which device has the allowed IP

Write-Host "=== Finding Device 192.168.1.17 ===" -ForegroundColor Cyan
Write-Host ""

# Check if 192.168.1.17 is reachable
Write-Host "Pinging 192.168.1.17..." -ForegroundColor Yellow
$ping = Test-Connection -ComputerName "192.168.1.17" -Count 2 -Quiet

if ($ping) {
    Write-Host "✓ Device found at 192.168.1.17!" -ForegroundColor Green
    Write-Host ""
    
    # Try to get hostname
    try {
        $hostname = [System.Net.Dns]::GetHostEntry("192.168.1.17").HostName
        Write-Host "Hostname: $hostname" -ForegroundColor Cyan
    }
    catch {
        Write-Host "Hostname: (unable to resolve)" -ForegroundColor Gray
    }
    
    # Try to get MAC address
    $arp = arp -a | Select-String "192.168.1.17"
    if ($arp) {
        Write-Host "ARP Entry: $arp" -ForegroundColor Gray
    }
    
    Write-Host ""
    Write-Host "This device can communicate with the controller!" -ForegroundColor Green
    Write-Host "If this is another PC, use N3000.exe on that PC to change the Allowed PC setting." -ForegroundColor Yellow
} else {
    Write-Host "✗ No device responding at 192.168.1.17" -ForegroundColor Red
    Write-Host ""
    Write-Host "Possible reasons:" -ForegroundColor Yellow
    Write-Host "  1. The device is offline" -ForegroundColor Gray
    Write-Host "  2. The device's IP changed (DHCP)" -ForegroundColor Gray
    Write-Host "  3. This was a temporary IP that no longer exists" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Your current PC's IP addresses:" -ForegroundColor Cyan
Get-NetIPAddress -AddressFamily IPv4 | Where-Object {$_.IPAddress -like "192.168.*"} | ForEach-Object {
    Write-Host "  $($_.IPAddress) - $($_.InterfaceAlias)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=== Recommendation ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Since 192.168.1.17 is not responding, the easiest solution is:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Temporarily change THIS PC's IP to 192.168.1.17" -ForegroundColor White
Write-Host "   Run: .\change_ip_helper.ps1 (as Administrator)" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Configure the controller to allow all PCs" -ForegroundColor White
Write-Host "   Run: .\configure_controller_allowedpc.ps1" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Change your IP back to automatic" -ForegroundColor White
Write-Host "   Run: .\change_ip_helper.ps1 again (as Administrator)" -ForegroundColor Gray
Write-Host ""
Write-Host "This is a ONE-TIME setup. After this, your GFC app will work from any PC!" -ForegroundColor Green

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
