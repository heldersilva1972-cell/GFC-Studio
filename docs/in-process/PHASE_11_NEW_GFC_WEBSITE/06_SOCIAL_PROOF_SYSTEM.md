# GFC Website - Social Proof System

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Status:** Operational Specification  
**Goal:** Build trust through community-driven reviews while maintaining 100% control over content.

---

## 🎭 The Philosophy

Word-of-mouth is the lifeblood of a community club. We want to enable people who have had great experiences (weddings, parties, community drives) to share their stories, but we must protect the club from spam or inappropriate content.

---

## 📥 1. Submission: "Share Your Story"

A discreet but accessible link (e.g., in the footer or on the Hall Rental success page) opens a simple, elegant submission form.

### Public Form Fields:
- **Display Name:** (e.g., "The Smith Family" or "Jane D.")
- **Event Type:** (Dropdown: Hall Rental, Community Event, Member Interaction, etc.)
- **The Story:** (Textarea: "Tell us about your experience.")
- **Rating:** (1-5 Stars - internal only, doesn't have to show on the card if preferred).
- **Consent:** Checkbox: *"I give GFC permission to share this story on their website."*

---

## 🛡️ 2. The Moderation Queue (Web App)

All submissions go into a "Holding Tank" in the GFC Web App. Nothing goes live automatically.

### Admin Controls:
- **Review List:** See all new entries with timestamps.
- **The "Featured" Toggle:** A switch that determines if a review is visible on the website.
- **Edit Capability:** Admins can fix minor typos or adjust formatting for a cleaner look before approving.
- **Archive:** Hide reviews that are old or no longer relevant.

---

## 🎨 3. Website Display (Social Proof Module)

Only "Approved" and "Featured" reviews are pulled by the Website API.

### Visual Presentation:
- **Card Design:** Sleek, minimalist cards sitting on the Charcoal background.
- **Typography:** Quotes in the secondary "Champagne" color to make them stand out.
- **Badge:** A small icon indicating the event type (e.g., a "Celebration" icon for a birthday rental).
- **Layout:** A smooth, auto-playing "Carousel" or a masonry grid (depending on Studio configuration).

---

## 🤖 4. Technical Integration

### Database Table: `PublicReviews`
| Field | Type | Description |
| :--- | :--- | :--- |
| `Id` | INT | Primary Key |
| `DisplayName` | STRING | Name to show publically |
| `EventType` | STRING | Category of the review |
| `Content` | TEXT | The actual review text |
| `IsApproved` | BIT | Moderation Status |
| `IsFeatured` | BIT | Should it be in the Homepage slider? |
| `CreatedAt` | DATETIME | Timestamp of submission |

### API Logic:
- **Endpoint:** `GET /api/public/reviews`
- **Filter:** `WHERE IsApproved = true AND IsFeatured = true`
- **Security:** Submission endpoint is rate-limited to 1 per user per hour to prevent spamming.

---

## 📈 Success Metric

When someone visits the Hall Rental page, they see 3-5 high-quality, approved reviews from real neighbors, significantly increasing the chance they will book a tour or start an application.
