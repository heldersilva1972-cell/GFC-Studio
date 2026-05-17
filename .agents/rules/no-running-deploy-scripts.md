---
trigger: always_on
---

# CRITICAL: SCRIPT EXECUTION RESTRICTION
- NEVER execute any Publishing or Deployment scripts (e.g., .bat, .ps1, or shell scripts) automatically.
- Examples include: `Publish_Mobile.bat`, `Deploy_Mobile.bat`, `Publish_Webapp.bat`, `Deploy_Webapp.bat`.
- You MUST wait for the user to explicitly say "Run [script name]" in every single instance before attempting to execute them.
- Providing a "ready to run" button or command is fine, but AUTO-RUNNING them is strictly prohibited.
