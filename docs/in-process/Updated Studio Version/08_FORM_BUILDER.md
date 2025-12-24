# GFC Studio - Form Builder

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Interactive Form Creation & Management

---

## 📝 Overview

The Form Builder allows users to create complex interactive forms (Contact Us, Hall Rental Applications) and manage the data collected through the GFC Web App.

---

## 🛠️ Visual Form Designer

### 1. Drag & Drop Fields
Users can drag standard inputs into a "Form Container" component:
- **Text:** Short and long (textarea).
- **Selection:** Dropdowns, radio buttons, and multi-select checkboxes.
- **Picker:** Date, time, and file upload.
- **Hidden:** For tracking source pages or IDs.

### 2. Validation Engine
Each field has a "Validation" tab:
- **Required:** Toggle with custom error message.
- **Pattern:** Email, Phone, or custom Regex.
- **Character Limit:** Min/Max constraints.

---

## 🏷️ GFC Specific Forms

### Hall Rental Wizard
A pre-built multi-step template that includes:
- **Step 1:** Date & Room Selection (Integrated with Hall Calendar).
- **Step 2:** User Information (Auto-populated if logged in).
- **Step 3:** Equipment & Bar Requests.
- **Step 4:** Terms & Electronic Signature.

---

## 📬 Submission Logic

### 1. Handling
- **Database:** All submissions are saved to the `FormSubmissions` table.
- **Email Notifications:** Triggers an SMTP email to the club administrators.
- **Auto-Reply:** Sends a confirmation email to the applicant.

### 2. Post-Submit Actions
- **Redirect:** Send user to a "Thank You" page.
- **Success Message:** Display a custom popup/toast.
- **Download:** Offer a PDF summary or brochure.

---

## 🔒 Security
- **Spam Protection:** Integrated reCAPTCHA (v3 - invisible).
- **Input Sanitization:** Automated stripping of malicious tags and scripts.
- **CSRF Protection:** Integrated with the GFC Web App security layer.

---

**Next:** 09_ASSET_MANAGER.md ➜
