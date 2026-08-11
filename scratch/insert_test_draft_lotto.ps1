$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$cmd = $connection.CreateCommand()
$cmd.CommandText = @"
INSERT INTO dbo.LotteryShifts (
    ShiftDate, EmployeeName, ShiftType, MachineId, StartingCash, EndingCash,
    TotalSales, TotalPayouts, TotalCancels, Commission, CashBonus, ClaimsBonus, NetDue,
    BackupBagAmount, EnvelopeAmount, BagRefillAmount, NetSales, ExpectedCash, Variance,
    LotteryIncome, NetIncome, ShiftSalesActivity, ShiftPayoutsActivity, ShiftCancelsActivity,
    ShiftNetDueActivity, Notes, Status, IsReconciled, CreatedBy, CreatedDate
) VALUES (
    '2026-08-10', 'MBrancaleone', 'Night', 1, 100.00, 250.00,
    500.00, 200.00, 10.00, 25.00, 0.00, 0.00, 290.00,
    0.00, 50.00, 0.00, 500.00, 250.00, 0.00,
    25.00, 25.00, 500.00, 200.00, 10.00,
    290.00, 'Test Draft Shift', 'Draft', 0, 'MBrancaleone', GETDATE()
);
"@
$rows = $cmd.ExecuteNonQuery()
Write-Host "Inserted $rows test draft row into LotteryShifts for Aug 10, 2026."

$connection.Close()
