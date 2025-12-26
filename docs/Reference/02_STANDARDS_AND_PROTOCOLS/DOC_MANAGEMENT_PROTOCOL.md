# Rule: Documentation Management Protocol

This rule must be followed by both the local agent (me) and the cloud agent (Jules) without exception. EVERYTHING we work from or are currently working on MUST be located in the `docs/in-process/` folder.

## Directory Structure
All project documentation and implementation plans are stored in the **unified `docs/` directory** at the project root:

```
GFC-Studio V2/
└── docs/
    ├── in-process/     - Active plans and ongoing tasks
    ├── complete/       - Finalized documentation and completed plans
    ├── archive/        - Historical documents and superseded files
    └── master-plans/   - High-level project planning documents
```

## Workflow Rules

### 1. **New Documents** (Automatic)
- Any new implementation plan or draft documentation MUST be created in `docs/in-process/`
- Both agents will automatically place new plans here without user intervention

### 2. **Task Completion** (Automatic)
- When a phase, task, or plan reaches "Success" or "Verification Passed", it MUST be moved from `in-process/` to `complete/`.
- The agent MUST:
  1. Automatically move the finalized document from `in-process/` to `complete/`.
  2. Update any references in README.md or other documentation.
  3. Notify the user of the move in the commit message.
- **No user approval required** - this is automatic cleanup and mandatory organization.

### 3. **Archival** (Manual)
- When documents become outdated or superseded, move them to `archive/`
- Archive folder is for historical reference only

### 4. **Consistency**
- Both the local agent (me) and the cloud agent (Jules) must adhere to this folder structure
- All documentation references should use relative paths from project root

## Automation Triggers
- ✅ Test suite passes → Move plan to `complete/`
- ✅ Task marked as "Success" → Move plan to `complete/`
- ✅ New feature implementation starts → Create plan in `in-process/`
