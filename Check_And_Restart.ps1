# Check the deployed application configuration
# Run this on the production server

$appPath = "C:\inetpub\GFCWebApp"

Write-Host "=== Checking Deployed Application ===" -ForegroundColor Cyan
Write-Host "Path: $appPath" -ForegroundColor Gray
Write-Host ""

# 1. Check appsettings.json
Write-Host "[1] Checking appsettings.json..." -ForegroundColor Green
$appsettings = Join-Path $appPath "appsettings.json"
if (Test-Path $appsettings) {
    $content = Get-Content $appsettings -Raw | ConvertFrom-Json
    $connString = $content.ConnectionStrings.GFC
    Write-Host "  Connection String: $connString" -ForegroundColor Yellow
    
    if ($connString -like "*SQLEXPRESS*") {
        Write-Host "  ✓ Correctly pointing to SQLEXPRESS" -ForegroundColor Green
    } else {
        Write-Host "  ✗ NOT pointing to SQLEXPRESS!" -ForegroundColor Red
    }
} else {
    Write-Host "  ERROR: appsettings.json not found" -ForegroundColor Red
}

# 2. Check when it was last deployed
Write-Host ""
Write-Host "[2] Checking deployment date..." -ForegroundColor Green
$dll = Get-ChildItem -Path $appPath -Filter "GFC.BlazorServer.dll" | Select-Object -First 1
if ($dll) {
    Write-Host "  Last deployed: $($dll.LastWriteTime)" -ForegroundColor Yellow
    
    $minutesAgo = [math]::Round(((Get-Date) - $dll.LastWriteTime).TotalMinutes, 1)
    Write-Host "  ($minutesAgo minutes ago)" -ForegroundColor Gray
    
    if ($minutesAgo -gt 60) {
        Write-Host "  ⚠ WARNING: This is OLD! You may not have published the latest code." -ForegroundColor Red
    } else {
        Write-Host "  ✓ Recently deployed" -ForegroundColor Green
    }
}

# 3. Check Program.cs for middleware (we can't directly check compiled code, but we can check file dates)
Write-Host ""
Write-Host "[3] Checking for recent changes..." -ForegroundColor Green
$allFiles = Get-ChildItem -Path $appPath -Recurse -File | Sort-Object LastWriteTime -Descending | Select-Object -First 10
Write-Host "  Most recently modified files:" -ForegroundColor Gray
$allFiles | ForEach-Object {
    Write-Host "    $($_.Name) - $($_.LastWriteTime)" -ForegroundColor Gray
}

# 4. Restart IIS to apply any changes
Write-Host ""
Write-Host "[4] Restarting IIS Application Pool..." -ForegroundColor Green
try {
    Restart-WebAppPool -Name "GFCWebApp"
    Write-Host "  ✓ Application pool restarted" -ForegroundColor Green
    Write-Host "  Wait 10 seconds for the app to restart..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
    Write-Host "  ✓ Ready to test" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to restart: $_" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Check Complete ===" -ForegroundColor Cyan
Write-Host "Now test: https://gfc.lovanow.com" -ForegroundColor Yellow
