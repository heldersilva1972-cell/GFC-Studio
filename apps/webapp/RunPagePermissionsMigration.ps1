# Run Page Permissions Migration
# This script executes the SQL migration to create the page permissions tables

$sqlFile = "Database\Migrations\AddPagePermissions.sql"
$server = "localhost"
$database = "GFC"

Write-Host "Running Page Permissions Migration..." -ForegroundColor Cyan
Write-Host "Server: $server" -ForegroundColor Gray
Write-Host "Database: $database" -ForegroundColor Gray
Write-Host "SQL File: $sqlFile" -ForegroundColor Gray
Write-Host ""

try {
    # Read the SQL file
    $sqlContent = Get-Content $sqlFile -Raw
    
    # Create connection
    $connectionString = "Server=$server;Database=$database;Integrated Security=True;TrustServerCertificate=True;"
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    
    # Open connection
    $connection.Open()
    Write-Host "✓ Connected to database" -ForegroundColor Green
    
    # Execute SQL
    $command = $connection.CreateCommand()
    $command.CommandText = $sqlContent
    $command.CommandTimeout = 60
    
    Write-Host "Executing migration..." -ForegroundColor Yellow
    $result = $command.ExecuteNonQuery()
    
    Write-Host "✓ Migration completed successfully!" -ForegroundColor Green
    Write-Host ""
    
    # Verify tables were created
    $verifyCommand = $connection.CreateCommand()
    $verifyCommand.CommandText = @"
SELECT 
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AppPages') as AppPagesExists,
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserPagePermissions') as UserPagePermissionsExists,
    (SELECT COUNT(*) FROM AppPages) as PageCount
"@
    
    $reader = $verifyCommand.ExecuteReader()
    if ($reader.Read()) {
        $appPagesExists = $reader["AppPagesExists"]
        $userPagePermissionsExists = $reader["UserPagePermissionsExists"]
        $pageCount = $reader["PageCount"]
        
        Write-Host "Verification:" -ForegroundColor Cyan
        Write-Host "  AppPages table: $(if($appPagesExists -eq 1){'✓ Created'}else{'✗ Not found'})" -ForegroundColor $(if($appPagesExists -eq 1){'Green'}else{'Red'})
        Write-Host "  UserPagePermissions table: $(if($userPagePermissionsExists -eq 1){'✓ Created'}else{'✗ Not found'})" -ForegroundColor $(if($userPagePermissionsExists -eq 1){'Green'}else{'Red'})
        Write-Host "  Pages seeded: $pageCount" -ForegroundColor Green
    }
    $reader.Close()
    
    # Close connection
    $connection.Close()
    Write-Host ""
    Write-Host "✓ Migration completed successfully! You can now use the Page Permissions feature." -ForegroundColor Green
    
} catch {
    Write-Host "✗ Error running migration:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "Stack Trace:" -ForegroundColor Yellow
    Write-Host $_.Exception.StackTrace -ForegroundColor Gray
    exit 1
}
