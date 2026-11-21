-- Migration script to add BeanoEndingUnexpendedNetProfit field to BingoSessions
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BingoSessions') AND name = 'BeanoEndingUnexpendedNetProfit')
BEGIN
    ALTER TABLE BingoSessions ADD BeanoEndingUnexpendedNetProfit DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT 'Added BeanoEndingUnexpendedNetProfit to BingoSessions.';
END
ELSE
BEGIN
    PRINT 'BeanoEndingUnexpendedNetProfit already exists in BingoSessions table.';
END
