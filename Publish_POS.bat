@echo off
setlocal
echo Publishing POS Standalone...
cd /d "%~dp0"
dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Mobile\GFC.Pos.Mobile.csproj" -c Release -o "publish_pos"
powershell -Command "Compress-Archive -Path 'publish_pos\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_POS_Standalone.zip' -Force"
rd /s /q "publish_pos"
echo DONE! Zip is on your desktop.
pause
