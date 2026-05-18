---
trigger: always_on
---

# PROJECT TECHNOLOGY & INFRASTRUCTURE GUIDELINES

## 1. Stack Focus
- **Advanced .NET:** Implementations utilize .NET 10, Blazor (Server/WebAssembly), and .NET MAUI. Beware of namespace ambiguities in hybrid setups.

## 2. Edge Hardware & Network Integration
- **Peripherals & Networking:** Code interacting with hardware (RFID card readers, thermal printers via raw TCP/IP sockets) or secure tunnels (WireGuard) must prioritize secure local loopback communications, strict error handling, and offline-safe fallback states.
- **Context Isolation:** Treat this repository as a completely distinct entity. Do not cross-pollinate architectural assumptions from past projects.