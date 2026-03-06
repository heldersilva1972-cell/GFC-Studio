$hash = [System.Security.Cryptography.SHA256]::Create().ComputeHash([System.Text.Encoding]::UTF8.GetBytes("851181"))
$b64 = [Convert]::ToBase64String($hash)
Write-Output $b64
