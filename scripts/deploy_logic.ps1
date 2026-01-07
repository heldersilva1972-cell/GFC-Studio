$zipName = "PublishGFCWebApp.zip"
$targetUser = "hnsil"
$desktopPath = "C:\Users\$targetUser\Desktop\$zipName"

function Write-Step($msg, $color = "Cyan") {
    Write-Host ""
    Write-Host ">>> $msg" -ForegroundColor $color
}

try {
    # 1. Look for the zip
    Write-Step "Searching for $zipName on Desktop..."
    if (-not (Test-Path $desktopPath)) {
        # Fallback to current user desktop
        $desktopPath = Join-Path ([Environment]::GetFolderPath("Desktop")) $zipName
    }

    if (-not (Test-Path $desktopPath)) {
        Write-Host "[ERROR] Could not find $zipName at: $desktopPath" -ForegroundColor Red
        Write-Host "Please make sure the zip file is named exactly '$zipName' and is on your Desktop."
        Read-Host "Press Enter to exit"
        return
    }
    Write-Host "Found: $desktopPath" -ForegroundColor Green

    # 2. Setup paths
    $staging = "C:\inetpub\PublishGFCWebApp"
    $live = "C:\inetpub\GFCWebApp"
    $backupRoot = "C:\inetpub\history"
    $siteName = "GFCWebApp"
    $appPool = "GFCWebApp"

    # 3. Prepare Staging
    Write-Step "Cleaning staging area..."
    if (Test-Path $staging) { Remove-Item $staging -Recurse -Force | Out-Null }
    New-Item -ItemType Directory -Path $staging | Out-Null

    # 4. Unzip
    Write-Step "Unzipping files..."
    Expand-Archive -Path $desktopPath -DestinationPath $staging -Force

    # 5. IIS Management
    Write-Step "Stopping IIS Site and App Pool..."
    if (Get-Module -ListAvailable WebAdministration) {
        Import-Module WebAdministration
    }
    
    try { Stop-Website $siteName -ErrorAction SilentlyContinue } catch {}
    try { Stop-WebAppPool $appPool -ErrorAction SilentlyContinue } catch {}
    Start-Sleep -Seconds 2

    # 6. Backup
    Write-Step "Creating backup..."
    if (-not (Test-Path $backupRoot)) { New-Item -ItemType Directory -Path $backupRoot | Out-Null }
    $ts = Get-Date -Format "yyyyMMdd_HHmmss"
    $bp = Join-Path $backupRoot "Backup_$ts"
    if (Test-Path $live) {
        Copy-Item -Path "$live\*" -Destination $bp -Recurse -Force
        Write-Host "Backup saved to: $bp"
    }

    # 7. Copy Files
    Write-Step "Deploying new files..."
    $items = Get-ChildItem -Path $staging -Recurse
    foreach ($item in $items) {
        if ($item.Name -eq "appsettings.Production.json") { continue }
        $rel = $item.FullName.Substring($staging.Length).TrimStart("\")
        $dest = Join-Path $live $rel
        if ($item.PSIsContainer) {
            if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Path $dest | Out-Null }
        } else {
            if (-not (Test-Path (Split-Path $dest))) { New-Item -ItemType Directory -Path (Split-Path $dest) | Out-Null }
            Copy-Item -Path $item.FullName -Destination $dest -Force
        }
    }

    # 8. Restart
    Write-Step "Restarting IIS..." "Green"
    try { Start-WebAppPool $appPool -ErrorAction Stop } catch { Write-Host "Could not start App Pool: $_" -ForegroundColor Yellow }
    try { Start-Website $siteName -ErrorAction Stop } catch { Write-Host "Could not start Website: $_" -ForegroundColor Yellow }

    # 9. Cleanup
    Write-Step "Cleaning up Desktop zip..."
    Remove-Item $desktopPath -Force
    Remove-Item $staging -Recurse -Force

    Write-Host ""
    Write-Host "=========================================" -ForegroundColor Green
    Write-Host "       DEPLOYMENT SUCCESSFUL!" -ForegroundColor Green
    Write-Host "=========================================" -ForegroundColor Green
} catch {
    Write-Host ""
    Write-Host "!!! DEPLOYMENT FAILED !!!" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
}

Write-Host ""
Read-Host "Press Enter to exit"
