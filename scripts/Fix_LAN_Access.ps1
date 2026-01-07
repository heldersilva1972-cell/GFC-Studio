# GFC LAN Access Fixer (PowerShell)
# This script points gfc.lovanow.com to the local server IP

$serverIp = "192.168.0.72"
$domain = "gfc.lovanow.com"
$hostsPath = "$env:windir\system32\drivers\etc\hosts"

Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "GFC LAN Access Fixer (NAT Loopback Bypass)" -ForegroundColor Cyan
Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "Pointing $domain to $serverIp..."

# Check Admin
$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "[ERROR] Please run this script as ADMINISTRATOR!" -ForegroundColor Red
    Pause
    return
}

# Backup
Copy-Item $hostsPath "$hostsPath.bak" -Force

# Read hosts file
$hosts = Get-Content $hostsPath

# Check if entry already exists
if ($hosts -match "$domain") {
    Write-Host "[INFO] Entry for $domain found. Updating..." -ForegroundColor Yellow
    $newHosts = $hosts -replace ".*$domain.*", "$serverIp $domain"
} else {
    Write-Host "[INFO] Adding new entry for $domain..." -ForegroundColor Green
    $newHosts = $hosts + "`n$serverIp $domain"
}

# Save (using ASCII to avoid BOM issues in some hosts parsers)
[System.IO.File]::WriteAllLines($hostsPath, $newHosts, [System.Text.Encoding]::ASCII)

Write-Host "[SUCCESS] Hosts file updated!" -ForegroundColor Green
ipconfig /flushdns | Out-Null
Write-Host "DNS Flushed."

Write-Host "========================================================"
Write-Host "Try visiting now: https://$domain" -ForegroundColor Cyan
Write-Host "========================================================"
Pause
