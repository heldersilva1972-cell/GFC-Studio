# GFC Access Control: N3000 Hardware Protocol Specification

### Document Metadata
* **Protocol Version**: N3000/WG3000 Baseline
* **Transmission Mode**: UDP (Connectionless)
* **Default Port**: 60000
* **Target Frame Size**: 64 Bytes (Fixed)
* **Encoding**: Little-Endian (for Integers) / BCD (for Date/Time)

---

## 1. Network Layer & Frame Geometry

All communication occurs via fixed-length 64-byte binary packets. Variable-length data is prohibited by the hardware firmware.

### The Master 64-Byte Frame Map
| Offset (Hex) | Field | Value / Description |
| :--- | :--- | :--- |
| `0x00` | **Packet Type** | `0x17` (Request) / `0x18` (Reply) |
| `0x01` | **Function Code** | The Command ID (e.g., `0x50`, `0x8E`, `0xB0`) |
| `0x02-0x03` | **CheckSum** | Salted CRC16-IBM (Salt = UDP Source Port, usually `60000`) |
| `0x04-0x07` | **Serial Number** | **Target Controller SN (Mandatory at this position)** |
| `0x08-0x0B` | **Transaction ID** | XID for tracking (Plain text) |
| `0x0C-0x0F` | **Source SN** | PC ID (usually 0, Plain text) |
| `0x10-0x3E` | **Payload** | Encrypted command data (Offset starts @ 16) |
| `0x3F` | **Integrity Tail** | 1-byte Summation of Bytes 0-62 |

---

## 2. Data Integrity & Security Protocols

### 2.2 Salted CRC (SSI Security)
The N3000 SSI protocol uses the **UDP Source Port** as a bitwise "salt" for the CRC16-IBM calculation. 
* **The Rule**: Before calculating the CRC, the source port (e.g., `0x60EA` for 60000) is written into Bytes 2-3.
* **The Implementation**:
  1. Set Bytes 2-3 to `60000` (Low byte first).
  2. Compute CRC16-IBM over the entire 64-byte frame.
  3. Write the resulting CRC back into Bytes 2-3.

### 2.3 Critical Memory Offsets
The hardware expects specific data at fixed positions within the 64-byte frame, regardless of "Payload" definitions:
* **Door Permission Mask**: MUST be at **Byte 20** (Packet Index 20).
* **Safety Password (`55 AA 55 AA`)**: MUST be at **Byte 32** (Packet Index 32) for command `0x50` and `0x5A`.
* **Hardware Timer/Result**: Response data often starts at **Byte 17** (Packet Index 17).

---

## 3. Binary Coded Decimal (BCD) Formatting

The N3000 hardware does **not** process binary integers for time or dates. It uses **BCD** (Binary Coded Decimal), where a decimal number is stored as its hexadecimal visual equivalent.

* **Example**: The year "2025" is sent as two bytes: `0x20 0x25`.
* **Example**: The month "December" is `0x12`.

---

## 4. Full Command Dictionary & Payload Structures

### 4.1 Discovery & Setup Commands
| Function | Name | Description |
| :--- | :--- | :--- |
| **0x94** | Search | Broadcast discovery. Returns IP, MAC, SN, and firmware version. |
| **0x20** | Get Run Status | Polls heartbeat, hardware clock, and the current event log index. |
| **0x30** | Sync Time | Updates the internal BCD clock of the hardware. |
| **0xFE** | Reboot | Software-triggered reset of the controller. |

### 4.2 Permission Management
| Function | Name | Description |
| :--- | :--- | :--- |
| **0x50** | Add/Update Card | Authorizes a card for specific doors. Uses a Door Mask byte. |
| **0x52** | Delete Card | Revokes authorization for a specific card number. |
| **0x54** | Clear All Cards | Wipes the hardware card database. Requires `0x55AA55AA` password. |

### 4.3 Door Control & Configuration
| Function | Name | Description |
| :--- | :--- | :--- |
| **0x01** | Open Door | Remotely triggers the relay to unlock a door immediately. |
| **0x8E** | Set Door Config | Configures Mode (Always Open/Closed/Controlled), Relay Delay, and Sensors. |
| **0x5A** | Get Door Params | Interrogates the hardware to read current relay/sensor settings. |

### 4.4 Event Log Management
| Function | Name | Description |
| :--- | :--- | :--- |
| **0xB0** | Get Events | Fetches the next available access log record from hardware memory. |
| **0xB2** | Acknowledge | Confirms log receipt so the hardware can reuse that memory slot. |

---

## 5. Advanced Logic: The "Rescue" Sequence

When a controller is in a corrupted state or the clock has drifted (preventing new card updates), the software implements a **Triple-Handshake Rescue**:

1. **Broad Search (0x94)**: Identifies the device and wakes up the maintenance listener.
2. **Rescue Clear (0x54)**: Addressed to **Wildcard SN `0x00118000`**. This forces a database wipe regardless of the stored SN.
3. **Clock Lock (0x30)**: Re-aligns the hardware clock to current server time. **Note**: If the hardware clock is behind the server year (e.g., stuck in 2000), it will reject card updates until this sync completes.

---

## 6. Verified Payload Layouts

### 6.1 Add/Update Card (0x50)
| Physical Byte | Content | Description |
| :--- | :--- | :--- |
| `0x10-0x13` | Card Number | Index 0 of payload |
| `0x14` | Door Mask | Index 4 of payload (Byte 20) |
| `0x15-0x18` | Start Date | Index 5 of payload (Byte 21-24) |
| `0x19-0x1C` | End Date | Index 9 of payload (Byte 25-28) |
| `0x20-0x23` | Password | Index 16 of payload (Byte 32-35) |

### 6.2 Get Door Params (0x5A)
| Physical Byte | Content | Description |
| :--- | :--- | :--- |
| `0x10` | Door Index | Index 0 of payload (Byte 16) |
| `0x11-0x14` | Password | Index 1 of payload (Byte 17-20) |

---

## 7. Access Result Codes (Event Parsing)
When reading logs (0xB0), the controller returns result codes within the response:
* `0x01`: Access Granted
* `0x02`: Access Denied (Card Not Found)
* `0x1B`: Access Denied (Unauthorized for this Door)
