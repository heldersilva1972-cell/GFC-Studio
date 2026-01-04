<#
.SYNOPSIS
    GFC Private Network Hardening & Lockdown Script
    Enforces VPN-only access to the web application.

.DESCRIPTION
    1. Configures Windows Firewall to allow WireGuard traffic.
    2. Restricts HTTP (80) and HTTPS (443) to the VPN subnet ONLY.
    3. Blocks generic web access from LAN or Public internet.

.NOTES
    Run as Administrator.
#>

param(
    [string]$VpnSubnet = "10.20.0.0/24",
    [int]$WgPort = 51820
)

Write-Host "--- GFC HARDENING & LOCKDOWN ---" -ForegroundColor Cyan

# 1. Ensure WireGuard Port is Open (UDP) from ANY
Write-Host "[1/3] Configuring WireGuard Firewall Rule..." -ForegroundColor Yellow
$wgRuleName = "GFC-WireGuard-Inbound"
if (Get-NetFirewallRule -Name $wgRuleName -ErrorAction SilentlyContinue) {
    Remove-NetFirewallRule -Name $wgRuleName
}
New-NetFirewallRule -DisplayName "GFC WireGuard Inbound (UDP-51820)" `
    -Name $wgRuleName `
    -Direction Inbound `
    -Protocol UDP `
    -LocalPort $WgPort `
    -Action Allow `
    -Description "Allows encrypted VPN tunnels from any source."

# 2. Restrict Web Traffic to VPN Subnet
Write-Host "[2/3] Hardening Web Traffic (Port 80/443)..." -ForegroundColor Yellow
$webRuleName = "GFC-Secure-Web-Inbound"

# First, disable existing generic World Wide Web rules to prevent conflicts
Write-Host "     Disabling default 'World Wide Web Services' rules..." -ForegroundColor Gray
Get-NetFirewallRule -DisplayGroup "World Wide Web Services (HTTP Traffic-In)" -ErrorAction SilentlyContinue | Disable-NetFirewallRule
Get-NetFirewallRule -DisplayGroup "World Wide Web Services (HTTPS Traffic-In)" -ErrorAction SilentlyContinue | Disable-NetFirewallRule

if (Get-NetFirewallRule -Name $webRuleName -ErrorAction SilentlyContinue) {
    Remove-NetFirewallRule -Name $webRuleName
}

New-NetFirewallRule -DisplayName "GFC Secure Access (VPN Only)" `
    -Name $webRuleName `
    -Direction Inbound `
    -Protocol TCP `
    -LocalPort 80, 443 `
    -RemoteAddress $VpnSubnet `
    -Action Allow `
    -Description "Restricts app access to authenticated VPN users only."

# 3. Verify and Finish
Write-Host "[3/3] Verifying Configuration..." -ForegroundColor Yellow
$rules = Get-NetFirewallRule | Where-Object { $_.Name -match "GFC" }

foreach ($rule in $rules) {
    $filter = Get-NetFirewallAddressFilter -AssociatedNetFirewallRule $rule
    Write-Host "     Rule: $($rule.DisplayName) | Action: $($rule.Action) | Remote: $($filter.RemoteAddress)" -ForegroundColor Green
}

Write-Host "`nSUCCESS: Firewall Lockdown Active." -ForegroundColor White
Write-Host "NOTE: Access from LAN or Public IP is now blocked. Use WireGuard to connect." -ForegroundColor Cyan
