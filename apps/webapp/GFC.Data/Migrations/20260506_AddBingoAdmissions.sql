-- Migration: Add Bingo Admission Packs
-- Description: Creates tables for configurable admission packs and per-session admission tracking.

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoAdmissionDefinitions' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[BingoAdmissionDefinitions] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [GlobalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedAt] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(MAX) NULL,
        [ModifiedBy] NVARCHAR(MAX) NULL,
        [SyncDate] DATETIME2 NULL,
        [RowVersion] ROWVERSION NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [DisplayColor] NVARCHAR(20) NOT NULL DEFAULT '#3b82f6',
        [Price] DECIMAL(18,2) NOT NULL,
        [DisplayOrder] INT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_BingoAdmissionDefinitions] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- Ensure BingoSheetDefinitions has the new column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoSheetDefinitions]') AND name = 'IsIncludedInAdmission')
BEGIN
    ALTER TABLE [dbo].[BingoSheetDefinitions] ADD [IsIncludedInAdmission] BIT NOT NULL DEFAULT 1;
END

-- Clean up unused column if it was created in a previous run
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoAdmissionDefinitions]') AND name = 'IncludedSheetColors')
BEGIN
    -- Drop the default constraint first
    DECLARE @ConstraintName nvarchar(200)
    SELECT @ConstraintName = Name FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[BingoAdmissionDefinitions]')
    AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoAdmissionDefinitions]') AND name = 'IncludedSheetColors')
    
    IF @ConstraintName IS NOT NULL
        EXEC('ALTER TABLE [dbo].[BingoAdmissionDefinitions] DROP CONSTRAINT [' + @ConstraintName + ']')

    ALTER TABLE [dbo].[BingoAdmissionDefinitions] DROP COLUMN [IncludedSheetColors];
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoAdmissionEntries' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[BingoAdmissionEntries] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [GlobalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedAt] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(MAX) NULL,
        [ModifiedBy] NVARCHAR(MAX) NULL,
        [SyncDate] DATETIME2 NULL,
        [RowVersion] ROWVERSION NULL,
        [BingoSessionId] INT NOT NULL,
        [AdmissionDefinitionId] INT NOT NULL,
        [Quantity] INT NOT NULL DEFAULT 0,
        [PriceAtTime] DECIMAL(18,2) NOT NULL,
        CONSTRAINT [PK_BingoAdmissionEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_BingoAdmissionEntries_BingoSessions_BingoSessionId] FOREIGN KEY ([BingoSessionId]) REFERENCES [dbo].[BingoSessions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BingoAdmissionEntries_BingoAdmissionDefinitions_AdmissionDefinitionId] FOREIGN KEY ([AdmissionDefinitionId]) REFERENCES [dbo].[BingoAdmissionDefinitions] ([Id]) ON DELETE NO ACTION
    );
END


-- Seed default admission packs if none exist
IF NOT EXISTS (SELECT 1 FROM [dbo].[BingoAdmissionDefinitions])
BEGIN
    INSERT INTO [dbo].[BingoAdmissionDefinitions] ([Name], [DisplayColor], [Price], [DisplayOrder], [IsActive])
    VALUES ('Standard Pack 9', '#3b82f6', 20.00, 1, 1),
           ('Addition Pack 12', '#10b981', 23.00, 2, 1);
END
