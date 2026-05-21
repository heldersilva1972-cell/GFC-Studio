# GFC System - Studio V2

## 🚀 MANDATORY VERSIONING PROTOCOL
All version updates **MUST** be performed using the synchronization script to ensure ecosystem parity (POS, Mobile, Server):
- **POS:** `powershell.exe -File .\sync-version.ps1 -Project POS -Next`
- **Mobile:** `powershell.exe -File .\sync-version.ps1 -Project Mobile -Next`

---

# GFC Web app studio

This is the unified workspace for the GFC Ecosystem, containing the consolidated applications as defined in the **R1 Consolidated Master Spec**.

## Structure

- **apps/webapp**: The .NET / Blazor "Web App" acting as the **System of Record** and backend. All business rules, validations, and permissions defined here MUST be respected by all other apps.
- **apps/GFC-Pos-Standalone**: The standalone Point-of-Sale Blazor Hybrid app for offline-capable member drawings and terminal services.
- **apps/GFC-Mobile-Standalone**: The member-facing standalone mobile client app.
- **apps/studio**: The Next.js / React-based "Studio" for no-code authoring and public website rendering.

---

## Project Reference Glossary

To prevent naming confusion during development and when working with AI coding assistants (like Cursor, Gemini, or Copilot), use this standard reference glossary:

| Repository Folder | Standard Developer/AI Alias | Key Namespaces / Projects | Purpose / Description |
| :--- | :--- | :--- | :--- |
| `apps/webapp` | **WebApp** / **Web Portal** | `GFC.BlazorServer`, `GFC.Core`, `GFC.Data` | The main .NET Blazor Server web application, API backend, and **System of Record** for core business logic, validations, and database governance. |
| `apps/GFC-Pos-Standalone` | **POS App** / **Terminal** | `GFC.Pos.UI`, `GFC.Pos.Terminal`, `GFC.Pos.Mobile` | The standalone Point-of-Sale terminal application used by staff for check-ins, physical member drawings, and offline operational states. |
| `apps/GFC-Mobile-Standalone` | **Mobile App** / **Member App** | `GFC.Mobile` | The standalone member-facing mobile client application for viewing stats, check-ins, and individual profiles. |
| `apps/studio` | **Studio** | `apps/studio` (Next.js / React) | The Next.js based no-code authoring dashboard and public-facing marketing/informational website. |

### 💡 Guidelines for AI Coding Assistants
When receiving requests, always use the mappings above to target the correct workspace paths:
- "Work on the **WebApp**" ➔ Target paths inside `apps/webapp/`
- "Work on the **POS App**" or "**Terminal**" ➔ Target paths inside `apps/GFC-Pos-Standalone/`
- "Work on the **Mobile App**" or "**Member App**" ➔ Target paths inside `apps/GFC-Mobile-Standalone/`
- "Work on the **Studio**" ➔ Target paths inside `apps/studio/`

---

## Governance & Validation
The **Web App** is the authoritative source for:

1.  **Data Validation**: All data written by the Studio must pass Web App validation rules.
2.  **Permissions**: Access control is governed by the Web App's identity system.
3.  **Business Logic**: Complex workflows (e.g. Hall Rentals) must execute via the Web App.


## Getting Started

### Studio (Frontend)
1. Navigate to `apps/studio`
2. Run `npm install`
3. Run `npm run dev`

### Web App (Backend)
1. Navigate to `apps/webapp`
2. Open the solution file or run `dotnet build`

## Documentation
Refer to the `GFC-Docs` folder in the root or the `README.md` files within each app for specific details.
