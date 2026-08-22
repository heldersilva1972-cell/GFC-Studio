$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT DISTINCT CreatedBy, COUNT(*) as Cnt FROM LotteryWeeklyStats GROUP BY CreatedBy"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

Write-Host "CreatedBy Values in LotteryWeeklyStats:"
while ($r.Read()) {
    Write-Host ("CreatedBy: '{0}' | Count: {1}" -f $r["CreatedBy"], $r["Cnt"])
}
$r.Close()
$connection.Close()
