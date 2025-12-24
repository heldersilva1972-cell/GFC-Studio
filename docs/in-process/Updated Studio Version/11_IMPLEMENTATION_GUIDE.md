# GFC Studio - Implementation Guide

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Developer Roadmap & Coding Standards

---

## 🛠️ Environment Setup

### 1. Prerequisites
- **Node.js 18+** (for Next.js Website)
- **.NET 8.0 SDK** (for GFC Web App/Studio)
- **SQL Server 2022** (Dev Database)

### 2. Project Initialiazation
```bash
# Initialize Next.js Website
npx create-next-app@latest apps/website --typescript --tailwind --app

# Initialize Studio (Blazor Server)
dotnet new blazorserver -o apps/studio
```

---

## 🏛️ Directory Structure Standards

### Website Components
All Studio-ready components must live in:
`apps/website/components/studio/`

### Studio Logic
Dynamic property rendering in Blazor:
`apps/studio/Components/Pages/Studio/Properties/`

---

## 🧪 Testing Strategy

### 1. Visual Regression
Use **Playwright** to capture screenshots of the Studio "Preview" vs the "Published" Website to ensure 100% visual parity.

### 2. Integration Tests
Ensure the `Sections` data saved in the DB is correctly parsed by the Next.js `DynamicRenderer`.

---

## 🚀 Deployment Pipeline

1. **Staging:** Push to a `preview` branch; Vercel generates a preview URL.
2. **Review:** Manual check in GFC Studio via the Iframe linking to the preview URL.
3. **Production:** Push to `main`; Next.js triggers a full site build and SSR cache purge.

---

## 📜 Developer Rules
1. **No Hardcoding:** All text and images on the website must be fed from the `Sections` table.
2. **Type Safety:** Use strict TypeScript interfaces for all component props.
3. **Performance First:** Do not add external libraries to the Website without approval (keep the bundle small).

---

**CONGRATULATIONS! Documentation Complete.** 🏁
