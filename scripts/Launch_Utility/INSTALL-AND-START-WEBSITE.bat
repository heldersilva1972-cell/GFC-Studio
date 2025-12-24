@echo off
echo ========================================
echo GFC WEBSITE - COMPLETE INSTALLATION
echo ========================================
echo.

cd /d "%~dp0apps\website"

echo Step 1: Cleaning up any previous attempts...
if exist node_modules rmdir /s /q node_modules
if exist package-lock.json del package-lock.json
echo Done.
echo.

echo Step 2: Installing dependencies...
echo This will take 2-3 minutes. Please wait...
echo.

call npm install --force

if errorlevel 1 (
    echo.
    echo Installation failed. Trying alternative method...
    call npm cache clean --force
    call npm install --legacy-peer-deps --force
)

echo.
echo ========================================
echo Installation Complete!
echo ========================================
echo.
echo Press any key to start the website server...
pause >nul

echo.
echo Starting Next.js development server...
echo Website will be available at http://localhost:3000
echo.
echo Press Ctrl+C to stop the server
echo.

start http://localhost:3000

call npm run dev
