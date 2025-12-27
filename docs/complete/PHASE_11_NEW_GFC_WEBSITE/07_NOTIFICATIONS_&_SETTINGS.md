# GFC Website - Notifications & Admin Settings

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Status:** Operational Specification  
**Focus:** Internal routing, payment toggles, and administrative control.

---

## 🔧 1. The Admin Settings Page (Web App)

Located within the Web App, this page acts as the "Brain" of the communication system.

### Financial Toggles
- **Public Donations:** [ENABLE / DISABLE] - Shows/Hides "Donate" button.
- **Member Dues:** [ENABLE / DISABLE] - Shows/Hides "Pay Dues" in portal.
- **Event RSVPs:** [ENABLE / DISABLE] - Globally enable/disable public sign-ups for events.
- **Blackout Manager:** Interface to mark all-day closures (Holidays, Maintenance) where the Hall is un-rentable.
- **Secure Document Roles:** Set permissions for uploads (Public, Registered Member, Director Only).
- **Payment Method:** [Text Field] - Instructions/Keys for future payment processors.

---

## 📬 2. Smart Alert Routing

You can choose which officials are notified for specific website actions. This prevents "Inbox Overload" by only sending relevant alerts to the right directors.

| Action from Website | Alert Type | Configurable Recipient(s) |
| :--- | :--- | :--- |
| **Hall Rental Inquiry** | Immediate Email/Push | Rental Coordinator, Treasurer |
| **New Community Review** | Moderation Alert | Secretary, President |
| **Member Dues Paid** | Financial Alert | Treasurer, Financial Secretary |
| **Contact Form** | Inquiry Alert | Recording Secretary |
| **Calendar Update** | Change Notify | All Directors (Optional) |

---

## 📧 3. User-Facing (Public) System Notifications

Administrators have granular control over which automated communications are sent to the public. Each toggle determines if a user receives an email or browser notification for specific lifecycle events.

### Hall Rental Lifecycle Notifications
- **[ON/OFF] Application Receipt:** Sent immediately after the user hits "Submit" on Step 3. Confirms we received the info.
- **[ON/OFF] Booking Approval/Confirmation:** Sent when an Admin clicks "Confirm" in the Web App. This is the official "You're Booked" notice.
- **[ON/OFF] Booking Denied/Cancellation:** Sent if an application is rejected or a confirmed booking is cancelled.
- **[ON/OFF] Payment/Deposit Required:** Sent when the club is ready to move from "Reserved" to "Contracted" (Future use).
- **[ON/OFF] "Save & Resume" Magic Link:** Ensures the user gets their recovery link to finish a partial application.

### Member & Community Notifications
- **[ON/OFF] Review Submission Receipt:** A simple "Thank you for your feedback" note.
- **[ON/OFF] Review Approved Notification:** Notifies the user that their story is now live on the GFC website.
- **[ON/OFF] Dues Payment Receipt:** Automated financial acknowledgement for members.

### Notification Channel Settings
- **Email Delivery:** Primary channel for all official club correspondence.
- **SMS/Text Alerts (Future):** Placeholder to enable mobile text alerts for urgent club status changes.

---

## 💾 4. "Save & Resume" Data Logic

To handle in-progress Hall Rental applications, the system maintains a `TransientApplications` table.
- **Security:** "Save & Resume" does NOT require a password. It uses a **Secure Token (Magic Link)** sent to the user's email.

## 📜 5. Communication Audit Logs (Internal)

To ensure no critical club request is missed, the "Settings" page provides a historical log of all automated system activity.

| Timestamp | Type | Event | Recipients / User | Result |
| :--- | :--- | :--- | :--- | :--- |
| `12/24 09:00` | Alert | Hall Rental | Director A, Director B | Delivered |
| `12/24 09:05` | Receipt | User Confirmation | hnsilva@email.com | Delivered |
| `12/24 10:15` | Financial | Dues Payment | Treasurer | Queued |

---

## 🔒 6. Secure Media & Document Library

The library in the Web App handles asset visibility based on user roles.

- **Role: Public:** Images for website, public flyers, brochures.
- **Role: Member:** Bylaws, Membership Directories, Club Newsletters.
- **Role: Director:** Meeting Minutes, P&L Statements, Security Logs.
- **Result:** Content is filtered before it is ever sent to the browser. If you aren't a Director, the Director documents do not even appear in your list.

---

## 💰 7. Future-Proof Financials

While no payments are processed yet, the UI is built with "Payment Hooks."

- **The Button:** A standard GFC-styled button: **[Donate to the Club]**.
- **The Transition:** Initially, this button can link to a simple "Thank you for your interest, we are currently setting up online payments" popup, or display your existing mailing address for checks.
- **The Upgrade:** When you are ready to accept cards, we simply swap the "Popup" logic for the "Payment Processor" logic in the code—no design changes needed.
