sqlcmd -S .\SQLEXPRESS -d ClubMembership -Q "SELECT PageId, PageName, PageRoute FROM AppPages WHERE PageName LIKE '%Portal%'"
pause
