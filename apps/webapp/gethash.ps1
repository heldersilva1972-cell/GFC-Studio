$password = "851181"
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$bytes = [System.Text.Encoding]::UTF8.GetBytes($password)
$hash = $sha256.ComputeHash($bytes)
$base64 = [System.Convert]::ToBase64String($hash)
Write-Output "HASH_START:$base64:HASH_END"
