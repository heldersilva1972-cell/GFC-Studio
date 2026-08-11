$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$tcmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, Name, SalePrice, CreditValue FROM PosTokens", $connection)
$tr = $tcmd.ExecuteReader()
while ($tr.Read()) {
    Write-Host ("PosToken: Id={0} | Name='{1}' | SalePrice={2} | CreditValue={3}" -f $tr["Id"], $tr["Name"], $tr["SalePrice"], $tr["CreditValue"])
}
$tr.Close()

$connection.Close()
