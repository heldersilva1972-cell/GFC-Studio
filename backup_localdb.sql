-- Connect to (localdb)\MSSQLLocalDB
USE ClubMembership;
GO
DECLARE @BackupFile NVARCHAR(500) = 'C:\Users\Public\Documents\ClubMembership_Migration.bak';

BACKUP DATABASE ClubMembership 
TO DISK = @BackupFile 
WITH FORMAT, INIT, STATS = 10;
GO
