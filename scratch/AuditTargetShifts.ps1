$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$targetIds = @(
    "3CE358C6-BA8B-4BF4-AFEE-E2C547E3B720",
    "BEF56E2D-29A0-44BB-A614-D59375BFAE74",
    "C8C85795-727F-4330-B847-F652A6CB9C4F",
    "C53B754D-AE85-4A4F-B1ED-A854542D6A7E"
)

foreach ($id in $targetIds) {
    Write-Host "=========================================================================="
    Write-Host "AUDITING SHIFT FOR Z-REPORT ID: $id"
    Write-Host "=========================================================================="
    
    # Get Z-Report details
    $zQuery = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Id = '$id'"
    $zCmd = New-Object System.Data.SqlClient.SqlCommand($zQuery, $connection)
    $zReader = $zCmd.ExecuteReader()
    
    $zTime = $null
    $term = ""
    $bart = ""
    $json = ""
    if ($zReader.Read()) {
        $zTime = [DateTime]$zReader["Timestamp"]
        $term = $zReader["TerminalName"]
        $bart = $zReader["BartenderName"]
        $json = $zReader["SalesSummaryJson"].ToString()
    }
    $zReader.Close()
    
    # Get Prev ZTime
    $prevQuery = "SELECT TOP 1 Timestamp FROM PosZReports WHERE TerminalName = '$term' AND Timestamp < '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp DESC"
    $prevCmd = New-Object System.Data.SqlClient.SqlCommand($prevQuery, $connection)
    $prevTimeObj = $prevCmd.ExecuteScalar()
    $prevTime = if ($prevTimeObj -and $prevTimeObj -ne [DBNull]::Value) { [DateTime]$prevTimeObj } else { $null }
    
    Write-Host "Z-Report Time: $zTime | Bartender: $bart"
    Write-Host "Previous Z-Report Time: $(if($prevTime){ $prevTime.ToString() } else { 'NONE (Start of DB)' })"
    
    # Query transactions between PrevZTime and ZTime
    $salesQuery = @"
SELECT Id, Timestamp, BartenderName, PaymentType, TotalAmount, ItemsJson 
FROM PosSales 
WHERE TerminalName = '$term' 
  AND Timestamp <= '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))'
  $(if($prevTime){ "AND Timestamp > '$($prevTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))'" })
ORDER BY Timestamp ASC
"@
    
    $sCmd = New-Object System.Data.SqlClient.SqlCommand($salesQuery, $connection)
    $sReader = $sCmd.ExecuteReader()
    
    $txCount = 0
    $firstTx = $null
    $lastTx = $null
    $totalSum = 0.0m
    $cashSum = 0.0m
    
    while ($sReader.Read()) {
        $txCount++
        $tTime = [DateTime]$sReader["Timestamp"]
        $amt = [decimal]$sReader["TotalAmount"]
        $pay = $sReader["PaymentType"].ToString()
        
        if ($txCount -eq 1) { $firstTx = $tTime }
        $lastTx = $tTime
        $totalSum += $amt
        if ($pay -eq "CASH") { $cashSum += $amt }
    }
    $sReader.Close()
    
    Write-Host "TRANSACTION AUDIT RESULTS:"
    Write-Host "  - Total Transactions Ring Up: $txCount"
    Write-Host "  - First Transaction Time:      $firstTx"
    Write-Host "  - Last Transaction Time:       $lastTx"
    Write-Host "  - Exact Sum of All Ring-Ups:   $($totalSum.ToString('C'))"
    Write-Host "  - Exact Sum of Cash Payments:  $($cashSum.ToString('C'))"
}

$connection.Close()
