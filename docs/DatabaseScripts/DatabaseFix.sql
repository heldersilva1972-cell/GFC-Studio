-- Database Fix Script for HallRentalRequests
-- Run this script if you encounter database errors related to 'HallRentalRequests' table or 'HallRentalRequest' type

-- Ensure you are using the correct database
-- USE [YourDatabaseName];
-- GO

-- Create HallRentalRequests table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HallRentalRequests')
BEGIN
    CREATE TABLE [dbo].[HallRentalRequests] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [RequesterName] NVARCHAR(MAX) NOT NULL DEFAULT N'',
        [RequesterEmail] NVARCHAR(MAX) NOT NULL DEFAULT N'',
        [RequesterPhone] NVARCHAR(MAX) NOT NULL DEFAULT N'',
        [MemberStatus] BIT NOT NULL DEFAULT 0,
        [GuestCount] INT NOT NULL DEFAULT 0,
        [RulesAgreed] BIT NOT NULL DEFAULT 0,
        [KitchenUsage] BIT NOT NULL DEFAULT 0,
        [RequestedDate] DATETIME2(7) NOT NULL,
        [Status] NVARCHAR(MAX) NULL DEFAULT N'Pending',
        [ApprovedBy] NVARCHAR(MAX) NULL,
        [ApprovalDate] DATETIME2(7) NULL,
        CONSTRAINT [PK_HallRentalRequests] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO
