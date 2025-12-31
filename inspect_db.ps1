$connString = "Server=(localdb)\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;Encoding=UTF8"
$query = "SELECT Id, Name, SerialNumber, IpAddress FROM Controllers"

$connection = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = $query
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
    $dataset = New-Object System.Data.DataSet
    $adapter.Fill($dataset)
    $dataset.Tables[0] | Format-Table | Out-String | Write-Host
} catch {
    Write-Host "Error: $($_.Exception.Message)"
} finally {
    $connection.Close()
}
