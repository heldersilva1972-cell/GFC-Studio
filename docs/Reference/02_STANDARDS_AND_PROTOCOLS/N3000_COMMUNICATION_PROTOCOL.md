# N3000 Controller Communication Protocol: Technical Integration Guide

| Version | Date | Author | Description |
| --- | --- | --- | --- |
| 1.0 | 2025-12-31 | Antigravity | Initial protocol documentation from verified sources. |

---

This document serves as the official technical guideline for establishing direct network communication with N3000 series industrial controllers, based on the analyzed source code and verified network behavior.

---

### 1. Network Environment & Infrastructure

* **Protocol**: All communication is handled via **UDP (User Datagram Protocol)**.
* **Port**: The controller listens and responds exclusively on **UDP Port 60000**.
* **IP Configuration**: The controller requires a valid IP address on the same subnet as the host PC (e.g., `192.168.0.x`). If the controller is in a factory-default "identity-less" state, it will issue **DHCP Discover** requests or require an **ARP static entry** to be reachable.
* **Firewall Requirements**: An inbound firewall rule must be active on the host PC to allow UDP traffic on port 60000, or the controller's responses will be blocked by the operating system.

### 2. Standard Frame Structure (64-Byte Format)

The protocol utilizes a fixed-length **64-byte binary packet** for both requests and responses. Data is primarily stored in **Little Endian** format.

| Byte Offset | Field Name | Description / Values |
| --- | --- | --- |
| **0** | **Frame Header** | Always `0x17` for N3000 series protocol logic. |
| **1** | **Function Code** | `0x94`: Search/Discovery; `0x96`: Write Configuration. |
| **2 – 3** | **CRC-16** | 16-bit Checksum (Low byte at 2, High byte at 3). |
| **4 – 7** | **Serial Number** | 4-byte Unique Identifier. Use `0x00` or `0xFF` for broadcast. |
| **8 – 11** | **IP Address** | 4-byte IPv4 address (e.g., `C0-A8-00-C4` for 192.168.0.196). |
| **12 – 15** | **Subnet Mask** | 4-byte network mask (e.g., `FF-FF-FF-00` for 255.255.255.0). |
| **16 – 19** | **Gateway** | 4-byte default gateway IP address. |
| **20 – 25** | **MAC Address** | 6-byte hardware Physical Address. |
| **26 – 27** | **Firmware Version** | Byte 26: Major version; Byte 27: Minor version. |
| **28 – 30** | **Build Date** | Bytes 28 (Year), 29 (Month), 30 (Day) in BCD or decimal. |

### 3. Checksum (CRC-16) Algorithm

The controller employs a specific **CRC-16** implementation to validate packet integrity. Packets with incorrect CRCs are discarded without a response.

* **Polynomial**: `0xA001`.
* **Initial Value**: `0x0000`.
* **Calculation Scope**: The CRC must be calculated over the entire 64-byte frame.
* **Important**: During calculation, the CRC bytes (index 2 and 3) must be set to `0x00` so they do not interfere with the mathematical result.

### 4. Communication Workflows

#### A. Device Discovery (The "Search" Command)

1. Initialize a 64-byte array with `0x17` at index 0 and `0x94` at index 1.
2. Set the target Serial Number (bytes 4-7). If unknown, use `0x00` for a general search.
3. Calculate the CRC-16 and place it at index 2 and 3.
4. Send the UDP packet to the broadcast address `255.255.255.255` or the specific IP on port 60000.
5. The controller will return its full 64-byte configuration block.

#### B. Remote Configuration (The "Write" Command)

1. Construct the 64-byte array using Header `0x17` and Function Code `0x96`.
2. Provide the exact Serial Number of the target controller in bytes 4-7.
3. Populate the desired new IP, Mask, and Gateway in their respective offsets (8 through 19).
4. Recalculate the CRC-16 and transmit to the controller's current IP on port 60000.

### 5. Troubleshooting Guidelines

* **Timeout Errors**: Usually caused by port conflicts (another app using port 60000) or the Windows Firewall blocking the response.
* **Incorrect Data**: Verify the byte order. Serial Numbers and IP addresses are transmitted in **Little Endian** (Least Significant Byte first).
* **Silent Controller**: If the controller receives the packet (verified via Wireshark) but does not reply, the CRC-16 calculation is likely incorrect or the Serial Number provided does not match the hardware.
