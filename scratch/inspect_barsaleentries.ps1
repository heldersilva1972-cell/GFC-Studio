$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;Connect Timeout=30;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM BarSaleEntries WHERE Id IN (3492, 3493)"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "BarSaleEntry ID: $($reader['Id'])"
    Write-Host "  SaleDate: $($reader['SaleDate']) | Shift: $($reader['Shift']) | User: $($reader['EmployeeUsername'])"
    Write-Host "  TotalSales: $($reader['TotalSales']) | TotalHours: $($reader['TotalHours']) | Status: $($reader['Status'])"
    Write-Host "  Notes: $($reader['Notes']) | CreatedBy: $($reader['CreatedBy'])"
}
$reader.Close()
$conn.Close()
