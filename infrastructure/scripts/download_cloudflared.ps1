$ErrorActionPreference = "Stop"
$url = "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe"
$destDir = "..\tools\cloudflared"
$destFile = Join-Path $destDir "cloudflared.exe"

# Ensure directory exists (relative to script location)
$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath
$absoluteDestDir = Join-Path $scriptDir $destDir

if (!(Test-Path $absoluteDestDir)) {
    New-Item -ItemType Directory -Force -Path $absoluteDestDir
}

$absoluteDestFile = Join-Path $absoluteDestDir "cloudflared.exe"

Write-Host "Downloading cloudflared to $absoluteDestFile..."
Invoke-WebRequest -Uri $url -OutFile $absoluteDestFile
Write-Host "Download complete."
