$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT TOP 10 ShiftDate, ShiftType, StartingCash, EndingCash, TotalSales, Variance FROM LotteryShifts ORDER BY ShiftDate DESC"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

while ($r.Read()) {
    [PSCustomObject]@{
        Date     = $r['ShiftDate']
        Type     = $r['ShiftType']
        Start    = $r['StartingCash']
        End      = $r['EndingCash']
        Sales    = $r['TotalSales']
        Var      = $r['Variance']
    } | Format-List
}
$r.Close()
$connection.Close()
