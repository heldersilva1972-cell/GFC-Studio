$source = "apps\services\AccessControlAgent\Gfc.ControllerClient"
$dest = "apps\webapp\GFC.BlazorServer\Connectors\Mengqi"
$namespace = "GFC.BlazorServer.Connectors.Mengqi"

# 1. Create Dest
New-Item -ItemType Directory -Force -Path $dest | Out-Null

# 2. Copy all .cs files recursively
Get-ChildItem -Path $source -Filter "*.cs" -Recurse | ForEach-Object {
    $relativePath = $_.FullName.Substring((Resolve-Path $source).Path.Length)
    $targetPath = Join-Path $dest $relativePath
    $targetDir = Split-Path $targetPath
    
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Force -Path $targetDir | Out-Null
    }
    
    # Read, replace namespace, write
    $content = Get-Content $_.FullName -Raw
    # Replace the base namespace declaration
    $content = $content -replace "namespace Gfc.ControllerClient", "namespace $namespace"
    # Replace using statements that refer to the old namespace
    $content = $content -replace "using Gfc.ControllerClient", "using $namespace"
    
    Set-Content -Path $targetPath -Value $content
}

Write-Host "Code copied and namespaced successfully."
