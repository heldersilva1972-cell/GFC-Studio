# Scan for WG3000 Controller
# Tries common default IPs

Write-Host "=== Scanning for Controller ===" -ForegroundColor Cyan
Write-Host ""

$commonIPs = @(
    "192.168.1.200",
    "192.168.1.201",
    "192.168.1.72",
    "192.168.0.200",
    "192.168.0.201"
)

Write-Host "Trying common controller IPs..." -ForegroundColor Yellow
Write-Host ""

foreach ($ip in $commonIPs) {
    Write-Host "Testing $ip..." -ForegroundColor Gray -NoNewline
    $ping = Test-Connection -ComputerName $ip -Count 1 -Quiet -ErrorAction SilentlyContinue
    
    if ($ping) {
        Write-Host " ✓ FOUND!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Controller IP: $ip" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "Update your configuration:" -ForegroundColor Yellow
        Write-Host "  1. Database: UPDATE Controllers SET IpAddress = '$ip'" -ForegroundColor Gray
        Write-Host "  2. appsettings.json: DefaultControllerIp = '$ip'" -ForegroundColor Gray
        Write-Host ""
        Write-Host "Then restart your GFC application!" -ForegroundColor Green
        break
    } else {
        Write-Host " No response" -ForegroundColor DarkGray
    }
}

if (-not $ping) {
    Write-Host ""
    Write-Host "Controller not found at common IPs." -ForegroundColor Red
    Write-Host ""
    Write-Host "Let's scan the entire 192.168.1.x subnet..." -ForegroundColor Yellow
    Write-Host "This will take about 2 minutes..." -ForegroundColor Gray
    Write-Host ""
    
    for ($i = 1; $i -le 254; $i++) {
        $ip = "192.168.1.$i"
        if ($i % 10 -eq 0) {
            Write-Host "Scanning 192.168.1.$i..." -ForegroundColor DarkGray
        }
        
        $ping = Test-Connection -ComputerName $ip -Count 1 -Quiet -ErrorAction SilentlyContinue
        
        if ($ping) {
            Write-Host ""
            Write-Host "✓ Device found at $ip" -ForegroundColor Green
            
            # Try to identify if it's the controller
            try {
                $hostname = [System.Net.Dns]::GetHostEntry($ip).HostName
                Write-Host "  Hostname: $hostname" -ForegroundColor Cyan
            } catch {
                Write-Host "  Hostname: (unknown)" -ForegroundColor Gray
            }
            
            Write-Host "  This might be your controller!" -ForegroundColor Yellow
            Write-Host ""
        }
    }
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
