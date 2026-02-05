-- Add TicketImageUrl
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'TicketImageUrl')
BEGIN
    ALTER TABLE LotteryShifts ADD TicketImageUrl NVARCHAR(500) NULL;
    PRINT 'Added TicketImageUrl';
END

-- Add Commission if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'Commission')
BEGIN
    ALTER TABLE LotteryShifts ADD Commission DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT 'Added Commission';
END

-- Add CashBonus if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'CashBonus')
BEGIN
    ALTER TABLE LotteryShifts ADD CashBonus DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT 'Added CashBonus';
END

-- Add ClaimsBonus if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'ClaimsBonus')
BEGIN
    ALTER TABLE LotteryShifts ADD ClaimsBonus DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT 'Added ClaimsBonus';
END

-- Add NetDue if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'NetDue')
BEGIN
    ALTER TABLE LotteryShifts ADD NetDue DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT 'Added NetDue';
END

-- Add OriginalTotalSales if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'OriginalTotalSales')
BEGIN
    ALTER TABLE LotteryShifts ADD OriginalTotalSales DECIMAL(18,2) NULL;
    PRINT 'Added OriginalTotalSales';
END
GO
