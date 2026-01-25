$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"

$pageQuery = "SELECT PageId FROM AppPages WHERE PageRoute LIKE '%/bartender-shift%' OR PageName = 'Bartender Shift'"

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connectionString
    $conn.Open()

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $pageQuery
    $pageId = $cmd.ExecuteScalar()

    if ($pageId -ne $null) {
        Write-Host "Found Bartender Shift page with ID: $pageId"
        
        # Delete permissions
        $deletePermsCmd = $conn.CreateCommand()
        $deletePermsCmd.CommandText = "DELETE FROM PagePermissions WHERE PageId = @PageId"
        $deletePermsCmd.Parameters.AddWithValue("@PageId", $pageId) | Out-Null
        $deletePermsCmd.ExecuteNonQuery()
        Write-Host "Deleted associated permissions."

        # Delete page
        $deletePageCmd = $conn.CreateCommand()
        $deletePageCmd.CommandText = "DELETE FROM AppPages WHERE PageId = @PageId"
        $deletePageCmd.Parameters.AddWithValue("@PageId", $pageId) | Out-Null
        $deletePageCmd.ExecuteNonQuery()
        Write-Host "Deleted AppPages entry."
    }
    else {
        Write-Host "Bartender Shift page not found in AppPages table."
    }
    
    $conn.Close()
}
catch {
    Write-Error "An error occurred: $_"
}
