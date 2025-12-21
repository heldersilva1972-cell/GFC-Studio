# GFC Camera System - DDNS Configuration

**Purpose:** Store DDNS configuration for reference and recovery  
**Security:** This file is safe to commit to GitHub (no secrets)

---

## 🌐 DDNS Service Information

**Provider:** DuckDNS  
**Website:** https://www.duckdns.org/  
**Hostname:** `yourclub.duckdns.org`  
**Subdomain:** `yourclub`  

**Account Login:**
- Sign in with: Google (or GitHub/Reddit)
- Account email: [Your email - add this manually, not committed]

---

## 📋 Configuration Details

### **Update Frequency:**
- Every 5 minutes via scheduled task

### **Update Script Location:**
- `C:\Scripts\update-duckdns.ps1`

### **Scheduled Task Name:**
- `DuckDNS Update`

### **Update URL Format:**
```
https://www.duckdns.org/update?domains=yourclub&token=YOUR_TOKEN
```

**⚠️ SECURITY NOTE:** The actual token is stored locally on the Video Agent PC only, NOT in GitHub!

---

## 🔐 Secret Storage Locations

**These are stored LOCALLY only (not in GitHub):**

| Secret | Location | How to Retrieve |
|--------|----------|-----------------|
| DuckDNS Token | `C:\Scripts\update-duckdns.ps1` | Open file and copy token |
| DuckDNS Token | DuckDNS website | Log in to https://www.duckdns.org/ |

**To retrieve your token:**
1. Go to: https://www.duckdns.org/
2. Sign in with Google (or your chosen method)
3. Token is displayed at top of page

---

## 🔧 How to Update DDNS Configuration

### **If You Need to Change Hostname:**

1. **Create new subdomain on DuckDNS:**
   - Go to: https://www.duckdns.org/
   - Sign in
   - Add new subdomain (e.g., `newclub`)

2. **Update local script:**
   ```powershell
   # Edit C:\Scripts\update-duckdns.ps1
   # Change line:
   $domain = 'yourclub'  # Change to 'newclub'
   ```

3. **Update VPN config on web app server:**
   ```ini
   # Edit webapp-client.conf
   # Change line:
   Endpoint = yourclub.duckdns.org:51820  # Change to newclub.duckdns.org:51820
   ```

4. **Restart VPN on web app server:**
   ```bash
   sudo wg-quick down wg0
   sudo wg-quick up wg0
   ```

---

### **If Token is Compromised:**

1. **Delete old subdomain on DuckDNS:**
   - Log in to https://www.duckdns.org/
   - Click trash icon next to old subdomain

2. **Create new subdomain:**
   - Add new subdomain with same or different name
   - New token will be generated automatically

3. **Update local script:**
   ```powershell
   # Edit C:\Scripts\update-duckdns.ps1
   # Update both token and domain if changed
   $token = 'NEW_TOKEN_HERE'
   $domain = 'yourclub'
   ```

4. **Restart scheduled task:**
   ```powershell
   Unregister-ScheduledTask -TaskName "DuckDNS Update" -Confirm:$false
   # Then re-run setup script from CAMERA_PHASE_1_LOCAL_PC_SETUP.md Step 1.7
   ```

---

## 🧪 Testing DDNS

### **Check if hostname resolves:**
```cmd
nslookup yourclub.duckdns.org
```

**Expected output:**
```
Server:  UnKnown
Address:  192.168.1.1

Non-authoritative answer:
Name:    yourclub.duckdns.org
Address:  73.45.123.89  (your public IP)
```

### **Check if update is working:**
```powershell
# Manually trigger update
C:\Scripts\update-duckdns.ps1

# Check DuckDNS website
# Go to: https://www.duckdns.org/
# Sign in
# Should show "last updated: just now"
```

### **Check scheduled task:**
```powershell
Get-ScheduledTask -TaskName "DuckDNS Update"
```

**Expected output:**
```
TaskName          State
--------          -----
DuckDNS Update    Ready
```

---

## 🔄 Disaster Recovery

### **If Video Agent PC is Lost/Rebuilt:**

1. **Retrieve token from DuckDNS:**
   - Go to: https://www.duckdns.org/
   - Sign in
   - Copy token from top of page

2. **Re-run setup from documentation:**
   - Follow `CAMERA_PHASE_1_LOCAL_PC_SETUP.md` Step 1.7
   - Use same subdomain name
   - Use retrieved token

3. **No changes needed on web app server:**
   - VPN config still points to same hostname
   - Will automatically reconnect once Video Agent PC is back online

---

## 📊 Current Configuration Summary

**Last Updated:** 2025-12-21

| Setting | Value |
|---------|-------|
| **DDNS Provider** | DuckDNS |
| **Hostname** | `yourclub.duckdns.org` |
| **Subdomain** | `yourclub` |
| **Update Frequency** | Every 5 minutes |
| **Update Method** | PowerShell script + Scheduled Task |
| **Script Location** | `C:\Scripts\update-duckdns.ps1` |
| **Task Name** | `DuckDNS Update` |
| **Used By** | Web app VPN client (Endpoint) |

---

## 🔗 Related Documentation

- **Setup Guide:** `CAMERA_PHASE_1_LOCAL_PC_SETUP.md` (Step 1.7)
- **IP Address Plan:** `CAMERA_IP_ADDRESS_PLAN.md`
- **VPN Configuration:** Web app server `webapp-client.conf`

---

## 📝 Change Log

| Date | Change | Reason |
|------|--------|--------|
| 2025-12-21 | Initial setup with DuckDNS | Free, no confirmation needed |

---

## ⚠️ Security Reminders

**NEVER commit these to GitHub:**
- ❌ DuckDNS token
- ❌ VPN private keys
- ❌ NVR passwords
- ❌ API keys

**If accidentally committed:**
1. Immediately rotate the secret (create new token/key)
2. Remove from Git history using `git filter-branch` or BFG Repo-Cleaner
3. Force push to GitHub
4. Update all systems with new secret

---

## 📞 Support

**DuckDNS Issues:**
- Website: https://www.duckdns.org/
- FAQ: https://www.duckdns.org/faqs.jsp
- No official support, but community forums available

**Alternative DDNS Providers:**
- No-IP: https://www.noip.com/
- Dynu: https://www.dynu.com/
- Cloudflare (if you own a domain): https://www.cloudflare.com/

---

**This file is safe to commit to GitHub and provides all non-secret configuration details for recovery and troubleshooting.**
