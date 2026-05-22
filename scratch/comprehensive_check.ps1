$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

function Test-Table-Row-By-Row($tableName) {
    Write-Host "Testing table row-by-row: $tableName"
    $query = "SELECT * FROM [$tableName]"
    $cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
    $reader = $cmd.ExecuteReader()
    
    $cols = @()
    for ($i = 0; $i -lt $reader.FieldCount; $i++) {
        $cols += $reader.GetName($i)
    }
    
    $rowCount = 0
    while ($reader.Read()) {
        $rowCount++
        $id = "Row $rowCount"
        try {
            if ($cols -contains "Id") {
                $id = $reader["Id"].ToString()
            }
        } catch {}
        
        for ($i = 0; $i -lt $cols.Count; $i++) {
            $colName = $cols[$i]
            try {
                $val = $reader.GetValue($i)
                # Try reading specific types to trigger any conversion or null issues
                if ($val -ne [System.DBNull]::Value) {
                    $type = $val.GetType().FullName
                }
            } catch {
                $err = $_.Exception.Message
                Write-Host "  >> ERROR on table $tableName, Row $id, Column $colName - Error: $err"
            }
        }
    }
    $reader.Close()
    Write-Host "Completed testing $rowCount rows in $tableName"
}

Test-Table-Row-By-Row "PosZReports"
Test-Table-Row-By-Row "PosSales"
Test-Table-Row-By-Row "PosTokens"

$connection.Close()
