$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM EventTemplates"
$dtTmpl = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dtTmpl) | Out-Null
Write-Output "--- EventTemplates ($($dtTmpl.Rows.Count)) ---"
$dtTmpl | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
