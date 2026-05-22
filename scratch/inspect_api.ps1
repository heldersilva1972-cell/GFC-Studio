[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12 -bor [System.Net.SecurityProtocolType]::Tls13
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
try {
    $resp = Invoke-RestMethod -Uri "http://localhost:5207/api/pos/menu?terminalName=TERMINAL%201"
    $resp.Tokens | ConvertTo-Json
} catch {
    Write-Error $_
}
