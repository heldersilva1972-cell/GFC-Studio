$conn = New-Object System.Data.SqlClient.SqlConnection("Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;")
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT MemberID, FirstName, LastName, Suffix FROM Members WHERE LastName LIKE '%Cloutman%'"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    $id = $reader["MemberID"]
    $first = $reader["FirstName"]
    $last = $reader["LastName"]
    $suffix = $reader["Suffix"]
    Write-Output "ID: $id | First: $first | Last: $last | Suffix: $suffix"
}
$conn.Close()
