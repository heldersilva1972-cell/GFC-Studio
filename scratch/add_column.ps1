$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$sql = @"
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PosZReports') AND name = 'ItemTotalsJson')
BEGIN
    ALTER TABLE PosZReports ADD ItemTotalsJson NVARCHAR(MAX) NOT NULL DEFAULT '{}';
    PRINT 'Column ItemTotalsJson added successfully';
END
ELSE
BEGIN
    PRINT 'Column ItemTotalsJson already exists';
END
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($sql, $connection)
$cmd.ExecuteNonQuery()
Write-Host "Database migration check complete."

$connection.Close()
