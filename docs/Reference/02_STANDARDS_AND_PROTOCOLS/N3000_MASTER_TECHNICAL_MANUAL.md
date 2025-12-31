# N3000 Series Access Controller: Master Technical Manual

## 1. System Architecture and Purpose
**What it is:** The N3000 is a distributed intelligent access controller designed to manage physical security barriers (doors, gates, mantraps).

**Why use it:** It provides autonomous decision-making; once programmed, the controller manages access rights, schedules, and alarms locally without requiring a persistent server connection.

## 2. Communication Protocol (The "How")
The controller communicates over **UDP Port 60000** using a structured binary protocol.

### Packet Structure
Standard commands utilize a **64-byte packet**, while bulk data transfers use **1024-byte** or **1152-byte** payloads.

*   **Header (Byte 0):** Always `0x17`.
*   **Function Code (Byte 1):** The specific command ID (e.g., `0x40` for Open).
*   **CRC-16 (Bytes 2-3):** IBM Checksum for data integrity.
*   **Serial Number (Bytes 4-7):** Unique 4-byte identifier for the target device.

## 3. Core Functional Commands
These commands represent the primary methods for interacting with the hardware.

| Command Code | Name | Description |
| :--- | :--- | :--- |
| **0x40** | **Remote Open** | Instantly triggers the relay for a specific door index (1–4). |
| **0x8E** | **Write Config** | Sets Door Mode (Controlled/Always Open/Always Closed) and Relay Delay. |
| **0x50** | **Add Privilege** | Writes a card ID, door access mask, and expiration dates to memory. |
| **0x10** | **Clear Database** | Clears all user data from the controller. |
| **0x11** | **Reset Counter** | Resets the internal user/privilege counter to zero. |
| **0x20** | **Run Status** | Requests a "heartbeat" containing real-time relay, sensor, and swipe data. |
| **0x30** | **Time Sync** | Updates the internal RTC to ensure scheduled access and logs are accurate. |

## 4. Programmable Hardware Logic
The N3000 controller can be programmed with advanced behavioral rules.

### A. Access Control Rules
*   **Time Segments:** Define up to 3 active windows per day for specific groups.
*   **Holiday Tables:** Define dates where standard weekly schedules are ignored.
*   **Multi-Card Authentication:** Requires 2-4 authorized cards swiped sequentially to unlock.
*   **First-Card Opening:** Keeps the door locked until a designated "Manager" arrives.

### B. Security & Safety Modes
*   **Anti-Passback (APB):** Tracks "In" and "Out" states to prevent card sharing.
*   **Interlocking (Mantrap):** Prevents Door 2 from opening unless Door 1 is physically closed.
*   **Fire Linkage:** An input trigger that can be programmed to automatically release all relays for emergency egress.

## 5. Alarm and Event Monitoring
The controller proactively monitors its ports and generates specific event codes.

*   **Forced Entry:** Triggered when a door sensor opens without a valid card or button press.
*   **Door Ajar:** Triggered if a door remains open past the "Max Open Time" limit.
*   **Duress Alarms:** Triggered when a user enters a secret PIN on a keypad reader during a card swipe.
*   **Swipe Logs:** The controller maintains a rolling circular buffer of events; large log batches can be retrieved via Flash Query commands.

## 6. Maintenance and Security Protocols

### Password & IP Protection
*   **Comm Password:** Access can be restricted with a communication password (Default: `654321`).
*   **IP Whitelist:** The controller can be programmed to ignore all commands except those from up to 9 authorized administrative IP addresses.

### Hardware Reset Procedure
If administrative access is lost, a physical bypass exists:
**Procedure:** Short **pin 2 (SDA)** and **pin 3 (GND)** on the 5-pin white socket for **3+ seconds** to restore factory default password and IP settings.

## 7. Operational Guidelines (When/Where)
*   **When to use 1024-byte packets:** Use for bulk user uploads (with names) or deep historical log retrieval.
*   **Where to store user data:** User records (Card ID, Door Mask, Expiry) are stored in the controller's internal privilege table starting at the address defined in the configuration.
*   **When to Time Sync:** It is recommended to sync the time daily to prevent drift, which could lead to unauthorized access via expired cards.
