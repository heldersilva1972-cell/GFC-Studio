$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;Connect Timeout=30;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=================================================================="
Write-Host "1. POS Z-REPORT (TEST CLOSEOUT)"
Write-Host "=================================================================="
$cmd1 = $conn.CreateCommand()
$cmd1.CommandTimeout = 30
$cmd1.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, ShiftType, CashTotal, TotalGrossSales, TokenCredits, HoursWorked, RecordSalesToBar, PhysicalTokensJson, ShiftDrinkJson, InventoryPullsJson, SalesSummaryJson, ItemTotalsJson FROM PosZReports WHERE Id = '5033a055-4376-485a-905b-e20d12a04eea' OR Timestamp >= '2026-09-17 23:40:00'"
$reader1 = $cmd1.ExecuteReader()
$foundZ = 0
while ($reader1.Read()) {
    $foundZ++
    Write-Host "ID: $($reader1['Id'])"
    Write-Host "Timestamp: $($reader1['Timestamp'])"
    Write-Host "Terminal: $($reader1['TerminalName'])"
    Write-Host "Bartender: $($reader1['BartenderName'])"
    Write-Host "ShiftType: $($reader1['ShiftType'])"
    Write-Host "GrossSales: $($reader1['TotalGrossSales'])"
    Write-Host "CashTotal: $($reader1['CashTotal'])"
    Write-Host "TokenCredits: $($reader1['TokenCredits'])"
    Write-Host "HoursWorked: $($reader1['HoursWorked'])"
    Write-Host "RecordSalesToBar: $($reader1['RecordSalesToBar'])"
    Write-Host "PhysicalTokensJson: $($reader1['PhysicalTokensJson'])"
    Write-Host "ShiftDrinkJson: $($reader1['ShiftDrinkJson'])"
    Write-Host "InventoryPullsJson: $($reader1['InventoryPullsJson'])"
    Write-Host "SalesSummaryJson: $($reader1['SalesSummaryJson'])"
    Write-Host "ItemTotalsJson: $($reader1['ItemTotalsJson'])"
    Write-Host "--------------------------------------------------"
}
$reader1.Close()
if ($foundZ -eq 0) { Write-Host "No Z-reports found in this window." }

Write-Host "`n=================================================================="
Write-Host "2. POS SALES (TEST TRANSACTIONS)"
Write-Host "=================================================================="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandTimeout = 30
$cmd2.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, PaymentType, TotalAmount, IsVoided, ItemsJson FROM PosSales WHERE Timestamp >= '2026-09-17 23:30:00' AND Timestamp <= '2026-09-18 01:00:00' ORDER BY Timestamp"
$reader2 = $cmd2.ExecuteReader()
$foundSales = 0
while ($reader2.Read()) {
    $foundSales++
    Write-Host "ID: $($reader2['Id']) | Time: $($reader2['Timestamp']) | User: $($reader2['BartenderName']) | Pay: $($reader2['PaymentType']) | Amount: $($reader2['TotalAmount']) | Void: $($reader2['IsVoided'])"
    Write-Host "Items: $($reader2['ItemsJson'])"
    Write-Host "--------------------------------------------------"
}
$reader2.Close()
if ($foundSales -eq 0) { Write-Host "No PosSales found in this window." }

Write-Host "`n=================================================================="
Write-Host "3. LIQUOR TRANSACTIONS (INVENTORY REMOVALS / CHECKOUTS)"
Write-Host "=================================================================="
$cmd3 = $conn.CreateCommand()
$cmd3.CommandTimeout = 30
$cmd3.CommandText = "SELECT Id, TransactionDate, ItemId, Quantity, TransactionType, Reason, Notes FROM LiquorTransactions WHERE TransactionDate >= '2026-09-17 23:30:00' ORDER BY TransactionDate"
$reader3 = $cmd3.ExecuteReader()
$foundLiquor = 0
while ($reader3.Read()) {
    $foundLiquor++
    Write-Host "ID: $($reader3['Id']) | Date: $($reader3['TransactionDate']) | ItemId: $($reader3['ItemId']) | Qty: $($reader3['Quantity']) | Type: $($reader3['TransactionType']) | Reason: $($reader3['Reason'])"
}
$reader3.Close()
if ($foundLiquor -eq 0) { Write-Host "No LiquorTransactions found." }

Write-Host "`n=================================================================="
Write-Host "4. BAR SALE ENTRIES (FINANCIAL / EMPLOYEE SHIFT ENTRIES)"
Write-Host "=================================================================="
$cmd4 = $conn.CreateCommand()
$cmd4.CommandTimeout = 30
$cmd4.CommandText = "SELECT Id, SaleDate, Shift, EmployeeUsername, TotalSales, TotalHours, Status, Notes, CreatedAt, ModifiedAt FROM BarSaleEntries WHERE SaleDate >= '2026-09-17' ORDER BY SaleDate DESC, Shift DESC"
$reader4 = $cmd4.ExecuteReader()
while ($reader4.Read()) {
    Write-Host "ID: $($reader4['Id']) | Date: $($reader4['SaleDate']) | Shift: $($reader4['Shift']) | User: $($reader4['EmployeeUsername']) | Sales: $($reader4['TotalSales']) | Hours: $($reader4['TotalHours']) | Status: $($reader4['Status']) | Created: $($reader4['CreatedAt'])"
}
$reader4.Close()

$conn.Close()
