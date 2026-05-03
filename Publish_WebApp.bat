@echo off
setlocal
echo Publishing Main WebApp...
cd /d "%~dp0"
dotnet publish "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" -c Release -o "publish_webapp"
powershell -Command "Compress-Archive -Path 'publish_webapp\*' -DestinationPath '%USERPROFILE%\Desktop\PublishGFCWebApp.zip' -Force"
rd /s /q "publish_webapp"
echo DONE! Zip is on your desktop.
pause
