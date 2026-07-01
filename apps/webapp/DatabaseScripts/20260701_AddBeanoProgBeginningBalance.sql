-- Migration script to add BeanoProgBeginningBalance field to BingoSessions
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSessions') AND name = 'BeanoProgBeginningBalance')
BEGIN
    ALTER TABLE BingoSessions ADD BeanoProgBeginningBalance DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT 'Added BeanoProgBeginningBalance to BingoSessions.';
END
ELSE
BEGIN
    PRINT 'BeanoProgBeginningBalance already exists in BingoSessions table.';
END
