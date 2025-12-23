# PHASE 6B-3: LAUNCH PREP & DEPLOYMENT

**Objective:** Final polish and moving the new website to the production domain.

## 1. SEO & Metadata
- [ ] **Dynamic Meta Tags**: Ensure the "Page Title" and "Description" from Studio are injected into the HTML `<head>`.
- [ ] **Sitemap.xml**: Auto-generate a sitemap based on active pages.
- [ ] **Robots.txt**: Configure crawling rules.

## 2. Performance Tuning
- [ ] **Image Optimization**: Ensure images uploaded in Studio are served in Next-Gen formats (WebP) via Next.js Image component.
- [ ] **Lighthouse Audit**: Aim for 90+ scores in Performance, Accessibility, Best Practices, and SEO.

## 3. Production Deployment
- [ ] **Environment Config**: Set up Production API URLs in `.env`.
- [ ] **Build Process**: `npm run build` validation.
- [ ] **DNS Cutover**: Determining the strategy to switch `gloucesterfraternityclub.com` to the new host.
