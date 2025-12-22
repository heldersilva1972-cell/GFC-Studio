# GFC Website - Phase 0 Implementation Plan

**Version:** 1.0.0  
**Status:** In Progress  
**Created:** 2025-12-22  
**Objective:** Create ultra-modern public website with Web App integration

---

## 🎯 Phase 0 Objectives

1. ✅ Create a standalone, ultra-modern Next.js website
2. ✅ Migrate content from current gloucesterfraternityclub.com
3. ✅ Add navigation button in GFC Web App to open website
4. ✅ Add return button in website to navigate back to Web App
5. ✅ Implement premium, state-of-the-art design
6. ✅ Ensure responsive design (desktop/tablet/mobile)

---

## 📋 Scope

### **In Scope (Phase 0)**
- Modern Next.js website with App Router
- Content from current live site:
  - Home page with hero and sections
  - Hall Rentals overview
  - Events/Activities information
  - Membership information
  - Contact information
  - Photo gallery placeholder
- Premium UI/UX with animations
- Bi-directional navigation (Web App ↔ Website)
- Responsive design
- Modern typography and color palette

### **Out of Scope (Future Phases)**
- Studio integration for content editing
- Draft/Preview/Publish workflow
- Database integration
- Hall rental application forms
- Event calendar integration
- Photo gallery with real images
- SEO optimization
- Analytics
- Contact form backend

---

## 🛠️ Technology Stack

- **Framework:** Next.js 14+ (App Router)
- **Language:** TypeScript
- **Styling:** Vanilla CSS (custom design system)
- **Animations:** Framer Motion
- **Icons:** Lucide React
- **Fonts:** Google Fonts (Inter, Outfit)
- **Deployment:** Standalone (runs on localhost for now)

---

## 🏗️ Architecture

```
GFC-Studio V2/
├── apps/
│   ├── webapp/          # Existing Blazor Web App
│   └── website/         # NEW - Next.js Website
│       ├── app/
│       │   ├── layout.tsx
│       │   ├── page.tsx (Home)
│       │   ├── hall-rentals/
│       │   ├── events/
│       │   ├── membership/
│       │   ├── contact/
│       │   └── gallery/
│       ├── components/
│       │   ├── Header.tsx
│       │   ├── Footer.tsx
│       │   ├── Hero.tsx
│       │   ├── FeatureCard.tsx
│       │   └── BackToWebAppButton.tsx
│       ├── styles/
│       │   └── globals.css
│       └── public/
│           └── images/
```

---

## 🎨 Design System

### **Color Palette**
- **Primary:** Deep Blue (#1e3a8a) - Trust, tradition
- **Secondary:** Gold (#f59e0b) - Warmth, community
- **Accent:** Teal (#0d9488) - Modern, fresh
- **Dark:** Charcoal (#1f2937)
- **Light:** Off-white (#f9fafb)

### **Typography**
- **Headings:** Outfit (Bold, 600-800 weight)
- **Body:** Inter (Regular, 400-500 weight)

### **Animations**
- Smooth fade-ins on scroll
- Hover effects on cards and buttons
- Subtle micro-animations
- Page transitions

---

## 📝 Content Structure

### **Home Page**
1. **Hero Section**
   - Headline: "Building Community, Friendship, and Tradition Since 1923"
   - Subheadline: Welcome message
   - CTA buttons: "Learn More" | "Rent Our Hall"
   - Background: Gradient with subtle animation

2. **Feature Sections**
   - 🎉 Upcoming Events
   - 🏛️ Hall Rentals
   - 👥 Membership
   - 🤝 Support the Club
   - 📸 Community in Action
   - 📍 Visit Us

3. **Footer**
   - Contact information
   - Social media links
   - Hours of operation

### **Additional Pages**
- Hall Rentals (overview, features, pricing info)
- Events (upcoming events list)
- Membership (benefits, how to join)
- Contact (address, phone, email, map placeholder)
- Gallery (placeholder for future photos)

---

## 🔗 Web App Integration

### **Web App Changes**
**File:** `apps/webapp/Components/Layout/NavMenu.razor`

Add navigation item:
```razor
<div class="nav-item px-3">
    <a class="nav-link" href="http://localhost:3000" target="_blank">
        <span class="oi oi-globe" aria-hidden="true"></span> Public Website
    </a>
</div>
```

### **Website Changes**
**Component:** `components/BackToWebAppButton.tsx`

Add floating button:
```tsx
<a href="http://localhost:5000" className="back-to-webapp-btn">
    ← Back to Admin
</a>
```

---

## ✅ Implementation Steps

### **Step 1: Initialize Next.js Project**
```bash
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps"
npx create-next-app@latest website --typescript --tailwind --app --no-src-dir
```

### **Step 2: Set Up Project Structure**
- Configure TypeScript
- Set up CSS design system
- Install dependencies (framer-motion, lucide-react)

### **Step 3: Build Core Components**
- Header with navigation
- Footer with contact info
- Hero section
- Feature cards
- Back to Web App button

### **Step 4: Create Pages**
- Home page
- Hall Rentals page
- Events page
- Membership page
- Contact page
- Gallery page

### **Step 5: Add Animations**
- Scroll animations
- Hover effects
- Page transitions
- Micro-interactions

### **Step 6: Integrate with Web App**
- Add "Public Website" button in Web App nav
- Add "Back to Admin" button in website
- Test navigation flow

### **Step 7: Polish & Test**
- Responsive design testing
- Cross-browser testing
- Performance optimization
- Accessibility check

---

## 🚀 Running the Applications

### **Web App (Blazor)**
```bash
cd apps/webapp/GFC.BlazorServer
dotnet run
# Runs on: http://localhost:5000
```

### **Website (Next.js)**
```bash
cd apps/website
npm run dev
# Runs on: http://localhost:3000
```

---

## 📊 Success Criteria

- ✅ Website loads on localhost:3000
- ✅ All pages are accessible and responsive
- ✅ Premium, modern design implemented
- ✅ Animations work smoothly
- ✅ Navigation from Web App to Website works
- ✅ Navigation from Website to Web App works
- ✅ Content from current site is migrated
- ✅ No console errors
- ✅ Fast load times (<2s)

---

## 🔮 Future Phases (Planned)

### **Phase 1: Studio Integration**
- Connect to GFC Studio for content editing
- Draft/Preview/Publish workflow
- Animation management

### **Phase 2: Dynamic Content**
- Database integration
- Real-time event calendar
- Photo gallery with uploads

### **Phase 3: Forms & Interactivity**
- Hall rental application form
- Contact form with email
- Member portal integration

### **Phase 4: Production Deployment**
- Domain configuration
- SSL/HTTPS
- CDN setup
- SEO optimization
- Analytics integration

---

## 📝 Notes

- Phase 0 is a **standalone proof-of-concept**
- Future phases will integrate with Studio and Web App backend
- Current navigation uses hardcoded URLs (localhost)
- Production will use environment variables for URLs

---

## 🎨 Visual Tracking

### **Modified Elements**
- 🆕 **NEW** - All website files (entire apps/website directory)
- 🔧 **MODIFIED** - `apps/webapp/Components/Layout/NavMenu.razor` (add website link)

### **Testing Checklist**
- [ ] Website runs on localhost:3000
- [ ] Web App runs on localhost:5000
- [ ] Click "Public Website" in Web App opens website
- [ ] Click "Back to Admin" in website returns to Web App
- [ ] All pages render correctly
- [ ] Responsive design works on mobile/tablet/desktop
- [ ] Animations are smooth and not distracting
- [ ] No console errors or warnings

---

**END OF PHASE 0 PLAN**
