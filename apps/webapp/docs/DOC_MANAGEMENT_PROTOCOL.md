# Rule: Documentation Management Protocol

This rule must be followed by both the local agent (me) and the cloud agent (Jules).

## Directory Structure
- All project documentation and implementation plans must be stored in the `apps/webapp/docs/` directory.
- This directory must maintain two sub-folders:
    1. `in-process/`: For active plans, draft documentation, and ongoing tasks.
    2. `complete/`: For finalized documentation, archived plans, and completed technical reports.

## Workflow Rules
1. **New Documents**: Any new implementation plan or draft documentation MUST be created within the `docs/in-process/` folder initially.
2. **Task Completion**: When a plan reaches "Success" or a documentation task is finalized, the corresponding file(s) MUST be moved from `in-process/` to `complete/`.
3. **Consistency**: Both the local agent (me) and the cloud agent (Jules) must adhere to this folder structure to maintain project organization.
4. **References**: Documentation links in the `README.md` or roadmap files should be updated to reflect the current folder location of the file.
