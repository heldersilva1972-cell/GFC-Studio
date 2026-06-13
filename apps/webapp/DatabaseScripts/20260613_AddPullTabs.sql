-- Migration script to create Pull Tab tracking tables and seed initial data

-- 1. Create PullTabGameDefinitions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PullTabGameDefinitions')
BEGIN
    CREATE TABLE PullTabGameDefinitions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        GameName NVARCHAR(255) NOT NULL,
        DefaultTicketCount INT NOT NULL,
        TicketPrice DECIMAL(18,2) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME NULL,
        CreatedBy NVARCHAR(MAX) NULL,
        ModifiedBy NVARCHAR(MAX) NULL,
        SyncDate DATETIME NULL,
        RowVersion ROWVERSION NULL
    );
    PRINT 'Created PullTabGameDefinitions table.';
END

-- 2. Create PullTabPrizeOptions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PullTabPrizeOptions')
BEGIN
    CREATE TABLE PullTabPrizeOptions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        GameDefinitionId INT NOT NULL,
        OptionLabel NVARCHAR(255) NOT NULL,
        PayoutAmount DECIMAL(18,2) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME NULL,
        CreatedBy NVARCHAR(MAX) NULL,
        ModifiedBy NVARCHAR(MAX) NULL,
        SyncDate DATETIME NULL,
        RowVersion ROWVERSION NULL,
        CONSTRAINT FK_PullTabPrizeOptions_GameDefinitions FOREIGN KEY (GameDefinitionId) REFERENCES PullTabGameDefinitions(Id) ON DELETE CASCADE
    );
    PRINT 'Created PullTabPrizeOptions table.';
END

-- 3. Create PullTabGameEntries
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PullTabGameEntries')
BEGIN
    CREATE TABLE PullTabGameEntries (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        SessionId INT NOT NULL,
        GameDefinitionId INT NOT NULL,
        SelectedPrizeOptionId INT NULL,
        SellerName NVARCHAR(255) NOT NULL,
        SerialNumber NVARCHAR(100) NOT NULL,
        TicketsIssued INT NOT NULL,
        TicketsReturned INT NOT NULL,
        PrizesPaid DECIMAL(18,2) NOT NULL,
        StartingBank DECIMAL(18,2) NOT NULL,
        CashReceived DECIMAL(18,2) NOT NULL,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME NULL,
        CreatedBy NVARCHAR(MAX) NULL,
        ModifiedBy NVARCHAR(MAX) NULL,
        SyncDate DATETIME NULL,
        RowVersion ROWVERSION NULL,
        CONSTRAINT FK_PullTabGameEntries_Sessions FOREIGN KEY (SessionId) REFERENCES BingoSessions(Id) ON DELETE CASCADE,
        CONSTRAINT FK_PullTabGameEntries_GameDefinitions FOREIGN KEY (GameDefinitionId) REFERENCES PullTabGameDefinitions(Id),
        CONSTRAINT FK_PullTabGameEntries_PrizeOptions FOREIGN KEY (SelectedPrizeOptionId) REFERENCES PullTabPrizeOptions(Id)
    );
    PRINT 'Created PullTabGameEntries table.';
END

-- Seed Initial Data
DECLARE @StingerId INT;
DECLARE @SpeedballId INT;

-- Seed Stingers
IF NOT EXISTS (SELECT * FROM PullTabGameDefinitions WHERE GameName = 'Stingers')
BEGIN
    INSERT INTO PullTabGameDefinitions (GameName, DefaultTicketCount, TicketPrice, IsActive, CreatedBy)
    VALUES ('Stingers', 360, 1.00, 1, 'System');
    SET @StingerId = SCOPE_IDENTITY();

    INSERT INTO PullTabPrizeOptions (GameDefinitionId, OptionLabel, PayoutAmount, CreatedBy)
    VALUES (@StingerId, '1 Winner @ $250.00 & 10 @ $1.00', 260.00, 'System');
    
    PRINT 'Seeded Stingers definition and options.';
END

-- Seed Speedball
IF NOT EXISTS (SELECT * FROM PullTabGameDefinitions WHERE GameName = 'Speedball')
BEGIN
    INSERT INTO PullTabGameDefinitions (GameName, DefaultTicketCount, TicketPrice, IsActive, CreatedBy)
    VALUES ('Speedball', 204, 1.00, 1, 'System');
    SET @SpeedballId = SCOPE_IDENTITY();

    INSERT INTO PullTabPrizeOptions (GameDefinitionId, OptionLabel, PayoutAmount, CreatedBy)
    VALUES (@SpeedballId, '1 Winner @ $150.00', 150.00, 'System');
    
    INSERT INTO PullTabPrizeOptions (GameDefinitionId, OptionLabel, PayoutAmount, CreatedBy)
    VALUES (@SpeedballId, '3 Winners @ $50.00', 150.00, 'System');
    
    INSERT INTO PullTabPrizeOptions (GameDefinitionId, OptionLabel, PayoutAmount, CreatedBy)
    VALUES (@SpeedballId, '6 Winners @ $25.00', 150.00, 'System');
    
    PRINT 'Seeded Speedball definition and options.';
END
