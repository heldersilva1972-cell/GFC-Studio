# Phase 12: Studio CMS & Publishing Workflow
## Objective
Implement the "CMS" infrastructure to manage multiple pages, maintain history, and handle the publishing lifecycle.

## Key Deliverables
- [ ] **Page Navigator:** Sidebar UI to switch between and create new pages.
- [ ] **Data Safety:** Debounced auto-save to `Drafts` table.
- [ ] **Versioning:** Snapshot system for rollbacks with custom naming.
- [ ] **Publishing:** Production JSON push with Next.js cache revalidation.

## Technical Notes
- Uses SQL-backed JSON storage.
- Implements a Workflow State engine (Draft -> Review -> Published).
