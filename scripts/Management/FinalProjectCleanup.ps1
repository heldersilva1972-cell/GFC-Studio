# FinalProjectCleanup.ps1
# Exhaustively organizes the root folder into structured categories.

$Root = Get-Location
$SqlDest = Join-Path $Root "docs\DatabaseScripts"
$LogDest = Join-Path $Root "docs\History_Logs"
$ScriptDest = Join-Path $Root "scripts\Launch_Utility"

# 1. Ensure Directories Exist
foreach ($dir in @($SqlDest, $LogDest, $ScriptDest)) {
    if (!(Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
        Write-Host "Prepared folder: $dir" -ForegroundColor Cyan
    }
}

# 2. Move SQL Scripts (Schemas, Fixes, Migrations)
Get-ChildItem -Path $Root -Filter "*.sql" | ForEach-Object {
    Write-Host "Moving SQL: $($_.Name)"
    Move-Item -Path $_.FullName -Destination $SqlDest -Force
}

# 3. Move Documents & Logs (.md, .txt)
# Stays: README.md, AGENTS.md, and essential config
$DocExtensions = @("*.md", "*.txt")
foreach ($ext in $DocExtensions) {
    Get-ChildItem -Path $Root -Filter $ext | Where-Object { 
        $_.Name -ne "README.md" -and 
        $_.Name -ne "AGENT_WORKFLOW_RULES.md" -and
        $_.Name -ne "AGENTS.md"
    } | ForEach-Object {
        Write-Host "Moving Log/Doc: $($_.Name)"
        Move-Item -Path $_.FullName -Destination $LogDest -Force
    }
}

# 4. Move Operational Scripts (.bat, .ps1)
# Skips the cleanup tools themselves so you can keep running them
foreach ($ext in @("*.bat", "*.ps1")) {
    Get-ChildItem -Path $Root -Filter $ext | Where-Object {
        $_.Name -notmatch "ProjectCleanup" -and
        $_.Name -notmatch "MoveCompletedDocs" -and
        $_.Name -notmatch "FinalProjectCleanup"
    } | ForEach-Object {
        Write-Host "Moving Script: $($_.Name)"
        Move-Item -Path $_.FullName -Destination $ScriptDest -Force
    }
}

Write-Host "`nWorkspace is now clean!" -ForegroundColor Green
