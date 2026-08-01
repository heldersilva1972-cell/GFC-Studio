$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT TOP 3 Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, SalesSummaryJson FROM PosZReports ORDER BY Timestamp DESC"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

while ($reader.Read()) {
    Write-Host "--- $($reader['BartenderName']) ($($reader['Timestamp'])) Gross: $($reader['TotalGrossSales']) ---"
    Write-Host $reader["SalesSummaryJson"]
}

$reader.Close()
$connection.Close()
