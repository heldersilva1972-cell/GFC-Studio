# GFC Web app studio

This is the unified workspace for the GFC Ecosystem, containing the consolidated applications as defined in the **R1 Consolidated Master Spec**.

## Structure

- **apps/studio**: The Next.js / React-based "Studio" for no-code authoring and public website rendering.
- **apps/webapp**: The .NET / Blazor "Web App" acting as the **System of Record** and backend. All business rules, validations, and permissions defined here MUST be respected by the Studio.

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
