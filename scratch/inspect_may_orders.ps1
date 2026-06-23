$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT o.Id, o.OrderDate, o.TotalCost, o.ItemsTotal, o.TaxAmount, o.AdditionalCosts, o.Status, v.Name as VendorName
FROM LiquorOrders o
LEFT JOIN LiquorVendors v ON o.VendorId = v.Id
WHERE o.OrderDate >= '2026-05-01' AND o.OrderDate < '2026-06-01'
"@

$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()

Write-Host "Orders for May 2026:"
while ($reader.Read()) {
    $id = $reader["Id"]
    $date = $reader["OrderDate"]
    $totalCost = $reader["TotalCost"]
    $itemsTotal = $reader["ItemsTotal"]
    $tax = $reader["TaxAmount"]
    $add = $reader["AdditionalCosts"]
    $status = $reader["Status"]
    $vendor = $reader["VendorName"]
    Write-Host "ID: $id | Date: $date | Status: $status | Vendor: $vendor | Total: $totalCost | Items: $itemsTotal | Tax: $tax | Add: $add"
}

$reader.Close()
$connection.Close()
