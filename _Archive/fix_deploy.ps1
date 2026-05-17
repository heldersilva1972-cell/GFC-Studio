 = Get-Content 'Deploy_Full_Suite.bat' -Raw
 = '         = Get-ChildItem -Path  -Recurse >> "%PS_PATH%"
        foreach ( in ) { >> "%PS_PATH%"
            if (.Name -eq "appsettings.Production.json") { continue } >> "%PS_PATH%"
             = .FullName.Substring(.Length).TrimStart("\") >> "%PS_PATH%"
             = Join-Path .Live  >> "%PS_PATH%"
            if (.PSIsContainer) { if (-not (Test-Path )) { New-Item -ItemType Directory -Path  ^| Out-Null } } >> "%PS_PATH%"
            else { if (-not (Test-Path (Split-Path ))) { New-Item -ItemType Directory -Path (Split-Path ) ^| Out-Null } ; Copy-Item -Path .FullName -Destination  -Force } >> "%PS_PATH%"
        } >> "%PS_PATH%"'

 = '        if (.Name -eq "GFCMobile") { >> "%PS_PATH%"
            Write-Step "Flattening GFCMobile deployment..." >> "%PS_PATH%"
             = Join-Path  "wwwroot" >> "%PS_PATH%"
            robocopy  .Live /S /E /PURGE /XD "history" /XF "appsettings.Production.json" "web.config" ^| Out-Null >> "%PS_PATH%"
             = Join-Path .Live "web.config" >> "%PS_PATH%"
             = @^"
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <staticContent>
      <remove fileExtension=".blat" />
      <remove fileExtension=".dat" />
      <remove fileExtension=".dll" />
      <remove fileExtension=".webcil" />
      <remove fileExtension=".json" />
      <remove fileExtension=".wasm" />
      <remove fileExtension=".woff" />
      <remove fileExtension=".woff2" />
      <mimeMap fileExtension=".blat" mimeType="application/octet-stream" />
      <mimeMap fileExtension=".dll" mimeType="application/octet-stream" />
      <mimeMap fileExtension=".webcil" mimeType="application/octet-stream" />
      <mimeMap fileExtension=".dat" mimeType="application/octet-stream" />
      <mimeMap fileExtension=".json" mimeType="application/json" />
      <mimeMap fileExtension=".wasm" mimeType="application/wasm" />
      <mimeMap fileExtension=".woff" mimeType="application/font-woff" />
      <mimeMap fileExtension=".woff2" mimeType="application/font-woff" />
      <remove fileExtension=".webmanifest" />
      <mimeMap fileExtension=".webmanifest" mimeType="application/manifest+json" />
    </staticContent>
    <httpCompression>
      <dynamicTypes>
        <add mimeType="application/octet-stream" enabled="true" />
        <add mimeType="application/wasm" enabled="true" />
      </dynamicTypes>
    </httpCompression>
    <rewrite>
      <rules>
        <rule name="SPA fallback routing" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
"^@ >> "%PS_PATH%"
            Set-Content -Path  -Value  -Encoding UTF8 >> "%PS_PATH%"
        } else { >> "%PS_PATH%"
             = Get-ChildItem -Path  -Recurse >> "%PS_PATH%"
            foreach ( in ) { >> "%PS_PATH%"
                if (.Name -eq "appsettings.Production.json") { continue } >> "%PS_PATH%"
                 = .FullName.Substring(.Length).TrimStart("\") >> "%PS_PATH%"
                 = Join-Path .Live  >> "%PS_PATH%"
                if (.PSIsContainer) { if (-not (Test-Path )) { New-Item -ItemType Directory -Path  ^| Out-Null } } >> "%PS_PATH%"
                else { if (-not (Test-Path (Split-Path ))) { New-Item -ItemType Directory -Path (Split-Path ) ^| Out-Null } ; Copy-Item -Path .FullName -Destination  -Force } >> "%PS_PATH%"
            } >> "%PS_PATH%"
        } >> "%PS_PATH%"'

 =  -replace '\r\n', "
"
 =  -replace '\r\n', "
"
 = .Replace(, )
[System.IO.File]::WriteAllText('Deploy_Full_Suite.bat', )
