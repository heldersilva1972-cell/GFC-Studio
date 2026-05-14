-- =============================================
-- BINGO SYSTEM REPAIR SCRIPT
-- Purpose: Ensures all Bingo tables and columns exist for 
--          Sessions, Game Entries, and Admissions.
-- =============================================
USE ClubMembership;

-- 1. BINGO SESSIONS
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
        RoundingAdjustment DECIMAL(18, 2) NOT NULL DEFAULT 0,
        Category NVARCHAR(100) NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Draft',
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(255) NULL
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSessions') AND name = 'RoundingAdjustment')
        ALTER TABLE BingoSessions ADD RoundingAdjustment DECIMAL(18, 2) NOT NULL DEFAULT 0;
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSessions') AND name = 'Category')
        ALTER TABLE BingoSessions ADD Category NVARCHAR(100) NULL;
END

-- 2. BINGO GAME ENTRIES
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
        RoundingAdjustment DECIMAL(18, 2) NOT NULL DEFAULT 0,
        BallsCalled INT NOT NULL DEFAULT 0,
        Category NVARCHAR(100) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(255) NULL,
        CONSTRAINT FK_BingoGameEntries_BingoSessions FOREIGN KEY (SessionId) REFERENCES BingoSessions(Id) ON DELETE CASCADE
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'RoundingAdjustment')
        ALTER TABLE BingoGameEntries ADD RoundingAdjustment DECIMAL(18, 2) NOT NULL DEFAULT 0;
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'BallsCalled')
        ALTER TABLE BingoGameEntries ADD BallsCalled INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'Category')
        ALTER TABLE BingoGameEntries ADD Category NVARCHAR(100) NULL;

    -- Standardize names if they were created differently
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'ClubPercent')
    AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'ClubPercentage')
    BEGIN
        EXEC sp_rename 'BingoGameEntries.ClubPercent', 'ClubPercentage', 'COLUMN';
    END
    
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'LotteryPercent')
    AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoGameEntries') AND name = 'LotteryPercentage')
    BEGIN
        EXEC sp_rename 'BingoGameEntries.LotteryPercent', 'LotteryPercentage', 'COLUMN';
    END
END

-- 3. BINGO ADMISSION DEFINITIONS
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoAdmissionDefinitions')
BEGIN
    CREATE TABLE BingoAdmissionDefinitions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        DisplayColor NVARCHAR(20) NOT NULL DEFAULT '#3b82f6',
        Price DECIMAL(18,2) NOT NULL,
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(255) NULL
    );
END

-- 4. BINGO ADMISSION ENTRIES
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoAdmissionEntries')
BEGIN
    CREATE TABLE BingoAdmissionEntries (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BingoSessionId INT NOT NULL,
        AdmissionDefinitionId INT NOT NULL,
        Quantity INT NOT NULL DEFAULT 0,
        PriceAtTime DECIMAL(18,2) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(255) NULL,
        CONSTRAINT FK_BingoAdmissionEntries_BingoSessions FOREIGN KEY (BingoSessionId) REFERENCES BingoSessions(Id) ON DELETE CASCADE,
        CONSTRAINT FK_BingoAdmissionEntries_Definitions FOREIGN KEY (AdmissionDefinitionId) REFERENCES BingoAdmissionDefinitions(Id)
    );
END

PRINT 'Bingo System Repair Complete. All tables and columns are verified.';
