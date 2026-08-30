$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM ActiveEvents"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
Write-Output "=== ActiveEvents (Total: $($dt.Rows.Count)) ==="
$dt | Format-Table Id, Name, Status, IsDeleted, CurrentBalance, InitialAmount, RecipientEventName, CreatedAt -AutoSize | Out-String | Write-Output

$cmd.CommandText = "SELECT * FROM EventTemplates WHERE Name LIKE '%Family%'"
$dtTmpl = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dtTmpl) | Out-Null
Write-Output "=== EventTemplates Family Picnic ==="
$dtTmpl | Format-Table Id, Name, DefaultType, IsDeleted, RecipientEventName, DonatedItemIdsJson -AutoSize | Out-String | Write-Output

$conn.Close()
