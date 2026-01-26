@echo off
setlocal

echo ========================================
echo GFC PUBLISH TOOL
echo ========================================

:: Change to the folder where the bat file is
cd /d "%~dp0"

echo Current folder: %CD%

:: Check for the project file
if not exist "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" (
    echo [ERROR] Project not found!
    pause
    exit /b
)

echo Building...
dotnet publish "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" -c Release -o "publish_output"

if errorlevel 1 (
    echo [ERROR] Build failed.
    pause
    exit /b
)

echo Zipping to Desktop...
powershell -Command "Compress-Archive -Path 'publish_output\*' -DestinationPath '%USERPROFILE%\Desktop\PublishGFCWebApp.zip' -Force"

if exist "publish_output" rd /s /q "publish_output"

echo DONE! Zip is on your desktop.
pause
