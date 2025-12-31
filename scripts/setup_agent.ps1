$ErrorActionPreference = "Stop"

$sourceDir = "apps\webapp\archive\Agent"
$targetDir = "apps\services\AccessControlAgent"
$webAppConfigPath = "apps\webapp\GFC.BlazorServer\appsettings.json"
$apiKey = "GFC-ACCESS-CONTROL-SECRET-KEY-2025"

Write-Host "Started Access Control Agent Setup..."

# 1. Create Target Directory
if (-not (Test-Path $targetDir)) {
    New-Item -ItemType Directory -Force -Path $targetDir | Out-Null
    Write-Host "Created directory: $targetDir"
}

# 2. Copy Projects
# Use Copy-Item with -Recurse. Robocopy is faster but Copy-Item is easier to script in one block for PowerShell.
Write-Host "Copying Agent files..."
Copy-Item -Path "$sourceDir\Gfc.Agent.Api" -Destination $targetDir -Recurse -Force
Copy-Item -Path "$sourceDir\Gfc.ControllerClient" -Destination $targetDir -Recurse -Force
Copy-Item -Path "$sourceDir\GFC.Agent.sln" -Destination $targetDir -Force

# 3. Configure Agent AppSettings
$agentConfigPath = "$targetDir\Gfc.Agent.Api\appsettings.json"
if (Test-Path $agentConfigPath) {
    $json = Get-Content $agentConfigPath -Raw
    # Simple string replacement to avoid parsing JSON if structure varies, 
    # but strictly "ApiKey": "CHANGE_ME" -> "ApiKey": "..." is safer with replace
    $json = $json -replace '"ApiKey": "CHANGE_ME"', "`"ApiKey`": `"$apiKey`""
    
    # Also update the controller IP to a placeholder that isn't localhost if needed, 
    # but let's stick to just the key for now.
    
    Set-Content -Path $agentConfigPath -Value $json
    Write-Host "Updated Agent API Key in $agentConfigPath"
} else {
    Write-Error "Agent appsettings.json not found at $agentConfigPath"
}

# 4. Configure WebApp AppSettings
if (Test-Path $webAppConfigPath) {
    $json = Get-Content $webAppConfigPath -Raw
    $json = $json -replace '"ApiKey": "REPLACE_WITH_AGENT_KEY"', "`"ApiKey`": `"$apiKey`""
    Set-Content -Path $webAppConfigPath -Value $json
    Write-Host "Updated WebApp API Key in $webAppConfigPath"
} else {
    Write-Error "WebApp appsettings.json not found at $webAppConfigPath"
}

Write-Host "Setup Complete!"
Write-Host "To start the agent: cd apps\services\AccessControlAgent\Gfc.Agent.Api; dotnet run"
