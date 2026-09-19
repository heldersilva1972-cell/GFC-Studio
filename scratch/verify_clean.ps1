$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM PosZReports WHERE Id = '575c3c9f-c7e6-46ba-8555-a6b29621d361'"
$r = $cmd.ExecuteReader()
if ($r.Read()) {
    Write-Host "Z-Report verified for $($r['BartenderName']) at $($r['Timestamp'])"
}
$r.Close()
$conn.Close()
