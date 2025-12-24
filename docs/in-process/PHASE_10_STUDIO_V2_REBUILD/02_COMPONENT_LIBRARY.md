# GFC Studio - Complete Component Library

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Comprehensive Component Catalog

---

## 📦 Overview

This document catalogs all components available in GFC Studio, including their properties, use cases, and visual specifications.

---

## 🏗️ Component Categories

1. **Layout Components** - Structure and containers
2. **Content Components** - Text, images, media
3. **Card Components** - Reusable card patterns
4. **Interactive Components** - Forms, calendars, accordions
5. **Media Components** - Galleries, videos, carousels
6. **Navigation Components** - Headers, footers, menus
7. **GFC-Specific Components** - Custom GFC features
8. **Advanced Components** - Custom code, embeds

---

## 1️⃣ Layout Components

### Hero Section

**Purpose:** Full-width header section with background image/video and call-to-action

**Visual:**
```
┌─────────────────────────────────────────┐
│                                         │
│         [Background Image/Video]        │
│                                         │
│            Main Headline                │
│          Supporting subtitle            │
│                                         │
│            [Call to Action]             │
│                                         │
└─────────────────────────────────────────┘
```

**Properties:**
- **Background Type:** Image | Video | Gradient | Solid Color
- **Background Image:** File upload
- **Background Video:** File upload or URL
- **Overlay Color:** Color picker with opacity
- **Height:** Viewport units (vh) or pixels
- **Content Alignment:** Left | Center | Right
- **Vertical Alignment:** Top | Center | Bottom

**Content Fields:**
- **Headline:** Text input (H1)
- **Subtitle:** Text input (paragraph)
- **Button Text:** Text input
- **Button Link:** URL input
- **Button Style:** Primary | Secondary | Outline

**Animation Options:**
- Fade In
- Slide Up
- Zoom In
- Parallax Scroll

**Responsive Settings:**
- Desktop: Full height, large text
- Tablet: 80% height, medium text
- Mobile: 60% height, small text, stack content

---

### Container

**Purpose:** Content wrapper with max-width and padding

**Visual:**
```
┌─────────────────────────────────────────┐
│ ┌─────────────────────────────────────┐ │
│ │                                     │ │
│ │         Content goes here           │ │
│ │                                     │ │
│ └─────────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

**Properties:**
- **Max Width:** 640px | 768px | 1024px | 1280px | 1536px | Full
- **Padding:** Slider (0-8rem)
- **Margin:** Slider (0-8rem)
- **Background:** Color picker or transparent
- **Border:** Width, style, color
- **Shadow:** None | Small | Medium | Large

---

### Grid Layout

**Purpose:** Responsive grid for organizing content

**Visual:**
```
┌───────────┬───────────┬───────────┐
│           │           │           │
│  Column 1 │  Column 2 │  Column 3 │
│           │           │           │
└───────────┴───────────┴───────────┘
```

**Properties:**
- **Columns (Desktop):** 1-6 columns
- **Columns (Tablet):** 1-4 columns
- **Columns (Mobile):** 1-2 columns
- **Gap:** Slider (0-4rem)
- **Alignment:** Start | Center | End | Stretch

---

### Columns

**Purpose:** Custom column layouts with flexible widths

**Visual:**
```
┌─────────────┬───────────────────────┐
│             │                       │
│  30% Width  │      70% Width        │
│             │                       │
└─────────────┴───────────────────────┘
```

**Properties:**
- **Number of Columns:** 2-4
- **Column Widths:** Percentage or fraction
- **Gap:** Slider (0-4rem)
- **Reverse on Mobile:** Toggle
- **Stack on Mobile:** Toggle

---

### Spacer

**Purpose:** Add vertical spacing between sections

**Visual:**
```
─────────────────────────────────────────
           (Empty Space)
─────────────────────────────────────────
```

**Properties:**
- **Height (Desktop):** Slider (0-12rem)
- **Height (Tablet):** Slider (0-8rem)
- **Height (Mobile):** Slider (0-4rem)
- **Background:** Color picker or transparent

---

## 2️⃣ Content Components

### Text Block

**Purpose:** Rich text content with formatting

**Visual:**
```
┌─────────────────────────────────────────┐
│ This is a paragraph of text with rich  │
│ formatting options like bold, italic,   │
│ links, and lists.                       │
│                                         │
│ • Bullet point one                      │
│ • Bullet point two                      │
└─────────────────────────────────────────┘
```

**Properties:**
- **Content:** Rich text editor
- **Font Family:** Dropdown (theme fonts)
- **Font Size:** Slider (0.75rem - 3rem)
- **Font Weight:** 400-900
- **Line Height:** Slider (1.0 - 2.5)
- **Text Color:** Color picker
- **Text Align:** Left | Center | Right | Justify
- **Max Width:** Slider or full width

**Rich Text Features:**
- Bold, Italic, Underline
- Headings (H1-H6)
- Lists (ordered/unordered)
- Links
- Blockquotes
- Code blocks

---

### Heading

**Purpose:** Section headings (H1-H6)

**Visual:**
```
Main Page Heading (H1)
```

**Properties:**
- **Text:** Text input
- **Level:** H1 | H2 | H3 | H4 | H5 | H6
- **Font Family:** Dropdown
- **Font Size:** Slider (1rem - 6rem)
- **Font Weight:** 400-900
- **Color:** Color picker
- **Text Align:** Left | Center | Right
- **Margin Top/Bottom:** Slider

---

### Paragraph

**Purpose:** Body text content

**Visual:**
```
This is a paragraph of body text. It can be
multiple lines and wraps naturally.
```

**Properties:**
- **Text:** Textarea
- **Font Size:** Slider (0.875rem - 1.5rem)
- **Line Height:** Slider (1.2 - 2.0)
- **Color:** Color picker
- **Text Align:** Left | Center | Right | Justify
- **Max Width:** Slider (for readability)

---

### Image

**Purpose:** Single image with optional caption

**Visual:**
```
┌─────────────────────────────────────────┐
│                                         │
│           [Image Display]               │
│                                         │
└─────────────────────────────────────────┘
          Image caption text
```

**Properties:**
- **Image:** File upload
- **Alt Text:** Text input (required for accessibility)
- **Width:** Percentage or pixels
- **Height:** Auto | Fixed pixels
- **Object Fit:** Cover | Contain | Fill | None
- **Border Radius:** Slider (0-2rem)
- **Shadow:** None | Small | Medium | Large
- **Caption:** Text input
- **Caption Position:** Below | Overlay
- **Link:** URL (optional)

**Advanced:**
- Lazy Loading: Toggle
- Srcset (responsive images): Auto-generated
- WebP conversion: Auto

---

### Video

**Purpose:** Embedded or uploaded video

**Visual:**
```
┌─────────────────────────────────────────┐
│                                         │
│          ▶ Video Player                 │
│                                         │
└─────────────────────────────────────────┘
```

**Properties:**
- **Source:** Upload | YouTube | Vimeo | URL
- **Video File:** File upload (MP4, WebM)
- **YouTube/Vimeo URL:** Text input
- **Autoplay:** Toggle
- **Loop:** Toggle
- **Muted:** Toggle
- **Controls:** Toggle
- **Aspect Ratio:** 16:9 | 4:3 | 1:1 | Custom
- **Poster Image:** File upload (thumbnail)

---

### Button

**Purpose:** Call-to-action button

**Visual:**
```
┌─────────────────┐
│  Button Text    │
└─────────────────┘
```

**Properties:**
- **Text:** Text input
- **Link:** URL input
- **Style:** Primary | Secondary | Outline | Ghost
- **Size:** Small | Medium | Large
- **Width:** Auto | Full Width
- **Icon:** None | Left | Right (icon picker)
- **Background Color:** Color picker
- **Text Color:** Color picker
- **Border:** Width, style, color
- **Border Radius:** Slider (0-2rem)
- **Padding:** Slider
- **Hover Effect:** Darken | Lighten | Scale | Lift

**Advanced:**
- Open in New Tab: Toggle
- Download Link: Toggle
- Scroll to Section: Anchor link

---

### Link

**Purpose:** Text hyperlink

**Visual:**
```
Click here to learn more
```

**Properties:**
- **Text:** Text input
- **URL:** URL input
- **Color:** Color picker
- **Underline:** None | Always | Hover
- **Font Weight:** 400-900
- **Open in New Tab:** Toggle

---

## 3️⃣ Card Components

### Feature Card

**Purpose:** Icon + title + description card

**Visual:**
```
┌─────────────────────────────────────────┐
│              🎯                         │
│                                         │
│         Feature Title                   │
│                                         │
│  Short description of the feature       │
│  explaining its benefits.               │
│                                         │
│         [Learn More →]                  │
└─────────────────────────────────────────┘
```

**Properties:**
- **Icon:** Emoji or upload SVG
- **Icon Size:** Slider (2rem - 6rem)
- **Icon Color:** Color picker
- **Title:** Text input
- **Description:** Textarea
- **Link Text:** Text input
- **Link URL:** URL input
- **Background:** Color picker or transparent
- **Border:** Width, style, color
- **Border Radius:** Slider
- **Padding:** Slider
- **Shadow:** None | Small | Medium | Large
- **Hover Effect:** Lift | Scale | Glow

---

### Image Card

**Purpose:** Image + title + text + button card

**Visual:**
```
┌─────────────────────────────────────────┐
│                                         │
│         [Card Image]                    │
│                                         │
├─────────────────────────────────────────┤
│  Card Title                             │
│                                         │
│  Card description text goes here        │
│  and can be multiple lines.             │
│                                         │
│  ┌─────────────┐                        │
│  │   Button    │                        │
│  └─────────────┘                        │
└─────────────────────────────────────────┘
```

**Properties:**
- **Image:** File upload
- **Image Height:** Slider (150px - 400px)
- **Title:** Text input
- **Description:** Textarea
- **Button Text:** Text input
- **Button Link:** URL input
- **Card Background:** Color picker
- **Border:** Width, style, color
- **Border Radius:** Slider
- **Shadow:** None | Small | Medium | Large
- **Hover Effect:** Lift | Scale | Image Zoom

---

### Testimonial Card

**Purpose:** Customer quote with author info

**Visual:**
```
┌─────────────────────────────────────────┐
│  "This is an amazing testimonial        │
│   quote from a satisfied customer."     │
│                                         │
│  ┌────┐                                 │
│  │ 👤 │  John Doe                       │
│  └────┘  Member since 2020              │
└─────────────────────────────────────────┘
```

**Properties:**
- **Quote:** Textarea
- **Author Name:** Text input
- **Author Title:** Text input
- **Author Photo:** File upload
- **Photo Size:** Slider (40px - 80px)
- **Quote Icon:** Toggle (show/hide quotation marks)
- **Background:** Color picker
- **Border:** Width, style, color
- **Border Radius:** Slider
- **Padding:** Slider

---

### Stats Card

**Purpose:** Display number/statistic with label

**Visual:**
```
┌─────────────────────────────────────────┐
│              1,234                      │
│                                         │
│          Happy Members                  │
└─────────────────────────────────────────┘
```

**Properties:**
- **Number:** Text input
- **Label:** Text input
- **Icon:** Emoji or SVG (optional)
- **Number Color:** Color picker
- **Number Size:** Slider (2rem - 6rem)
- **Label Color:** Color picker
- **Background:** Color picker
- **Border:** Width, style, color
- **Border Radius:** Slider
- **Animation:** Count Up | Fade In | None

---

### Pricing Card

**Purpose:** Pricing tier with features list

**Visual:**
```
┌─────────────────────────────────────────┐
│           Member Pricing                │
│                                         │
│            $50/month                    │
│                                         │
│  ✓ Full hall access                    │
│  ✓ Event discounts                     │
│  ✓ Member lounge                       │
│  ✗ Private events                      │
│                                         │
│  ┌─────────────────┐                   │
│  │   Join Now      │                   │
│  └─────────────────┘                   │
└─────────────────────────────────────────┘
```

**Properties:**
- **Plan Name:** Text input
- **Price:** Text input
- **Billing Period:** Text input (/month, /year)
- **Features:** List (add/remove items)
- **Feature Icon (included):** Emoji or SVG
- **Feature Icon (excluded):** Emoji or SVG
- **Button Text:** Text input
- **Button Link:** URL input
- **Highlight:** Toggle (featured plan)
- **Background:** Color picker
- **Border:** Width, style, color
- **Shadow:** None | Small | Medium | Large

---

## 4️⃣ Interactive Components

### Contact Form

**Purpose:** General contact form

**Visual:**
```
┌─────────────────────────────────────────┐
│  Name                                   │
│  [_________________________________]    │
│                                         │
│  Email                                  │
│  [_________________________________]    │
│                                         │
│  Message                                │
│  [_________________________________]    │
│  [_________________________________]    │
│  [_________________________________]    │
│                                         │
│  ┌─────────────┐                        │
│  │    Send     │                        │
│  └─────────────┘                        │
└─────────────────────────────────────────┘
```

**Properties:**
- **Fields:** Customizable list
  - Name (text)
  - Email (email)
  - Phone (tel)
  - Subject (text)
  - Message (textarea)
- **Submit Button Text:** Text input
- **Submit To:** Email address
- **Success Message:** Text input
- **Redirect URL:** URL (optional)
- **Enable reCAPTCHA:** Toggle
- **Field Styling:** Inherit from theme
- **Validation:** Required fields, email format, phone format

---

### Hall Rental Application Form

**Purpose:** GFC-specific rental application

**Visual:**
```
┌─────────────────────────────────────────┐
│  Event Date                             │
│  [MM/DD/YYYY]                           │
│                                         │
│  Event Time                             │
│  Start: [__:__ AM/PM]                   │
│  End:   [__:__ AM/PM]                   │
│                                         │
│  Applicant Information                  │
│  Name:  [_________________________]     │
│  Email: [_________________________]     │
│  Phone: [_________________________]     │
│                                         │
│  Event Details                          │
│  Type: [Birthday ▼]                     │
│  Guests: [___]                          │
│                                         │
│  Facilities Needed                      │
│  ☐ Bar Service                          │
│  ☐ Kitchen Access                       │
│  ☐ A/V Equipment                        │
│                                         │
│  ☐ I am a GFC member                    │
│                                         │
│  ┌─────────────────┐                    │
│  │  Submit Request │                    │
│  └─────────────────┘                    │
└─────────────────────────────────────────┘
```

**Properties:**
- **Pre-configured fields** (based on GFC requirements)
- **Member discount toggle**
- **Availability check integration**
- **Pricing calculation**
- **Submit to:** Admin email + database
- **Auto-response email:** Toggle

---

### Calendar

**Purpose:** Display events or availability

**Visual:**
```
┌─────────────────────────────────────────┐
│  December 2024          [< Today >]     │
├─────────────────────────────────────────┤
│  Sun Mon Tue Wed Thu Fri Sat            │
│   1   2   3   4   5   6   7             │
│   8   9  10  11  12  13  14             │
│  15  16  17  18  19  20  21             │
│  22  23  24  25  26  27  28             │
│  29  30  31                             │
│                                         │
│  • Available  • Booked  • Tentative     │
└─────────────────────────────────────────┘
```

**Properties:**
- **Type:** Events | Availability | Both
- **Data Source:** Manual | API | Database
- **Default View:** Month | Week | Day
- **Show Legend:** Toggle
- **Color Coding:** Customizable
- **Click Behavior:** Show Details | Link to Page | None
- **Past Events:** Show | Hide | Gray Out

---

### Accordion

**Purpose:** Expandable content sections (FAQ)

**Visual:**
```
┌─────────────────────────────────────────┐
│  ▼ Question 1                           │
│     Answer to question 1 goes here      │
│     and can be multiple lines.          │
│                                         │
│  ▶ Question 2                           │
│                                         │
│  ▶ Question 3                           │
└─────────────────────────────────────────┘
```

**Properties:**
- **Items:** List (add/remove)
  - Title: Text input
  - Content: Rich text
- **Allow Multiple Open:** Toggle
- **Default Open:** First | All | None
- **Icon:** Arrow | Plus/Minus | Custom
- **Background:** Color picker
- **Border:** Width, style, color
- **Spacing:** Slider

---

### Tabs

**Purpose:** Tabbed content sections

**Visual:**
```
┌─────────────────────────────────────────┐
│  [Tab 1]  Tab 2   Tab 3                 │
├─────────────────────────────────────────┤
│                                         │
│  Content for Tab 1 displays here        │
│                                         │
└─────────────────────────────────────────┘
```

**Properties:**
- **Tabs:** List (add/remove)
  - Label: Text input
  - Content: Rich text or components
- **Tab Position:** Top | Bottom | Left | Right
- **Active Tab Color:** Color picker
- **Inactive Tab Color:** Color picker
- **Content Background:** Color picker

---

### Modal/Popup

**Purpose:** Overlay content window

**Visual:**
```
        ┌─────────────────────────┐
        │  Modal Title        ✕   │
        ├─────────────────────────┤
        │                         │
        │  Modal content here     │
        │                         │
        │  ┌─────────┐            │
        │  │  Close  │            │
        │  └─────────┘            │
        └─────────────────────────┘
```

**Properties:**
- **Trigger:** Button | Link | Auto (on page load)
- **Title:** Text input
- **Content:** Rich text or components
- **Width:** Slider (300px - 1200px)
- **Close Button:** Toggle
- **Backdrop Click to Close:** Toggle
- **Backdrop Color:** Color picker with opacity

---

## 5️⃣ Media Components

### Image Gallery

**Purpose:** Grid of images with lightbox

**Visual:**
```
┌───────┬───────┬───────┬───────┐
│ Img 1 │ Img 2 │ Img 3 │ Img 4 │
├───────┼───────┼───────┼───────┤
│ Img 5 │ Img 6 │ Img 7 │ Img 8 │
└───────┴───────┴───────┴───────┘
```

**Properties:**
- **Images:** Multiple file upload
- **Columns (Desktop):** 2-6
- **Columns (Tablet):** 2-4
- **Columns (Mobile):** 1-2
- **Gap:** Slider (0-2rem)
- **Image Aspect Ratio:** Square | 16:9 | 4:3 | Original
- **Lightbox:** Toggle
- **Captions:** Toggle
- **Hover Effect:** Zoom | Overlay | Lift

---

### Video Gallery

**Purpose:** Grid of video thumbnails

**Visual:**
```
┌───────┬───────┬───────┐
│ ▶ Vid │ ▶ Vid │ ▶ Vid │
│   1   │   2   │   3   │
├───────┼───────┼───────┤
│ ▶ Vid │ ▶ Vid │ ▶ Vid │
│   4   │   5   │   6   │
└───────┴───────┴───────┘
```

**Properties:**
- **Videos:** Multiple uploads or URLs
- **Columns:** 2-4
- **Gap:** Slider
- **Poster Images:** Auto or custom upload
- **Play in Lightbox:** Toggle
- **Autoplay on Hover:** Toggle

---

### Carousel/Slider

**Purpose:** Rotating content slides

**Visual:**
```
┌─────────────────────────────────────────┐
│  ◀                                   ▶  │
│                                         │
│         [Slide Content]                 │
│                                         │
│         ● ○ ○ ○                         │
└─────────────────────────────────────────┘
```

**Properties:**
- **Slides:** List (add/remove)
  - Content: Image, text, or components
- **Autoplay:** Toggle
- **Autoplay Speed:** Slider (1-10 seconds)
- **Loop:** Toggle
- **Navigation Arrows:** Toggle
- **Pagination Dots:** Toggle
- **Transition:** Slide | Fade | Zoom
- **Transition Speed:** Slider (0.3-2 seconds)

---

### Background Video

**Purpose:** Full-screen video background

**Visual:**
```
┌─────────────────────────────────────────┐
│                                         │
│      [Video Playing in Background]      │
│                                         │
│         Content Overlaid Here           │
│                                         │
└─────────────────────────────────────────┘
```

**Properties:**
- **Video File:** Upload (MP4, WebM)
- **Fallback Image:** Upload (for mobile)
- **Autoplay:** Always on
- **Loop:** Always on
- **Muted:** Always on
- **Overlay Color:** Color picker with opacity
- **Play on Mobile:** Toggle (bandwidth consideration)

---

## 6️⃣ Navigation Components

### Header/Navigation

**Purpose:** Site navigation bar

**Visual:**
```
┌─────────────────────────────────────────┐
│ [Logo]  Home  About  Events  Contact   │
└─────────────────────────────────────────┘
```

**Properties:**
- **Logo:** Image upload
- **Logo Width:** Slider (100px - 300px)
- **Menu Items:** List (add/remove)
  - Label: Text input
  - Link: URL input
  - Submenu: Nested items
- **Layout:** Horizontal | Vertical
- **Alignment:** Left | Center | Right | Space Between
- **Sticky:** Toggle (fixed on scroll)
- **Background:** Color picker
- **Text Color:** Color picker
- **Mobile Menu:** Hamburger | Drawer | Dropdown

---

### Footer

**Purpose:** Site footer with links and info

**Visual:**
```
┌─────────────────────────────────────────┐
│  Column 1     Column 2     Column 3     │
│  • Link 1     • Link 1     • Link 1     │
│  • Link 2     • Link 2     • Link 2     │
│  • Link 3     • Link 3     • Link 3     │
│                                         │
│  © 2024 GFC. All rights reserved.       │
└─────────────────────────────────────────┘
```

**Properties:**
- **Columns:** 1-4
- **Column Content:** Rich text or links
- **Social Media Links:** List
  - Platform: Dropdown
  - URL: Text input
  - Icon: Auto or custom
- **Copyright Text:** Text input
- **Background:** Color picker
- **Text Color:** Color picker
- **Border Top:** Width, style, color

---

### Breadcrumbs

**Purpose:** Navigation trail

**Visual:**
```
Home > Hall Rentals > Pricing
```

**Properties:**
- **Auto-generate:** Toggle (from page hierarchy)
- **Manual Items:** List (if not auto)
- **Separator:** / | > | • | Custom
- **Color:** Color picker
- **Hover Color:** Color picker

---

## 7️⃣ GFC-Specific Components

### Hall Rental Pricing Table

**Purpose:** Display member vs non-member pricing

**Visual:**
```
┌─────────────────────────────────────────┐
│  Hall Rental Pricing                    │
├─────────────────┬───────────┬───────────┤
│  Room           │  Member   │ Non-Member│
├─────────────────┼───────────┼───────────┤
│  Main Hall      │  $200     │  $350     │
│  Lower Hall     │  $150     │  $250     │
│  Full Facility  │  $300     │  $500     │
└─────────────────┴───────────┴───────────┘
```

**Properties:**
- **Pricing Data:** Table editor
  - Room Name: Text
  - Member Price: Number
  - Non-Member Price: Number
- **Currency Symbol:** Text input
- **Table Style:** Striped | Bordered | Minimal
- **Highlight Column:** None | Member | Non-Member

---

### Event Calendar (GFC Events)

**Purpose:** Display GFC club events

**Visual:**
```
┌─────────────────────────────────────────┐
│  Upcoming Events                        │
│                                         │
│  Dec 25 - Christmas Party               │
│  7:00 PM - Main Hall                    │
│                                         │
│  Dec 31 - New Year's Eve                │
│  9:00 PM - Full Facility                │
│                                         │
│  [View Full Calendar]                   │
└─────────────────────────────────────────┘
```

**Properties:**
- **Data Source:** GFC Web App API
- **Display:** List | Calendar | Grid
- **Number of Events:** Slider (1-20)
- **Filter by Category:** Dropdown
- **Show Past Events:** Toggle
- **Link to Details:** Toggle

---

### Photo Album (GFC Photos)

**Purpose:** Display GFC event photos

**Visual:**
```
┌─────────────────────────────────────────┐
│  Christmas Party 2024                   │
│                                         │
│  ┌────┬────┬────┬────┐                 │
│  │ 📷 │ 📷 │ 📷 │ 📷 │                 │
│  └────┴────┴────┴────┘                 │
│                                         │
│  [View Album]                           │
└─────────────────────────────────────────┘
```

**Properties:**
- **Album Source:** GFC Web App API or manual
- **Display:** Grid | Masonry | Carousel
- **Thumbnail Size:** Slider
- **Photos per Row:** 2-6
- **Lightbox:** Toggle

---

### Member Login Widget

**Purpose:** Member authentication

**Visual:**
```
┌─────────────────────────────────────────┐
│  Member Login                           │
│                                         │
│  Username                               │
│  [_________________________________]    │
│                                         │
│  Password                               │
│  [_________________________________]    │
│                                         │
│  ☐ Remember me                          │
│                                         │
│  ┌─────────────┐                        │
│  │    Login    │                        │
│  └─────────────┘                        │
│                                         │
│  Forgot password?                       │
└─────────────────────────────────────────┘
```

**Properties:**
- **Integration:** GFC Web App auth
- **Redirect After Login:** URL input
- **Show Remember Me:** Toggle
- **Show Forgot Password:** Toggle
- **Show Register Link:** Toggle

---

### Social Media Links

**Purpose:** GFC social media profiles

**Visual:**
```
Follow Us:  [f] [📷] [🐦]
```

**Properties:**
- **Platforms:** List
  - Facebook: URL
  - Instagram: URL
  - Twitter/X: URL
  - YouTube: URL
- **Icon Style:** Solid | Outline | Colored
- **Icon Size:** Slider (24px - 64px)
- **Layout:** Horizontal | Vertical
- **Spacing:** Slider

---

## 8️⃣ Advanced Components

### Custom Code Block

**Purpose:** Insert custom HTML/CSS/JS

**Visual:**
```
┌─────────────────────────────────────────┐
│  [Custom Code Renders Here]             │
└─────────────────────────────────────────┘
```

**Properties:**
- **HTML:** Code editor
- **CSS:** Code editor
- **JavaScript:** Code editor
- **Preview:** Live preview toggle
- **Sandbox:** Security toggle

**Use Cases:**
- Third-party widgets
- Custom animations
- Advanced interactions
- External integrations

---

### Embed Widget

**Purpose:** Embed third-party content

**Visual:**
```
┌─────────────────────────────────────────┐
│  [Embedded Content]                     │
└─────────────────────────────────────────┘
```

**Properties:**
- **Embed Code:** Textarea (paste iframe/script)
- **Width:** Percentage or pixels
- **Height:** Pixels
- **Aspect Ratio:** Maintain | Custom

**Supported:**
- YouTube/Vimeo videos
- Google Maps
- Social media posts
- Eventbrite calendars
- Any iframe-based embed

---

### Map

**Purpose:** Google Maps integration

**Visual:**
```
┌─────────────────────────────────────────┐
│                                         │
│         [Google Map]                    │
│         📍 GFC Location                 │
│                                         │
└─────────────────────────────────────────┘
```

**Properties:**
- **Address:** Text input (auto-geocode)
- **Zoom Level:** Slider (1-20)
- **Map Type:** Roadmap | Satellite | Hybrid | Terrain
- **Marker:** Toggle
- **Marker Icon:** Default | Custom upload
- **Height:** Slider (200px - 600px)
- **Interactive:** Toggle (pan/zoom)

---

### Newsletter Signup

**Purpose:** Email collection form

**Visual:**
```
┌─────────────────────────────────────────┐
│  Subscribe to Our Newsletter            │
│                                         │
│  [email@example.com] [Subscribe]        │
│                                         │
│  We respect your privacy.               │
└─────────────────────────────────────────┘
```

**Properties:**
- **Heading:** Text input
- **Placeholder:** Text input
- **Button Text:** Text input
- **Privacy Text:** Text input
- **Integration:** Mailchimp | SendGrid | Custom API
- **Success Message:** Text input
- **Inline Layout:** Toggle (email + button on same line)

---

## 🎨 Component Styling System

### Global Component Settings

All components inherit from global theme:
- **Primary Color**
- **Secondary Color**
- **Font Families**
- **Spacing Scale**
- **Border Radius**
- **Shadows**

### Component-Specific Overrides

Each component can override global settings:
- **Custom Colors**
- **Custom Fonts**
- **Custom Spacing**
- **Custom Borders**

### Responsive Behavior

All components support:
- **Desktop Settings** (>1024px)
- **Tablet Settings** (768px-1024px)
- **Mobile Settings** (<768px)

---

## 🔄 Component Reusability

### Save as Template

Any configured component can be saved as a template:
1. Configure component
2. Click "Save as Template"
3. Name and categorize
4. Reuse across pages

### Component Library

Saved templates appear in component library:
- Personal templates
- Team templates (future)
- Public templates (future)

---

## ✅ Component Checklist

When creating a new component, ensure:
- [ ] All properties are documented
- [ ] Responsive behavior defined
- [ ] Accessibility features included
- [ ] Animation options available
- [ ] SEO considerations addressed
- [ ] Performance optimized
- [ ] Browser compatibility tested

---

**This component library provides a comprehensive foundation for building professional, modern websites with GFC Studio.**
