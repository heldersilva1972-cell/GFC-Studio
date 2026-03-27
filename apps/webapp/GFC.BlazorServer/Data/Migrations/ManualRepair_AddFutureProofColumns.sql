/*
  GFC GOLD STANDARD DATABASE PATCH (CORE TABLES)
  --------------------------------------------------
  Safely adds future-proofing columns:
  - GlobalId (UUID fingerprint)
  - IsDeleted (Soft-delete support)
  - CreatedAt/ModifiedAt (Audit timestamps)
  - CreatedBy/ModifiedBy (Audit authorship)
  - SyncDate (Tablet sync readiness)
  - RowVersion (Concurrency safety lock)

  Note: Wrap in OBJECT_ID checks to skip missing tables or columns.
*/

-- 1. Members
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Members')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'GlobalId')
    BEGIN
        ALTER TABLE Members ADD 
        GlobalId UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
        IsDeleted BIT DEFAULT 0 NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION NOT NULL;
    END
END

-- 2. BarSaleEntries
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BarSaleEntries')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BarSaleEntries') AND name = 'GlobalId')
    BEGIN
        ALTER TABLE BarSaleEntries ADD 
        GlobalId UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
        IsDeleted BIT DEFAULT 0 NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION NOT NULL;
    END
END

-- 3. DuesPayments
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DuesPayments')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DuesPayments') AND name = 'GlobalId')
    BEGIN
        ALTER TABLE DuesPayments ADD 
        GlobalId UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
        IsDeleted BIT DEFAULT 0 NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION NOT NULL;
    END
END

-- 4. ShiftReports
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ShiftReports')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ShiftReports') AND name = 'GlobalId')
    BEGIN
        ALTER TABLE ShiftReports ADD 
        GlobalId UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
        IsDeleted BIT DEFAULT 0 NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION NOT NULL;
    END
END

-- 5. StaffShifts (Alternate name for ShiftReports in some DB versions)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StaffShifts')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StaffShifts') AND name = 'GlobalId')
    BEGIN
        ALTER TABLE StaffShifts ADD 
        GlobalId UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
        IsDeleted BIT DEFAULT 0 NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION NOT NULL;
    END
END
GO
