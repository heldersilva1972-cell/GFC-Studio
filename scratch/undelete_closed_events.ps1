$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "UPDATE dbo.ActiveEvents SET IsDeleted = 0 WHERE Id = 50 OR Status = 1;"
$rowsAffected = $cmd.ExecuteNonQuery()
Write-Output "Updated rows: $rowsAffected"

$cmd.CommandText = "SELECT Id, Name, Status, IsDeleted, CurrentBalance, InitialAmount, RecipientEventName, CreatedAt FROM ActiveEvents"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
$dt | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
