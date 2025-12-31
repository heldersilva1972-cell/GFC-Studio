$controllerIP = "192.168.0.196"
$localIP      = "192.168.0.72"
$port         = 60000
$serialNumber = 223213880 # From your config tool screenshot

# 1. Build the Targeted Search Packet (64 Bytes)
# The source code wgMjController.cs often uses a 64-byte buffer for extended info
$packet = New-Object byte[] 64
$packet[0] = 0x17  # Frame Type
$packet[1] = 0x94  # Function: Search

# Insert Serial Number at Offset 4 (Little Endian)
$snBytes = [BitConverter]::GetBytes([uint32]$serialNumber)
$packet[4] = $snBytes[0]
$packet[5] = $snBytes[1]
$packet[6] = $snBytes[2]
$packet[7] = $snBytes[3]

# 2. CRC-16 Calculation
function Get-CRC16 {
    param([byte[]]$data)
    $crc = 0
    # Calculate over the header and data, skipping the 2-byte CRC slot at index 2
    foreach ($b in $data[0,1 + 4..63]) {
        $crc = $crc -bxor $b
        for ($i = 0; $i -lt 8; $i++) {
            if (($crc -band 1) -ne 0) { $crc = ($crc -shr 1) -bxor 0xA001 }
            else { $crc = $crc -shr 1 }
        }
    }
    return $crc
}

$crcValue = Get-CRC16 $packet
$packet[2] = [byte]($crcValue -band 0xFF)
$packet[3] = [byte](($crcValue -shr 8) -band 0xFF)

# 3. Network Execution
Write-Host "--- Sending Targeted N3000 Search (SN: $serialNumber) ---" -ForegroundColor Cyan
$socket = New-Object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Dgram, [System.Net.Sockets.ProtocolType]::Udp)
$socket.ReceiveTimeout = 5000

try {
    $socket.Bind((New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($localIP), 0)))
    $socket.SendTo($packet, (New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $port)))
    
    Write-Host "Packet Sent. Waiting for response..." -ForegroundColor Yellow
    
    $buffer = New-Object byte[] 1024
    $ep = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $len = $socket.ReceiveFrom($buffer, [ref]$ep)
    
    Write-Host "SUCCESS! Received $($len) bytes from $($ep.Address)" -ForegroundColor Green
    Write-Host "Response Hex: $([BitConverter]::ToString($buffer[0..($len-1)]))" -ForegroundColor Yellow
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
finally { $socket.Close() }