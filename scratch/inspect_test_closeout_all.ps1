$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== 1. TEST Z-REPORT DETAILS ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM PosZReports WHERE Id = '5033a055-4376-485a-905b-e20d12a04eea'"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "ID: $($reader['Id'])"
    Write-Host "Time: $($reader['Timestamp'])"
    Write-Host "User: $($reader['BartenderName'])"
    Write-Host "Gross: $($reader['TotalGrossSales']) | Cash: $($reader['CashTotal'])"
    Write-Host "HoursWorked: $($reader['HoursWorked'])"
    Write-Host "RecordSalesToBar: $($reader['RecordSalesToBar'])"
    Write-Host "PhysicalTokensJson: $($reader['PhysicalTokensJson'])"
    Write-Host "ShiftDrinkJson: $($reader['ShiftDrinkJson'])"
    Write-Host "InventoryPullsJson: $($reader['InventoryPullsJson'])"
    Write-Host "SalesSummaryJson: $($reader['SalesSummaryJson'])"
    Write-Host "ItemTotalsJson: $($reader['ItemTotalsJson'])"
}
$reader.Close()

Write-Host "`n=== 2. LIQUOR TRANSACTIONS AROUND TEST TIME ==="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = "SELECT * FROM LiquorTransactions WHERE TransactionDate >= '2026-09-17 23:30:00'"
$reader2 = $cmd2.ExecuteReader()
while ($reader2.Read()) {
    Write-Host "ID: $($reader2['Id']) | Date: $($reader2['TransactionDate']) | ItemId: $($reader2['ItemId']) | Type: $($reader2['TransactionType']) | Qty: $($reader2['Quantity']) | Reason: $($reader2['Reason'])"
}
$reader2.Close()

Write-Host "`n=== 3. BAR SALE ENTRIES FOR SEP 17 ==="
$cmd3 = $conn.CreateCommand()
$cmd3.CommandText = "SELECT * FROM BarSaleEntries WHERE SaleDate >= '2026-09-17'"
$reader3 = $cmd3.ExecuteReader()
while ($reader3.Read()) {
    Write-Host "ID: $($reader3['Id']) | Date: $($reader3['SaleDate']) | Shift: $($reader3['Shift']) | TotalSales: $($reader3['TotalSales']) | TotalHours: $($reader3['TotalHours']) | Status: $($reader3['Status']) | User: $($reader3['EmployeeUsername'])"
}
$reader3.Close()

$conn.Close()
