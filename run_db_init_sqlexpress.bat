@echo off
echo Attempting to initialize ClubMembership database on SQLEXPRESS...
sqlcmd -S ".\SQLEXPRESS" -E -Q "IF DB_ID('ClubMembership') IS NULL CREATE DATABASE ClubMembership"
sqlcmd -S ".\SQLEXPRESS" -E -d ClubMembership -i "docs\DatabaseScripts\INITIALIZE_DATABASE_FINAL.sql"
echo.
echo Attempting to grant permissions to common local users...
sqlcmd -S ".\SQLEXPRESS" -E -Q "IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'BUILTIN\Users') CREATE USER [BUILTIN\Users] FOR LOGIN [BUILTIN\Users]; ALTER ROLE db_owner ADD MEMBER [BUILTIN\Users];" -d ClubMembership
echo Done.
