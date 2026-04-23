[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
$response = Invoke-RestMethod -Uri 'https://localhost:7073/api/mobile-auth/users' -Method Get
$response | Select-Object -ExpandProperty Username
