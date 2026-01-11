Write-Host "=== GFC DIAGNOSTIC TOOL ===" -ForegroundColor Cyan

# 1. READ APP SETTINGS
$appSettingsPath = "C:\inetpub\GFCWebApp\appsettings.json"

if (-not (Test-Path $appSettingsPath)) {
    Write-Host "ERROR: appsettings.json not found at $appSettingsPath" -ForegroundColor Red
    return
}

# Use -Raw to read the whole file as a single string
$jsonContent = Get-Content $appSettingsPath -Raw
# Parse with ConvertFrom-Json
$json = $jsonContent | ConvertFrom-Json

# Access the nested property
$connString = $json.ConnectionStrings.GFC

Write-Host "1. APP CONFIGURATION" -ForegroundColor Yellow
Write-Host "   File: $appSettingsPath"
Write-Host "   Connection String: $connString"

# 2. TEST CONNECTION & COUNT DATA
Write-Host "`n2. DATABASE PROBE" -ForegroundColor Yellow

$query = @"
SELECT 
    @@SERVERNAME as [SQL Instance], 
    DB_NAME() as [Database Name], 
    (SELECT COUNT(*) FROM AppUsers) as [Total Users],
    (SELECT COUNT(*) FROM AppUsers WHERE Username = 'admin') as [Admin Count]
"@

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connString
    $connection.Open()

    $command = $connection.CreateCommand()
    $command.CommandText = $query
    
    $reader = $command.ExecuteReader()
    
    if ($reader.Read()) {
        $server = $reader["SQL Instance"]
        $dbName = $reader["Database Name"]
        $userCount = $reader["Total Users"]
        $adminCount = $reader["Admin Count"]

        Write-Host "   CONNECTED SUCCESSFULY!" -ForegroundColor Green
        Write-Host "   ---------------------"
        Write-Host "   Server Hosted On:  $server"
        Write-Host "   Database Name:     $dbName"
        Write-Host "   Total Members:     $userCount"
        Write-Host "   Admin Account:     $adminCount"
        Write-Host "   ---------------------"

        if ($userCount -gt 1) {
            Write-Host "   [!] ALERT: This database HAS DATA ($userCount members)!" -ForegroundColor Red
            Write-Host "       This explains why the App shows members." -ForegroundColor Yellow
            Write-Host "       The App is pointing to: $server" -ForegroundColor Yellow
            Write-Host "       But you likely wiped:   .\SQLEXPRESS (or a different instance)" -ForegroundColor Yellow
        } elseif ($userCount -eq 1) {
            Write-Host "   [OK] Database 'ClubMembership' on '$server' IS CLEAN." -ForegroundColor Green
            Write-Host "        If the App still shows 144 members, try restarting IIS." -ForegroundColor Cyan
        } else {
            Write-Host "   [!] ALERT: Database is COMPLETELY EMPTY (0 users)." -ForegroundColor Red
        }
    }
    $connection.Close()
}
catch {
    Write-Host "   [!] CONNECTION FAILED: $_" -ForegroundColor Red
}

Write-Host "`n=== END DIAGNOSTIC ===" -ForegroundColor Cyan
