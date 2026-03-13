# Comprehensive Guide: Remote Door Controller WireGuard Setup

This document is a complete, step-by-step master guide for configuring the GFC Web App to communicate with a remote, hardcoded Door Controller. It covers physical cabling, router configurations, VPN tunneling, and troubleshooting.

>**The Core Problem:** The Door Controller has a hardcoded IP of `192.168.0.196` and a Gateway of `192.168.0.1`. If the Home Server's network also uses `192.168.0.x`, the controller falls into an "ARP Trap"—it receives VPN packets but tries to reply locally instead of sending them back through the VPN. 
>**The Solution:** The Home Network must be changed to a completely different subnet (e.g., `192.168.5.x`) to force the controller to route replies through the VPN gateway.

---

## 1. Hardware Overview & Physical Cabling

### Location A: Home Server Environment
1. **Motorola Cable Modem/Router:** Provides internet to the house.
2. **Windows Server:** Hosts the GFC Web App and the WireGuard VPN App.
   * **Cabling:** Standard Ethernet cable connecting the Windows Server's network port directly to one of the **LAN ports (1-4)** on the back of the Motorola router.

### Location B: Remote Club Environment
1. **Comcast Business Router:** The main internet gateway for the club.
2. **TP-Link ER605 VPN Gigabit Router:** Acts as the VPN receiver and the dedicated gateway for the Door Controller.
   * **Cabling:** Ethernet cable from a **LAN port** on the Comcast router connecting to the **WAN port** on the TP-Link ER605.
3. **Door Controller Hardware:**
   * **Cabling:** Ethernet cable connecting the Door Controller's network port directly to a **LAN port** on the TP-Link ER605.

---

## 2. Phase 1: Home Network Setup (The "Client" Side)

### A. Motorola Router Configuration (Home)
We must ensure the home subnet does not conflict with the club's `.0.x` subnet.
1. Log into the Motorola Router (default is usually `http://192.168.0.1` or `192.168.1.1`).
2. Go to **Basic Router -> Setup** (or similar LAN Configuration page).
3. Change the **IPv4 Address (LAN Primary)** to **`192.168.5.1`**.
4. Save and allow the router to reboot. *(Note: All devices in your house will briefly disconnect and get new `.5.x` addresses).*
5. Log back in using `http://192.168.5.1`.
6. Go to **DHCP** settings. Ensure the starting/ending IP range is now on the `.5` subnet (e.g., `192.168.5.2` to `192.168.5.253`).
7. **Lock the Server IP:** Go to **DHCP Reservation** (or Static IP Assignment). Find the Windows Server in the client list (by its MAC Address). Assign it a permanent reserved IP: **`192.168.5.248`**. Save.
8. *Note: No Port Forwarding is needed on the home router because the Windows Server initiates the connection outward.*

### B. Windows Server Network Configuration
1. Reboot the Windows Server (or unplug/replug the ethernet cable) so it receives its new `192.168.5.248` IP address.
2. **Fix Firewall Block:** Windows often flags subnet changes as "Public" networks.
   * Open **Windows Defender Firewall**.
   * Click **Turn Windows Defender Firewall on or off**.
   * Turn **OFF** the firewall for **Private** and **Public** networks to ensure VPN traffic isn't blocked.

---

## 3. Phase 2: Club Network Setup (The "Server/Listener" Side)

### A. Comcast Business Router Configuration
The Comcast router needs to pass incoming VPN traffic specifically to the TP-Link.
1. Connect a laptop to the Comcast Wi-Fi or physically plug into a Comcast LAN port.
2. Log into the Comcast router (`http://192.168.1.1`).
3. **Lock the TP-Link IP:**
   * Go to **Connected Devices -> Devices**.
   * Find the TP-Link ER605 in the list. Click **Edit**.
   * Change Configuration to **Reserved IP** and set it to **`192.168.1.250`**. Save.
4. **Port Forwarding:**
   * Go to **Advanced -> Port Forwarding**.
   * Create a new rule for **WireGuard**.
   * Service Type: **UDP**
   * Server IPv4 Address: **`192.168.1.250`** (The TP-Link's locked IP).
   * Start Port & End Port: **`51820`**
   * Save the rule.

### B. TP-Link ER605 Configuration
1. Connect a laptop physically to a **LAN port** on the TP-Link (or its specific Wi-Fi if enabled).
2. Log into the TP-Link UI (Default usually `192.168.0.1`).
3. **Verify WAN:** On the main Status page, ensure the WAN IPv4 address reads `192.168.1.250`.
4. **Set LAN Subnet:** Go to **Network -> LAN**. Ensure the IP Address is set perfectly to **`192.168.0.1`**. *(This is critical, as it acts as the hardcoded Gateway the Door Controller expects).*
5. **Firmware:** Ensure firmware is upgraded to v2.3.3 or higher to prevent UI bugs and allow Domain Name routing.

---

## 4. Phase 3: WireGuard VPN Configuration

### A. Setup TP-Link ER605 (The Listener)
1. In the TP-Link UI, go to **VPN -> WireGuard**.
2. **Main WireGuard Tab:**
   * Click **Add** (or Edit your existing profile).
   * Name: `S2S_Tunnel`
   * MTU: `1420`
   * Listen Port: **`51820`**
   * Public Key: *(Copy this string, you will need it for the Windows Server).*
   * Local IP Address: **`10.8.0.2`**
   * Enable: Checked. Save.
3. **Peers Tab:**
   * Click **Add**.
   * Interface: `S2S_Tunnel`
   * Public Key: *(Paste the Public Key from the Windows Server's WireGuard configuration).*
   * Endpoint: *(Leave Blank).*
   * Endpoint Port: *(Leave Blank).*
   * Allowed Address: **`10.8.0.0/24`** *(IMPORTANT: Use only ONE row here. Do not try to add commas or multiple rows for the same public key, it will crash the handshake).*
   * Persistent Keepalive: `25`
   * Enable: Checked. Save.

### B. Setup Windows Server (The Caller)
1. Remote Desktop into the Windows Server at home.
2. Open the **WireGuard** application.
3. Click **Edit** on your tunnel (or create a new empty tunnel).
4. Configure exactly as follows:
   ```ini
   [Interface]
   PrivateKey = (Auto-generated by WireGuard, do not change)
   Address = 10.8.0.1/24
   # IMPORTANT: Do NOT include a 'ListenPort = 51820' line here.
   # By omitting it, the Server picks a random port, bypassing any stuck NAT rules at home.

   [Peer]
   PublicKey = (Paste the Public Key from the TP-Link ER605 main WireGuard tab)
   AllowedIPs = 10.8.0.2/32, 192.168.0.196/32
   Endpoint = (The Club's Comcast Public IP Address):51820  (e.g., 73.249.88.209:51820)
   PersistentKeepalive = 25
   ```
5. Click **Save**.
6. Click **Activate**. Verify under the `Transfer` statistics that it shows bytes **Received**. If it says `0 B received`, the handshake failed (see Troubleshooting).

---

## 5. Phase 4: Door Controller & Web App Verification

1. On the Windows Server, open PowerShell.
2. Run exactly: `ping 192.168.0.196`
3. You should receive successful replies. (If not, verify cables and that the TP-Link LAN is `192.168.0.1`).
4. Open the **GFC Web App** on the Server.
5. Go to the Door Controller settings.
   * Static IP: `192.168.0.196`
   * Gateway: `192.168.0.1`
   * **Allowed PC IP (Security):** MUST BE BLANK.
6. Apply settings. The Controller indicator in the Web App will turn Green (Online).

---

## 6. Troubleshooting & Future-Proofing

### TP-Link Power Outage & System Time Sync
If the TP-Link router loses power, it may forget its internal system time and revert to a past year (e.g., 2025). 
Because WireGuard has strict "Anti-Replay" security protection, if the TP-Link attempts to send a VPN handshake from 2025 to a Windows Server living in 2026, the Windows Server will assume it is a hacker and instantly block the tunnel.

**Symptoms of a Desynced Clock:**
* The tunnel says `0 B received` even if the Public IP addresses are perfectly correct.
* The TP-Link's main Status page shows the wrong System Time.

**The Fix:**
1. Log into the TP-Link router (`192.168.0.1`).
2. Go to **Preferences -> System Time**.
3. Ensure the router is set to "Get Dynamically from NTP Server" or manually correct the date to match today's date.
4. Go to the Windows Server, Deactivate the WireGuard tunnel, wait 3 seconds, and Reactivate it to force a fresh handshake using the corrected time.

---

### The #1 Vulnerability: Dynamic Public IP Changes
The entire setup hinges on the Windows Server knowing the exact Public IP address of the Club's Comcast modem (the `Endpoint` value in the Server's WireGuard config). 
Comcast Business IPs rarely change, but a prolonged power outage or hardware replacement can cause Comcast to assign a new Public IP.

**Symptoms of an IP Change:**
* The Web App suddenly says the controller is Offline.
* WireGuard on the Server shows "Latest Handshake: (hours ago)" and `0 B received`.

**Manual Fix (5 minutes):**
1. At the club (on a laptop connected to Comcast WiFi), google `What is my IP`.
2. Copy the IPv4 address shown.
3. Remote into the Windows Server at home.
4. Edit the WireGuard tunnel and paste the new IP into the `Endpoint=` line (keep the `:51820` at the end).
5. Deactivate and Reactivate.

**Permanent Fix using Dynamic DNS (DDNS):**
The TP-Link ER605 supports DDNS, which automatically tracks Public IP changes and updates a permanent URL.
1. Log into the TP-Link router at the club.
2. Go to **Network -> DDNS** (or Services -> Dynamic DNS).
3. Select **TP-Link** as the service provider.
4. Register a name (e.g., `gfc-club.tplinkdns.com`) and Enable the service. The router will now keep this URL updated with Comcast's current IP.
5. Go to the Windows Server at home, Edit the WireGuard tunnel, and change the Endpoint to the URL:
   `Endpoint = gfc-club.tplinkdns.com:51820`
   
This completely immunizes the VPN against future IP address changes.
