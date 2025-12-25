# GFC Website - Hall Rental Flow

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Status:** Core Feature Specification  
**UX Goal:** Frictionless, non-overwhelming, and professional.

---

## 🧭 The 3-Step Journey

To ensure users find the process easy and not confusing, we split the experience into three distinct stages.

---

### 📥 Step 1: The "Visual Invitation"
*   **Goal:** Showcase why they should choose GFC.
*   **Content:**
    *   **High-End Gallery:** Large, professional photos of both the Upper and Lower halls.
    *   **Feature Grid:** Quick icons for "Full Bar," "Catering Kitchen," "150+ Capacity," "Ample Parking."
    *   **Transparent Details:** A clean table showing rental rates for Members vs. Non-Members and Non-Profits.
    *   **The Action:** A prominent button: **[Check Availability & Book]**.

---

### 📅 Step 2: The "Availability Checker"
*   **Goal:** Provide instant answers without requiring a phone call.
*   **Integration:** Pulls live data from the Web App.
*   **Interface:**
    *   A clean, modern calendar view.
    *   **Green Dates:** Available.
    *   **Red/Gray Dates:** Reserved or Club Event.
    *   **Selection:** User clicks a Green date.
*   **Immediate Feedback:** A small box appears: *"Saturday, June 14th is available! Rates start at $XXX."*
*   **The Action:** Button: **[Begin Application]**.

---

### 📝 Step 3: The "Smart Application"
*   **Goal:** Collect necessary info quickly.
*   **Form Logic:**
    *   **Progress Bar:** Shows "Info -> Details -> Review."
    *   **Field Group 1 (Who):** Name, Email, Phone, GFC Member Status.
    *   **Field Group 2 (What):** Type of event (Birthday, Wedding, Meeting), Estimated Guests.
    *   **Field Group 3 (Requirements):** Checklist for Bar Service, Kitchen Access, A/V Equipment.
*   **The Agreement:** A simple "Terms of Rental" scroll-box with a checkbox: *"I have read and agree to the GFC Hall Rental rules."*
*   **The Action:** **[Submit Application]**.

---

## ⚙️ Post-Submission Workflow (Automated)

1.  **User Side:**
    *   An elegant "Success" screen appears: *"Thank you! Our rental coordinator will review your request and contact you within 24 hours."*
    *   The user receives an automated, professionally formatted email with a copy of their request.
2.  **Admin Side (Web App):**
    *   A "New Rental Request" alert appears in the Web App dashboard.
    *   The date on the public calendar is marked as **"Pending"** (Yellow) to prevent others from thinking it's fully open.
3.  **Completion:**
    *   Once the Admin approves in the Web App, the date turns **"Reserved"** (Red/Gray), and the user is notified.

---

## 📱 Mobile Considerations

- **Calendar:** Replaces the full grid with a "Date List" or a swipeable month view for easier tapping on small screens.
- **Form:** Uses large input fields and auto-capitalization for names to speed up typing.
- **Sticky Footer:** A "Book Now" button stays at the bottom of the screen while the user scrolls through hall photos.
