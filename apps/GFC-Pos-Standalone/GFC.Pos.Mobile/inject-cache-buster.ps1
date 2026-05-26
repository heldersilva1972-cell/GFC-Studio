param(
    [string]$indexHtmlPath,
    [string]$version
)

if (Test-Path $indexHtmlPath) {
    $content = Get-Content -Path $indexHtmlPath -Raw
    
    # 1. Inject cache-buster query strings on CSS links
    $content = $content -replace 'href="(_content/GFC.Pos.UI/css/app.css)(\?v=[^"]*)?"', ('href="_content/GFC.Pos.UI/css/app.css?v=' + $version + '"')
    $content = $content -replace 'href="(_content/GFC.Pos.UI/css/pos-terminal.css)(\?v=[^"]*)?"', ('href="_content/GFC.Pos.UI/css/pos-terminal.css?v=' + $version + '"')
    $content = $content -replace 'href="(_content/GFC.Pos.UI/css/pos-terminal-print.css)(\?v=[^"]*)?"', ('href="_content/GFC.Pos.UI/css/pos-terminal-print.css?v=' + $version + '"')
    
    # 2. Inject cache-buster query strings on JS scripts
    $content = $content -replace 'src="(_content/GFC.Pos.UI/js/connectivity.js)(\?v=[^"]*)?"', ('src="_content/GFC.Pos.UI/js/connectivity.js?v=' + $version + '"')
    $content = $content -replace 'Worker\("(_content/GFC.Pos.UI/gfc-storage-worker.js)(\?v=[^"]*)?"\)', ('Worker("_content/GFC.Pos.UI/gfc-storage-worker.js?v=' + $version + '")')
    
    Set-Content -Path $indexHtmlPath -Value $content -NoNewline
    Write-Output ">>> [GFC BUILD] Cache-buster injected successfully: v$version"
} else {
    Write-Warning ">>> [GFC BUILD] index.html not found for cache-buster injection at $indexHtmlPath"
}
