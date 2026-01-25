# Organize Markdown Files - Fixed Path Version
$root = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2"
$docsRoot = Join-Path $root "docs"
$mdDest = Join-Path $docsRoot "markdown_guides"

Write-Host "Organizing Markdown from: $docsRoot" -ForegroundColor Cyan
Write-Host "Target: $mdDest" -ForegroundColor Cyan

# ensure destination exists
if (-not (Test-Path $mdDest)) {
    New-Item -ItemType Directory -Path $mdDest -Force | Out-Null
    Write-Host "Created directory: $mdDest" -ForegroundColor Green
}

# 1. Move root-level .md files FROM docs/ folder
# We filter for files only to avoid trying to move directories
$files = Get-ChildItem -Path $docsRoot -File -Filter "*.md"
foreach ($file in $files) {
    if ($file.Name -ne "README.md") { 
        $dest = Join-Path $mdDest $file.Name
        Move-Item -Path $file.FullName -Destination $dest -Force
        Write-Host "Moved: $($file.Name) -> markdown_guides" -ForegroundColor Gray
    }
}

# 2. Rescue/Move files from docs/project_info if it exists
$projectInfo = Join-Path $docsRoot "project_info"
if (Test-Path $projectInfo) {
    Write-Host "Checking project_info folder..." -ForegroundColor Yellow
    $infoFiles = Get-ChildItem -Path $projectInfo -File
    foreach ($file in $infoFiles) {
        if ($file.Extension -eq ".md") {
             $dest = Join-Path $mdDest $file.Name
             Move-Item -Path $file.FullName -Destination $dest -Force
             Write-Host "Moved: $($file.Name) from project_info" -ForegroundColor Gray
        }
    }
} else {
    Write-Host "project_info folder NOT found." -ForegroundColor DarkGray
}

Write-Host "Markdown organization complete." -ForegroundColor Green
