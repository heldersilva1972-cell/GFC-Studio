$connString = "Server=(localdb)\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;Encrypt=False;"
try {
    $conn = New-Object Microsoft.Data.SqlClient.SqlConnection($connString)
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM Members"
    $count = $cmd.ExecuteScalar()
    "Member Count: $count" | Out-File -FilePath "db_status.txt"
    $conn.Close()
} catch {
    $_.Exception.Message | Out-File -FilePath "db_status.txt"
}
