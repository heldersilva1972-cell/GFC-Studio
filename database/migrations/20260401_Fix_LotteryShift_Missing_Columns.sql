/* 
   GFC Studio Diagnostic Fix: 
   Add missing columns to LotteryShifts table to resolve analytics crash
*/

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LotteryShifts') AND name = 'BagRefillAmount')
BEGIN
    ALTER TABLE [LotteryShifts] ADD [BagRefillAmount] decimal(18,2) NOT NULL DEFAULT 0.00;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LotteryShifts') AND name = 'TicketImageUrl')
BEGIN
    ALTER TABLE [LotteryShifts] ADD [TicketImageUrl] nvarchar(MAX) NULL;
END
GO
