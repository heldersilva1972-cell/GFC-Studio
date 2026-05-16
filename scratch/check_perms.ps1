$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$query = @"
SELECT p.PageId, p.PageName, p.PageRoute, u.Username, upp.CanAccess
FROM AppPages p
LEFT JOIN UserPagePermissions upp ON p.PageId = upp.PageId
LEFT JOIN AppUsers u ON upp.UserId = u.UserId
WHERE p.PageRoute LIKE '%pos%' OR p.PageName LIKE '%pos%'
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
