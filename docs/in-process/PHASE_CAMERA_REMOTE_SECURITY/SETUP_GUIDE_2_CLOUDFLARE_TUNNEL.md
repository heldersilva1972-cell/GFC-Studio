# SETUP GUIDE 2: CLOUDFLARE TUNNEL & DNS CONFIGURATION
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
   - **Confirm Password:** Re-enter your password
4. Click **"Create Account"**
5. Check your email inbox
6. Click the **"Verify email address"** link in the email from Cloudflare
7. You'll be redirected to the Cloudflare dashboard

✅ **You now have a free Cloudflare account!**

---

## PART 2: CHOOSE YOUR DOMAIN NAME OPTION

You have **two options** for getting a domain name:

### Option A: Use Cloudflare's Free Subdomain (EASIEST - RECOMMENDED)
- **Cost:** FREE
- **Setup Time:** 5 minutes
- **Domain Example:** `gfc-cameras.pages.dev`
- **Best For:** Testing or if you don't want to buy a domain

### Option B: Use Your Own Custom Domain
- **Cost:** $10-15/year (from domain registrar)
- **Setup Time:** 15 minutes
- **Domain Example:** `cameras.yourclub.com`
- **Best For:** Professional setup or if you already own a domain

**Which option do you want?**
- [ ] Option A (Free - Skip to Part 3)
- [ ] Option B (Custom - Continue to Part 2B)

---

### PART 2B: OPTION B - REGISTER A CUSTOM DOMAIN (OPTIONAL)

⚠️ **Only follow this section if you chose Option B above**

#### Step 2B.1: Choose a Domain Registrar
Popular options:
- **Namecheap** - `https://www.namecheap.com` (Recommended - Easy to use)
- **Google Domains** - `https://domains.google.com`
- **GoDaddy** - `https://www.godaddy.com`

#### Step 2B.2: Search for Available Domains
1. Go to your chosen registrar's website
2. In the search box, type your desired domain name
   - Example: `gfc-cameras.com`
   - Example: `yourclubname.com`
3. Click **"Search"**
4. If available, you'll see the price (usually $10-15/year)
5. If not available, try variations:
   - `gfc-video.com`
   - `yourclub-security.com`
   - `yourclub.net`

#### Step 2B.3: Purchase the Domain
1. Click **"Add to Cart"** next to your chosen domain
2. **IMPORTANT:** When asked about extras:
   - ❌ **DECLINE** "Domain Privacy" (Cloudflare provides this free)
   - ❌ **DECLINE** "Email Hosting" (not needed)
   - ❌ **DECLINE** "Website Builder" (not needed)
   - ✅ **ACCEPT** only the domain registration itself
3. Proceed to checkout
4. Enter your payment information
5. Complete the purchase
6. **Save your registrar login info!**

#### Step 2B.4: Add Domain to Cloudflare
1. Go back to your Cloudflare dashboard: `https://dash.cloudflare.com`
2. Click **"Add a Site"** (blue button, top right)
3. Enter your domain name (example: `yourclub.com`)
4. Click **"Add Site"**
5. Select the **"Free"** plan
6. Click **"Continue"**
7. Cloudflare will scan your domain (this takes 30-60 seconds)
8. Click **"Continue"** when scan completes

#### Step 2B.5: Change Your Domain's Nameservers
Cloudflare will show you two nameservers that look like:
- `ava.ns.cloudflare.com`
- `bob.ns.cloudflare.com`

**Write these down:**
```
Nameserver 1: _______________________________
Nameserver 2: _______________________________
```

Now go back to your domain registrar:

**For Namecheap:**
1. Log into Namecheap
2. Click **"Domain List"**
3. Click **"Manage"** next to your domain
4. Find **"Nameservers"** section
5. Select **"Custom DNS"**
6. Enter the two Cloudflare nameservers
7. Click **"Save"**

**For Google Domains:**
1. Log into Google Domains
2. Click on your domain
3. Click **"DNS"** in the left menu
4. Click **"Use custom name servers"**
5. Enter the two Cloudflare nameservers
6. Click **"Save"**

**For GoDaddy:**
1. Log into GoDaddy
2. Click **"My Products"**
3. Click **"DNS"** next to your domain
4. Scroll to **"Nameservers"**
5. Click **"Change"**
6. Select **"Custom"**
7. Enter the two Cloudflare nameservers
8. Click **"Save"**

⏰ **Wait Time:** Nameserver changes can take 2-24 hours to propagate. Cloudflare will email you when it's ready.

---

## PART 3: CREATE CLOUDFLARE TUNNEL

### Step 3.1: Navigate to Cloudflare Zero Trust
1. In your Cloudflare dashboard, click **"Zero Trust"** in the left menu
   - If you don't see it, go directly to: `https://one.dash.cloudflare.com/`
2. If this is your first time:
   - You'll see a welcome screen
   - Click **"Get Started"**
   - Choose a **team name** (example: `gfc-club`)
   - This becomes part of your free subdomain: `gfc-club.cloudflareaccess.com`
   - Click **"Continue"**
3. Select the **"Free"** plan
4. Click **"Continue"**
5. You'll see the Zero Trust dashboard

### Step 3.2: Create a Tunnel
1. In the left menu, click **"Networks"**
2. Click **"Tunnels"**
3. Click **"Create a tunnel"** (blue button)
4. Select **"Cloudflared"** (should be pre-selected)
5. Click **"Next"**
6. **Tunnel Name:** `gfc-camera-system`
7. Click **"Save tunnel"**

### Step 3.3: Install Cloudflared on Your Windows Server
Cloudflare will now show you installation instructions.

1. **Select your operating system:** Click **"Windows"**
2. You'll see a download link and a command
3. Click the **"Download"** button to download `cloudflared-windows-amd64.exe`
4. Save it to your **Downloads** folder

**Now install it:**
1. Press `Windows Key + R`
2. Type: `cmd`
3. Right-click **"Command Prompt"**
4. Click **"Run as administrator"**
5. Type the following commands **one at a time**:

```cmd
cd %USERPROFILE%\Downloads
```
Press Enter

```cmd
mkdir C:\Cloudflared
```
Press Enter

```cmd
move cloudflared-windows-amd64.exe C:\Cloudflared\cloudflared.exe
```
Press Enter

```cmd
cd C:\Cloudflared
```
Press Enter

### Step 3.4: Connect Your Tunnel
Back in the Cloudflare dashboard, you'll see a command that looks like this:

```
cloudflared.exe service install <LONG_TOKEN_HERE>
```

**Copy that entire command** (click the copy icon)

1. Go back to your Command Prompt window
2. **Right-click** to paste the command
3. Press **Enter**
4. You should see: `"INFO Successfully installed cloudflared service"`

✅ **Your tunnel is now connected!**

### Step 3.5: Verify Tunnel is Active
1. Go back to the Cloudflare dashboard
2. You should see your tunnel status change to **"HEALTHY"** (green)
   - If it shows "DOWN" (red), wait 30 seconds and refresh the page
3. Click **"Next"**

---

## PART 4: CONFIGURE PUBLIC HOSTNAME

### Step 4.1: Create Your Public URL
Now you'll tell Cloudflare what domain name to use for your GFC system.

**If you're using Option A (Free Subdomain):**
1. **Subdomain:** Type `gfc-cameras` (or any name you want)
2. **Domain:** Select `<your-team-name>.cloudflareaccess.com` from dropdown
3. Your full URL will be: `gfc-cameras.<your-team-name>.cloudflareaccess.com`

**If you're using Option B (Custom Domain):**
1. **Subdomain:** Type `cameras` (or any name you want)
2. **Domain:** Select your custom domain from dropdown (example: `yourclub.com`)
3. Your full URL will be: `cameras.yourclub.com`

**Write down your full URL:**
```
Public URL: https://_________________________________
```

### Step 4.2: Configure Service
1. **Type:** Select **"HTTP"** from dropdown
2. **URL:** Type `localhost:5000`
   - ⚠️ **IMPORTANT:** This must match the port your GFC Web App runs on
   - If your app uses port 5001, type `localhost:5001` instead
   - If you're not sure, use `localhost:5000` (default)

3. Scroll down to **"Additional application settings"**
4. Click **"TLS"** section to expand it
5. **UNCHECK** "No TLS Verify" (leave it checked for now during setup)
   - We'll fix SSL later once everything is working

6. Click **"Save tunnel"**

✅ **Your public URL is now live!**

---

## PART 5: TEST YOUR TUNNEL

### Step 5.1: Test External Access
1. Open a **new browser tab** (or use your phone's browser)
2. Go to your public URL (from Step 4.1)
   - Example: `https://gfc-cameras.yourteam.cloudflareaccess.com`
3. **What you should see:**
   - ✅ **Best Case:** Your GFC login page loads
   - ⚠️ **Acceptable:** "Bad Gateway" or "502 Error" (means tunnel works, but Web App isn't running)
   - ❌ **Problem:** "This site can't be reached" (tunnel not working - see troubleshooting)

### Step 5.2: Verify Tunnel Service is Running
1. Press `Windows Key + R`
2. Type: `services.msc`
3. Press Enter
4. Scroll down to find **"Cloudflare Tunnel"** or **"cloudflared"**
5. Verify:
   - **Status:** Running
   - **Startup Type:** Automatic
6. If not running:
   - Right-click the service
   - Click **"Start"**

---

## PART 6: CONFIGURE GFC WEB APP SETTINGS

### Step 6.1: Update appsettings.json
1. Navigate to your GFC Web App folder:
   ```
   C:\Users\[YourName]\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer
   ```
2. Find the file: `appsettings.json`
3. **Right-click** it and open with **Notepad**
4. Find the line that says `"AllowedHosts": "*"`
5. Change it to include your public URL:
   ```json
   "AllowedHosts": "*",
   "PublicUrl": "https://gfc-cameras.yourteam.cloudflareaccess.com"
   ```
6. Save the file

### Step 6.2: Update Settings Page (After Implementation)
Once the Settings page is built (in Phase 2), you'll enter:
1. **Cloudflare Tunnel Token:** (We'll get this in next step)
2. **Public Domain:** Your full URL from Step 4.1

---

## PART 7: GET YOUR TUNNEL TOKEN (FOR SETTINGS PAGE)

### Step 7.1: Retrieve Tunnel Token
1. Go back to Cloudflare dashboard
2. Click **"Networks"** → **"Tunnels"**
3. Find your tunnel: `gfc-camera-system`
4. Click the **three dots** (⋮) on the right
5. Click **"Configure"**
6. Click the **"Overview"** tab
7. Scroll down to find **"Connector ID"** or look for the install command
8. The token is the long string after `cloudflared.exe service install`
9. **Copy the entire token** (it's very long - about 200 characters)

**Save your token securely:**
```
Tunnel Token: 
_________________________________________________________________
_________________________________________________________________
```

⚠️ **IMPORTANT:** This token is sensitive! Don't share it publicly.

---

## PART 8: CONFIGURE DNS (CUSTOM DOMAIN ONLY)

⚠️ **Skip this section if you're using the free Cloudflare subdomain**

### Step 8.1: Verify DNS Record Was Created
1. In Cloudflare dashboard, click on your domain name (top left)
2. Click **"DNS"** in the left menu
3. Click **"Records"**
4. You should see a **CNAME** record:
   - **Type:** CNAME
   - **Name:** cameras (or whatever subdomain you chose)
   - **Target:** `<tunnel-id>.cfargotunnel.com`
   - **Proxy status:** Orange cloud (Proxied)

✅ If you see this record, DNS is configured correctly!

### Step 8.2: Test DNS Propagation
1. Go to: `https://dnschecker.org/`
2. Enter your full domain: `cameras.yourclub.com`
3. Select **"CNAME"** from the dropdown
4. Click **"Search"**
5. You should see green checkmarks around the world
   - ✅ All green = DNS is working
   - ⚠️ Some red = Wait 5-10 minutes and check again
   - ❌ All red = Check your DNS settings in Cloudflare

---

## PART 9: ENABLE HTTPS (SSL CERTIFICATE)

### Step 9.1: Configure SSL/TLS Settings
1. In Cloudflare dashboard, click on your domain
2. Click **"SSL/TLS"** in the left menu
3. Select **"Full"** encryption mode
   - ⚠️ Do NOT select "Full (strict)" yet - we'll do that later
4. Click **"Overview"** tab
5. Verify **"Universal SSL Status"** shows **"Active Certificate"**
   - If it says "Pending", wait 15-30 minutes

### Step 9.2: Enable Always Use HTTPS
1. Click **"Edge Certificates"** (under SSL/TLS)
2. Scroll down to **"Always Use HTTPS"**
3. Toggle it **ON**
4. This forces all HTTP traffic to redirect to HTTPS

---

## PART 10: SECURITY ENHANCEMENTS (RECOMMENDED)

### Step 10.1: Enable Bot Protection
1. In Cloudflare dashboard, click **"Security"**
2. Click **"Bots"**
3. Toggle **"Bot Fight Mode"** to **ON**
4. This blocks malicious bots automatically (free feature)

### Step 10.2: Configure Firewall Rules (Optional)
To block all countries except yours:
1. Click **"Security"** → **"WAF"**
2. Click **"Create rule"**
3. **Rule name:** `Block Non-US Traffic` (adjust for your country)
4. **Field:** Country
5. **Operator:** does not equal
6. **Value:** Select your country (example: United States)
7. **Action:** Block
8. Click **"Deploy"**

⚠️ **Warning:** Only do this if all your users are in one country!

### Step 10.3: Enable Rate Limiting (Prevent Brute Force)
1. Click **"Security"** → **"WAF"**
2. Click **"Rate limiting rules"**
3. Click **"Create rule"**
4. **Rule name:** `Login Protection`
5. **If incoming requests match:**
   - **Field:** URI Path
   - **Operator:** equals
   - **Value:** `/login`
6. **Then:**
   - **Action:** Block
   - **Duration:** 1 hour
   - **Requests:** 10 requests per 1 minute
7. Click **"Deploy"**

This blocks anyone who tries to login more than 10 times in 1 minute.

---

## PART 11: DOCUMENT YOUR CONFIGURATION

Fill in this information sheet:

```
=================================================================
GFC CAMERA SYSTEM - CLOUDFLARE CONFIGURATION SHEET
=================================================================

CLOUDFLARE ACCOUNT:
- Email: _______________________________
- Team Name: ___________________________

DOMAIN INFORMATION:
- Public URL: https://____________________________________
- Domain Type: [ ] Free Subdomain  [ ] Custom Domain
- Domain Registrar: _____________________ (if custom)

TUNNEL INFORMATION:
- Tunnel Name: gfc-camera-system
- Tunnel ID: ___________________________________________
- Tunnel Token: 
  _______________________________________________________
  _______________________________________________________
- Status: [ ] HEALTHY  [ ] DOWN

DNS CONFIGURATION (Custom Domain Only):
- Nameserver 1: ________________________________________
- Nameserver 2: ________________________________________
- CNAME Record: ________________________________________

SSL/TLS SETTINGS:
- Encryption Mode: Full
- Universal SSL: [ ] Active  [ ] Pending
- Always Use HTTPS: [ ] Enabled

SECURITY FEATURES:
- Bot Fight Mode: [ ] Enabled
- Firewall Rules: [ ] Configured
- Rate Limiting: [ ] Configured

SERVICE STATUS:
- Cloudflared Service: [ ] Running  [ ] Stopped
- Startup Type: [ ] Automatic

NEXT STEPS:
- [ ] Test public URL from external network
- [ ] Configure GFC Web App settings
- [ ] Test VPN setup flow

=================================================================
```

---

## 🔧 TROUBLESHOOTING

### Problem: Tunnel shows "DOWN" status
**Solution:**
1. Check if cloudflared service is running (services.msc)
2. Restart the service
3. Check Windows Event Viewer for errors
4. Verify your tunnel token is correct

### Problem: "This site can't be reached"
**Solution:**
1. Verify tunnel status is "HEALTHY" in Cloudflare
2. Check your public hostname configuration
3. Verify GFC Web App is running on the correct port
4. Try accessing `localhost:5000` directly on the server

### Problem: "Bad Gateway" or "502 Error"
**Solution:**
1. This means the tunnel works, but can't reach your app
2. Verify GFC Web App is running
3. Check the port number in tunnel config matches your app
4. Try changing `localhost:5000` to `127.0.0.1:5000`

### Problem: SSL certificate shows "Pending"
**Solution:**
1. Wait 15-30 minutes (certificate generation takes time)
2. Verify your domain's nameservers are pointing to Cloudflare
3. Check DNS propagation at dnschecker.org
4. If still pending after 24 hours, contact Cloudflare support

### Problem: Nameserver change not working
**Solution:**
1. Verify you entered the exact nameservers from Cloudflare
2. Check for typos (they're case-sensitive)
3. Wait 2-4 hours and check again
4. Contact your domain registrar if still not working after 24 hours

### Problem: Can't access from external network
**Solution:**
1. Verify you're testing from a different network (not your home WiFi)
2. Try using mobile data on your phone
3. Check firewall rules in Cloudflare
4. Verify tunnel is HEALTHY

---

## ✅ COMPLETION CHECKLIST

Before moving to the next phase, verify:

- [ ] Cloudflare account created
- [ ] Domain name configured (free subdomain or custom)
- [ ] Cloudflare Tunnel created and status is HEALTHY
- [ ] Cloudflared service installed and running
- [ ] Public hostname configured
- [ ] SSL certificate is active
- [ ] Tested access from external network
- [ ] Tunnel token saved securely
- [ ] Configuration sheet filled out
- [ ] Security features enabled (bot protection, rate limiting)

**If all boxes are checked, your Cloudflare setup is complete!**

---

## 📊 WHAT YOU'VE ACCOMPLISHED

✅ **Your home IP is now hidden** - Attackers can't find your actual location  
✅ **You have a professional URL** - No more typing IP addresses  
✅ **Free SSL certificate** - All traffic is encrypted  
✅ **DDoS protection** - Cloudflare blocks malicious traffic  
✅ **Works with dynamic IP** - No more worrying about IP changes  
✅ **Bot protection** - Automated attacks are blocked  
✅ **Rate limiting** - Brute force attacks are prevented  

---

## 🎓 UNDERSTANDING WHAT YOU BUILT

**Before Cloudflare:**
```
User → Your Home IP (123.45.67.89:5000) → Your Computer → GFC App
      ❌ IP exposed
      ❌ No DDoS protection
      ❌ Breaks if IP changes
```

**After Cloudflare:**
```
User → Cloudflare (gfc-cameras.yourteam.com) → Tunnel → Your Computer → GFC App
      ✅ IP hidden
      ✅ DDoS protection
      ✅ Works even if IP changes
      ✅ Free SSL
```

The "tunnel" is a secure, encrypted connection from your computer to Cloudflare. It's always running in the background, waiting for incoming requests.

---

## 📞 NEED HELP?

**Cloudflare Support:**
- Community Forum: `https://community.cloudflare.com/`
- Documentation: `https://developers.cloudflare.com/cloudflare-one/`
- Email Support: Available on paid plans only

**Common Questions:**

**Q: Is the free plan really free forever?**  
A: Yes! Cloudflare's free plan has no time limit. You can use it indefinitely.

**Q: What if I want to change my domain later?**  
A: You can easily add a new public hostname in the tunnel settings. The old one will stop working.

**Q: Can I use this for other services too?**  
A: Yes! You can add multiple public hostnames to the same tunnel (example: `app.yourclub.com`, `cameras.yourclub.com`, etc.)

**Q: What happens if Cloudflare goes down?**  
A: Cloudflare has 99.99% uptime. If they go down, your remote access won't work, but local (LAN) access will still work fine.

**Q: Do I need to renew anything?**  
A: If you used a custom domain, you'll need to renew it yearly with your registrar ($10-15/year). The Cloudflare tunnel itself never expires.

---

**Next Guide:** [SETUP_GUIDE_3_GFC_WEB_APP_CONFIGURATION.md](./SETUP_GUIDE_3_GFC_WEB_APP_CONFIGURATION.md)

---

## 🎉 CONGRATULATIONS!

You've successfully set up enterprise-grade security for your GFC Camera System! Your home IP is now protected, and you have a professional domain name that will work reliably for years to come.

**What's Next?**
1. Configure the GFC Web App settings (Guide 3)
2. Test the VPN setup flow
3. Add authorized users
4. Start monitoring your cameras remotely!
