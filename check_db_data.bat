@echo off
sqlcmd -S .\SQLEXPRESS -d ClubMembership -Q "SELECT TOP 1 * FROM SystemSettings" > db_data.txt 2>&1
sqlcmd -S .\SQLEXPRESS -d ClubMembership -Q "SELECT TOP 5 * FROM Controllers" >> db_data.txt 2>&1
sqlcmd -S .\SQLEXPRESS -d ClubMembership -Q "SELECT TOP 5 * FROM ControllerEvents ORDER BY Id DESC" >> db_data.txt 2>&1
