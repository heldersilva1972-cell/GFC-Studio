-- =============================================
-- Migration: CreateClubEventsFinancials.sql
-- Description: Creates ClubEvents and ClubEventTransactions tables
--              and registers the page in AppPages.
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ClubEvents')
BEGIN
    CREATE TABLE dbo.ClubEvents (
        Id            INT           IDENTITY(1,1) NOT NULL,
        GlobalId      UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_ClubEvents_GlobalId DEFAULT NEWID(),
        EventName     NVARCHAR(255) NOT NULL,
        EventGroup    NVARCHAR(100) NOT NULL,
        EventDate     DATETIME2     NOT NULL,
        IsArchived    BIT           NOT NULL CONSTRAINT DF_ClubEvents_IsArchived DEFAULT 0,
        InitialBudget DECIMAL(18,2) NULL,
        Note          NVARCHAR(MAX) NULL,
        IsDeleted     BIT           NOT NULL CONSTRAINT DF_ClubEvents_IsDeleted DEFAULT 0,
        CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_ClubEvents_CreatedAt DEFAULT GETUTCDATE(),
        ModifiedAt    DATETIME2     NULL,
        CreatedBy     NVARCHAR(255) NULL,
        ModifiedBy    NVARCHAR(255) NULL,
        SyncDate      DATETIME2     NULL,
        RowVersion    ROWVERSION    NOT NULL,
        CONSTRAINT PK_ClubEvents PRIMARY KEY (Id)
    );
    CREATE INDEX IX_ClubEvents_GlobalId ON dbo.ClubEvents (GlobalId);
    PRINT 'Created: ClubEvents';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ClubEventTransactions')
BEGIN
    CREATE TABLE dbo.ClubEventTransactions (
        Id              INT           IDENTITY(1,1) NOT NULL,
        GlobalId        UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_ClubEventTx_GlobalId DEFAULT NEWID(),
        EventId         INT           NOT NULL,
        TransactionDate DATETIME2     NOT NULL,
        Description     NVARCHAR(500) NOT NULL,
        Category        NVARCHAR(100) NULL,
        Amount          DECIMAL(18,2) NOT NULL,
        Note            NVARCHAR(MAX) NULL,
        IsDeleted       BIT           NOT NULL CONSTRAINT DF_ClubEventTx_IsDeleted DEFAULT 0,
        CreatedAt       DATETIME2     NOT NULL CONSTRAINT DF_ClubEventTx_CreatedAt DEFAULT GETUTCDATE(),
        ModifiedAt      DATETIME2     NULL,
        CreatedBy       NVARCHAR(255) NULL,
        ModifiedBy      NVARCHAR(255) NULL,
        SyncDate        DATETIME2     NULL,
        RowVersion      ROWVERSION    NOT NULL,
        CONSTRAINT PK_ClubEventTransactions PRIMARY KEY (Id),
        CONSTRAINT FK_ClubEventTx_Events FOREIGN KEY (EventId)
            REFERENCES dbo.ClubEvents (Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_ClubEventTx_EventId ON dbo.ClubEventTransactions (EventId);
    CREATE INDEX IX_ClubEventTx_GlobalId ON dbo.ClubEventTransactions (GlobalId);
    PRINT 'Created: ClubEventTransactions';
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.AppPages WHERE PageRoute = '/finance/club-events')
BEGIN
    INSERT INTO dbo.AppPages (PageName, PageRoute, Category, Description, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Club Events Financials', '/finance/club-events', 'FINANCE', 'Track income and expenses per recurring event.', 1, 1, 40);
    PRINT 'Registered: AppPages /finance/club-events';
END
GO
