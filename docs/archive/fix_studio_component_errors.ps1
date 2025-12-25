# ============================================================================
# Fix Studio Component Errors - Choose Your Option
# ============================================================================
# You have 64 errors because the Studio components expect a different
# StudioSection model than what exists in the database.
#
# OPTION 1: Delete the incompatible Studio components (RECOMMENDED)
# OPTION 2: Update StudioSection model to match the components
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Studio Component Error Fix Options" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "You have 64 compilation errors because the Studio components" -ForegroundColor Yellow
Write-Host "expect properties that don't exist in the StudioSection model." -ForegroundColor Yellow
Write-Host ""
Write-Host "Missing properties:" -ForegroundColor Red
Write-Host "  - Content" -ForegroundColor Red
Write-Host "  - AnimationSettings" -ForegroundColor Red
Write-Host "  - Title" -ForegroundColor Red
Write-Host "  - ClientId" -ForegroundColor Red
Write-Host "  - PageIndex" -ForegroundColor Red
Write-Host ""
Write-Host "Choose an option:" -ForegroundColor Cyan
Write-Host "  [1] Delete incompatible Studio components (RECOMMENDED)" -ForegroundColor Green
Write-Host "  [2] Update StudioSection model to match components" -ForegroundColor Yellow
Write-Host "  [3] Cancel" -ForegroundColor Gray
Write-Host ""

$choice = Read-Host "Enter your choice (1, 2, or 3)"

switch ($choice) {
    "1" {
        Write-Host ""
        Write-Host "Deleting incompatible Studio components..." -ForegroundColor Yellow
        
        $baseDir = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"
        
        # Delete old Studio.razor
        $studioFile = "$baseDir\Components\Pages\Studio.razor"
        if (Test-Path $studioFile) {
            Remove-Item $studioFile -Force
            Write-Host "  ✓ Deleted Studio.razor" -ForegroundColor Green
        }
        
        # Delete StudioWizard.razor
        $wizardFile = "$baseDir\Components\Shared\StudioWizard.razor"
        if (Test-Path $wizardFile) {
            Remove-Item $wizardFile -Force
            Write-Host "  ✓ Deleted StudioWizard.razor" -ForegroundColor Green
        }
        
        Write-Host ""
        Write-Host "✓ Incompatible components removed!" -ForegroundColor Green
        Write-Host "  The new Studio components in /Admin/Studio/ are the correct ones." -ForegroundColor Cyan
    }
    
    "2" {
        Write-Host ""
        Write-Host "This option is NOT recommended because:" -ForegroundColor Yellow
        Write-Host "  - It will require database migration" -ForegroundColor Yellow
        Write-Host "  - The old Studio components may have other issues" -ForegroundColor Yellow
        Write-Host "  - The new Studio components in /Admin/Studio/ are better" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Please use Option 1 instead." -ForegroundColor Red
    }
    
    "3" {
        Write-Host ""
        Write-Host "Operation cancelled." -ForegroundColor Gray
    }
    
    default {
        Write-Host ""
        Write-Host "Invalid choice. Operation cancelled." -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
