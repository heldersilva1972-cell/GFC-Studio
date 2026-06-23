$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, Name, CurrentPrice, CasePrice, PackSize, IsBeer, InventoryTrackType FROM LiquorItems WHERE Id = 8"
$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()
while ($reader.Read()) {
    $id = $reader["Id"]
    $name = $reader["Name"]
    $price = $reader["CurrentPrice"]
    $casePrice = $reader["CasePrice"]
    $packSize = $reader["PackSize"]
    $isBeer = $reader["IsBeer"]
    $track = $reader["InventoryTrackType"]
    Write-Host "ID: $id | Name: $name | Price: $price | CasePrice: $casePrice | PackSize: $packSize | IsBeer: $isBeer | Track: $track"
}
$reader.Close()
$connection.Close()
