@echo off
echo Current Branch: > git_debug.log
git rev-parse --abbrev-ref HEAD >> git_debug.log
echo. >> git_debug.log
echo Git Status: >> git_debug.log
git status >> git_debug.log
echo. >> git_debug.log
echo Unmerged Files (Conflicts): >> git_debug.log
git diff --name-only --diff-filter=U >> git_debug.log
echo. >> git_debug.log
echo List Files in Conflict State: >> git_debug.log
git ls-files -u >> git_debug.log
