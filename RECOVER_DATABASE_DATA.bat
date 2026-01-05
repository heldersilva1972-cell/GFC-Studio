@echo off
set "localdb_instance=(localdb)\MSSQLLocalDB"
set "db_name=ClubMembership"

echo ============================================================
echo GFC Master Database Recovery and Seeding
echo ============================================================
echo Connecting to: %localdb_instance%
echo Target Database: %db_name%
echo.

echo [1/10] Building Foundation...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\CREATE_IDENTITY_SCHEMA.sql"
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\INITIALIZE_DATABASE_COMPLETE.sql"

echo [2/10] Seeding Admin...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\CREATE_ADMIN_USER.sql"

echo [3/10] Setting up KeyCards...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\CREATE_KEYCARDS_TABLE.sql"

echo [4/10] Applying Dashboard Initialization...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\INITIALIZE_DASHBOARD.sql"

echo [5/10] Applying Staff Management Migration (Bartenders)...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\Database_Migration_StaffManagement.sql"

echo [6/10] Applying Studio Schema Fixes...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\MASTER_STUDIO_SCHEMA_FIX.sql"

echo [7/10] Seeding Demo Data (Members, Payments, Bar Sales)...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "docs\DatabaseScripts\SEED_DEMO_DATA.sql"

echo [8/10] Applying Controller Sync Schema...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "SyncControllerDatabase.sql"

echo [9/10] Applying Phase 3 Agent Secure Channel Schema...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "database\migrations\Phase3_Agent_Secure_Channel.sql"

echo [10/10] Final Performance Fixes...
sqlcmd -S "%localdb_instance%" -d %db_name% -i "database\migrations\FIX_CARD_LOADING_PERFORMANCE.sql"

echo.
echo ============================================================
echo Recovery and Seeding Complete!
echo ============================================================
pause
