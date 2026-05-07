-- Description: Add Bingo Tracking and Setup tables
-- Author: Antigravity

-- 1. SETUP TABLES
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoSheetDefinitions')
BEGIN
    CREATE TABLE BingoSheetDefinitions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ColorName NVARCHAR(100) NOT NULL,
        DisplayColor NVARCHAR(20) NOT NULL DEFAULT '#3b82f6',
        DefaultPrice DECIMAL(18, 2) NOT NULL DEFAULT 0,
        LotteryPercentage DECIMAL(18, 4) NOT NULL DEFAULT 0.05,
        ClubPercentage DECIMAL(18, 4) NOT NULL DEFAULT 0.05,
        IsFiftyFifty BIT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        DisplayOrder INT NOT NULL DEFAULT 0,
        
        -- BaseEntity fields
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION
    );
    PRINT 'Created table BingoSheetDefinitions.';
END
GO

-- Ensure new columns exist if table was already there
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoSheetDefinitions')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSheetDefinitions') AND name = 'LotteryPercentage')
    BEGIN
        ALTER TABLE BingoSheetDefinitions ADD LotteryPercentage DECIMAL(18, 4) NOT NULL DEFAULT 0.05;
        PRINT 'Added LotteryPercentage to BingoSheetDefinitions.';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSheetDefinitions') AND name = 'ClubPercentage')
    BEGIN
        ALTER TABLE BingoSheetDefinitions ADD ClubPercentage DECIMAL(18, 4) NOT NULL DEFAULT 0.05;
        PRINT 'Added ClubPercentage to BingoSheetDefinitions.';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSheetDefinitions') AND name = 'DisplayOrder')
    BEGIN
        ALTER TABLE BingoSheetDefinitions ADD DisplayOrder INT NOT NULL DEFAULT 0;
        PRINT 'Added DisplayOrder to BingoSheetDefinitions.';
    END
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoGameDefinitions')
BEGIN
    CREATE TABLE BingoGameDefinitions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SheetDefinitionId INT NOT NULL,
        GameName NVARCHAR(255) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        DisplayOrder INT NOT NULL DEFAULT 0,

        -- BaseEntity fields
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION,

        CONSTRAINT FK_BingoGameDefinitions_BingoSheetDefinitions FOREIGN KEY (SheetDefinitionId) REFERENCES BingoSheetDefinitions(Id) ON DELETE CASCADE
    );
    PRINT 'Created table BingoGameDefinitions.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameDefinitions') AND name = 'DefaultPayout')
    BEGIN
        ALTER TABLE BingoGameDefinitions ADD DefaultPayout DECIMAL(18, 2) NOT NULL DEFAULT 0;
        PRINT 'Added DefaultPayout to BingoGameDefinitions.';
    END
END
GO

-- 2. TRACKING TABLES
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoSessions')
BEGIN
    CREATE TABLE BingoSessions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SessionDate DATETIME2 NOT NULL,
        AdmissionCount INT NOT NULL DEFAULT 0,
        TotalGrossReceipts DECIMAL(18, 2) NOT NULL DEFAULT 0,
        TotalPrizesPaid DECIMAL(18, 2) NOT NULL DEFAULT 0,
        TotalLotteryTake DECIMAL(18, 2) NOT NULL DEFAULT 0,
        TotalClubTake DECIMAL(18, 2) NOT NULL DEFAULT 0,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Draft',
        Notes NVARCHAR(MAX) NULL,
        
        -- BaseEntity fields
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION
    );
    PRINT 'Created table BingoSessions.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoGameEntries')
BEGIN
    CREATE TABLE BingoGameEntries (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SessionId INT NOT NULL,
        SheetColor NVARCHAR(100) NOT NULL,
        GameName NVARCHAR(255) NOT NULL,
        SheetsSold INT NOT NULL DEFAULT 0,
        PricePerSheet DECIMAL(18, 2) NOT NULL DEFAULT 0,
        GrossReceipts DECIMAL(18, 2) NOT NULL DEFAULT 0,
        PrizePaid DECIMAL(18, 2) NOT NULL DEFAULT 0,
        LotteryPercent DECIMAL(18, 4) NOT NULL DEFAULT 0,
        ClubPercent DECIMAL(18, 4) NOT NULL DEFAULT 0,
        LotteryTake DECIMAL(18, 2) NOT NULL DEFAULT 0,
        ClubTake DECIMAL(18, 2) NOT NULL DEFAULT 0,
        NetProceeds DECIMAL(18, 2) NOT NULL DEFAULT 0,

        -- BaseEntity fields
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(255) NULL,
        ModifiedBy NVARCHAR(255) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION,

        CONSTRAINT FK_BingoGameEntries_BingoSessions FOREIGN KEY (SessionId) REFERENCES BingoSessions(Id) ON DELETE CASCADE
    );
    PRINT 'Created table BingoGameEntries.';
END
GO

-- 3. PERMISSIONS
IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/mobile/bingo')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Bingo Tracking (Mobile)', '/mobile/bingo', 'Offline-first tracking for Massachusetts Bingo sessions', 'MOBILE HUB', 0, 1, 150);
END

IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/admin/bingo')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Bingo Reports (Web)', '/admin/bingo', 'Financial reports and analytics for Bingo sessions', 'FINANCE', 0, 1, 160);
END

IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/admin/bingo/setup')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Bingo Setup', '/admin/bingo/setup', 'Configure sheet colors, prices, and games', 'FINANCE', 0, 1, 161);
END
GO

-- 4. SEED DATA
IF NOT EXISTS (SELECT * FROM BingoSheetDefinitions)
BEGIN
    -- Blue Sheet
    INSERT INTO BingoSheetDefinitions (ColorName, DisplayColor, DefaultPrice, IsFiftyFifty, DisplayOrder, LotteryPercentage, ClubPercentage)
    VALUES ('Blue', '#3b82f6', 5.00, 0, 1, 0.05, 0.05);
    DECLARE @BlueId INT = SCOPE_IDENTITY();
    INSERT INTO BingoGameDefinitions (SheetDefinitionId, GameName, DisplayOrder) VALUES (@BlueId, 'Any Bingo', 1), (@BlueId, 'Large Crazy Kite', 2);

    -- Orange Sheet
    INSERT INTO BingoSheetDefinitions (ColorName, DisplayColor, DefaultPrice, IsFiftyFifty, DisplayOrder, LotteryPercentage, ClubPercentage)
    VALUES ('Orange', '#f97316', 5.00, 0, 2, 0.05, 0.05);
    DECLARE @OrangeId INT = SCOPE_IDENTITY();
    INSERT INTO BingoGameDefinitions (SheetDefinitionId, GameName, DisplayOrder) VALUES (@OrangeId, 'Double Bingo', 1);

    -- Green Sheet
    INSERT INTO BingoSheetDefinitions (ColorName, DisplayColor, DefaultPrice, IsFiftyFifty, DisplayOrder, LotteryPercentage, ClubPercentage)
    VALUES ('Green', '#10b981', 5.00, 0, 3, 0.05, 0.05);
    DECLARE @GreenId INT = SCOPE_IDENTITY();
    INSERT INTO BingoGameDefinitions (SheetDefinitionId, GameName, DisplayOrder) VALUES (@GreenId, 'Letter X', 1);

    -- Yellow (50/50)
    INSERT INTO BingoSheetDefinitions (ColorName, DisplayColor, DefaultPrice, IsFiftyFifty, DisplayOrder, LotteryPercentage, ClubPercentage)
    VALUES ('Yellow', '#eab308', 2.00, 1, 4, 0.05, 0.50);
    DECLARE @YellowId INT = SCOPE_IDENTITY();
    INSERT INTO BingoGameDefinitions (SheetDefinitionId, GameName, DisplayOrder) VALUES (@YellowId, 'Coverall (50/50)', 1);

    PRINT 'Seeded default Bingo program.';
END
GO
