-- =============================================
-- Lottery Sales Management Tables
-- =============================================
-- Run this script to create the necessary tables for lottery sales tracking

-- LotteryShifts table: Stores lottery shift data
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotteryShifts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LotteryShifts] (
        [ShiftId] INT IDENTITY(1,1) PRIMARY KEY,
        [ShiftDate] DATETIME NOT NULL,
        [EmployeeName] NVARCHAR(100) NOT NULL,
        [ShiftType] NVARCHAR(50) NULL,
        [MachineId] NVARCHAR(50) NULL,
        [StartingCash] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [EndingCash] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [TotalSales] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [TotalPayouts] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [TotalCancels] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Notes] NVARCHAR(1000) NULL,
        [Status] NVARCHAR(50) NULL,
        [IsReconciled] BIT NOT NULL DEFAULT 0,
        [ReconciledBy] NVARCHAR(100) NULL,
        [ReconciledDate] DATETIME NULL,
        [CreatedBy] NVARCHAR(100) NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedBy] NVARCHAR(100) NULL,
        [ModifiedDate] DATETIME NULL
    );
    
    CREATE INDEX [IX_LotteryShifts_ShiftDate] ON [dbo].[LotteryShifts]([ShiftDate] DESC);
    CREATE INDEX [IX_LotteryShifts_EmployeeName] ON [dbo].[LotteryShifts]([EmployeeName]);
    CREATE INDEX [IX_LotteryShifts_IsReconciled] ON [dbo].[LotteryShifts]([IsReconciled]);
    CREATE INDEX [IX_LotteryShifts_Status] ON [dbo].[LotteryShifts]([Status]);
END
GO

PRINT 'Lottery sales tables created successfully!';

