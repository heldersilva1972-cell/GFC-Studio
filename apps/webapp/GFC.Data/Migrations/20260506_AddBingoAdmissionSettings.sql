-- Migration: Add Bingo Admission Settings (V2 - Card Based)
-- Description: Adds base pricing and extra card rates to SystemSettings table.

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BingoBaseAdmissionPrice')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BingoBaseAdmissionPrice] DECIMAL(18,2) NOT NULL DEFAULT 15.00;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BingoAdditionalCardPrice')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BingoAdditionalCardPrice] DECIMAL(18,2) NOT NULL DEFAULT 3.00;
END
GO

-- Clean up the 'Square' version if it was added in a previous run
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BingoAdditionalSquarePrice')
BEGIN
    -- Drop the default constraint first
    DECLARE @ConstraintName nvarchar(200)
    SELECT @ConstraintName = Name FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[SystemSettings]')
    AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BingoAdditionalSquarePrice')
    
    IF @ConstraintName IS NOT NULL
        EXEC('ALTER TABLE [dbo].[SystemSettings] DROP CONSTRAINT [' + @ConstraintName + ']')

    ALTER TABLE [dbo].[SystemSettings] DROP COLUMN [BingoAdditionalSquarePrice];
END
GO

-- Update the main SystemSettings row (Id=1) if it exists
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BingoAdditionalCardPrice')
BEGIN
    EXEC('UPDATE [dbo].[SystemSettings] SET [BingoBaseAdmissionPrice] = 15.00, [BingoAdditionalCardPrice] = 3.00 WHERE [Id] = 1')
END
GO
