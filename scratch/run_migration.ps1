$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PosTokens]') AND name = 'UpgradesFromTokenId')
BEGIN
    ALTER TABLE [dbo].[PosTokens] ADD [UpgradesFromTokenId] INT NULL;
    SELECT 'ADDED' AS Result;
END
ELSE
BEGIN
    SELECT 'EXISTS' AS Result;
END
"@
    $res = $cmd.ExecuteScalar()
    Write-Host "Migration Result: $res"
}
catch {
    Write-Error $_.Exception.Message
}
finally {
    if ($conn.State -eq [System.Data.ConnectionState]::Open) {
        $conn.Close()
    }
}
