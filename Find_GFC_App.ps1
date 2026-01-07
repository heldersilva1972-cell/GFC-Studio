# Find where the GFC application is actually deployed
# Run this on the production server

Write-Host "=== Finding GFC Application ===" -ForegroundColor Cyan
Write-Host ""

# Check common IIS paths
$commonPaths = @(
    "C:\inetpub\wwwroot",
    "C:\inetpub\wwwroot\GFC",
    "C:\inetpub\sites",
    "C:\PublishGFCWebApp",
    "C:\GFC",
    "D:\inetpub\wwwroot"
)

Write-Host "[1] Searching common paths..." -ForegroundColor Green
foreach ($path in $commonPaths) {
    if (Test-Path $path) {
        $dll = Get-ChildItem -Path $path -Recurse -Filter "GFC.BlazorServer.dll" -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($dll) {
            Write-Host "  ✓ FOUND: $($dll.DirectoryName)" -ForegroundColor Green
            Write-Host "    Last modified: $($dll.LastWriteTime)" -ForegroundColor Gray
        }
    }
}

Write-Host ""
Write-Host "[2] Checking IIS Sites..." -ForegroundColor Green
Import-Module WebAdministration -ErrorAction SilentlyContinue
$sites = Get-ChildItem IIS:\Sites

foreach ($site in $sites) {
    Write-Host "  Site: $($site.Name)" -ForegroundColor Yellow
    Write-Host "    Physical Path: $($site.PhysicalPath)" -ForegroundColor Gray
    Write-Host "    State: $($site.State)" -ForegroundColor Gray
    
    # Check if GFC app is in this site
    $dll = Get-ChildItem -Path $site.PhysicalPath -Recurse -Filter "GFC.BlazorServer.dll" -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($dll) {
        Write-Host "    ✓ GFC Application FOUND HERE!" -ForegroundColor Green
    }
    Write-Host ""
}

Write-Host "=== Search Complete ===" -ForegroundColor Cyan
