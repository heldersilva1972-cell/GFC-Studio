IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LotteryShifts' AND COLUMN_NAME = 'TicketImageUrl')
BEGIN
    ALTER TABLE LotteryShifts ADD TicketImageUrl NVARCHAR(500) NULL;
    PRINT 'Added TicketImageUrl column successfully.';
END
ELSE
BEGIN
    PRINT 'TicketImageUrl column already exists.';
END
GO
