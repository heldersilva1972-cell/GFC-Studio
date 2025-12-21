# GFC Website Baseline (from current live site export) — v1.0.0 R1

## Source artifacts used
These were provided as HTML exports from gloucesterfraternityclub.com:
- gloucesterfraternityclub.com.html  |  Page: Gloucester Fraternity Club  |  <title>: Gloucester Fraternity Club – 27 Webster Street, Gloucester, MA 01930
- hall-rentals.html  |  Page: Hall Rentals  |  <title>: Hall Rentals – Gloucester Fraternity Club
- hall-rental-policy.html  |  Page: Hall Rental Policy  |  <title>: Hall Rental Policy – Gloucester Fraternity Club
- hall-rental-pricing.html  |  Page: Hall Rental Pricing  |  <title>: Hall Rental Pricing – Gloucester Fraternity Club
- hall-rental-calendar.html  |  Page: Hall Rental Calendar  |  <title>: Hall Rental Calendar – Gloucester Fraternity Club
- hall-rental-application.html  |  Page: Hall Rental Application (old)  |  <title>: Hall Rental Application (old) – Gloucester Fraternity Club
- club-activities.html  |  Page: GFC Club Activities Calendar  |  <title>: GFC Club Activities Calendar – Gloucester Fraternity Club
- photos.html  |  Page: Photos  |  <title>: Photos – Gloucester Fraternity Club
- special-needs.html  |  Page: Special Needs  |  <title>: Special Needs – Gloucester Fraternity Club
- contact-us-2.html  |  Page: Contact Us  |  <title>: Contact Us – Gloucester Fraternity Club

## Goal of this baseline
- Provide a **known-good content baseline** Studio can import and modernize.
- Define the **editable content model** so Studio can edit without coding.
- Define the **preview surfaces** (Desktop/Tablet/Mobile) so edits can be reviewed before publish.

## Page Inventory + Recommended Studio Template Mapping

### HOME (gloucesterfraternityclub.com.html)
Template: `HomePage`
Blocks (recommended):
- Hero (headline + short welcome text)
- "What We Offer" / Highlights (cards/tiles)
- "Hall Rentals" CTA tile linking to Hall Rentals area
- "Activities" CTA tile linking to Club Activities calendar
- "Photos" CTA tile linking to Gallery
- Footer contact/address block

Editable fields:
- Hero headline + body
- CTA tile titles/subtitles/links
- Footer contact fields

### HALL RENTALS OVERVIEW (hall-rentals.html)
Template: `HallRentalsLanding`
Blocks:
- Hero + summary
- "How to Rent" (ordered steps)  **(must stay structured)**
- Hall Manager contact block
- Mailing address block

Editable fields:
- Hero title/subtitle
- Steps: title + body + links (Calendar/Pricing/Application)
- Hall manager name/phone/email link
- Mailing address

### HALL RENTAL POLICY (hall-rental-policy.html)
Template: `PolicyPage`
Blocks:
- Title + Last updated (optional)
- Policy sections (accordion recommended)

Editable fields:
- All policy text
- Optional “download/print” switch

### HALL RENTAL PRICING (hall-rental-pricing.html)
Template: `PricingPage`
Blocks:
- Pricing tables (member/non-member if applicable)
- Deposits / add-ons / notes
- CTA to apply

Editable fields:
- Pricing rows
- Deposit amount (future: payment placeholder)
- Add-on list

### HALL RENTAL CALENDAR (hall-rental-calendar.html)
Template: `CalendarPage`
Blocks:
- Calendar component
- Filters (room/type/status)
- Legend (Available / Tentative / Booked / Blocked)
- CTA to apply

Editable fields:
- Page copy above calendar
- Legend labels/colors (Studio config)
- Help text

### HALL RENTAL APPLICATION (hall-rental-application.html)
Template: `ApplicationPage`
Blocks:
- Application form
- “What happens next” info
- Privacy notice

Editable fields:
- Intro copy
- Confirmation message
- Routing rules (who gets emailed) **(Web App config, surfaced in Studio)**

Form fields detected in export (legacy / Contact Form 7 style):
| Field (name) | Type | Notes |
|---|---|---|
| _wpcf7 | hidden | WP Contact Form 7 hidden |
| _wpcf7_version | hidden | WP Contact Form 7 hidden |
| _wpcf7_locale | hidden | WP Contact Form 7 hidden |
| _wpcf7_unit_tag | hidden | WP Contact Form 7 hidden |
| _wpcf7_container_post | hidden | WP Contact Form 7 hidden |
| _wpcf7_posted_data_hash | hidden | WP Contact Form 7 hidden |
| _wpcf7_recaptcha_response | hidden | WP Contact Form 7 hidden |
| FunctionDate | date |  |
| menu-START | select |  |
| menu-END | select |  |
| applicant-name | text |  |
| applicant-email | email |  |
| applicant-tel | tel |  |
| menu-function_type | select |  |
| Function_desc | text |  |
| number-people | number |  |
| checkbox-bar | checkbox |  |
| checkbox-bar | checkbox |  |
| checkbox-kitchen | checkbox |  |
| checkbox-kitchen | checkbox |  |
| checkbox-member | checkbox |  |
| checkbox-member | checkbox |  |
| applicant-street | text |  |
| applicant-city | text |  |
| applicant-city | select |  |

### CLUB ACTIVITIES (club-activities.html)
Template: `ClubCalendarPage`
Blocks:
- Calendar component (events)
- Categories filter
- CTA to subscribe/add to calendar (optional)

Existing site appears to be a WordPress site using the "google-calendar-events" plugin (Simple Calendar style).
Evidence: pages include CSS/asset paths containing `/wp-content/plugins/google-calendar-events/` (hall-rental-calendar.html, club-activities.html).
In the new baseline, the calendar should be first-class in the GFC Website + Web App (Hall Rentals module) instead of relying on WP plugins.


### PHOTOS (photos.html)
Template: `GalleryPage`
Blocks:
- Album list OR gallery grid
- Lightbox viewer

Editable fields:
- Album names/descriptions
- Gallery ordering
- Upload destinations (album assignments)

### SPECIAL NEEDS (special-needs.html)
Template: `ContentPage`
Blocks:
- Rich text sections
- Photos/CTA blocks

Editable fields:
- All section copy + images

### CONTACT (contact-us-2.html)
Template: `ContactPage`
Blocks:
- Contact details block (address/phone/email)
- Contact form
- Map (optional)

Form fields detected:
| Field (name) | Type | Notes |
|---|---|---|
| _wpcf7 | hidden | WP Contact Form 7 hidden |
| _wpcf7_version | hidden | WP Contact Form 7 hidden |
| _wpcf7_locale | hidden | WP Contact Form 7 hidden |
| _wpcf7_unit_tag | hidden | WP Contact Form 7 hidden |
| _wpcf7_container_post | hidden | WP Contact Form 7 hidden |
| _wpcf7_posted_data_hash | hidden | WP Contact Form 7 hidden |
| _wpcf7_recaptcha_response | hidden | WP Contact Form 7 hidden |
| your-name | text | Contact Us form |
| your-email | email | Contact Us form |
| your-tel | tel | Contact Us form |
| your-subject | text | Contact Us form |
| your-message | textarea | Contact Us form |

## Studio Preview Requirement (Desktop/Tablet/Mobile)
Studio must provide 3 preview modes for any page edit:
- Desktop (wide)
- Tablet (medium)
- Mobile (narrow)

Implementation notes:
- Use a “device toggle” (Desktop/Tablet/Mobile) that resizes a preview frame.
- Preview supports:
  - Content changes (text/images/tiles)
  - Animation changes (for blocks that include animations)
- Provide “Draft vs Published” mode.
- Provide “Publish” button that commits changes to the live site (never auto-publish).

## Publish / Draft Model
- Every page exists as:
  - Draft (editable)
  - Published (live)
- Publish creates a new site revision entry:
  - Page, author, timestamp, summary, diff metadata.

