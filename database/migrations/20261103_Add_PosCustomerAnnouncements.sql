USE [ClubMembership];
GO

-- 1. If table does not exist, create it with all BaseEntity columns
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosCustomerAnnouncements')
BEGIN
    CREATE TABLE dbo.PosCustomerAnnouncements (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        GlobalId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_GlobalId DEFAULT (NEWID()),
        Title NVARCHAR(150) NOT NULL,
        Message NVARCHAR(500) NOT NULL,
        Theme NVARCHAR(50) NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_Theme DEFAULT ('Bakery'),
        IconOrImageUrl NVARCHAR(500) NULL,
        BadgeText NVARCHAR(100) NULL,
        StartDate DATETIME2(7) NULL,
        EndDate DATETIME2(7) NULL,
        DisplayOrder INT NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_DisplayOrder DEFAULT (0),
        IsActive BIT NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_IsActive DEFAULT (1),
        IsDeleted BIT NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_IsDeleted DEFAULT (0),
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_CreatedAt DEFAULT (SYSUTCDATETIME()),
        ModifiedAt DATETIME2(7) NULL,
        CreatedBy NVARCHAR(100) NULL,
        ModifiedBy NVARCHAR(100) NULL,
        SyncDate DATETIME2(7) NULL,
        RowVersion ROWVERSION
    );

    CREATE NONCLUSTERED INDEX IX_PosCustomerAnnouncements_Active_Schedule 
    ON dbo.PosCustomerAnnouncements (IsActive, IsDeleted, DisplayOrder)
    INCLUDE (Title, Message, Theme, StartDate, EndDate);

    PRINT 'Table PosCustomerAnnouncements created successfully.';
END
ELSE
BEGIN
    -- 2. If table was already created with older script, add the missing BaseEntity columns safely
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosCustomerAnnouncements') AND name = 'GlobalId')
        ALTER TABLE dbo.PosCustomerAnnouncements ADD GlobalId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_PosCustomerAnnouncements_GlobalId DEFAULT (NEWID());

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosCustomerAnnouncements') AND name = 'ModifiedAt')
        ALTER TABLE dbo.PosCustomerAnnouncements ADD ModifiedAt DATETIME2(7) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosCustomerAnnouncements') AND name = 'CreatedBy')
        ALTER TABLE dbo.PosCustomerAnnouncements ADD CreatedBy NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosCustomerAnnouncements') AND name = 'ModifiedBy')
        ALTER TABLE dbo.PosCustomerAnnouncements ADD ModifiedBy NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosCustomerAnnouncements') AND name = 'SyncDate')
        ALTER TABLE dbo.PosCustomerAnnouncements ADD SyncDate DATETIME2(7) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosCustomerAnnouncements') AND name = 'RowVersion')
        ALTER TABLE dbo.PosCustomerAnnouncements ADD RowVersion ROWVERSION;

    PRINT 'Missing BaseEntity columns added to PosCustomerAnnouncements successfully.';
END
GO
