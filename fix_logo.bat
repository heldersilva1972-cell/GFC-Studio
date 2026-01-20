@echo off
set SRC="C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\Pictures\version 2 logo.png"
set DEST1="C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\wwwroot\images\pwa-icon-192.png"
set DEST2="C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\wwwroot\images\pwa-icon-512.png"

echo Copying new logo...
copy /Y %SRC% %DEST1%
copy /Y %SRC% %DEST2%
echo Done.
