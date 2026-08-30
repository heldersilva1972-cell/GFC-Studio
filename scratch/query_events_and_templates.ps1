$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

# 1. Query all ActiveEvents
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, Status, IsDeleted, CurrentBalance, InitialAmount, RecipientEventName, DonatedItemIdsJson, DonatedItemTalliesJson, CreatedAt FROM ActiveEvents ORDER BY Id DESC"
$dtActive = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dtActive) | Out-Null
Write-Output "--- ActiveEvents ($($dtActive.Rows.Count)) ---"
$dtActive | Format-Table -AutoSize | Out-String | Write-Output

# 2. Query all EventTemplates
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, IsDeleted, RecipientEventName, DonatedItemIdsJson, DonatedItemTalliesJson, Enable100PercentDonatedProceeds FROM EventTemplates ORDER BY Id DESC"
$dtTmpl = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dtTmpl) | Out-Null
Write-Output "--- EventTemplates ($($dtTmpl.Rows.Count)) ---"
$dtTmpl | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
