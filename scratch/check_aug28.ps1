$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()

Write-Host "=== POS Z-REPORTS for 2026-08-28 ==="
$cmd.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, CashTotal, TotalGrossSales, ShiftType FROM PosZReports WHERE CAST(Timestamp AS DATE) = '2026-08-28' ORDER BY Timestamp DESC"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host ("ID: {0} | Time: {1} | Bartender: {2} | Cash: {3:C} | Gross: {4:C} | Shift: {5}" -f $reader["Id"], $reader["Timestamp"], $reader["BartenderName"], $reader["CashTotal"], $reader["TotalGrossSales"], $reader["ShiftType"])
}
$reader.Close()

Write-Host "`n=== BAR SALE ENTRIES for 2026-08-28 ==="
$cmd.CommandText = "SELECT Id, SaleDate, AdjustedSaleDate, Shift, TotalSales, TotalHours, EmployeeUsername, CreatedBy, Status FROM BarSaleEntries WHERE SaleDate = '2026-08-28' OR AdjustedSaleDate = '2026-08-28'"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host ("Id: {0} | SaleDate: {1:yyyy-MM-dd} | Shift: {2} | TotalSales: {3:C} | TotalHours: {4} | User: {5} | Status: {6}" -f $reader["Id"], $reader["SaleDate"], $reader["Shift"], $reader["TotalSales"], $reader["TotalHours"], $reader["EmployeeUsername"], $reader["Status"])
}
$reader.Close()
$conn.Close()
