# GFC Site Finder
# Run this on the HOST computer to find the correct IIS site and port

Import-Module WebAdministration
Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "SCANNING FOR GFC WEBSITE..." -ForegroundColor Cyan
Write-Host "========================================================"

$sites = Get-Website
foreach ($site in $sites) {
    $bindings = $site.Bindings.Collection.bindingInformation
    Write-Host "Site Name: $($site.Name)" -ForegroundColor Green
    Write-Host "  State:   $($site.State)"
    Write-Host "  Ports:   $bindings"
}

Write-Host "========================================================"
Write-Host "Please tell me which Site Name and Port you see above." -ForegroundColor Yellow
Pause
