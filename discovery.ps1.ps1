# UPDATE THESE TO MATCH YOUR NEW ROUTER SETUP
$controllerIP = "192.168.0.196"   # Found in your config tool
$controllerPort = 60000           # Found in your config tool
$localIP = "192.168.0.72"         # Your PC's Wi-Fi IP (from ipconfig)

Write-Host "=== TESTING CONNECTION TO NEW IP ===" -ForegroundColor Cyan
Write-Host "Target: $controllerIP" -ForegroundColor Yellow

# ... (Keep your existing CRC function and packet building code) ...

$socket = New-Object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Dgram, [System.Net.Sockets.ProtocolType]::Udp)
$socket.ReceiveTimeout = 5000

try {
    # Bind to your ACTUAL Wi-Fi IP
    $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($localIP), 0)
    $socket.Bind($localEndpoint)
    
    # Send directly to the new IP
    $remoteEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $controllerPort)
    $socket.SendTo($packet, $remoteEndpoint)
    
    Write-Host "Packet Sent. Waiting for response..." -ForegroundColor Yellow
    
    $buffer = New-Object byte[] 1500
    $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $bytesReceived = $socket.ReceiveFrom($buffer, [ref]$responseEndpoint)
    
    Write-Host "SUCCESS! Controller responded from $($responseEndpoint.Address)" -ForegroundColor Green
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Try pinging 192.168.0.196 first to verify the path." -ForegroundColor Gray
}
finally { $socket.Close() }