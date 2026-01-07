# Check what's actually deployed on the production server
# Run this ON THE PRODUCTION SERVER (192.168.1.72) via Remote Desktop

$publishPath = "C:\inetpub\wwwroot\GFC"  # Adjust if your publish path is different

Write-Host "=== Production Server Diagnostics ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check if the middleware is disabled in the deployed code
Write-Host "[1] Checking Program.cs for middleware status..." -ForegroundColor Green
$programCs = Get-ChildItem -Path $publishPath -Recurse -Filter "GFC.BlazorServer.dll" -ErrorAction SilentlyContinue

if ($programCs) {
    Write-Host "  Found deployed application at: $($programCs.FullName)" -ForegroundColor Gray
    Write-Host "  Last modified: $($programCs.LastWriteTime)" -ForegroundColor Gray
} else {
    Write-Host "  ERROR: Cannot find deployed application" -ForegroundColor Red
}

# 2. Check appsettings.json
Write-Host ""
Write-Host "[2] Checking appsettings.json..." -ForegroundColor Green
$appsettings = Join-Path $publishPath "appsettings.json"
if (Test-Path $appsettings) {
    $content = Get-Content $appsettings -Raw | ConvertFrom-Json
    $connString = $content.ConnectionStrings.GFC
    Write-Host "  Connection String: $connString" -ForegroundColor Yellow
    
    if ($connString -like "*SQLEXPRESS*") {
        Write-Host "  ✓ Pointing to SQLEXPRESS" -ForegroundColor Green
    } else {
        Write-Host "  ✗ NOT pointing to SQLEXPRESS!" -ForegroundColor Red
    }
} else {
    Write-Host "  ERROR: appsettings.json not found" -ForegroundColor Red
}

# 3. Check IIS Application Pool status
Write-Host ""
Write-Host "[3] Checking IIS Application Pool..." -ForegroundColor Green
Import-Module WebAdministration -ErrorAction SilentlyContinue
$appPool = Get-ChildItem IIS:\AppPools | Where-Object { $_.Name -like "*GFC*" } | Select-Object -First 1

if ($appPool) {
    Write-Host "  App Pool: $($appPool.Name)" -ForegroundColor Gray
    Write-Host "  Status: $($appPool.State)" -ForegroundColor Gray
    
    if ($appPool.State -ne "Started") {
        Write-Host "  WARNING: App pool is not running!" -ForegroundColor Red
        Write-Host "  Run: Start-WebAppPool -Name '$($appPool.Name)'" -ForegroundColor Yellow
    }
} else {
    Write-Host "  ERROR: Cannot find GFC application pool" -ForegroundColor Red
}

# 4. Check recent error logs
Write-Host ""
Write-Host "[4] Checking recent errors..." -ForegroundColor Green
$logPath = "C:\inetpub\logs\LogFiles"
$recentLogs = Get-ChildItem -Path $logPath -Recurse -Filter "*.log" | 
    Sort-Object LastWriteTime -Descending | 
    Select-Object -First 1

if ($recentLogs) {
    Write-Host "  Latest log: $($recentLogs.FullName)" -ForegroundColor Gray
    Write-Host "  Last modified: $($recentLogs.LastWriteTime)" -ForegroundColor Gray
    
    $errors = Get-Content $recentLogs.FullName | Select-String "500|error" -SimpleMatch | Select-Object -Last 5
    if ($errors) {
        Write-Host "  Recent errors found:" -ForegroundColor Red
        $errors | ForEach-Object { Write-Host "    $_" -ForegroundColor Gray }
    } else {
        Write-Host "  No recent errors in IIS logs" -ForegroundColor Green
    }
}

# 5. Test database connection
Write-Host ""
Write-Host "[5] Testing database connection..." -ForegroundColor Green
try {
    $result = sqlcmd -S ".\SQLEXPRESS" -d "ClubMembership" -Q "SELECT COUNT(*) FROM Members" -E -h -1 -W
    Write-Host "  ✓ Database accessible, Member count: $($result.Trim())" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Cannot connect to database: $_" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Diagnostics Complete ===" -ForegroundColor Cyan
