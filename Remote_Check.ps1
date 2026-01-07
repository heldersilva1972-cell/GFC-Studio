# Run diagnostics on remote server from your laptop
# Run this on your LAPTOP

$serverIP = "192.168.1.72"
$appPath = "C:\inetpub\GFCWebApp"

Write-Host "=== Remote Server Diagnostics ===" -ForegroundColor Cyan
Write-Host "Server: $serverIP" -ForegroundColor Gray
Write-Host ""

# Method 1: Check via web request
Write-Host "[1] Testing web access..." -ForegroundColor Green
try {
    $response = Invoke-WebRequest -Uri "https://gfc.lovanow.com/controllers" -UseBasicParsing -TimeoutSec 10
    Write-Host "  ✓ Page responded with status: $($response.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Page failed: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.Exception.Response) {
        $statusCode = $_.Exception.Response.StatusCode.value__
        Write-Host "  HTTP Status: $statusCode" -ForegroundColor Red
    }
}

# Method 2: Check file timestamp via network share (if accessible)
Write-Host ""
Write-Host "[2] Checking deployment via network share..." -ForegroundColor Green
$networkPath = "\\$serverIP\C$\inetpub\GFCWebApp\GFC.BlazorServer.dll"
try {
    if (Test-Path $networkPath) {
        $dll = Get-Item $networkPath
        Write-Host "  Last deployed: $($dll.LastWriteTime)" -ForegroundColor Yellow
        $minutesAgo = [math]::Round(((Get-Date) - $dll.LastWriteTime).TotalMinutes, 1)
        Write-Host "  ($minutesAgo minutes ago)" -ForegroundColor Gray
    } else {
        Write-Host "  Cannot access network share" -ForegroundColor Red
        Write-Host "  You may need to enable admin shares or use Remote Desktop" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  Cannot access network share: $_" -ForegroundColor Red
    Write-Host "  Try: Remote Desktop to $serverIP" -ForegroundColor Yellow
}

# Method 3: Try remote PowerShell (if enabled)
Write-Host ""
Write-Host "[3] Attempting remote PowerShell..." -ForegroundColor Green
try {
    $session = New-PSSession -ComputerName $serverIP -ErrorAction Stop
    
    $result = Invoke-Command -Session $session -ScriptBlock {
        $dll = Get-Item "C:\inetpub\GFCWebApp\GFC.BlazorServer.dll"
        @{
            LastWriteTime = $dll.LastWriteTime
            Size = $dll.Length
        }
    }
    
    Write-Host "  ✓ Remote PowerShell successful" -ForegroundColor Green
    Write-Host "  Last deployed: $($result.LastWriteTime)" -ForegroundColor Yellow
    
    Remove-PSSession $session
} catch {
    Write-Host "  ✗ Remote PowerShell not available: $_" -ForegroundColor Red
    Write-Host "  You'll need to use Remote Desktop" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Recommendation ===" -ForegroundColor Cyan
Write-Host "Use Remote Desktop to connect to $serverIP and run the diagnostics there" -ForegroundColor Yellow
Write-Host "Or press F12 in your browser and screenshot the Console errors" -ForegroundColor Yellow
