$port = 5500
$path = "$PSScriptRoot\apps\website\public"

Write-Host "STARTING SIMPLE SERVER..." -ForegroundColor Green
Write-Host "Directory: $path"
Write-Host "Port: $port"

$listener = New-Object System.Net.HttpListener
$listener.Prefixes.Add("http://localhost:$port/")
$listener.Start()

Write-Host ""
Write-Host "✅ SERVER IS RUNNING!" -ForegroundColor Green
Write-Host "👉 http://localhost:$port" -ForegroundColor Cyan
Write-Host ""
Write-Host "Keep this window open." -ForegroundColor Yellow

# Auto-open browser
Start-Process "http://localhost:$port"

try {
    while ($listener.IsListening) {
        $context = $listener.GetContext()
        $response = $context.Response
        
        # Always serve index.html for simplicity 
        # (This acts like a Single Page App fallback which is perfect for our static site)
        $filepath = Join-Path $path "index.html"
        
        $buffer = [System.IO.File]::ReadAllBytes($filepath)
        $response.ContentLength64 = $buffer.Length
        $response.ContentType = "text/html"
        $response.OutputStream.Write($buffer, 0, $buffer.Length)
        $response.Close()
    }
}
catch {
    Write-Error $_
}
finally {
    $listener.Stop()
}
