$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$sql = @"
-- STEP 1: Add ItemTotalsJson column if missing
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('PosZReports') 
      AND name = 'ItemTotalsJson'
)
BEGIN
    ALTER TABLE PosZReports 
    ADD ItemTotalsJson NVARCHAR(MAX) NOT NULL DEFAULT '{}';
    PRINT 'Added ItemTotalsJson column to PosZReports.';
END;

-- STEP 2: Backfill historical PosZReports with ItemTotalsJson aggregated from PosSales
WITH ShiftWindows AS (
    SELECT 
        z.Id AS ZReportId,
        z.TerminalName,
        z.Timestamp AS EndTime,
        ISNULL(
            (SELECT MAX(z2.Timestamp) 
             FROM PosZReports z2 
             WHERE z2.TerminalName = z.TerminalName AND z2.Timestamp < z.Timestamp),
            DATEADD(day, -1, z.Timestamp)
        ) AS StartTime
    FROM PosZReports z
),
ParsedItemSales AS (
    SELECT 
        sw.ZReportId,
        j.ItemName,
        SUM(j.ItemTotal) AS TotalAmount
    FROM ShiftWindows sw
    JOIN PosSales s 
      ON s.Timestamp > sw.StartTime 
     AND s.Timestamp <= sw.EndTime
     AND (s.IsVoided = 0 OR s.IsVoided IS NULL)
     AND (s.PaymentType IS NULL OR s.PaymentType <> 'PAYOUT')
    CROSS APPLY OPENJSON(s.ItemsJson) WITH (
        ItemName NVARCHAR(200) '$.Name',
        Price DECIMAL(18,2) '$.Price',
        Quantity INT '$.Quantity'
    ) AS j
    WHERE j.ItemName IS NOT NULL
      AND j.ItemName NOT LIKE 'TAB DEPOSIT:%'
      AND j.ItemName NOT LIKE 'INITIAL DEPOSIT:%'
      AND j.ItemName NOT LIKE 'DEPOSIT CORRECTION:%'
      AND j.ItemName NOT LIKE 'RETURNED FUNDS:%'
    GROUP BY sw.ZReportId, j.ItemName
),
JsonAggregated AS (
    SELECT 
        ZReportId,
        '{' + STRING_AGG('"' + STRING_ESCAPE(ItemName, 'json') + '":' + CAST(TotalAmount AS NVARCHAR(50)), ',') + '}' AS ComputedItemTotalsJson
    FROM (
        SELECT 
            ZReportId, 
            ItemName, 
            SUM(TotalAmount) AS TotalAmount
        FROM ParsedItemSales
        GROUP BY ZReportId, ItemName
    ) AS Sub
    GROUP BY ZReportId
)
UPDATE z
SET z.ItemTotalsJson = ja.ComputedItemTotalsJson
FROM PosZReports z
JOIN JsonAggregated ja ON z.Id = ja.ZReportId;

PRINT 'Completed production backfill of historical Z-Reports.';
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($sql, $connection)
$cmd.CommandTimeout = 120
$cmd.ExecuteNonQuery()

Write-Host "SQL execution complete."

$connection.Close()
