# DocsOrganization.ps1
# Further refines the organization of the docs and root directory.

$DocsRoot = "docs"
$GuidesDir = Join-Path $DocsRoot "Guides"
$RefDir = Join-Path $DocsRoot "Reference"
$CleanupDir = "scripts\Cleanup"

# 1. Create target directories if they don't exist
foreach ($dir in @($GuidesDir, $RefDir, $CleanupDir)) {
    if (!(Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
        Write-Host "Created folder: $dir" -ForegroundColor Cyan
    }
}

# 2. Organize loose files in docs/
$GuidesFiles = @(
    "CAMERA_DISCOVERY_GUIDE.md",
    "CAMERA_SETUP_GUIDE.md",
    "REMOTE_ACCESS_SETUP.md",
    "TROUBLESHOOTING.md",
    "VIDEO_AGENT_SETUP.md",
    "QUICK_START_WEBSITE.md"
)

$RefFiles = @(
    "GFC_MASTER_OPERATIONAL_SPEC.md",
    "GFC_STUDIO_LIVING_SPEC.md",
    "IMPLEMENTATION_GUIDELINES.md",
    "DOC_MANAGEMENT_PROTOCOL.md"
)

foreach ($f in $GuidesFiles) {
    $src = Join-Path $DocsRoot $f
    if (Test-Path $src) {
        Write-Host "Moving Guide: $f"
        Move-Item $src $GuidesDir -Force
    }
}

foreach ($f in $RefFiles) {
    $src = Join-Path $DocsRoot $f
    if (Test-Path $src) {
        Write-Host "Moving Reference: $f"
        Move-Item $src $RefDir -Force
    }
}

# Move the big report to history logs
$masterReport = Join-Path $DocsRoot "01_GFC_WEB_APP_v1.0.0_R4_TRUE_FULL_MASTER_REPORT.txt"
if (Test-Path $masterReport) {
    Write-Host "Moving Master Report to history logs"
    Move-Item $masterReport "docs\History_Logs" -Force
}

# 3. Handle root files
# Move Agent rules to Reference
foreach ($f in @("AGENTS.md", "AGENT_WORKFLOW_RULES.md")) {
    if (Test-Path $f) {
        Write-Host "Moving $f to Reference"
        Move-Item $f $RefDir -Force
    }
}

# Move all cleanup/management scripts to scripts/Cleanup
Get-ChildItem -Path . -Filter "*.ps1" | Where-Object { 
    $_.Name -match "Cleanup" -or $_.Name -match "Docs" 
} | ForEach-Object {
    if ($_.Name -ne "DocsOrganization.ps1") {
        Write-Host "Moving Script: $($_.Name)"
        Move-Item $_.FullName $CleanupDir -Force
    }
}

Write-Host "`nDeep organization complete!" -ForegroundColor Green
