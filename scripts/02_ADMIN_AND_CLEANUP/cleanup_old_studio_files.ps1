# ============================================================================
# Delete Remaining Incompatible Studio Files
# ============================================================================
# These files are part of the old Studio implementation and are no longer needed
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Cleaning Up Old Studio Files" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"

# Delete StudioService.cs
$studioService = "$baseDir\Services\StudioService.cs"
if (Test-Path $studioService) {
    Remove-Item $studioService -Force
    Write-Host "✓ Deleted StudioService.cs" -ForegroundColor Green
} else {
    Write-Host "ℹ StudioService.cs not found" -ForegroundColor Gray
}

# Delete IStudioService.cs
$iStudioService = "$baseDir\Services\IStudioService.cs"
if (Test-Path $iStudioService) {
    Remove-Item $iStudioService -Force
    Write-Host "✓ Deleted IStudioService.cs" -ForegroundColor Green
} else {
    Write-Host "ℹ IStudioService.cs not found" -ForegroundColor Gray
}

# Delete TemplateService.cs
$templateService = "$baseDir\Services\TemplateService.cs"
if (Test-Path $templateService) {
    Remove-Item $templateService -Force
    Write-Host "✓ Deleted TemplateService.cs" -ForegroundColor Green
} else {
    Write-Host "ℹ TemplateService.cs not found" -ForegroundColor Gray
}

# Delete ITemplateService.cs
$iTemplateService = "$baseDir\Services\ITemplateService.cs"
if (Test-Path $iTemplateService) {
    Remove-Item $iTemplateService -Force
    Write-Host "✓ Deleted ITemplateService.cs" -ForegroundColor Green
} else {
    Write-Host "ℹ ITemplateService.cs not found" -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Cleanup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "All old Studio files have been removed." -ForegroundColor Green
Write-Host "The new Studio implementation is in:" -ForegroundColor Cyan
Write-Host "  /Components/Pages/Admin/Studio/" -ForegroundColor Cyan
Write-Host ""
Write-Host "All compilation errors should now be resolved!" -ForegroundColor Green
