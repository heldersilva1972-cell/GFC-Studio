<#
.SYNOPSIS
    Fixes the missing KeyCards table in the GFC database

.DESCRIPTION
    This script executes the CREATE_KEYCARDS_TABLE.sql script to create
    the KeyCards table and related dependencies.

.PARAMETER ServerInstance
    SQL Server instance name (default: localhost\SQLEXPRESS)

.PARAMETER Database
    Database name (default: ClubMembership)

.EXAMPLE
    .\Fix-KeyCardsTable.ps1

.EXAMPLE
    .\Fix-KeyCardsTable.ps1 -ServerInstance "localhost" -Database "ClubMembership"
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [string]$ServerInstance = "localhost\SQLEXPRESS",
    
    [Parameter(Mandatory=$false)]
    [string]$Database = "ClubMembership"
)

$ErrorActionPreference = "Stop"

# Color output functions
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Cyan
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Header {
    param([string]$Message)
    Write-Host "`n═══════════════════════════════════════════════════════" -ForegroundColor Magenta
    Write-Host "  $Message" -ForegroundColor Magenta
    Write-Host "═══════════════════════════════════════════════════════`n" -ForegroundColor Magenta
}

try {
    Write-Header "GFC KeyCards Table Fix"
    
    # Get script directory
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    $sqlScriptPath = Join-Path (Split-Path -Parent $scriptDir) "DatabaseScripts\CREATE_KEYCARDS_TABLE.sql"
    
    # Verify SQL script exists
    if (-not (Test-Path $sqlScriptPath)) {
        throw "SQL script not found: $sqlScriptPath"
    }
    
    Write-Info "SQL Script: $sqlScriptPath"
    Write-Info "Server: $ServerInstance"
    Write-Info "Database: $Database"
    Write-Host ""
    
    # Test SQL Server connection
    Write-Info "Testing SQL Server connection..."
    
    try {
        $connectionString = "Server=$ServerInstance;Database=master;Integrated Security=True;TrustServerCertificate=True"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        $connection.Close()
        Write-Success "SQL Server connection successful"
    }
    catch {
        throw "Cannot connect to SQL Server: $_"
    }
    
    # Check if database exists
    Write-Info "Checking if database '$Database' exists..."
    
    try {
        $checkDbQuery = "SELECT database_id FROM sys.databases WHERE name = '$Database'"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        $command = New-Object System.Data.SqlClient.SqlCommand($checkDbQuery, $connection)
        $result = $command.ExecuteScalar()
        $connection.Close()
        
        if ($null -eq $result) {
            throw "Database '$Database' does not exist. Please create it first."
        }
        
        Write-Success "Database '$Database' exists"
    }
    catch {
        throw "Error checking database: $_"
    }
    
    # Execute SQL script
    Write-Header "Executing SQL Script"
    
    Write-Info "Creating KeyCards table and dependencies..."
    Write-Host ""
    
    try {
        # Use Invoke-Sqlcmd if available
        if (Get-Command Invoke-Sqlcmd -ErrorAction SilentlyContinue) {
            $result = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $Database -InputFile $sqlScriptPath -Verbose 4>&1
            
            # Display output
            foreach ($line in $result) {
                if ($line -match "✓") {
                    Write-Success $line
                }
                elseif ($line -match "→") {
                    Write-Info "  $line"
                }
                else {
                    Write-Host $line -ForegroundColor Gray
                }
            }
        }
        else {
            # Fallback to SqlClient
            Write-Info "Using SqlClient (Invoke-Sqlcmd not available)"
            
            $sqlScript = Get-Content $sqlScriptPath -Raw
            $connectionString = "Server=$ServerInstance;Database=$Database;Integrated Security=True;TrustServerCertificate=True"
            
            $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
            $connection.Open()
            
            # Split by GO statements
            $batches = $sqlScript -split '\r?\nGO\r?\n'
            
            foreach ($batch in $batches) {
                if ($batch.Trim() -ne "") {
                    $command = New-Object System.Data.SqlClient.SqlCommand($batch, $connection)
                    $command.CommandTimeout = 60
                    
                    # Execute and capture messages
                    $connection.FireInfoMessageEventOnUserErrors = $true
                    $connection.add_InfoMessage({
                        param($sender, $event)
                        $msg = $event.Message
                        if ($msg -match "✓") {
                            Write-Success $msg
                        }
                        elseif ($msg -match "→") {
                            Write-Info "  $msg"
                        }
                        else {
                            Write-Host $msg -ForegroundColor Gray
                        }
                    })
                    
                    $command.ExecuteNonQuery() | Out-Null
                }
            }
            
            $connection.Close()
        }
        
        Write-Host ""
        Write-Success "SQL script executed successfully!"
    }
    catch {
        throw "Error executing SQL script: $_"
    }
    
    # Verify tables were created
    Write-Header "Verification"
    
    Write-Info "Verifying tables were created..."
    
    try {
        $verifyQuery = @"
SELECT 
    t.name AS TableName,
    (SELECT COUNT(*) FROM sys.columns c WHERE c.object_id = t.object_id) AS ColumnCount,
    (SELECT COUNT(*) FROM sys.indexes i WHERE i.object_id = t.object_id AND i.is_primary_key = 0) AS IndexCount
FROM sys.tables t
WHERE t.name IN ('KeyCards', 'KeyCardHistory', 'ControllerSyncQueue')
ORDER BY t.name
"@
        
        $connectionString = "Server=$ServerInstance;Database=$Database;Integrated Security=True;TrustServerCertificate=True"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        $command = New-Object System.Data.SqlClient.SqlCommand($verifyQuery, $connection)
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
        $dataset = New-Object System.Data.DataSet
        $adapter.Fill($dataset) | Out-Null
        $connection.Close()
        
        if ($dataset.Tables[0].Rows.Count -gt 0) {
            Write-Host ""
            $dataset.Tables[0] | Format-Table -AutoSize
            Write-Success "All tables created successfully!"
        }
        else {
            Write-Error "No tables found. Script may have failed."
        }
    }
    catch {
        Write-Error "Error verifying tables: $_"
    }
    
    # Summary
    Write-Header "Summary"
    
    Write-Host @"
✓ KeyCards table created
✓ KeyCardHistory table created
✓ ControllerSyncQueue table created
✓ Indexes created for performance
✓ Database schema is ready

NEXT STEPS:
1. Restart your GFC application
2. Test the KeyCard functionality
3. The error "Invalid object name 'dbo.KeyCards'" should be resolved

"@ -ForegroundColor Green

    Write-Info "You can now proceed with the Cloudflare Tunnel setup once the app is running."
    
}
catch {
    Write-Host "`n"
    Write-Error "Script failed: $_"
    Write-Host "`nStack Trace:" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    Write-Host ""
    Write-Host "TROUBLESHOOTING:" -ForegroundColor Yellow
    Write-Host "1. Verify SQL Server is running: Get-Service MSSQL*" -ForegroundColor Gray
    Write-Host "2. Check server instance name: $ServerInstance" -ForegroundColor Gray
    Write-Host "3. Verify database exists: $Database" -ForegroundColor Gray
    Write-Host "4. Ensure you have permissions to create tables" -ForegroundColor Gray
    exit 1
}
