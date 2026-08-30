$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
INSERT INTO dbo.ActiveEvents (
    Name, 
    Status, 
    Type, 
    TemplateId, 
    CurrentBalance, 
    InitialAmount, 
    EnableBeerTally, 
    ClubDonatedCasesCap, 
    DonatedItemIdsJson, 
    Enable100PercentDonatedProceeds, 
    IsRecurring, 
    RecipientEventName, 
    IsDeleted, 
    CreatedAt
) VALUES (
    'Family Picnic', 
    1, -- Closed
    0, -- RunningTab
    6, 
    0.00, 
    0.00, 
    0, 
    3, 
    '{"51":24,"50":24,"54":24}', 
    1, 
    0, 
    'Horseshoes', 
    0, -- Not deleted
    GETUTCDATE()
);
"@
$rowsAffected = $cmd.ExecuteNonQuery()
Write-Output "Inserted Family Picnic into ActiveEvents. Rows affected: $rowsAffected"

$cmd.CommandText = "SELECT Id, Name, Status, IsDeleted, CurrentBalance, InitialAmount, RecipientEventName, DonatedItemIdsJson, CreatedAt FROM ActiveEvents"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
$dt | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
