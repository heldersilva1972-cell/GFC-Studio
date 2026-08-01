$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT TOP 10 
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
ORDER BY ISNULL(AdjustedSaleDate, SaleDate) DESC;
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

Write-Host "=========================================================================="
Write-Host "TOP 10 MOST RECENT BAR SALE ENTRIES IN DATABASE"
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

$reader.Close()
$connection.Close()
