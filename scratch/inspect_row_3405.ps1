$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT * FROM BarSaleEntries WHERE Id = 3405"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

if ($r.Read()) {
    for ($i = 0; $i -lt $r.FieldCount; $i++) {
        Write-Host "$($r.GetName($i)): $($r.GetValue($i))"
    }
}

$r.Close()
$connection.Close()
