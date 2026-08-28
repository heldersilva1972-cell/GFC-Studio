$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$cmd = New-Object System.Data.SqlClient.SqlCommand("SELECT TOP 1 * FROM PosSales", $connection)
$reader = $cmd.ExecuteReader()

for ($i = 0; $i -lt $reader.FieldCount; $i++) {
    Write-Host $reader.GetName($i)
}

$reader.Close()
$connection.Close()
