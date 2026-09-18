$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== TERMINALS IN DB ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM PosTerminals"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "Name: $($reader['Name']) | IP: $($reader['IpAddress']) | LastSeen: $($reader['LastSeen']) | ActiveUser: $($reader['ActiveUser'])"
}
$reader.Close()

Write-Host "`n=== RECENT TELEMETRY FOR DOWNSTAIRS BAR POS ==="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = "SELECT TOP 15 Timestamp, Category, Level, Message FROM PosTelemetryLogs WHERE TerminalName LIKE '%Downstairs%' ORDER BY Timestamp DESC"
$reader2 = $cmd2.ExecuteReader()
while ($reader2.Read()) {
    Write-Host "[$($reader2['Timestamp'])] [$($reader2['Category'])/$($reader2['Level'])]: $($reader2['Message'])"
}
$reader2.Close()

$conn.Close()
