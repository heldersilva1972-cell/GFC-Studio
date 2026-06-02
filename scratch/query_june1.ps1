$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT ShiftDate, ShiftType, StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels, ShiftSalesActivity, ShiftPayoutsActivity, ShiftCancelsActivity, EnvelopeAmount, BagRefillAmount, BackupBagAmount, ExpectedCash, Variance FROM LotteryShifts WHERE CAST(ShiftDate as Date) = '2026-06-01'"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

while ($r.Read()) {
    [PSCustomObject]@{
        Date     = $r['ShiftDate']
        Type     = $r['ShiftType']
        Start    = $r['StartingCash']
        End      = $r['EndingCash']
        Sales    = $r['TotalSales']
        Payouts  = $r['TotalPayouts']
        Cancels  = $r['TotalCancels']
        SalesAct = $r['ShiftSalesActivity']
        PayAct   = $r['ShiftPayoutsActivity']
        CanAct   = $r['ShiftCancelsActivity']
        Env      = $r['EnvelopeAmount']
        Refill   = $r['BagRefillAmount']
        Bag      = $r['BackupBagAmount']
        Expected = $r['ExpectedCash']
        Var      = $r['Variance']
    } | Format-List
}
$r.Close()
$connection.Close()
