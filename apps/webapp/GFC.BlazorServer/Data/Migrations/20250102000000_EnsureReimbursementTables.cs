using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

public partial class EnsureReimbursementTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementCategories', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementCategories](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT(1)
    );
    
    CREATE INDEX [IX_ReimbursementCategories_IsActive] ON [dbo].[ReimbursementCategories]([IsActive]);
    
    INSERT INTO [dbo].[ReimbursementCategories] ([Id], [Name], [IsActive]) VALUES
        (1, 'Costco', 1),
        (2, 'BJ''s', 1),
        (3, 'Supplies', 1),
        (4, 'Misc', 1);
END
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementRequests', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementRequests](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestorMemberId] INT NOT NULL,
        [RequestDate] DATE NOT NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT('Draft'),
        [TotalAmount] DECIMAL(10,2) NOT NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [CreatedUtc] DATETIME2 NOT NULL,
        [UpdatedUtc] DATETIME2 NOT NULL,
        [EditedFlag] BIT NOT NULL DEFAULT(0),
        [ApprovedByMemberId] INT NULL,
        [ApprovedDateUtc] DATETIME2 NULL,
        [RejectedByMemberId] INT NULL,
        [RejectedDateUtc] DATETIME2 NULL,
        [RejectReason] NVARCHAR(MAX) NULL,
        [PaidByMemberId] INT NULL,
        [PaidDateUtc] DATETIME2 NULL
    );
    
    CREATE INDEX [IX_ReimbursementRequests_RequestorMemberId] ON [dbo].[ReimbursementRequests]([RequestorMemberId]);
    CREATE INDEX [IX_ReimbursementRequests_RequestDate] ON [dbo].[ReimbursementRequests]([RequestDate]);
    CREATE INDEX [IX_ReimbursementRequests_Status] ON [dbo].[ReimbursementRequests]([Status]);
END
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementItems', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementItems](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestId] INT NOT NULL,
        [ExpenseDate] DATE NOT NULL,
        [Amount] DECIMAL(10,2) NOT NULL,
        [CategoryId] INT NOT NULL,
        [Vendor] NVARCHAR(200) NULL,
        [Notes] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_ReimbursementItems_ReimbursementRequests_RequestId] 
            FOREIGN KEY ([RequestId]) REFERENCES [dbo].[ReimbursementRequests]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ReimbursementItems_ReimbursementCategories_CategoryId] 
            FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ReimbursementCategories]([Id])
    );
    
    CREATE INDEX [IX_ReimbursementItems_RequestId] ON [dbo].[ReimbursementItems]([RequestId]);
    CREATE INDEX [IX_ReimbursementItems_CategoryId] ON [dbo].[ReimbursementItems]([CategoryId]);
END
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReceiptFiles', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReceiptFiles](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestItemId] INT NOT NULL,
        [FileName] NVARCHAR(200) NOT NULL,
        [RelativePath] NVARCHAR(500) NOT NULL,
        [UploadedUtc] DATETIME2 NOT NULL,
        [UploadedByMemberId] INT NOT NULL,
        CONSTRAINT [FK_ReceiptFiles_ReimbursementItems_RequestItemId] 
            FOREIGN KEY ([RequestItemId]) REFERENCES [dbo].[ReimbursementItems]([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_ReceiptFiles_RequestItemId] ON [dbo].[ReceiptFiles]([RequestItemId]);
    CREATE INDEX [IX_ReceiptFiles_UploadedByMemberId] ON [dbo].[ReceiptFiles]([UploadedByMemberId]);
END
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementChangeLogs', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementChangeLogs](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestId] INT NOT NULL,
        [ChangedByMemberId] INT NOT NULL,
        [ChangeUtc] DATETIME2 NOT NULL,
        [FieldName] NVARCHAR(100) NOT NULL,
        [OldValue] NVARCHAR(MAX) NULL,
        [NewValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_ReimbursementChangeLogs_ReimbursementRequests_RequestId] 
            FOREIGN KEY ([RequestId]) REFERENCES [dbo].[ReimbursementRequests]([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_ReimbursementChangeLogs_RequestId] ON [dbo].[ReimbursementChangeLogs]([RequestId]);
    CREATE INDEX [IX_ReimbursementChangeLogs_ChangedByMemberId] ON [dbo].[ReimbursementChangeLogs]([ChangedByMemberId]);
    CREATE INDEX [IX_ReimbursementChangeLogs_ChangeUtc] ON [dbo].[ReimbursementChangeLogs]([ChangeUtc]);
END
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementSettings', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementSettings](
        [Id] INT NOT NULL PRIMARY KEY,
        [ReceiptRequired] BIT NOT NULL DEFAULT(0),
        [NotificationRecipients] NVARCHAR(MAX) NULL
    );
    
    INSERT INTO [dbo].[ReimbursementSettings] ([Id], [ReceiptRequired], [NotificationRecipients]) 
    VALUES (1, 0, NULL);
END
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementChangeLogs', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReimbursementChangeLogs];
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementSettings', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReimbursementSettings];
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReceiptFiles', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceiptFiles];
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementItems', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReimbursementItems];
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementRequests', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReimbursementRequests];
");

        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementCategories', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReimbursementCategories];
");
    }
}
