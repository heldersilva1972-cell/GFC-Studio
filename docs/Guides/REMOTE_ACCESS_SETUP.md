# Remote Access Guide: Secure VPN via Tailscale (Method 1)

This method uses **Tailscale** (based on WireGuard) to create a private, encrypted network between your devices. It is the **most secure** way to access your GFC System remotely because you do not need to open any ports on your router.

---

## Phase 1: Create Your Account
1.  Go to [tailscale.com](https://tailscale.com) in your web browser.
2.  Click **"Get Started"** or **"Log in"**.
3.  Sign in using an existing identity provider (Google, Microsoft, GitHub, or Apple). This ensures your network is protected by your email's security (like 2FA).

---

## Phase 2: Setup the GFC "Station" (This Computer)
*Perform these steps on the computer running the GFC Video Agent and Web App.*

1.  **Download:** On the [tailscale.com/download](https://tailscale.com/download) page, select **Windows**.
2.  **Install:** Run the installer (`Tailscale-setup.exe`).
3.  **Log In:**
    *   Once installed, the Tailscale icon will appear in your system tray (near the clock, you might have to click the `^` arrow).
    *   Click the icon and select **"Log in"**.
    *   A browser window will open. Click **"Connect"** or authorize the login.
4.  **Verify Name:**
    *   Go to the [Tailscale Admin Console](https://login.tailscale.com/admin/machines).
    *   You should see your computer listed (e.g., `desktop-abc1234`).
    *   **Write down the IP Address** listed next to it (it will look like `100.x.y.z`). This is your **Tailscale IP**.

---

## Phase 3: Setup Your Remote Device (e.g., Phone or Laptop)
*Perform these steps on the device you want to use to VIEW the cameras from elsewhere.*

**For iPhone / Android:**
1.  Go to the App Store (iOS) or Google Play Store (Android).
2.  Search for and install **"Tailscale"**.
3.  Open the app and **Log in** with the **SAME account** you used in Phase 1.
4.  Tap the toggle switch to **"Active"** (you may need to approve adding a VPN configuration).
5.  You should now see a list of your specific devices, including the GFC computer.

**For Another Laptop:**
1.  Install Tailscale from the website.
2.  Log in with the same account.

---

## Phase 4: Connecting to GFC Studio
*Now that both devices are on the "Tail Net":*

1.  Keep Tailscale **ON** (Active) on your remote device.
2.  Open your web browser (Chrome/Safari).
3.  Enter the URL using the **Tailscale IP** from Phase 2 and the GFC Port (usually 5000 or 5001).
    *   Format: `http://[TAILSCALE-IP]:5000`
    *   Example: `http://100.85.122.45:5000`
4.  The GFC Login page should appear!

### Viewing Video
The GFC Web App needs to talk to the Video Agent. Since you are on the VPN, the Web App (running on the server) can talk to the Video Agent (running on the server) via `localhost` usually.
However, if your browser tries to load the video stream directly:
*   The stream URL currently points to the Video Agent.
*   If the video doesn't play remotely, you may need to ensure the **Video Agent Base URL** setting in the GFC App is accessible.
*   **Simple Test:** Try to open `http://[TAILSCALE-IP]:5101/health` in your remote browser. If it says "Healthy" or loads text, connection is perfect.

---

## Security Note implies
*   **Only** devices logged into YOUR Tailscale account can access this network.
*   Strangers on the internet **cannot** see your cameras, even if they guess the IP, because they don't have the VPN key.
