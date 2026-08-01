$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
WITH ZRanges AS (
    SELECT 
        z.Id,
        z.TerminalName,
        z.BartenderName,
        z.Timestamp AS ZTime,
        z.TotalGrossSales AS StoredGross,
        z.CashTotal AS StoredCash,
        LAG(z.Timestamp) OVER (PARTITION BY z.TerminalName ORDER BY z.Timestamp) AS PrevZTime
    FROM PosZReports z
),
CalculatedShiftTotals AS (
    SELECT 
        r.Id,
        r.TerminalName,
        r.BartenderName,
        r.ZTime,
        r.StoredGross,
        r.StoredCash,
        ISNULL(SUM(s.TotalAmount), 0) AS ActualSalesGross,
        ISNULL(SUM(CASE WHEN s.PaymentType = 'CASH' THEN s.TotalAmount ELSE 0 END), 0) AS ActualSalesCash,
        COUNT(s.Id) AS TxCount
    FROM ZRanges r
    LEFT JOIN PosSales s 
        ON s.TerminalName = r.TerminalName 
       AND s.Timestamp <= r.ZTime 
       AND (r.PrevZTime IS NULL OR s.Timestamp > r.PrevZTime)
    GROUP BY r.Id, r.TerminalName, r.BartenderName, r.ZTime, r.StoredGross, r.StoredCash
)
SELECT * FROM CalculatedShiftTotals
WHERE TxCount > 0 AND ABS(StoredGross - ActualSalesGross) > 0.01
ORDER BY ZTime DESC;
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

Write-Host "=========================================================================="
Write-Host "EXACT PosZReports ROWS THAT WILL BE UPDATED BY THE FIX SCRIPT"
Write-Host "=========================================================================="

$count = 0
while ($reader.Read()) {
    $count++
    $id = $reader["Id"]
    $time = $reader["ZTime"]
    $bart = $reader["BartenderName"]
    $storedG = [decimal]$reader["StoredGross"]
    $actualG = [decimal]$reader["ActualSalesGross"]
    $storedC = [decimal]$reader["StoredCash"]
    $actualC = [decimal]$reader["ActualSalesCash"]
    $txCount = $reader["TxCount"]
    
    Write-Host "Row ${count}:"
    Write-Host "  Date / Time:   $time"
    Write-Host "  Bartender:     $bart"
    Write-Host "  Z-Report ID:   $id"
    Write-Host "  Sales Count:   $txCount transactions ring up in this shift"
    Write-Host "  Gross Sales:   $($storedG.ToString('C'))  ---> CHANGING TO --->  $($actualG.ToString('C'))"
    Write-Host "  Cash Total:    $($storedC.ToString('C'))  ---> CHANGING TO --->  $($actualC.ToString('C'))"
    Write-Host "--------------------------------------------------------------------------"
}

if ($count -eq 0) {
    Write-Host "No active shift Z-reports need changes."
}

$reader.Close()
$connection.Close()
