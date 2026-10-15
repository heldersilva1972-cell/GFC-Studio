-- =========================================================================
-- Migration: Add AcknowledgedBy and AcknowledgedAt to LotteryShifts
-- Description: Supports the shift variance acknowledgment feature in 
--              GFC-Connect and WebApp, distinguishing Active Alerts from Shift History.
-- =========================================================================

IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[LotteryShifts]') 
      AND name = 'AcknowledgedBy'
)
BEGIN
    ALTER TABLE [dbo].[LotteryShifts] 
    ADD [AcknowledgedBy] NVARCHAR(100) NULL;
    
    PRINT 'Added AcknowledgedBy column to LotteryShifts table.';
END
ELSE
BEGIN
    PRINT 'AcknowledgedBy column already exists in LotteryShifts table.';
END
GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[LotteryShifts]') 
      AND name = 'AcknowledgedAt'
)
BEGIN
    ALTER TABLE [dbo].[LotteryShifts] 
    ADD [AcknowledgedAt] DATETIME2 NULL;
    
    PRINT 'Added AcknowledgedAt column to LotteryShifts table.';
END
ELSE
BEGIN
    PRINT 'AcknowledgedAt column already exists in LotteryShifts table.';
END
GO
