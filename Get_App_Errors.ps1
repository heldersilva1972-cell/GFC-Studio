# Get the actual error from the application
# Run this on the production server

$appPath = "C:\inetpub\GFCWebApp"

Write-Host "=== Checking Application Errors ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check Windows Event Log for ASP.NET Core errors
Write-Host "[1] Checking Windows Event Log..." -ForegroundColor Green
$events = Get-EventLog -LogName Application -Source "IIS*" -Newest 10 -ErrorAction SilentlyContinue
if ($events) {
    $events | Where-Object { $_.EntryType -eq "Error" } | ForEach-Object {
        Write-Host "  [$($_.TimeGenerated)] $($_.Message)" -ForegroundColor Red
    }
} else {
    Write-Host "  No recent errors in Event Log" -ForegroundColor Gray
}

# 2. Check stdout log (ASP.NET Core writes here)
Write-Host ""
Write-Host "[2] Checking ASP.NET Core stdout logs..." -ForegroundColor Green
$stdoutPath = Join-Path $appPath "logs"
if (Test-Path $stdoutPath) {
    $latestLog = Get-ChildItem -Path $stdoutPath -Filter "*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if ($latestLog) {
        Write-Host "  Latest log: $($latestLog.Name)" -ForegroundColor Gray
        Write-Host "  Content (last 50 lines):" -ForegroundColor Gray
        Get-Content $latestLog.FullName -Tail 50 | ForEach-Object {
            if ($_ -match "error|exception|fail") {
                Write-Host "    $_" -ForegroundColor Red
            } else {
                Write-Host "    $_" -ForegroundColor Gray
            }
        }
    }
} else {
    Write-Host "  No stdout logs found at: $stdoutPath" -ForegroundColor Yellow
}

# 3. Check if web.config has stdout logging enabled
Write-Host ""
Write-Host "[3] Checking web.config..." -ForegroundColor Green
$webConfig = Join-Path $appPath "web.config"
if (Test-Path $webConfig) {
    $config = Get-Content $webConfig -Raw
    if ($config -match 'stdoutLogEnabled="true"') {
        Write-Host "  ✓ Stdout logging is enabled" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Stdout logging is DISABLED" -ForegroundColor Red
        Write-Host "  Enabling it now..." -ForegroundColor Yellow
        $config = $config -replace 'stdoutLogEnabled="false"', 'stdoutLogEnabled="true"'
        Set-Content -Path $webConfig -Value $config
        Write-Host "  ✓ Enabled. Restarting app pool..." -ForegroundColor Green
        Restart-WebAppPool -Name "GFCWebApp"
        Start-Sleep -Seconds 5
    }
}

# 4. Try to access the app and capture the error
Write-Host ""
Write-Host "[4] Testing application response..." -ForegroundColor Green
try {
    $response = Invoke-WebRequest -Uri "http://localhost/controllers" -UseBasicParsing -ErrorAction Stop
    Write-Host "  ✓ Application responded with status: $($response.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Application error: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "  Error details:" -ForegroundColor Red
        Write-Host $responseBody -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "=== Check Complete ===" -ForegroundColor Cyan
