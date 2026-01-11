Write-Host "=== GFC DEEP INSPECTION TOOL ===" -ForegroundColor Cyan
$webDir = "C:\inetpub\GFCWebApp"

# 1. LIST JSON FILES
Write-Host "`n1. CHECKING FOR CONFIGURATION OVERRIDES..." -ForegroundColor Yellow
$jsonFiles = Get-ChildItem -Path $webDir -Filter "*.json"
foreach ($file in $jsonFiles) {
    Write-Host "   Found: $($file.Name)" -ForegroundColor Cyan
    if ($file.Name -like "appsettings.*.json") {
       Write-Host "   [!] POTENTIAL OVERRIDE DETECTED: $($file.Name)" -ForegroundColor Red
       $content = Get-Content $file.FullName -Raw
       Write-Host "       Content Snippet: $($content.Substring(0, [math]::Min(100, $content.Length)))..." -ForegroundColor Gray
    }
}

# 2. CHECK WEB.CONFIG
Write-Host "`n2. CHECKING WEB.CONFIG..." -ForegroundColor Yellow
$webConfigPath = "$webDir\web.config"
if (Test-Path $webConfigPath) {
    $xml = [xml](Get-Content $webConfigPath)
    $envVars = $xml.configuration.system.webServer.aspNetCore.environmentVariables.environmentVariable
    if ($envVars) {
        foreach ($var in $envVars) {
             Write-Host "   Env Var: $($var.name) = $($var.value)" -ForegroundColor Magenta
        }
    } else {
        Write-Host "   No environment variables found in web.config." -ForegroundColor Green
    }
} else {
    Write-Host "   web.config not found." -ForegroundColor Red
}

# 3. CHECK CONNECTIONS AGAIN
Write-Host "`n3. DOUBLE CHECKING SQL..." -ForegroundColor Yellow
# Redefine checking logic
function Check-Db($connStr, $label) {
    try {
        $conn = New-Object System.Data.SqlClient.SqlConnection
        $conn.ConnectionString = $connStr
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT COUNT(*) FROM AppUsers"
        $count = $cmd.ExecuteScalar()
        $conn.Close()
        Write-Host "   Using ($label): Found $count members." -ForegroundColor ($count -eq 1 ? "Green" : "Red")
    } catch {
        Write-Host "   Using ($label): Connection Failed." -ForegroundColor Gray
    }
}

# Check main appsettings
$mainJson = Get-Content "$webDir\appsettings.json" -Raw | ConvertFrom-Json
Check-Db $mainJson.ConnectionStrings.GFC "appsettings.json"

# Check Production if exists
if (Test-Path "$webDir\appsettings.Production.json") {
    $prodJson = Get-Content "$webDir\appsettings.Production.json" -Raw | ConvertFrom-Json
    Check-Db $prodJson.ConnectionStrings.GFC "appsettings.Production.json"
}

Write-Host "`n=== INSPECTION COMPLETE ===" -ForegroundColor Cyan
