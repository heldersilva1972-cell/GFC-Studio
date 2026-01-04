@echo off
sqlcmd -S "(localdb)\MSSQLLocalDB" -d ClubMembership -i "docs\DatabaseScripts\INITIALIZE_DATABASE_FINAL.sql" > db_init_final.log 2>&1
echo Database initialization attempted. Check db_init_final.log for details.
type db_init_final.log
