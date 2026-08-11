$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$cmd = $connection.CreateCommand()
$cmd.CommandText = "DELETE FROM dbo.LotteryShifts WHERE ShiftId = 2314"
$rows = $cmd.ExecuteNonQuery()
Write-Host "Removed $rows test row (ShiftId 2314)."

$connection.Close()
