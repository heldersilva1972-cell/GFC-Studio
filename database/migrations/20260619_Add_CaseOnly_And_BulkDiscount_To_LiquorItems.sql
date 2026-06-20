USE [ClubMembership]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'OrderByCaseOnly'
)
BEGIN
    PRINT 'Adding OrderByCaseOnly column to dbo.LiquorItems...'
    ALTER TABLE [dbo].[LiquorItems]
    ADD [OrderByCaseOnly] BIT NOT NULL DEFAULT 0;
    PRINT 'OrderByCaseOnly column added successfully!'
END
ELSE
BEGIN
    PRINT 'OrderByCaseOnly column already exists in dbo.LiquorItems.'
END
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'BulkDiscountThreshold'
)
BEGIN
    PRINT 'Adding BulkDiscountThreshold column to dbo.LiquorItems...'
    ALTER TABLE [dbo].[LiquorItems]
    ADD [BulkDiscountThreshold] INT NULL;
    PRINT 'BulkDiscountThreshold column added successfully!'
END
ELSE
BEGIN
    PRINT 'BulkDiscountThreshold column already exists in dbo.LiquorItems.'
END
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'BulkDiscountPrice'
)
BEGIN
    PRINT 'Adding BulkDiscountPrice column to dbo.LiquorItems...'
    ALTER TABLE [dbo].[LiquorItems]
    ADD [BulkDiscountPrice] DECIMAL(18,2) NULL;
    PRINT 'BulkDiscountPrice column added successfully!'
END
ELSE
BEGIN
    PRINT 'BulkDiscountPrice column already exists in dbo.LiquorItems.'
END
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'MinOrderCases'
)
BEGIN
    PRINT 'Adding MinOrderCases column to dbo.LiquorItems...'
    ALTER TABLE [dbo].[LiquorItems]
    ADD [MinOrderCases] INT NULL;
    PRINT 'MinOrderCases column added successfully!'
END
ELSE
BEGIN
    PRINT 'MinOrderCases column already exists in dbo.LiquorItems.'
END
GO
