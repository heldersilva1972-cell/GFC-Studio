# Check what version is actually deployed
# Run this on the production server

$appPath = "C:\inetpub\GFCWebApp"

Write-Host "=== Checking Deployed Version ===" -ForegroundColor Cyan
Write-Host ""

# Check when files were last deployed
Write-Host "[1] Deployment timestamp..." -ForegroundColor Green
$dll = Get-ChildItem -Path $appPath -Filter "GFC.BlazorServer.dll" | Select-Object -First 1
if ($dll) {
    Write-Host "  Last deployed: $($dll.LastWriteTime)" -ForegroundColor Yellow
    $minutesAgo = [math]::Round(((Get-Date) - $dll.LastWriteTime).TotalMinutes, 1)
    Write-Host "  ($minutesAgo minutes ago)" -ForegroundColor Gray
}

# Check the actual error from logs
Write-Host ""
Write-Host "[2] Latest application error..." -ForegroundColor Green
$logsPath = Join-Path $appPath "logs"
if (Test-Path $logsPath) {
    $latestLog = Get-ChildItem -Path $logsPath -Filter "*.log" | 
        Sort-Object LastWriteTime -Descending | 
        Select-Object -First 1
    
    if ($latestLog) {
        Write-Host "  Checking: $($latestLog.Name)" -ForegroundColor Gray
        $errors = Get-Content $latestLog.FullName | Select-String "exception|error" -Context 0,3
        if ($errors) {
            Write-Host "  ERRORS FOUND:" -ForegroundColor Red
            $errors | Select-Object -Last 10 | ForEach-Object {
                Write-Host "    $_" -ForegroundColor Red
            }
        } else {
            Write-Host "  No errors in latest log" -ForegroundColor Green
        }
    }
}

# Try to access the page and see what happens
Write-Host ""
Write-Host "[3] Testing /controllers page..." -ForegroundColor Green
try {
    $response = Invoke-WebRequest -Uri "http://localhost/controllers" -UseBasicParsing -TimeoutSec 10
    Write-Host "  Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "  Page loaded successfully!" -ForegroundColor Green
} catch {
    Write-Host "  ERROR: $($_.Exception.Message)" -ForegroundColor Red
    
    # Try to get more details
    if ($_.Exception.Response) {
        $statusCode = $_.Exception.Response.StatusCode.value__
        Write-Host "  HTTP Status Code: $statusCode" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== Check Complete ===" -ForegroundColor Cyan
