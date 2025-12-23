@echo off
REM Script to organize GFC documentation according to DOC_MANAGEMENT_PROTOCOL
REM Run this from the project root directory

echo ========================================
echo GFC Documentation Organization Script
echo ========================================
echo.

REM Move active task documents to in-process folder
echo Moving active tasks to docs/in-process/...
move "docs\JULES_TASK_PHASE_6A_3_MISSION_CONTROL.md" "docs\in-process\JULES_TASK_PHASE_6A_3_MISSION_CONTROL.md"
move "docs\JULES_TASK_PHASE_6A_4_STAFF_SHIFTS_UX.md" "docs\in-process\JULES_TASK_PHASE_6A_4_STAFF_SHIFTS_UX.md"

echo.
echo Moving completed tasks to docs/complete/...
REM Move completed task documents to complete folder
move "docs\JULES_TASK_PHASE_6_GFC_MERGER.md" "docs\complete\JULES_TASK_PHASE_6_GFC_MERGER.md"
move "docs\PHASE_6_CAMERA_REVAMP.md" "docs\complete\PHASE_6_CAMERA_REVAMP.md"

echo.
echo ========================================
echo Documentation organization complete!
echo ========================================
echo.
echo Active tasks are now in: docs/in-process/
echo Completed tasks are now in: docs/complete/
echo.
pause
