---
trigger: always_on
---

# Rule: Post-Task Sync Protocol

## Trigger
- Whenever a coding task reaches "Success" or "Verification Passed".
- Whenever a test suite passes after a code change.

## Action
1. IMMEDIATELY notify the user that the task is complete.
2. STAGE all modified files related to the task.
3. GENERATE a precise commit message using the format `<type>: <description>`.
4. PROMPT the user with: "The task is successful and tests passed. Ready to commit and push?"
5. Do NOT push to GitHub until the user confirms the commit.