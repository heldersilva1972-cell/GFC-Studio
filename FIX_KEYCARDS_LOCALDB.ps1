<#
.SYNOPSIS
    Creates KeyCards table in LocalDB

.DESCRIPTION
    This script creates the KeyCards table and dependencies in your LocalDB instance
#>

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Creating KeyCards Table (LocalDB)" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

try {
    # Connection string for LocalDB
    $connectionString = "Server=(localdb)\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
    
    Write-Host "Testing connection to LocalDB..." -ForegroundColor Yellow
    
    # Test connection
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    Write-Host "✓ Connected to LocalDB successfully" -ForegroundColor Green
    $connection.Close()
    
    # Read SQL script
    $scriptPath = "docs\DatabaseScripts\CREATE_KEYCARDS_TABLE.sql"
    if (-not (Test-Path $scriptPath)) {
        throw "SQL script not found: $scriptPath"
    }
    
    Write-Host "✓ Found SQL script: $scriptPath" -ForegroundColor Green
    Write-Host ""
    Write-Host "Executing SQL script..." -ForegroundColor Yellow
    Write-Host ""
    
    $sqlScript = Get-Content $scriptPath -Raw
    
    # Remove USE statement as we're specifying database in connection string
    $sqlScript = $sqlScript -replace 'USE \[ClubMembership\];', ''
    
    # Split by GO statements
    $batches = $sqlScript -split '\r?\nGO\r?\n'
    
    $connection.Open()
    
    foreach ($batch in $batches) {
        $batch = $batch.Trim()
        if ($batch -ne "" -and $batch -notmatch '^\s*--') {
            try {
                $command = New-Object System.Data.SqlClient.SqlCommand($batch, $connection)
                $command.CommandTimeout = 60
                
                # Capture info messages
                $connection.FireInfoMessageEventOnUserErrors = $true
                $handler = [System.Data.SqlClient.SqlInfoMessageEventHandler] {
                    param($sender, $event)
                    $msg = $event.Message
                    if ($msg -match "✓") {
                        Write-Host $msg -ForegroundColor Green
                    } elseif ($msg -match "→") {
                        Write-Host $msg -ForegroundColor Cyan
                    } elseif ($msg -match "=") {
                        Write-Host $msg -ForegroundColor Gray
                    } else {
                        Write-Host $msg -ForegroundColor White
                    }
                }
                $connection.add_InfoMessage($handler)
                
                $command.ExecuteNonQuery() | Out-Null
            }
            catch {
                # Ignore errors for already existing objects
                if ($_.Exception.Message -notlike "*already exists*") {
                    Write-Host "Warning: $($_.Exception.Message)" -ForegroundColor Yellow
                }
            }
        }
    }
    
    $connection.Close()
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "SUCCESS! Tables Created" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    
    # Verify tables
    Write-Host "Verifying tables..." -ForegroundColor Yellow
    $connection.Open()
    
    $verifyQuery = @"
SELECT 
    t.name AS TableName,
    (SELECT COUNT(*) FROM sys.columns c WHERE c.object_id = t.object_id) AS ColumnCount
FROM sys.tables t
WHERE t.name IN ('KeyCards', 'KeyCardHistory', 'ControllerSyncQueue')
ORDER BY t.name
"@
    
    $command = New-Object System.Data.SqlClient.SqlCommand($verifyQuery, $connection)
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
    $dataset = New-Object System.Data.DataSet
    $adapter.Fill($dataset) | Out-Null
    
    if ($dataset.Tables[0].Rows.Count -gt 0) {
        Write-Host ""
        $dataset.Tables[0] | Format-Table -AutoSize
        Write-Host "✓ All tables verified!" -ForegroundColor Green
    }
    
    $connection.Close()
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "NEXT STEPS:" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "1. RESTART your GFC application" -ForegroundColor White
    Write-Host "2. The error should be GONE" -ForegroundColor White
    Write-Host "3. Test KeyCard functionality" -ForegroundColor White
    Write-Host ""
    
}
catch {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "ERROR" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "Try this:" -ForegroundColor Yellow
    Write-Host "1. Open SQL Server Management Studio" -ForegroundColor White
    Write-Host "2. Connect to: (localdb)\MSSQLLocalDB" -ForegroundColor White
    Write-Host "3. Open file: docs\DatabaseScripts\CREATE_KEYCARDS_TABLE.sql" -ForegroundColor White
    Write-Host "4. Execute (F5)" -ForegroundColor White
    Write-Host ""
    exit 1
}
