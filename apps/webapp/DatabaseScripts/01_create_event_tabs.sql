-- CREATE EVENT TABS TABLES
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[EventTemplates] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] VARCHAR(100) NOT NULL,
        [DefaultType] INT NOT NULL,
        
        -- BaseEntity fields
        [GlobalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedAt] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(MAX) NULL,
        [ModifiedBy] NVARCHAR(MAX) NULL,
        [SyncDate] DATETIME2 NULL,
        [RowVersion] ROWVERSION
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ActiveEvents] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [TemplateId] INT NULL,
        [Name] VARCHAR(100) NOT NULL,
        [Type] INT NOT NULL,
        [InitialAmount] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [CurrentBalance] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Status] INT NOT NULL DEFAULT 0,
        
        -- BaseEntity fields
        [GlobalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedAt] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(MAX) NULL,
        [ModifiedBy] NVARCHAR(MAX) NULL,
        [SyncDate] DATETIME2 NULL,
        [RowVersion] ROWVERSION,
        
        CONSTRAINT [FK_ActiveEvents_EventTemplates] FOREIGN KEY ([TemplateId]) REFERENCES [dbo].[EventTemplates]([Id])
    );
END
GO

-- ADD COLUMN TO POS SALES
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[PosSales]') 
    AND name = 'ActiveEventId'
)
BEGIN
    ALTER TABLE [dbo].[PosSales] ADD [ActiveEventId] INT NULL;
    
    ALTER TABLE [dbo].[PosSales] ADD CONSTRAINT [FK_PosSales_ActiveEvents] FOREIGN KEY ([ActiveEventId]) REFERENCES [dbo].[ActiveEvents]([Id]);
END
GO
