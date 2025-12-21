# AGENT COMPLIANCE & WORKFLOW RULES

## 1. Repository Structure & Branching
- **Primary Branch:** `master` (Do NOT use `main`).
- **Feature Branches:** Use format `feat/your-feature-name`.
- **Merging:** All Pull Requests must target `master`.

## 2. File Paths
- **WebApp:** `apps/webapp/GFC.BlazorServer/`
- **New Pages:** Must be placed in `apps/webapp/GFC.BlazorServer/Components/Pages/`
- **Validation:** Before committing, verify the absolute path matches the existing project structure.

## 3. Visual Indicators for New/Updated Features
- **MANDATORY:** All new or updated pages/features MUST include visual "NEW" or "UPDATED" badges.
- **Navigation Menu:** Add `<NewFeatureBadge BadgeText="NEW" TooltipText="Description" />` to new menu items in `NavMenu.razor`.
- **Page Headers:** Include `<NewFeatureBadge BadgeText="NEW" CssClass="section" />` in page titles.
- **Sections:** Mark new sections within existing pages with appropriate badges.
- **Component Location:** `Components/Shared/NewFeatureBadge.razor`

## 4. "Definition of Done" Checklist
- [ ] Code compiles without errors (`dotnet build`).
- [ ] New files exist in the correct directory.
- [ ] Visual indicators (NEW/UPDATED badges) are present on all new/modified UI elements.
- [ ] Navigation menu updated with proper badges.
- [ ] A verification test (if applicable) passes.
- [ ] Pull Request targets `master`.
- [ ] All files are committed and pushed to the feature branch.

## 5. Post-Merge Protocol
- After a PR is merged, the `master` branch MUST contain the new files.
- If files are missing after a `git pull origin master`, the task is FAILED.
- User should see visual indicators immediately upon viewing the updated pages.
