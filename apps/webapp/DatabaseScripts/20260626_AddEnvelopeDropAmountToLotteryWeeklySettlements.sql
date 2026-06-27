-- Migration: Add EnvelopeDropAmount to LotteryWeeklySettlements
USE [ClubMembership];
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[LotteryWeeklySettlements]') 
    AND name = 'EnvelopeDropAmount'
)
BEGIN
    ALTER TABLE [dbo].[LotteryWeeklySettlements]
    ADD [EnvelopeDropAmount] DECIMAL(18,2) NOT NULL DEFAULT 0;
    
    PRINT 'Added EnvelopeDropAmount column to LotteryWeeklySettlements table.';
END
ELSE
BEGIN
    PRINT 'EnvelopeDropAmount column already exists.';
END
GO
