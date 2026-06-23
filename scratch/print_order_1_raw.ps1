$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT oi.OrderId, oi.Quantity, oi.UnitPriceAtTimeOfOrder, oi.CasePriceAtTimeOfOrder, oi.BottleFeeAtTimeOfOrder, i.Name, i.PackSize, i.IsBeer
FROM LiquorOrderItems oi
JOIN LiquorItems i ON oi.LiquorItemId = i.Id
WHERE oi.OrderId = 1
"@

$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()

while ($reader.Read()) {
    $name = $reader["Name"]
    $qty = $reader["Quantity"]
    $unitPrice = $reader["UnitPriceAtTimeOfOrder"]
    $casePrice = $reader["CasePriceAtTimeOfOrder"]
    $fee = $reader["BottleFeeAtTimeOfOrder"]
    $packSize = $reader["PackSize"]
    $isBeer = $reader["IsBeer"]
    
    Write-Host "Item: $name"
    Write-Host "  Quantity: $qty (type: $($qty.GetType().Name))"
    Write-Host "  UnitPriceAtTimeOfOrder: $unitPrice (type: $($unitPrice.GetType().Name))"
    Write-Host "  CasePriceAtTimeOfOrder: $casePrice (type: $($casePrice.GetType().Name))"
    Write-Host "  BottleFeeAtTimeOfOrder: $fee (type: $($fee.GetType().Name))"
    Write-Host "  PackSize: $packSize (type: $($packSize.GetType().Name))"
    Write-Host "  IsBeer: $isBeer (type: $($isBeer.GetType().Name))"
}

$reader.Close()
$connection.Close()
