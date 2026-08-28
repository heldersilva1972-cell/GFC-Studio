$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT TOP 5 Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, TokenCredits, SalesSummaryJson FROM PosZReports ORDER BY Timestamp DESC"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

while ($reader.Read()) {
    $id = $reader["Id"]
    $term = $reader["TerminalName"]
    $bartender = $reader["BartenderName"]
    $ts = $reader["Timestamp"]
    $gross = $reader["TotalGrossSales"]
    $cash = $reader["CashTotal"]
    $tokens = $reader["TokenCredits"]
    $json = $reader["SalesSummaryJson"]

    Write-Host "ID: $id | Term: $term | User: $bartender | Time: $ts | Gross: $gross | Cash: $cash | TokenCredits: $tokens"
    Write-Host "SalesSummaryJson: $json"
    Write-Host "------------------------------------"
}

$reader.Close()
$connection.Close()
