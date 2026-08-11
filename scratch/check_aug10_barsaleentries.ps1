$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, SaleDate, AdjustedSaleDate, Shift, TotalSales, TotalHours, CreatedBy, EmployeeUsername, IsRentalHall, BarLocation FROM BarSaleEntries WHERE SaleDate >= '2026-08-09' AND SaleDate <= '2026-08-12' ORDER BY SaleDate, Shift"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

while ($r.Read()) {
    Write-Host "=========================================================================="
    Write-Host "Id: $($r['Id'])"
    Write-Host "SaleDate: $($r['SaleDate'])"
    Write-Host "AdjustedSaleDate: $($r['AdjustedSaleDate'])"
    Write-Host "Shift: $($r['Shift'])"
    Write-Host "TotalSales: $($r['TotalSales'])"
    Write-Host "TotalHours: $($r['TotalHours'])"
    Write-Host "CreatedBy: $($r['CreatedBy'])"
    Write-Host "EmployeeUsername: $($r['EmployeeUsername'])"
    Write-Host "IsRentalHall: $($r['IsRentalHall'])"
    Write-Host "BarLocation: $($r['BarLocation'])"
}

$r.Close()
$connection.Close()
