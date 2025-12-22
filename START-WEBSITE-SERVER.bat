@echo off
title GFC Website Server
echo ========================================
echo    GFC WEBSITE SERVER
echo ========================================
echo.
echo Starting web server on port 3000...
echo.

cd /d "%~dp0apps\website\public"

echo Website will be available at:
echo http://localhost:3000
echo.
echo Opening browser...
timeout /t 2 /nobreak >nul
start http://localhost:3000

echo.
echo Server is running. Press Ctrl+C to stop.
echo ========================================
echo.

python -m http.server 3000 2>nul
if errorlevel 1 (
    echo Python not found. Trying alternative method...
    powershell -Command "Start-Process 'http://localhost:3000'; & {$listener = New-Object System.Net.HttpListener; $listener.Prefixes.Add('http://localhost:3000/'); $listener.Start(); Write-Host 'Server started on http://localhost:3000'; while ($listener.IsListening) { $context = $listener.GetContext(); $response = $context.Response; $content = [System.IO.File]::ReadAllBytes('index.html'); $response.ContentLength64 = $content.Length; $response.OutputStream.Write($content, 0, $content.Length); $response.Close() }}"
)
