$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal FROM PosZReports WHERE Id = '3CE358C6-BA8B-4BF4-AFEE-E2C547E3B720'"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

if ($reader.Read()) {
    Write-Host "Id: " $reader["Id"]
    Write-Host "Timestamp: " $reader["Timestamp"]
    Write-Host "Bartender: " $reader["BartenderName"]
    Write-Host "Stored Gross: " $reader["TotalGrossSales"]
    Write-Host "Stored Cash: " $reader["CashTotal"]
} else {
    Write-Host "Row 3CE358C6... not found."
}

$reader.Close()
$connection.Close()
