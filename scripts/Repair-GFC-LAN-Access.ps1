# GFC LAN Access & Security Repair Script
# Run this as ADMINISTRATOR on the SERVER computer

Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "GFC LAN ACCESS & SECURITY REPAIR" -ForegroundColor Cyan
Write-Host "========================================================" -ForegroundColor Cyan

# 1. FIX FIREWALL
Write-Host "[1/3] Configuring Windows Firewall for Port 8080..." -ForegroundColor Yellow
$ruleName = "GFC-Studio-Inbound-8080"
if (Get-NetFirewallRule -DisplayName $ruleName -ErrorAction SilentlyContinue) {
    Remove-NetFirewallRule -DisplayName $ruleName
}
New-NetFirewallRule -DisplayName $ruleName -Direction Inbound -LocalPort 8080 -Protocol TCP -Action Allow -Description "Allow GFC Web App LAN Access" | Out-Null
Write-Host "✓ Firewall Rule Created/Updated." -ForegroundColor Green

# 2. UPDATE DATABASE SETTINGS
Write-Host "[2/3] Updating Database Subnet Settings..." -ForegroundColor Yellow
$sqlQuery = "UPDATE SystemSettings SET LanSubnet = '192.168.0.0/24', AccessMode = 0 WHERE Id = 1"
sqlcmd -S .\SQLEXPRESS -d ClubMembership -Q "$sqlQuery"
Write-Host "✓ Database updated to trust 192.168.0.x network." -ForegroundColor Green

# 3. VERIFY IIS
Write-Host "[3/3] Verifying IIS Site Status..." -ForegroundColor Yellow
Import-Module WebAdministration
$site = Get-Website | Where-Object { $_.Bindings.bindingInformation -like "*:8080:*" }
if ($site) {
    Write-Host "✓ IIS Site found listening on port 8080: $($site.Name)" -ForegroundColor Green
    if ($site.State -ne "Started") {
        Start-Website -Name $site.Name
        Write-Host "✓ Started IIS Site." -ForegroundColor Green
    }
} else {
    Write-Host "X WARNING: No IIS site found on port 8080. Please check IIS Manager." -ForegroundColor Red
}

Write-Host "`n========================================================" -ForegroundColor Cyan
Write-Host "REPAIR COMPLETE" -ForegroundColor Green
Write-Host "========================================================"
Write-Host "Try this link on your LAPTOP:"
Write-Host "http://192.168.0.72:8080" -ForegroundColor White -BackgroundColor Blue
Write-Host "========================================================"
Pause
