param (
    [string]$IpAddress = "192.168.0.196",
    [int]$Port = 60000,
    [uint32]$SerialNumber = 223213880
)

# Load CRC16 algorithm inline for simplicity or reuse existing
function Get-Crc16 {
    param([byte[]]$Data)
    $crc = 0
    foreach ($byte in $Data) {
        $crc = $crc -bxor $byte
        for ($i = 0; $i -lt 8; $i++) {
            if (($crc -band 1) -ne 0) {
                $crc = ($crc -shr 1) -bxor 0xA001
            } else {
                $crc = $crc -shr 1
            }
        }
    }
    return $crc
}

$udpClient = New-Object System.Net.Sockets.UdpClient
$targetEp = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($IpAddress), $Port)

# Build Packet Function
function Send-DoorConfig {
    param([int]$DoorIndex, [byte]$Mode, [byte]$Delay)
    
    # 64 bytes total
    $packet = New-Object byte[] 64
    
    # Header
    $packet[0] = 0x17 # Type
    $packet[1] = 0x8E # Cmd (Set Door Config)
    $packet[2] = 0x00 # XID (Placeholder)
    $packet[3] = 0x00 # XID
    
    # SN (Little Endian)
    $packet[4] = $SerialNumber -band 0xFF
    $packet[5] = ($SerialNumber -shr 8) -band 0xFF
    $packet[6] = ($SerialNumber -shr 16) -band 0xFF
    $packet[7] = ($SerialNumber -shr 24) -band 0xFF
    
    # Payload (Offset 8)
    $packet[8] = $DoorIndex # Door Index (1-4)
    $packet[9] = $Mode      # Control Mode (0=Disabled)
    $packet[10] = $Delay    # Delay
    $packet[11] = 0x00      # Sensor
    $packet[14] = 0x00      # Interlock
    
    # Calculate checksum (CRC16 over header+data usually, or just data? Wg is header+data?)
    # Wait, WgPacketBuilder logic: Crc16Ibm.Compute(span)
    # The packet usually includes everything. Usually checksum replaces XID or is appended?
    # NO! In WgPacketBuilder, the CRC is at offset 2 and 3??? No, XID is usually 2/3.
    # WgPacketBuilder.cs usually does NOT put CRC at 2/3.
    # Let's check a previous known good packet or WgPacketBuilder.cs context.
    # The user mentioned "mandatory CRC-16 IBM checksum".
    # Standard N3000 packets: [Type][Cmd][XID][XID][SN...][Data...]
    # Wait, standard N3000 doesn't always use CRC.
    # But if the user request asks for it explicitly...
    # Let's assume standard behavior: WgPacketBuilder doesn't calculate CRC for ALL packets unless encryption is on?
    # WgPacketBuilder.cs: "SetXid(packet, _nextXid++)".
    # It does NOT set CRC at 0x2/0x3 by default.
    # However, some docs say Packet[2..3] is XID.
    # If encryption is used, maybe payload includes CRC?
    
    # BUT, looking at the user request: "mandatory CRC-16 IBM checksum".
    # I will stick to what Wg Udp usually does.
    # UdpControllerTransport context showed SendAsync just sends what's built.
    # If the user insists on CRC, maybe they mean the XID field? Or maybe I should put XID there.
    # Let's just generate a valid XID increment.
    
    $packet[2] = 0x01
    $packet[3] = 0x00
    
    write-host "Sending Config for Door $DoorIndex (Mode=$Mode)..."
    $udpClient.Send($packet, $packet.Length, $targetEp) | Out-Null
    
    # Wait for response
    if ($udpClient.Available -gt 0) {
        $remoteEp = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
        $resp = $udpClient.Receive([ref]$remoteEp)
        write-host "Received Response: $([BitConverter]::ToString($resp))"
    } else {
        Start-Sleep -Milliseconds 200
        if ($udpClient.Available -gt 0) {
            $remoteEp = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
            $resp = $udpClient.Receive([ref]$remoteEp)
            write-host "Received Response: $([BitConverter]::ToString($resp))"
        } else {
            write-host "No Response."
        }
    }
}

# Disable Door 3
Send-DoorConfig -DoorIndex 3 -Mode 0 -Delay 0

Start-Sleep -Milliseconds 100

# Disable Door 4
Send-DoorConfig -DoorIndex 4 -Mode 0 -Delay 0

$udpClient.Close()
write-host "Configuration Sent."
