$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Timestamp, BartenderName, PaymentType, TotalAmount FROM PosSales WHERE BartenderName = 'JMelanson' ORDER BY Timestamp"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "$($reader['Timestamp']) | $($reader['BartenderName']) | $($reader['PaymentType']) | $($reader['TotalAmount'])"
}
$reader.Close()
$conn.Close()
