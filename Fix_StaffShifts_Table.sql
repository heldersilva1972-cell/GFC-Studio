-- Database Fix Script for StaffShifts
-- Run this script if the Staff Shifts page fails to load data

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StaffShifts')
BEGIN
    CREATE TABLE [dbo].[StaffShifts] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [StaffMemberId] INT NOT NULL,
        [Date] DATETIME2 NOT NULL,
        [ShiftType] INT NOT NULL,
        [Status] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_StaffShifts] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO
