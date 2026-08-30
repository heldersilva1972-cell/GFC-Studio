$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, Enable100PercentDonatedProceeds, DonatedItemIdsJson, RecipientEventName, CurrentBalance, InitialAmount FROM ActiveEvents WHERE Id = 50"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
$dt | Format-Table -AutoSize | Out-String | Write-Output

$cmd.CommandText = "SELECT Id, Name, Enable100PercentDonatedProceeds, DonatedItemIdsJson, RecipientEventName FROM EventTemplates WHERE Id = 3"
$dtTmpl = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dtTmpl) | Out-Null
$dtTmpl | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
