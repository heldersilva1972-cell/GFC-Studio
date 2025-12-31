param (
    [string]$IpAddress = "192.168.0.196",
    [int]$Port = 60000,
    [uint32]$SerialNumber = 223213880
)

$udpClient = New-Object System.Net.Sockets.UdpClient
$targetEp = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($IpAddress), $Port)

# Build Packet (0x20 Get Run Status)
$packet = New-Object byte[] 64
$packet[0] = 0x17
$packet[1] = 0x20
$packet[2] = 0x00
$packet[3] = 0x00
$packet[4] = $SerialNumber -band 0xFF
$packet[5] = ($SerialNumber -shr 8) -band 0xFF
$packet[6] = ($SerialNumber -shr 16) -band 0xFF
$packet[7] = ($SerialNumber -shr 24) -band 0xFF

write-host "Sending GetRunStatus (0x20)..."
$udpClient.Send($packet, $packet.Length, $targetEp) | Out-Null
Start-Sleep -Milliseconds 200

if ($udpClient.Available -gt 0) {
    $remoteEp = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $resp = $udpClient.Receive([ref]$remoteEp)
    
    # Parse Door Status (Offset 8 in payload, offset 12 in packet?)
    # WgResponseParser: payload starts at 4 (Response Packet Offset 4 is SN).
    # Packet: [Type][Cmd][XID][XID][SN...][Data...]
    # Wait, WgResponseParser says payload starts at 4 IF it strips header?
    # No, it says Payload = packet[HEADER_LENGTH..]. Header is 4 bytes.
    # So Payload[0] is Packet[4] (which is SN).
    
    # Run Status Data starts at Payload Offset 4 (Packet Offset 8).
    # Data[0] = Door Open Bits (1=Open)
    # Data[1] = Relay State Bits (1=On)
    # Data[2] = Sensor State Bits (1=Active)
    
    $doorOpen = $resp[8]
    $relayState = $resp[9]
    $sensorState = $resp[10]
    
    write-host "--- Run Status ---"
    for ($i = 0; $i -lt 4; $i++) {
        $isOpen = ($doorOpen -band (1 -shl $i)) -ne 0
        $isRelay = ($relayState -band (1 -shl $i)) -ne 0
        $isSensor = ($sensorState -band (1 -shl $i)) -ne 0
        write-host "Door $($i+1): Open=$isOpen, Relay=$isRelay, Sensor=$isSensor"
    }
} else {
    write-host "No Response."
}

$udpClient.Close()
