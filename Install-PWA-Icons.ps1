# Copy PWA Icons to wwwroot/images
# This script copies the generated PWA icons to the correct location

$sourceDir = "C:\Users\hnsil\.gemini\antigravity\brain\aab58657-422c-4597-8479-c0202c17b2a5"
$targetDir = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\wwwroot\images"

# Create images directory if it doesn't exist
if (-not (Test-Path $targetDir)) {
    New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
    Write-Host "✅ Created directory: $targetDir" -ForegroundColor Green
}

# Find the most recent PWA icon files
$icon512 = Get-ChildItem -Path $sourceDir -Filter "pwa_icon_512_*.png" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
$icon192 = Get-ChildItem -Path $sourceDir -Filter "pwa_icon_192_*.png" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($icon512) {
    Copy-Item -Path $icon512.FullName -Destination "$targetDir\pwa-icon-512.png" -Force
    Write-Host "✅ Copied 512x512 icon: pwa-icon-512.png" -ForegroundColor Green
} else {
    Write-Host "❌ 512x512 icon not found" -ForegroundColor Red
}

if ($icon192) {
    Copy-Item -Path $icon192.FullName -Destination "$targetDir\pwa-icon-192.png" -Force
    Write-Host "✅ Copied 192x192 icon: pwa-icon-192.png" -ForegroundColor Green
} else {
    Write-Host "❌ 192x192 icon not found" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎉 PWA icons installed successfully!" -ForegroundColor Cyan
Write-Host "   Location: $targetDir" -ForegroundColor Gray
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Restart your application" -ForegroundColor White
Write-Host "2. Clear browser cache (Ctrl+Shift+Delete)" -ForegroundColor White
Write-Host "3. Reload the page (Ctrl+F5)" -ForegroundColor White
