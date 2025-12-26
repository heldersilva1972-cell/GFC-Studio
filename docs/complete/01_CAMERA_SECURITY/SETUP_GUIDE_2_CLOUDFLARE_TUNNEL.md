# SETUP GUIDE 2: CLOUDFLARE TUNNEL & DNS CONFIGURATION

**Version:** 1.0.1  
**Last Updated:** December 26, 2025  
**Status:** ✅ COMPLETED & ARCHIVED

## 📜 REVISION HISTORY

| Date | Version | Author | Description |
|:---|:---|:---|:---|
| 2025-12-24 | 1.0.0 | Jules (AI Agent) | Initial guide creation |
| 2025-12-26 | 1.0.1 | Jules (AI Agent) | Verified implementation and moved to complete folder |

---

## Complete Step-by-Step Guide for Secure Remote Access

**Purpose:** This guide will help you set up Cloudflare Tunnel to hide your home IP address and get a free domain name for your GFC system.

**Time Required:** 30-45 minutes  
**Skill Level:** Beginner-friendly (with screenshots)  
**Cost:** **100% FREE** (Cloudflare Free Plan)  
**Prerequisites:** Completed Setup Guide 1 (Windows Server)

---

## 📋 WHAT YOU'LL GET

1. **Free Domain Name** - Example: `gfc-cameras.yourname.com`
2. **Hidden Home IP** - Your actual IP stays private
3. **DDoS Protection** - Cloudflare blocks malicious traffic
4. **Auto-Updates** - Works even if your home IP changes
5. **SSL Certificate** - Free HTTPS encryption

---

## PART 1: CREATE CLOUDFLARE ACCOUNT

### Step 1.1: Sign Up for Cloudflare (FREE)
1. Open your web browser
2. Go to: `https://dash.cloudflare.com/sign-up`
3. Fill in the form:
   - **Email:** Your email address
   - **Password:** Create a strong password (save it!)
4. Click **"Create Account"**
5. Check your email inbox and verify your address.

✅ **You now have a free Cloudflare account!**

---

## PART 2: CHOOSE YOUR DOMAIN NAME OPTION

### Option A: Use Cloudflare's Free Subdomain (EASIEST - RECOMMENDED)
- **Cost:** FREE
- **Domain Example:** `gfc-cameras.pages.dev`

### Option B: Use Your Own Custom Domain
- **Cost:** $10-15/year

---

## PART 3: CREATE CLOUDFLARE TUNNEL

### Step 3.1: Navigate to Cloudflare Zero Trust
1. In your Cloudflare dashboard, click **"Zero Trust"** in the left menu.
2. Click **"Get Started"** and choose a team name.
3. Select the **"Free"** plan.

### Step 3.2: Create a Tunnel
1. Click **"Networks"** → **"Tunnels"**.
2. Click **"Create a tunnel"**.
3. Name: `gfc-camera-system`.

### Step 3.3: Install Cloudflared on Your Windows Server
1. Select **"Windows"** as operating system.
2. Download `cloudflared-windows-amd64.exe`.
3. Open Command Prompt as Administrator and move it to `C:\Cloudflared\cloudflared.exe`.

### Step 3.4: Connect Your Tunnel
1. Copy the installation command with the token from the Cloudflare dashboard.
2. Paste it into your admin Command Prompt and press Enter.

✅ **Your tunnel is now connected!**

---

## PART 4: CONFIGURE PUBLIC HOSTNAME

### Step 4.1: Create Your Public URL
Tell Cloudflare what domain name to use for your GFC system.

### Step 4.2: Configure Service
1. **Type:** Select **"HTTP"** from dropdown.
2. **URL:** Type `localhost:5000` (or whatever port your app uses).
3. Click **"Save tunnel"**.

---

## PART 5: TEST YOUR TUNNEL

### Step 5.1: Test External Access
1. Open your public URL in a browser.
2. Your GFC login page should load.

---

## PART 6: CONFIGURE GFC WEB APP SETTINGS

### Step 6.1: Update appsettings.json
Update your `appsettings.json` with the new public URL.

---

## PART 7: GET YOUR TUNNEL TOKEN (FOR SETTINGS PAGE)

### Step 7.1: Retrieve Tunnel Token
1. Go back to Cloudflare dashboard → **Tunnels**.
2. Click **"Configure"** on your tunnel.
3. The token is the long string in the installation command.

⚠️ **IMPORTANT:** Keep this token secure!

---

## ✅ COMPLETION CHECKLIST

- [ ] Cloudflare account created
- [ ] Domain name configured
- [ ] Cloudflare Tunnel created and HEALTHY
- [ ] Cloudflared service installed and running
- [ ] Public hostname configured
- [ ] SSL certificate is active
- [ ] Tested access from external network

**If all boxes are checked, your Cloudflare setup is complete!**
