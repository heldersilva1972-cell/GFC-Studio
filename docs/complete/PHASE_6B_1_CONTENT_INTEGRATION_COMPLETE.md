# PHASE 6B-1: CONTENT INTEGRATION (The "Wiring")

**Objective:** Connect the Next.js public website to the Studio API so it displays dynamic content instead of hardcoded text.

## 1. API Development (Web App Side)
- [ ] **Public API Endpoints**: Create open (read-only) endpoints in the .NET App.
    - `GET /api/public/pages/{slug}`: Returns the JSON content for a specific page.
    - `GET /api/public/navigation`: Returns the menu structure.
    - `GET /api/public/events`: Returns upcoming events.

## 2. Front-End Integration (Next.js Side)
- [ ] **API Client**: Service in Next.js to fetch data from the .NET API.
- [ ] **Dynamic Page Component**: A "Catch-All" route (`[[...slug]].tsx`) that renders the page based on the JSON received from the API.
- [ ] **Component Mapper**: Logic to map Studio "Block Types" (e.g., "HeroBlock", "TextColumn") to React Components.

## 3. Legacy Content Seeding
- [ ] **Seed Script**: A one-time SQL script or C# utility to take the text extracted in Phase 0 and insert it into the `StudioPages` table so the site isn't empty on launch.
