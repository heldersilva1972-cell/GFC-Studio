-- =============================================
-- MASTER POS SYSTEM REPAIR SCRIPT
-- Purpose: Ensures all POS tables and columns exist for 
--          Categories, Items, Sales, Tokens, and Z-Reports.
-- =============================================

-- 1. POS CATEGORIES (With DisplayOrder)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosCategories')
BEGIN
    CREATE TABLE PosCategories (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1
    );
END
ELSE
BEGIN
    -- Ensure DisplayOrder exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PosCategories') AND name = 'DisplayOrder')
    BEGIN
        ALTER TABLE PosCategories ADD DisplayOrder INT NOT NULL DEFAULT 0;
    END
END

-- 2. LIQUOR ITEMS (Ensure POS Specific Columns)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'LiquorItems')
BEGIN
    -- Ensure ShowInPos exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LiquorItems') AND name = 'ShowInPos')
    BEGIN
        ALTER TABLE LiquorItems ADD ShowInPos BIT NOT NULL DEFAULT 0;
    END

    -- Ensure DisplayOrder exists (New for custom sorting)
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LiquorItems') AND name = 'DisplayOrder')
    BEGIN
        ALTER TABLE LiquorItems ADD DisplayOrder INT NOT NULL DEFAULT 0;
    END

    -- Ensure IsActive exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LiquorItems') AND name = 'IsActive')
    BEGIN
        ALTER TABLE LiquorItems ADD IsActive BIT NOT NULL DEFAULT 1;
    END
END

-- 3. POS SALES (Transaction History)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosSales')
BEGIN
    CREATE TABLE PosSales (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
        TerminalName NVARCHAR(100) NOT NULL,
        BartenderName NVARCHAR(100) NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL,
        PaymentType NVARCHAR(50) NOT NULL DEFAULT 'CASH',
        ItemsJson NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        IsSynced BIT NOT NULL DEFAULT 0
    );
END

-- 4. POS Z-REPORTS (Shift Closeout Audits)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosZReports')
BEGIN
    CREATE TABLE PosZReports (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
        TerminalName NVARCHAR(100) NOT NULL,
        BartenderName NVARCHAR(100) NOT NULL,
        CashTotal DECIMAL(18,2) NOT NULL DEFAULT 0,
        TotalGrossSales DECIMAL(18,2) NOT NULL DEFAULT 0,
        InventoryPullsJson NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        SalesSummaryJson NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        IsSynced BIT NOT NULL DEFAULT 0
    );
END
ELSE
BEGIN
    -- Ensure TotalGrossSales exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PosZReports') AND name = 'TotalGrossSales')
    BEGIN
        ALTER TABLE PosZReports ADD TotalGrossSales DECIMAL(18,2) NOT NULL DEFAULT 0;
    END

    -- Ensure SalesSummaryJson exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PosZReports') AND name = 'SalesSummaryJson')
    BEGIN
        ALTER TABLE PosZReports ADD SalesSummaryJson NVARCHAR(MAX) NOT NULL DEFAULT '[]';
    END
END

-- 5. POS TOKENS (Management)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosTokens')
BEGIN
    CREATE TABLE PosTokens (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(50) NOT NULL,
        SalePrice DECIMAL(18,2) NOT NULL,
        ColorHex NVARCHAR(20) NOT NULL DEFAULT '#3b82f6',
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        EligibleItemIds NVARCHAR(MAX) NOT NULL DEFAULT ''
    );
END

-- 6. SEED DEFAULT CATEGORIES (If empty)
IF NOT EXISTS (SELECT 1 FROM PosCategories)
BEGIN
    INSERT INTO PosCategories (Name, DisplayOrder) VALUES ('LIQUOR', 10);
    INSERT INTO PosCategories (Name, DisplayOrder) VALUES ('BEER', 20);
    INSERT INTO PosCategories (Name, DisplayOrder) VALUES ('FOOD', 30);
END

PRINT 'POS Master Repair Complete. All tables and columns are verified.';
