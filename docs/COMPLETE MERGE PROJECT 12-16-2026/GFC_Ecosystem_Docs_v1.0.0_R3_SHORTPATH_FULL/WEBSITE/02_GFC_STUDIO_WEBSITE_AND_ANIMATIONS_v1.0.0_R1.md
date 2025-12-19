# Studio: Website Editor + Animation Editor — How it works (v1.0.0 R1)

## What Studio edits
Studio edits TWO things, side-by-side:
1) Website content (pages, blocks, tiles, images, calendars)
2) Animations (components/JSON-based animations + parameters)

The key idea: website pages are built from **structured blocks**, and blocks can optionally reference an animation.

## Website editing model (no-code)
- A “Site” contains Pages.
- A Page contains Blocks.
- Blocks are typed (Hero, TileGrid, RichText, Gallery, Calendar, PricingTable, ContactForm, HallRentalApplyCTA).
- Each block exposes a schema of editable properties (text, images, links, options).

No raw HTML editing. No code required.

## Animation editing model
- Animation library is organized by categories.
- Each animation has:
  - JSON definition (Studio-native format)
  - Parameter schema (sliders/toggles/color pickers/text inputs)
  - Preview sandbox surface

Animations can be attached to:
- Specific blocks (e.g., Hero background animation)
- Specific UI elements (e.g., button hover animation)
- Event tiles (e.g., “Sweet Bread” countdown tile)

## The 3 preview modes (Desktop / Tablet / Mobile)
Studio shows a single “Preview Panel” with:
- Device toggles (Desktop/Tablet/Mobile)
- A live render of the page using the same rendering engine used by the public site
- Optional “Split View”:
  - Left = Edit controls
  - Right = Preview

Preview can also show:
- “Published” vs “Draft”
- “Before / After” comparison (optional)
- “Safe publish” step (confirmation + summary)

## Hall Rentals section inside Studio
Studio gets a dedicated navigation section:
- Hall Rentals
  - Calendar (availability, blocks, tentative holds)
  - Policies (editable content)
  - Pricing (editable table)
  - Application workflow settings (email routing, temporary holds, required deposit placeholders)
  - History / Reporting (who rented, booked, canceled, deposit not paid, discounts, add-ons)

Studio can edit what is “shown” on the website.
The Web App owns the real workflow and data (applications, holds, approvals, payments later).

## Merge boundary: Web App vs Studio
- Web App = system-of-record for:
  - hall rental requests, approvals, holds, calendar state, audit logs, payments (future), notifications
- Studio = no-code editor for:
  - how pages look, the copy, tiles/templates, images, which data views are embedded, animation selection

Studio never decides business rules. It only controls presentation + templates.
