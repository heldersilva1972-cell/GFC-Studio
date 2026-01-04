@echo off
set LOGFILE=c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\db_log.txt
echo Running initialization... > "%LOGFILE%"
sqlcmd -S "(localdb)\MSSQLLocalDB" -d ClubMembership -i "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs\DatabaseScripts\INITIALIZE_DASHBOARD.sql" >> "%LOGFILE%" 2>&1
echo Done. >> "%LOGFILE%"
