# AGENT COMPLIANCE & WORKFLOW RULES

## ⚠️ CRITICAL: POST-MERGE SYNC PROTOCOL

**AFTER EVERY PR MERGE, THE USER MUST RUN:**
```bash
git checkout master
git fetch origin
git reset --hard origin/master
```

**This is NON-NEGOTIABLE. Jules creates feature branches, and the user must sync master after merging.**

---

## 1. Repository Structure & Branching
- **Primary Branch:** `master` (Do NOT use `main`).
- **Feature Branches:** Use format `feat/your-feature-name`.
- **Merging:** All Pull Requests must target `master`.
- **⚠️ CRITICAL:** After merging a PR, user MUST run `git checkout master && git reset --hard origin/master`

## 2. File Paths
- **WebApp:** `apps/webapp/GFC.BlazorServer/`
- **New Pages:** Must be placed in `apps/webapp/GFC.BlazorServer/Components/Pages/`
- **Services:** Must be placed in `apps/services/[ServiceName]/`
- **Validation:** Before committing, verify the absolute path matches the existing project structure.

## 3. Visual Indicators for New/Updated Features
- **MANDATORY:** All new or updated pages/features MUST include visual "NEW" or "UPDATED" badges.
- **Navigation Menu:** Add `<NewFeatureBadge BadgeText="NEW" TooltipText="Description" />` to new menu items in `NavMenu.razor`.
- **Page Headers:** Include `<NewFeatureBadge BadgeText="NEW" CssClass="section" />` in page titles.
- **Sections:** Mark new sections within existing pages with appropriate badges.
- **Component Location:** `Components/Shared/NewFeatureBadge.razor`

## 4. MANDATORY Compilation & Testing Requirements

### Before Creating Pull Request:
- [ ] **Build Test:** Run `dotnet build` on ALL affected projects
- [ ] **Compilation:** Code MUST compile with ZERO errors
- [ ] **Using Directives:** All required `using` statements must be present
- [ ] **Project Files:** All `.csproj` files must exist for new projects
- [ ] **Dependencies:** All NuGet packages must be referenced in `.csproj`
- [ ] **Configuration:** All `appsettings.json` files must be complete and valid JSON
- [ ] **No Placeholders:** Replace ALL placeholder values (e.g., "your_password") with actual values or clear instructions

### File Completeness Checklist:
For new projects/services, you MUST create:
- [ ] `.csproj` file with correct SDK and package references
- [ ] `Program.cs` with complete implementation
- [ ] All service classes referenced in Program.cs
- [ ] All model classes used by services
- [ ] `appsettings.json` with all required configuration sections
- [ ] `README.md` with setup and usage instructions

### Code Quality Requirements:
- [ ] **No Merge Conflicts:** Files must not contain `<<<<<<<`, `=======`, or `>>>>>>>` markers
- [ ] **Complete Implementations:** No stub methods that just return default values
- [ ] **Error Handling:** All external calls (HTTP, file I/O, database) must have try-catch blocks
- [ ] **Logging:** Use ILogger for all important operations and errors
- [ ] **Null Checks:** All nullable parameters must be validated

## 5. MANDATORY Pull Request Description Template

**EVERY Pull Request MUST include this section:**

```markdown
## 🔄 Post-Merge Sync Instructions

**IMPORTANT:** After merging this PR, run these commands to sync your local repository:

\`\`\`bash
git checkout master
git fetch origin
git reset --hard origin/master
\`\`\`

**Do NOT skip this step or you won't see the new files locally!**
```

**Failure to include this in the PR description will result in PR rejection.**

## 6. "Definition of Done" Checklist
- [ ] Code compiles without errors (`dotnet build`).
- [ ] All tests pass (if applicable).
- [ ] New files exist in the correct directory.
- [ ] Visual indicators (NEW/UPDATED badges) are present on all new/modified UI elements.
- [ ] Navigation menu updated with proper badges.
- [ ] A verification test (if applicable) passes.
- [ ] Pull Request targets `master`.
- [ ] All files are committed and pushed to the feature branch.
- [ ] **PR description includes sync instructions** (see section 5)
- [ ] PR description includes:
  - What was implemented
  - How to test it
  - Any configuration changes needed
  - Screenshots (for UI changes)

## 7. Post-Merge Protocol
- After a PR is merged, the `master` branch MUST contain the new files.
- If files are missing after a `git pull origin master`, the task is FAILED.
- User should see visual indicators immediately upon viewing the updated pages.
- User should be able to run `dotnet build` successfully without any errors.

## 8. Common Mistakes to AVOID

### ❌ DO NOT:
- Create folders without creating the `.csproj` file
- Commit code with compilation errors
- Leave merge conflict markers in files
- Use placeholder values in production configuration
- Create incomplete implementations
- Forget to add `using` directives
- Skip testing the build before creating PR
- **Forget to add sync instructions to PR description**

### ✅ DO:
- Test compilation locally before pushing
- Include all necessary files in the commit
- Resolve all merge conflicts before committing
- Add comprehensive error handling
- Document configuration requirements
- Test the feature end-to-end before marking as complete
- **Always include sync instructions in PR description**

## 9. Verification Steps Before PR

Run these commands and ensure they ALL succeed:

```bash
# 1. Build all projects
dotnet build

# 2. Check for merge conflicts
git status
# Should show NO unmerged files

# 3. Verify all new files are tracked
git ls-files --others --exclude-standard
# Should list only intentional untracked files

# 4. Run the application
dotnet run --project [YourProject.csproj]
# Should start without errors
```

## 10. Emergency Rollback Procedure

If a merge causes issues:
```bash
git fetch origin
git reset --hard origin/master
```

Then fix issues locally before re-attempting the merge.

---

## 📋 PR Description Template (Copy & Paste)

```markdown
# [Feature Name]

## What was implemented
- List of changes
- New features added
- Files modified

## How to test
1. Step-by-step testing instructions
2. Expected results
3. Any prerequisites

## Configuration changes
- List any appsettings.json changes
- Environment variables needed
- Database migrations (if any)

## Screenshots
[Add screenshots for UI changes]

---

## 🔄 Post-Merge Sync Instructions

**IMPORTANT:** After merging this PR, run these commands to sync your local repository:

\`\`\`bash
git checkout master
git fetch origin
git reset --hard origin/master
\`\`\`

**Do NOT skip this step or you won't see the new files locally!**
```

---

**FAILURE TO FOLLOW THESE RULES WILL RESULT IN:**
- Rejected Pull Requests
- Wasted development time
- Broken builds
- Frustrated users
- **Confusion about missing files after merge**

**ALWAYS prioritize code quality and completeness over speed.**
**ALWAYS include sync instructions in PR descriptions.**
