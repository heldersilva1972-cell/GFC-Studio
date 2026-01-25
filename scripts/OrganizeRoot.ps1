# PowerShell Script to Organize GFC Project Root
# This script moves loose files into categorized subfolders to clean up the workspace.

$root = $PSScriptRoot
Write-Host "Organizing files in: $root" -ForegroundColor Cyan

# Define destination folders with explicit full paths
$destScripts = "$root\scripts\ops"
$destDocs = "$root\docs\project_info"
$destDb = "$root\database\maintenance"
$destSnippets = "$root\dev_scratchpad"
$destLogs = "$root\logs\archive"

# Create folders if they don't exist
$folders = @($destScripts, $destDocs, $destDb, $destSnippets, $destLogs)
foreach ($folder in $folders) {
    if (-not (Test-Path -Path $folder -PathType Container)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
        Write-Host "Created directory: $folder" -ForegroundColor Green
    }
}

# Define file categories (Extensions and patterns)
$scriptExtensions = @(".ps1", ".bat")
$docExtensions = @(".txt", ".docx", ".xlsx", ".pdf") 
$dbExtensions = @(".sql", ".bak")
$snippetExtensions = @(".cs", ".razor")
$logExtensions = @(".log")

# Files to specific EXCLUDE (Critical root files)
$excludes = @(
    "OrganizeRoot.ps1",
    "README.md",
    "package.json",
    "package-lock.json",
    ".gitignore",
    ".gitattributes",
    "RemoveBartenderShiftPage.ps1"
)

# Function to move files safely
function Move-FileSafe {
    param (
        [string]$FilePath,
        [string]$Destination
    )
    
    if ([string]::IsNullOrWhiteSpace($Destination)) {
        Write-Host "Skipping move for $FilePath - Destination is empty/null" -ForegroundColor Red
        return
    }

    $fileName = Split-Path $FilePath -Leaf
    $destPath = Join-Path $Destination $fileName
    
    # Handle duplicates by appending timestamp
    if (Test-Path $destPath) {
        $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
        $nameOnly = [System.IO.Path]::GetFileNameWithoutExtension($fileName)
        $ext = [System.IO.Path]::GetExtension($fileName)
        $newName = "${nameOnly}_${timestamp}${ext}"
        $destPath = Join-Path $Destination $newName
        Write-Host "Duplicate found. Renaming to: $newName" -ForegroundColor Yellow
    }
    
    Move-Item -Path $FilePath -Destination $destPath -Force
    Write-Host "Moved: $fileName -> $Destination" -ForegroundColor Gray
}

# Get all files in the root folder (not recursive)
$files = Get-ChildItem -Path $root -File

foreach ($file in $files) {
    # Skip excluded files
    if ($excludes -contains $file.Name) {
        continue
    }
    # Skip hidden files/dotfiles (like .env if exists, though usually specific)
    if ($file.Name.StartsWith(".")) {
        continue
    }

    $ext = $file.Extension.ToLower()
    $targetDir = $null

    # Determine target directory based on extension
    if ($scriptExtensions -contains $ext) {
        $targetDir = $destScripts
    }
    elseif ($dbExtensions -contains $ext) {
        $targetDir = $destDb
    }
    elseif ($snippetExtensions -contains $ext) {
        $targetDir = $destSnippets
    }
    elseif ($logExtensions -contains $ext) {
        $targetDir = $destLogs
    }
    elseif ($docExtensions -contains $ext -or ($ext -eq ".md" -and $file.Name -ne "README.md")) {
        $targetDir = $destDocs
    }

    # Only move if a valid target directory was assigned
    if (-not [string]::IsNullOrWhiteSpace($targetDir)) {
        Move-FileSafe -FilePath $file.FullName -Destination $targetDir
    }
}

Write-Host "Organization Complete!" -ForegroundColor Cyan
