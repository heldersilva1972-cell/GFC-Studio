# Delete orphaned Studio.razor.css file

$cssFile = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\Components\Pages\Studio.razor.css"

if (Test-Path $cssFile) {
    Remove-Item $cssFile -Force
    Write-Host "✓ Deleted orphaned Studio.razor.css" -ForegroundColor Green
} else {
    Write-Host "ℹ Studio.razor.css not found (may already be deleted)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "All Studio component errors should now be resolved!" -ForegroundColor Green
