---
trigger: always_on
---

# VERSIONING RULE
* **NO MANUAL EDITS:** Do NOT manually edit version numbers in .props, .csproj, .cs, or .json files.
* **NO AUTOMATIC VERSION SYNC:** NEVER run `powershell.exe -File .\sync-version.ps1` automatically after code changes or bug fixes.
* **USER EXPLICIT REQUEST ONLY:** You MUST wait for the user to explicitly say "Run sync-version" or ask you to bump/sync the version before executing `powershell.exe -File .\sync-version.ps1`.

