# Get the ACTUAL application error from logs
# Run on production server

$appPath = "C:\inetpub\GFCWebApp"

Write-Host "=== Getting Real Application Error ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check stdout logs
Write-Host "[1] Checking application logs..." -ForegroundColor Green
$logsPath = Join-Path $appPath "logs"
if (Test-Path $logsPath) {
    $latestLog = Get-ChildItem -Path $logsPath -Filter "*.log" -ErrorAction SilentlyContinue | 
        Sort-Object LastWriteTime -Descending | 
        Select-Object -First 1
    
    if ($latestLog) {
        Write-Host "  Latest log: $($latestLog.Name)" -ForegroundColor Gray
        Write-Host "  Last 100 lines:" -ForegroundColor Yellow
        Get-Content $latestLog.FullName -Tail 100
    } else {
        Write-Host "  No log files found" -ForegroundColor Red
    }
} else {
    Write-Host "  Logs directory doesn't exist: $logsPath" -ForegroundColor Red
}

# 2. Check Windows Event Log
Write-Host ""
Write-Host "[2] Checking Windows Event Log for ASP.NET errors..." -ForegroundColor Green
$events = Get-EventLog -LogName Application -Newest 20 -ErrorAction SilentlyContinue | 
    Where-Object { $_.Source -like "*ASP.NET*" -or $_.Source -like "*IIS*" } |
    Where-Object { $_.EntryType -eq "Error" }

if ($events) {
    $events | ForEach-Object {
        Write-Host "  [$($_.TimeGenerated)] $($_.Message)" -ForegroundColor Red
    }
} else {
    Write-Host "  No recent ASP.NET errors" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=== End of Logs ===" -ForegroundColor Cyan
