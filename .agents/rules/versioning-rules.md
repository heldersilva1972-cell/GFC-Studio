---
trigger: always_on
---

# VERSIONING RULE
* **NO MANUAL EDITS:** Do NOT manually edit version numbers in .props, .csproj, .cs, or .json files.
* **MANDATORY SCRIPT:** You MUST use `powershell.exe -File .\sync-version.ps1`.
* **REMOTE SYNC:** Ensure the script completes the "Remote Database Sync" step before finishing.
