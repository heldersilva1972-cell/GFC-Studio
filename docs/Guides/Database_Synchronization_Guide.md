# Database Synchronization Guide
## How to Clone Host Database to Laptop

To make your laptop database EXACTLY like the host, you need to perform a **Backup and Restore**. This process takes a snapshot of the live database and overwrites your local development database with it.

### Prerequisites
*   **SQL Server Management Studio (SSMS)** installed on both machines.
*   Access to both the Host (Server) and Laptop.
*   A way to transfer a file (USB drive, Network Share, OneDrive, etc.).

---

### Step 1: Backup on Host Computer
Run this SQL script on the **Host Computer** (Production) to create a backup file.

1.  Open SSMS and connect to the Host instance (usually `.\SQLEXPRESS`).
2.  Open a **New Query**.
3.  Run the following T-SQL:

```sql
-- 1. Configure Backup Path (Update this path if needed)
DECLARE @BackupPath NVARCHAR(255) = N'C:\Backups\ClubMembership_Sync.bak';

-- Ensure directory exists manually if needed, or save to a known folder like Downloads
-- For this example, we'll assume C:\Backups exists or use a standard default:
-- SET @BackupPath = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup\ClubMembership_Sync.bak';

BACKUP DATABASE [ClubMembership] 
TO DISK = @BackupPath 
WITH FORMAT, 
     MEDIANAME = 'ClubMembership_Sync', 
     NAME = 'Full Backup of ClubMembership';

PRINT 'Backup completed successfully to: ' + @BackupPath;
```

*Alternatively, use the UI: Right-click `ClubMembership` > Tasks > Backup.*

### Step 2: Transfer the File
Copy the `.bak` file created in Step 1 to your **Laptop**.
*   Example Destination: `C:\Users\hnsil\Documents\ClubMembership_Sync.bak`

---

### Step 3: Restore on Laptop (TARGET)
Run this script on your **Laptop** to overwrite your local database with the production copy.

**WARNING: This will delete correctly local data and replace it with the host's data.**

1.  Open SSMS on your Laptop (`.\SQLEXPRESS`).
2.  Open a **New Query**.
3.  Run this T-SQL (Update the path to where you saved the file):

```sql
USE [master];
GO

-- 1. Force Entry (Kick off other users/apps to allow restore)
ALTER DATABASE [ClubMembership] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- 2. Restore Database
-- Update this path to match where you put the file on your laptop
RESTORE DATABASE [ClubMembership] 
FROM DISK = N'C:\Users\hnsil\Documents\ClubMembership_Sync.bak' 
WITH REPLACE, -- Overwrite existing database
     RECOVERY; -- Bring database online
GO

-- 3. Reset Access
ALTER DATABASE [ClubMembership] SET MULTI_USER;
GO

PRINT 'Database restored successfully!';
```

### Sync Complete
Your laptop database is now an exact byte-for-byte copy of the host database at the time of backup.
