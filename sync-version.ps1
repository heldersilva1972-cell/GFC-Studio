<#
.SYNOPSIS
    Syncs version numbers across the GFC Mobile and Webapp projects.
    
    Usage:
    ./sync-version.ps1 -Version "2.4.26"
    ./sync-version.ps1 -Next
    ./sync-version.ps1 -Next -DryRun
#>

param (
    [Parameter(Mandatory=$false)]
    [ValidateSet("Mobile", "POS", "WebApp")]
    [string]$Project = "Mobile",

    [string]$Version,
    [switch]$Next,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

# Base Paths
$basePath = $PSScriptRoot

# --- MOBILE PATHS ---
$mobileVersionJson = Join-Path $basePath "apps/GFC-Mobile-Standalone/version.json"
$mobileSW = Join-Path $basePath "apps/GFC-Mobile-Standalone/GFC.Mobile/wwwroot/service-worker.js"
$mobileSWPub = Join-Path $basePath "apps/GFC-Mobile-Standalone/GFC.Mobile/wwwroot/service-worker.published.js"
$webappSettings = Join-Path $basePath "apps/webapp/GFC.BlazorServer/appsettings.json"

# --- POS PATHS ---
$posVersionService = Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/Services/PosVersionService.cs"
$posCsproj = Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Mobile/GFC.Pos.Mobile.csproj"
$posSWFiles = @(
    (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/wwwroot/service-worker.js"),
    (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/wwwroot/service-worker.published.js"),
    (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Terminal/wwwroot/service-worker.js"),
    (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Terminal/wwwroot/service-worker.published.js")
)
$posProps = Join-Path $basePath "apps/GFC-Pos-Standalone/PosVersion.props"
$posVersionTxtFiles = @(
    (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Terminal/wwwroot/version.txt"),
    (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/wwwroot/version.txt")
)

function Write-Step ([string]$msg) { Write-Host "[WAIT] $msg" -ForegroundColor Cyan }
function Write-Success ([string]$msg) { Write-Host "[OK]   $msg" -ForegroundColor Green }
function Write-DryRun ([string]$msg) { Write-Host "[DRY]  $msg" -ForegroundColor Yellow }

try {
    $currentVersion = ""
    
    # 1. GET CURRENT VERSION
    if ($Project -eq "Mobile") {
        if (-not (Test-Path $mobileVersionJson)) { throw "Missing $mobileVersionJson" }
        $vJson = Get-Content $mobileVersionJson | ConvertFrom-Json
        $currentVersion = $vJson.version
    } elseif ($Project -eq "WebApp") {
        if (-not (Test-Path $webappSettings)) { throw "Missing $webappSettings" }
        $s = Get-Content $webappSettings | ConvertFrom-Json
        $currentVersion = $s.ApplicationVersion.Revision
    } else {
        if (-not (Test-Path $posProps)) { throw "Missing $posProps" }
        $content = Get-Content $posProps -Raw
        if ($content -match '<PosVersion>(.*)</PosVersion>') { $currentVersion = $Matches[1] }
        elseif ($content -match '<PosBuild>(.*)</PosBuild>') { $currentVersion = $Matches[1] }
    }

    # 2. DETERMINE TARGET VERSION
    $targetVersion = $Version
    if ($Next) {
        $parts = $currentVersion.Split('.')
        if ($parts.Length -ge 2) {
            $lastIdx = $parts.Length - 1
            $last = [int]$parts[$lastIdx] + 1
            $parts[$lastIdx] = $last
            $targetVersion = $parts -join "."
        }
    }

    if (-not $targetVersion) { throw "Specify -Version or use -Next" }
    Write-Host "Syncing $Project to Version: $targetVersion" -ForegroundColor Magenta

    # 3. APPLY UPDATES
    if ($Project -eq "Mobile") {
        # version.json
        Write-Step "Updating version.json..."
        if ($DryRun) { Write-DryRun "Would set version.json to $targetVersion" }
        else {
            $vJson = Get-Content $mobileVersionJson | ConvertFrom-Json
            $vJson.version = $targetVersion
            $vJson.build = $targetVersion
            $vJson.description = "GFC Mobile Revision: $targetVersion (Sync)"
            $vJson | ConvertTo-Json | Set-Content $mobileVersionJson
        }

        # Service Workers
        foreach ($f in @($mobileSW, $mobileSWPub)) {
            if (Test-Path $f) {
                Write-Step "Updating $(Split-Path $f -Leaf)..."
                $c = Get-Content $f
                $nc = $c -replace "// GFC Mobile Revision: .*", "// GFC Mobile Revision: $targetVersion"
                if ($DryRun) { Write-DryRun "Would update $f" } else { Set-Content $f $nc }
            }
        }

        # Webapp Settings
        Write-Step "Updating appsettings.json..."
        if (Test-Path $webappSettings) {
            $s = Get-Content $webappSettings | ConvertFrom-Json
            $oldRev = $s.ApplicationVersion.Revision
            $revParts = $oldRev.Split('.')
            if ($revParts.Length -ge 2) { $revParts[$revParts.Length-1] = [int]$revParts[$revParts.Length-1] + 1 }
            $newRev = $revParts -join "."
            
            if ($DryRun) { Write-DryRun "Would update server: MobileRev=$targetVersion, Rev=$newRev" }
            else {
                $s.ApplicationVersion.MobileRevision = $targetVersion
                $s.ApplicationVersion.Revision = $newRev
                $s | ConvertTo-Json -Depth 20 | Set-Content $webappSettings
            }
        }

        # Mobile Client Settings (wwwroot)
        $mobileClientSettings = Join-Path $basePath "apps/GFC-Mobile-Standalone/GFC.Mobile/wwwroot/appsettings.json"
        if (Test-Path $mobileClientSettings) {
            Write-Step "Updating Mobile Client appsettings.json..."
            if ($DryRun) { Write-DryRun "Would update client: MobileRev=$targetVersion" }
            else {
                $s = Get-Content $mobileClientSettings | ConvertFrom-Json
                $s.ApplicationVersion.MobileRevision = $targetVersion
                $s | ConvertTo-Json | Set-Content $mobileClientSettings
            }
        }

        # index.html (Blazor Boot Cache Buster)
        $mobileIndex = Join-Path $basePath "apps/GFC-Mobile-Standalone/GFC.Mobile/wwwroot/index.html"
        if (Test-Path $mobileIndex) {
            Write-Step "Updating index.html (Cache Buster)..."
            $c = Get-Content $mobileIndex -Raw
            $nc = $c -replace "(const version = ')[^']+", ("`${1}" + $targetVersion)
            if ($DryRun) { Write-DryRun "Would update $mobileIndex" } else { Set-Content $mobileIndex $nc }
        }
    } 
    elseif ($Project -eq "WebApp") {
        Write-Step "Updating appsettings.json (WebApp Revision)..."
        if (Test-Path $webappSettings) {
            if ($DryRun) { Write-DryRun "Would set server revision to $targetVersion" }
            else {
                $s = Get-Content $webappSettings | ConvertFrom-Json
                $s.ApplicationVersion.Revision = $targetVersion
                $s | ConvertTo-Json -Depth 20 | Set-Content $webappSettings
            }
        }
    }
    else { # POS PROJECT
        # PosVersionService.cs
        Write-Step "Updating PosVersionService.cs..."
        $c = Get-Content $posVersionService -Raw
        $nc = $c -replace '(GetRevision\(\)\s*=>\s*")[^"]+(")', ('${1}' + $targetVersion + '${2}')
        if ($nc -eq $c) { $nc = $c -replace '(return\s*")[^"]+(";)', ('${1}' + $targetVersion + '${2}') }
        if ($DryRun) { Write-DryRun "Would update $posVersionService" } else { Set-Content $posVersionService $nc }

        # PosVersion.props
        if (Test-Path $posProps) {
            Write-Step "Updating PosVersion.props..."
            $c = Get-Content $posProps -Raw
            $nc = $c -replace '(<PosVersion>)[^<]+(</PosVersion>)', ('${1}' + $targetVersion + '${2}')
            if ($DryRun) { Write-DryRun "Would update $posProps" } else { Set-Content $posProps $nc }
        }

        # Service Workers
        foreach ($f in $posSWFiles) {
            if (Test-Path $f) {
                Write-Step "Updating $(Split-Path $f -Parent | Split-Path -Leaf)/$(Split-Path $f -Leaf)..."
                $c = Get-Content $f -Raw
                $nc = $c -replace "// GFC POS Revision: .*", "// GFC POS Revision: $targetVersion"
                # [FIX] Also update the cacheName for production updates
                $nc = $nc -replace '(\$\{cacheNamePrefix\})[^`'']+', ("`${1}" + $targetVersion)
                if ($DryRun) { Write-DryRun "Would update $f" } else { Set-Content $f $nc }
            }
        }

        # index.html (Blazor Boot Cache Buster) - BOTH LOCATIONS
        $posIndices = @(
            (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Terminal/wwwroot/index.html"),
            (Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/wwwroot/index.html")
        )
        foreach ($f in $posIndices) {
            if (Test-Path $f) {
                Write-Step "Updating $(Split-Path $f -Parent | Split-Path -Leaf)/$(Split-Path $f -Leaf) (Cache Buster)..."
                $c = Get-Content $f -Raw
                $nc = $c -replace "(const version = ')[^']+", ("`${1}" + $targetVersion)
                # [FIX] Also update CSS cache-busters so UI changes are forced
                $nc = $nc -replace '(\.css\?v=)[^"]+', ("`${1}" + $targetVersion)
                if ($DryRun) { Write-DryRun "Would update $f" } else { Set-Content $f $nc }
            }
        }

        # version.txt (Primary Update Authority)
        foreach ($f in $posVersionTxtFiles) {
            if (Test-Path $f) {
                Write-Step "Updating $(Split-Path $f -Parent | Split-Path -Leaf)/$(Split-Path $f -Leaf)..."
                if ($DryRun) { Write-DryRun "Would set $f to $targetVersion" } 
                else { Set-Content $f $targetVersion }
            }
        }

        # CSPROJ
        Write-Step "Updating GFC.Pos.Mobile.csproj..."
        $c = Get-Content $posCsproj
        $appVer = $targetVersion.Replace(".", "")
        $nc = $c -replace '<ApplicationDisplayVersion>.*</ApplicationDisplayVersion>', "<ApplicationDisplayVersion>$targetVersion</ApplicationDisplayVersion>"
        $nc = $nc -replace '<ApplicationVersion>.*</ApplicationVersion>', "<ApplicationVersion>$appVer</ApplicationVersion>"
        if ($DryRun) { Write-DryRun "Would update $posCsproj (AppVersion: $appVer)" } else { Set-Content $posCsproj $nc }

        # Webapp Settings (POS Revision Track)
        Write-Step "Updating appsettings.json (PosRevision)..."
        if (Test-Path $webappSettings) {
            $s = Get-Content $webappSettings | ConvertFrom-Json
            if ($DryRun) { Write-DryRun "Would update server: PosRevision=$targetVersion" }
            else {
                $s.ApplicationVersion.PosRevision = $targetVersion
                $s | ConvertTo-Json -Depth 20 | Set-Content $webappSettings
            }
        }
    }

    # --- REMOTE DATABASE SYNC (INSTANT UPDATES) ---
    Write-Step "Syncing version to Remote Database..."
    $serverUrl = "https://gfc.lovanow.com"  # Update this if your production URL changes
    $apiKey = "GFC_SYNC_V2_SECRET_2026"
    $syncUrl = "$serverUrl/api/mobile-reporting/sync-version?project=$Project&version=$targetVersion&apiKey=$apiKey"
    
    try {
        if ($DryRun) {
            Write-DryRun "Would call API: $syncUrl"
        } else {
            $response = Invoke-RestMethod -Uri $syncUrl -Method Post
            Write-Host "Remote Sync Successful: $($response.Message)" -ForegroundColor Gray
        }
    } catch {
        Write-Warning "Remote Database Sync failed. This is expected if the site is not yet deployed or is offline."
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    }

    Write-Host "`nDONE! $Project is now on $targetVersion" -ForegroundColor Green

} catch {
    Write-Host "`nERROR: $($_.Exception.Message)" -ForegroundColor Red
}
