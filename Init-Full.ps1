$Server = "(localdb)\MSSQLLocalDB"
$Database = "ClubMembership"
$SqlScriptPath = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs\DatabaseScripts\INITIALIZE_DATABASE_COMPLETE.sql"

$LogFile = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\db_init_ps.log"
"Starting..." | Out-File $LogFile

try {
    Write-Host "Connecting to $Server..."
    $SqlConnection = New-Object Microsoft.Data.SqlClient.SqlConnection
    $SqlConnection.ConnectionString = "Server=$Server;Database=$Database;Integrated Security=True;Encrypt=False"
    $SqlConnection.Open()
    "Connected." | Out-File $LogFile -Append
    
    $SqlScript = Get-Content $SqlScriptPath -Raw
    $Commands = $SqlScript -split "(?m)^\s*GO\s*$"
    
    foreach ($CmdText in $Commands) {
        if (-not [string]::IsNullOrWhiteSpace($CmdText)) {
            $SqlCommand = $SqlConnection.CreateCommand()
            $SqlCommand.CommandText = $CmdText
            $SqlCommand.ExecuteNonQuery() | Out-Null
            "." | Out-File $LogFile -Append -NoNewline
        }
    }
    
    "`nSuccess!" | Out-File $LogFile -Append
    Write-Host "Database Initialization Complete!"
}
catch {
    "Error: $($_.Exception.Message)" | Out-File $LogFile -Append
    Write-Error "Error: $($_.Exception.Message)"
}
finally {
    if ($SqlConnection) { $SqlConnection.Close() }
}
