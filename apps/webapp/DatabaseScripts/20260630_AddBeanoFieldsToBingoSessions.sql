-- Migration script to add Beano and Special Game mapping fields to BingoSessions
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSessions') AND name = 'WtaColor')
BEGIN
    ALTER TABLE BingoSessions ADD WtaColor NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD ProgColor NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD BeanoSuppliesSales DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoOtherReceipts DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoUnexpendedNetProfit DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoInterest DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoGameBank DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoOthers DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoCheckbookBalance DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoSavingsBalance DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoCdBalance DECIMAL(18,2) NOT NULL DEFAULT 0;
    ALTER TABLE BingoSessions ADD BeanoTaxCheckNumber NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD BeanoOccasionTime NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD FiftyFifty1Color NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD FiftyFifty2Color NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD FiftyFifty3Color NVARCHAR(255) NULL;
    ALTER TABLE BingoSessions ADD BeanoDisbursementsJson NVARCHAR(MAX) NULL;
    ALTER TABLE BingoSessions ADD BeanoOtherExpensesJson NVARCHAR(MAX) NULL;
    PRINT 'Added Beano and Special Game fields to BingoSessions table.';
END
ELSE
BEGIN
    PRINT 'Beano fields already exist in BingoSessions table.';
END
