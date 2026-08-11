$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Timestamp >= '2026-08-07 00:00:00' AND Timestamp <= '2026-08-09 23:59:59' ORDER BY Timestamp"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

while ($reader.Read()) {
    Write-Host "=========================================================================="
    Write-Host "Id: $($reader['Id'])"
    Write-Host "Terminal: $($reader['TerminalName'])"
    Write-Host "Bartender: $($reader['BartenderName'])"
    Write-Host "Timestamp: $($reader['Timestamp'])"
    Write-Host "TotalGrossSales: $($reader['TotalGrossSales'])"
    Write-Host "CashTotal: $($reader['CashTotal'])"
    Write-Host "SalesSummaryJson:"
    Write-Host $reader["SalesSummaryJson"]
}

$reader.Close()
$connection.Close()
