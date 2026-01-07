@echo off
echo TEST SCRIPT STARTING...
echo Current Dir: %CD%
pause
echo Attempting CD /D to: "%~dp0"
cd /d "%~dp0"
echo New Dir: %CD%
pause
echo SUCCESS.
pause
