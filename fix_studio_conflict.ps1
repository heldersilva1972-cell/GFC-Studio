$ErrorActionPreference = "Stop"

# Define the conflicting path
$targetPath = Join-Path (Get-Location) "apps\website\app\studio-preview\[pageId]"

Write-Host "Checking for conflicting directory at: $targetPath"

if (Test-Path -LiteralPath $targetPath) {
    Write-Host "Found conflicting '[pageId]' directory."
    
    # Attempt to delete
    try {
        Remove-Item -LiteralPath $targetPath -Recurse -Force
        Write-Host "SUCCESS: Conflicting directory has been removed."
    } catch {
        Write-Warning "COULD NOT DELETE DIRECTORY."
        Write-Warning "The files are likely locked by your running Next.js server."
        Write-Warning "ACTION REQUIRED: Please stop your local server (Ctrl+C in your terminal) and run this script again."
        Write-Error $_
    }
} else {
    Write-Host "Directory not found. It might have already been deleted."
}

# Clean up the temporary repair scripts I created earlier
$tempFiles = @(
    "delete_conflict.js",
    "delete_conflict_v2.js",
    "rename_conflict.js",
    "rename_file.js",
    "bypass_lock.js",
    "force_delete.js",
    "disable_page.js",
    "disable_slug.js",
    "unlock_slug.js",
    "directory_swap.js",
    "delete_slug.js"
)

foreach ($file in $tempFiles) {
    if (Test-Path $file) {
        Remove-Item $file -Force
    }
}
