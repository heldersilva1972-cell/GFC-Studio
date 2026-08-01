$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT 
    Id, 
    SaleDate, 
    AdjustedSaleDate, 
    Shift, 
    TotalSales, 
    OriginalTotalSales, 
    TotalHours, 
    CreatedBy, 
    ModifiedBy, 
    ModifiedDate
FROM BarSaleEntries
WHERE CAST(ISNULL(AdjustedSaleDate, SaleDate) AS DATE) = '2026-08-01'
ORDER BY SaleDate DESC;
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

Write-Host "=========================================================================="
Write-Host "BAR SALE ENTRIES FOR AUGUST 1, 2026"
Write-Host "=========================================================================="

$count = 0
while ($reader.Read()) {
    $count++
    $amt = [decimal]$reader["TotalSales"]
    Write-Host "Row ${count}:"
    Write-Host "  Id:                 "$reader["Id"]
    Write-Host "  SaleDate:           "$reader["SaleDate"]
    Write-Host "  AdjustedSaleDate:   "$reader["AdjustedSaleDate"]
    Write-Host "  Shift:              "$reader["Shift"]
    Write-Host "  TotalSales:         $($amt.ToString('C'))"
    Write-Host "  OriginalTotalSales: "$reader["OriginalTotalSales"]
    Write-Host "  TotalHours:         "$reader["TotalHours"]
    Write-Host "  CreatedBy:          "$reader["CreatedBy"]
    Write-Host "  ModifiedBy:         "$reader["ModifiedBy"]
    Write-Host "  ModifiedDate:       "$reader["ModifiedDate"]
    Write-Host "--------------------------------------------------------------------------"
}

if ($count -eq 0) {
    Write-Host "No records found in BarSaleEntries for August 1, 2026."
}

$reader.Close()
$connection.Close()
