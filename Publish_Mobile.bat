@echo off
setlocal
echo Publishing Mobile Standalone...
cd /d "%~dp0"
dotnet publish "apps\GFC-Mobile-Standalone\GFC.Mobile\GFC.Mobile.csproj" -c Release -o "publish_mobile"
powershell -Command "Compress-Archive -Path 'publish_mobile\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_Mobile_Standalone.zip' -Force"
rd /s /q "publish_mobile"
echo DONE! Zip is on your desktop.
pause
