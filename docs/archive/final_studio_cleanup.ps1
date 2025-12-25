# ============================================================================
# Final Studio Cleanup - Remove All Remaining Old Studio Files
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Final Studio Cleanup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"

# Delete ContentController.cs (uses old IStudioService)
$contentController = "$baseDir\Controllers\ContentController.cs"
if (Test-Path $contentController) {
    Remove-Item $contentController -Force
    Write-Host "✓ Deleted ContentController.cs" -ForegroundColor Green
} else {
    Write-Host "ℹ ContentController.cs not found" -ForegroundColor Gray
}

# Delete StudioIndex.razor (uses old IStudioService)
$studioIndex = "$baseDir\Components\Pages\StudioIndex.razor"
if (Test-Path $studioIndex) {
    Remove-Item $studioIndex -Force
    Write-Host "✓ Deleted StudioIndex.razor" -ForegroundColor Green
} else {
    Write-Host "ℹ StudioIndex.razor not found" -ForegroundColor Gray
}

# Delete TopBar component (uses old IStudioService)
$topBarDir = "$baseDir\Components\Pages\Admin\Studio\TopBar"
if (Test-Path $topBarDir) {
    Remove-Item $topBarDir -Recurse -Force
    Write-Host "✓ Deleted TopBar directory" -ForegroundColor Green
} else {
    Write-Host "ℹ TopBar directory not found" -ForegroundColor Gray
}

# Delete LeftPanel component
$leftPanelDir = "$baseDir\Components\Pages\Admin\Studio\LeftPanel"
if (Test-Path $leftPanelDir) {
    Remove-Item $leftPanelDir -Recurse -Force
    Write-Host "✓ Deleted LeftPanel directory" -ForegroundColor Green
} else {
    Write-Host "ℹ LeftPanel directory not found" -ForegroundColor Gray
}

# Delete RightPanel component
$rightPanelDir = "$baseDir\Components\Pages\Admin\Studio\RightPanel"
if (Test-Path $rightPanelDir) {
    Remove-Item $rightPanelDir -Recurse -Force
    Write-Host "✓ Deleted RightPanel directory" -ForegroundColor Green
} else {
    Write-Host "ℹ RightPanel directory not found" -ForegroundColor Gray
}

# Delete Canvas component
$canvasDir = "$baseDir\Components\Pages\Admin\Studio\Canvas"
if (Test-Path $canvasDir) {
    Remove-Item $canvasDir -Recurse -Force
    Write-Host "✓ Deleted Canvas directory" -ForegroundColor Green
} else {
    Write-Host "ℹ Canvas directory not found" -ForegroundColor Gray
}

# Delete Editor.razor
$editor = "$baseDir\Components\Pages\Admin\Studio\Editor.razor"
if (Test-Path $editor) {
    Remove-Item $editor -Force
    Write-Host "✓ Deleted Editor.razor" -ForegroundColor Green
} else {
    Write-Host "ℹ Editor.razor not found" -ForegroundColor Gray
}

# Delete DeviceView.cs
$deviceView = "$baseDir\Components\Pages\Admin\Studio\DeviceView.cs"
if (Test-Path $deviceView) {
    Remove-Item $deviceView -Force
    Write-Host "✓ Deleted DeviceView.cs" -ForegroundColor Green
} else {
    Write-Host "ℹ DeviceView.cs not found" -ForegroundColor Gray
}

# Delete entire Admin/Studio directory if empty
$studioDir = "$baseDir\Components\Pages\Admin\Studio"
if (Test-Path $studioDir) {
    $items = Get-ChildItem -Path $studioDir
    if ($items.Count -eq 0) {
        Remove-Item $studioDir -Force
        Write-Host "✓ Deleted empty Studio directory" -ForegroundColor Green
    } else {
        Write-Host "ℹ Studio directory not empty, keeping it" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Cleanup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "All old Studio components have been removed." -ForegroundColor Green
Write-Host "All compilation errors should now be resolved!" -ForegroundColor Green
Write-Host ""
Write-Host "Note: The Studio feature will need to be reimplemented" -ForegroundColor Yellow
Write-Host "when you're ready to work on Phase 10." -ForegroundColor Yellow
