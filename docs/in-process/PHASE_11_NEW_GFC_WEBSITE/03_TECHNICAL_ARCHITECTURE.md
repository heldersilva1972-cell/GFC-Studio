# GFC Website - Technical Architecture

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Status:** Implementation Blueprint  
**Stack:** Next.js 14, Tailwind, Framer Motion, GFC API.

---

## 🏗️ The Rendering Strategy

To achieve a **"Wow and Load Fast"** experience, we utilize a hybrid rendering approach:

### 1. ISR (Incremental Static Regeneration)
- **Why:** Delivers lightning-fast loads of static files while allowing updates without a full site rebuild.
- **Trigger:** When an admin updates an event or news item in the Web App, a "revalidation" pulse is sent to the Website to refresh that specific data in the background.

### 2. Client-Side Interactivity
- **Framework:** React 18 / Next.js Client Components.
- **Use Cases:** Calendars, Form validations, and Popups.

---

## 🎨 Animation Framework: "The Eased Reveal"

We avoid "Parallax" to prevent confusion, focusing instead on **High-Frame-Rate Reveal** animations using **Framer Motion**.

### Standard Reveal Logic:
```javascript
// Example reveal settings for every section
const revealVariants = {
  hidden: { opacity: 0, y: 30 },
  visible: { 
    opacity: 1, 
    y: 0, 
    transition: { duration: 0.8, ease: "easeOut" } 
  }
};
```

---

## 🔗 Data & API Layer

The website communicates with the GFC SQL Database through a secure API layer within the Web App.

### Key Data Endpoints:
- `GET /api/public/status`: Returns current Open/Closed status.
- `GET /api/public/events`: Returns upcoming calendar events.
- `GET /api/public/booking-calendar`: Returns Reserved/Available slots.
- `GET /api/public/reviews`: Returns approved social proof items.
- `POST /api/public/inquiry`: Sends Hall Rental applications to the backend.

---

## 📦 Component Architecture (Studio Compatible)

Every part of the website is a "Block" that can be moved or deleted.

### Directory Structure:
```
/apps/website
  /components
    /layout        (Header, Footer, Navigation)
    /blocks        (Hero, Text, Cards, Gallery)
    /interactive   (Calendar, Form, NewsTicker)
    /studio        (The glue that links sections to Studio DB)
  /lib
    /api           (Data fetching logic)
    /utils         (Date formatting, sorting)
  /styles
    globals.css    (Tailwind + Modern Legacy tokens)
```

---

## 🖼️ Image Optimization Logic

- **Format:** Automatic `WebP` and `AVIF` conversion.
- **Processing:** Small thumbnails are generated for the "Impact" photo grid to ensure 100+ Performance scores.
- **Lazy Loading:** All images below-the-fold are lazy-loaded by default.

---

## 🔒 Security & Performance

### Performance Goals:
- **First Contentful Paint:** < 0.8s.
- **Total Blocking Time:** < 50ms.
- **Cumulative Layout Shift:** 0 (Ensured by reserving space for dynamic content).

### Security:
- **CSRF Protection:** For all form submissions.
- **Sanitization:** All user-submitted reviews are sanitized before database entry.
- **CORS:** API access restricted to the GFC production domain.
