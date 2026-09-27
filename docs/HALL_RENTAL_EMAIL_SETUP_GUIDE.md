# Hall Rentals Email Configuration & Inbound Reply Sync Guide
**Gloucester Fraternity Club (GFC) - Studio & Web Application Architecture**  
*Document Version: 1.0 | Target Environment: Local Host & Live Production*

---

## Executive Overview
The GFC Hall Rentals module is designed to handle bidirectional email communication with rental applicants and inquiring patrons:
1. **Outbound Emails:** Confirmation receipts, booking approvals, payment reminders, cancellation notices, and manual staff responses.
2. **Inbound Replies:** When an applicant replies to an email from their phone or mail client (Gmail, Outlook, Apple Mail), the system can automatically capture their reply and log it into the Hall Rental conversation history.

Because your web application is hosted locally on your home/studio server while your official domain (`gloucesterfraternityclub.com`) is hosted on a remote web hosting provider, you have **two primary architectural pathways** to configure email sending and receiving.

---

## Architecture Comparison Matrix

| Criteria | Option 1: Website Direct Mailbox (IMAP & SMTP) | Option 2: Resend API & Inbound Webhooks |
| :--- | :--- | :--- |
| **Best Used For** | Standard domain mailboxes (cPanel, Google Workspace, Microsoft 365, web hosting email). | Modern API delivery with real-time webhooks, high deliverability, and bounce tracking. |
| **Outbound Sending Protocol** | SMTP (Port 587 TLS or 465 SSL) | HTTPS REST API (`api.resend.com`) |
| **Inbound Receiving Protocol** | Background IMAP Poller (Port 993 SSL) | Real-time HTTP POST Webhook via Cloudflare Tunnel |
| **Network Requirements** | Pure outbound internet connection (No open ports, no port forwarding). | Requires a public ingress tunnel (e.g. Cloudflare Tunnel). |
| **DNS Configuration Needed** | Standard domain MX records already on your web host. | Requires adding Resend DKIM/SPF TXT records and Inbound MX records. |
| **Sync Speed** | Scheduled interval (e.g., every 60 seconds). | Instantaneous (Sub-second real-time push). |

---

---

# OPTION 1: Website Direct Mailbox (SMTP Sending + IMAP Sync)

In this setup, your locally hosted web application connects directly to your website's existing mail server.

```mermaid
flowchart LR
    subgraph WebApp [Local GFC Web Application]
        Sender[SMTP Client]
        Receiver[IMAP Sync Worker]
        DB[(Local SQL Database)]
    end

    subgraph Host [Remote Web Host / cPanel Mail Server]
        Mailbox[rentals@gloucesterfraternityclub.com]
    end

    subgraph Customer [Applicant Device]
        EmailClient[Applicant Mail App]
    end

    Sender -->|1. Outbound SMTP| Mailbox
    Mailbox -->|2. Delivers Email| EmailClient
    EmailClient -->|3. Customer Replies| Mailbox
    Receiver -->|4. Checks IMAP & Pulls Replies| Mailbox
    Receiver -->|5. Logs to History| DB
```

---

### Step 1: Gather Your Website's Mail Server Credentials
Log in to your web hosting control panel (cPanel, Plesk, Hostinger, GoDaddy, Google Workspace, etc.) and locate your email account settings for `rentals@gloucesterfraternityclub.com`:
- **Incoming Server (IMAP):** e.g., `mail.gloucesterfraternityclub.com` (Port `993`, SSL/TLS)
- **Outgoing Server (SMTP):** e.g., `mail.gloucesterfraternityclub.com` (Port `587`, STARTTLS or Port `465`, SSL)
- **Username / Account:** `rentals@gloucesterfraternityclub.com`
- **Password:** The mailbox password created in your hosting control panel.

---

### Step 2: Configure Outbound SMTP in the Web App
1. Open the GFC Web Application and log in as Administrator.
2. Navigate to **Hall Rentals & Calendar Settings &rarr; Email & Notifications**.
3. Under **1. Outgoing Delivery Engine**, select **SMTP Server**.
4. Fill in the following fields:
   - **SMTP Host / Server:** `mail.gloucesterfraternityclub.com` (or your host's specific hostname)
   - **Port:** `587`
   - **SMTP Username / Mailbox:** `rentals@gloucesterfraternityclub.com`
   - **SMTP Password:** Enter your mailbox password
   - **Enable TLS / SSL Encryption:** Checked (`ON`)
   - **Sender Email Address (From):** `rentals@gloucesterfraternityclub.com`
   - **Sender Display Name:** `Gloucester Fraternity Club - Hall Rentals`
5. In the **Test Connection & Send Test Email** box:
   - Enter your personal email address and click **Test Send**.
   - Verify that you receive the green success alert and the test email in your inbox.
6. Click **Save All Settings** at the top right of the page.

---

### Step 3: Verify Inbound Customer Replies (IMAP Sync)
1. In the Web App, ensure that **Notification Triggers** has **Applicant Inbound Email Replies (Auto-Logged)** enabled.
2. When you send an inquiry reply or booking message from the web app, the message subject will automatically include a tracking token like `[GFC-INQ-104]` or `[GFC-RENT-205]`.
3. When the applicant replies to that email, the response arrives in your `rentals@gloucesterfraternityclub.com` inbox on your website host.
4. The background IMAP synchronization worker connects to your mailbox, finds the matching tracking token, and logs the customer's response directly under the applicant's record inside **Hall Rentals**.
5. You can also log into webmail or open Outlook/Gmail on your phone at any time to read and manage the email directly.

---

---

# OPTION 2: Resend API & Inbound Webhooks

In this setup, Resend handles both outbound delivery (with high sender reputation) and captures inbound replies via an automated Webhook pushed to your web app.

```mermaid
flowchart LR
    subgraph WebApp [Local GFC Web Application]
        Dispatcher[Resend API Dispatcher]
        WebhookEndpoint[/api/webhooks/resend-inbound]
        DB[(Local SQL Database)]
    end

    subgraph Resend [Resend Cloud Service]
        API[Resend REST API]
        InboundWorker[Resend Inbound Router]
    end

    subgraph Tunnel [Cloudflare Tunnel]
        CloudflareEdge[your-domain.com / Tunnel]
    end

    subgraph Customer [Applicant Device]
        EmailClient[Applicant Mail App]
    end

    Dispatcher -->|1. HTTPS API Send| API
    API -->|2. Delivers with DKIM/SPF| EmailClient
    EmailClient -->|3. Customer Replies| InboundWorker
    InboundWorker -->|4. HTTP Webhook POST| CloudflareEdge
    CloudflareEdge -->|5. Forward to Local App| WebhookEndpoint
    WebhookEndpoint -->|6. Logs to History| DB
```

---

### Step 1: Verify Your Domain in Resend
1. Log in to your [Resend Dashboard](https://resend.com).
2. Go to **Domains &rarr; Add Domain**.
3. Enter your domain: `gloucesterfraternityclub.com` and select your region (North America / us-east-1).
4. Resend will provide DNS records:
   - **DKIM (TXT):** `resend._domainkey.gloucesterfraternityclub.com`
   - **SPF (TXT):** `v=spf1 include:resend.com ~all`
5. Log in to your domain DNS provider (Cloudflare, GoDaddy, Namecheap, etc.) and add these DNS records.
6. Click **Verify Records** in Resend until the status shows **Verified** (green checkmark).

---

### Step 2: Configure Resend Outbound in the Web App
1. In the Resend Dashboard, go to **API Keys &rarr; Create API Key**.
   - **Name:** `GFC Studio Webapp`
   - **Permission:** `Sending access` (or `Full access`)
   - **Domain:** `gloucesterfraternityclub.com` (or All Domains)
   - Copy the generated API key (it starts with `re_`).
2. Open the GFC Web Application &rarr; **Hall Rentals & Calendar Settings &rarr; Email & Notifications**.
3. Under **1. Outgoing Delivery Engine**, select **Resend API**.
4. Paste your API key into **Resend API Key** (`re_xxxxxxxxxxxx`).
5. Set **Sender Email Address (From)** to `rentals@gloucesterfraternityclub.com`.
6. Set **Sender Display Name** to `Gloucester Fraternity Club - Hall Rentals`.
7. Enter your email in the test box and click **Test Send**.
8. Verify that the email is delivered with the verified sender domain.

---

### Step 3: Configure Inbound Email Webhook in Resend
1. In the Web App &rarr; **Hall Rentals & Calendar Settings &rarr; Email & Notifications**:
   - Scroll down to **Inbound Email Reply Sync (Resend Webhook & Mailbox Routing)**.
   - Turn ON **Enable Inbound Sync**.
   - Copy the **Inbound Webhook Endpoint URL** (e.g., `https://your-public-domain.com/api/webhooks/resend-inbound`).
   - Copy or generate the **Webhook Secret Token** (e.g., `whsec_xxxxxxxxxx`).
   - Click **Save All Settings**.
2. In the [Resend Dashboard](https://resend.com):
   - Go to **Webhooks &rarr; Add Webhook**.
   - **Endpoint URL:** Paste the URL copied from the web app.
   - **Events to Listen:** Select `email.received` (and `email.bounced` if desired).
   - If prompted for a Signing Secret / Header, paste your `whsec_...` token.
   - Click **Create Webhook**.

---

### Step 4: Configure Inbound MX Records in DNS
To allow Resend to receive emails sent to `@gloucesterfraternityclub.com`:
1. In your domain DNS manager, add the Resend Inbound MX record:
   - **Type:** `MX`
   - **Host / Name:** `@` (or `inbound.gloucesterfraternityclub.com` if using a subdomain)
   - **Priority:** `10`
   - **Value / Target:** `inbound.resend.com`
2. Once the MX record propagates, any email sent or replied to `rentals@gloucesterfraternityclub.com` will be processed by Resend and instantly posted to your web application webhook.

---

---

# Verification & End-to-End Test Plan

Once you have chosen and configured either **Option 1** or **Option 2**, follow these steps to verify full functionality:

```
[TEST 1: Outbound Auto-Responder]
1. Go to the public Hall Rental Application page: /rentals/apply
2. Fill out and submit a test rental application using your personal email.
3. Verify that your personal inbox receives the automated confirmation receipt from rentals@gloucesterfraternityclub.com.

[TEST 2: Pre-Booking Question & Inquiry Reply]
1. Open the floating "Have a Question?" drawer on the website and submit a test inquiry.
2. Log into the Web App -> Hall Rentals and verify the new Inquiry appears in the list.
3. Click "View Details" on the inquiry and click the blue "Reply to Inquiry" button.
4. Type a test response and click "Send Email Reply".
5. Check your personal inbox for the reply. Notice the subject line contains:
   "Re: [GFC-INQ-###] Hall Rental Inquiry - Gloucester Fraternity Club"

[TEST 3: Customer Reply Synchronization]
1. In your personal mail app, hit "Reply" to the email from Step 2, type:
   "Thank you for the quick answer! Does the rental include chairs and tables?"
2. Hit Send.
3. Within 1-2 minutes (Option 1) or immediately (Option 2), refresh the Hall Rentals page in the web app.
4. Verify that:
   - The status badge changes to "Inquiry" / "Applicant Replied".
   - The inquiry details card shows your new reply text logged in the correspondence history.
```

---

# Frequently Asked Questions (FAQ)

### Q1: What happens if I don't set up Inbound Webhooks or IMAP?
**A:** You will still receive all applicant replies! They will simply land in your regular website email inbox (e.g. your webmail, Outlook, or Apple Mail on your phone). The only feature you miss is the automatic logging of the text inside the web app database.

### Q2: Can I switch from Resend to SMTP (or vice versa) later?
**A:** Yes. The web app allows you to toggle between **SMTP Server** and **Resend API** at any time with a single click in **Hall Rental Settings**.

### Q3: How do tracking tokens work?
**A:** Every email generated by the system embeds a structured identifier in the subject line (e.g., `[GFC-INQ-104]` for inquiries or `[GFC-RENT-205]` for bookings). When the applicant clicks "Reply", standard email clients preserve this subject line, allowing the system to match the reply to the exact booking.

---

*Documentation maintained by Gloucester Fraternity Club Engineering Team.*
