# Create CA directory
New-Item -ItemType Directory -Force -Path "infrastructure\ca"

$caCommonName = "GFC Internal Root CA"
$serverCommonName = "gfc.lovanow.com"
$validityYears = 10

# 1. Generate Root CA
Write-Host "Generating Root CA..." -ForegroundColor Cyan
$rootCA = New-SelfSignedCertificate -DnsName $caCommonName -CertStoreLocation "Cert:\LocalMachine\My" -KeyUsage PropertyLength -NotAfter (Get-Date).AddYears($validityYears) -FriendlyName $caCommonName

# Export Root CA public certificate
Export-Certificate -Cert $rootCA -FilePath "infrastructure\ca\GFC_Root_CA.cer"

# Export Root CA to PFX (for backup/signing)
$password = ConvertTo-SecureString -String "GfcAdmin2025!" -Force -AsPlainText
Export-PfxCertificate -Cert $rootCA -FilePath "infrastructure\ca\GFC_Root_CA.pfx" -Password $password

# 2. Generate Server Certificate signed by Root CA
Write-Host "Generating Server Certificate for $serverCommonName..." -ForegroundColor Cyan
$serverCert = New-SelfSignedCertificate -DnsName $serverCommonName -CertStoreLocation "Cert:\LocalMachine\My" -Signer $rootCA -NotAfter (Get-Date).AddYears(2) -FriendlyName $serverCommonName

# Export Server Certificate to PFX (for IIS)
Export-PfxCertificate -Cert $serverCert -FilePath "infrastructure\ca\GFC_Server.pfx" -Password $password

Write-Host "Certificates generated successfully in infrastructure\ca\" -ForegroundColor Green
Write-Host "GFC_Root_CA.cer: Provide this to users for onboarding"
Write-Host "GFC_Server.pfx: Install this in IIS for gfc.lovanow.com"
