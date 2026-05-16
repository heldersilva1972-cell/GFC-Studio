$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$query = @"
SELECT u.UserId, u.Username, u.IsAdmin, u.IsActive, kc.CardNumber
FROM AppUsers u
LEFT JOIN KeyCards kc ON u.MemberId = kc.MemberId AND kc.IsActive = 1
WHERE u.IsActive = 1
ORDER BY u.Username
"@

$connection = New-Object System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$connection.Open()

$command = $connection.CreateCommand()
$command.CommandText = $query
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter $command
$dataset = New-Object System.Data.DataSet
$adapter.Fill($dataset) | Out-Null

$connection.Close()

$dataset.Tables[0] | Format-Table -AutoSize
