# DuckDNS Update Script Template
# SAFE TO COMMIT - Contains no secrets

# INSTRUCTIONS:
# 1. Copy this file to: C:\Scripts\update-duckdns.ps1
# 2. Replace YOUR_TOKEN_HERE with your actual DuckDNS token
# 3. Replace yourclub with your actual subdomain
# 4. DO NOT commit the actual script with real values!

$token = 'YOUR_TOKEN_HERE'  # Get from https://www.duckdns.org/ after signing in
$domain = 'yourclub'         # Your chosen subdomain (e.g., 'mygfc' for mygfc.duckdns.org)

# Update DuckDNS
Invoke-WebRequest -Uri "https://www.duckdns.org/update?domains=$domain&token=$token" -UseBasicParsing | Out-Null

# Optional: Log the update
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Add-Content -Path "C:\Scripts\duckdns-update.log" -Value "$timestamp - Updated $domain.duckdns.org"
