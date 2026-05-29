Write-Host "--- APK PHYSICAL LOCATION CHECK ---" -ForegroundColor Cyan
$paths = @(
    "C:\WebSites\GFCPos\downloads\GFC_POS_Mobile.apk",
    "C:\WebSites\GFCPos\GFC_POS_Mobile.apk",
    "C:\inetpub\wwwroot\GFCPOS\downloads\GFC_POS_Mobile.apk",
    "C:\inetpub\wwwroot\GFCMobile\downloads\GFC_POS_Mobile.apk"
)
foreach ($p in $paths) {
    if (Test-Path $p) {
        $size = (Get-Item $p).Length / 1MB
        Write-Host "[FOUND] $p" ("({0:N2} MB)" -f $size) -ForegroundColor Green
    } else {
        Write-Host "[NOT FOUND] $p" -ForegroundColor Red
    }
}
