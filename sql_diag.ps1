
$results = @{}

# Check SQL Services
$results["Services"] = Get-Service | Where-Object { $_.Name -like "*SQL*" } | Select-Object Name, Status, DisplayName

# Check LocalDB instances
try {
    $results["LocalDB"] = sqllocaldb info
} catch {
    $results["LocalDB"] = "sqllocaldb not found"
}

# Check Hostname
$results["Hostname"] = hostname

# Try to connect to common local instances
$instances = @("localhost", "(local)", ".\SQLEXPRESS", "localhost\SQLEXPRESS", "(localdb)\MSSQLLocalDB")
$connectionResults = @()

foreach ($instance in $instances) {
    $connString = "Server=$instance;Database=master;Integrated Security=True;Connection Timeout=2"
    $conn = New-Object System.Data.SqlClient.SqlConnection($connString)
    try {
        $conn.Open()
        $connectionResults += "$instance: SUCCESS"
        $conn.Close()
    } catch {
        $connectionResults += "$instance: FAILED ($($_.Exception.Message))"
    }
}
$results["Connections"] = $connectionResults

$results | ConvertTo-Json | Out-File -FilePath "sql_diag.json"
