$server = ".\SQLEXPRESS"
$db = "ClubMembership"
$user = "$env:USERDOMAIN\$env:USERNAME"

Write-Host "--- SQL SERVER DIAGNOSTICS ---" -ForegroundColor Cyan
Write-Host "Checking connection to '$server' as current Windows user: '$user'"
Write-Host "--------------------------------"

# Helper function to run sqlcmd
function Run-SqlCmd {
    param($Query, $Description)
    Write-Host "`n[$Description]" -ForegroundColor Yellow
    $cmd = "sqlcmd -S `"$server`" -E -W -s `",`" -Q `"$Query`""
    
    # We use cmd /c to ensure sqlcmd behaves predictably regarding output streams
    cmd /c $cmd 2>&1
}

# 1. Check basic connectivity and currently recognized user
Run-SqlCmd -Description "1. Server Connection & Identity Check" -Query "SELECT @@SERVERNAME as [ServerName], SYSTEM_USER as [LoginName], IS_SRVROLEMEMBER('sysadmin') as [IsSysAdmin];"

# 2. Check if the database exists and is online
Run-SqlCmd -Description "2. Database Status Check ('$db')" -Query "SELECT name, state_desc, user_access_desc FROM sys.databases WHERE name = '$db';"

# 3. Check Server-Level Logins for your user
Run-SqlCmd -Description "3. Checking System Logins for '$user'" -Query "SELECT name, type_desc, is_disabled, create_date FROM sys.server_principals WHERE name LIKE '%Helder%' OR name LIKE '%$env:USERNAME%';"

# 4. Check Database-Level Users
Run-SqlCmd -Description "4. Checking Database Users inside '$db'" -Query "USE [$db]; SELECT name, type_desc, authentication_type_desc FROM sys.database_principals WHERE name LIKE '%Helder%' OR name LIKE '%$env:USERNAME%';"

# 5. Check Database Role Membership (e.g., db_owner)
Run-SqlCmd -Description "5. Checking Permissions/Roles inside '$db'" -Query "USE [$db]; SELECT p.name as [User], r.name as [Role] FROM sys.database_role_members rm JOIN sys.database_principals p ON rm.member_principal_id = p.principal_id JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id WHERE p.name LIKE '%Helder%' OR p.name LIKE '%$env:USERNAME%';"

Write-Host "`n--------------------------------"
Write-Host "Diagnostics Complete." -ForegroundColor Cyan
