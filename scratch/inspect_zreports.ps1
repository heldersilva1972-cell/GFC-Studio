$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Function to inspect a table
function Inspect-Table($tableName) {
    Write-Host "`n========================================="
    Write-Host "INSPECTING TABLE: $tableName"
    Write-Host "========================================="

    # Get schema
    $queryColumns = "
    SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = '$tableName'"
    $cmd = New-Object System.Data.SqlClient.SqlCommand($queryColumns, $connection)
    $reader = $cmd.ExecuteReader()
    Write-Host "--- Schema Columns ---"
    $cols = @()
    while ($reader.Read()) {
        $colName = $reader['COLUMN_NAME']
        $isNullable = $reader['IS_NULLABLE']
        $dataType = $reader['DATA_TYPE']
        Write-Host "$colName - Nullable: $isNullable - Type: $dataType"
        $cols += @{ Name = $colName; Nullable = $isNullable; Type = $dataType }
    }
    $reader.Close()

    # Check for null counts
    if ($cols.Count -gt 0) {
        $sumPart = ""
        foreach ($c in $cols) {
            $sumPart += ", SUM(CASE WHEN [$($c.Name)] IS NULL THEN 1 ELSE 0 END) as [$($c.Name)_Nulls]"
        }
        $queryCheck = "SELECT COUNT(*) as TotalRows $sumPart FROM [$tableName]"
        $cmdCheck = New-Object System.Data.SqlClient.SqlCommand($queryCheck, $connection)
        
        try {
            $readerCheck = $cmdCheck.ExecuteReader()
            Write-Host "`n--- Null Counts ---"
            if ($readerCheck.Read()) {
                Write-Host "Total Rows: $($readerCheck['TotalRows'])"
                foreach ($c in $cols) {
                    $nullCount = $readerCheck["$($c.Name)_Nulls"]
                    if ($nullCount -gt 0) {
                        Write-Host "  >> Column [$($c.Name)] has $nullCount NULL values!"
                    } else {
                        Write-Host "  Column [$($c.Name)]: 0 nulls"
                    }
                }
            }
            $readerCheck.Close()
        } catch {
            $err = $_
            Write-Host "Error running null count check for $tableName - Error = $err"
        }
    }
}

Inspect-Table "PosZReports"
Inspect-Table "PosSales"
Inspect-Table "PosTokens"

$connection.Close()
