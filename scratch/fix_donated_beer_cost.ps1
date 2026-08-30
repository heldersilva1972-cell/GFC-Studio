$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

# 1. Update ActiveEvent #50 to 0.00 balance and Enable100PercentDonatedProceeds = 1
$cmd = $conn.CreateCommand()
$cmd.CommandText = "UPDATE dbo.ActiveEvents SET CurrentBalance = 0.00, Enable100PercentDonatedProceeds = 1 WHERE Id = 50;"
$cmd.ExecuteNonQuery()

# 2. Update Template #3 (Horseshoes) Enable100PercentDonatedProceeds = 1
$cmd.CommandText = "UPDATE dbo.EventTemplates SET Enable100PercentDonatedProceeds = 1 WHERE Id = 3;"
$cmd.ExecuteNonQuery()

# 3. Update the settlement sale for Event 50 to TotalAmount = 0.00 and unit price = 0.00
$cmd.CommandText = @"
UPDATE dbo.PosSales 
SET TotalAmount = 0.00, 
    ItemsJson = '[{"CartItemId":"b7b06dc0-36ee-46cf-a031-623b8dca3c8e","Id":0,"Name":"TALLY: Bud Light","Price":0.00,"Quantity":1,"Category":"BEER","IsTokenApplied":false,"DisplayOrder":0,"Modifiers":[],"AssociatedCartItemId":null,"AppliedTokenId":null,"ZReportGroup":0}]'
WHERE ActiveEventId = 50;
"@
$cmd.ExecuteNonQuery()

Write-Output "Database updated successfully."

$cmd.CommandText = "SELECT Id, Name, CurrentBalance, Enable100PercentDonatedProceeds, DonatedItemIdsJson FROM ActiveEvents WHERE Id = 50"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
$dt | Format-Table -AutoSize | Out-String | Write-Output

$cmd.CommandText = "SELECT Id, ActiveEventId, TotalAmount, ItemsJson FROM PosSales WHERE ActiveEventId = 50"
$dtSales = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dtSales) | Out-Null
$dtSales | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
