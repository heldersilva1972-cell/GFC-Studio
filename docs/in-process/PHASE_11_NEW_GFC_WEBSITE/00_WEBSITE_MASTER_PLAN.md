# GFC Website - Master Plan

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Status:** Planning Phase  
**Purpose:** Rebuilding the public face of the Gloucester Fraternity Club as a modern, high-performance, and operationally integrated web presence.

---

## 🎯 Executive Summary

The new GFC Website is designed to be a **"Modern Legacy"** platform. It combines a premium, sleek aesthetic with robust operational features that are controlled directly from the GFC Web App. 

The primary goals are to facilitate frictionless hall rentals, showcase community impact, and provide real-time club information with zero technical overhead for club administrators.

### Core Pillars
1. **Premium Aesthetic:** Modern, dark-themed, sophisticated design.
2. **Operational Ease:** Daily updates (Events, News, Status) happen in the Web App, not the code.
3. **Interactive Utility:** Real-time availability calendars and frictionless rental applications.
4. **Community Showcase:** Focused on impact (Kids/Special Needs events) rather than just dates.

---

## 🎨 Visual Identity: "Modern Legacy"

### Tone & Style
- **Modern & Sleek:** High-end boutique/private club feel.
- **Micro-Animations:** Subtle reveal fades and smooth transitions. No jarring parallax.
- **Breathing Room:** High use of white space and large, crisp typography.

### Color Palette
- **Midnight Blue / Charcoal:** Primary background and structural color.
- **Burnished Gold / Champagne:** Brand accent for buttons, active states, and icons.
- **Pure White / Soft Gray:** Primary and secondary text for maximum legibility.

---

## 🏗️ Sitemap & Page Structure (Modular Design)

The site is built using **Modules**. Any section can be added or removed instantly via the Studio Editor.

### 1. Home Page
- **Hero Module:** Large-format exterior photo + Mission statement.
- **Status Bar:** Live "Open/Closed" indicator (Controlled by Web App).
- **News/Announcement Card:** Top-level club updates (Controlled by Web App).
- **Interactive Calendar Preview:** Next 3 major events.
- **Hall Rental Teaser:** Link to rental flow.
- **Social Proof:** Approved community reviews.

### 2. Hall Rentals (The 3-Step Flow)
- **Step 1: The Vibe:** Photo gallery of both halls + detail tables (pricing/capacity).
- **Step 2: Check Availability:** Live calendar showing "Available" vs "Reserved" (Pulls from Web App).
- **Step 3: Application:** Sleek, integrated form (Saves to Web App).

### 3. Community & Impact (History)
- **Focus:** Child-focused events, Special Needs dances, community support.
- **Format:** Image-heavy "Story" layout.

### 4. Interactive Calendar
- **Full View:** Comprehensive monthly calendar with event popups.
- **Popup Details:** "Save to Calendar" and "Share" options.

---

## ⚙️ Operational Control (The "Must-Have" Integration)

The website is a **"Consumer"** of data from the GFC Web App. No developer interaction is needed for daily club management.

| Feature | Controlled In... | Appears on Website As... |
| :--- | :--- | :--- |
| **Club Status** | Web App Toggle | "We are Currently Open / Closed" |
| **Events** | Web App Calendar | Public Event Cards & Full Calendar |
| **Announcement** | Web App News | Top Update Bar & Homepage Card |
| **Availability** | Web App Bookings | "Reserved" blocks on public calendar |
| **Reviews** | Web App Moderation | Social Proof Testimonials |

---

## 🛠️ Technology Stack

- **Framework:** Next.js 14+ (App Router).
- **Styles:** Tailwind CSS (Custom Design Tokens).
- **Animation:** Framer Motion (Subtle reveals).
- **Data:** Direct API connection to GFC Web App SQL Database.
- **Hosting:** Optimzed for speed and SEO.

---

## 📈 Success Criteria

1. **User Experience:** A user can check hall availability in under 30 seconds.
2. **Management:** Administrators can post a club update in under 60 seconds from their phone (Web App).
3. **Performance:** Lighthouse score of 95+ for Performance and SEO.
4. **Design:** A first impression that matches the professional pride of a 100-year-old tradition.
