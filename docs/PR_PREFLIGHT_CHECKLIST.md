# Pre-Flight Checklist for Pull Requests

**MANDATORY: Complete this checklist BEFORE creating a Pull Request**

## 1. Compilation Check ✅
```bash
cd [project-directory]
dotnet build
```
- [ ] Build completes with **ZERO errors**
- [ ] Build completes with **ZERO warnings** (or warnings are documented)
- [ ] All projects in the solution build successfully

**If build fails:** Fix ALL errors before proceeding.

---

## 2. File Completeness Check ✅

### For New Projects:
- [ ] `.csproj` file exists and contains all package references
- [ ] `Program.cs` exists and is complete
- [ ] All service classes exist
- [ ] All model classes exist
- [ ] `appsettings.json` exists and is valid JSON
- [ ] `README.md` exists with setup instructions

### For All Changes:
- [ ] All referenced files exist
- [ ] All `using` directives are present
- [ ] No placeholder values remain (e.g., "your_password", "TODO", "FIXME")
- [ ] All configuration values are set or clearly documented

---

## 3. Code Quality Check ✅

- [ ] No merge conflict markers (`<<<<<<<`, `=======`, `>>>>>>>`)
- [ ] No commented-out code blocks (unless documented why)
- [ ] All methods have implementations (no empty stubs)
- [ ] Error handling exists for all external calls
- [ ] Logging statements exist for important operations
- [ ] Null checks exist for all nullable parameters

---

## 4. Visual Indicators Check ✅ (UI Changes Only)

- [ ] `<NewFeatureBadge>` added to navigation menu items
- [ ] `<NewFeatureBadge>` added to page headers
- [ ] Visual indicators match the feature (NEW vs UPDATED)
- [ ] Tooltips are descriptive and helpful

---

## 5. Testing Check ✅

- [ ] Application starts without errors (`dotnet run`)
- [ ] New features are accessible through the UI
- [ ] New features work as expected
- [ ] No console errors in browser (F12)
- [ ] No runtime exceptions in terminal

---

## 6. Git Check ✅

```bash
git status
```
- [ ] All new files are staged (`git add`)
- [ ] No unmerged files
- [ ] Branch targets `master` (not `main`)
- [ ] Commit message follows format: `<type>: <description>`

---

## 7. Documentation Check ✅

- [ ] PR description explains what was implemented
- [ ] PR description explains how to test
- [ ] Configuration changes are documented
- [ ] Breaking changes are clearly marked
- [ ] Screenshots included for UI changes

---

## 8. Final Verification ✅

Run these commands in sequence:

```bash
# Clean build
dotnet clean
dotnet build

# Run application
dotnet run --project [YourProject.csproj]
```

- [ ] Clean build succeeds
- [ ] Application starts
- [ ] Feature works end-to-end
- [ ] No errors in console/terminal

---

## ✅ READY TO CREATE PR

If ALL checkboxes above are checked, you may create the Pull Request.

If ANY checkbox is unchecked, **DO NOT CREATE PR**. Fix the issues first.

---

## 🚨 Common Failures

**Missing .csproj file:**
```bash
dotnet new console -n ProjectName
# or
dotnet new web -n ProjectName
```

**Missing using directive:**
Add at top of file:
```csharp
using System.Collections.Concurrent;
using System.Text.Json;
// etc.
```

**Merge conflicts:**
```bash
git fetch origin
git merge origin/master
# Resolve conflicts manually
git add .
git commit
```

**Placeholder values:**
Search for:
- "your_password"
- "REPLACE_WITH"
- "TODO"
- "FIXME"

Replace with actual values or add clear instructions in README.

---

**Remember: Quality over speed. A complete, working PR is better than a fast, broken one.**
