# ProjectCleanup.ps1
# Organizes the root directory by moving loose files into secondary folders.

$Root = Get-Location
$DocsDir = Join-Path $Root "docs"
$SqlDest = Join-Path $DocsDir "DatabaseScripts"
$LogDest = Join-Path $DocsDir "History_Logs"
$ScriptDest = Join-Path $Root "scripts\Launch_Utility"

# Create target directories
$Targets = @($SqlDest, $LogDest, $ScriptDest)
foreach ($t in $Targets) {
    if (!(Test-Path $t)) {
        New-Item -ItemType Directory -Path $t -Force | Out-Null
        Write-Host "Created: $t" -ForegroundColor Cyan
    }
}

# 1. SQL Scripts
Get-ChildItem -Path $Root -Filter "*.sql" | ForEach-Object {
    Write-Host "Moving SQL: $($_.Name)"
    Move-Item -Path $_.FullName -Destination $SqlDest -Force
}

# 2. Documentation & Logs (.md, .txt)
# Exclude README.md and essential config
$DocExtensions = @("*.md", "*.txt")
foreach ($ext in $DocExtensions) {
    Get-ChildItem -Path $Root -Filter $ext | Where-Object { 
        $_.Name -ne "README.md" -and 
        $_.Name -ne "AGENT_WORKFLOW_RULES.md" -and
        $_.Name -ne "AGENTS.md"
    } | ForEach-Object {
        Write-Host "Moving Doc: $($_.Name)"
        Move-Item -Path $_.FullName -Destination $LogDest -Force
    }
}

# 3. Operational Scripts (.bat, .ps1)
# Exclude the cleanup script itself
Get-ChildItem -Path $Root -Include "*.bat", "*.ps1" -File | Where-Object {
    $_.Name -ne "ProjectCleanup.ps1" -and
    $_.Name -ne "MoveCompletedDocs.ps1"
} | ForEach-Object {
    Write-Host "Moving Utility: $($_.Name)"
    Move-Item -Path $_.FullName -Destination $ScriptDest -Force
}

Write-Host "`nProject directory cleaned successfully!" -ForegroundColor Green
