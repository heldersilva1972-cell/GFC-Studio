$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT SalesSummaryJson FROM PosZReports WHERE Id = '575c3c9f-c7e6-46ba-8555-a6b29621d361'"
$val = $cmd.ExecuteScalar()
Write-Host "Contains PAYOUT:FOOD? " ($val.Contains("PAYOUT:FOOD"))
$conn.Close()
