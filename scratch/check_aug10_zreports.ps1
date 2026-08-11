$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Timestamp >= '2026-08-09 00:00:00' AND Timestamp <= '2026-08-12 23:59:59' ORDER BY Timestamp"
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
}

$reader.Close()

# Also check PosSales count and timestamps for Aug 10 & 11
$scmd = New-Object System.Data.SqlClient.SqlCommand("SELECT COUNT(*) AS Cnt, MIN(Timestamp) AS MinTime, MAX(Timestamp) AS MaxTime FROM PosSales WHERE Timestamp >= '2026-08-10 00:00:00'", $connection)
$sr = $scmd.ExecuteReader()
if ($sr.Read()) {
    Write-Host "=========================================================================="
    Write-Host "PosSales since Aug 10 count: $($sr['Cnt']) | Min: $($sr['MinTime']) | Max: $($sr['MaxTime'])"
}
$sr.Close()

$connection.Close()
