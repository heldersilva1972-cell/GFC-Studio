$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT oi.OrderId, oi.LiquorItemId, oi.Quantity, oi.UnitPriceAtTimeOfOrder, i.Name as ItemName
FROM LiquorOrderItems oi
JOIN LiquorItems i ON oi.LiquorItemId = i.Id
WHERE oi.OrderId IN (1, 2, 5, 8)
"@

$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()

Write-Host "Order Items for Orders 1, 2, 5, 8:"
while ($reader.Read()) {
    $orderId = $reader["OrderId"]
    $itemId = $reader["LiquorItemId"]
    $qty = $reader["Quantity"]
    $price = $reader["UnitPriceAtTimeOfOrder"]
    $name = $reader["ItemName"]
    Write-Host "Order: $orderId | Item: $name (ID: $itemId) | Qty: $qty | Unit Price: $price"
}

$reader.Close()
$connection.Close()
