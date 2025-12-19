# GFC Studio + Web App Merge Verification Scan (v1.0.0 R1)
# Runs from repo root. Outputs a single folder you can zip and upload.
# Usage:
#   powershell -ExecutionPolicy Bypass -File .\GFC_StudioMerge_Scan.ps1 -OutDir .\_DIAGNOSTICS\StudioMergeScan

param(
  [string]$OutDir = ".\_DIAGNOSTICS\StudioMergeScan"
)

$ErrorActionPreference = "Stop"

function Write-File($path, $content) {
  $dir = Split-Path -Parent $path
  if ($dir -and !(Test-Path $dir)) { New-Item -ItemType Directory -Force -Path $dir | Out-Null }
  $content | Out-File -FilePath $path -Encoding UTF8
}

New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

# 0) Root snapshot
$root = Get-Location
Write-File (Join-Path $OutDir "00_ROOT.txt") ("Root: " + $root.Path + "`r`nUTC: " + (Get-Date).ToUniversalTime().ToString("s") + "Z")

# 1) Solutions / projects / node packages
Get-ChildItem -Recurse -Filter *.sln -File | Select-Object FullName | Sort-Object FullName |
  ForEach-Object { $_.FullName } | Out-File (Join-Path $OutDir "01_SOLUTIONS.txt") -Encoding UTF8

Get-ChildItem -Recurse -Filter *.csproj -File | Select-Object FullName | Sort-Object FullName |
  ForEach-Object { $_.FullName } | Out-File (Join-Path $OutDir "02_CSPROJ.txt") -Encoding UTF8

Get-ChildItem -Recurse -Filter package.json -File | Select-Object FullName | Sort-Object FullName |
  ForEach-Object { $_.FullName } | Out-File (Join-Path $OutDir "03_PACKAGE_JSON.txt") -Encoding UTF8

# 2) Fast term hits (Studio merge indicators)
$terms = @(
  "studio", "web studio", "animation studio", "animation playground",
  "GFC.WebStudio", "Studio", "StudioRoute", "StudioEditor",
  "hall rentals", "HallRental", "WebsiteEditor", "PageBuilder",
  "Preview", "DevicePreview", "Draft", "Publish"
)

$codeExt = @("*.cs","*.razor","*.ts","*.tsx","*.js","*.jsx","*.json","*.md","*.sql")
$termHits = New-Object System.Collections.Generic.List[object]

foreach ($ext in $codeExt) {
  Get-ChildItem -Recurse -File -Filter $ext -ErrorAction SilentlyContinue | ForEach-Object {
    $file = $_.FullName
    try {
      $text = Get-Content -Raw -LiteralPath $file -ErrorAction Stop
    } catch { return }
    foreach ($t in $terms) {
      if ($text -match [regex]::Escape($t)) {
        $termHits.Add([pscustomobject]@{ Term=$t; File=$file })
      }
    }
  }
}

$termHits | Sort-Object Term, File | Format-Table -AutoSize | Out-String |
  Out-File (Join-Path $OutDir "10_TERM_HITS.txt") -Encoding UTF8

# 3) Blazor/ASP.NET route hints (if any)
Get-ChildItem -Recurse -File -Filter *.razor -ErrorAction SilentlyContinue |
  ForEach-Object {
    $file=$_.FullName
    $lines = Select-String -LiteralPath $file -Pattern '@page\s+' -SimpleMatch -ErrorAction SilentlyContinue
    if ($lines) { $lines | ForEach-Object { "{0}:{1}" -f $file, $_.Line.Trim() } }
  } | Out-File (Join-Path $OutDir "20_BLAZOR_ROUTES.txt") -Encoding UTF8

# 4) Key configs (copy only; do not alter)
$cfg = Get-ChildItem -Recurse -File -Include appsettings.json,appsettings.Development.json,launchSettings.json -ErrorAction SilentlyContinue
$cfg | Select-Object FullName | ForEach-Object { $_.FullName } | Out-File (Join-Path $OutDir "30_CONFIG_FILES.txt") -Encoding UTF8
foreach ($c in $cfg) {
  $safe = ($c.FullName -replace "[:\\\/]","_")
  Copy-Item -LiteralPath $c.FullName -Destination (Join-Path $OutDir ("CFG_" + $safe + ".txt")) -Force -ErrorAction SilentlyContinue
}

# 5) Directory inventory (helps confirm what exists)
Get-ChildItem -Directory -Recurse | Select-Object FullName | Sort-Object FullName |
  ForEach-Object { $_.FullName } | Out-File (Join-Path $OutDir "40_DIRS.txt") -Encoding UTF8

# 6) Git status (if repo)
try {
  $git = Get-Command git -ErrorAction Stop
  (& git status --porcelain=v1) 2>$null | Out-File (Join-Path $OutDir "50_GIT_STATUS.txt") -Encoding UTF8
  (& git rev-parse HEAD) 2>$null | Out-File (Join-Path $OutDir "51_GIT_HEAD.txt") -Encoding UTF8
} catch {
  Write-File (Join-Path $OutDir "50_GIT_STATUS.txt") "git not available or not a git repo."
}

Write-Host "DONE. Output: $OutDir"
