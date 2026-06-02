-- ==============================================================================
-- Restore-BingoSession-Template.sql
-- SQL Template executed by Restore-BingoSession.ps1 using sqlcmd scripting.
-- ==============================================================================

USE master;
GO

PRINT 'Step 1: Restoring temporary database from backup...';
RESTORE DATABASE [$(TempDbName)]
FROM DISK = '$(BackupPath)'
WITH 
    MOVE 'ClubMembership' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\$(TempDbName).mdf',
    MOVE 'ClubMembership_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\$(TempDbName)_log.ldf',
    REPLACE;
GO

USE $(DatabaseName);
GO

PRINT 'Step 2: Copying May 27th data from backup...';
BEGIN TRANSACTION;

-- Enable IDENTITY_INSERT to preserve original database IDs
SET IDENTITY_INSERT BingoSessions ON;

-- A. Restore the Parent Session
INSERT INTO BingoSessions (
    Id, SessionDate, AdmissionCount, TotalGrossReceipts, TotalPrizesPaid, 
    TotalLotteryTake, TotalClubTake, RoundingAdjustment, Category, Status, 
    Notes, GlobalId, IsDeleted, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy, SyncDate
)
SELECT 
    Id, SessionDate, AdmissionCount, TotalGrossReceipts, TotalPrizesPaid, 
    TotalLotteryTake, TotalClubTake, RoundingAdjustment, Category, Status, 
    Notes, GlobalId, IsDeleted, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy, SyncDate
FROM $(TempDbName).dbo.BingoSessions
WHERE CAST(SessionDate AS DATE) = '$(TargetDate)';

SET IDENTITY_INSERT BingoSessions OFF;

-- B. Restore Child Game Entries
SET IDENTITY_INSERT BingoGameEntries ON;

INSERT INTO BingoGameEntries (
    Id, SessionId, SheetColor, GameName, SheetsSold, PricePerSheet, GrossReceipts, 
    PrizePaid, LotteryPercentage, ClubPercentage, LotteryTake, ClubTake, NetProceeds, 
    RoundingAdjustment, BallsCalled, Category, GlobalId, IsDeleted, CreatedAt, ModifiedAt, 
    CreatedBy, ModifiedBy, SyncDate
)
SELECT 
    g.Id, g.SessionId, g.SheetColor, g.GameName, g.SheetsSold, g.PricePerSheet, g.GrossReceipts, 
    g.PrizePaid, g.LotteryPercentage, g.ClubPercentage, g.LotteryTake, g.ClubTake, g.NetProceeds, 
    g.RoundingAdjustment, g.BallsCalled, g.Category, g.GlobalId, g.IsDeleted, g.CreatedAt, g.ModifiedAt, 
    g.CreatedBy, g.ModifiedBy, g.SyncDate
FROM $(TempDbName).dbo.BingoGameEntries g
INNER JOIN $(TempDbName).dbo.BingoSessions s ON g.SessionId = s.Id
WHERE CAST(s.SessionDate AS DATE) = '$(TargetDate)';

SET IDENTITY_INSERT BingoGameEntries OFF;

-- C. Restore Child Admission Entries
SET IDENTITY_INSERT BingoAdmissionEntries ON;

INSERT INTO BingoAdmissionEntries (
    Id, BingoSessionId, AdmissionDefinitionId, Quantity, PriceAtTime, CreatedAt, CreatedBy
)
SELECT 
    ae.Id, ae.BingoSessionId, ae.AdmissionDefinitionId, ae.Quantity, ae.PriceAtTime, ae.CreatedAt, ae.CreatedBy
FROM $(TempDbName).dbo.BingoAdmissionEntries ae
INNER JOIN $(TempDbName).dbo.BingoSessions s ON ae.BingoSessionId = s.Id
WHERE CAST(s.SessionDate AS DATE) = '$(TargetDate)';

SET IDENTITY_INSERT BingoAdmissionEntries OFF;

-- D. Restore Financial Audit Ledgers
SET IDENTITY_INSERT BingoLotteryTransactions ON;

INSERT INTO BingoLotteryTransactions (
    Id, Date, Type, Amount, Description, CategoryId, SessionId, GlobalId, IsDeleted, 
    CreatedAt, ModifiedAt, CreatedBy, ModifiedBy, SyncDate, CheckNumber
)
SELECT 
    t.Id, t.Date, t.Type, t.Amount, t.Description, t.CategoryId, t.SessionId, t.GlobalId, t.IsDeleted, 
    t.CreatedAt, t.ModifiedAt, t.CreatedBy, t.ModifiedBy, t.SyncDate, t.CheckNumber
FROM $(TempDbName).dbo.BingoLotteryTransactions t
INNER JOIN $(TempDbName).dbo.BingoSessions s ON t.SessionId = s.Id
WHERE CAST(s.SessionDate AS DATE) = '$(TargetDate)';

SET IDENTITY_INSERT BingoLotteryTransactions OFF;

COMMIT TRANSACTION;
PRINT 'Step 3: May 27th Data Restored Successfully!';
GO

-- 3. Clean up by dropping the temporary database
USE master;
GO
PRINT 'Step 4: Cleaning up and dropping temporary database...';
DROP DATABASE [$(TempDbName)];
PRINT 'Restore complete!';
GO
