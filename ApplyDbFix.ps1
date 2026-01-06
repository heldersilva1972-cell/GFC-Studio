$ErrorActionPreference = "Stop"
try {
    $connStr = "Server=localhost;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
    $conn = New-Object System.Data.SqlClient.SqlConnection $connStr
    $conn.Open()
    Write-Host "Connected to Database."
    
    $scriptContent = Get-Content "docs\DatabaseScripts\Fix_SystemSettings_Operations_Columns.sql" -Raw
    
    # Split by GO (case insensitive, on its own line)
    $batches = $scriptContent -split "(?m)^\s*GO\s*$"
    
    foreach ($batch in $batches) {
        if (-not [string]::IsNullOrWhiteSpace($batch)) {
            $cmd = $conn.CreateCommand()
            $cmd.CommandText = $batch
            $cmd.ExecuteNonQuery()
        }
    }
    
    Write-Host "Database fix applied successfully."
} catch {
    Write-Host "Error executing script: $_"
    Write-Host "Stack Trace: $($_.ScriptStackTrace)"
    exit 1
} finally {
    if ($conn) { $conn.Close() }
}
