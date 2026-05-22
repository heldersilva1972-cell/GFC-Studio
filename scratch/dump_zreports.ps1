$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, TerminalName, BartenderName, CashTotal, TotalGrossSales, TokenCredits, BanquetSummaryJson, InventoryPullsJson, SalesSummaryJson FROM PosZReports"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

$i = 0
while ($reader.Read()) {
    $i++
    Write-Host "`n--- Row $i ---"
    Write-Host "Id: $($reader['Id'])"
    Write-Host "TerminalName: $($reader['TerminalName']) (IsNull: $($reader.IsDBNull(1)))"
    Write-Host "BartenderName: $($reader['BartenderName']) (IsNull: $($reader.IsDBNull(2)))"
    Write-Host "CashTotal: $($reader['CashTotal'])"
    Write-Host "TotalGrossSales: $($reader['TotalGrossSales'])"
    Write-Host "TokenCredits: $($reader['TokenCredits']) (IsNull: $($reader.IsDBNull(5)))"
    Write-Host "BanquetSummaryJson IsNull: $($reader.IsDBNull(6))"
    Write-Host "InventoryPullsJson IsNull: $($reader.IsDBNull(7))"
    Write-Host "SalesSummaryJson IsNull: $($reader.IsDBNull(8))"
}
$reader.Close()
$connection.Close()
