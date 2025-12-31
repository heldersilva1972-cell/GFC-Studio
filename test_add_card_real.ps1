$controllerIP = "192.168.0.196"
$localIP      = "192.168.0.72"
$port         = 60000
$serialNumber = 223213880 

# Packet Structure Config
$type = 0x17
$cmd  = 0x3E # 62 - Add/Update Card

$packet = New-Object byte[] 64
$packet[0] = $type
$packet[1] = $cmd

# SN at offset 4
$snBytes = [BitConverter]::GetBytes([uint32]$serialNumber)
[Array]::Copy($snBytes, 0, $packet, 4, 4)

# Payload at offset 8
# Card Number: 1001
$cardNo = 1001
$cardBytes = [BitConverter]::GetBytes([uint32]$cardNo)
[Array]::Copy($cardBytes, 0, $packet, 8, 4)

# Door Mask (Offset 12 in frame, 4 in payload) - All doors
$packet[12] = 0xFF # Allow all doors

# TimeZones (Offset 13-16 in frame, 5-8 in payload) - 0x01 (Allowed always)
$packet[13] = 1
$packet[14] = 1
$packet[15] = 1
$packet[16] = 1

# Start Date (Offset 17-20 in frame) - 2024-01-01
# Y=2024, M=1, D=1
# Controller Date format: Y<<16 | M<<8 | D?
# WgPayloadFactory: var y = (uint)local.Year; var m = (uint)local.Month; var d = (uint)local.Day; return (y << 16) | (m << 8) | d;
$dateStart = (2024 -shl 16) -bor (1 -shl 8) -bor 1
$dsBytes = [BitConverter]::GetBytes([uint32]$dateStart)
[Array]::Copy($dsBytes, 0, $packet, 17, 4)

# End Date (Offset 21-24 in frame) - 2025-12-31
$dateEnd = (2034 -shl 16) -bor (12 -shl 8) -bor 31
$deBytes = [BitConverter]::GetBytes([uint32]$dateEnd)
[Array]::Copy($deBytes, 0, $packet, 21, 4)

# Flags (Offset 25-26 in frame) - 0 (Normal) ? 
# WgPayloadFactory used (ushort)model.Flags. 
# 1 = Normal? 0? CardPrivilegeFlags.Normal?
# Let's say 1 (Normal)
$packet[25] = 1
$packet[26] = 0

# Name (Offset 27+ in frame)
$packet[27] = 0

# CRC-16
function Get-CRC16 {
    param([byte[]]$data)
    $crc = 0
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

Write-Host "Sending Add Card Command ($([BitConverter]::ToString($packet)))" -ForegroundColor Yellow

$socket = New-Object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Dgram, [System.Net.Sockets.ProtocolType]::Udp)
$socket.ReceiveTimeout = 5000

try {
    $socket.Bind((New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($localIP), 0)))
    $socket.SendTo($packet, (New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $port)))
    
    $buffer = New-Object byte[] 1024
    $ep = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $len = $socket.ReceiveFrom($buffer, [ref]$ep)
    
    Write-Host "RESPONSE RECEIVED! ($len bytes)" -ForegroundColor Green
    Write-Host "Bytes: $([BitConverter]::ToString($buffer[0..($len-1)]))" -ForegroundColor Cyan
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
finally { $socket.Close() }
