@echo off
echo Starting GFC Website...
echo.

cd /d "%~dp0apps\website"

REM Check if node_modules exists
if not exist "node_modules\" (
    echo Installing dependencies for the first time...
    echo This may take 2-3 minutes...
    call npm install --legacy-peer-deps
    echo.
)

echo Starting Next.js development server...
echo Website will open at http://localhost:3000
echo.
echo Press Ctrl+C to stop the server
echo.

start http://localhost:3000

call npm run dev
