-- ============================================================
-- Migration: Add FinancePaymentTypes table + FK on FinanceVendors
-- Run this script ONCE against the GFC production/development database
-- ============================================================

-- 1. Create the FinancePaymentTypes table (if it doesn't already exist)
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'FinancePaymentTypes'
)
BEGIN
    CREATE TABLE [dbo].[FinancePaymentTypes] (
        [Id]        INT            IDENTITY(1,1) NOT NULL,
        [Name]      NVARCHAR(100)  NOT NULL,
        [IsActive]  BIT            NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_FinancePaymentTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Unique index on Name to prevent duplicates
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FinancePaymentTypes_Name]
        ON [dbo].[FinancePaymentTypes] ([Name] ASC);

    PRINT 'Created table FinancePaymentTypes';
END
ELSE
BEGIN
    PRINT 'Table FinancePaymentTypes already exists – skipped creation';
END
GO

-- 2. Seed common payment types (skip if already seeded)
IF NOT EXISTS (SELECT 1 FROM [dbo].[FinancePaymentTypes])
BEGIN
    INSERT INTO [dbo].[FinancePaymentTypes] ([Name], [IsActive], [CreatedAt]) VALUES
        ('Check',           1, GETDATE()),
        ('ACH / Bank Transfer', 1, GETDATE()),
        ('Credit Card',     1, GETDATE()),
        ('Cash',            1, GETDATE()),
        ('Wire Transfer',   1, GETDATE()),
        ('Online (Bill Pay)', 1, GETDATE()),
        ('Debit Card',      1, GETDATE());

    PRINT 'Seeded default payment types';
END
ELSE
BEGIN
    PRINT 'FinancePaymentTypes already has data – skipped seeding';
END
GO

-- 3. Add DefaultPaymentTypeId column to FinanceVendors (if not already present)
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo'
      AND TABLE_NAME   = 'FinanceVendors'
      AND COLUMN_NAME  = 'DefaultPaymentTypeId'
)
BEGIN
    ALTER TABLE [dbo].[FinanceVendors]
        ADD [DefaultPaymentTypeId] INT NULL;

    PRINT 'Added column DefaultPaymentTypeId to FinanceVendors';
END
ELSE
BEGIN
    PRINT 'Column DefaultPaymentTypeId already exists on FinanceVendors – skipped';
END
GO

-- 4. Add the foreign key constraint (if not already present)
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
    WHERE CONSTRAINT_NAME = 'FK_FinanceVendors_FinancePaymentTypes'
)
BEGIN
    ALTER TABLE [dbo].[FinanceVendors]
        ADD CONSTRAINT [FK_FinanceVendors_FinancePaymentTypes]
        FOREIGN KEY ([DefaultPaymentTypeId])
        REFERENCES [dbo].[FinancePaymentTypes] ([Id])
        ON DELETE SET NULL;

    PRINT 'Added FK_FinanceVendors_FinancePaymentTypes';
END
ELSE
BEGIN
    PRINT 'Foreign key FK_FinanceVendors_FinancePaymentTypes already exists – skipped';
END
GO

PRINT '=== Migration complete ===';
GO
