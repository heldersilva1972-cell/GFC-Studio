[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
$response = Invoke-RestMethod -Uri 'https://localhost:7073/api/mobile-auth/permissions/1' -Method Get
$response | Where-Object { $_.Page.Category -like "*MOBILE*" } | Select-Object -ExpandProperty Page | Select-Object PageName, Route, Category | ConvertTo-Json
