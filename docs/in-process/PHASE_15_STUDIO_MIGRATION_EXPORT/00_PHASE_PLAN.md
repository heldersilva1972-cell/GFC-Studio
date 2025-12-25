# Phase 15: Migration & Data Portability
## Objective
Implement tools for harvesting legacy site content and ensuring the new system's data can be exported and backed up.

## Key Deliverables
- [ ] **URL Scraper:** Content harvesting engine to pull text/images from the old site.
- [ ] **Block Converter:** Intelligent conversion of raw HTML to Studio blocks.
- [ ] **Static Export:** One-click full-site backup in `.zip` or `Static HTML` format.
- [ ] **Redirect Engine:** Automatic generation of `301 Redirects` for old links.

## Technical Notes
- Uses `Playwright` or `Puppeteer` for headful scraping during ingest.
- Export logic generates optimized static bundles for disaster recovery.
