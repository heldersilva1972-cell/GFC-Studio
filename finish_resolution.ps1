$ErrorActionPreference = "Stop"

Write-Host ">>> FINALIZING CONFLICT RESOLUTION..." -ForegroundColor Cyan

# 1. Force add all files, which tells Git "I have resolved the conflict by keeping these files"
Write-Host "1. Staging all files..."
git add .

# 2. Commit the resolution
Write-Host "2. Committing merge..."
try {
    git commit -m "Finalize conflict resolution: Kept Visual Editor V2 files"
    Write-Host "SUCCESS: Merge committed." -ForegroundColor Green
} catch {
    # If commit fails (e.g. nothing to commit), check if clean
    $status = git status --porcelain
    if (-not $status) {
        Write-Host "SUCCESS: Repository is already clean and committed." -ForegroundColor Green
    } else {
        Write-Host "ERROR: Could not commit. Please check git status output:" -ForegroundColor Red
        git status
        exit 1
    }
}

# 3. Verify status one last time
Write-Host "----------------------------------------"
git status
Write-Host "----------------------------------------"
Write-Host ">>> DONE. You are ready to work." -ForegroundColor Cyan
