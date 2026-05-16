---
description: 
---

# POS Versioning Workflow
1. Propose the next version number based on the current history.
2. Wait for USER approval of the version number.
3. Run: `powershell.exe -File .\sync-version.ps1 -Project POS -Next`
4. Verify all 12+ files were updated (Check console output).
5. Manually add the "Revision History" line to `PosVersionService.cs`.
