$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Audit today's latest Z-report specifically for Downstairs Bar POS
$zQuery = "SELECT TOP 1 Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal FROM PosZReports WHERE TerminalName LIKE '%Downstairs%' ORDER BY Timestamp DESC"
$zCmd = New-Object System.Data.SqlClient.SqlCommand($zQuery, $connection)
$zReader = $zCmd.ExecuteReader()

$id = ""
$zTime = [DateTime]::MinValue
$term = ""
$bart = ""
$storedGross = 0.0
if ($zReader.Read()) {
    $id = $zReader["Id"].ToString()
    $zTime = [DateTime]$zReader["Timestamp"]
    $term = $zReader["TerminalName"].ToString()
    $bart = $zReader["BartenderName"].ToString()
    $storedGross = [double]$zReader["TotalGrossSales"]
}
$zReader.Close()

Write-Host "=========================================================================="
Write-Host "LATEST DOWNSTAIRS Z-REPORT: $zTime | Bartender: $bart | Terminal: $term"
Write-Host "Stored Z-Report Gross Total: $($storedGross.ToString('C'))"
Write-Host "=========================================================================="

# Find previous Z-report time for this terminal
$prevQuery = "SELECT TOP 1 Timestamp FROM PosZReports WHERE TerminalName = '$term' AND Timestamp < '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp DESC"
$prevCmd = New-Object System.Data.SqlClient.SqlCommand($prevQuery, $connection)
$prevTimeObj = $prevCmd.ExecuteScalar()
$prevTime = [DateTime]$prevTimeObj

Write-Host "Previous Z-Report Close Time: $prevTime"

# Query transactions
$salesQuery = "SELECT Id, Timestamp, BartenderName, PaymentType, TotalAmount FROM PosSales WHERE TerminalName = '$term' AND Timestamp <= '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' AND Timestamp > '$($prevTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp ASC"
$sCmd = New-Object System.Data.SqlClient.SqlCommand($salesQuery, $connection)
$sReader = $sCmd.ExecuteReader()

$count = 0
$sum = 0.0
$first = ""
$last = ""

while ($sReader.Read()) {
    $count++
    $tTime = $sReader["Timestamp"].ToString()
    $amt = [double]$sReader["TotalAmount"]
    if ($count -eq 1) { $first = $tTime }
    $last = $tTime
    $sum += $amt
}
$sReader.Close()

Write-Host "ACTUAL POS TRANSACTIONS RING UP BETWEEN $prevTime AND $zTime :"
Write-Host "  - Transaction Count:          $count"
Write-Host "  - First Transaction Ring Up:  $first"
Write-Host "  - Last Transaction Ring Up:   $last"
Write-Host "  - SUM OF ALL SHIFT SALES:     $($sum.ToString('C'))"

$connection.Close()
