#requires -RunAsAdministrator
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

<#
setup_tunnel.ps1
- Windows Cloudflare Tunnel setup (idempotent)
- Writes config.yml safely using a here-string (no YAML parsed as PS)
#>

# =========================
# USER SETTINGS
# =========================
$TunnelName        = "gfc-webapp"
$PublicHostname    = "gfc.lovanow.com"

# Local service the tunnel should forward to:
# Example: Blazor/IIS reverse proxy -> localhost:5000, or whatever your app listens on.
$LocalServiceUrl   = "http://localhost:5000"

# Optional: if you want a local-only convenience hostname without typing full public domain
# Set $EnableLocalHostsEntry = $true to add to Windows hosts file:
$EnableLocalHostsEntry = $false
$LocalConvenienceHost  = "gfc.lanovanow.com"
$LocalConvenienceIP    = "127.0.0.1"

# =========================
# PATHS
# =========================
$CloudflaredExe = Join-Path $env:ProgramFiles "cloudflared\cloudflared.exe"
$CloudflaredDir = Split-Path $CloudflaredExe -Parent
$CfDataDir      = Join-Path $env:ProgramData "cloudflared"
$ConfigPath     = Join-Path $CfDataDir "config.yml"

# =========================
# HELPERS
# =========================
function Write-Section($msg) {
  Write-Host ""
  Write-Host "=== $msg ===" -ForegroundColor Cyan
}

function Ensure-Dir($path) {
  if (-not (Test-Path $path)) {
    New-Item -ItemType Directory -Force -Path $path | Out-Null
  }
}

function Download-Cloudflared {
  Write-Section "Installing/Updating cloudflared"
  Ensure-Dir $CloudflaredDir

  # Official Cloudflared Windows 64-bit download endpoint
  $url = "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe"
  $tmp = Join-Path $env:TEMP "cloudflared.exe"

  Write-Host "Downloading $url"
  Invoke-WebRequest -Uri $url -OutFile $tmp -UseBasicParsing

  Move-Item -Force $tmp $CloudflaredExe
  Write-Host "Installed: $CloudflaredExe"
}

function Cf {
  param([Parameter(ValueFromRemainingArguments=$true)] $Args)
  & $CloudflaredExe @Args
}

function Ensure-Cloudflared {
  if (-not (Test-Path $CloudflaredExe)) {
    Download-Cloudflared
  } else {
    # Try running version; if broken, reinstall
    try { Cf "version" | Out-Null } catch { Download-Cloudflared }
  }
}

function Ensure-Login {
  Write-Section "Cloudflare login (one-time)"
  # If cert.pem exists, login already done
  $cert = Join-Path $env:USERPROFILE ".cloudflared\cert.pem"
  if (Test-Path $cert) {
    Write-Host "Already logged in (found $cert)"
    return
  }

  Write-Host "Opening browser to authorize Cloudflare account..."
  Cf "tunnel" "login" | Out-Host
}

function Get-TunnelIdByName($name) {
  # cloudflared tunnel list outputs a table; parse it for the UUID.
  $out = Cf "tunnel" "list" 2>$null | Out-String
  if (-not $out) { return $null }

  # Look for line containing the tunnel name and a UUID
  $lines = $out -split "`r?`n"
  foreach ($line in $lines) {
    if ($line -match "\b$name\b") {
      # UUID v4 pattern
      if ($line -match "([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})") {
        return $Matches[1]
      }
    }
  }
  return $null
}

function Ensure-Tunnel {
  param([string]$name)

  Write-Section "Ensure tunnel exists: $name"
  $id = Get-TunnelIdByName $name
  if ($id) {
    Write-Host "Tunnel exists: $name -> $id"
    return $id
  }

  Write-Host "Creating tunnel: $name"
  $createOut = Cf "tunnel" "create" $name | Out-String

  # Create output usually contains the UUID
  if ($createOut -match "([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})") {
    $id = $Matches[1]
    Write-Host "Created: $name -> $id"
    return $id
  }

  # Fallback: re-list
  $id = Get-TunnelIdByName $name
  if (-not $id) { throw "Could not determine Tunnel ID after create." }
  return $id
}

function Ensure-Config {
  param(
    [string]$tunnelId,
    [string]$hostname,
    [string]$localService
  )

  Write-Section "Writing config.yml"
  Ensure-Dir $CfDataDir

  $credsPath = Join-Path $CfDataDir "$tunnelId.json"
  if (-not (Test-Path $credsPath)) {
    # cloudflared writes creds to ProgramData during create/service install in many cases,
    # but to be safe we copy from default location if present.
    $defaultCreds = Join-Path $env:USERPROFILE ".cloudflared\$tunnelId.json"
    if (Test-Path $defaultCreds) {
      Copy-Item -Force $defaultCreds $credsPath
    }
  }

  if (-not (Test-Path $credsPath)) {
    throw "Tunnel credentials not found. Expected $credsPath (or $env:USERPROFILE\.cloudflared\$tunnelId.json). Re-run login/create."
  }

  # IMPORTANT: YAML is written as a HERE-STRING so PowerShell never parses '-' lines as operators.
  $yaml = @"
tunnel: $tunnelId
credentials-file: $credsPath

ingress:
  - hostname: $hostname
    service: $localService
  - service: http_status:404
"@

  $yaml | Set-Content -Path $ConfigPath -Encoding utf8
  Write-Host "Wrote: $ConfigPath"
}

function Ensure-DnsRoute {
  param([string]$tunnelId, [string]$hostname)
  Write-Section "Ensuring DNS route exists for $hostname"

  # This creates/updates the CNAME in Cloudflare to point hostname to the tunnel.
  # Idempotent-ish; if it already exists, cloudflared may say it exists.
  try {
    Cf "tunnel" "route" "dns" $tunnelId $hostname | Out-Host
  } catch {
    Write-Host "Route command returned error; checking if it already exists..." -ForegroundColor Yellow
    # Not all versions have a 'route dns list'. We'll just continue.
  }
}

function Ensure-Service {
  param([string]$tunnelId)
  Write-Section "Installing/Updating Windows service"

  # cloudflared service install reads config.yml in ProgramData
  # If service already exists, this may error; we'll handle restart.
  try {
    Cf "service" "install" | Out-Host
  } catch {
    Write-Host "Service install may already be present. Continuing..." -ForegroundColor Yellow
  }

  # Ensure service is set to Automatic and running
  $svcName = "cloudflared"
  $svc = Get-Service -Name $svcName -ErrorAction SilentlyContinue
  if (-not $svc) { throw "Service '$svcName' not found after install." }

  Set-Service -Name $svcName -StartupType Automatic

  if ($svc.Status -eq "Running") {
    Restart-Service -Name $svcName -Force
  } else {
    Start-Service -Name $svcName
  }

  Write-Host "Service '$svcName' is $((Get-Service $svcName).Status)"
}

function Ensure-HostsEntry {
  param([bool]$enable, [string]$hostName, [string]$ip)
  if (-not $enable) { return }

  Write-Section "Adding local hosts entry ($hostName -> $ip)"
  $hostsFile = Join-Path $env:SystemRoot "System32\drivers\etc\hosts"
  $line = "$ip`t$hostName"

  $content = Get-Content $hostsFile -ErrorAction Stop
  if ($content -match "^\s*$([regex]::Escape($ip))\s+$([regex]::Escape($hostName))\s*$") {
    Write-Host "Hosts entry already present."
    return
  }

  Add-Content -Path $hostsFile -Value $line
  Write-Host "Added: $line"
}

# =========================
# MAIN
# =========================
Write-Section "Validate & Prepare"
Ensure-Cloudflared
Ensure-Dir $CfDataDir

Ensure-Login

$tunnelId = Ensure-Tunnel -name $TunnelName

Ensure-Config -tunnelId $tunnelId -hostname $PublicHostname -localService $LocalServiceUrl
Ensure-DnsRoute -tunnelId $tunnelId -hostname $PublicHostname
Ensure-Service -tunnelId $tunnelId

Ensure-HostsEntry -enable $EnableLocalHostsEntry -hostName $LocalConvenienceHost -ip $LocalConvenienceIP

Write-Section "Done"
Write-Host "Public URL: https://$PublicHostname"
Write-Host "Tunnel Name: $TunnelName"
Write-Host "Tunnel ID:   $tunnelId"
Write-Host "Config:      $ConfigPath"
