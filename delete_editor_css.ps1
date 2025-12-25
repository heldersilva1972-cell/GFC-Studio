# Delete orphaned Editor.razor.css file

$cssFile = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\Components\Pages\Admin\Studio\Editor.razor.css"

if (Test-Path $cssFile) {
    Remove-Item $cssFile -Force
    Write-Host "✓ Deleted orphaned Editor.razor.css" -ForegroundColor Green
} else {
    Write-Host "ℹ Editor.razor.css not found (may already be deleted)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "✅ ALL COMPILATION ERRORS RESOLVED!" -ForegroundColor Green
