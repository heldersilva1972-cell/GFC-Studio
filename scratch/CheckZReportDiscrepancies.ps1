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
)
SELECT 
    r.Id,
    r.TerminalName,
    r.BartenderName,
    r.ZTime,
    r.StoredGross,
    r.StoredCash,
    ISNULL(SUM(s.TotalAmount), 0) AS CalculatedSalesGross,
    ISNULL(SUM(CASE WHEN s.PaymentType = 'CASH' THEN s.TotalAmount ELSE 0 END), 0) AS CalculatedSalesCash,
    COUNT(s.Id) AS SaleCount
FROM ZRanges r
LEFT JOIN PosSales s 
    ON s.TerminalName = r.TerminalName 
   AND s.Timestamp <= r.ZTime 
   AND (r.PrevZTime IS NULL OR s.Timestamp > r.PrevZTime)
GROUP BY r.Id, r.TerminalName, r.BartenderName, r.ZTime, r.StoredGross, r.StoredCash
HAVING ABS(r.StoredGross - ISNULL(SUM(s.TotalAmount), 0)) > 0.01
ORDER BY r.ZTime DESC;
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

Write-Host "=========================================================================="
Write-Host "Z-REPORTS WITH DISCREPANCIES BETWEEN STORED TOTALS & ACTUAL POS SALES"
Write-Host "=========================================================================="

$count = 0
while ($reader.Read()) {
    $count++
    $id = $reader["Id"]
    $term = $reader["TerminalName"]
    $bart = $reader["BartenderName"]
    $time = $reader["ZTime"]
    $storedG = [decimal]$reader["StoredGross"]
    $calcG = [decimal]$reader["CalculatedSalesGross"]
    $storedC = [decimal]$reader["StoredCash"]
    $calcC = [decimal]$reader["CalculatedSalesCash"]
    $salesCount = $reader["SaleCount"]
    
    Write-Host "[$count] ZReport: $id | $time | $term | Bartender: $bart"
    Write-Host "    - Stored Gross: $($storedG.ToString('C')) | True Sales Sum: $($calcG.ToString('C')) (Diff: $(($storedG - $calcG).ToString('C')))"
    Write-Host "    - Stored Cash:  $($storedC.ToString('C')) | True Cash Sum:  $($calcC.ToString('C'))"
    Write-Host "    - Associated Transactions Count: $salesCount"
    Write-Host "--------------------------------------------------------------------------"
}

if ($count -eq 0) {
    Write-Host "No discrepancies found! All Z-report stored totals match the actual sales transaction sums."
}

$reader.Close()
$connection.Close()
