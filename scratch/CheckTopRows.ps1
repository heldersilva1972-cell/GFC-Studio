$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT TOP 5 Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal FROM PosZReports ORDER BY Timestamp DESC"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

while ($reader.Read()) {
    Write-Host "Id: "$reader["Id"] " | Time: "$reader["Timestamp"] " | Bartender: "$reader["BartenderName"] " | Gross: "$reader["TotalGrossSales"]
}

$reader.Close()
$connection.Close()
