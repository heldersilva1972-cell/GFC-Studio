# AGENT COMPLIANCE & WORKFLOW RULES

## 1. Repository Structure & Branching
- **Primary Branch:** `master` (Do NOT use `main`).
- **Feature Branches:** Use format `feat/your-feature-name`.
- **Merging:** All Pull Requests must effectively target `master`.

## 2. File Paths
- **WebApp:** `apps/webapp/GFC.BlazorServer/`
- **New Pages:** Must be placed in `apps/webapp/GFC.BlazorServer/Components/Pages/`
- **Validation:** Before committing, verify the absolute path matches the existing project structure.

## 3. "Definition of Done" Checklist
- [ ] Code compiles without errors (`dotnet build`).
- [ ] New files exist in the correct directory.
- [ ] A verification test (if applicable) passes.
- [ ] Pull Request targets `master`.

## 4. Post-Merge Protocol
- After a PR is merged, the `master` branch MUST contain the new files.
- If files are missing after a `git pull origin master`, the task is FAILED.
