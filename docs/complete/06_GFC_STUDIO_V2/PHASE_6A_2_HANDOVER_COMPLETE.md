# JULES HANDOVER: PHASE 6A-2 (ADVANCED BUILDER TOOLS)

## Current Status
- **Phase 6A-1 (Core Engine)** is complete. 
- You can now save multiple draft versions of a page.
- "Publish" promotes a specific draft to the live `StudioPage` sections.
- The UI in `Studio.razor` has a "Draft History" dropdown and distinct "Save Draft" / "Publish" buttons.

## Your Objective: Phase 6A-2
Enhance the Visual Editor UI and intelligence.

### 1. Animation Orchestrator (UI)
- **File:** `apps\webapp\GFC.BlazorServer\Components\Pages\Studio.razor`
- **Task:** Improve the `animation-orchestrator` div.
- **Goal:** Create a visual "Timeline" bar. 
  - Each section in the canvas should appear as a track or a point on a 5-second bar.
  - Allow users to drag a slider to set `Delay` and `Duration`.
  - The "Preview" button should trigger the `previewAnimation` JS function (already in `Studio.razor` via `IJSRuntime`).

### 2. Template Library Refinement
- **Current State:** Basic `ITemplateService` exists. `Studio.razor` allows saving a section as a template and dragging from a simple list.
- **Task:** 
  - Enhance the "Left Toolbox" -> "Templates" tab.
  - Use visual cards/thumbnails for templates if possible (placeholders for now).
  - Implement a search/filter by Category (Category field is already in the `StudioTemplate` model).
  - Ensure `TemplateService` correctly handles persistence in the `StudioTemplates` table.

### 3. Wizard AI (Logic)
- **File:** `apps\webapp\GFC.BlazorServer\Components\Shared\StudioWizard.razor` (and related services)
- **Task:** Move beyond placeholders.
- **Layout Doctor:** 
  - Implement actual logic in `RunLayoutDoctor`. 
  - Example: Check if the section content contains too many nested divs or if images lack `max-width: 100%`.
- **SEO Health Bar:**
  - Logic to check if the page has at least one (and only one) `<h1>`.
  - Check for missing `alt` attributes on `<img>` tags.
  - Provide a score or warning list.

## Files to Watch
- `apps\webapp\GFC.BlazorServer\Components\Pages\Studio.razor` (Main Editor)
- `apps\webapp\GFC.BlazorServer\Components\Shared\StudioWizard.razor` (Wizard Logic)
- `apps\webapp\GFC.Core\Models\StudioTemplate.cs` (Template Schema)
- `apps\webapp\GFC.BlazorServer\Services\TemplateService.cs` (Template Logic)

## Context Reference
Refer to `docs\in-process\PHASE_6A_STUDIO\PHASE_6A_2_ADVANCED_TOOLS.md` for the original checklist.
