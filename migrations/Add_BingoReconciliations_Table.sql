-- =============================================
-- Create BingoReconciliations Table
-- Database: ClubMembership
-- =============================================
USE ClubMembership;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoReconciliations' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.BingoReconciliations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION NULL,
        StatementDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        EndingBalance DECIMAL(18, 2) NOT NULL,
        ClearedTransactionIdsJson NVARCHAR(MAX) NOT NULL
    );
    PRINT 'BingoReconciliations table created successfully!';
END
ELSE
BEGIN
    PRINT 'BingoReconciliations table already exists.';
END
GO
