$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT 
    z.Id AS ZId,
    z.Timestamp,
    z.TerminalName,
    z.BartenderName,
    z.TotalGrossSales AS ZSales,
    b.Id AS BarEntryId,
    b.SaleDate,
    b.AdjustedSaleDate,
    b.Shift,
    b.TotalSales AS BarSales,
    b.Status
FROM PosZReports z
LEFT JOIN BarSaleEntries b 
    ON CAST(ISNULL(b.AdjustedSaleDate, b.SaleDate) AS DATE) = CAST(
        CASE 
            WHEN DATEPART(HOUR, z.Timestamp) < 5 THEN DATEADD(DAY, -1, z.Timestamp)
            ELSE z.Timestamp
        END AS DATE)
   AND b.Shift = CASE WHEN DATEPART(HOUR, z.Timestamp) >= 5 AND DATEPART(HOUR, z.Timestamp) < 19 THEN 'Day' ELSE 'Night' END
   AND b.IsRentalHall = 0
WHERE z.Timestamp >= '2026-08-01'
ORDER BY z.Timestamp
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

while ($r.Read()) {
    $zTime = $r["Timestamp"]
    $bart = $r["BartenderName"]
    $zSales = $r["ZSales"]
    $barId = $r["BarEntryId"]
    $sDate = $r["SaleDate"]
    $shift = $r["Shift"]
    $bSales = $r["BarSales"]
    $status = $r["Status"]

    Write-Host ("ZReport: {0} | {1,-12} | ZSales: {2,7:C2} || BarEntry: Id={3,-4} | Date={4:yyyy-MM-dd} | Shift={5,-5} | BarSales={6,7:C2} | Status={7}" -f $zTime, $bart, $zSales, $barId, $sDate, $shift, $bSales, $status)
}

$r.Close()
$connection.Close()
