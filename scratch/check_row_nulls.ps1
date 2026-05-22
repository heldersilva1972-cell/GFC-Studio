$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, 
                 CASE WHEN TerminalName IS NULL THEN 'TerminalName' ELSE '' END +
                 CASE WHEN BartenderName IS NULL THEN ' BartenderName' ELSE '' END +
                 CASE WHEN InventoryPullsJson IS NULL THEN ' InventoryPullsJson' ELSE '' END +
                 CASE WHEN SalesSummaryJson IS NULL THEN ' SalesSummaryJson' ELSE '' END +
                 CASE WHEN BanquetSummaryJson IS NULL THEN ' BanquetSummaryJson' ELSE '' END as NullColumns
          FROM PosZReports"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

while ($reader.Read()) {
    $nullCols = $reader['NullColumns'].ToString().Trim()
    if ($nullCols) {
        Write-Host "Row with Id $($reader['Id']) has NULL in: $nullCols"
    }
}

$reader.Close()
$connection.Close()
