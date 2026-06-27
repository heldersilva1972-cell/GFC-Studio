-- Migration: Add RecordedShiftsCount to LotteryWeeklySettlements
USE [ClubMembership];
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[LotteryWeeklySettlements]') 
    AND name = 'RecordedShiftsCount'
)
BEGIN
    ALTER TABLE [dbo].[LotteryWeeklySettlements]
    ADD [RecordedShiftsCount] INT NOT NULL DEFAULT 0;
    
    PRINT 'Added RecordedShiftsCount column to LotteryWeeklySettlements table.';
END
ELSE
BEGIN
    PRINT 'RecordedShiftsCount column already exists.';
END
GO
