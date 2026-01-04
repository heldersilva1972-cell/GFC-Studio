$Server = "(localdb)\MSSQLLocalDB"
$Database = "ClubMembership"
$SqlScriptPath = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs\DatabaseScripts\INITIALIZE_DASHBOARD.sql"

Write-Host "Connecting to $Server..."
$SqlConnection = New-Object Microsoft.Data.SqlClient.SqlConnection
$SqlConnection.ConnectionString = "Server=$Server;Database=$Database;Integrated Security=True;Encrypt=False"

try {
    $SqlConnection.Open()
    Write-Host "Connected successfully. Running script..."
    
    $SqlScript = Get-Content $SqlScriptPath -Raw
    # Split by GO
    $Commands = $SqlScript -split "(?m)^\s*GO\s*$"
    
    foreach ($CmdText in $Commands) {
        if (-not [string]::IsNullOrWhiteSpace($CmdText)) {
            $SqlCommand = $SqlConnection.CreateCommand()
            $SqlCommand.CommandText = $CmdText
            $SqlCommand.ExecuteNonQuery() | Out-Null
        }
    }
    
    Write-Host "Database Initialization Complete!"
}
catch {
    Write-Error "Error: $($_.Exception.Message)"
}
finally {
    $SqlConnection.Close()
}
